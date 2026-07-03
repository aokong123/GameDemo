using CoreDomain.GameDomain.Scripts.GameInitiator;
using CoreDomain.GameDomain.Scripts.Services.Levels;
using CoreDomain.GameDomain.Scripts.States.GamePlayState;
using CoreDomain.GameDomain.Scripts.States.LobbyState;
using CoreDomain.Scripts.Services.AudioService;
using UnityEngine;
using Zenject;

namespace CoreDomain.GameDomain.Scripts.ZenjectInstallers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] AudioClipsScriptableObject _audioClips;
        
        public override void InstallBindings()
        {
            Container.Bind<IGameInitiator>().To<GameInitiator.GameInitiator>().AsSingle().NonLazy();
            Container.BindFactory<GamePlayInitatorEnterData, GamePlayState, GamePlayState.Factory>();
            Container.BindInterfacesTo<LevelsDataService>().AsSingle().NonLazy();
            Container.BindFactory<LobbyInitiatorEnterData, LobbyState, LobbyState.Factory>().AsSingle().NonLazy();
        }
    }
}