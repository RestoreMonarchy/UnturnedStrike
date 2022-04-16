using Rocket.Core.Utils;
using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Extensions;
using UnturnedStrike.Plugin.Helpers;
using UnturnedStrike.Plugin.Models;

namespace UnturnedStrike.Plugin.Services
{
    public class TeamsService : MonoBehaviour
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;
        public Dictionary<ETeamType, List<GamePlayer>> TeamPlayers { get; private set; }
        private System.Random Random { get; set; }

        public delegate void TeamPlayersUpdated(ETeamType team);
        public static event TeamPlayersUpdated OnTeamPlayersUpdated;

        public delegate void PlayerTeamJoined(GamePlayer player, ETeamType team);
        public event PlayerTeamJoined OnPlayerTeamJoined;

        public delegate void PlayerTeamLeft(GamePlayer player, ETeamType team);
        public event PlayerTeamLeft OnPlayerTeamLeft;

        public IEnumerable<GamePlayer> Players => TeamPlayers[ETeamType.Terrorist].Concat(TeamPlayers[ETeamType.CounterTerrorist]);

        public bool TryJoinTeam(UnturnedStrikePlayer player, ETeamType team)
        {
            if (IsTeamFull(team))
            {
                player.Message("TeamFull", team.GetTranslation());
                return false;
            }

            if (player.GetComponent<GamePlayer>() != null)
            {
                player.Message("TeamAlready");
                return false;
            }

            Destroy(player);
            JoinTeam(player.NativePlayer, team);
            return true;
        }

        public GamePlayer JoinTeam(Player player, ETeamType team)
        {
            GamePlayer comp;
            if (team == ETeamType.Terrorist)
                comp = player.gameObject.AddComponent<TerroristPlayer>();
            else
                comp = player.gameObject.AddComponent<CounterTerroristPlayer>();

            TeamPlayers[team].Add(comp);
            OnTeamPlayersUpdated?.Invoke(team);
            OnPlayerTeamJoined?.Invoke(comp, team);
            return comp;
        }

        public bool IsTeamFull(ETeamType team)
        {
            if (TeamPlayers[team].Count >= pluginInstance.Configuration.Instance.MaxTeamMembers)
                return true;

            if (TeamPlayers[team].Count + 1 - TeamPlayers.First(x => x.Key != team).Value.Count
                > pluginInstance.Configuration.Instance.MaxTeamMembersDifference)
            {
                return true;
            }

            return false;
        }

        public bool IsAnyEmpty()
        {
            if (TeamPlayers.Any(x => x.Value.Count == 0))
                return true;
            return false;
        }

        public bool IsEmpty(ETeamType team)
        {
            if (TeamPlayers[team].Count == 0)
                return true;
            return false;
        }

        public bool TryGetEmptyTeam(out ETeamType team)
        {
            team = default;
            if (IsEmpty(ETeamType.Terrorist))
            {
                team = ETeamType.Terrorist;
                return true;
            }
            if (IsEmpty(ETeamType.CounterTerrorist))
            {
                team = ETeamType.CounterTerrorist;
                return true;
            }
            return false;
        }

        public GamePlayer GetRandomPlayer(ETeamType team)
        {
            return TeamPlayers[team].ElementAtOrDefault(Random.Next(0, TeamPlayers[team].Count));
        }

        public void PayEachTeamPlayer(ETeamType team, int amount)
        {
            foreach (var player in TeamPlayers[team])
            {
                player.GiveMoney(amount);
            }
        }

        public void PlayEffectToEachPlayer(ushort effectId)
        {
            foreach (var player in TeamPlayers[ETeamType.Terrorist].Concat(TeamPlayers[ETeamType.CounterTerrorist]))
            {
                player.SendSoundEffect(effectId);
            }
        }

        public void ChangePlayerTeam(GamePlayer gamePlayer)
        {
            var bonusBalance = 0;
            if (gamePlayer.IsVIP)
                bonusBalance = gamePlayer.Balance;

            var newTeam = gamePlayer.TeamType.GetOppositeTeam();
            if (gamePlayer.IsAlive)
                gamePlayer.SelfKill();

            Destroy(gamePlayer);
            TaskDispatcher.QueueOnMainThread(() =>
            {
                gamePlayer = JoinTeam(gamePlayer.NativePlayer, newTeam);
                if (gamePlayer.Balance == 0)
                    gamePlayer.GiveMoney(bonusBalance);
                gamePlayer.Message("ChangeTeamSuccess", newTeam.GetTranslation());
            }, 2);
        }

        public void RemovePlayer(GamePlayer gamePlayer)
        {
            if (TeamPlayers.ContainsKey(gamePlayer.TeamType))
            {
                TeamPlayers[gamePlayer.TeamType].Remove(gamePlayer);
                OnPlayerTeamLeft?.Invoke(gamePlayer, gamePlayer.TeamType);
            }
        }

        void Awake()
        {
            TeamPlayers = new Dictionary<ETeamType, List<GamePlayer>>() 
            {
                { ETeamType.Terrorist, new List<GamePlayer>() },
                { ETeamType.CounterTerrorist, new List<GamePlayer>() }
            };
            Random = new System.Random();
        }

        void OnDestroy()
        {
            foreach (var player in TeamPlayers[ETeamType.Terrorist].Concat(TeamPlayers[ETeamType.CounterTerrorist]))
            {
                var nativePlayer = player.NativePlayer;
                if (player.IsAlive)
                    SpawnsHelper.TeleportPlayer(player.NativePlayer, EPlayerSpawnType.Lobby);
                Destroy(player);
                nativePlayer.gameObject.AddComponent<LobbyPlayer>();
            }
        }
    }
}
