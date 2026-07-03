using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services.LevelCancellationToken;
using CoreDomain.GameDomain.Scripts.States.LobbyState;
using CoreDomain.Scripts.Mvc.UICamera;
using CoreDomain.Scripts.Services.AudioService;
using CoreDomain.Scripts.Services.Logger.Base;
using CoreDomain.Scripts.Services.StateMachineService;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.GamePlayUi
{
    public class GamePlayUiController : IGamePlayUiController
    {
        private readonly IStateMachineService _stateMachineService;
        private readonly LobbyState.Factory _lobbyStateFactory;
        private readonly IUICameraController _uiCameraController;
        private readonly GamePlayUiView _gamePlayView;
        private readonly ILevelCancellationTokenService _levelCancellationTokenService;
        private readonly IAudioService _audioService;

        public GamePlayUiController(IStateMachineService stateMachineService, LobbyState.Factory lobbyStateFactory, IUICameraController uiCameraController,
            GamePlayUiView gamePlayView, ILevelCancellationTokenService levelCancellationTokenService, IAudioService audioService)
        {
            _stateMachineService = stateMachineService;
            _lobbyStateFactory = lobbyStateFactory;
            _uiCameraController = uiCameraController;
            _gamePlayView = gamePlayView;
            _levelCancellationTokenService = levelCancellationTokenService;
            _audioService = audioService;
        }

        public void InitEntryPoint()
        {
            _gamePlayView.InitEntryPoint(_uiCameraController.UICamera, OnExitButtonClicked);
        }

        private void OnExitButtonClicked()
        {
            LogService.LogTopic("Exit button clicked", LogTopicType.GamePlayUi );
            _audioService.PlayAudio(AudioClipType.UiClick, AudioChannelType.Fx);
            _stateMachineService.SwitchState(_lobbyStateFactory.Create(new LobbyInitiatorEnterData()));
        }

        public void UpdateScore(int newScore)
        {
            _gamePlayView.UpdateScore(newScore, _levelCancellationTokenService.CancellationTokenSource);
        }
        
        public void ShowWinPanel(int scoreAchieved, int scoreGoal)
        {
            _gamePlayView.ShowWinPanel(scoreAchieved, scoreGoal, _levelCancellationTokenService.CancellationTokenSource);
        }

        public void InitExitPoint()
        {
            _gamePlayView.InitExitPoint();
        }

        public void ShowGameOverPanel(int score, int scoreGoal, bool shouldShowScore)
        {
            _gamePlayView.ShowGameOverPanel(score, scoreGoal, shouldShowScore, _levelCancellationTokenService.CancellationTokenSource);
        }
        
        public void SwitchToInGameView(int score, int scoreGoal)
        {
            _gamePlayView.SwitchToInGameView();
            _gamePlayView.SetStartingValues(score, scoreGoal, _levelCancellationTokenService.CancellationTokenSource);
        }
        
        public void SwitchToBeforeGameView(int currentLevel)
        {
            _gamePlayView.SwitchToBeforeGameView(currentLevel);
        }
    }
}