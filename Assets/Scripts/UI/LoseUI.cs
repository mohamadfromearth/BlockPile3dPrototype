using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class LoseUI : MonoBehaviour
    {
        [SerializeField] private Button retryButton;
        [SerializeField] private Button advertiseReviveButton;
        [SerializeField] private Button coinReviveButton;
        [SerializeField] private Button cancelButton;

        [SerializeField] private GameObject panel;
        [SerializeField] private Transform background;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI needToCollectText;


        public void AddRetryClickListener(UnityAction action) => retryButton.onClick.AddListener(action);
        public void RemoveRetryClickListener(UnityAction action) => retryButton.onClick.RemoveListener(action);

        public void AddCoinReviveClickListener(UnityAction action) =>
            coinReviveButton.onClick.AddListener(action);

        public void RemoveCoinReviveClickListener(UnityAction action) =>
            coinReviveButton.onClick.RemoveListener(action);

        public void AddAdvertiseReviveClickListener(UnityAction action) =>
            advertiseReviveButton.onClick.AddListener(action);


        public void RemoveAdvertiseReviveClickListener(UnityAction action) =>
            advertiseReviveButton.onClick.RemoveListener(action);


        public void AddCancelClickListener(UnityAction action) =>
            cancelButton.onClick.AddListener(action);

        public void RemoveCancelClickListener(UnityAction action) =>
            cancelButton.onClick.RemoveListener(action);

        public void Show(string level, string needToCollect)
        {
            panel.SetActive(true);
            background.ShowPopUp();
            levelText.text = level;
            needToCollectText.text = needToCollect;
        }

        public void Hide()
        {
            background.localScale = Vector3.zero;
            panel.SetActive(false);
        }
    }
}