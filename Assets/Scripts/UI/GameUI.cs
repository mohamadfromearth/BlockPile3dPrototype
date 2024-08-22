using CoreUI;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private Image progressImage;
        [SerializeField] private Image progressBackgroundImage;
        [SerializeField] private Button punchButton;
        [SerializeField] private Button swapButton;
        [SerializeField] private Button refreshButton;
        [SerializeField] private TextMeshProUGUI punchButtonText;
        [SerializeField] private TextMeshProUGUI swapButtonText;
        [SerializeField] private TextMeshProUGUI refreshButtonText;
        [SerializeField] private HorizontalLayoutGroup abilityBarLg;
        [SerializeField] private DialogueTypeA buyingAbilityDialog;
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private Button abilityCancelButton;
        private AbilityData _abilityData;

        public AbilityData AbilityData => _abilityData;


        public void SetProgress(float value) => progressImage.fillAmount = value;

        public void SetProgressText(string text) => progressText.text = text;

        public void SetCoinText(string text) => coinText.text = text;

        public void SetPunchButtonText(string text) => punchButtonText.text = text;

        public void SetSwapButtonText(string text) => swapButtonText.text = text;

        public void SetRefreshButtonText(string text) => refreshButtonText.text = text;


        public void AddPunchClickListener(UnityAction action) => punchButton.onClick.AddListener(action);
        public void RemovePunchClickListener(UnityAction action) => punchButton.onClick.RemoveListener(action);
        public void AddSwapButtonClickListener(UnityAction action) => swapButton.onClick.AddListener(action);
        public void RemoveSwapButtonClickListener(UnityAction action) => swapButton.onClick.RemoveListener(action);
        public void AddRefreshButtonClickListener(UnityAction action) => refreshButton.onClick.AddListener(action);

        public void AddBuyAbilityClickListener(UnityAction action) =>
            buyingAbilityDialog.AddButtonAClickListener(action);


        public void AddWatchAdForAbilityClickListener(UnityAction action) =>
            buyingAbilityDialog.AddButtonBClickListener(action);

        public void RemoveBuyAbilityClickListener(UnityAction action) =>
            buyingAbilityDialog.RemoveButtonAClickListener(action);

        public void RemoveWatchAdForAbilityClickListener(UnityAction action) =>
            buyingAbilityDialog.RemoveButtonBClickListener(action);

        public void RemoveRefreshButtonClickListener(UnityAction action) =>
            refreshButton.onClick.RemoveListener(action);

        public void AddAbilityCancelClickListener(UnityAction action) =>
            abilityCancelButton.onClick.AddListener(action);

        public void RemoveAbilityCancelButtonListener(UnityAction action) =>
            abilityCancelButton.onClick.AddListener(action);


        public void SetRefreshIntractable(bool isIntractable) => refreshButton.interactable = isIntractable;

        public void SetSwapIntractable(bool isIntractable) => swapButton.interactable = isIntractable;

        public void SetPunchIntractable(bool isIntractable) => punchButton.interactable = isIntractable;


        public void ShowBuyingAbilityDialog(AbilityData abilityData)
        {
            _abilityData = abilityData;
            buyingAbilityDialog.SetTitle(abilityData.name)
                .SetDescription(abilityData.description)
                .SetImage(abilityData.image)
                .SetButtonAText("Buy")
                .SetButtonBText("Watch Ad")
                .Show();
        }

        public void HideBuyingAbilityDialog() => buyingAbilityDialog.Hide();


        public void Show()
        {
            panel.SetActive(true);
            progressImage.gameObject.SetActive(true);
            progressText.gameObject.SetActive(true);
            progressBackgroundImage.gameObject.SetActive(true);
        }

        public void Hide()
        {
            panel.SetActive(false);
            progressImage.gameObject.SetActive(false);
            progressText.gameObject.SetActive(false);
            progressBackgroundImage.gameObject.SetActive(false);
        }

        public void ShowAbilityCancelButton() => abilityCancelButton.gameObject.SetActive(true);


        public void HideAbilityCancelButton() => abilityCancelButton.gameObject.SetActive(false);
    }
}