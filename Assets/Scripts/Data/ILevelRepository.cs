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
        public List<string> color;
    }


    public interface ILevelRepository
    {
        public int LevelIndex { get; }

        public LevelDataSo GetLevelData();

        public void NextLevel();
    }
}