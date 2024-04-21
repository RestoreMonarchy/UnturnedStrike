using SDG.Unturned;
using UnityEngine;
using UnturnedStrike.Plugin.Components;

namespace UnturnedStrike.Plugin.Effects
{
    public class GameWinEffectComponent : MonoBehaviour
    {
        private UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;
        public UnturnedStrikePlayer Player { get; private set; }

        public const int Key = 2582;

        void Start()
        {
            Player = GetComponent<UnturnedStrikePlayer>();
        }

        public void Show(string msg)
        {
            EffectManager.sendUIEffect(pluginInstance.Configuration.Instance.GameWinEffectId, Key, Player.TransportConnection, true);
            EffectManager.sendUIEffectText(Key, Player.TransportConnection, true, "Title", msg);
        }

        public void Hide()
        {
            EffectManager.askEffectClearByID(pluginInstance.Configuration.Instance.GameWinEffectId, Player.TransportConnection);
        }

        void OnDestroy()
        {
            Hide();
        }
    }
}
