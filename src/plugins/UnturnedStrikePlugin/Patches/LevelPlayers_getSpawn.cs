using HarmonyLib;
using SDG.Unturned;
using UnturnedStrike.Plugin.Helpers;

namespace UnturnedStrike.Plugin.Patches
{
    [HarmonyPatch(typeof(LevelPlayers), "getSpawn")]
    class LevelPlayers_getSpawn
    {
        [HarmonyPrefix]
        static bool Prefix(LevelPlayers __instance, ref PlayerSpawnpoint __result, bool isAlt)
        {
            if (UnturnedStrikePlugin.Instance.GameService?.IsGameStarted ?? true)
            {
                __result = SpawnsHelper.GetLobbySpawnPoint(isAlt);
            } else
            {
                __result = SpawnsHelper.GetWarmupSpawnPoint(isAlt);
            }
            
            return false;
        } 
    }
}
