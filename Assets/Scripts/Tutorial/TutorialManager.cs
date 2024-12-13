using System.Collections.Generic;
using Core;
using Data;
using Event;
using Objects.NoneValueLockBlock;
using TMPro;
using Tutorial.Data;
using UI;
using UnityEngine;
using Zenject;


public enum TutorialCommandType
{
    Command1,
    Command2,
    Command3,
    Command4
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
    private TutorialCommand4 _command4;
    private TutorialCommand1Factory _command1Factory;
    private BlockContainerSelectionBar _selectionBar;

    private List<INoneValueLockBlock> _lockBlocks;

    [SerializeField] private GameObject indicator;
    [SerializeField] private GameObject arrow;
    [SerializeField] private Camera camera;
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private GameUI gameUI;

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
        if (levelRepository.LevelIndex != 0 || tutorialRepository.IsTutorialAvailable == false)
        {
            tutorialRepository.TutorialIndex = (int)TutorialCommandType.Command4;
            return;
        }


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
            tutorialRepository,
            (int)TutorialCommandType.Command3
        );

        _command4 = new TutorialCommand4(hintText.gameObject, gameUI, (int)TutorialCommandType.Command4);


        _tutorialCommands = new List<ICommand>()
        {
            _command1,
            _command2,
            _command3,
            _command4
        };
    }


    public void OnStartLevel()
    {
        var levelIndex = levelRepository.LevelIndex;

        
        
        if (levelIndex == 0 && tutorialRepository.IsTutorialAvailable)
        {
            gameUI.Hide();
            var indicatorStartPos = camera.WorldToScreenPoint(_selectionBar.ContainerPositionsList[2].position);
            indicator.transform.position = indicatorStartPos;
            _tutorialCommands[tutorialRepository.TutorialIndex].Execute();
        }
    }

    public void OnBlockContainerPointerDown(int index)
    {
        if (levelRepository.LevelIndex != 0 || tutorialRepository.IsTutorialAvailable == false) return;
        Debug.Log("Container index is : " + index);
        _latestSelectionBarIndex = index;
        indicator.SetActive(false);
        arrow.SetActive(false);
    }


    public void OnBlockContainerPointerUp()
    {
        if (levelRepository.LevelIndex != 0 || tutorialRepository.IsTutorialAvailable == false) return;

        if (tutorialRepository.TutorialIndex < (int)TutorialCommandType.Command3)
        {
            indicator.SetActive(true);
            arrow.SetActive(true);
        }
    }


    public void OnPlacedBlockContainer()
    {
        if (levelRepository.LevelIndex != 0 || tutorialRepository.IsTutorialAvailable == false) return;

        if (tutorialRepository.TutorialIndex == (int)TutorialCommandType.Command4)
        {
            tutorialRepository.IsTutorialAvailable = false;
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