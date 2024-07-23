using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [Serializable]
    public struct LevelData
    {
        public int width, height;
        public List<Color> colors;
        public List<BlockContainerData> blockContainerDataList;
        public List<Vector3Int> emptyHoldersPosList;
        public int targetScore;
    }


    [Serializable]
    public struct BlockContainerData
    {
        public Vector3Int position;
        public List<Color> color;
    }


    public interface ILevelRepository
    {
        public LevelData GetLevelData();

        public void NextLevel();
    }
}