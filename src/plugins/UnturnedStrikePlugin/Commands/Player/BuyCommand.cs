using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using UnturnedStrike.Plugin.Components;

namespace UnturnedStrike.Plugin.Commands.Player
{
    public class BuyCommand : IRocketCommand
    {
        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = (UnturnedPlayer)caller;
            var comp = player.GetComponent<GamePlayer>();
            if (comp != null)
            {
                if (!comp.TryOpenBuyMenu())
                {
                    comp.Message("BuyOnlyFreeze");
                }
            }
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "buy";

        public string Help => "Opens buy menu";

        public string Syntax => string.Empty;

        public List<string> Aliases => new List<string>() { "b" };

        public List<string> Permissions => new List<string>();
    }
}
