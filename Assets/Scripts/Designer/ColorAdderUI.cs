using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Designer
{
    public class ColorAdderUI : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropDown;
        [SerializeField] private Button createButton;
        [SerializeField] private GameObject panel;


        public void SetColors(List<string> colors)
        {
            dropDown.options =
                colors.Select(color => new TMP_Dropdown.OptionData(color)).ToList();
        }

        public void AddCreateClickListener(UnityAction action) => createButton.onClick.AddListener(action);
        public void RemoveCreateClickListener(UnityAction action) => createButton.onClick.RemoveListener(action);

        public int GetValue() => dropDown.value;


        public void Show() => panel.SetActive(true);

        public void Hide() => panel.SetActive(false);
    }
}