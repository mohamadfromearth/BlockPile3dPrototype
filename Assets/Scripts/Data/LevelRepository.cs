using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "so/LevelRepository", fileName = "LevelRepository")]
    public class LevelRepository : ScriptableObject, ILevelRepository
    {
        [SerializeField] private List<LevelDataSo> levelDataList;


        private int _levelIndex;


        public LevelDataSo GetLevelData() => levelDataList[_levelIndex];

        public void NextLevel()
        {
            if (_levelIndex < levelDataList.Count - 1) _levelIndex++;
        }
    }
}