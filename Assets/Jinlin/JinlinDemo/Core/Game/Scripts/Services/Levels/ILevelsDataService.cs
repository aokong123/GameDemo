
using System.Threading;
using CoreDomain.GameDomain.Scripts.Services.Levels.ScriptableObjects;
using UnityEngine;

namespace CoreDomain.GameDomain.Scripts.Services.Levels
{
    public interface ILevelsDataService
    {
        int MaxLevelNumberReached { get; }
        Awaitable LoadLevelsData(CancellationTokenSource cancellationTokenSource);
        int GetLevelsAmount();
        LevelData GetLevelData(int levelNumber);
        LevelData[] GetAllLevelsData();
        void SetLastSavedLevel(int levelNumber);
    }
}