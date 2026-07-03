using System.Threading;
using CoreDomain.GameDomain.Scripts.Services.Levels;
using CoreDomain.GameDomain.Scripts.States.LobbyState;
using CoreDomain.Scripts.CoreInitiator;
using CoreDomain.Scripts.CoreInitiator.Base;
using CoreDomain.Scripts.Mvc.LoadingScreen;
using CoreDomain.Scripts.Services.InitiatorInvokerService;
using CoreDomain.Scripts.Services.SceneService;
using CoreDomain.Scripts.Services.StateMachineService;
using CoreDomain.Scripts.Utils;
using UnityEngine;

namespace CoreDomain.GameDomain.Scripts.GameInitiator
{
    public class GameInitiator : ISceneInitiator, IGameInitiator
    {
        private readonly IStateMachineService _stateMachine;
        private readonly ILoadingScreenController _loadingScreenController;
        private readonly LobbyState.Factory _lobbyStateFactory;
        private readonly ILevelsDataService _levelsDataService;
        private readonly ISceneInitiatorsService _sceneInitiatorsService;
        
        public SceneType SceneType => SceneType.GameScene;

        public GameInitiator(IStateMachineService stateMachine, LobbyState.Factory LobbyStateFactory, ILoadingScreenController loadingScreenController, ILevelsDataService levelsDataService, ISceneInitiatorsService sceneInitiatorsService)
        {
            _stateMachine = stateMachine;
            _lobbyStateFactory = LobbyStateFactory;
            _loadingScreenController = loadingScreenController;
            _levelsDataService = levelsDataService;
            _sceneInitiatorsService = sceneInitiatorsService;
            _sceneInitiatorsService.RegisterInitiator(this);
        }

        public async Awaitable LoadEntryPoint(IInitiatorEnterData enterDataObject, CancellationTokenSource cancellationTokenSource)
        {
            var enterData = (GameInitiatorEnterData)enterDataObject; // kept for future use
            _ = _loadingScreenController.SetLoadingSlider(0.5f, cancellationTokenSource);
            await _levelsDataService.LoadLevelsData(cancellationTokenSource);
            await _stateMachine.EnterInitialGameState(_lobbyStateFactory.Create(new LobbyInitiatorEnterData()), cancellationTokenSource);
        }

        public Awaitable StartEntryPoint(IInitiatorEnterData enterDataObject, CancellationTokenSource cancellationTokenSource)
        {
            return AwaitableUtils.CompletedTask;
        }

        public Awaitable InitExitPoint(CancellationTokenSource cancellationTokenSource)
        {
            _sceneInitiatorsService.UnregisterInitiator(this);
            return AwaitableUtils.CompletedTask;
        }
    }
}