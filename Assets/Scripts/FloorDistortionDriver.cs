using UnityEngine;

public class FloorDistortionDriver : MonoBehaviour
{
    [Header("References")]
    public StressManager stressManager;     // ton script de stress
    public Renderer floorRenderer;          // le Renderer du sol (Mesh9)

    [Header("Distortion Settings")]
    [Range(0f, 1f)]
    public float startAt = 0.8f;            // commence à 80% de stress

    public float maxDistortion = 1f;        // intensité max envoyée au shader
    public float transitionSpeed = 1.5f;    // vitesse de montée/descente

    [Header("Optional")]
    public bool useCurve = true;            // rend la montée plus progressive

    private Material mat;
    private float current;

    void Awake()
    {
        if (floorRenderer != null)
        {
            // material = instance runtime (évite de modifier l'asset)
            mat = floorRenderer.material;
        }
    }

    void Update()
    {
        if (stressManager == null || mat == null) return;

        // 1) Stress normalisé 0..1
        float stress01 = Mathf.Clamp01(stressManager.currentStress / stressManager.maxStress);

        // 2) Déclenchement tardif : avant startAt => 0, après => 0..1
        float late01 = Mathf.InverseLerp(startAt, 1f, stress01);

        // 3) Optionnel : courbe pour que ça démarre doucement puis accélère
        if (useCurve)
            late01 = late01 * late01;

        // 4) Appliquer au shader avec transition douce
        float target = late01 * maxDistortion;
        current = Mathf.Lerp(current, target, Time.deltaTime * transitionSpeed);

        mat.SetFloat("_DistortionIntensity", current);
    }
}
