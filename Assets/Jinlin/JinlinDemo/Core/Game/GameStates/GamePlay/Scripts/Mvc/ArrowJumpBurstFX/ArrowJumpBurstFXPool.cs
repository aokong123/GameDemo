using CoreDomain.Scripts.Helpers.Pools;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.ArrowJumpBurstFX
{
    public class ArrowJumpBurstFXPool : PrefabsPool<ArrowJumpBurstFXView>
    {
        public ArrowJumpBurstFXPool(PoolData poolData, DiContainer diContainer, ArrowJumpBurstFXView arrowJumpBurstFXView) : base(poolData, diContainer, arrowJumpBurstFXView)
        {
        }

        protected override string ParentGameObjectName => "ArrowJumpBurstFXParent";
    }
}
