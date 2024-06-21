using System.Collections.Generic;
using Scripts.Data;
using UnityEngine;

namespace Scrips.Data
{
    [CreateAssetMenu(menuName = "so/LevelRepository", fileName = "LevelRepository")]
    public class LevelRepository : ScriptableObject, ILevelRepository
    {
        [SerializeField] private List<LevelData> levelDataList;


        private int levelIndex = 0;

        public LevelData GetLevelData() => levelDataList[levelIndex];
    }
}