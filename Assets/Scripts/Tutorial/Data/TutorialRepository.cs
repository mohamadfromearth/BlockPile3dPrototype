using UnityEngine;

[CreateAssetMenu(menuName = "so/TutorialRepository", fileName = "TutorialRepository")]
public class TutorialRepository : ScriptableObject
{
    private const string TutorialIndexKey = "TUTORIAL_INDEX_KEY";

    private int _tutorialIndex = 0;

    [SerializeField] private Vector3Int firstAvailablePos;
    [SerializeField] private Vector3Int secondAvailablePos;


    private void OnEnable() => _tutorialIndex = PlayerPrefs.GetInt(TutorialIndexKey, 0);


    public int TutorialIndex => _tutorialIndex;

    public Vector3Int FirstAvailablePos => firstAvailablePos;
    public Vector3Int SecondAvailablePos => secondAvailablePos;
}