using HarmonyLib;
using SDG.Unturned;

namespace UnturnedStrike.Plugin.Patches
{
    [HarmonyPatch(typeof(PlayerCrafting))]
    class PlayerCrafting_Patches
    {
        [HarmonyPatch("ReceiveStripAttachments")]
        [HarmonyPrefix]
        static bool ReceiveStripAttachmentsPrefix()
        {
            return false;
        }
    }
}
