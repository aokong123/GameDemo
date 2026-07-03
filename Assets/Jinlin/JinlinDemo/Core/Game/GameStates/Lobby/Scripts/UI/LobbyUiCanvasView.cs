using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Scripts.UI
{
    public class LobbyUiCanvasView : MonoBehaviour
    {
        [SerializeField] Canvas _screenCanvas;
        
        public void InitEntryPoint(Camera uiCamera)
        {
            _screenCanvas.worldCamera = uiCamera;
        }
    }
}
