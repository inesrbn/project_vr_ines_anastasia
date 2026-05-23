using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class HallucinationPostProcess : MonoBehaviour
{
    public StressManager stressManager;

    private PostProcessVolume volume;

    private ChromaticAberration chromatic;
    private Bloom bloom;
    private Grain grain;
    private Vignette vignette;
    private ColorGrading colorGrading;

    void Start()
    {
        volume = GetComponent<PostProcessVolume>();

        volume.profile.TryGetSettings(out chromatic);
        volume.profile.TryGetSettings(out bloom);
        volume.profile.TryGetSettings(out grain);
        volume.profile.TryGetSettings(out vignette);
        volume.profile.TryGetSettings(out colorGrading);
    }

    void Update()
    {
        if (stressManager == null) return;

        float stressPercent =
            Mathf.Clamp01(stressManager.currentStress / stressManager.maxStress);

        // PHASES
        float phase1 = Mathf.InverseLerp(0.8f, 0.85f, stressPercent);
        float phase2 = Mathf.InverseLerp(0.85f, 0.9f, stressPercent);
        float phase3 = Mathf.InverseLerp(0.9f, 0.95f, stressPercent);
        float phase4 = Mathf.InverseLerp(0.95f, 1f, stressPercent);

        // Vision instable
        if (chromatic != null)
            chromatic.intensity.value =
                Mathf.Lerp(chromatic.intensity.value, 0.6f * phase1, Time.deltaTime);

        if (bloom != null)
            bloom.intensity.value =
                Mathf.Lerp(bloom.intensity.value, 2f * phase1, Time.deltaTime);

        // Couleurs
        if (colorGrading != null)
        {
            colorGrading.temperature.value =
                Mathf.Lerp(colorGrading.temperature.value, -30f * phase2, Time.deltaTime);

            colorGrading.saturation.value =
                Mathf.Lerp(colorGrading.saturation.value, -40f * phase2, Time.deltaTime);
        }

        // Grain
        if (grain != null)
            grain.intensity.value =
                Mathf.Lerp(grain.intensity.value, 0.7f * phase3, Time.deltaTime);

        // Vignette finale
        if (vignette != null)
            vignette.intensity.value =
                Mathf.Lerp(vignette.intensity.value, 0.5f * phase4, Time.deltaTime);
    }
}
