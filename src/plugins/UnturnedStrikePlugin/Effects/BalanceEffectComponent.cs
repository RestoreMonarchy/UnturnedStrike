using SDG.NetTransport;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using UnturnedStrike.Plugin.Components;

namespace UnturnedStrike.Plugin.Effects
{
    public class BalanceEffectComponent : MonoBehaviour
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public GamePlayer Player { get; set; }
        private ITransportConnection TransportConnection => Player.TransportConnection;

        public const int Key = 2577;

        void Awake()
        {
            Player = GetComponent<GamePlayer>();
        }

        void Start()
        {
            EffectManager.sendUIEffect(pluginInstance.Configuration.Instance.BalanceEffectId, Key, TransportConnection, true);
            UpdateBalance();
            Player.OnBalanceUpdated += UpdateBalance;
        }

        public void UpdateBalance()
        {
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "csgo_text_balance", pluginInstance.Translate("Balance", Player.Balance));
        }

        void OnDestroy()
        {
            Player.OnBalanceUpdated -= UpdateBalance;
            EffectManager.askEffectClearByID(pluginInstance.Configuration.Instance.BalanceEffectId, TransportConnection);
        }
    }
}
