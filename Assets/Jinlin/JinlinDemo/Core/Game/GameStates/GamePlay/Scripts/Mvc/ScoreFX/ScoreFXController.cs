using System;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services.LevelCancellationToken;
using CoreDomain.Scripts.Helpers.Pools;
using CoreDomain.Scripts.Services.Logger.Base;
using UnityEngine;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.ScoreFX
{
    public class ScoreFXController : IScoreFXController
    {
        private readonly ILevelCancellationTokenService _levelCancellationTokenService;
        private readonly ScoreGainedFXPool _scoreGainedFXPool;

        public ScoreFXController(DiContainer diContainer, ScoreGainedFXView scoreGainedFXViewPrefab, ILevelCancellationTokenService levelCancellationTokenService)
        {
            _levelCancellationTokenService = levelCancellationTokenService;
            _scoreGainedFXPool = new ScoreGainedFXPool(new PoolData(10, 5), diContainer, scoreGainedFXViewPrefab);
        }

        public void InitEntryPoint()
        {
            _scoreGainedFXPool.InitPool();
        }
        
        public void ShowArrowJumpBurstFX(Vector3 position, int scoreGained)
        {
            _ = ShowScoreGainedFxAsync(position, scoreGained);
        }
        
        private async Awaitable ShowScoreGainedFxAsync(Vector3 position, int scoreGained)
        {
            ScoreGainedFXView scoreGainedFXView = null;
            try
            {
                scoreGainedFXView = _scoreGainedFXPool.Spawn();
                scoreGainedFXView.Setup(scoreGained, position);
                await scoreGainedFXView.DoShowAnimation(_levelCancellationTokenService.CancellationTokenSource);
            }
            catch (OperationCanceledException)
            {
                LogService.Log("Operation ShowScoreGainedFxAsync was cancelled");
            }
            catch (Exception e)
            {
                LogService.LogError(e.Message);
                throw;
            }
            finally
            {
                if (scoreGainedFXView != null)
                {
                    scoreGainedFXView.Despawn();
                }
            }
        }
    }
}
