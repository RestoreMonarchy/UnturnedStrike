using HarmonyLib;
using SDG.Unturned;
using UnityEngine;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Helpers;
using Logger = Rocket.Core.Logging.Logger;

namespace UnturnedStrike.Plugin.Patches
{
    [HarmonyPatch(typeof(UseableBarricade))]
    class UseableBarricade_Patches
    {
        [HarmonyPatch("ReceiveBarricadeNone")]
        [HarmonyPrefix]
        static bool ReceiveBarricadeNone_Prefix(UseableBarricade __instance, Vector3 newPoint)
        {
            TerroristPlayer terrorist;
            if (__instance.equippedBarricadeAsset.id == UnturnedStrikePlugin.Instance.Configuration.Instance.BombItemId
                && (terrorist = __instance.player.GetComponent<TerroristPlayer>()) != null)
            {
                if (!RegionsHelper.IsPointOnRegion(newPoint))
                {
                    terrorist.Message("NotBombsite");
                    __instance.player.equipment.dequip();
                    return false;
                }
            }
            return true;
        }
    }
}
