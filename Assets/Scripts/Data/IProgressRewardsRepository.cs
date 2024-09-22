namespace Data
{
    public interface IProgressRewardsRepository
    {
        public int SpinLevelIndex { get; }
        public int SpinLevelTarget { get; }

        public void IncreaseIndex();
    }
}