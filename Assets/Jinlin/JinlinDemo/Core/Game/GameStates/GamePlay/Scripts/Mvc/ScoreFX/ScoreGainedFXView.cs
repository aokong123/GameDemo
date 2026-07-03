using System;
using System.Threading;
using CoreDomain.Scripts.Extensions;
using CoreDomain.Scripts.Helpers.Pools;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.ScoreFX
{
    public class ScoreGainedFXView : MonoBehaviour, IPoolable
    {
        private const string TEXT_EFFECT_FORMAT = "+{0}";
    
        [SerializeField] private TextMeshPro _text;
        [SerializeField] private float _enterDurationSeconds;
        [SerializeField] private float _idleDurationSeconds;
        [SerializeField] private float _exitDurationSeconds;
        [SerializeField] private float _scale;
        [SerializeField] private Ease _enterEase = Ease.OutBounce;
        [SerializeField] private Ease _exitEase = Ease.InCubic;

        private Transform _transform;

        public void OnCreated()
        {
            _transform = transform;
        }

        public void Setup(int scoreGained, Vector3 position)
        {
            _transform.position = position;
            _transform.localScale = Vector3.zero;
            _text.text = string.Format(TEXT_EFFECT_FORMAT, scoreGained.ToString());
        }

        public async Awaitable DoShowAnimation(CancellationTokenSource cancellationTokenSource)
        {
            await transform.DOScale(_scale, _enterDurationSeconds).SetEase(_enterEase).WithCancellationSafe(cancellationTokenSource.Token);
            await Awaitable.WaitForSecondsAsync(_idleDurationSeconds, cancellationToken: cancellationTokenSource.Token);
            await transform.DOScale(Vector3.zero, _exitDurationSeconds).SetEase(_exitEase).WithCancellationSafe(cancellationTokenSource.Token);
        }

        public Action Despawn { get; set; }
    
        public void OnSpawned()
        {
            gameObject.SetActive(true);
        }

        public void OnDespawned()
        {
            gameObject.SetActive(false);
        }
    }
}
