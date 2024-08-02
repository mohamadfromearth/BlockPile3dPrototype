using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "so/ColorRepo", fileName = "ColorRepo")]
    public class ColorRepository : ScriptableObject
    {
        public List<Color> colors;

        public List<string> colorsNames;


        public Color GetColor(int index) => colors[index];

        public string GetColorName(int index) => colorsNames[index];

        public List<string> GetColorsNames() => colorsNames;
    }
}