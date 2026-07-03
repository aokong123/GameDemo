using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.ArrowCollisionEnter;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.ArrowTriggerEnter;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.ArrowJumpBurstFX;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services.LevelCancellationToken;
using CoreDomain.Scripts.Services.AudioService;
using CoreDomain.Scripts.Services.CommandFactory;
using CoreDomain.Scripts.Services.Logger.Base;
using CoreDomain.Scripts.Services.ResourcesLoaderService;
using CoreDomain.Scripts.Services.UpdateService;
using CoreDomain.Scripts.Utils;
using UnityEngine;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Arrow
{
    public class ArrowController : IArrowController, IFixedUpdatable
    {
        private static readonly Vector3 ARROW_START_POSITION = new (0, 10, 0);
        private static readonly Vector3 DEFAULT_JUMP_SURFACE_NORMAL = Vector3.up;
        private const float ARROW_START_ROTATION_ANGLE = -50f;  
        private const float MINIMAL_STAB_ANGLE = 85f;
        private const float STAB_ANGLE_TOLERANCE = 30f;
        private const float START_NEW_LOOP_ANGLE = 275f;
        private const float START_NEW_LOOP_ANGLE_TOLERANCE = 10f;

        private readonly IUpdateSubscriptionService _updateSubscriptionService;
        private readonly IAudioService _audioService;
        private readonly ICommandFactory _commandFactory;
        private readonly IResourcesLoaderService _resourcesLoaderService;
        
        public Transform ArrowTransform => _arrowView.transform;

        private ArrowView _arrowView;
        private bool _canStabInCurrentLoop = true;
        private bool _isCurrentlyStabbing;
        private float _previousZRotation;
        private readonly ArrowFactory _arrowFactory;
        private readonly ArrowMovementController _arrowMovementController;
        private readonly ArrowMovementConfiguration _arrowMovementConfiguration;
        private readonly ArrowJumpBurstFXController _arrowJumpBurstFXController;
        private Vector3 _contactSurfaceNormal;

        public ArrowController(IUpdateSubscriptionService updateSubscriptionService,
            IAudioService audioService, ICommandFactory commandFactory, DiContainer diContainer, ArrowJumpBurstFXView arrowJumpBurstFXViewPrefab,
            ILevelCancellationTokenService levelCancellationTokenService, ArrowMovementConfiguration arrowMovementConfiguration, ArrowView arrowViewPrefab)
        {
            _arrowFactory = new ArrowFactory(arrowViewPrefab);
            _arrowMovementController = new ArrowMovementController(levelCancellationTokenService);
            _arrowJumpBurstFXController = new ArrowJumpBurstFXController(diContainer, arrowJumpBurstFXViewPrefab, levelCancellationTokenService);
            _updateSubscriptionService = updateSubscriptionService;
            _audioService = audioService;
            _commandFactory = commandFactory;
            _arrowMovementConfiguration = arrowMovementConfiguration;
        }

        public void InitEntryPoint()
        {
            _arrowMovementController.SetArrowMovementConfiguration(_arrowMovementConfiguration);
            _arrowJumpBurstFXController.InitEntryPoint();
        }

        public void RegisterListeners()
        {
            _updateSubscriptionService.RegisterFixedUpdatable(this);
        }

        public void SetIsThrusterEnabled(bool isEnabled)
        {
            _arrowView.SetIsThrusterEnabled(isEnabled);
        }

        public void DisableCallbacks()
        {
            _arrowView.RemoveAllCallbacks();
        }

        public void FreezeMovement()
        {
            _arrowMovementController.FreezeMovement(true, true);
        }
        
        public void UnfreezeMovement()
        {
            _arrowMovementController.UnfreezeMovement();
        }

        public void CreateArrow()
        {
            _arrowView = _arrowFactory.CreateArrow();
            _arrowView.SetupCallbacks(OnArrowCollisionEnter, OnArrowTriggerEnter, OnArrowParticleCollisionEnter);
            _arrowMovementController.SetArrow(_arrowView, ARROW_START_POSITION, ARROW_START_ROTATION_ANGLE);
        }

        private void Shoot()
        {
            LogService.LogTopic("Shoot", LogTopicType.Arrow);
            _audioService.PlayAudio(AudioClipType.Spin, AudioChannelType.Fx);
            _arrowMovementController.Shoot();
        }

        public void TryShoot()
        {
            if (_isCurrentlyStabbing)
            {
                return;
            }

            Shoot();
        }

        public void Jump()
        {
            LogService.LogTopic("Jump", LogTopicType.Arrow);
            _audioService.PlayAudio(AudioClipType.Jump, AudioChannelType.Fx);
            var surfaceNormal = _isCurrentlyStabbing ? _contactSurfaceNormal : DEFAULT_JUMP_SURFACE_NORMAL;
            _arrowMovementController.Jump(surfaceNormal);
            var effectRotation = Quaternion.LookRotation(surfaceNormal);
            _arrowJumpBurstFXController.ShowArrowJumpBurstFx(ArrowTransform.position, effectRotation);
            SetIsThrusterEnabled(false);
            _isCurrentlyStabbing = false;
        }

        public void ManagedFixedUpdate()
        {
            var currentZRotation = Mathf.Repeat(_arrowView.GetZRotation(), 360);

            TrySelfLoop(currentZRotation);
            TryResetIfCanStab(currentZRotation);
        
            _previousZRotation = currentZRotation;
        }

        public bool TryStabContactPoint(ContactPoint collisionContact)
        {
            if (!_canStabInCurrentLoop || !_arrowMovementController.DidStabContactPoint(collisionContact))
            {
                return false;
            }

            _contactSurfaceNormal = collisionContact.normal;
            _audioService.PlayAudio(AudioClipType.Stab, AudioChannelType.Fx);
            FreezeMovement();
            _isCurrentlyStabbing = true;
            _canStabInCurrentLoop = false;

            return true;
        }

        private void OnArrowCollisionEnter(Collision collision)
        {
            _commandFactory.CreateCommandVoid<ArrowCollisionEnterCommand>().SetEnterData(new ArrowCollisionEnterCommandData(collision)).Execute();
        }

        private void OnArrowTriggerEnter(Collider collider)
        {
            _commandFactory.CreateCommandVoid<ArrowTriggerEnterCommand>().SetEnterData(new ArrowTriggerEnterCommandData(collider)).Execute();
        }

        private void OnArrowParticleCollisionEnter(ParticleSystem particleSystem)
        {
            _commandFactory.CreateCommandVoid<ArrowParticleCollisionEnterCommand>().SetData(new ArrowParticleCollisionEnterCommandData(particleSystem, _arrowView.gameObject)).Execute();
        }

        private void TryResetIfCanStab(float currentZRotation)
        {
            if (_canStabInCurrentLoop)
            {
                return;
            }
            
            var didPassMinimalStabAngle = MathUtils.DidCrossTargetAngle(_previousZRotation, currentZRotation, MINIMAL_STAB_ANGLE, STAB_ANGLE_TOLERANCE);
            if (didPassMinimalStabAngle)
            {
                _canStabInCurrentLoop = true;
            }
        }

        private void TrySelfLoop(float currentZRotation)
        {
            var shouldStartANewLoop = MathUtils.DidCrossTargetAngle(_previousZRotation, currentZRotation, START_NEW_LOOP_ANGLE, START_NEW_LOOP_ANGLE_TOLERANCE);
            if (shouldStartANewLoop)
            {
                _arrowMovementController.SetLoopAngularVelocity();
            }
        }

        public void ResetController()
        {
            _updateSubscriptionService.UnregisterFixedUpdatable(this);
            Object.Destroy(_arrowView.gameObject);
            _canStabInCurrentLoop = true;
            _isCurrentlyStabbing = false;
        }
    }
}