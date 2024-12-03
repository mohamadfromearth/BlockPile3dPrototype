using System;
using UnityEngine;

namespace Data
{
    public enum FortuneWheelItemType
    {
        Coin,
        Punch,
        Refresh,
        Swap
    }


    [Serializable]
    public struct FortuneWheelItemsData
    {
        public FortuneWheelItemData[] fortuneWheelItemsDataList;
    }


    [Serializable]
    public struct FortuneWheelItemData
    {
        public int count;
        public Sprite sprite;
        public FortuneWheelItemType type;
    }


    [CreateAssetMenu(fileName = "FortuneWheelRepository", menuName = "so/FortuneWheelRepository")]
    public class FortuneWheelRepository : ScriptableObject
    {
        private const string IndexPrefKey = "INDEX_PREF_KEY";
        private const string ProgressIndexPrefKey = "PROGRESS_INDEX_PREF_KEY";
        private const string CanClaimWheelPrefKey = "CAN_CLAIM_WHEEL_PREF_KEY";

        private const int CanClaimWheelValue = 1;
        private const int CanNotClaimWheelValue = 0;


        private int _index;
        private int _progressIndex;
        private bool _canClaimWheel;


        [SerializeField] private FortuneWheelItemsData[] itemsData;
        [SerializeField] private int[] progressTargets;

        public bool CanClaimWheel
        {
            get => _canClaimWheel;
            set
            {
                _canClaimWheel = value;
                PlayerPrefs.SetInt(CanClaimWheelPrefKey, _canClaimWheel ? CanClaimWheelValue : CanNotClaimWheelValue);
            }
        }


        private void OnEnable()
        {
            _index = PlayerPrefs.GetInt(IndexPrefKey, 0);
            _progressIndex = PlayerPrefs.GetInt(ProgressIndexPrefKey, 0);
            _canClaimWheel = PlayerPrefs.GetInt(CanClaimWheelPrefKey, CanNotClaimWheelValue) == CanClaimWheelValue
                ? true
                : false;
        }


        public FortuneWheelItemData[] GetData() => itemsData[_index].fortuneWheelItemsDataList;


        public bool IsReached() => _progressIndex == progressTargets[_index];


        public float GetProgress() => _progressIndex / (float)progressTargets[_index];


        public int GetProgressIndex() => _progressIndex;

        public int GetProgressTarget() => progressTargets[_index];


        public void IncreaseProgressIndex()
        {
            _progressIndex += 1;
        }


        public void IncreaseIndex()
        {
            if (_index == itemsData.Length - 1)
            {
                _progressIndex = 0;
                PlayerPrefs.SetInt(ProgressIndexPrefKey, _progressIndex);
                return;
            }

            _index += 1;
            _progressIndex = 0;


            PlayerPrefs.SetInt(IndexPrefKey, _index);
            PlayerPrefs.SetInt(ProgressIndexPrefKey, _progressIndex);
        }


        public FortuneWheelItemData GetFortuneWheelItemData(Quaternion rotation)
        {
            int index = Mathf.RoundToInt(rotation.eulerAngles.z) / 45;
            Debug.Log("Index is " + index);
            return itemsData[_index].fortuneWheelItemsDataList[index];
        }
    }
}