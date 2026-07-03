using System;
using System.Threading;
using CoreDomain.Scripts.Extensions;
using DG.Tweening;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Balloon
{
    public class BalloonView : PopableView
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private BalloonHeadView _balloonHeadView;
        [SerializeField] private GameObject _balloonString;
        [SerializeField] private ParticleSystem _popParticleSystem;
        [SerializeField] private float _stringDisappearAnimationDurationInSeconds = 0.5f;
        
        private Action<BalloonView, Vector3> _onPoppedAction;
        
        public void Setup(Action<BalloonView, Vector3> onPoppedAction, Color color)
        {
            _onPoppedAction = onPoppedAction;
            _balloonHeadView.SetColor(color);
        }
    
        public override void Pop(Vector3 popPosition)
        {
            _onPoppedAction?.Invoke(this, popPosition);
        }

        public async Awaitable PlayPopEffect(CancellationTokenSource cancellationTokenSource)
        {
            _collider.enabled = false;
            _balloonHeadView.gameObject.SetActive(false);
            _popParticleSystem.Play();
            await _balloonString.transform.DOScale(0, _stringDisappearAnimationDurationInSeconds).SetEase(Ease.OutCubic).WithCancellationSafe(cancellationTokenSource.Token);
            _balloonString.gameObject.SetActive(false);
        }
    }
}
