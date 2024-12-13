using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    [System.Serializable]
    public class EndingUI
    {
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private float showingDuration;
        [SerializeField] private GameObject panel;


        public float ShowingDuration => showingDuration;


        public void Show()
        {
            panel.SetActive(true);
            messageText.transform.localScale = Vector3.zero;
            messageText.transform.ShowPopUp();
        }

        public void Hide() => panel.SetActive(false);
    }
}