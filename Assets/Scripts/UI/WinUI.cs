using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class WinUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private GameObject panel;
        [SerializeField] private Transform background;
        [SerializeField] private TextMeshProUGUI collectedBlocksText;
        [SerializeField] private TextMeshProUGUI starRewardText;
        [SerializeField] private TextMeshProUGUI buildingItemRewardText;
        [SerializeField] private Image fortuneWheelProgressImage;
        [SerializeField] private Button advertiseRewardButton;


        public void AddNextLevelClickListener(UnityAction action) => nextLevelButton.onClick.AddListener(action);

        public void RemoveNextLevelClickListener(UnityAction action) => nextLevelButton.onClick.RemoveListener(action);

        public void AddAdvertiseRewardClickListener(UnityAction action) =>
            advertiseRewardButton.onClick.AddListener(action);

        public void RemoveAdvertiseRewardClickListener(UnityAction action) =>
            advertiseRewardButton.onClick.RemoveListener(action);

        public void Show(
            string level,
            string collectedText,
            string starReward,
            string buildingItemReward,
            float fortuneWheelProgress
        )
        {
            panel.SetActive(true);
            background.ShowPopUp();
            levelText.text = level;
            collectedBlocksText.text = collectedText;
            starRewardText.text = starReward;
            buildingItemRewardText.text = buildingItemReward;
            fortuneWheelProgressImage.fillAmount = fortuneWheelProgress;
        }

        public void Hide()
        {
            background.localScale = Vector3.zero;
            panel.SetActive(false);
        }
    }
}