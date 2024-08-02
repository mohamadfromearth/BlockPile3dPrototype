using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Designer
{
    public class DesignerUI : MonoBehaviour
    {
        [SerializeField] private Button setUpBoardButton;

        [SerializeField] private Button addColorButton;

        [SerializeField] private VerticalLayoutGroup colorsLg;

        [SerializeField] private TMP_InputField targetScoreInputField;


        private Stack<TextMeshProUGUI> _colorsTexts = new();

        public void AddSetUpBoardClickListener(UnityAction action) => setUpBoardButton.onClick.AddListener(action);

        public void RemoveSetUpBoardClickListener(UnityAction action) =>
            setUpBoardButton.onClick.RemoveListener(action);


        public void AddColor(string colorName)
        {
            var colorTextMeshPro = new GameObject().AddComponent<TextMeshProUGUI>();
            colorTextMeshPro.fontSize = 50;

            colorTextMeshPro.enableAutoSizing = true;

            colorTextMeshPro.text = colorName;

            colorTextMeshPro.transform.SetParent(colorsLg.transform);

            _colorsTexts.Push(colorTextMeshPro);
        }

        public void RemoveColor()
        {
            var textMeshPro = _colorsTexts.Pop();
            Destroy(textMeshPro.gameObject);
        }


        public int GetTargetScore() => Int32.Parse(targetScoreInputField.text);
    }
}