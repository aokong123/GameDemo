namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services.GamePlayData
{
    public interface IGamePlayDataService
    {
        int CurrentLevelNumber { get; }
        void SetCurrentLevelNumber(int levelNumber);
    }
}
