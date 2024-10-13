using UnityEngine;

namespace Data
{
    public enum AbilityType
    {
        Refresh,
        Swap,
        Punch
    }


    [CreateAssetMenu(fileName = "AbilityData", menuName = "so/AbilityData")]
    public class AbilityData : ScriptableObject
    {
        public AbilityType type;
        public new string name;
        public Sprite image;
        public string description;
        public int cost;
        public int count;
        public int unLockLevel;
        public bool isUnlocked;
    }
}