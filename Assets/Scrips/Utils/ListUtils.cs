using System.Collections.Generic;
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
    }
}