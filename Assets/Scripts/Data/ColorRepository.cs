using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "so/ColorRepo", fileName = "ColorRepo")]
    public class ColorRepository : ScriptableObject
    {
        public List<Color> colors;


        public Color GetColor(int index) => colors[index];
    }
}