using System.Threading;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.StartLevel;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Arrow;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.GameInputActions;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.GamePlayUi;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Score;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services.GamePlayData;
using CoreDomain.GameDomain.Scripts.Services.Levels;
using CoreDomain.Scripts.Mvc.WorldCamera;
using CoreDomain.Scripts.Services.CommandFactory;
using CoreDomain.Scripts.Utils;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands
{
    public class StabbedBullseyeCommand : BaseCommand, ICommandAsync
    {
        private IGamePlayUiController _gamePlayUiController;
        private IScoreDataService _scoreDataService;
        private IGameInputActionsController _gameInputActionsController;
        private ILevelsDataService _levelsDataService;
        private IArrowController _arrowController;
        private IGamePlayDataService _gamePlayDataService;
        private ICommandFactory _commandFactory;
        private IWorldCameraController _worldCameraController;

        public override void ResolveDependencies()
        {
            _gamePlayUiController = _diContainer.Resolve<IGamePlayUiController>();
            _scoreDataService = _diContainer.Resolve<IScoreDataService>();
            _gameInputActionsController = _diContainer.Resolve<IGameInputActionsController>();
            _levelsDataService = _diContainer.Resolve<ILevelsDataService>();
            _arrowController = _diContainer.Resolve<IArrowController>();
            _gamePlayDataService = _diContainer.Resolve<IGamePlayDataService>();
            _commandFactory = _diContainer.Resolve<ICommandFactory>();
            _worldCameraController = _diContainer.Resolve<IWorldCameraController>();
        }

        public async Awaitable Execute(CancellationTokenSource cancellationTokenSource)
        {
            int scoreGoal = _levelsDataService.GetLevelData(_gamePlayDataService.CurrentLevelNumber).ScoreGoal;
            bool didAchievedLessScoreThanGoal = scoreGoal > _scoreDataService.PlayerScore;
            if (didAchievedLessScoreThanGoal)
            {
                await _commandFactory.CreateCommandAsync<GameOverCommand>().SetShouldShowScore(true).Execute(cancellationTokenSource);
                return;
            }
            
            _gamePlayUiController.ShowWinPanel(_scoreDataService.PlayerScore, scoreGoal);
            _gameInputActionsController.UnregisterAllInputListeners();
            _arrowController.DisableCallbacks();
            var didReachLastLevel = TryGetNextLevelNumberOrStayOnCurrent(out var nextLevelNumber); 
            TryUpdateMaxLevelReached(nextLevelNumber);
        
            await _gameInputActionsController.WaitForAnyKeyPressed(cancellationTokenSource, true);
        
            _commandFactory.CreateCommandVoid<DisposeLevelCommand>().SetShouldReleaseAssetsFromMemory(true).Execute();
            await _commandFactory.CreateCommandAsync<LoadLevelCommand>().SetEnterData(new LoadLevelCommandData(nextLevelNumber)).Execute(cancellationTokenSource);

            if (!didReachLastLevel)
            {
                await _worldCameraController.DoLockOnTargetAnimation(_arrowController.ArrowTransform, cancellationTokenSource);
            }

            await _commandFactory.CreateCommandAsync<StartLevelCommand>().Execute(cancellationTokenSource);
        }

        private bool TryGetNextLevelNumberOrStayOnCurrent(out int nextLevelNumber)
        {
            nextLevelNumber = _gamePlayDataService.CurrentLevelNumber + 1;
            var didReachLastLevel = nextLevelNumber > _levelsDataService.GetLevelsAmount();
            
            if (didReachLastLevel)
            {
                nextLevelNumber--;
            }
            
            return didReachLastLevel;
        }

        private void TryUpdateMaxLevelReached(int maxLevelReached)
        {
            if (maxLevelReached > _levelsDataService.MaxLevelNumberReached)
            {
                _levelsDataService.SetLastSavedLevel(maxLevelReached);
            }
        }
    }
}
