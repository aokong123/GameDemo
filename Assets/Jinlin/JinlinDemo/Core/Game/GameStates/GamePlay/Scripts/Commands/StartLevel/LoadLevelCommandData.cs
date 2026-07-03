namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.StartLevel
{
    public class LoadLevelCommandData
    {
        public readonly int LevelNumber;

        public LoadLevelCommandData(int levelNumber)
        {
            LevelNumber = levelNumber;
        }
    }
}
