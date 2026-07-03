using System;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Balloon;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Bubble
{
    public class BubblesView : PopableView
    {
        private Action<Vector3> _onBubblePopTriggered;

        public void Setup(Action<Vector3> onBubblePopTriggered)
        {
            _onBubblePopTriggered = onBubblePopTriggered;
        }

        public override void Pop(Vector3 popPosition)
        {
            _onBubblePopTriggered?.Invoke(popPosition);
        }
    }
}
