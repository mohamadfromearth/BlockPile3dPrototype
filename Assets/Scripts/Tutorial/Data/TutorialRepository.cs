using System.Collections.Generic;
using UnityEngine;

namespace Tutorial.Data
{
    [CreateAssetMenu(menuName = "so/TutorialRepository", fileName = "TutorialRepository")]
    public class TutorialRepository : ScriptableObject
    {
        private const string TutorialIndexKey = "TUTORIAL_INDEX_KEY";
        private const string IsTutorialAvailableKey = "IS_TUTORIAL_AVAILABLE";
        private const int TutorialAvailableValue = 1;

        private int _tutorialIndex = 0;


        [SerializeField] private Vector3Int firstAvailablePos;
        [SerializeField] private Vector3Int secondAvailablePos;

        [SerializeField] private Vector3Int[] noneValueLockBlocksPositions;
        [SerializeField] private string[] tutorialHintTexts;
        [SerializeField] private List<string> blocksColors;
        [SerializeField] private List<int> blocksCount;

        private bool _isTutorialAvailable;


        private void OnEnable()
        {
            _tutorialIndex = 0;
            _isTutorialAvailable = PlayerPrefs.GetInt(IsTutorialAvailableKey, TutorialAvailableValue) ==
                                   TutorialAvailableValue;
        }


        public int TutorialIndex
        {
            get => _tutorialIndex;
            set => _tutorialIndex = value;
        }

        public Vector3Int FirstAvailablePos => firstAvailablePos;
        public Vector3Int SecondAvailablePos => secondAvailablePos;

        public Vector3Int[] NoneValueLockBlockPositions => noneValueLockBlocksPositions;

        public void IncreaseIndex()
        {
            _tutorialIndex += 1;
        }


        public bool IsTutorialAvailable
        {
            get => _isTutorialAvailable;
            set
            {
                _isTutorialAvailable = value;
                var tutorialAvailableValue = _isTutorialAvailable ? TutorialAvailableValue : 0;
                PlayerPrefs.SetInt(IsTutorialAvailableKey, tutorialAvailableValue);
            }
        }

        public string GetHint(int index) => tutorialHintTexts[index];


        public List<string> BlocksColors => blocksColors;

        public List<int> BlocksCount => blocksCount;
    }
}