using Rocket.Unturned.Chat;
using SDG.Unturned;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnturnedStrike.Plugin.Extensions;
using UnturnedStrike.Plugin.Helpers;
using UnturnedStrike.Plugin.Models;
using Math = System.Math;

namespace UnturnedStrike.Plugin.Services
{
    public class GameService : MonoBehaviour
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public int GameID { get; set; }
        public EGameType GameType { get; set; }
        public Dictionary<ETeamType, byte> TeamScores { get; set; }

        public TeamsService TeamsService { get; private set; }
        public GameStatisticsService StatisticsService { get; private set; }
        public WarmupService WarmupService { get; private set; }

        public int CurrentRoundID { get; private set; }
        public GlobalRoundService CurrentRound { get; private set; }

        public bool IsGameStarted { get; private set; }

        public ETeamType LastRoundWinner { get; set; }

        #region Events

        public delegate void GameStarted(GameService game);
        public delegate void GameFinished(GameResults results);
        public delegate void NewRoundCreated(int roundId);
        public delegate void RoundStarted(int roundId);
        public delegate void RoundFinished(int roundId);
        public delegate void RoundWon(ETeamType winner, ERounWinType winType, MVP mvp);
        public delegate void TimeUpdated(string newTime);
        public delegate void ScoreUpdated(ETeamType team, int newScore);
        public delegate void AliveUpdated(ETeamType team, int newAlive);
        public delegate void FreezeStarted();
        public delegate void FreezeFinished();

        public static event GameStarted OnGameStarted;
        public static event GameFinished OnGameFinished;
        public event NewRoundCreated OnNewRoundCreated;
        public event RoundStarted OnRoundStarted;
        public event RoundFinished OnRoundFinished;
        public event RoundWon OnRoundWon;
        public event TimeUpdated OnTimeUpdated;
        public event ScoreUpdated OnScoreUpdated;
        public event AliveUpdated OnAliveUpdated;
        public event FreezeStarted OnFreezeStarted;
        public event FreezeFinished OnFreezeFinished;

        internal void TriggerOnGameFinished(GameResults results)
        {
            OnGameFinished?.Invoke(results);
        }
        internal void TriggerOnNewRoundStarted(int roundId)
        {
            OnNewRoundCreated?.Invoke(roundId);
        }
        internal void TriggerOnRoundStarted(int roundId)
        {
            OnRoundStarted?.Invoke(roundId);
        }
        internal void TriggerOnRoundFinished(int roundId)
        {
            OnRoundFinished?.Invoke(roundId);
        }
        internal void TriggerOnRoundWon(ETeamType winner, ERounWinType winType, MVP mvp)
        {
            OnRoundWon?.Invoke(winner, winType, mvp);
        }
        internal void TriggerOnTimeUpdated(string newTime)
        {
            OnTimeUpdated?.Invoke(newTime);
        }
        internal void TriggerOnScoreUpdated(ETeamType team, int newScore)
        {
            OnScoreUpdated?.Invoke(team, newScore);
        }
        internal void TriggerOnAliveUpdated(ETeamType team, int newAlive)
        {
            OnAliveUpdated?.Invoke(team, newAlive);
        }
        internal void TriggerOnFreezeStarted()
        {
            OnFreezeStarted?.Invoke();
        }
        internal void TriggerOnFreezeFinished()
        {
            OnFreezeFinished?.Invoke(); 
        }

        #endregion

        void Awake()
        {
            GameType = pluginInstance.GameType;
            TeamScores = new Dictionary<ETeamType, byte>() 
            {
                { ETeamType.Terrorist, 0 },
                { ETeamType.CounterTerrorist, 0 }
            };
        }

        void Start()
        {
            TeamsService = gameObject.AddComponent<TeamsService>();
            StatisticsService = gameObject.AddComponent<GameStatisticsService>();
            WarmupService = gameObject.AddComponent<WarmupService>();
        }

        public void StartGame()
        {
            IsGameStarted = true;

            Destroy(WarmupService);

            foreach (var player in TeamsService.Players)
            {
                player.ClearInventory();
            }

            UnturnedChat.Say(pluginInstance.Translate("GameStarting"));
            if (TeamsService.IsAnyEmpty() || pluginInstance.Configuration.Instance.IsSetUpRun)
            {
                CancelGame();
            } else
            {
                OnGameStarted?.Invoke(this);
                StartNewRound();
                UnturnedChat.Say(pluginInstance.Translate("GameBegin"));
            }
        }

        public void CancelGame()
        {            
            UnturnedChat.Say(pluginInstance.Translate("GameBeginCanceled"));
            IsGameStarted = false;
            Start();
        }

        public void FinishRound(ETeamType winner, ERounWinType winType)
        {
            LastRoundWinner = winner;
            var loser = winner.GetOppositeTeam();
            TeamsService.PayEachTeamPlayer(winner, pluginInstance.Configuration.Instance.MoneyRewardWin);
            TeamsService.PayEachTeamPlayer(loser, pluginInstance.Configuration.Instance.MoneyRewardLose);

            var mvp = StatisticsService.RoundStatistics.GetMVP(winner, winType);
            TriggerOnRoundWon(winner, winType, mvp);

            if (winner == ETeamType.Terrorist)
                TeamsService.PlayEffectToEachPlayer(pluginInstance.Configuration.Instance.TerroristsWinEffectId);
            else
                TeamsService.PlayEffectToEachPlayer(pluginInstance.Configuration.Instance.CounterTerroristsWinEffectId);

            TriggerOnScoreUpdated(winner, ++TeamScores[winner]);            
            Invoke("StartNewRound", pluginInstance.Configuration.Instance.NewRoundStartDelay);
        }

        private void StartNewRound()
        {
            if (CurrentRound != null)
                Destroy(CurrentRound);

            if (TeamScores.Any(x => x.Value == pluginInstance.Configuration.Instance.RoundsToWin))
            {
                StartFinishGame(EGameWinType.Team, TeamScores.First(x => x.Value == pluginInstance.Configuration.Instance.RoundsToWin).Key);
                return;
            }
            if (TeamScores.Sum(x => x.Value) == pluginInstance.Configuration.Instance.MaxRounds)
            {
                StartFinishGame(EGameWinType.Draw, ETeamType.Terrorist);
                return;
            }
            if (TeamsService.TryGetEmptyTeam(out ETeamType loser))
            {
                StartFinishGame(EGameWinType.Empty, loser.GetOppositeTeam());
                return;
            }
            
            TriggerOnNewRoundStarted(++CurrentRoundID);
            if (GameType == EGameType.Defuse)
                CurrentRound = gameObject.AddComponent<DefuseRoundService>();
            else
                CurrentRound = gameObject.AddComponent<HostageRoundService>();
        }

        public void StartFinishGame(EGameWinType winType, ETeamType winner)
        {
            if (CurrentRound != null)
                Destroy(CurrentRound);

            TriggerOnGameFinished(new GameResults() 
            { 
                GameId = GameID,
                GameType = GameType,
                Winner = winner,
                GameWinType = winType,
                TerroristScore = TeamScores[ETeamType.Terrorist],
                CounterTerroristScore = TeamScores[ETeamType.CounterTerrorist],
                Stats = StatisticsService.GetPlayerStats()
            });

            string msg = string.Empty;
            switch (winType)
            {
                case EGameWinType.Draw:
                    msg = pluginInstance.Translate("GameDraw");
                    break;
                case EGameWinType.Empty:
                    msg = pluginInstance.Translate("GameWinEmpty", pluginInstance.Translate(winner.ToString()),
                        pluginInstance.Translate(winner.GetOppositeTeam().ToString()));
                    break;
                case EGameWinType.Team:
                    msg = pluginInstance.Translate("GameWin", pluginInstance.Translate(winner.ToString()));
                    break;
                case EGameWinType.Restart:
                    msg = pluginInstance.Translate("GameRestart");
                    break;
            }

            foreach (var player in TeamsService.Players)
            {
                if (player.IsAlive)
                {
                    player.ClearInventory();
                    SpawnsHelper.TeleportPlayer(player.NativePlayer, EPlayerSpawnType.Lobby);
                } else
                {
                    player.ForceRespawnPlayer();
                }
                player.LeaderboardComponent.Open(true);
                player.HideWaitingUI();
                player.GameWinComponent.Show(msg);
            }

            Invoke("FinishGame", pluginInstance.Configuration.Instance.NewGameStartDelay);
        }

        private void FinishGame()
        {            
            pluginInstance.RestartGame();
        }
    }
}
