using Data;
using Event;
using Zenject;


public class TutorialManager
{
    private LevelRepository _levelRepository;
    private EventChannel _channel;
    private Board _board;


    [Inject]
    private void Construct(LevelRepository levelRepository,
        EventChannel channel,
        Board board
    )
    {
        _levelRepository = levelRepository;
        _channel = channel;
        _board = board;
    }


    private void OnStartLevel()
    {
        var levelIndex = _levelRepository.LevelIndex;

        if (levelIndex == 0)
        {
            
        }
    }
}