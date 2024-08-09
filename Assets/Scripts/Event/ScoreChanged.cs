namespace Event
{
    public struct ScoreChanged : IEventData
    {
        public float Score;

        public ScoreChanged(float score)
        {
            Score = score;
        }
    }
}