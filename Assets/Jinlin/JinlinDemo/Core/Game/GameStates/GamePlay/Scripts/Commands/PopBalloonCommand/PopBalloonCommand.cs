using System;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.ScoreChanged;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Balloon;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.ScoreFX;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services.LevelCancellationToken;
using CoreDomain.Scripts.Services.AudioService;
using CoreDomain.Scripts.Services.CommandFactory;
using CoreDomain.Scripts.Services.Logger.Base;
using CoreDomain.Scripts.Services.StateMachineService;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.PopBalloonCommand
{
    public class PopBalloonCommand : BaseCommand, ICommandVoid
    {
        private PopBalloonCommandData _commandData;
        private IBalloonsController _balloonsController;
        private IScoreFXController _scoreFXController;
        private IAudioService _audioService;
        private ICommandFactory _commandFactory;
        private IStateMachineService _stateMachineService;
        private ILevelCancellationTokenService _levelCancellationTokenService;

        public PopBalloonCommand SetData(PopBalloonCommandData commandData)
        {
            _commandData = commandData;
            return this;
        }
        public override void ResolveDependencies()
        {
            _balloonsController = _diContainer.Resolve<IBalloonsController>();
            _scoreFXController = _diContainer.Resolve<IScoreFXController>();
            _audioService = _diContainer.Resolve<IAudioService>();
            _commandFactory = _diContainer.Resolve<ICommandFactory>();
            _levelCancellationTokenService = _diContainer.Resolve<ILevelCancellationTokenService>();
        }

        public void Execute()
        {
            var popScore = _balloonsController.BalloonPopScore;
            _ = PlayPopBallonAsync();
            _scoreFXController.ShowArrowJumpBurstFX(_commandData.PopPosition, popScore);
            _commandFactory.CreateCommandVoid<ScoreChangedCommand>().SetData(new ScoreChangedCommandData(popScore)).Execute();
            _audioService.PlayAudio(AudioClipType.BalloonPop, AudioChannelType.Fx);
        }

        private async Awaitable PlayPopBallonAsync()
        {
            try
            {
                await _commandData.BalloonView.PlayPopEffect(_levelCancellationTokenService.CancellationTokenSource);
            }
            catch (OperationCanceledException)
            {
                LogService.Log("Operation PlayPopBallonAsync was cancelled");
            }
            catch (Exception e)
            {
                LogService.LogError(e.Message);
                throw;
            }
        }
    }
}
