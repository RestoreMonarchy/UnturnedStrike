using Rocket.Unturned.Chat;
using SDG.Unturned;
using System.Linq;
using UnityEngine;
using UnturnedStrike.Plugin.Components;

namespace UnturnedStrike.Plugin.Effects
{
    public class LeaderboardEffectComponent : MonoBehaviour
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public UnturnedStrikePlayer Player { get; set; }

        public const int Key = 2581;

        public bool IsOpened { get; private set; }

        void Awake()
        {
            IsOpened = false;
            Player = GetComponent<UnturnedStrikePlayer>();            
        }

        void Start()
        {
            Player.OnPluginKeyTicked += OnPluginKeyTicked;
            Rocket.Core.Logging.Logger.Log($"LeaderboardEffectComponent.Start {Player.DisplayName}");
        }

        private void OnPluginKeyTicked(uint simulation, byte key, bool state)
        {
            if (state)
            {
                Rocket.Core.Logging.Logger.Log($"LeaderboardEffectComponent.OnPluginKeyTicked {key} {state}");
            }
            
            if (key == 1)
            {
                if (state)
                    Open();
                else
                    Close();
            }
        }

        public void Open(bool isFinalOpen = false)
        {
            if (isFinalOpen)
            {
                Player.OnPluginKeyTicked -= OnPluginKeyTicked;
                Player.NativePlayer.enablePluginWidgetFlag(EPluginWidgetFlags.ForceBlur);
            }

            if (!IsOpened)
            {
                EffectManager.sendUIEffect(pluginInstance.Configuration.Instance.LeaderboardEffectId, Key, Player.TransportConnection, true);
                
                string itemString;
                foreach (var group in pluginInstance.GameService.StatisticsService.GetPlayingPlayersStats().GroupBy(x => x.Team))
                {
                    int item = 1;
                    if (group.Key == Models.ETeamType.CounterTerrorist)
                        item = 11;

                    foreach (var stat in group.OrderByDescending(x => x.Score))
                    {
                        itemString = $"leaderborad_item{item}_";
                        EffectManager.sendUIEffectImageURL(Key, Player.TransportConnection, true, itemString + "avatar", stat.IconUrl);
                        EffectManager.sendUIEffectText(Key, Player.TransportConnection, true, itemString + "name", stat.DisplayName);
                        EffectManager.sendUIEffectText(Key, Player.TransportConnection, true, itemString + "kills", stat.Kills.ToString());
                        EffectManager.sendUIEffectText(Key, Player.TransportConnection, true, itemString + "deaths", stat.Deaths.ToString());
                        EffectManager.sendUIEffectText(Key, Player.TransportConnection, true, itemString + "kd", stat.KD.ToString("0.00"));
                        EffectManager.sendUIEffectText(Key, Player.TransportConnection, true, itemString + "mvps", stat.MVPs.ToString());
                        EffectManager.sendUIEffectText(Key, Player.TransportConnection, true, itemString + "score", stat.Score.ToString());
                        item++;
                    }
                }
                IsOpened = true;
            }
        }

        public void Close()
        {           
            if (IsOpened)
            {
                EffectManager.askEffectClearByID(pluginInstance.Configuration.Instance.LeaderboardEffectId, Player.TransportConnection);
                IsOpened = false;
            }
        }

        void OnDestroy()
        {
            Player.OnPluginKeyTicked -= OnPluginKeyTicked;
            Player.NativePlayer.disablePluginWidgetFlag(EPluginWidgetFlags.ForceBlur);
            Close();
        }
    }
}