using Event;
using UnityEngine;
using Utils;
using Zenject;

public class ShuffleController
{
    private Transform _shuffleButton;

    private Board _board;
    private EventChannel _channel;

    private bool _isHidden = true;
    private bool _isConsumed = false;


    [Inject]
    private void Construct(Board board, EventChannel channel)
    {
        _board = board;
        _channel = channel;

        _channel.Subscribe<UpdateBoardCompleted>(OnBoardUpdateCompleted);
        _channel.Subscribe<Won>(OnWon);
        _channel.Subscribe<Lose>(OnLose);
        _channel.Subscribe<Shuffle>(OnShuffle);
    }


    public ShuffleController(Transform shuffleButton)
    {
        _shuffleButton = shuffleButton;
    }


    private void OnBoardUpdateCompleted()
    {
        if (_board.CellsItemCount - _board.FilledCellItemsCount <= 2 && _isHidden && _isConsumed == false)
        {
            _shuffleButton.ShowPopUp();
            _isHidden = false;
        }
    }


    private void OnShuffle()
    {
        _isConsumed = true;
        Hide();
    }


    private void OnWon()
    {
        _isConsumed = false;
        if (_isHidden == false) Hide();
    }

    private void OnLose()
    {
        _isConsumed = false;
        if (_isHidden == false) Hide();
    }


    public void Hide()
    {
        _shuffleButton.HidePopUp();
        _isHidden = true;
    }
}