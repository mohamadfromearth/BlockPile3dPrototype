using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scrips.Data
{
    [Serializable]
    public struct LevelData
    {
        public int width, height;
        public List<BlockContainerData> blockContainerDataList;
        public List<Vector3Int> emptyHoldersPosList;
        public List<BlockContainerData> selectionBarBlockContainerDataList;
    }


    [Serializable]
    public struct BlockContainerData
    {
        public Vector3Int position;
        public Color color;
        public List<BlockGroupData> blockGroupDataList;
    }

    [Serializable]
    public struct BlockGroupData
    {
        public Color color;
        public int count;
    }


    public interface ILevelRepository
    {
        public LevelData GetLevelData();
    }
}