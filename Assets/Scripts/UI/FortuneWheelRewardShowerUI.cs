using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    [System.Serializable]
    public struct FortuneWheelRewardShowerUI
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Image rewardImage;
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private Button claimButton;
        [SerializeField] private ParticleSystem confettiParticle;


        public void Show(Sprite rewardSprite, string count)
        {
            panel.SetActive(true);
            rewardImage.sprite = rewardSprite;
            countText.text = count;
            confettiParticle.Play();
        }


        public void Hide() => panel.SetActive(false);


        public void AddClaimButtonClickListener(UnityAction action) => claimButton.onClick.AddListener(action);

        public void RemoveClaimButtonClickListener(UnityAction action) => claimButton.onClick.RemoveListener(action);
    }
}