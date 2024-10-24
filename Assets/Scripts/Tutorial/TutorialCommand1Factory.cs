using System;
using System.Collections.Generic;
using Event;
using Objects.NoneValueLockBlock;
using TMPro;
using UnityEngine;
using Zenject;

public class TutorialCommand1Factory
{
    private TutorialRepository _tutorialRepository;
    private GameObject _indicator;
    private GameObject _arrow;
    private TextMeshProUGUI _hintText;
    [Inject] private INoneValueLockBlockFactory _lockBlockFactory;
    [Inject] private Board _board;
    [Inject] private EventChannel _channel;

    public TutorialCommand1Factory(TutorialRepository tutorialRepository, GameObject indicator, GameObject arrow,
        TextMeshProUGUI hintText)
    {
        _tutorialRepository = tutorialRepository;
        _indicator = indicator;
        _arrow = arrow;
        _hintText = hintText;
    }


    public TutorialCommand1 Create(int id, Action<List<INoneValueLockBlock>> lockBlocks)
    {
        return new TutorialCommand1(
            _tutorialRepository,
            _indicator,
            _arrow,
            _hintText,
            Camera.main,
            lockBlocks,
            _lockBlockFactory,
            _board,
            _channel,
            id
        );
    }
}