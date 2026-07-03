using CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Scripts.UI;
using CoreDomain.Scripts.Services.CommandFactory;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Scripts.Commands.EntryPoint
{
    public class ExitLobbyStateCommand : BaseCommand, ICommandVoid
    {
        private ILevelsButtonsController _levelsButtonsController;
        
        public override void ResolveDependencies()
        {
            _levelsButtonsController = _diContainer.Resolve<ILevelsButtonsController>();
        }

        public void Execute()
        {
            _levelsButtonsController.Dispose();
        }
    }
}
