using UnityEngine;
using System.Collections;

public class CityToHallucination3 : MonoBehaviour
{
    [Header("--- AUDIO ---")]
    public AudioSource doorAudio;

    [Header("--- STRESS ---")]
    public StressManager stressManager;
    public float stressIncreaseRate = 20f; // J'ai augmenté la vitesse pour être sûr d'atteindre 99 vite
    public float stressDuration = 5f;

    private bool eventTriggered = false;

    void Start()
    {
        // --- VERIFICATION CRITIQUE ---
        // Si le joueur revient de l'hallucination (valeur 1), on détruit ce script ou on l'arrête.
        if (PlayerPrefs.GetInt("ReturnedFromHallucination3", 0) == 1)
        {
            Debug.Log("Script Porte désactivé : Le joueur est déjà revenu de l'hallucination.");

            if (doorAudio != null) doorAudio.Stop();

            // Désactive le composant pour qu'il ne fasse rien
            this.enabled = false;
            return;
        }

        StartCoroutine(DelayedTrigger());
    }

    IEnumerator DelayedTrigger()
    {
        yield return new WaitForSeconds(0.8f);
        TriggerStressEvent();
    }

    void TriggerStressEvent()
    {
        if (eventTriggered) return;
        eventTriggered = true;

        StartCoroutine(StressAfterDoor());
    }

    IEnumerator StressAfterDoor()
    {
        if (doorAudio != null)
        {
            doorAudio.Play();
        }

        float elapsed = 0f;
        while (elapsed < stressDuration)
        {
            elapsed += Time.deltaTime;

            if (stressManager != null)
            {
                // On ajoute du stress manuellement
                stressManager.currentStress += stressIncreaseRate * Time.deltaTime;
            }

            yield return null;
        }
    }
}