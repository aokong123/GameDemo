using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.GamePlayUi;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Score;
using CoreDomain.Scripts.Services.CommandFactory;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.ScoreChanged
{
    public class ScoreChangedCommand : BaseCommand, ICommandVoid
    {
        private ScoreChangedCommandData _commandData;
        private IScoreDataService _scoreDataService;
        private IGamePlayUiController _gamePlayUiController;
        
        public ScoreChangedCommand SetData(ScoreChangedCommandData commandData)
        {
            _commandData = commandData;
            return this;
        }
        
        public override void ResolveDependencies()
        {
            _scoreDataService = _diContainer.Resolve<IScoreDataService>();
            _gamePlayUiController = _diContainer.Resolve<IGamePlayUiController>();
        }

        public void Execute()
        {
            _scoreDataService.AddScore((int)_commandData.ScoreAdded);
            _gamePlayUiController.UpdateScore(_scoreDataService.PlayerScore);
        }
    }
}
