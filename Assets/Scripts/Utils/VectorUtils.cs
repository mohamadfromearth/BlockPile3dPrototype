using UnityEngine;

namespace Utils
{
    public static class VectorUtils
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

        public static Vector3 WorldToLocal(this Vector3 worldPos, Transform localOrigin)
        {
            Vector3 rel = worldPos - localOrigin.position;
            float x = Vector3.Dot(rel, localOrigin.right);
            float y = Vector3.Dot(rel, localOrigin.up);
            float z = Vector3.Dot(rel, localOrigin.forward);

            return new Vector3(x, y, z);
        }


        public static Vector3 LocalToWorld(this Vector3 local, Transform t)
        {
            Vector3 position = t.position;
            position += local.x * t.right;
            position += local.y * t.up;
            position += local.z * t.forward;

            return position;
        }
    }
}