using SDG.NetTransport;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Models;

namespace UnturnedStrike.Plugin.Effects
{
    public class RoundWinEffectComponent : MonoBehaviour
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;
        public UnturnedStrikePlayer Player { get; private set; }
        private ITransportConnection TransportConnection => Player.TransportConnection;

        public const int Key = 2576;

        void Start()
        {
            Player = GetComponent<UnturnedStrikePlayer>();
            pluginInstance.GameService.OnRoundWon += StartWinUI;
            pluginInstance.GameService.OnRoundFinished += StopWinUI;
        }

        private void StartWinUI(ETeamType winner, ERounWinType winType, MVP mvp)
        {
            EffectManager.sendUIEffect(pluginInstance.Configuration.Instance.WinEffectId, Key, TransportConnection, true);

            EffectManager.sendUIEffectText(Key, TransportConnection, true, "csgo_text_windesc", pluginInstance.Translate("RoundWinTitle", pluginInstance.Translate(winner.ToString())));
            if (mvp.Player != null)
            {
                string msg = string.Empty;
                switch (mvp.Type)
                {
                    case EMVPType.Eliminations:
                        msg = pluginInstance.Translate("MVPEliminations", mvp.Player.DisplayName);
                        break;
                    case EMVPType.BombPlant:
                        msg = pluginInstance.Translate("MVPBombPlant", mvp.Player.DisplayName);
                        break;
                    case EMVPType.BombDefuse:
                        msg = pluginInstance.Translate("MVPBombDefuse", mvp.Player.DisplayName);
                        break;
                    case EMVPType.HostageRescue:
                        msg = pluginInstance.Translate("MVPHostageRescued", mvp.Player.DisplayName);
                        break;
                }
                EffectManager.sendUIEffectText(Key, TransportConnection, true, "csgo_text_wintext", msg);
            }
        }

        public void StopWinUI(int roundId)
        {
            EffectManager.askEffectClearByID(pluginInstance.Configuration.Instance.WinEffectId, TransportConnection);
        }

        void OnDestroy()
        {
            pluginInstance.GameService.OnRoundWon -= StartWinUI;
            pluginInstance.GameService.OnRoundFinished -= StopWinUI;
            EffectManager.askEffectClearByID(pluginInstance.Configuration.Instance.WinEffectId, TransportConnection);
        }
    }
}
