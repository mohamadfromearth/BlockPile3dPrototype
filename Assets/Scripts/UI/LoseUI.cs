using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class LoseUI : MonoBehaviour
    {
        [SerializeField] private Button retryButton;
        [SerializeField] private Button getChanceButton;

        [SerializeField] private GameObject panel;


        public void AddRetryClickListener(UnityAction action) => retryButton.onClick.AddListener(action);
        public void RemoveRetryClickListener(UnityAction action) => retryButton.onClick.RemoveListener(action);

        public void AddGetChanceClickListener(UnityAction action) => getChanceButton.onClick.AddListener(action);

        public void RemoveGetChanceClickListener(UnityAction action) => getChanceButton.onClick.RemoveListener(action);
        public void Show() => panel.SetActive(true);

        public void Hide() => panel.SetActive(false);
    }
}