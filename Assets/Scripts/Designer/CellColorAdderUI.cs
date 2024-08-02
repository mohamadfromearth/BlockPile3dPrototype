using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Designer
{
    public class CellColorAdderUI : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropDown;

        [SerializeField] private TMP_InputField colorsCountInput;

        [SerializeField] private Button addButton;

        [SerializeField] private Button cancelButton;

        [SerializeField] private GameObject panel;


        public void SetColors(List<string> colors)
        {
            dropDown.options = colors.Select((s, i) =>
            {
                return new TMP_Dropdown.OptionData(s);
            }).ToList();
        }

        private void OnEnable()
        {
            cancelButton.onClick.AddListener(Hide);
        }

        private void OnDisable()
        {
            cancelButton.onClick.RemoveListener(Hide);
        }


        public void AddDropDownSelectListener(UnityAction<int> action) => dropDown.onValueChanged.AddListener(action);


        public void RemoveDropDownSelectListener(UnityAction<int> action) =>
            dropDown.onValueChanged.RemoveListener(action);

        public void AddAddClickListener(UnityAction action) => addButton.onClick.AddListener(action);

        public void RemoveAddClickListener(UnityAction action) => addButton.onClick.RemoveListener(action);

        public int GetColorsCount() => Int32.Parse(colorsCountInput.text);


        public int GetColorIndex() => dropDown.value;


        public void Show() => panel.SetActive(true);

        public void Hide() => panel.SetActive(false);
    }
}