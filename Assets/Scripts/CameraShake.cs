using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public StressManager stressManager;
    public float maxShakeMagnitude = 0.05f; // réduit pour tremblement léger
    public float shakeSpeed = 10f;

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (stressManager == null) return;

        float stressPercent = stressManager.currentStress / stressManager.maxStress;

        if (stressPercent >= 0.5f)
        {
            // Tremblement à partir de 50%, va de 0 à maxShakeMagnitude
            float shakeAmount = maxShakeMagnitude * ((stressPercent - 0.5f) * 2); // 0 à 1
            Vector3 shakeOffset = Random.insideUnitSphere * shakeAmount;
            shakeOffset.z = 0; // pas de tremblement sur l'axe Z
            transform.localPosition = originalPosition + shakeOffset;
        }
        else
        {
            transform.localPosition = originalPosition;
        }
    }
}
