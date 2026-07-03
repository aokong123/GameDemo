using System;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Arrow
{
    public class ArrowView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private ParticleSystem _thrusterParticleSystem;
        [SerializeField] private Collider[] _colliders;
    
        private Action<Collision> _onCollisionEnter;
        private Action<Collider> _onTriggerEnter;
        private Action<ParticleSystem> _onParticleCollisionEnter;

        public void SetupCallbacks(Action<Collision> onCollisionEnter, Action<Collider> onTriggerEnter, Action<ParticleSystem> onParticleCollisionEnter)
        {
            _onCollisionEnter = onCollisionEnter;
            _onTriggerEnter = onTriggerEnter;
            _onParticleCollisionEnter = onParticleCollisionEnter;
        }

        public void RemoveAllCallbacks()
        {
            _onCollisionEnter = null;
            _onTriggerEnter = null;
            _onParticleCollisionEnter = null;
        }

        public void SetAngularDrag(float angularDrag)
        {
            _rigidbody.angularDamping = angularDrag;
        }

        private void OnCollisionEnter(Collision collision)
        {
            _onCollisionEnter?.Invoke(collision);
        }
    
        private void OnParticleCollision(GameObject particleSystemGO)
        {
            _onParticleCollisionEnter?.Invoke(particleSystemGO.GetComponent<ParticleSystem>());
        }

        private void OnTriggerEnter(Collider otherCollider)
        {
            _onTriggerEnter?.Invoke(otherCollider);
        }

        public void SetZAngularVelocity(float angularVelocity)
        {
            _rigidbody.angularVelocity = new Vector3(0, 0, angularVelocity);
        }
    
        public void SetZRotation(float angle)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
            _rigidbody.rotation = Quaternion.Euler(0, 0, angle);
        }

        public void SetPoisiton(Vector3 position)
        {
            transform.position = position;
            _rigidbody.position = position;
        }

        public void FreezeMovement(bool isDisableGravity, bool isEnableKinematic)
        {
            SetVelocity(Vector3.zero);
            SetAngularVelocity(Vector3.zero);
        
            if (isDisableGravity)
            {
                SetIsGravityEnabled(false);
            } 
        
            if (isEnableKinematic)
            {
                SetIsKinematicEnabled(true);
            }
        }

        public float GetZRotation()
        {
            return _rigidbody.rotation.eulerAngles.z;
        }

        public void SetIsGravityEnabled(bool isEnabled)
        {
            _rigidbody.useGravity = isEnabled;
        }

        public void SetAngularVelocity(Vector3 velocity)
        {
            _rigidbody.angularVelocity = velocity;
        }

        public void SetVelocity(Vector3 velocity)
        {
            _rigidbody.linearVelocity = velocity;
        }

        public void UnfreezeMovement()
        {
            SetIsKinematicEnabled(false);
            SetIsGravityEnabled(true);
        }
        
        public void SetIsKinematicEnabled(bool isEnabled)
        {
            _rigidbody.isKinematic = isEnabled;
        }
    
        public void SetIsThrusterEnabled(bool isEnabled)
        {
            if (isEnabled)
            {
                _thrusterParticleSystem.Play();
            }
            else
            {
                _thrusterParticleSystem.Stop();
            }
        }
    }
}
