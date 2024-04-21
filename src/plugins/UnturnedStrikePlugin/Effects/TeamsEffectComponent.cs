using SDG.Unturned;
using Steamworks;
using UnityEngine;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Extensions;
using UnturnedStrike.Plugin.Models;
using UnturnedStrike.Plugin.Services;

namespace UnturnedStrike.Plugin.Effects
{
    public class TeamsEffectComponent : MonoBehaviour
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public LobbyPlayer Player { get; private set; }        

        public const int Key = 2580;

        public bool IsOpened { get; private set; }

        void Awake()
        {
            IsOpened = false;
            Player = GetComponent<LobbyPlayer>();
        }

        void Start()
        {
            Player.OnButtonClicked += OnButtonClicked;
            TeamsService.OnTeamPlayersUpdated += UpdateCount;
        }

        public void OnButtonClicked(string buttonName)
        {
            switch (buttonName)
            {
                case "TeamA_Button":
                    if (pluginInstance.TeamsService.TryJoinTeam(Player, ETeamType.Terrorist))
                        Close();
                    break;
                case "TeamB_Button":
                    if (pluginInstance.TeamsService.TryJoinTeam(Player, ETeamType.CounterTerrorist))
                        Close();
                    break;
                case "Connect_Lobby_Button":
                    Close();
                    break;
            }
        }

        public void Open()
        {
            if (!IsOpened)
            {
                Player.NativePlayer.enablePluginWidgetFlag(EPluginWidgetFlags.Modal);
                Player.NativePlayer.enablePluginWidgetFlag(EPluginWidgetFlags.ForceBlur);
                EffectManager.sendUIEffect(pluginInstance.Configuration.Instance.TeamsEffectId, Key, Player.TransportConnection, false);
                EffectManager.sendUIEffectText(Key, Player.TransportConnection, false, "Connect_TeamA_Name", ETeamType.Terrorist.GetTranslation());
                EffectManager.sendUIEffectText(Key, Player.TransportConnection, false, "Connect_TeamB_Name", ETeamType.CounterTerrorist.GetTranslation());
                IsOpened = true;
                UpdateCount(ETeamType.Terrorist);
                UpdateCount(ETeamType.CounterTerrorist);
            }
        }

        private void UpdateCount(ETeamType team)
        {
            if (IsOpened)
            {
                var childName = team == ETeamType.Terrorist ? "Connect_TeamA_Count" : "Connect_TeamB_Count";
                EffectManager.sendUIEffectText(Key, Player.TransportConnection, false, childName,
                    $"{pluginInstance.TeamsService.TeamPlayers[team].Count}/{pluginInstance.Configuration.Instance.MaxTeamMembers}");
            }
        }

        public void Close()
        {
            if (IsOpened)
            {
                Player.NativePlayer.disablePluginWidgetFlag(EPluginWidgetFlags.Modal);
                Player.NativePlayer.disablePluginWidgetFlag(EPluginWidgetFlags.ForceBlur);
                EffectManager.askEffectClearByID(pluginInstance.Configuration.Instance.TeamsEffectId, Player.TransportConnection);
                IsOpened = false;
            }
        }

        void OnDestroy()
        {
            Player.OnButtonClicked -= OnButtonClicked;
            TeamsService.OnTeamPlayersUpdated -= UpdateCount;
            Close();
        }
    }
}
