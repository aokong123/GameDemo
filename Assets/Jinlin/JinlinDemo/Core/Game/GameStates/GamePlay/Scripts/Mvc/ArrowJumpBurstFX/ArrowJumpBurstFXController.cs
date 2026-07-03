using System;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services.LevelCancellationToken;
using CoreDomain.Scripts.Helpers.Pools;
using CoreDomain.Scripts.Services.Logger.Base;
using UnityEngine;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.ArrowJumpBurstFX
{
    public class ArrowJumpBurstFXController
    {
        private readonly ILevelCancellationTokenService _levelCancellationTokenService;
        private readonly ArrowJumpBurstFXPool _arrowJumpBurstFXPool;

        public ArrowJumpBurstFXController(DiContainer diContainer, ArrowJumpBurstFXView arrowJumpBurstFXViewPrefab, ILevelCancellationTokenService levelCancellationTokenService)
        {
            _levelCancellationTokenService = levelCancellationTokenService;
            _arrowJumpBurstFXPool = new ArrowJumpBurstFXPool(new PoolData(5, 2), diContainer, arrowJumpBurstFXViewPrefab);
        }

        public void InitEntryPoint()
        {
            _arrowJumpBurstFXPool.InitPool();
        }
        
        public void ShowArrowJumpBurstFx(Vector3 position, Quaternion rotation)
        {
            _ = ShowArrowJumpBurstFxViewAsync(position, rotation);
        }
        
        private async Awaitable ShowArrowJumpBurstFxViewAsync(Vector3 position, Quaternion rotation)
        {
            ArrowJumpBurstFXView arrowJumpBurstFXView = null;
            try
            { 
                arrowJumpBurstFXView = _arrowJumpBurstFXPool.Spawn();
                arrowJumpBurstFXView.Setup(position, rotation);
                await arrowJumpBurstFXView.DoShowAnimation(_levelCancellationTokenService.CancellationTokenSource);
            }
            catch (OperationCanceledException)
            {
                LogService.Log("Operation ShowArrowJumpBurstFXViewAsync was cancelled");
            }
            catch (Exception e)
            {
                LogService.LogError(e.Message);
                throw;
            }
            finally
            {
                if (arrowJumpBurstFXView != null)
                {
                    arrowJumpBurstFXView.Despawn();
                }
            }
        }
    }
}
