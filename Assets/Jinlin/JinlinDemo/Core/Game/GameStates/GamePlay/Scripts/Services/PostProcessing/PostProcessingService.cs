using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Services.PostProcessing
{
    public class PostProcessingService : IPostProcessingService
    {
        private readonly Vignette _vignette;
        private readonly ColorAdjustments _colorAdjustments;

        public PostProcessingService(Volume postProcessVolume)
        {
           if (postProcessVolume.profile.TryGet<Vignette>(out var vignette))
           {
               _vignette = vignette;
           }
           
           if (postProcessVolume.profile.TryGet<ColorAdjustments>(out var colorAdjustments))
           {
               _colorAdjustments = colorAdjustments;
           }
        }

        public void SetAreAllPostProcessingActive(bool isActive)
        {
           SetIsColorAdjustmentsActive(isActive);
           SetIsVignetteActive(isActive);
        }

        private  void SetIsColorAdjustmentsActive(bool isActive)
        {
            _colorAdjustments.active = isActive;
        }

        private void SetIsVignetteActive(bool isActive)
        {
            _vignette.active = isActive;
        }
    }
}
