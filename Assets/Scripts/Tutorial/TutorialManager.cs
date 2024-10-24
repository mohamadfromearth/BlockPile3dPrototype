using System.Collections.Generic;
using Core;
using Data;
using Event;
using Objects.NoneValueLockBlock;
using UnityEngine;
using Zenject;


public enum TutorialCommandType
{
    Command1
}


public class TutorialManager : MonoBehaviour
{
    [SerializeField] private LevelRepository levelRepository;
    private EventChannel _channel;
    private Board _board;
    [SerializeField] private TutorialRepository tutorialRepository;
    private TutorialCommand1 _command1;
    private TutorialCommand1Factory _command1Factory;
    private BlockContainerSelectionBar _selectionBar;

    private List<INoneValueLockBlock> _lockBlocks;

    [SerializeField] private GameObject indicator;
    [SerializeField] private Camera camera;


    private List<ICommand> _tutorialCommands;


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


        _tutorialCommands = new List<ICommand>()
        {
            _command1
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
}