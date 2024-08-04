using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessController : MonoBehaviour
{
    private PostProcessVolume volume;
    private Vignette vignette;
    void Start()
    {
        volume = gameObject.GetComponent<PostProcessVolume>();
        foreach (PostProcessEffectSettings item in volume.profile.settings)
        {
            if (item as Vignette) vignette = item as Vignette;
        }
        vignette.intensity.value = 0.0f;
    }

    public void SetVignetteIntensity(float value)
    {
        vignette.intensity.value = value;
    }
}
