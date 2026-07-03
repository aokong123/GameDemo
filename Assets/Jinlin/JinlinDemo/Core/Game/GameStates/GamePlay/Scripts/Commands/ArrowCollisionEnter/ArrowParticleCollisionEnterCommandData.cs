using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.ArrowCollisionEnter
{
    public class ArrowParticleCollisionEnterCommandData
    {
        public ParticleSystem ParticleSystem;
        public GameObject ArrowGameObject;

        public ArrowParticleCollisionEnterCommandData(ParticleSystem particleSystem, GameObject arrowGameObject)
        {
            ParticleSystem = particleSystem;
            ArrowGameObject = arrowGameObject;
        }
    }
}
