using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CoreUI
{
    public class DialogueTypeA : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Image image;
        [SerializeField] private Button buttonA;
        [SerializeField] private Button buttonB;
        [SerializeField] private Image buttonAImage;
        [SerializeField] private Image buttonBImage;
        [SerializeField] private TextMeshProUGUI buttonAText;
        [SerializeField] private TextMeshProUGUI buttonBText;
        [SerializeField] private GameObject panel;


        public DialogueTypeA SetTitle(string title)
        {
            titleText.text = title;
            return this;
        }

        public DialogueTypeA SetDescription(string description)
        {
            descriptionText.text = description;
            return this;
        }

        public DialogueTypeA SetImage(Sprite sprite)
        {
            image.sprite = sprite;
            return this;
        }

        public DialogueTypeA SetButtonAImage(Sprite sprite)
        {
            buttonAImage.sprite = sprite;
            return this;
        }

        public DialogueTypeA SetButtonBImage(Sprite sprite)
        {
            buttonBImage.sprite = sprite;
            return this;
        }

        public DialogueTypeA SetButtonAText(string text)
        {
            buttonAText.text = text;
            return this;
        }

        public DialogueTypeA SetButtonBText(string text)
        {
            buttonBText.text = text;
            return this;
        }

        public DialogueTypeA AddButtonAClickListener(UnityAction action)
        {
            buttonA.onClick.AddListener(action);
            return this;
        }

        public DialogueTypeA AddButtonBClickListener(UnityAction action)
        {
            buttonB.onClick.AddListener(action);
            return this;
        }

        public void RemoveButtonAClickListener(UnityAction action)
        {
            buttonA.onClick.RemoveListener(action);
        }

        public void RemoveButtonBClickListener(UnityAction action)
        {
            buttonB.onClick.RemoveListener(action);
        }

        public void Show() => panel.SetActive(true);

        public void Hide() => panel.SetActive(false);
    }
}