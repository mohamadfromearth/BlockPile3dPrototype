using System;
using System.Collections.Generic;
using Core;
using DG.Tweening;
using Event;
using Objects.NoneValueLockBlock;
using TMPro;
using UnityEngine;
using Zenject;

public class TutorialCommand1 : ICommand
{
    public int Id { get; set; }

    private TutorialRepository _tutorialRepository;
    private GameObject _indicator;
    private GameObject _arrow;
    private TextMeshProUGUI _hintText;
    private INoneValueLockBlockFactory _lockBlockFactory;
    private Board _board;
    private Camera _camera;
    private Action<List<INoneValueLockBlock>> _lockBlockInitializationCompleted;

    public TutorialCommand1(TutorialRepository tutorialRepository, GameObject indicator, GameObject arrow,
        TextMeshProUGUI hintText, Camera camera,
        Action<List<INoneValueLockBlock>> lockBlockInitializationCompleted,
        INoneValueLockBlockFactory lockBlockFactory,
        Board board,
        EventChannel channel, int id)
    {
        _tutorialRepository = tutorialRepository;
        _indicator = indicator;
        _arrow = arrow;
        _hintText = hintText;
        _camera = camera;
        _lockBlockInitializationCompleted = lockBlockInitializationCompleted;
        _lockBlockFactory = lockBlockFactory;
        _board = board;
        Id = id;
    }

    public void Execute()
    {
        _hintText.text = _tutorialRepository.GetHint(_tutorialRepository.TutorialIndex);

        _indicator.SetActive(true);
        var targetPos = _camera.WorldToScreenPoint(_board.CellToWorld(_tutorialRepository.FirstAvailablePos));
        _indicator.transform.DOMove(targetPos, 1f).SetLoops(-1, LoopType.Yoyo);

        _arrow.SetActive(true);
        _arrow.transform.position = targetPos;
        var arrowTargetPos = targetPos;
        arrowTargetPos.y += 50f;
        _arrow.transform.DOMove(arrowTargetPos, 0.5f).SetLoops(-1, LoopType.Yoyo);

        List<INoneValueLockBlock> lockBlocks = new();

        foreach (var pos in _tutorialRepository.NoneValueLockBlockPositions)
        {
            var worldPosition = _board.CellToWorld(pos);
            var lockBlock = _lockBlockFactory.Create();
            lockBlock.SetPosition(worldPosition);
            lockBlocks.Add(lockBlock);
        }

        _lockBlockInitializationCompleted?.Invoke(lockBlocks);
    }
}