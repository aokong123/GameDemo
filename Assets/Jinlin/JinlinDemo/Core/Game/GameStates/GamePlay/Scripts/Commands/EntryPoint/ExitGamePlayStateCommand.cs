using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Audio;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.GameInputActions;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.GamePlayUi;
using CoreDomain.Scripts.Services.AudioService;
using CoreDomain.Scripts.Services.CommandFactory;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.EntryPoint
{
    public class ExitGamePlayStateCommand : BaseCommand, ICommandVoid
    {
        private IGameInputActionsController _gameInputActionsController;
        private IGamePlayUiController _gamePlayUiController;
        private ICommandFactory _commandFactory;
        private GamePlayAudioClipsScriptableObject _gamePlayAudioClipsScriptableObject;
        private IAudioService _audioService;

        public override void ResolveDependencies()
        {
            _gameInputActionsController = _diContainer.Resolve<IGameInputActionsController>();
            _gamePlayUiController = _diContainer.Resolve<IGamePlayUiController>();
            _commandFactory = _diContainer.Resolve<ICommandFactory>();
            _audioService = _diContainer.Resolve<IAudioService>();
            _gamePlayAudioClipsScriptableObject = _diContainer.Resolve<GamePlayAudioClipsScriptableObject>();
        }

        public void Execute()
        {
            _audioService.RemoveAudioClips(_gamePlayAudioClipsScriptableObject);
            _commandFactory.CreateCommandVoid<DisposeLevelCommand>().SetShouldReleaseAssetsFromMemory(true).Execute();
            _gameInputActionsController.DisableInputs();
            _gamePlayUiController.InitExitPoint();
        }
    }
}