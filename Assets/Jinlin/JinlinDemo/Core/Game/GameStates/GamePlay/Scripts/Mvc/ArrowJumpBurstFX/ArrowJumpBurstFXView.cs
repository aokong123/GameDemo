using System;
using System.Threading;
using CoreDomain.Scripts.Helpers.Pools;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.ArrowJumpBurstFX
{
    public class ArrowJumpBurstFXView : MonoBehaviour, IPoolable
    {
        [SerializeField] private ParticleSystem _particleSystem;

        private Transform _transform;

        public void OnCreated()
        {
            _transform = transform;
        }

        public void Setup(Vector3 position, Quaternion rotation)
        {
            _transform.position = position;
            _transform.rotation = rotation;
        }

        public async Awaitable DoShowAnimation(CancellationTokenSource cancellationTokenSource)
        {
            _particleSystem.Stop();
            _particleSystem.Play();
            await Awaitable.WaitForSecondsAsync(_particleSystem.main.duration, cancellationTokenSource.Token);
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
