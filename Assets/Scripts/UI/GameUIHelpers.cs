using System;
using Data;
using UnityEngine;

namespace UI
{
    public class GameUIHelpers : MonoBehaviour
    {
        [SerializeField] private Transform hammerButton;
        [SerializeField] private Transform swapButton;
        [SerializeField] private Transform refreshButton;


        public Vector3 GetBoosterButtonPosition(AbilityType type)
        {
            switch (type)
            {
                case AbilityType.Refresh:
                    return refreshButton.position;
                case AbilityType.Swap:
                    return swapButton.position;
                case AbilityType.Punch:
                    return hammerButton.position;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}