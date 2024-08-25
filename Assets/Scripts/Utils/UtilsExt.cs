using UnityEngine;

namespace Utils
{
    public static class UtilsExt
    {

        public static void  SetParent(this GameObject obj, Transform parent)
        {
            obj.SetParent(parent);
        }

        public static Transform Parent(this GameObject obj) => obj.transform.parent;

    }
}