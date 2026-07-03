using System.Threading;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.StartLevel;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Arrow;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.GameInputActions;
using CoreDomain.GameDomain.Scripts.States.GamePlayState;
using CoreDomain.Scripts.Mvc.WorldCamera;
using CoreDomain.Scripts.Services.AudioService;
using CoreDomain.Scripts.Services.CommandFactory;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.EntryPoint
{
    public class StartGamePlayStateCommand : BaseCommand, ICommandAsync
    {
        private ICommandFactory _commandFactory;
        private IWorldCameraController _worldCameraController;
        private IArrowController _arrowController;
        private IAudioService _audioService;
        private IGameInputActionsController _gameInputActionsController;

        private GamePlayInitatorEnterData _enterData; // kept this for future use

        public StartGamePlayStateCommand SetEnterData(GamePlayInitatorEnterData enterData)
        {
            _enterData = enterData;
            return this;
        }
        
        public override void ResolveDependencies()
        {
            _commandFactory = _diContainer.Resolve<ICommandFactory>();
            _worldCameraController = _diContainer.Resolve<IWorldCameraController>();
            _arrowController = _diContainer.Resolve<IArrowController>();
            _audioService = _diContainer.Resolve<IAudioService>();
            _gameInputActionsController = _diContainer.Resolve<IGameInputActionsController>();
        }

        public async Awaitable Execute(CancellationTokenSource cancellationTokenSource)
        {
            _audioService.PlayAudio(AudioClipType.GamePlayBGMusic, AudioChannelType.Master, AudioPlayType.Loop);
            _gameInputActionsController.EnableInputs();
            await _worldCameraController.DoLockOnTargetAnimation(_arrowController.ArrowTransform, cancellationTokenSource);
            await _commandFactory.CreateCommandAsync<StartLevelCommand>().Execute(cancellationTokenSource);
        }
    }
}