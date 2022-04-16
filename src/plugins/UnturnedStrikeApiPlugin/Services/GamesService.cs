using Rocket.Unturned;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnturnedStrike.Plugin.Models;
using UnturnedStrike.Plugin.Services;
using UnturnedStrikeAPI;
using UnturnedStrikeApiPlugin.Utilities;
using Logger = Rocket.Core.Logging.Logger;

namespace UnturnedStrikeApiPlugin.Services
{
    public class GamesService : MonoBehaviour
    {
        private UnturnedStrikeApiPlugin pluginInstance => UnturnedStrikeApiPlugin.Instance;
        public GameSummary GameSummary { get; private set; }
        private ApiHttpClient HttpClient { get; set; }

        void Awake()
        {
            HttpClient = new ApiHttpClient();
        }

        void Start()
        {            
            GameService.OnGameStarted += OnGameStarted;
            GameService.OnGameFinished += OnGameFinished;
        }

        void OnDestroy()
        {
            GameService.OnGameStarted -= OnGameStarted;
            GameService.OnGameFinished -= OnGameFinished;
        }

        private void OnGameStarted(GameService gameService)
        {
            ThreadPool.QueueUserWorkItem((a) =>
            {
                try
                {
                    CreateGameSummary(gameService);
                }
                catch (Exception e)
                {
                    Logger.LogException(e, "An exception occurated while processing game started!");
                }
            });
        }

        internal void CreateGameSummary(GameService gameService)
        {
            GameSummary = new GameSummary()
            {
                ServerGroup = pluginInstance.Configuration.Instance.ServerGroup,
                GameType = gameService.GameType.ToString(),
                Map = SDG.Unturned.Provider.map
            };
            GameSummary = HttpClient.SendAsJson<GameSummary>("gamesummaries", GameSummary);
            gameService.GameID = GameSummary.Id;
        }

        private void OnGameFinished(GameResults results)
        {
            var gameSummary = new GameSummary()
            {
                Id = results.GameId,
                TerroristScore = results.TerroristScore,
                CounterTerroristScore = results.CounterTerroristScore,
                WinType = results.GameWinType.ToString(),
                IsWinnerTerrorist = results.Winner == ETeamType.Terrorist,
                PlayerGameSummaries = new List<PlayerGameSummary>()
            };

            foreach (var playerStat in results.Stats)
            {
                gameSummary.PlayerGameSummaries.Add(new PlayerGameSummary()
                {
                    PlayerId = playerStat.SteamId.m_SteamID.ToString(),
                    GameId = results.GameId,
                    IsTerrorist = playerStat.Team == ETeamType.Terrorist,
                    Kills = playerStat.Kills,
                    Deaths = playerStat.Deaths,
                    MVPs = playerStat.MVPs,
                    BombsDefused = playerStat.BombDefuses,
                    BombsPlanted = playerStat.BombPlants,
                    HostagesRescued = playerStat.HostageRescues,
                    Score = playerStat.Score
                });
            }

            ThreadPool.QueueUserWorkItem((a) =>
            {
                try
                {
                    HttpClient.SendAsJson("gamesummaries", gameSummary, "PUT");
                }
                catch (Exception e)
                {
                    Logger.LogException(e, "An exception occurated while processing game finished!");
                }                
            });          
        }
    }
}
