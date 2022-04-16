using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Models;

namespace UnturnedStrike.Plugin.Helpers
{
    public class VIPHelper : MonoBehaviour
    {
        private static UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public static Dictionary<CSteamID, DateTime> VIPPlayers { get; set; }

        void Awake()
        {
            VIPPlayers = new Dictionary<CSteamID, DateTime>();
        }

        public static bool IsVIP(UnturnedStrikePlayer player)
        {
            return IsVIP(player.CSteamID);            
        }

        public static bool IsVIP(CSteamID steamID)
        {
            if (VIPPlayers.TryGetValue(steamID, out var expireDate))
            {
                return expireDate > DateTime.UtcNow;
            }
            return false;
        }

        public static void GiveVIPBonusMoney(ETeamType team)
        {
            var vipPlayers = pluginInstance.TeamsService.TeamPlayers[team].Where(x => x.IsVIP);
            if (vipPlayers.Count() > 0)
            {
                foreach (var player in pluginInstance.TeamsService.TeamPlayers[team])
                {
                    player.GiveMoney(pluginInstance.Configuration.Instance.BonusVIPMoney);
                    player.Message("VIPBonus", pluginInstance.Configuration.Instance.BonusVIPMoney, string.Join(", ", vipPlayers.Select(x => x.DisplayName)));
                }
            }
        }
    }
}
