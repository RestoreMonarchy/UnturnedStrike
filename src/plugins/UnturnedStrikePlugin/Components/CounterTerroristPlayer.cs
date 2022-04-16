using Rocket.Core.Logging;
using System;
using UnturnedStrike.Plugin.Models;

namespace UnturnedStrike.Plugin.Components
{
    public class CounterTerroristPlayer : GamePlayer
    {
        protected override void Awake()
        {
            TeamType = ETeamType.CounterTerrorist;
            base.Awake();
        }

        protected override void Start()
        {            
            base.Start();
            if (pluginInstance.GameType == EGameType.Defuse)
                NativePlayer.interact.sendSalvageTimeOverride(pluginInstance.Configuration.Instance.BombSalvageTime);
            else
                NativePlayer.interact.sendSalvageTimeOverride(pluginInstance.Configuration.Instance.HostageSalvageTime);
        }
    }
}
