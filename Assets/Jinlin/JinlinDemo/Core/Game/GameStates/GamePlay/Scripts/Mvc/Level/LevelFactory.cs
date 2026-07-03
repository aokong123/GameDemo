using System.Threading;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Track;
using CoreDomain.Scripts.Services.AddressablesLoader;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Mvc.Level
{
   public class LevelFactory
   {
      private readonly IAddressablesLoaderService _addressablesLoaderService;

      public LevelFactory(IAddressablesLoaderService addressablesLoaderService)
      {
         _addressablesLoaderService = addressablesLoaderService;
      }

      public async Awaitable<LevelTrackView> CreateLevelTrack(string trackAddress, CancellationTokenSource cancellationTokenSource)
      {
         var levelTrack = (await _addressablesLoaderService.LoadAsync<LevelTrackView>(trackAddress, cancellationTokenSource));
         return Object.Instantiate(levelTrack);
      }

      public void ReleaseTrackFromMemory(string trackAddress)
      {
         _addressablesLoaderService.Release(trackAddress);
      }
   }
}
