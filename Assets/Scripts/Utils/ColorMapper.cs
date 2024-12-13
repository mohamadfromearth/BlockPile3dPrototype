using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Data;
using UnityEngine;

namespace Utils
{
    public static class ColorMapper
    {
        private static Dictionary<string, int> colorStringToIndexDic = new();


        public static void SetColorStringToIndexDic(List<ColorData> colorDataList)
        {
            for (int i = 0; i < colorDataList.Count; i++)
            {
                var colorData = colorDataList[i];
                colorStringToIndexDic[colorData.name] = i;
            }
        }


        public static int ToColorIndex(this string colorName)
        {
            return colorStringToIndexDic[colorName];
        }


        public static int ToColorIndex(this Color color)
        {
            Debug.Log("Color is " + color);

            Color blue = Color.white;
            blue.r = 49;
            blue.g = 57;
            blue.b = 194;

            Color yellow = Color.white;
            yellow.r = 255;
            yellow.g = 237;
            yellow.b = 0;

            Color red = Color.white;
            red.r = 220;
            red.g = 0;
            red.b = 0;

            Color lightBlue = Color.white;
            lightBlue.r = 47;
            lightBlue.g = 94;
            lightBlue.b = 186;

            Color green = Color.green;
            green.r = 36;
            green.g = 233;
            green.b = 36;

            Color lightpink = Color.white;
            lightpink.r = 178;
            lightpink.g = 86;
            lightpink.b = 151;

            Color black = Color.black;

            Color orange = Color.white;
            orange.r = 255;
            orange.g = 86;
            orange.b = 0;

            if (color == blue) return 0;
            if (color == yellow) return 1;
            if (color == red) return 2;
            if (color == lightBlue) return 3;
            if (color == green) return 4;
            if (color == lightpink) return 5;
            if (color == black) return 6;
            if (color == orange) return 7;

            return -1;
        }
    }
}