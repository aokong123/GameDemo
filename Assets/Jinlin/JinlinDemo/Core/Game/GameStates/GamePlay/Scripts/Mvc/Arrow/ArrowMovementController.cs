using System;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services.LevelCancellationToken;
using CoreDomain.Scripts.Extensions;
using CoreDomain.Scripts.Services.Logger.Base;
using CoreDomain.Scripts.Utils;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Arrow
{
    public class ArrowMovementController
    {
        private const int ANGLES_IN_CIRCLE = 360;
        private const int HALF_ANGLES_IN_CIRCLE = 180;
        
        private ArrowView _arrowView;
        private ArrowMovementConfiguration _arrowMovementConfiguration;
        private Vector3 _shootVector;
        private Vector3 _jumpVector;
        private TweenerCore<float,float,FloatOptions> _spinBeforeShootTween;
        private readonly ILevelCancellationTokenService _levelCancellationTokenService;

        public ArrowMovementController(ILevelCancellationTokenService levelCancellationTokenService)
        {
            _levelCancellationTokenService = levelCancellationTokenService;
        }

        public void SetArrow(ArrowView arrowView, Vector3 arrowPosition, float arrowRotationAngle)
        {
            _arrowView = arrowView;
            _arrowView.SetAngularDrag(_arrowMovementConfiguration.AngularDrag);
            _arrowView.SetZRotation(arrowRotationAngle);
            _arrowView.SetPoisiton(arrowPosition);
        }
    
        public void SetArrowMovementConfiguration(ArrowMovementConfiguration arrowMovementConfiguration)
        {
            _arrowMovementConfiguration = arrowMovementConfiguration;
            _shootVector = Quaternion.Euler(0, 0, _arrowMovementConfiguration.ShootAngleRelativeToFloor) * Vector3.right * _arrowMovementConfiguration.ShootVelocity;
            _jumpVector = Quaternion.Euler(0, 0, _arrowMovementConfiguration.JumpAngleRelativeToFloor) * Vector3.right * _arrowMovementConfiguration.JumpForce;
        }
    
        public void Shoot()
        {
            _ = ShootAsync();
        }

        private async Awaitable ShootAsync()
        {
            try
            {
                FreezeMovement(true, false);

                var currentRotationAngle = Mathf.Repeat(_arrowView.GetZRotation(), 360);
                var loopAnimationAngles = AddLoopForPrettierAnimationIfAngleIsSmall(currentRotationAngle);
        
                _arrowView.SetZRotation(loopAnimationAngles);
                _spinBeforeShootTween?.Kill();
                _spinBeforeShootTween = DOTween.To(
                    () => loopAnimationAngles - _arrowMovementConfiguration.ShootAngleRelativeToFloor,
                    x =>
                    {
                        loopAnimationAngles = x + _arrowMovementConfiguration.ShootAngleRelativeToFloor;
                        _arrowView.SetZRotation(loopAnimationAngles);
                    },
                    0, _arrowMovementConfiguration.ShootRotationDuration);
                _spinBeforeShootTween.OnComplete(LaunchArrow);
                await _spinBeforeShootTween.WithCancellationSafe(_levelCancellationTokenService.CancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                LogService.Log("Operation ShootAsync was cancelled");
            }
            catch (Exception e)
            {
                LogService.LogError(e.Message);
                throw;
            }
        }

        private float AddLoopForPrettierAnimationIfAngleIsSmall(float rotationAngle)
        {
            if (rotationAngle < HALF_ANGLES_IN_CIRCLE && rotationAngle > _arrowMovementConfiguration.ShootAngleRelativeToFloor)
            {
                rotationAngle += ANGLES_IN_CIRCLE;
            }

            return rotationAngle;
        }

        private void LaunchArrow()
        {
            _arrowView.SetIsThrusterEnabled(true);
            _arrowView.SetIsGravityEnabled(true);
            _arrowView.SetAngularVelocity(Vector3.zero);
            _arrowView.SetVelocity(_shootVector);
        }

        public void Jump(Vector3 surfaceNormal)
        {
            _spinBeforeShootTween?.Kill();
            _arrowView.UnfreezeMovement();
            var jumpVector = MathUtils.RotateVectorRelativeToSurface(_jumpVector, surfaceNormal);
            _arrowView.SetVelocity(jumpVector);
            _arrowView.SetZAngularVelocity(_arrowMovementConfiguration.JumpRotationLoopSpeed);
        }

        public bool DidStabContactPoint(ContactPoint contactPoint)
        {
            var contactPointNormal = contactPoint.normal;
            var hitVector = -_arrowView.transform.right;
            return Vector3.Angle(hitVector, contactPointNormal) < _arrowMovementConfiguration.MaxStabAngleWithSurface;
        }

        public void FreezeMovement(bool isDisableGravity, bool isEnableKinematic)
        {
            _arrowView.FreezeMovement(isDisableGravity, isEnableKinematic);
        }

        public void UnfreezeMovement()
        {
            _arrowView.UnfreezeMovement();
        }

        public void SetLoopAngularVelocity()
        {
            _arrowView.SetZAngularVelocity(_arrowMovementConfiguration.StartRotationLoopSpeed);
        }
    }
}
