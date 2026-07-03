using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.ArrowTriggerEnter
{
    public class ArrowTriggerEnterCommandData
    {
        public Collider Collider;

        public ArrowTriggerEnterCommandData(Collider collider)
        {
            Collider = collider;
        }

    }
}
