using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private FortuneWheelUI fortuneWheelUI;


    public void OnPlayClick()
    {
        SceneManager.LoadScene("Game");
    }
}