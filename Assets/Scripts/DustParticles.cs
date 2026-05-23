using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DustParticles : MonoBehaviour
{
    [Range(0f, 1f)] public float intensity = 0.3f;
    public float transitionSpeed = 2f;

    ParticleSystem ps;
    ParticleSystem.EmissionModule em;

    float baseRate;
    float target;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        em = ps.emission;

        // On lit le rate actuel, peu importe le mode (constant/curve)
        baseRate = em.rateOverTimeMultiplier;
        if (baseRate <= 0f) baseRate = 10f; // sécurité

        target = intensity;
    }

    void Update()
    {
        intensity = Mathf.Lerp(intensity, target, Time.deltaTime * transitionSpeed);
        em.rateOverTimeMultiplier = baseRate * intensity;

        if (!ps.isPlaying) ps.Play();
    }

    public void SetIntensity(float v) => target = Mathf.Clamp01(v);
}