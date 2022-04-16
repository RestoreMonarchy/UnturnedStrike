using HarmonyLib;
using SDG.Unturned;

namespace UnturnedStrike.Plugin.Patches
{
    [HarmonyPatch(typeof(PlayerLife))]
    class PlayerLife_Patches
    {
        [HarmonyPatch("askStarve")]
        [HarmonyPrefix]
        static bool askStarvePrefix()
        {
            return false;
        }

        [HarmonyPatch("askDehydrate")]
        [HarmonyPrefix]
        static bool askDehydratePrefix()
        {
            return false;
        }

        [HarmonyPatch("askInfect")]
        [HarmonyPrefix]
        static bool askInfectPrefix()
        {
            return false;
        }

        [HarmonyPatch("askTire")]
        [HarmonyPostfix]
        private static void askTirePostfix(PlayerLife __instance, byte amount)
        {
            __instance.serverModifyStamina(amount);
        }

        [HarmonyPatch("ReceiveSuicideRequest")]
        [HarmonyPrefix]
        static bool ReceiveSuicideRequestPrefix()
        {
            return false;
        }
    }
}
