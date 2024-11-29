using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;

namespace UI
{
    [System.Serializable]
    public class TwoButtonsDialog
    {
        [SerializeField] private Button rightButton;
        [SerializeField] private Button leftButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Transform backGround;
        [SerializeField] private GameObject panel;


        public void AddRightButtonClickListener(UnityAction action) => rightButton.onClick.AddListener(action);

        public void RemoveRightButtonClickListener(UnityAction action) => rightButton.onClick.RemoveListener(action);


        public void AddLeftButtonClickListener(UnityAction action) => leftButton.onClick.AddListener(action);
        public void RemoveLeftButtonClickListener(UnityAction action) => leftButton.onClick.AddListener(action);


        public void AddCancelClickListener(UnityAction action) => cancelButton.onClick.AddListener(action);
        public void RemoveCancelClickListener(UnityAction action) => cancelButton.onClick.RemoveListener(action);


        public void Show()
        {
            panel.SetActive(true);
            backGround.ShowPopUp();
        }


        public void Hide() => backGround.HidePopUp(panel.transform);
    }
}