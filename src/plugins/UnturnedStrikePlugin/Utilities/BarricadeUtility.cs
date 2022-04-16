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

        [System.Obsolete]
        public static Transform DropBarricade(ushort barricadeId, Vector3 position, Vector3 angles, ulong ownerGroup)
        {
            var instanceCountField = typeof(BarricadeManager).GetField("instanceCount", BindingFlags.NonPublic | BindingFlags.Static);
            var spawnBarricadeMethod = typeof(BarricadeManager).GetMethod("spawnBarricade", BindingFlags.NonPublic | BindingFlags.Instance);

            if (!Regions.tryGetCoordinate(position, out byte x, out byte y))
            {
                return null;
            }

            if (!BarricadeManager.tryGetRegion(x, y, 65535, out var region))
            {
                return null;
            }

            var instanceId = (uint)instanceCountField.GetValue(null) + 1;

            var data = new BarricadeData(new Barricade(barricadeId), position, (byte)angles.x, (byte)angles.y, (byte)angles.z, 
                0, ownerGroup, uint.MaxValue, instanceId);

            instanceCountField.SetValue(null, instanceId);

            // do it on server
            var result = (Transform)spawnBarricadeMethod.Invoke(BarricadeManager.instance, new object[]
            {
                region,
                data.barricade.id,
                data.barricade.state,
                data.point,
                data.angle_x,
                data.angle_y,
                data.angle_z,
                (byte)100,
                data.owner,
                data.group,
                data.instanceID
            });


            if (result != null)
            {
                region.barricades.Add(data);
                // send to players
                BarricadeManager.instance.channel.send("tellBarricade", ESteamCall.OTHERS, x, y, BarricadeManager.BARRICADE_REGIONS,
                    ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    x,
                    y,
                    ushort.MaxValue,
                    data.barricade.id,
                    data.barricade.state,
                    data.point,
                    data.angle_x,
                    data.angle_y,
                    data.angle_z,
                    data.owner,
                    data.group,
                    data.instanceID
                });
            }

            return result;
        }
    }
}
