using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnturnedStrike.Plugin.Models;
using UnturnedStrike.Plugin.Storage;

namespace UnturnedStrike.Plugin.Helpers
{
    public class RegionsHelper : MonoBehaviour
    {
        private static UnturnedStrikePlugin pluginInstance => UnturnedStrikePlugin.Instance;

        private static DataStorage<List<RegionData>> RegionsDataStorage { get; set; }
        private static List<RegionData> RegionsData { get; set; }

        public static List<ConvertableVector3> Nodes { get; private set; }

        void Awake()
        {
            RegionsDataStorage = new DataStorage<List<RegionData>>(pluginInstance.Directory, 
                $"RegionsData.{Provider.map.Replace(' ', '_')}.json");
            Nodes = new List<ConvertableVector3>();
        }

        void Start()
        {
            RegionsData = RegionsDataStorage.Read();
            if (RegionsData == null)
                RegionsData = new List<RegionData>();
        }

        public static bool MakeRegion(char character, float height, out RegionData region)
        {
            region = null;
            if (Nodes.Count < 2)
                return false;

            region = new RegionData()
            {
                Character = character,
                Height = height,
                Nodes = Nodes.Reverse<ConvertableVector3>().Take(2).ToArray()
            };
            
            RegionsData.Add(region);
            RegionsDataStorage.Save(RegionsData);
            Nodes.Clear();
            return true;
        }

        public static bool IsPointOnRegion(Vector3 pos)
        {
            return RegionsData.Any(x => x.IsInRegion(pos));
        }

        public static float GetRegionHeight(Vector3 pos)
        {
            var region = RegionsData.FirstOrDefault(x => x.IsInRegion(pos));
            return region != null ? region.Nodes[0].Y : pos.y;
        }
    }
}