namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Score
{
    public interface IScoreDataService
    {
        void AddScore(int score);
        int PlayerScore { get; }
        void ResetScore();
    }
}