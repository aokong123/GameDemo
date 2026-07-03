using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Balloon
{
    public class BalloonHeadView : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;

        public void SetColor(Color color)
        {
            _renderer.material.color = color;
        }
    }
}
