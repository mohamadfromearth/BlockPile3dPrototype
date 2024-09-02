using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

//
// public enum LevelDifficulty
// {
//     Amateur,
//     Beginner,
//     SemiPro,
//     Pro,
//     WorldClass
// }
//
//
// public class LevelGenerator
// {
//     private BoardDataList _boardDataList;
//
//     private List<Color> _colors;
//
//     private List<int> _targetScoreList = new List<int>()
//     {
//         100,
//         200,
//         300,
//         340,
//         360,
//         370,
//         400
//     };
//
//     private int _index;
//
//
//     public LevelGenerator(BoardDataList boardDataList, List<Color> colors)
//     {
//         _boardDataList = boardDataList;
//         _colors = colors;
//     }
//
//
//     public LevelData Generate()
//     {
//         LevelData levelData = new LevelData();
//
//
//         var boardData = GetBoardData();
//         boardData.InitPositions();
//
//         var colorIndices = ListUtils.GetUniqueRandomIntList(4, 0, _colors.Count);
//
//         var colors = new List<Color>();
//         foreach (var colorIndex in colorIndices) colors.Add(_colors[colorIndex]);
//
//
//         levelData.colors = colors;
//         levelData.height = boardData.size;
//         levelData.width = boardData.size;
//         levelData.blockContainerDataList = GetContainers(boardData, colorIndices);
//         levelData.emptyHoldersPosList = boardData.emptyPosArray;
//         levelData.targetScore = 100;
//
//         levelData.targetScore = _targetScoreList[_index];
//
//         if (_index < _targetScoreList.Count - 1) _index++;
//
//
//         return levelData;
//     }
//
//
//     private BoardData GetBoardData()
//     {
//         var index = Random.Range(0, _boardDataList.boardDataArray.Length);
//         return _boardDataList.boardDataArray[index];
//     }
//
//     private List<BlockContainerData> GetContainers(BoardData boardData, List<int> colorIndices)
//     {
//         List<BlockContainerData> blockContainers = new();
//         List<Color> colors = new();
//
//         var count = Random.Range(2, 5);
//         var index = 0;
//
//         while (index < count)
//         {
//             var position = GetRandomPosition(boardData.positions, out var positionIndex);
//
//             var colorIndicesClone = colorIndices.ToList();
//
//             boardData.positions.RemoveAt(positionIndex);
//
//             var blockContainerData = new BlockContainerData();
//             blockContainerData.position = position;
//
//
//             var colorsCount = Random.Range(1, 4);
//
//
//             for (int i = 0; i < colorsCount; i++)
//             {
//                 var colorIndex = Random.Range(0, colorIndicesClone.Count);
//                 var innerColorCount = Random.Range(1, 6);
//
//                 for (int j = 0; j < innerColorCount; j++)
//                 {
//                     colors.Add(_colors[colorIndices[colorIndex]]);
//                 }
//
//                 colorIndicesClone.RemoveAt(colorIndex);
//             }
//
//             blockContainerData.color = colors;
//
//             colors = new List<Color>();
//
//             blockContainers.Add(blockContainerData);
//
//
//             index++;
//         }
//
//         return blockContainers;
//     }
//
//     private Vector3Int GetRandomPosition(List<Vector3Int> cellPositions, out int index)
//     {
//         index = Random.Range(0, cellPositions.Count);
//         return cellPositions[index];
//     }
// }