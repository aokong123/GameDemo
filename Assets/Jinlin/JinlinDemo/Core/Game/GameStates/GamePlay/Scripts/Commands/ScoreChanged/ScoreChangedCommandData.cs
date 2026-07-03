namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.ScoreChanged
{
    public class ScoreChangedCommandData
    {
        public float ScoreAdded;

        public ScoreChangedCommandData(float scoreAdded)
        {
            ScoreAdded = scoreAdded;
        }
    }
}
