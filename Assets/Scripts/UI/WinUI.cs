using System;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class WinUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Button claimButton;
        [SerializeField] private GameObject panel;
        [SerializeField] private Transform background;
        [SerializeField] private TextMeshProUGUI collectedBlocksText;
        [SerializeField] private TextMeshProUGUI starRewardText;
        [SerializeField] private TextMeshProUGUI buildingItemRewardText;
        [SerializeField] private TextMeshProUGUI fortuneWheelProgressHintText;
        [SerializeField] private Image fortuneWheelProgressImage;
        [SerializeField] private Button advertiseRewardButton;
        [SerializeField] private ParticleSystem confettiParticleSystem;


        private TweenCallback _hidingComplete;


        private void OnEnable()
        {
            _hidingComplete += DeActiveBackground;
        }

        private void OnDisable()
        {
            _hidingComplete -= DeActiveBackground;
        }


        public void AddClaimClickListener(UnityAction action) => claimButton.onClick.AddListener(action);

        public void RemoveClaimClickListener(UnityAction action) => claimButton.onClick.RemoveListener(action);

        public void AddAdvertiseRewardClickListener(UnityAction action) =>
            advertiseRewardButton.onClick.AddListener(action);

        public void RemoveAdvertiseRewardClickListener(UnityAction action) =>
            advertiseRewardButton.onClick.RemoveListener(action);


        public void AddHidingCompleteListener(TweenCallback callback)
        {
            _hidingComplete += callback;
        }

        public void RemoveHidingCompleteListener(TweenCallback callback)
        {
            _hidingComplete -= callback;
        }


        public void Show(
            string level,
            string collectedText,
            string starReward,
            string buildingItemReward,
            float fortuneWheelProgress,
            string fortuneWheelProgressHint
        )
        {
            confettiParticleSystem.GameObject().SetActive(true);
            confettiParticleSystem.Play();
            panel.SetActive(true);
            background.ShowPopUp();
            levelText.text = level;
            collectedBlocksText.text = collectedText;
            starRewardText.text = starReward;
            buildingItemRewardText.text = buildingItemReward;
            fortuneWheelProgressImage.fillAmount = fortuneWheelProgress;
            fortuneWheelProgressHintText.text = fortuneWheelProgressHint;
            confettiParticleSystem.Play();
        }

        public void Hide()
        {
            background.transform.HidePopUp(_hidingComplete);
        }




        private void DeActiveBackground() => panel.SetActive(false);
    }
}