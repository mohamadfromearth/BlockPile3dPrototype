using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "so/LevelRepository", fileName = "LevelRepository")]
    public class LevelRepository : ScriptableObject, ILevelRepository
    {
        [SerializeField] private List<LevelDataSo> levelDataList;


        private const string LevelIndexPrefKey = "LevelIndexPrefKey";



        private int _levelIndex;

       

        private void OnEnable()
        {
            _levelIndex = PlayerPrefs.GetInt(LevelIndexPrefKey, 0);
        }

        public int LevelIndex => _levelIndex;


        public LevelDataSo GetLevelData()
        {
            if (_levelIndex > levelDataList.Count - 1)
            {
                return levelDataList[0];
            }

            return levelDataList[_levelIndex];
        }

        public void Clear()
        {
            _levelIndex = 0;
            PlayerPrefs.SetInt(LevelIndexPrefKey, _levelIndex);
        }

        public void NextLevel()
        {
            _levelIndex++;

            if (_levelIndex < levelDataList.Count)
            {
                PlayerPrefs.SetInt(LevelIndexPrefKey, _levelIndex);
            }
        }

        public bool IsLastLevel() => _levelIndex > levelDataList.Count - 1;
    }
}