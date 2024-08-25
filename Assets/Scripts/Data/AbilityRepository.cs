using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "AbilityRepository", menuName = "so/AbilityRepository")]
    public class AbilityRepository : ScriptableObject
    {
        [SerializeField] private List<AbilityData> abilityDataList;

        private Dictionary<AbilityType, AbilityData> _abilityDataDic = new();

        private const string AbilityPrefKey = "ABILITY_PREF_KEY";


        private void OnEnable()
        {
            foreach (var abilityData in abilityDataList)
            {
                Debug.Log("Ability repo enabled!");

                _abilityDataDic[abilityData.type] = abilityData;
                abilityData.count = PlayerPrefs.GetInt(AbilityPrefKey + abilityData.type, 0);
            }
        }

        public AbilityData GetAbilityData(AbilityType type) => _abilityDataDic[type];


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