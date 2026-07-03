using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.PopBalloonCommand;
using CoreDomain.Scripts.Extensions;
using CoreDomain.Scripts.Services.CommandFactory;
using CoreDomain.Scripts.Services.Logger.Base;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Balloon
{
    public class BalloonsController : IBalloonsController
    {
        public int BalloonPopScore => _balloonsConfiguration.PopScore;
        private readonly ICommandFactory _commandFactory;
        private readonly BalloonsConfiguration _balloonsConfiguration;
    
        public BalloonsController(ICommandFactory commandFactory, BalloonsConfiguration balloonsConfiguration)
        {
            _commandFactory = commandFactory;
            _balloonsConfiguration = balloonsConfiguration;
        }
    
        public void SetupBalloons(BalloonView[] balloonViews)
        {
            if (balloonViews.IsNullOrEmpty())
            {
                return;
            }
            
            var balloonsColorPalette = _balloonsConfiguration.BalloonsColorPalette;
            var balloonsColorPaletteLength = balloonsColorPalette.Length;
        
            foreach (var balloonView in balloonViews)
            {
                var randomColor = balloonsColorPalette[Random.Range(0, balloonsColorPaletteLength)];
                balloonView.Setup(OnBalloonPopTriggered, randomColor);
            }
        }

        private void OnBalloonPopTriggered(BalloonView balloonView, Vector3 popPosition)
        {
            LogService.LogTopic($"Poped balloon in position : {popPosition}", LogTopicType.Balloon);
            _commandFactory.CreateCommandVoid<PopBalloonCommand>().SetData(new PopBalloonCommandData(balloonView, popPosition)).Execute();
        }
    }
}
