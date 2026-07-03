using System.Collections.Generic;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Balloon;
using CoreDomain.Scripts.Services.CommandFactory;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.ArrowCollisionEnter
{
    public class ArrowParticleCollisionEnterCommand : BaseCommand, ICommandVoid
    {
        private ArrowParticleCollisionEnterCommandData _commandData;

        public ArrowParticleCollisionEnterCommand SetData(ArrowParticleCollisionEnterCommandData commandData)
        {
            _commandData = commandData;
            return this;
        }
        
        public override void ResolveDependencies()
        {
            
        }

        public void Execute()
        {
            var particleSystem = _commandData.ParticleSystem;
            var popableView = particleSystem.GetComponent<PopableView>();

            if (popableView == null) return;

            var collisionEvents = new List<ParticleCollisionEvent>();
            var numCollisionEvents = particleSystem.GetCollisionEvents(_commandData.ArrowGameObject, collisionEvents);

            for (int i = 0; i < numCollisionEvents; i++)
            {
                popableView.Pop(collisionEvents[i].intersection);
            }
        }
    }
}
