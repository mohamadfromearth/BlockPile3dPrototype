using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Designer
{
    public class BoardSetUpUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField sizeInput;

        [SerializeField] private Button clearButton;

        [SerializeField] private Button generateButton;

        [SerializeField] private Button cancelButton;


        [SerializeField] private GameObject panel;


        public void AddClearClickListener(UnityAction action) => clearButton.onClick.AddListener(action);

        public void RemoveClearClickListener(UnityAction action) => clearButton.onClick.RemoveListener(action);


        public void AddGenerateClickListener(UnityAction action) => generateButton.onClick.AddListener(action);

        public void AddCancelListener(UnityAction action) => cancelButton.onClick.AddListener(action);

        public void RemoveCancelListener(UnityAction action) => generateButton.onClick.RemoveListener(action);
        public void RemoveGenerateClickListener(UnityAction action) => generateButton.onClick.RemoveListener(action);


        public int GetSize() => Int32.Parse(sizeInput.text);


        public void Show() => panel.SetActive(true);

        public void Hide() => panel.SetActive(false);
    }
}