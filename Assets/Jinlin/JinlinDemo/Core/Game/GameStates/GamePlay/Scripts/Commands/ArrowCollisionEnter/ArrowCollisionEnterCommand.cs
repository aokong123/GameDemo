using System;
using System.Threading;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Arrow;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Balloon;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Bullseye;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Lava;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services.LevelCancellationToken;
using CoreDomain.Scripts.Services.AudioService;
using CoreDomain.Scripts.Services.CommandFactory;
using CoreDomain.Scripts.Services.Logger.Base;
using CoreDomain.Scripts.Services.StateMachineService;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.ArrowCollisionEnter
{
    public class ArrowCollisionEnterCommand : BaseCommand, ICommandVoid
    {
        private IArrowController _arrowController;
        private IAudioService _audioService;
        private ArrowCollisionEnterCommandData _enterCommandData;
        private ICommandFactory _commandFactory;
        private ILevelCancellationTokenService _levelCancellationTokenService;

        public ArrowCollisionEnterCommand SetEnterData(ArrowCollisionEnterCommandData enterCommandData)
        {
            _enterCommandData = enterCommandData;
            return this;
        }

        public override void ResolveDependencies()
        {
            _arrowController = _diContainer.Resolve<IArrowController>();
            _audioService = _diContainer.Resolve<IAudioService>();
            _commandFactory = _diContainer.Resolve<ICommandFactory>();
            _levelCancellationTokenService = _diContainer.Resolve<ILevelCancellationTokenService>();
        }

        public void Execute()
        {
            var collision = _enterCommandData.Collision;
            _arrowController.SetIsThrusterEnabled(false);
        
            var isCollisionPopable = _enterCommandData.Collision.transform.GetComponent<PopableView>() != null;
            if (isCollisionPopable)
            {
                return;
            }
        
            var didStab = false;
            if (collision.contacts.Length > 0)
            {
                didStab = _arrowController.TryStabContactPoint(collision.contacts[0]);
            }

            if (didStab)
            {
                var isCollisionWithBullseye = collision.transform.GetComponent<BullseyeView>() != null;
                if (isCollisionWithBullseye)
                {
                    _ = ExecuteStabbedBullseyeCommand();
                }
            }
            else
            {
                _audioService.PlayAudio(AudioClipType.Hit, AudioChannelType.Fx);
            }
            
            var isCollisionWithLava = collision.transform.GetComponent<LavaView>() != null;
            if (isCollisionWithLava)
            {
                _ = ExecuteGameOverCommand();
            }
        }

        private async Awaitable ExecuteStabbedBullseyeCommand()
        {
            try
            {
                var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(Application.exitCancellationToken);
                await _commandFactory.CreateCommandAsync<StabbedBullseyeCommand>().Execute(cancellationTokenSource);
            }
            catch (OperationCanceledException)
            {
                LogService.Log("Operation StabbedBullseyeCommand was cancelled");
            }
            catch (Exception e)
            {
                LogService.LogError(e.Message);
                throw;
            }
        }
        
        private async Awaitable ExecuteGameOverCommand()
        {
            try
            {
                await _commandFactory.CreateCommandAsync<GameOverCommand>().Execute(_levelCancellationTokenService.CancellationTokenSource);
            }
            catch (OperationCanceledException)
            {
                LogService.Log("Operation GameOverCommand was cancelled");
            }
            catch (Exception e)
            {
                LogService.LogError(e.Message);
                throw;
            }
        } 
    }
}
