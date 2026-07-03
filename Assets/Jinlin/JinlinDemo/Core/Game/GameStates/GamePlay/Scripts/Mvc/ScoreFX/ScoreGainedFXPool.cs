using CoreDomain.Scripts.Helpers.Pools;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.ScoreFX
{
    public class ScoreGainedFXPool : PrefabsPool<ScoreGainedFXView>
    {
        public ScoreGainedFXPool(PoolData poolData, DiContainer diContainer, ScoreGainedFXView scoreGainedFXViewPrefab) : base(poolData, diContainer, scoreGainedFXViewPrefab)
        {
        }

        protected override string ParentGameObjectName => "ScoreGainedFXParent";
    }
}
