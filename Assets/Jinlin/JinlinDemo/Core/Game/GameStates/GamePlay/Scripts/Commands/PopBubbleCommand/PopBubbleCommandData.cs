using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.PopBubbleCommand
{
    public class PopBubbleCommandData
    {
        public Vector3 Position;

        public PopBubbleCommandData(Vector3 position)
        {
            Position = position;
        }
    }
}
