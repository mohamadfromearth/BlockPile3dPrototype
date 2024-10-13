using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "AbilityRepository", menuName = "so/AbilityRepository")]
    public class AbilityRepository : ScriptableObject
    {
        [SerializeField] private List<AbilityData> abilityDataList;

        private Dictionary<AbilityType, AbilityData> _abilityDataDic = new();

        private Dictionary<int, AbilityData> _levelAbilityDataDic = new();

        private const string AbilityPrefKey = "ABILITY_PREF_KEY";

        private const string AbilityUnlockPrefKey = "ABILITY_UNLOCK_PREF_KEY";


        private void OnEnable()
        {
            foreach (var abilityData in abilityDataList)
            {
                _abilityDataDic[abilityData.type] = abilityData;
                abilityData.count = PlayerPrefs.GetInt(AbilityPrefKey + abilityData.type, 0);

                Debug.Log("Key is " + AbilityUnlockPrefKey + abilityData.type);

                abilityData.isUnlocked =
                    PlayerPrefs.GetInt(AbilityUnlockPrefKey + abilityData.type, 0) != 0;

                _levelAbilityDataDic[abilityData.unLockLevel] = abilityData;
            }
        }

        public AbilityData GetAbilityData(AbilityType type) => _abilityDataDic[type];


        public AbilityData GetAbilityData(int levelIndex)
        {
            if (_levelAbilityDataDic.TryGetValue(levelIndex + 1, out var abilityData) &&
                abilityData.isUnlocked == false)
            {
                return abilityData;
            }

            return null;
        }

        public void UnLockAbility(AbilityData abilityData)
        {
            PlayerPrefs.SetInt(AbilityUnlockPrefKey + abilityData.type, 1);
            abilityData.isUnlocked = true;
        }


        public void AddAbility(AbilityType type, int amount)
        {
            _abilityDataDic[type].count += amount;
            PlayerPrefs.SetInt(AbilityPrefKey + type, _abilityDataDic[type].count);
        }

        public void RemoveAbility(AbilityType type, int amount)
        {
            _abilityDataDic[type].count -= amount;
            PlayerPrefs.SetInt(AbilityPrefKey + type, _abilityDataDic[type].count);
        }
    }
}