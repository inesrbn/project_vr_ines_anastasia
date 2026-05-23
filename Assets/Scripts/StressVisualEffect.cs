using UnityEngine;
using UnityEngine.UI;

public class StressVisualEffect : MonoBehaviour
{
    public StressManager stressManager; // Référence à ton StressManager
    private Image overlayImage;

    public float maxAlpha = 0.5f;        // Alpha max quand stress = 100
    public float pulseSpeed = 2f;        // Vitesse du clignotement

    private void Start()
    {
        overlayImage = GetComponent<Image>();
    }

    void Update()
    {
        if (stressManager == null || overlayImage == null)
            return;

        float stressPercent = stressManager.currentStress / stressManager.maxStress;

        // Alpha varie entre 0 et maxAlpha selon le stress
        float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f; // oscillation 0 → 1
        float alpha = stressPercent * maxAlpha * pulse;

        Color color = overlayImage.color;
        color.a = alpha;
        overlayImage.color = color;
    }
}
