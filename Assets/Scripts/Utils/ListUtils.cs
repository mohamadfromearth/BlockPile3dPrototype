using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scrips.Utils
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
}