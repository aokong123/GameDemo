using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Arrow;
using CoreDomain.Scripts.Services.CommandFactory;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands
{
    public class JumpInputInvokedCommand : BaseCommand, ICommandVoid
    {
        private IArrowController _arrowController;

        public override void ResolveDependencies()
        {
            _arrowController = _diContainer.Resolve<IArrowController>();
        }

        public void Execute()
        {
            _arrowController.Jump();
        }
    }
}