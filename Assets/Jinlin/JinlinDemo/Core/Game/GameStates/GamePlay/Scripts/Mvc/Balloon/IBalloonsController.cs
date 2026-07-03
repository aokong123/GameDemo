namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Balloon
{
    public interface IBalloonsController
    {
        int BalloonPopScore { get; }
        void SetupBalloons(BalloonView[] balloonViews);
    }
}