using UnturnedStrike.Plugin.Effects;
using UnturnedStrike.Plugin.Helpers;

namespace UnturnedStrike.Plugin.Components
{
    public class LobbyPlayer : UnturnedStrikePlayer
    {
        public TeamsEffectComponent TeamsEffectComponent { get; private set; }

        protected override void Start()
        {
            base.Start();
            TeamsEffectComponent = gameObject.AddComponent<TeamsEffectComponent>();
            ClearInventory();
            if (NativePlayer.quests.isMemberOfAGroup)
                TeamsHelper.LeaveTeam(NativePlayer);
            if (NativePlayer.life.isDead)
                ForceRespawnPlayer();
            TeamsEffectComponent.Open();
        }

        protected override void OnDestroy()
        {
            Destroy(TeamsEffectComponent);
            base.OnDestroy();
        }
    }
}