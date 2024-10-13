using Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;

namespace UI
{
    [System.Serializable]
    public class BoosterInfoUI
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Button claimButton;
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject background;

        public void Show(AbilityData abilityData)
        {
            title.text = abilityData.name;
            image.sprite = abilityData.image;
            description.text = abilityData.description;

            panel.SetActive(true);
            background.transform.ShowPopUp();
        }


        public void Hide()
        {
            background.transform.localScale = Vector3.zero;
            panel.SetActive(false);
        }


        public void AddClaimClickListener(UnityAction action) => claimButton.onClick.AddListener(action);
        public void RemoveClaimClickListener(UnityAction action) => claimButton.onClick.RemoveListener(action);
    }
}