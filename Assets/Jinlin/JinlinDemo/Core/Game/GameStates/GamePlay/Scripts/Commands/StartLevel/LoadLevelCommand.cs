using System.Threading;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Arrow;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Balloon;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Bubble;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.GamePlayUi;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Level;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services.GamePlayData;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services.LevelCancellationToken;
using CoreDomain.Scripts.Services.CommandFactory;
using CoreDomain.Scripts.Services.StateMachineService;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.StartLevel
{
    public class LoadLevelCommand : BaseCommand, ICommandAsync
    {
        private IGamePlayUiController _gamePlayUiController;
        private ILevelTrackController _levelTrackController;
        private IArrowController _arrowController;
        private IBalloonsController _balloonsController;
        private IBubblesController _bubblesController;
        private IGamePlayDataService _gamePlayDataService;
        private IStateMachineService _stateMachineService;
        private ILevelCancellationTokenService _levelCancellationTokenService;
        
        private LoadLevelCommandData _commandData;

        public LoadLevelCommand SetEnterData(LoadLevelCommandData commandData)
        {
            _commandData = commandData;
            return this;
        }

        public override void ResolveDependencies()
        {
            _gamePlayUiController = _diContainer.Resolve<IGamePlayUiController>();
            _levelTrackController = _diContainer.Resolve<ILevelTrackController>();
            _arrowController = _diContainer.Resolve<IArrowController>();
            _balloonsController = _diContainer.Resolve<IBalloonsController>();
            _bubblesController = _diContainer.Resolve<IBubblesController>();
            _gamePlayDataService = _diContainer.Resolve<IGamePlayDataService>();
            _levelCancellationTokenService = _diContainer.Resolve<ILevelCancellationTokenService>();
        }

        public async Awaitable Execute(CancellationTokenSource cancellationTokenSource)
        {
            _levelCancellationTokenService.InitCancellationToken();
            int levelNumber = _commandData.LevelNumber;
            _gamePlayDataService.SetCurrentLevelNumber(levelNumber);
            _gamePlayUiController.SwitchToBeforeGameView(levelNumber);
            await CreateLevelTrack(levelNumber, cancellationTokenSource);
            _arrowController.CreateArrow();
            _arrowController.FreezeMovement();
        }

        private async Awaitable CreateLevelTrack(int levelNumber, CancellationTokenSource cancellationTokenSource)
        {
            await _levelTrackController.CreateLevelTrack(levelNumber, cancellationTokenSource);
            _balloonsController.SetupBalloons(_levelTrackController.CurrentLevelTrackView.BalloonViews);
            _bubblesController.SetupBubbles(_levelTrackController.CurrentLevelTrackView.BubbleViews);
        }
    }
}
