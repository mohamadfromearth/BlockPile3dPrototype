using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [Serializable]
    public class TargetGoalUI
    {
        public Image panel;
        public Image background;
        public TextMeshProUGUI levelText;
        public TextMeshProUGUI titleText;
        public Image blocksImage;
        public TextMeshProUGUI goalText;
        public Transform hidingTarget;


        public float scaleUpDuration = 0.8f;
        public Ease scaleUpEase = Ease.Linear;
        public Ease hidingTranslationEase = Ease.Linear;
        public float hidingTranslationDuration = 0.8f;
        public float hidingFadeDuration = 0.8f;
        public float hideDelay = 0.3f;
        public Color panelColor;


        private Vector3 _initPos;


        public IEnumerator Show(string level, string goal)
        {
            panel.gameObject.SetActive(true);
            panel.color = panelColor;

            levelText.text = level;
            goalText.text = goal;
            _initPos = background.transform.position;

            background.transform.DOScale(Vector3.one, scaleUpDuration).SetEase(scaleUpEase);

            yield return new WaitForSeconds(scaleUpDuration);

            levelText.transform.DOScale(Vector3.one, scaleUpDuration).SetEase(scaleUpEase);
            titleText.transform.DOScale(Vector3.one, scaleUpDuration).SetEase(scaleUpEase);
            blocksImage.transform.DOScale(Vector3.one, scaleUpDuration).SetEase(scaleUpEase);
            goalText.transform.DOScale(Vector3.one, scaleUpDuration).SetEase(scaleUpEase);

            yield return new WaitForSeconds(scaleUpDuration + hideDelay);


            yield return Hide();
        }


        public IEnumerator Hide()
        {
            panel.DOFade(0, hidingFadeDuration);
            background.transform.DOMoveY(hidingTarget.position.y, hidingTranslationDuration)
                .SetEase(hidingTranslationEase);

            yield return new WaitForSeconds(hidingTranslationDuration);


            background.transform.localScale = Vector3.zero;
            background.transform.position = _initPos;
            levelText.transform.localScale = Vector3.zero;
            titleText.transform.localScale = Vector3.zero;
            blocksImage.transform.localScale = Vector3.zero;
            goalText.transform.localScale = Vector3.zero;


            panel.gameObject.SetActive(false);
        }
    }
}