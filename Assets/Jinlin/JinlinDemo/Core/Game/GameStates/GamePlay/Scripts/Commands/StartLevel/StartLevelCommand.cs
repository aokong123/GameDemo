using System.Threading;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Arrow;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.GameInputActions;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.GamePlayUi;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Score;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services.GamePlayData;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services.LevelCancellationToken;
using CoreDomain.GameDomain.Scripts.Services.Levels;
using CoreDomain.Scripts.Mvc.WorldCamera;
using CoreDomain.Scripts.Services.CommandFactory;
using CoreDomain.Scripts.Services.StateMachineService;
using CoreDomain.Scripts.Utils;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.StartLevel
{
    public class StartLevelCommand :  BaseCommand, ICommandAsync
    {
        private LoadLevelCommandData _commandData;
        private IScoreDataService _scoreDataService;
        private IGamePlayUiController _gamePlayUiController;
        private IArrowController _arrowController;
        private IGameInputActionsController _gameInputActionsController;
        private IGamePlayDataService _gamePlayDataService;
        private IWorldCameraController _iWorldCameraController;
        private IStateMachineService _stateMachineService;
        private ILevelCancellationTokenService _levelCancellationTokenService;
        private ILevelsDataService _levelsDataService;
        
        public override void ResolveDependencies()
        {
            _scoreDataService = _diContainer.Resolve<IScoreDataService>();
            _gamePlayUiController = _diContainer.Resolve<IGamePlayUiController>();
            _arrowController = _diContainer.Resolve<IArrowController>();
            _gameInputActionsController = _diContainer.Resolve<IGameInputActionsController>();
            _gamePlayDataService = _diContainer.Resolve<IGamePlayDataService>();
            _iWorldCameraController = _diContainer.Resolve<IWorldCameraController>();
            _levelCancellationTokenService = _diContainer.Resolve<ILevelCancellationTokenService>();
            _levelsDataService = _diContainer.Resolve<ILevelsDataService>();
        }

        public async Awaitable Execute(CancellationTokenSource cancellationTokenSource)
        {
            _gameInputActionsController.RegisterAllInputListeners();
            _iWorldCameraController.StartFollowTarget(_arrowController.ArrowTransform);
            _arrowController.RegisterListeners();
            _arrowController.UnfreezeMovement();
            await AwaitableUtils.WaitUntil(() => _gameInputActionsController.IsJumpInputPressed(), _levelCancellationTokenService.CancellationTokenSource.Token);
            _gamePlayUiController.SwitchToInGameView(_scoreDataService.PlayerScore, _levelsDataService.GetLevelData(_gamePlayDataService.CurrentLevelNumber).ScoreGoal);
        }
    }
}
