using SDG.Unturned;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Helpers;

namespace UnturnedStrike.Plugin.Extensions
{
    public static class PlayerClothingExtensions
    {
        private static UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        public static bool ValidateSwapClohting(this PlayerClothing clothing, byte page, byte x, byte y)
        {
            GamePlayer player;
            if ((player = clothing.player.GetComponent<GamePlayer>()) != null)
            {
                if (page == 255)
                    return false;
                
                var item = player.NativePlayer.inventory.getItem(page, player.NativePlayer.inventory.getIndex(page, x, y));
                if (!LoadoutsHelper.IsTeamClothing(item.item.id, player.TeamType))
                {
                    return false;
                }
            }
            return true;
        }

        public static void ForceRemoveBackpack(this PlayerClothing clothing)
        {
            clothing.askWearBackpack(0, 0, new byte[0], false);
        }
    }
}
