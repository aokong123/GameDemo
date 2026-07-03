using System.Threading;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Track;
using CoreDomain.GameDomain.Scripts.Services.Levels;
using CoreDomain.Scripts.Services.AddressablesLoader;
using CoreDomain.Scripts.Services.Logger.Base;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Level
{
    public class LevelTrackController : ILevelTrackController
    {
        private readonly ILevelsDataService _levelsDataService;
        private readonly LevelFactory _levelFactory;
        
        private LevelTrackData _currentLevelTrackData;
        public  LevelTrackView CurrentLevelTrackView => _currentLevelTrackData.LevelTrackView;

        public LevelTrackController(IAddressablesLoaderService addressablesLoaderService, ILevelsDataService levelsDataService)
        {
            _levelsDataService = levelsDataService;
            _levelFactory = new LevelFactory(addressablesLoaderService);
        }
    
        public async Awaitable CreateLevelTrack(int levelNumber, CancellationTokenSource cancellationTokenSource)
        {
            var trackAddress = _levelsDataService.GetLevelData(levelNumber).TrackAddress;
            LogService.LogTopic($"Create level {levelNumber} track , track adress: {trackAddress}", LogTopicType.LevelTrack );
            _currentLevelTrackData = new LevelTrackData(await _levelFactory.CreateLevelTrack(trackAddress, cancellationTokenSource), trackAddress);
        }

        public void DestroyTrack(bool shouldReleaseFromMemory)
        {
            Object.Destroy(_currentLevelTrackData.LevelTrackView.gameObject);

            if (shouldReleaseFromMemory)
            {
                _levelFactory.ReleaseTrackFromMemory(_currentLevelTrackData.TrackAddress);
            }
        }
        
        private class LevelTrackData
        {
            public readonly LevelTrackView LevelTrackView;
            public readonly string TrackAddress;

            public LevelTrackData(LevelTrackView levelTrackView, string trackAddress)
            {
                LevelTrackView = levelTrackView;
                TrackAddress = trackAddress;
            }
        }
    }
}