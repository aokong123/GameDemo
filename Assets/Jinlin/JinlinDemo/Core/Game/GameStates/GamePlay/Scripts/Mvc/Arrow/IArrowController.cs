using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Arrow
{
    public interface IArrowController
    {
        void InitEntryPoint();
        void CreateArrow();
        Transform ArrowTransform { get; }
        bool TryStabContactPoint(ContactPoint collisionContact);
        void Jump();
        void TryShoot();
        void ResetController();
        void RegisterListeners();
        void SetIsThrusterEnabled(bool isEnabled);
        void DisableCallbacks();
        public void FreezeMovement();
        public void UnfreezeMovement();
    }
}