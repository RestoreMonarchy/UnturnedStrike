using HarmonyLib;
using SDG.Unturned;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Extensions;
using UnturnedStrike.Plugin.Services;

namespace UnturnedStrike.Plugin.Patches
{
    [HarmonyPatch(typeof(PlayerClothing))]
    class PlayerClothing_Patches
    {
        [HarmonyPatch("ReceiveSwapBackpackRequest")]
        [HarmonyPrefix]
        static bool ReceiveSwapBackpackRequestPrefix(PlayerClothing __instance, byte page, byte x, byte y)
        {
            return __instance.ValidateSwapClohting(page, x, y);
        }

        [HarmonyPatch("ReceiveSwapGlassesRequest")]
        [HarmonyPrefix]
        static bool ReceiveSwapGlassesRequestPrefix(PlayerClothing __instance, byte page, byte x, byte y)
        {
            return __instance.ValidateSwapClohting(page, x, y);
        }

        [HarmonyPatch("ReceiveSwapHatRequest")]
        [HarmonyPrefix]
        static bool ReceiveSwapHatRequestPrefix(PlayerClothing __instance, byte page, byte x, byte y)
        {
            return __instance.ValidateSwapClohting(page, x, y);
        }

        [HarmonyPatch("ReceiveSwapMaskRequest")]
        [HarmonyPrefix]
        static bool ReceiveSwapMaskRequestPrefix(PlayerClothing __instance, byte page, byte x, byte y)
        {
            return __instance.ValidateSwapClohting(page, x, y);
        }

        [HarmonyPatch("ReceiveSwapPantsRequest")]
        [HarmonyPrefix]
        static bool ReceiveSwapPantsRequestPrefix(PlayerClothing __instance, byte page, byte x, byte y)
        {
            return __instance.ValidateSwapClohting(page, x, y);
        }

        [HarmonyPatch("ReceiveSwapShirtRequest")]
        [HarmonyPrefix]
        static bool ReceiveSwapShirtRequestPrefix(PlayerClothing __instance, byte page, byte x, byte y)
        {
            return __instance.ValidateSwapClohting(page, x, y);
        }

        [HarmonyPatch("ReceiveSwapVestRequest")]
        [HarmonyPrefix]
        static bool ReceiveSwapVestRequestPrefix(PlayerClothing __instance, byte page, byte x, byte y)
        {
            return __instance.ValidateSwapClohting(page, x, y);
        }

        [HarmonyPatch("ReceiveVisualToggleRequest")]
        [HarmonyPrefix]
        static bool ReceiveVisualToggleRequestPrefix(PlayerClothing __instance, EVisualToggleType type)
        {
            if (type == EVisualToggleType.COSMETIC)
            {
                GamePlayer comp = __instance.player.GetComponent<GamePlayer>();
                if (comp == null)
                    return true;

                if (!__instance.isVisual && !comp.CanToggleCosmetics)
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}
