using System.Threading;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Track;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Level
{
    public interface ILevelTrackController
    {
        Awaitable CreateLevelTrack(int levelNumber, CancellationTokenSource cancellationTokenSource);
        void DestroyTrack(bool shouldReleaseFromMemory);
        LevelTrackView CurrentLevelTrackView { get; }
    }
}