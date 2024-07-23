using Zenject;

namespace Data
{
    public class GenerativeLevelRepository : ILevelRepository
    {
        private LevelGenerator _levelGenerator;

        private LevelData _levelData;


        [Inject]
        private void Construct(LevelGenerator levelGenerator)
        {
            _levelGenerator = levelGenerator;
            _levelData = _levelGenerator.Generate();
        }


        public LevelData GetLevelData() => _levelData;

        public void NextLevel()
        {
            _levelData = _levelGenerator.Generate();
        }
    }
}