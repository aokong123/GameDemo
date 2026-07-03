using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.PopBubbleCommand;
using CoreDomain.Scripts.Extensions;
using CoreDomain.Scripts.Services.CommandFactory;
using CoreDomain.Scripts.Services.Logger.Base;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Bubble
{
    public class BubblesController : IBubblesController
    {
        public int BubblesPopScore => _bubblesConfiguration.PopScore;
        
        private readonly ICommandFactory _commandFactory;
        private readonly BubblesConfiguration _bubblesConfiguration;

        public BubblesController(ICommandFactory commandFactory, BubblesConfiguration bubblesConfiguration)
        {
            _commandFactory = commandFactory;
            _bubblesConfiguration = bubblesConfiguration;
        }

        public void SetupBubbles(BubblesView[] bubblesViews)
        {
            if (bubblesViews.IsNullOrEmpty())
            {
                return;
            }
            
            foreach (var bubblesView in bubblesViews)
            {
                bubblesView.Setup(OnBubblePopTriggered);
            }
        }
    
        private void OnBubblePopTriggered(Vector3 position)
        {
            LogService.LogTopic($"Poped bubble in position : {position}", LogTopicType.Bubble);
            _commandFactory.CreateCommandVoid<PopBubbleCommand>().SetData(new PopBubbleCommandData(position)).Execute();
        }
    }
}
