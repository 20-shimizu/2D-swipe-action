using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessController : MonoBehaviour
{
    private Volume volume;
    private Vignette vignette;
    private Bloom bloom;
    void Start()
    {
        volume = gameObject.GetComponent<Volume>();
        if (volume.profile.TryGet(out Vignette vig)) this.vignette = vig;
        if (volume.profile.TryGet(out Bloom blm)) this.bloom = blm;
        vignette.intensity.value = 0.0f;
    }

    public void SetVignetteIntensity(float value)
    {
        vignette.intensity.value = value;
    }
    public void SetBloomIntensity(float value)
    {
        bloom.intensity.value = value;
    }
}
