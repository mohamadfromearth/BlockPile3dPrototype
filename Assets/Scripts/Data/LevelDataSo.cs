using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "so/levelData")]
    public class LevelDataSo : ScriptableObject
    {
        public int size;
        public List<string> colors;
        public List<BlockContainerData> blockContainerDataList;
        public List<Vector3Int> emptyHoldersPosList;
        public List<Vector3Int> advertiseBlocks;
        public List<LockBlockData> lockBlocks;
        public int targetScore;
        public Vector3Int leftEdgePosition;
        public Vector3Int rightEdgePosition;
    }

    [System.Serializable]
    public struct LockBlock
    {
        public Vector3Int Position;
        public int Count;

        public LockBlock(Vector3Int position, int count)
        {
            Position = position;
            Count = count;
        }
    }
}