using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnturnedStrike.Plugin.Models;

namespace UnturnedStrike.Plugin.Helpers
{
    public class TeamsHelper : MonoBehaviour
    {
        private static UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public static Dictionary<ETeamType, CSteamID> Teams { get; set; }

        void Awake()
        {
            Teams = new Dictionary<ETeamType, CSteamID>()
            {
                { ETeamType.Terrorist, new CSteamID(pluginInstance.Configuration.Instance.TerroristGroupId) },
                { ETeamType.CounterTerrorist, new CSteamID(pluginInstance.Configuration.Instance.CounterTerroristGroupId) }
            };
        }

        void Start()
        {
            if (!Level.isLoaded)
                Level.onLevelLoaded += OnLevelLoaded;
            else
                OnLevelLoaded(0);
        }

        private void OnLevelLoaded(int level)
        {
            GroupManager.addGroup(Teams[ETeamType.Terrorist], pluginInstance.Translate(ETeamType.Terrorist.ToString()));
            GroupManager.addGroup(Teams[ETeamType.CounterTerrorist], pluginInstance.Translate(ETeamType.CounterTerrorist.ToString()));
        }

        void OnDestroy()
        {
            Level.onLevelLoaded -= OnLevelLoaded;
        }

        [Obsolete]
        public static void JoinTeam(Player player, ETeamType team)
        {
            // join unturned group
            player.channel.send("tellSetGroup", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
            {
                Teams[team],
                0
            });
        }

        [Obsolete]
        public static void LeaveTeam(Player player)
        {
            player.channel.send("tellSetGroup", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
            {
                CSteamID.Nil,
                0
            });
        }

        public static bool IsTeamGroup(CSteamID groupID)
        {
            if (Teams.ContainsValue(groupID) || groupID == CSteamID.Nil)
                return true;
            else
                return false;
        }
    }
}
