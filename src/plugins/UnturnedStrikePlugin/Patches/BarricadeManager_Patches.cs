using HarmonyLib;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using UnturnedStrike.Plugin.Bombsite;
using UnturnedStrike.Plugin.Components;
using UnturnedStrike.Plugin.Helpers;
using UnturnedStrike.Plugin.Models;
using UnturnedStrike.Plugin.Services;

namespace UnturnedStrike.Plugin.Patches
{
    [HarmonyPatch(typeof(BarricadeManager))]
    class BarricadeManager_Patches
    {
        [HarmonyPatch("dropBarricade")]
        [HarmonyPrefix]
        static bool dropBarricadePrefix(out TerroristPlayer __state, Vector3 point, ref ulong owner, ref ulong group)
        {
            __state = null;

            if (UnturnedStrikePlugin.Instance.GameType == EGameType.Hostage)
            {
                return false;
            }

            var comp = PlayerTool.getPlayer(new CSteamID(owner))?.GetComponent<TerroristPlayer>() ?? null;
            if (comp != null && comp.IsAlive)
            {
                if (RegionsHelper.IsPointOnRegion(point))
                {
                    owner = 0;
                    group = UnturnedStrikePlugin.Instance.Configuration.Instance.CounterTerroristGroupId;
                    __state = comp;
                    return true;
                }
                else
                    comp.Message("NotBombsite");
            }

            return false;
        }

        [HarmonyPatch("dropBarricade")]
        [HarmonyPostfix]
        static void dropBarricadePostfix(Transform __result, TerroristPlayer __state)
        {
            if (UnturnedStrikePlugin.Instance.GameType == EGameType.Defuse && __state != null)
            {                
                (UnturnedStrikePlugin.Instance.GameService.CurrentRound as DefuseRoundService)
                    .StartBombPlanted(__state, __result.gameObject.AddComponent<BombComponent>());
            }
        }
    }
}
