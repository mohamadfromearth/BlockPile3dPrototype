using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [CreateAssetMenu(menuName = "so/LevelRepository", fileName = "LevelRepository")]
    public class LevelRepository : ScriptableObject, ILevelRepository
    {
        [SerializeField] private List<LevelDataSo> levelDataList;


         public int levelIndex;


        public LevelDataSo GetLevelData() => levelDataList[levelIndex];

        public void NextLevel()
        {
            if (levelIndex < levelDataList.Count - 1) levelIndex++;
        }
    }
}