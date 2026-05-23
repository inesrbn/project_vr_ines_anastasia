using UnityEngine;
using System.Collections;

public class CityStressEvent : MonoBehaviour
{
    [Header("--- AUDIO ---")]
    public AudioSource cityTrafficAudio;
    public AudioSource normalCityAudio;
    public float stressSoundVolume = 1f;
    public float volumeIncreaseSpeed = 3f;

    [Header("--- STRESS LINK ---")]
    public StressManager stressManager;

    private bool eventTriggered = false;

    void Start()
    {
        if (PlayerPrefs.GetInt("ReturnedFromHallucination2", 0) == 1)
        {
            if (normalCityAudio != null) normalCityAudio.Play();
            this.enabled = false;
            return;
        }

        if (normalCityAudio != null) normalCityAudio.Play();
        Invoke(nameof(TriggerStressEvent), 10f);
    }

    public void TriggerStressEvent()
    {
        if (eventTriggered) return;
        eventTriggered = true;
        StartCoroutine(StressSequence());
    }

    private IEnumerator StressSequence()
    {
        if (normalCityAudio != null) normalCityAudio.Stop();
        if (cityTrafficAudio != null)
        {
            cityTrafficAudio.volume = 0f;
            cityTrafficAudio.Play();
        }

        float elapsed = 0f;
        float duration = 4f; // Temps pour monter le stress

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (cityTrafficAudio != null)
                cityTrafficAudio.volume += volumeIncreaseSpeed * Time.deltaTime;

            if (stressManager != null)
            {
                // On fait monter le stress. Quand il arrivera à 100, 
                // le StressManager.Update() changera de scène automatiquement.
                stressManager.currentStress += (100f / duration) * Time.deltaTime;
            }
            yield return null;
        }
    }
}