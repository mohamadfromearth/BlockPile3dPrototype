using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public class ListUtils
    {
        public static int GetIndex(Vector3 position, List<Vector3> positionList)
        {
            for (int i = 0; i < positionList.Count; i++)
            {
                if (position == positionList[i])
                {
                    return i;
                }
            }

            return -1;
        }


        public static List<int> GetUniqueRandomIntList(int count, int min, int max)
        {
            HashSet<int> uniqueInts = new HashSet<int>();
            while (uniqueInts.Count < count)
            {
                int newInt = Random.Range(min, max);
                uniqueInts.Add(newInt);
            }

            return uniqueInts.ToList();
        }
    }


    public static class ListUtilsExt
    {
        public static List<T> Shuffle<T>(this List<T> list)
        {
            List<T> clonedList = new List<T>(list);

            var rng = new System.Random();
            int n = clonedList.Count;

            for (int i = n - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                T temp = clonedList[i];
                clonedList[i] = clonedList[j];
                clonedList[j] = temp;
            }

            return clonedList;
        }
    }
}