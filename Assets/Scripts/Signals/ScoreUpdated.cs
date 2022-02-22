namespace Asteroids.Signals
{
    /// <summary>
    /// ScoreUpdated is a signal which will be fired after the player score has changed.
    /// </summary>
    public struct ScoreUpdated
    {
        public int AddedAmount;
        public int FinalCount;

        public ScoreUpdated(int addedAmount, int finalCount)
        {
            AddedAmount = addedAmount;
            FinalCount = finalCount;
        }
    }
}