using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Designer
{
    public class AddLockUI : MonoBehaviour
    {
        [SerializeField] private Button createButton;
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_InputField countInputField;


        public void AddCreateClickListener(UnityAction action) => createButton.onClick.AddListener(action);


        public void RemoveCreateClickListener(UnityAction action) => createButton.onClick.RemoveListener(action);

        public void Show() => panel.SetActive(true);

        public void Hide() => panel.SetActive(false);

        public int GetCount() => Int32.Parse(countInputField.text);
    }
}