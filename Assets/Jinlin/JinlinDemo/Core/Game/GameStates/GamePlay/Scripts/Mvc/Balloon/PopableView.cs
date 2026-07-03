using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Balloon
{
    public abstract class PopableView : MonoBehaviour
    {
        public abstract void Pop(Vector3 popPosition);
    }
}
