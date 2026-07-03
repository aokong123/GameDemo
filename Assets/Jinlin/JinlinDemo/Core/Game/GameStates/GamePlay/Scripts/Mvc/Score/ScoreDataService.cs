namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Score
{
    public class ScoreDataService : IScoreDataService
    {
        private int _playerCurrentScore;
        public int PlayerScore => _playerCurrentScore;

        public void ResetScore()
        {
            _playerCurrentScore = 0;
        }

        public void AddScore(int score)
        {
            _playerCurrentScore += score;
        }
    }
}