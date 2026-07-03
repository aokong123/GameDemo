using System.Threading;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Arrow;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.GameInputActions;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Level;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Score;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services.LevelCancellationToken;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services.PostProcessing;
using CoreDomain.Scripts.Mvc.WorldCamera;
using CoreDomain.Scripts.Services.CommandFactory;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands
{
    public class DisposeLevelCommand : BaseCommand, ICommandVoid
    {
        private ILevelTrackController _levelTrackController;
        private IScoreDataService _scoreDataService;
        private IArrowController _arrowController;
        private IWorldCameraController _iWorldCameraController;
        private ILevelCancellationTokenService _levelCancellationTokenService;
        private IPostProcessingService _postProcessingService;
        private IGameInputActionsController _gameInputActionsController;
        
        private bool _shouldReleaseFromMemory;

        public DisposeLevelCommand SetShouldReleaseAssetsFromMemory(bool shouldReleaseFromMemory)
        {
            _shouldReleaseFromMemory = shouldReleaseFromMemory;
            return this;
        }
        
        public override void ResolveDependencies()
        {
            _levelTrackController = _diContainer.Resolve<ILevelTrackController>();
            _scoreDataService = _diContainer.Resolve<IScoreDataService>();
            _arrowController = _diContainer.Resolve<IArrowController>();
            _iWorldCameraController = _diContainer.Resolve<IWorldCameraController>();
            _levelCancellationTokenService = _diContainer.Resolve<ILevelCancellationTokenService>();
            _postProcessingService = _diContainer.Resolve<IPostProcessingService>();
            _gameInputActionsController = _diContainer.Resolve<IGameInputActionsController>();
        }
        
        public void Execute()
        {
            _levelCancellationTokenService.CancelCancellationToken();
            _gameInputActionsController.UnregisterAllInputListeners();
            _levelTrackController.DestroyTrack(_shouldReleaseFromMemory);
            _scoreDataService.ResetScore();
            _arrowController.ResetController();
            _iWorldCameraController.StopFollowTarget();
            _postProcessingService.SetAreAllPostProcessingActive(false);
        }
    }
}
