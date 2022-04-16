using HarmonyLib;
using SDG.Unturned;
using Steamworks;
using UnturnedStrike.Plugin.Helpers;

namespace UnturnedStrike.Plugin.Patches
{
    [HarmonyPatch(typeof(PlayerQuests))]
    class PlayerQuests_Patches
    {
        [HarmonyPatch("ReceiveLeaveGroupRequest")]
        [HarmonyPrefix]
        static bool ReceiveLeaveGroupRequestPrefix()
        {
            return false;
        }

        [HarmonyPatch("ReceiveGroupState")]
        [HarmonyPrefix]
        static bool ReceiveGroupStatePrefix(CSteamID newGroupID, byte newGroupRank)
        {
            if (!TeamsHelper.IsTeamGroup(newGroupID))
                return false;
            else
                return true;
        }
    }
}
