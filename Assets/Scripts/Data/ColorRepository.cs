using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
    [Serializable]
    public struct ColorData
    {
        public string name;
        public Color color;
    }

    [CreateAssetMenu(menuName = "so/ColorRepo", fileName = "ColorRepo")]
    public class ColorRepository : ScriptableObject
    {
        public List<ColorData> colorDataList;

        private Dictionary<string, Color> _colors = new();


        private void OnEnable()
        {
            foreach (var colorData in colorDataList)
            {
                _colors[colorData.name] = colorData.color;
            }
        }


        public Color GetColor(int index) => colorDataList[index].color;

        public string GetColorName(int index) => colorDataList[index].name;

        public List<string> GetColorsNames() => colorDataList.Select(data => data.name).ToList();

        public Color GetColor(string name) => _colors[name];

        public string GetName(Color color)
        {
            foreach (var keyValuePair in _colors)
            {
                if (keyValuePair.Value == color) return keyValuePair.Key;
            }

            throw new Exception("Color is not found !!!");
        }
    }
}