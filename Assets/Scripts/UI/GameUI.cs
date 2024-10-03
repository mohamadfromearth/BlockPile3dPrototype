using System;
using Data;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;

namespace UI
{
    [Serializable]
    public class AbilityButton
    {
        public Button button;
        public Image countBackground;
        public TextMeshProUGUI countText;
        public Image lockImage;
        public TextMeshProUGUI unLockLevelText;

        public void SetIntractable(bool isIntractable)
        {
            button.enabled = isIntractable;
            countBackground.gameObject.SetActive(isIntractable);
            countText.gameObject.SetActive(isIntractable);
            lockImage.gameObject.SetActive(!isIntractable);
            unLockLevelText.gameObject.SetActive(!isIntractable);
        }
    }


    public class GameUI : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private Image progressImage;
        [SerializeField] private Transform blockProgressImage;
        [SerializeField] private Image background;
        [SerializeField] private Sprite verticalSprite;
        [SerializeField] private Sprite horizontalSprite;

        [SerializeField] private Image progressBackgroundImage;

        [SerializeField] private Transform blockToTargetScoreImage;


        [SerializeField] private AbilityButton punchButton;
        [SerializeField] private AbilityButton swapButton;
        [SerializeField] private AbilityButton refreshButton;

        [SerializeField] private HorizontalLayoutGroup abilitiesHl;
        [SerializeField] private VerticalLayoutGroup abilitiesVl;

        [SerializeField] private Transform verticalY;
        [SerializeField] private Transform horizontalY;

        [SerializeField] private Button settingButton;
        [SerializeField] private Transform coinTransform;
        [SerializeField] private Transform progressTransform;
        [SerializeField] private Transform blocksImageTransform;

        [SerializeField] private TargetGoalUI targetGoal;


        [SerializeField] private BuyAbilityDialog buyingAbilityDialog;

//private DialogueTypeA buyingAbilityDialog;
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private Button abilityCancelButton;
        [SerializeField] private TextMeshProUGUI abilityHintText;
        [SerializeField] private Image abilityHintImage;
        [SerializeField] private GameObject abilityHint;
        private AbilityData _abilityData;

        [SerializeField] private float progressFillingDuration = 0.2f;
        [SerializeField] private float blockToProgressDuration = 0.5f;
        [SerializeField] private float progressTextAnimationDuration = 1f;
        [SerializeField] private Ease blockToProgressEase = Ease.Linear;

        public AbilityData AbilityData => _abilityData;


        private Action blockToProgressAnimationFinished;

        private void Start()
        {
            bool isLandScape = Screen.width > Screen.height;

            float y = isLandScape ? horizontalY.position.y : verticalY.position.y;

            progressTransform.position = new Vector3(progressTransform.position.x, y, progressTransform.position.z);
            coinTransform.position = new Vector3(coinTransform.position.x, y, coinTransform.position.z);
            settingButton.transform.position =
                new Vector3(settingButton.transform.position.x, y, settingButton.transform.position.z);

            var abilityButtonsParent = isLandScape ? abilitiesVl.transform : abilitiesHl.transform;
            punchButton.button.transform.SetParent(abilityButtonsParent);
            swapButton.button.transform.SetParent(abilityButtonsParent);
            refreshButton.button.transform.SetParent(abilityButtonsParent);

            background.sprite = isLandScape ? horizontalSprite : verticalSprite;
        }


        public void ShowTargetGoal(string level, string goal) => StartCoroutine(targetGoal.Show(
            level,
            goal
        ));


        public void SetProgress(float value) => progressImage.DOFillAmount(value, progressFillingDuration);

        public void SetProgressText(string text) => progressText.text = text;

        public void AnimateProgressText(int startValue, int endValue, string extraText)
        {
            StartCoroutine(progressText.AnimateTextCounter(startValue, endValue, extraText,
                progressTextAnimationDuration));
        }

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
            buyingAbilityDialog.AddBuyClickListener(action);


        public void AddWatchAdForAbilityClickListener(UnityAction action) =>
            buyingAbilityDialog.AddWatchAddClickListener(action);

        public void RemoveBuyAbilityClickListener(UnityAction action) =>
            buyingAbilityDialog.RemoveWatchAddClickListener(action);

        public void AddBuyingAbilityCancelClickListener(UnityAction action) =>
            buyingAbilityDialog.AddCancelClickListener(action);

        public void RemoveBuyingAbilityCancelClickListener(UnityAction action) =>
            buyingAbilityDialog.RemoveCancelClickListener(action);

        public void RemoveWatchAdForAbilityClickListener(UnityAction action) =>
            buyingAbilityDialog.RemoveWatchAddClickListener(action);

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

            buyingAbilityDialog.Show(abilityData.name.ToString(), abilityData.description.ToString(),
                abilityData.cost.ToString(), abilityData.image
            );
        }

        public void HideBuyingAbilityDialog() => buyingAbilityDialog.Hide();


        public void ShowBlockToProgressAnimation(Vector3 position)
        {
            var screenPosition = camera.WorldToScreenPoint(position);
            blockToTargetScoreImage.gameObject.SetActive(true);
            blockToTargetScoreImage.localScale = Vector3.one;
            blockToTargetScoreImage.position = screenPosition;
            blockToTargetScoreImage.DOScale(Vector3.zero, blockToProgressDuration + blockToProgressDuration);
            var tween = blockToTargetScoreImage.DOMove(blockProgressImage.position, blockToProgressDuration);
            tween.SetEase(blockToProgressEase);
            tween.onComplete = () =>
            {
                blockToTargetScoreImage.gameObject.SetActive(false);
                blockToProgressAnimationFinished?.Invoke();
            };
        }

        public void AddBlockToProgressAnimationFinishListener(Action action) =>
            blockToProgressAnimationFinished += action;


        public void RemoveBlockToProgressAnimationFinishListener(Action action) =>
            blockToProgressAnimationFinished -= action;


        public void Show()
        {
            panel.SetActive(true);
            progressImage.gameObject.SetActive(true);
            progressText.gameObject.SetActive(true);
            progressBackgroundImage.gameObject.SetActive(true);
            blocksImageTransform.gameObject.SetActive(true);
        }

        public void Hide()
        {
            panel.SetActive(false);
            progressImage.gameObject.SetActive(false);
            progressText.gameObject.SetActive(false);
            progressBackgroundImage.gameObject.SetActive(false);
            blocksImageTransform.gameObject.SetActive(false);
        }

        public void ShowAbilityHintButton(AbilityData abilityData)
        {
            abilityHint.SetActive(true);
            abilityHintText.text = abilityData.description;
            abilityHintImage.sprite = abilityData.image;
            abilityHint.transform.ShowPopUp();
        }


        public void HideAbilityHintButton()
        {
            abilityHint.SetActive(false);
            abilityHint.transform.localScale = Vector3.zero;
        }
    }
}