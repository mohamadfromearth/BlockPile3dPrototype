using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class WinUI : MonoBehaviour
    {
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private GameObject panel;


        public void AddNextLevelClickListener(UnityAction action) => nextLevelButton.onClick.AddListener(action);

        public void RemoveNextLevelClickListener(UnityAction action) => nextLevelButton.onClick.RemoveListener(action);

        public void Show() => panel.SetActive(true);

        public void Hide() => panel.SetActive(false);
    }
}