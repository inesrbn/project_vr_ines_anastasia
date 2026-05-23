using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class VisionStressFX : MonoBehaviour
{
    [Header("References")]
    public PostProcessVolume volume;
    public Transform cameraTransform;

    [Header("Fade")]
    public float fadeIn = 0.2f;
    public float fadeOut = 0.35f;

    [Range(0f, 1f)] public float maxTrouble = 1f;

    // Supprimé : DepthOfField et Grain (Trop lourds pour VR)
    ChromaticAberration ca;
    Vignette vignette;
    ColorGrading cg;

    float baseTemp, baseSat, baseContrast, baseVignette;
    float current, target;
    Vector3 baseLocalPos;
    Quaternion baseLocalRot;

    [Header("Stress Effects")]
    public float vignetteAdd = 0.25f;
    public float chromaMax = 0.14f; // Garde ça bas pour éviter la cinétose

    [Header("Color Shift")]
    public float coldTempDelta = -25f;
    public float satDelta = -12f;
    public float contrastDelta = +12f;

    [Header("Disorientation")]
    public float swayPosition = 0.03f; // Réduit pour la VR
    public float swayRotation = 1.2f;  // Réduit pour la VR
    public float swaySpeed = 2.5f;

    void Awake()
    {
        if (!cameraTransform) cameraTransform = transform;
        baseLocalPos = cameraTransform.localPosition;
        baseLocalRot = cameraTransform.localRotation;

        if (volume != null && volume.profile != null)
        {
            volume.profile.TryGetSettings(out ca);
            volume.profile.TryGetSettings(out vignette);
            volume.profile.TryGetSettings(out cg);
        }

        if (cg != null)
        {
            baseTemp = cg.temperature.value;
            baseSat = cg.saturation.value;
            baseContrast = cg.contrast.value;
        }
        if (vignette != null) baseVignette = vignette.intensity.value;

        current = 0f; target = 0f;
        Apply(0f);
    }

    void Update()
    {
        float speed = (target > current) ? (1f / fadeIn) : (1f / fadeOut);
        current = Mathf.MoveTowards(current, target, Time.deltaTime * speed);
        float t = current * maxTrouble;

        Apply(t);
        ApplySway(t);
    }

    public void StartStress(float intensity = 1f) => target = intensity;
    public void StopStress() => target = 0f;

    void Apply(float t)
    {
        bool active = t > 0.01f;

        // On active/désactive les effets seulement si nécessaire
        if (ca != null)
        {
            ca.enabled.value = active;
            ca.intensity.value = Mathf.Lerp(0f, chromaMax, t);
        }

        if (vignette != null)
        {
            vignette.enabled.value = true; // Toujours utile en VR pour réduire le malaise
            vignette.intensity.value = baseVignette + (vignetteAdd * t);
        }

        if (cg != null)
        {
            cg.temperature.value = baseTemp + coldTempDelta * t;
            cg.saturation.value = baseSat + satDelta * t;
            cg.contrast.value = baseContrast + contrastDelta * t;
        }

        if (!active) ResetCameraPose();
    }

    void ApplySway(float t)
    {
        if (t <= 0.01f) return;
        float time = Time.time;
        // Le sway en VR doit être TRES subtil sinon le joueur va tomber dans la vraie vie
        Vector3 posOffset = new Vector3(Mathf.Sin(time * swaySpeed) * swayPosition * t, Mathf.Sin(time * (swaySpeed * 0.8f)) * swayPosition * t, 0f);
        cameraTransform.localPosition = baseLocalPos + posOffset;
        cameraTransform.localRotation = baseLocalRot * Quaternion.Euler(0, 0, Mathf.Sin(time * swaySpeed) * swayRotation * t);
    }

    void ResetCameraPose()
    {
        cameraTransform.localPosition = baseLocalPos;
        cameraTransform.localRotation = baseLocalRot;
    }
}