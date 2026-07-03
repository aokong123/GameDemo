using System.Threading;
using CoreDomain.GameDomain.Scripts.Services.Levels.ScriptableObjects;
using CoreDomain.Scripts.Services.AddressablesLoader;
using CoreDomain.Scripts.Services.DataPersistence;
using CoreDomain.Scripts.Services.Logger.Base;
using UnityEngine;

namespace CoreDomain.GameDomain.Scripts.Services.Levels
{
    public class LevelsDataService : ILevelsDataService
    {
        private const string LEVELS_ASSET_ADRESS = "LevelsSettings";
        private const string MAX_LEVEL_NUMBER_REACHED_SAVE_KEY = "MaxLevelReachedNumber";

        public int MaxLevelNumberReached => _maxLevelNumberReached;
        
        private readonly IAddressablesLoaderService _addressablesLoaderService;
        private readonly IDataPersistence _dataPersistence;
        
        private LevelsData _levelsData;
        private int _maxLevelNumberReached;

        public LevelsDataService(IAddressablesLoaderService addressablesLoaderService, IDataPersistence dataPersistence)
        {
            _addressablesLoaderService = addressablesLoaderService;
            _dataPersistence = dataPersistence;
        }

        public async Awaitable LoadLevelsData(CancellationTokenSource cancellationTokenSource)
        {
            _levelsData = await _addressablesLoaderService.LoadAsync<LevelsData>(LEVELS_ASSET_ADRESS, cancellationTokenSource);
            _maxLevelNumberReached = _dataPersistence.Load(MAX_LEVEL_NUMBER_REACHED_SAVE_KEY, 1);
        }

        public LevelData[] GetAllLevelsData()
        {
            return _levelsData.LevelsByOrder;
        }

        public void SetLastSavedLevel(int levelNumber)
        {
            LogService.LogTopic($"Set last saved level to {levelNumber}", LogTopicType.LevelsData );
            _dataPersistence.Save(MAX_LEVEL_NUMBER_REACHED_SAVE_KEY, levelNumber);
            _maxLevelNumberReached = levelNumber;
        }

        public int GetLevelsAmount()
        {
            return _levelsData.LevelsByOrder.Length;
        }

        public LevelData GetLevelData(int levelNumber)
        {
            return _levelsData.LevelsByOrder[levelNumber - 1];
        }
    }
}