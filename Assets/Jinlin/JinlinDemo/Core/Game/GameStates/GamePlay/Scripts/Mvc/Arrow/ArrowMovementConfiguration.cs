using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Arrow
{
    [CreateAssetMenu(fileName = "ArrowMovementConfiguration", menuName = "Jinlin/GamePlay/ArrowMovementConfiguration")]
    public class ArrowMovementConfiguration : ScriptableObject
    {
        public float JumpRotationLoopSpeed = -13.5f;
        public float MaxStabAngleWithSurface = 60;
        public float StartRotationLoopSpeed = -12;
        public float ShootAngleRelativeToFloor = -60;
        public float ShootRotationDuration = 0.5f;
        public float JumpAngleRelativeToFloor = 75;
        public float JumpForce = 8;
        public float ShootVelocity = 32f;
        public float AngularDrag = 2;
    }
}