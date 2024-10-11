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


    [Inject]
    private void Construct(Board board, EventChannel channel)
    {
        _board = board;
        _channel = channel;

        _channel.Subscribe<UpdateBoardCompleted>(OnBoardUpdateCompleted);
        _channel.Subscribe<Won>(Hide);
        _channel.Subscribe<Lose>(Hide);
    }


    public ShuffleController(Transform shuffleButton)
    {
        _shuffleButton = shuffleButton;
    }


    private void OnBoardUpdateCompleted()
    {
        if (_board.CellsItemCount - _board.FilledCellItemsCount <= 2 && _isHidden)
        {
            _shuffleButton.ShowPopUp();
            _isHidden = false;
        }
    }


    private void Hide()
    {
        _shuffleButton.transform.localScale = Vector3.zero;
        _isHidden = true;
    }
}