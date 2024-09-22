using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public static class UtilsExt
    {
        public static void SetParent(this GameObject obj, Transform parent)
        {
            obj.SetParent(parent);
        }

        public static Transform Parent(this GameObject obj) => obj.transform.parent;


        public static void ShowPopUp(this Transform transform)
        {
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
    }
}