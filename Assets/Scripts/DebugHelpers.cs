using UI;
using UnityEngine;

public class DebugHelpers : MonoBehaviour
{
    [SerializeField] private GameUI gameUI;


    public void ShowCoinCollectionAnimation()
    {
        gameUI.ShowCoinCollection();
    }
}