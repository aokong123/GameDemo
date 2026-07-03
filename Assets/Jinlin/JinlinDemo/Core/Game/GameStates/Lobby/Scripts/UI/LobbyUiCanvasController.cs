using CoreDomain.Scripts.Mvc.UICamera;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Scripts.UI
{
    public class LobbyUiCanvasController : ILobbyUiCanvasController
    {
        private readonly LobbyUiCanvasView _lobbyUiCanvasView;
        private readonly IUICameraController _uiCameraController;

        public LobbyUiCanvasController(IUICameraController uiCameraController, LobbyUiCanvasView lobbyUiCanvasView)
        {
            _uiCameraController = uiCameraController;
            _lobbyUiCanvasView = lobbyUiCanvasView;
        }
        
        public void InitEntryPoint()
        {
            _lobbyUiCanvasView.InitEntryPoint(_uiCameraController.UICamera);
        }
    }
}
