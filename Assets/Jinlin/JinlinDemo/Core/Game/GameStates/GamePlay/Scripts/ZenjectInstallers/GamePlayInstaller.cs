using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Audio;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Initiator;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Arrow;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.ArrowJumpBurstFX;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Balloon;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Bubble;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.GameInputActions;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.GamePlayUi;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Level;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Score;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.ScoreFX;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services.GamePlayData;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services.LevelCancellationToken;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services.PostProcessing;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.ZenjectInstallers
{
    public class GamePlayInstaller : MonoInstaller
    {
        [SerializeField] private GamePlayAudioClipsScriptableObject _gamePlayAudioClipsScriptableObject;
        [SerializeField] private BalloonsConfiguration _balloonsConfiguration;
        [SerializeField] private BubblesConfiguration _bubblesConfiguration;
        [SerializeField] private ArrowMovementConfiguration _arrowMovementConfiguration;
        [SerializeField] private GamePlayUiView _gamePlayUiView;
        [SerializeField] private ArrowView _arrowViewPrefab;
        [SerializeField] private ScoreGainedFXView _scoreGainedFXViewPrefab;
        [SerializeField] private ArrowJumpBurstFXView _arrowJumpBurstFXViewPrefab;
        [SerializeField] private Volume _postProcessVolume;

        public override void InstallBindings()
        {
            BindServices();
            BindControllers();
        }

        private void BindServices()
        {
            Container.BindInterfacesTo<LevelCancellationTokenService>().AsSingle().NonLazy();
            Container.Bind<IGamePlayInitiator>().To<GamePlayInitiator>().AsSingle().NonLazy();
            Container.BindInterfacesTo<GamePlayDataService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<ScoreDataService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<PostProcessingService>().AsSingle().WithArguments(_postProcessVolume).NonLazy();
            Container.Bind<GamePlayAudioClipsScriptableObject>().FromScriptableObject(_gamePlayAudioClipsScriptableObject).AsSingle().NonLazy();
        }

        private void BindControllers()
        {
            Container.BindInterfacesTo<ArrowController>().AsSingle().WithArguments(_arrowMovementConfiguration, _arrowViewPrefab, _arrowJumpBurstFXViewPrefab).NonLazy();
            Container.BindInterfacesTo<GamePlayUiController>().AsSingle().WithArguments(_gamePlayUiView).NonLazy();
            Container.BindInterfacesTo<LevelTrackController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<ScoreFXController>().AsSingle().WithArguments(_scoreGainedFXViewPrefab).NonLazy();
            Container.BindInterfacesTo<BalloonsController>().AsSingle().WithArguments(_balloonsConfiguration).NonLazy();
            Container.BindInterfacesTo<BubblesController>().AsSingle().WithArguments(_bubblesConfiguration).NonLazy();
            Container.BindInterfacesTo<GameInputActionsController>().AsSingle().NonLazy();
        }
    }
}