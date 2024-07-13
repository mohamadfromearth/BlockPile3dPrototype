using System.Collections.Generic;
using Scripts.Data;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "so/LevelRepository", fileName = "LevelRepository")]
    public class LevelRepository : ScriptableObject, ILevelRepository
    {
        [SerializeField] private List<LevelData> levelDataList;


        private int _levelIndex;

        public LevelData GetLevelData() => levelDataList[_levelIndex];

        public void NextLevel()
        {
            if (_levelIndex < levelDataList.Count - 1) _levelIndex++;
        }
    }
}