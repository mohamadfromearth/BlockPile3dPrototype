using UnityEngine;

namespace Utils
{
    public class VectorUtils
    {
        public static Vector3 RotateVector90DegreesAroundZ(Vector3 originalVector)
        {
            return new Vector3(-originalVector.y, originalVector.x, originalVector.z);
        }

        public static Vector3 RotateVector90DegreesAroundX(Vector3 originalVector)
        {
            return new Vector3(originalVector.x, -originalVector.z, originalVector.y);
        }

        public static Vector3 RotateVector90DegreesAroundY(Vector3 originalVector)
        {
            return new Vector3(originalVector.z, originalVector.y, -originalVector.x);
        }
    }
}