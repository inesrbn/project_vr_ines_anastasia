using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class StressVisuals : MonoBehaviour
{
    public StressManager stressManager;
    public PostProcessVolume volume;

    private ChromaticAberration chroma;
    private Vignette vignette;
    private DepthOfField dof;

    void OnEnable()
    {
        if (volume != null && volume.profile != null)
        {
            volume.profile.TryGetSettings(out chroma);
            volume.profile.TryGetSettings(out vignette);
            volume.profile.TryGetSettings(out dof);
        }
    }

    void Update()
    {
        if (stressManager == null) return;

        float stress01 = stressManager.currentStress / stressManager.maxStress;

        if (chroma != null)
            chroma.intensity.value = Mathf.Lerp(0f, 0.4f, stress01);

        if (vignette != null)
            vignette.intensity.value = Mathf.Lerp(0.15f, 0.45f, stress01);

        if (dof != null)
            dof.focusDistance.value = Mathf.Lerp(10f, 2f, stress01);
    }
}
