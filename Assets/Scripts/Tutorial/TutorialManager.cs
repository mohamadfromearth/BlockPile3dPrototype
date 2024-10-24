using System.Collections.Generic;
using Core;
using Data;
using Event;
using Objects.NoneValueLockBlock;
using TMPro;
using UnityEngine;
using Zenject;


public enum TutorialCommandType
{
    Command1,
    Command2,
    Command3
}


public class TutorialCommand3 : ICommand
{
    private Board _board;
    private TextMeshProUGUI _hintText;

    public TutorialCommand3(Board board, TextMeshProUGUI hintText, int id)
    {
        _board = board;
        _hintText = hintText;
        Id = id;
    }

    public int Id { get; set; }

    public void Execute()
    {
        foreach (var keyValuePair in _board.Cells)
        {
            var lockBlock = keyValuePair.Value.NoneValueLockBlock;

            if (lockBlock != null)
            {
                _board.AddNonValueLockBlock(null, _board.WorldToCell(lockBlock.GetPosition()));
                lockBlock.Destroy();
            }
        }
    }
}


public class TutorialManager : MonoBehaviour
{
    [SerializeField] private LevelRepository levelRepository;
    private EventChannel _channel;
    private Board _board;
    [SerializeField] private TutorialRepository tutorialRepository;
    private TutorialCommand1 _command1;
    private TutorialCommand2 _command2;
    private TutorialCommand3 _command3;
    private TutorialCommand1Factory _command1Factory;
    private BlockContainerSelectionBar _selectionBar;

    private List<INoneValueLockBlock> _lockBlocks;

    [SerializeField] private GameObject indicator;
    [SerializeField] private GameObject arrow;
    [SerializeField] private Camera camera;
    [SerializeField] private TextMeshProUGUI hintText;

    public int Index => tutorialRepository.TutorialIndex;

    private List<ICommand> _tutorialCommands;

    private int _latestSelectionBarIndex;


    [Inject]
    private void Construct(
        EventChannel channel,
        Board board,
        BlockContainerSelectionBar selectionBar,
        TutorialCommand1Factory command1Factory
    )
    {
        _channel = channel;
        _board = board;
        _selectionBar = selectionBar;
        _command1Factory = command1Factory;
        _selectionBar = selectionBar;


        _command1 = _command1Factory.Create((int)TutorialCommandType.Command1,
            lockBlocks => { _lockBlocks = lockBlocks; });

        _command2 = new TutorialCommand2(
            indicator,
            arrow,
            _board,
            hintText,
            tutorialRepository,
            camera,
            (int)TutorialCommandType.Command2
        );

        _command3 = new TutorialCommand3(
            _board,
            hintText,
            (int)TutorialCommandType.Command3
        );


        _tutorialCommands = new List<ICommand>()
        {
            _command1,
            _command2,
            _command3
        };
    }


    public void OnStartLevel()
    {
        var levelIndex = levelRepository.LevelIndex;

        if (levelIndex == 0)
        {
            var indicatorStartPos = camera.WorldToScreenPoint(_selectionBar.ContainerPositionsList[2].position);
            indicator.transform.position = indicatorStartPos;
            _tutorialCommands[tutorialRepository.TutorialIndex].Execute();
        }
    }

    public void OnBlockContainerPointerDown(int index)
    {
        _latestSelectionBarIndex = index;
        indicator.SetActive(false);
        arrow.SetActive(false);
    }


    public void OnBlockContainerPointerUp()
    {
        indicator.SetActive(true);
        arrow.SetActive(true);
    }


    public void OnPlacedBlockContainer()
    {
        if (tutorialRepository.TutorialIndex == (int)TutorialCommandType.Command3)
        {
            return;
        }


        if (tutorialRepository.TutorialIndex == (int)TutorialCommandType.Command1)
        {
            var containerIndex = _latestSelectionBarIndex == 2 ? 1 : 2;
            indicator.transform.position =
                camera.WorldToScreenPoint(_selectionBar.ContainerPositionsList[containerIndex].position);
        }

        tutorialRepository.IncreaseIndex();
        _tutorialCommands[tutorialRepository.TutorialIndex].Execute();
    }
}