using SDG.NetTransport;
using SDG.Unturned;
using UnityEngine;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Services;

namespace UnturnedStrike.Plugin.Effects
{
    public class WarmupEffectComponent : MonoBehaviour
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;
        public UnturnedStrikePlayer Player { get; private set; }
        private ITransportConnection TransportConnection => Player.TransportConnection;

        public const int Key = 4692;

        private bool isOpen;

        void Awake()
        {
            isOpen = false;
            Player = GetComponent<UnturnedStrikePlayer>();
        }

        void Start()
        {
            WarmupService.OnWarmupTitleUpdated += UpdateMessage;
        }

        void OnDestroy()
        {
            WarmupService.OnWarmupTitleUpdated -= UpdateMessage;
            Hide();
        }

        public void Show()
        {
            if (!isOpen)
                EffectManager.sendUIEffect(pluginInstance.Configuration.Instance.WarmupEffectId, Key, TransportConnection, true);
        }

        public void UpdateMessage(string msg)
        {
            if (!isOpen)
                EffectManager.sendUIEffect(pluginInstance.Configuration.Instance.WarmupEffectId, Key, TransportConnection, true);
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "Title", msg);
        }

        public void Hide()
        {
            EffectManager.askEffectClearByID(pluginInstance.Configuration.Instance.WarmupEffectId, TransportConnection);
        }        
    }
}
