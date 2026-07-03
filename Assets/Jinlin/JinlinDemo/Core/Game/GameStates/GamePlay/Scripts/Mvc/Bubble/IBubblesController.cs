namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Bubble
{
    public interface IBubblesController
    {
        int BubblesPopScore { get; }
        void SetupBubbles(BubblesView[] bubbles);
    }
}