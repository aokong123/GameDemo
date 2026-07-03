using System.Threading;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Audio;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.StartLevel;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Arrow;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.GamePlayUi;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.ScoreFX;
using CoreDomain.GameDomain.Scripts.States.GamePlayState;
using CoreDomain.Scripts.Mvc.WorldCamera;
using CoreDomain.Scripts.Services.AudioService;
using CoreDomain.Scripts.Services.CommandFactory;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.EntryPoint
{
    public class LoadGamePlayStateCommand : BaseCommand, ICommandAsync
    {
        private IGamePlayUiController _gamePlayUiController;
        private IAudioService _audioService;
        private IArrowController _arrowController;
        private GamePlayAudioClipsScriptableObject _gamePlayAudioClipsScriptableObject;
        private ICommandFactory _commandFactory;
        private IScoreFXController _scoreFXController;
        private IWorldCameraController _worldCameraController;

        private GamePlayInitatorEnterData _enterData;

        public LoadGamePlayStateCommand SetEnterData(GamePlayInitatorEnterData enterData)
        {
            _enterData = enterData;
            return this;
        }
        
        public override void ResolveDependencies()
        {
            _audioService = _diContainer.Resolve<IAudioService>();
            _gamePlayAudioClipsScriptableObject = _diContainer.Resolve<GamePlayAudioClipsScriptableObject>();
            _gamePlayUiController = _diContainer.Resolve<IGamePlayUiController>();
            _arrowController = _diContainer.Resolve<IArrowController>();
            _commandFactory = _diContainer.Resolve<ICommandFactory>();
            _scoreFXController = _diContainer.Resolve<IScoreFXController>();
        }
        
        public async Awaitable Execute(CancellationTokenSource cancellationTokenSource)
        {
            _arrowController.InitEntryPoint();
            _scoreFXController.InitEntryPoint();
            _audioService.AddAudioClips(_gamePlayAudioClipsScriptableObject);
            _gamePlayUiController.InitEntryPoint();
            await _commandFactory.CreateCommandAsync<LoadLevelCommand>().SetEnterData(new LoadLevelCommandData(_enterData.LevelNumberToEnter)).Execute(cancellationTokenSource);
        }
    }
}