using HarmonyLib;
using SDG.Unturned;

namespace UnturnedStrike.Plugin.Patches
{
    [HarmonyPatch(typeof(UseableGun))]
    class UseableGun_Patches
    {
        [HarmonyPatch("ReceiveAttachSight")]
        [HarmonyPrefix]
        static bool ReceiveAttachSightPrefix()
        {
            return false;
        }

        [HarmonyPatch("ReceiveAttachBarrel")]
        [HarmonyPrefix]
        static bool ReceiveAttachBarrelPrefix()
        {
            return false;
        }

        [HarmonyPatch("ReceiveAttachGrip")]
        [HarmonyPrefix]
        static bool ReceiveAttachGripPrefix()
        {
            return false;
        }

        [HarmonyPatch("ReceiveAttachTactical")]
        [HarmonyPrefix]
        static bool ReceiveAttachTacticalPrefix()
        {
            return false;
        }        
    }
}
