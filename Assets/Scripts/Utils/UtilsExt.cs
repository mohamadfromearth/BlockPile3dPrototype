﻿using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Utils
{
    public static class UtilsExt
    {
        public static void SetParent(this GameObject obj, Transform parent)
        {
            obj.SetParent(parent);
        }


        public static IEnumerator DeActiveObjectWithDelay(this GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            obj.SetActive(false);
        }

        public static Transform Parent(this GameObject obj) => obj.transform.parent;


        public static void ShowPopUp(this Transform transform)
        {
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }

        public static void HidePopUp(this Transform transform)
        {
            transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
        }

        public static void HidePopUp(this GameObject obj)
        {
            obj.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).onComplete = () => { obj.SetActive(false); };
        }


        public static IEnumerator AnimateTextCounter(this TextMeshProUGUI text, int startValue, int endValue,
            float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float currentValue = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
                text.text = Mathf.FloorToInt(currentValue).ToString(); // Update the TextMeshPro text
                yield return null; // Wait until the next frame
            }

            // Ensure the final value is set correctly
            text.text = endValue.ToString();
        }

        public static IEnumerator AnimateTextCounter(this TextMeshProUGUI text, int startValue, int endValue,
            string extraText, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float currentValue = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
                text.text = Mathf.FloorToInt(currentValue) + extraText; // Update the TextMeshPro text
                yield return null; // Wait until the next frame
            }

            // Ensure the final value is set correctly
            text.text = endValue + extraText;
        }
    }
}