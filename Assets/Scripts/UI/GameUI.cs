using System;
using CoreUI;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    [Serializable]
    public struct AbilityButton
    {
        public Button button;
        public Image countBackground;
        public TextMeshProUGUI countText;
        public Image lockImage;
        public TextMeshProUGUI unLockLevelText;

        public void SetIntractable(bool isIntractable)
        {
            button.interactable = isIntractable;
            countBackground.gameObject.SetActive(isIntractable);
            countText.gameObject.SetActive(isIntractable);
            lockImage.gameObject.SetActive(!isIntractable);
            unLockLevelText.gameObject.SetActive(!isIntractable);
        }
    }

    public class GameUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private Image progressImage;

        [SerializeField] private Image progressBackgroundImage;


        [SerializeField] private AbilityButton punchButton;
        [SerializeField] private AbilityButton swapButton;
        [SerializeField] private AbilityButton refreshButton;


        [SerializeField] private DialogueTypeA buyingAbilityDialog;
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private Button abilityCancelButton;
        private AbilityData _abilityData;

        public AbilityData AbilityData => _abilityData;


        public void SetProgress(float value) => progressImage.fillAmount = value;

        public void SetProgressText(string text) => progressText.text = text;

        public void SetCoinText(string text) => coinText.text = text;

        public void SetPunchCountText(string text) => punchButton.countText.text = text;

        public void SetSwapCountText(string text) => swapButton.countText.text = text;

        public void SetRefreshCountText(string text) => refreshButton.countText.text = text;


        public void AddPunchClickListener(UnityAction action) =>
            punchButton.button.onClick.AddListener(action);

        public void RemovePunchClickListener(UnityAction action) => punchButton.button.onClick.RemoveListener(action);
        public void AddSwapButtonClickListener(UnityAction action) => swapButton.button.onClick.AddListener(action);

        public void RemoveSwapButtonClickListener(UnityAction action) =>
            swapButton.button.onClick.RemoveListener(action);

        public void AddRefreshButtonClickListener(UnityAction action) =>
            refreshButton.button.onClick.AddListener(action);

        public void AddBuyAbilityClickListener(UnityAction action) =>
            buyingAbilityDialog.AddButtonAClickListener(action);


        public void AddWatchAdForAbilityClickListener(UnityAction action) =>
            buyingAbilityDialog.AddButtonBClickListener(action);

        public void RemoveBuyAbilityClickListener(UnityAction action) =>
            buyingAbilityDialog.RemoveButtonAClickListener(action);

        public void RemoveWatchAdForAbilityClickListener(UnityAction action) =>
            buyingAbilityDialog.RemoveButtonBClickListener(action);

        public void RemoveRefreshButtonClickListener(UnityAction action) =>
            refreshButton.button.onClick.RemoveListener(action);

        public void AddAbilityCancelClickListener(UnityAction action) =>
            abilityCancelButton.onClick.AddListener(action);

        public void RemoveAbilityCancelButtonListener(UnityAction action) =>
            abilityCancelButton.onClick.AddListener(action);


        public void SetRefreshIntractable(bool isIntractable) => refreshButton.SetIntractable(isIntractable);


        public void SetSwapIntractable(bool isIntractable) => swapButton.SetIntractable(isIntractable);

        public void SetPunchIntractable(bool isIntractable) => punchButton.SetIntractable(isIntractable);

        public void SetRefreshUnLockLevelText(string text) => refreshButton.unLockLevelText.text = text;
        public void SetSwapUnLockLevelText(string text) => swapButton.unLockLevelText.text = text;
        public void SetPunchUnLockLevelText(string text) => punchButton.unLockLevelText.text = text;


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