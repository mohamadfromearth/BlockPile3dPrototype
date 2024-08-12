using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private Image progressImage;
        [SerializeField] private Button punchButton;
        [SerializeField] private Button swapButton;
        [SerializeField] private Button refreshButton;
        [SerializeField] private HorizontalLayoutGroup abilityBarLg;


        public void SetProgress(float value) => progressImage.fillAmount = value;

        public void SetProgressText(string text) => progressText.text = text;

        public void ShowAbilityBar() => abilityBarLg.gameObject.SetActive(true);
        public void HideAbilityBar() => abilityBarLg.gameObject.SetActive(false);


        public void AddPunchClickListener(UnityAction action) => punchButton.onClick.AddListener(action);
        public void RemovePunchClickListener(UnityAction action) => punchButton.onClick.RemoveListener(action);
        public void AddSwapButtonClickListener(UnityAction action) => swapButton.onClick.AddListener(action);
        public void RemoveSwapButtonClickListener(UnityAction action) => swapButton.onClick.RemoveListener(action);
        public void AddRefreshButtonClickListener(UnityAction action) => refreshButton.onClick.AddListener(action);

        public void RemoveRefreshButtonClickListener(UnityAction action) =>
            refreshButton.onClick.RemoveListener(action);
    }
}