using Core;
using TMPro;
using UI;
using UnityEngine;

public class TutorialCommand4 : ICommand
{
    public int Id { get; set; }

    public TutorialCommand4(GameObject hintText, GameUI gameUI, int id)
    {
        _hintText = hintText;
        _gameUI = gameUI;
        Id = id;
    }

    private GameObject _hintText;
    private GameUI _gameUI;


    public void Execute()
    {
        _hintText.SetActive(false);
        _gameUI.Show();
    }
}