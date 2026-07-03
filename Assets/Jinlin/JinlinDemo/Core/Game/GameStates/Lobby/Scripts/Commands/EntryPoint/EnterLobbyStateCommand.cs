using System.Threading;
using CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Scripts.UI;
using CoreDomain.GameDomain.Scripts.States.LobbyState;
using CoreDomain.Scripts.Services.CommandFactory;
using CoreDomain.Scripts.Utils;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Scripts.Commands.EntryPoint
{
    public class EnterLobbyStateCommand: BaseCommand, ICommandAsync
    {
        private ILevelsButtonsController _levelsButtonsController;
        private ILobbyUiCanvasController _lobbyUiCanvasController;
        private LobbyInitiatorEnterData _enterData;

        public EnterLobbyStateCommand SetEnterData(LobbyInitiatorEnterData enterData)
        {
            _enterData = enterData;
            return this;
        }
        
        public override void ResolveDependencies()
        {
            _levelsButtonsController = _diContainer.Resolve<ILevelsButtonsController>();
            _lobbyUiCanvasController = _diContainer.Resolve<ILobbyUiCanvasController>();
        }

        public Awaitable Execute(CancellationTokenSource cancellationTokenSource)
        {
            _lobbyUiCanvasController.InitEntryPoint();
            _levelsButtonsController.InitEntryPoint();
            return AwaitableUtils.CompletedTask;
        }
    }
}
