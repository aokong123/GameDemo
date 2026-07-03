using System.Threading;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.StartLevel;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Arrow;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.GameInputActions;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.GamePlayUi;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Score;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services.GamePlayData;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services.LevelCancellationToken;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services.PostProcessing;
using CoreDomain.GameDomain.Scripts.Services.Levels;
using CoreDomain.Scripts.Services.CommandFactory;
using CoreDomain.Scripts.Utils;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands
{
    public class GameOverCommand : BaseCommand, ICommandAsync
    {
        private IGamePlayUiController _gamePlayUiController;
        private IGameInputActionsController _gameInputActionsController;
        private IArrowController _arrowController;
        private IGamePlayDataService _gamePlayDataService;
        private ICommandFactory _commandFactory;
        private ILevelsDataService _levelsDataService;
        private IScoreDataService _scoreDataService;
        private IPostProcessingService _postProcessingService;
        
        private bool _shouldShowScore;

        public GameOverCommand SetShouldShowScore(bool shouldShowScore)
        {
            _shouldShowScore = shouldShowScore;
            return this;
        }
        
        public override void ResolveDependencies()
        {
            _gamePlayUiController = _diContainer.Resolve<IGamePlayUiController>();
            _gameInputActionsController = _diContainer.Resolve<IGameInputActionsController>();
            _commandFactory = _diContainer.Resolve<ICommandFactory>();
            _arrowController = _diContainer.Resolve<IArrowController>();
            _gamePlayDataService = _diContainer.Resolve<IGamePlayDataService>();
            _levelsDataService = _diContainer.Resolve<ILevelsDataService>();
            _scoreDataService = _diContainer.Resolve<IScoreDataService>();
            _postProcessingService = _diContainer.Resolve<IPostProcessingService>();
        }
        
        public async Awaitable Execute(CancellationTokenSource cancellationTokenSource)
        {
            _gamePlayUiController.ShowGameOverPanel(_scoreDataService.PlayerScore, _levelsDataService.GetLevelData(_gamePlayDataService.CurrentLevelNumber).ScoreGoal, _shouldShowScore);
            _gameInputActionsController.UnregisterAllInputListeners();
            _arrowController.DisableCallbacks();
            _postProcessingService.SetAreAllPostProcessingActive(true);

            await _gameInputActionsController.WaitForAnyKeyPressed(cancellationTokenSource, true);
            _commandFactory.CreateCommandVoid<DisposeLevelCommand>().Execute();
            await _commandFactory.CreateCommandAsync<LoadLevelCommand>().SetEnterData(new LoadLevelCommandData(_gamePlayDataService.CurrentLevelNumber)).Execute(cancellationTokenSource);
            await _commandFactory.CreateCommandAsync<StartLevelCommand>().Execute(cancellationTokenSource);
        }
    }
}
