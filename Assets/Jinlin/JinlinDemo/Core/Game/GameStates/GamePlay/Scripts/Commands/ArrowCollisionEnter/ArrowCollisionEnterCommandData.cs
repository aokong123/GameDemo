using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.ArrowCollisionEnter
{
    public class ArrowCollisionEnterCommandData
    {
        public Collision Collision;

        public ArrowCollisionEnterCommandData(Collision collision)
        {
            Collision = collision;
        }
    }
}
