using HarmonyLib;
using SDG.Unturned;

namespace UnturnedStrike.Plugin.Patches
{
    [HarmonyPatch(typeof(LightingManager))]
    class LightingManager_Patches
    {
        [HarmonyPrefix]
        [HarmonyPatch("updateLighting")]
        static bool updateLightingPrefix()
        {
            return false;
        }
    }
}
