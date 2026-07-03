using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Arrow
{
    public class ArrowFactory
    {
        private readonly ArrowView _arrowViewPrefab;

        public ArrowFactory(ArrowView arrowViewPrefab)
        {
            _arrowViewPrefab = arrowViewPrefab;
        }

        public ArrowView CreateArrow()
        {
            return Object.Instantiate(_arrowViewPrefab);
        }
    }
}
