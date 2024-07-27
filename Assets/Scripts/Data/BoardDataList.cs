using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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


    [CreateAssetMenu(fileName = "BoardData", menuName = "so/BoardData")]
    public class BoardDataSo : ScriptableObject
    {
        public int size;
        public List<Vector3Int> emptyPositions;
        public List<BlockContainerData> blockContainersDataList;
        public List<Vector3Int> advertiseBlocks;
        public List<LockBlockData> lockBlockDataList;
        public int targetScore;
        
    }


    [System.Serializable]
    public struct LockBlockData
    {
        public Vector3Int position;
        public int count;
    }
}