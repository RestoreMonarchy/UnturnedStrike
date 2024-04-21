using SDG.Unturned;
using UnturnedStrike.Plugin.Effects;

namespace UnturnedStrike.Plugin.Components
{
    public class SpectatorPlayer : UnturnedStrikePlayer
    {
        private EPluginWidgetFlags pluginWidgetFlags;

        public WarmupEffectComponent WarmupComponent { get; private set; }
        public GameWinEffectComponent GameWinComponent { get; private set; }
        public LeaderboardEffectComponent LeaderboardComponent { get; private set; }

        protected override void Start()
        {
            base.Start();

            WarmupComponent = gameObject.AddComponent<WarmupEffectComponent>();
            GameWinComponent = gameObject.AddComponent<GameWinEffectComponent>();
            LeaderboardComponent = gameObject.AddComponent<LeaderboardEffectComponent>();

            gameObject.AddComponent<RoundsEffectComponent>();
            gameObject.AddComponent<RoundWinEffectComponent>();

            pluginInstance.SpectatorService.AddSpectator(this);
            HideWaitingUI();

            pluginWidgetFlags = NativePlayer.pluginWidgetFlags;
            NativePlayer.setPluginWidgetFlag(EPluginWidgetFlags.Default, false);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            pluginInstance.SpectatorService.RemoveSpectator(this);

            Destroy(WarmupComponent);
            Destroy(GameWinComponent);
            Destroy(LeaderboardComponent);

            Destroy(gameObject.GetComponent<RoundsEffectComponent>());
            Destroy(gameObject.GetComponent<RoundWinEffectComponent>());

            NativePlayer.setPluginWidgetFlag(pluginWidgetFlags, true);
        }
    }
}
