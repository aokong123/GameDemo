using CoreDomain.GameDomain.Scripts.Services.Levels;
using CoreDomain.GameDomain.Scripts.States.GamePlayState;
using CoreDomain.Scripts.Services.AudioService;
using CoreDomain.Scripts.Services.Logger.Base;
using CoreDomain.Scripts.Services.StateMachineService;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Scripts.UI
{
    public class LevelsButtonsController : ILevelsButtonsController
    {
        private readonly ILevelsDataService _levelsDataService;
        private readonly LevelsButtonsView _levelsButtonsView;
        private readonly IStateMachineService _stateMachineService;
        private readonly GamePlayState.Factory _gamePlayStateFactory;
        private readonly IAudioService _audioService;

        [Inject]
        public LevelsButtonsController(ILevelsDataService levelsDataService, LevelsButtonsView levelsButtonsView, IStateMachineService stateMachineService,
            GamePlayState.Factory gamePlayStateFactory, IAudioService audioService)
        {
            _levelsDataService = levelsDataService;
            _levelsButtonsView = levelsButtonsView;
            _stateMachineService = stateMachineService;
            _gamePlayStateFactory = gamePlayStateFactory;
            _audioService = audioService;
        }

        public void InitEntryPoint()
        {
            var levelsData = _levelsDataService.GetAllLevelsData();
            _levelsButtonsView.Setup(levelsData, _levelsDataService.MaxLevelNumberReached, OnLevelButtonClicked);
        }

        private void OnLevelButtonClicked(int levelNumber)
        {
            LogService.LogTopic($"Level {levelNumber} button clicked", LogTopicType.LobbyUi );
            _audioService.PlayAudio(AudioClipType.UiClick, AudioChannelType.Fx);
            _stateMachineService.SwitchState(_gamePlayStateFactory.Create(new GamePlayInitatorEnterData(levelNumber)));
        }

        public void Dispose()
        {
            _levelsButtonsView.Dispose();
        }
    }
}
