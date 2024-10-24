using Core;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TutorialCommand2 : ICommand
{
    public int Id { get; set; }

    private GameObject _indicator;
    private GameObject _arrow;
    private Board _board;
    private TextMeshProUGUI _hintText;
    private TutorialRepository _tutorialRepository;
    private Camera _camera;


    public TutorialCommand2(GameObject indicator, GameObject arrow, Board board, TextMeshProUGUI hintText,
        TutorialRepository tutorialRepository, Camera camera, int id)
    {
        _indicator = indicator;
        _arrow = arrow;
        _board = board;
        _hintText = hintText;
        _tutorialRepository = tutorialRepository;
        _camera = camera;
        Id = id;
    }


    public void Execute()
    {
        _hintText.text = _tutorialRepository.GetHint(_tutorialRepository.TutorialIndex);
        _indicator.SetActive(true);
        _indicator.transform.DOKill();
        _arrow.transform.DOKill();
        var targetPos = _camera.WorldToScreenPoint(_board.CellToWorld(_tutorialRepository.SecondAvailablePos));
        _indicator.transform.DOMove(targetPos, 1f).SetLoops(-1, LoopType.Yoyo);

        _arrow.SetActive(true);
        _arrow.transform.position = targetPos;
        var arrowTargetPos = targetPos;
        arrowTargetPos.y += 50f;
        _arrow.transform.DOMove(arrowTargetPos, 0.5f).SetLoops(-1, LoopType.Yoyo);


        var cell = _board.GetCell(_tutorialRepository.SecondAvailablePos);
        cell.NoneValueLockBlock.Destroy();
        _board.AddNonValueLockBlock(null, _tutorialRepository.SecondAvailablePos);
    }
}