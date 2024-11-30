using System;
using Data;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

namespace UI
{
    [Serializable]
    public class FortuneWheelUI
    {
        [SerializeField] private GameObject parent;
        [SerializeField] private GameObject panel;
        [SerializeField] private Button spinButton;
        [SerializeField] private GameObject spinText;
        [SerializeField] private GameObject advertiseSpinText;
        [SerializeField] private Button closeButton;
        [SerializeField] private Image advertiseImage;
        [SerializeField] private Image[] itemsImages;
        [SerializeField] private TextMeshProUGUI[] itemsTexts;

        [SerializeField] private AnimationCurve rotationAnimationCurve;

        [SerializeField] private float rotationDuration = 2f;


        private float degree;

        public float Degree => degree;

        private Action _rotationCompleted;


        public void SetData(FortuneWheelItemData[] fortuneWheelItemDataList, bool isAdvertise)
        {
            advertiseImage.gameObject.SetActive(isAdvertise);
            advertiseSpinText.SetActive(isAdvertise);
            spinText.SetActive(!isAdvertise);


            for (int i = 0; i < fortuneWheelItemDataList.Length; i++)
            {
                var sprite = fortuneWheelItemDataList[i].sprite;
                var countText = fortuneWheelItemDataList[i].count.ToString();

                itemsImages[i].sprite = sprite;
                itemsTexts[i].text = countText;
            }
        }


        public void Show()
        {
            parent.SetActive(true);
            panel.transform.ShowPopUp();
            spinButton.transform.ShowPopUp();
        }

        public void Hide()
        {
            parent.SetActive(false);
            panel.transform.localScale = Vector3.zero;
            spinButton.transform.localScale = Vector3.zero;
            panel.transform.rotation = Quaternion.identity;
        }


        public void SetItemsSprites(Sprite[] sprites)
        {
            for (int i = 0; i < itemsImages.Length; i++)
            {
                itemsImages[i].sprite = sprites[i];
            }
        }

        public void Rotate()
        {
            var rotationCount = Random.Range(12, 24);
            var rotationValue = rotationCount * 45;

            panel.transform.DORotate(new Vector3(0, 0, rotationValue), rotationDuration,
                    RotateMode.FastBeyond360)
                .SetEase(rotationAnimationCurve)
                .onComplete = () => { _rotationCompleted?.Invoke(); };
        }

        public Quaternion GetRotation() => panel.transform.rotation;


        public void AddSpinClickListener(UnityAction action) => spinButton.onClick.AddListener(action);
        public void RemoveSpinClickListener(UnityAction action) => spinButton.onClick.RemoveListener(action);


        public void AddCloseClickListener(UnityAction action) => closeButton.onClick.AddListener(action);

        public void RemoveCloseClickListener(UnityAction action) => closeButton.onClick.RemoveListener(action);

        public void AddRotationCompletionListener(Action action) => _rotationCompleted += action;

        public void RemoveRotationCompletionListener(Action action) => _rotationCompleted -= action;
    }
}