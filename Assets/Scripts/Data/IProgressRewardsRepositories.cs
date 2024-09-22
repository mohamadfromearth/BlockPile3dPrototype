using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "So/ProgressRewardRepository")]
    public class ProgressRewardsRepository : ScriptableObject, IProgressRewardsRepository
    {
        private const string SpinLevelIndexKey = "SPIN_LEVEL_INDEX_KEY";


        public int SpinLevelIndex => spinLevelIndex;

        public int SpinLevelTarget => spinTargetsList[spinLevelIndex];


        [SerializeField] private List<int> spinTargetsList;

        [SerializeField] private int spinLevelIndex;


        private void OnEnable()
        {
            spinLevelIndex = PlayerPrefs.GetInt(SpinLevelIndexKey, 0);
        }


        public void IncreaseIndex()
        {
            if (spinLevelIndex == spinTargetsList.Count - 1) return;

            spinLevelIndex++;
            PlayerPrefs.SetInt(SpinLevelIndexKey, spinLevelIndex);
        }
    }
}