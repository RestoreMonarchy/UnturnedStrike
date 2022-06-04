using SDG.Framework.Utilities;
using SDG.Unturned;
using System.Reflection;
using UnityEngine;

namespace UnturnedStrike.Plugin.Utilities
{
    public class BarricadeUtility
    {
        public static void DestroyBarricade(Transform transform)
        {
            BarricadeDrop barricadeDrop = BarricadeManager.FindBarricadeByRootTransform(transform);
            if (barricadeDrop != null)
                DestroyBarricade(barricadeDrop);
        }

        public static void DestroyBarricade(BarricadeDrop barricadeDrop)
        {
            if (BarricadeManager.tryGetRegion(barricadeDrop.model, out byte x, out byte y, out ushort plant, out BarricadeRegion _))
            {
                BarricadeManager.destroyBarricade(barricadeDrop, x, y, plant);
            }
        }

        public static bool RaycastBarricade(Player player, out Transform transform)
        {
            transform = null;
            RaycastHit hit;
            if (PhysicsUtility.raycast(new Ray(player.look.aim.position, player.look.aim.forward), out hit, 3, RayMasks.BARRICADE_INTERACT))
            {
                transform = hit.transform;
                return true;
            }
            return false;
        }

        public static Transform DropBarricade(ushort barricadeId, Vector3 position, Vector3 angles, ulong ownerGroup)
        {
            ItemBarricadeAsset asset = Assets.find(EAssetType.ITEM, barricadeId) as ItemBarricadeAsset; 
            Barricade barricade = new Barricade(asset);
            Quaternion rotation = Quaternion.Euler(angles);

            Transform transform = BarricadeManager.dropNonPlantedBarricade(
                barricade, 
                position,
                rotation,
                0, 
                ownerGroup);

            return transform;
        }
    }
}
