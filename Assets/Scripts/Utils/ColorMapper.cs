using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Utils
{
    public static class ColorMapper
    {
        private static Dictionary<string, int> colorToIndexDic = new()
        {
            { "Blue", 0 },
            { "Red", 1 },
            { "Yellow", 2 },
            { "Light Blue", 3 },
            { "Green", 4 },
            { "Light pink", 5 },
            { "Black", 6 },
            { "Orange", 7 },
        };


        public static int ToColorIndex(this string colorName)
        {
            return colorToIndexDic[colorName];
        }
    }
}