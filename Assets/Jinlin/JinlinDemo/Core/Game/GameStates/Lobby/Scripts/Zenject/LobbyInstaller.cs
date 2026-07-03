using CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Scripts.Initiator;
using CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Scripts.UI;
using UnityEngine;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Scripts.Zenject
{
    public class LobbyInstaller : MonoInstaller
    {
        [SerializeField] private LevelsButtonsView _levelsButtonsView;
        [SerializeField] private LobbyUiCanvasView _lobbyUiCanvasView;

        public override void InstallBindings()
        {
            Container.Bind<ILobbyInitiator>().To<LobbyInitiator>().AsSingle().NonLazy();
            Container.Bind<ILobbyUiCanvasController>().To<LobbyUiCanvasController>().AsSingle().WithArguments(_lobbyUiCanvasView).NonLazy();
            Container.BindInterfacesTo<LevelsButtonsController>().AsSingle().WithArguments(_levelsButtonsView).NonLazy();
        }
    }
}
