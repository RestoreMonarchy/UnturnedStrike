using UnityEngine;

namespace UnturnedStrike.Plugin.Models
{
    public class ConvertableVector3
    {
        public ConvertableVector3() { }
        public ConvertableVector3(Vector3 vector)
        {
            X = vector.x;
            Y = vector.y;
            Z = vector.z;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }

        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }
    }
}
