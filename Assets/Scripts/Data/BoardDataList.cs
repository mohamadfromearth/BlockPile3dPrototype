using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "BoardDataList", menuName = "so/BoardDataList")]
    public class BoardDataList : ScriptableObject
    {
        public BoardData[] boardDataArray;
    }

    [Serializable]
    public struct BoardData
    {
        public int size;
        public List<Vector3Int> emptyPosArray;
        public List<Vector3Int> positions;
        public float cameraSize;

        public void InitPositions()
        {
            positions = new();

            for (int x = 0; x < size; x++)
            {
                for (int z = 0; z < size; z++)
                {
                    var position = new Vector3Int(x, 0, z);
                    if (emptyPosArray.Contains(position) == false) positions.Add(position);
                }
            }
        }
    }
}