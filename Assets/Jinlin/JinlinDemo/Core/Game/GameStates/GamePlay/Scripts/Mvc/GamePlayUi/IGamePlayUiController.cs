
namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.GamePlayUi
{
    public interface IGamePlayUiController
    {
        void InitEntryPoint();
        void UpdateScore(int newScore);
        void InitExitPoint();
        void SwitchToInGameView(int score, int scoreGoal);
        void SwitchToBeforeGameView(int currentLevel);
        void ShowGameOverPanel(int score, int scoreGoal, bool shouldShowScore);
        void ShowWinPanel(int scoreAchieved, int scoreGoal);
    }
}