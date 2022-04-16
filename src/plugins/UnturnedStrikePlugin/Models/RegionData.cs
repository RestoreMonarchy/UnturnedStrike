using System;
using UnityEngine;

namespace UnturnedStrike.Plugin.Models
{
    public class RegionData
    {
        public char Character { get; set; }
        public ConvertableVector3[] Nodes { get; set; }
        public float Height { get; set; }

        public bool IsInRegion(Vector3 vector)
        {
            if (Nodes.Length < 2)
                return false;

            var xMin = Math.Min(Nodes[0].X, Nodes[1].X);
            var xMax = Math.Max(Nodes[0].X, Nodes[1].X);
            var zMin = Math.Min(Nodes[0].Z, Nodes[1].Z);
            var zMax = Math.Max(Nodes[0].Z, Nodes[1].Z);
            var yMin = Math.Min(Nodes[0].Y - 0.5f, Nodes[1].Y - 0.5f);
            var yMax = yMin + Height;

            if (vector.x > xMin && vector.x < xMax && 
                vector.z > zMin && vector.z < zMax && 
                vector.y > yMin && vector.y < yMax)
                return true;
            else
                return false;
        }
    }
}
