using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.ScoreFX
{
    public interface IScoreFXController
    {
        void InitEntryPoint();
        void ShowArrowJumpBurstFX(Vector3 positon, int scoreGained);
    }
}