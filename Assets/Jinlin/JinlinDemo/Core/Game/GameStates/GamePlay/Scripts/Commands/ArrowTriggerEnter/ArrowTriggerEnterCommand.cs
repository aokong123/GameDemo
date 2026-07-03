using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Balloon;
using CoreDomain.Scripts.Services.CommandFactory;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.ArrowTriggerEnter
{
    public class ArrowTriggerEnterCommand : BaseCommand, ICommandVoid
    {
        private Collider _collider;

        public ArrowTriggerEnterCommand SetEnterData(ArrowTriggerEnterCommandData enterCommandData)
        {
            _collider = enterCommandData.Collider;
            return this;
        }

        public override void ResolveDependencies()
        {
            
        }

        public void Execute()
        {
            var otherPopableView = _collider.GetComponent<PopableView>();

            if (otherPopableView != null)
            {
                otherPopableView.Pop(_collider.bounds.center);
            }
        }
    }
}
