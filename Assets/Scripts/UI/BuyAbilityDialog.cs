using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;

namespace UI
{
    [System.Serializable]
    public class BuyAbilityDialog
    {
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI requiredCoinText;
        public Image abilityImage;
        public GameObject panel;
        public Button buyButton;
        public Button watchAdvertiseButton;
        public Button cancelButton;


        public void Show(string title, string description, string requiredCoin, Sprite abilitySprite)
        {
            panel.SetActive(true);
            panel.transform.ShowPopUp();
            titleText.text = title;
            descriptionText.text = description;
            requiredCoinText.text = requiredCoin;
            abilityImage.sprite = abilitySprite;
        }


        public void Hide()
        {
            panel.transform.localScale = Vector3.zero;
            panel.SetActive(false);
        }


        public void AddBuyClickListener(UnityAction action) => buyButton.onClick.AddListener(action);

        public void RemoveBuyClickListener(UnityAction action) => buyButton.onClick.RemoveListener(action);

        public void AddWatchAddClickListener(UnityAction action) =>
            watchAdvertiseButton.onClick.AddListener(action);

        public void RemoveWatchAddClickListener(UnityAction action) =>
            watchAdvertiseButton.onClick.RemoveListener(action);

        public void AddCancelClickListener(UnityAction action) => cancelButton.onClick.AddListener(action);

        public void RemoveCancelClickListener(UnityAction action) => cancelButton.onClick.RemoveListener(action);
    }
}