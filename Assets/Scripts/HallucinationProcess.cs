using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class HallucinationProcess : MonoBehaviour
{
    public StressManager stressManager;

    private PostProcessVolume volume;

    private ChromaticAberration chromatic;
    private Bloom bloom;
    private Grain grain;
    private Vignette vignette;
    private ColorGrading colorGrading;

    [Header("Réglages max")]
    public float maxChromatic = 0.6f;
    public float maxBloom = 2f;
    public float maxGrain = 0.7f;
    public float maxVignette = 0.5f;
    public float maxTempShift = -30f;
    public float maxSaturationShift = -40f;

    public float reactionSpeed = 5f;

    void Start()
    {
        volume = GetComponent<PostProcessVolume>();

        if (volume == null)
        {
            Debug.LogError("PostProcessVolume manquant !");
            enabled = false;
            return;
        }

        volume.profile.TryGetSettings(out chromatic);
        volume.profile.TryGetSettings(out bloom);
        volume.profile.TryGetSettings(out grain);
        volume.profile.TryGetSettings(out vignette);
        volume.profile.TryGetSettings(out colorGrading);
    }

    void Update()
    {
        if (stressManager == null) return;

        float s = Mathf.Clamp01(stressManager.currentStress / stressManager.maxStress);

        // Définition des phases plus larges pour qu'on les voie
        float phase1 = Mathf.InverseLerp(0.6f, 0.75f, s);
        float phase2 = Mathf.InverseLerp(0.75f, 0.85f, s);
        float phase3 = Mathf.InverseLerp(0.85f, 0.95f, s);
        float phase4 = Mathf.InverseLerp(0.95f, 1f, s);

        // Vision instable
        if (chromatic != null)
            chromatic.intensity.value = Mathf.MoveTowards(chromatic.intensity.value, maxChromatic * phase1, reactionSpeed * Time.deltaTime);

        if (bloom != null)
            bloom.intensity.value = Mathf.MoveTowards(bloom.intensity.value, maxBloom * phase1, reactionSpeed * Time.deltaTime);

        // Couleurs
        if (colorGrading != null)
        {
            colorGrading.temperature.value = Mathf.MoveTowards(colorGrading.temperature.value, maxTempShift * phase2, reactionSpeed * 50f * Time.deltaTime);
            colorGrading.saturation.value = Mathf.MoveTowards(colorGrading.saturation.value, maxSaturationShift * phase2, reactionSpeed * 50f * Time.deltaTime);
        }

        // Grain
        if (grain != null)
            grain.intensity.value = Mathf.MoveTowards(grain.intensity.value, maxGrain * phase3, reactionSpeed * Time.deltaTime);

        // Vignette
        if (vignette != null)
            vignette.intensity.value = Mathf.MoveTowards(vignette.intensity.value, maxVignette * phase4, reactionSpeed * Time.deltaTime);
    }
}
