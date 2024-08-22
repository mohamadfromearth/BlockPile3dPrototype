using System;
using Data;

namespace Managers
{
    public static class GameManagerExt
    {
        public static GameStateType GameStateType(this AbilityType type)
        {
            switch (type)
            {
                case AbilityType.Refresh:
                    return Managers.GameStateType.Default;
                    break;
                case AbilityType.Swap:
                    return Managers.GameStateType.Swap;
                    break;
                case AbilityType.Punch:
                    return Managers.GameStateType.Punch;
                    break;
                default:
                    return Managers.GameStateType.Default;
            }
        }
    }
}