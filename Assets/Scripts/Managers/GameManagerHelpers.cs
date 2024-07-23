using Data;
using UnityEngine;
using Zenject;

public class GameManagerHelpers : MonoBehaviour
{
    [Inject] private ILevelRepository _levelRepository;

    public string GetTargetScoreString(float currentScore)
    {
        var levelData = _levelRepository.GetLevelData();

        return currentScore + "/" + levelData.targetScore;
    }
}