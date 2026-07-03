using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.ScoreChanged;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Bubble;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.ScoreFX;
using CoreDomain.Scripts.Services.AudioService;
using CoreDomain.Scripts.Services.CommandFactory;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Commands.PopBubbleCommand
{
    public class PopBubbleCommand : BaseCommand, ICommandVoid
    {
        private IScoreFXController _scoreFXController;
        private ICommandFactory _commandFactory;
        private IAudioService _audioService;
        private IBubblesController _bubblesController;
        
        private PopBubbleCommandData _commandData;
        
        public PopBubbleCommand SetData(PopBubbleCommandData data)
        {
            _commandData = data;
            return this;
        }

        public override void ResolveDependencies()
        {
            _scoreFXController = _diContainer.Resolve<IScoreFXController>();
            _commandFactory = _diContainer.Resolve<ICommandFactory>();
            _bubblesController = _diContainer.Resolve<IBubblesController>();
            _audioService = _diContainer.Resolve<IAudioService>();
        }

        public void Execute()
        {
            // NOTE: We don't invoke here the bubble pop effect because it's being handled by the particle system collision itself
            var popScore = _bubblesController.BubblesPopScore;
            _scoreFXController.ShowArrowJumpBurstFX(_commandData.Position, popScore);
            _commandFactory.CreateCommandVoid<ScoreChangedCommand>().SetData(new ScoreChangedCommandData(popScore)).Execute();
            _audioService.PlayAudio(AudioClipType.BubblePop, AudioChannelType.Fx);
        }
    }
}
