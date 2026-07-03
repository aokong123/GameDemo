using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Balloon;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.PopBalloonCommand
{
    public class PopBalloonCommandData
    {
        public readonly BalloonView BalloonView;
        public readonly Vector3 PopPosition;

        public PopBalloonCommandData(BalloonView balloonView, Vector3 popPosition)
        {
            BalloonView = balloonView;
            PopPosition = popPosition;
        }
    }
}
