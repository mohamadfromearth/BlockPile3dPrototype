using Core;
using TMPro;
using Tutorial.Data;

public class TutorialCommand3 : ICommand
{
    private Board _board;
    private TextMeshProUGUI _hintText;
    private TutorialRepository _tutorialRepository;

    public TutorialCommand3(Board board, TextMeshProUGUI hintText, TutorialRepository tutorialRepository, int id)
    {
        _board = board;
        _hintText = hintText;
        Id = id;
        _tutorialRepository = tutorialRepository;
    }

    public int Id { get; set; }

    public void Execute()
    {
        _hintText.text = _tutorialRepository.GetHint(_tutorialRepository.TutorialIndex);

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