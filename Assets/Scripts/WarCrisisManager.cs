using UnityEngine;

public class WarCrisisManager : MonoBehaviour
{
    [Header("--- CRISES (GAMEPLAY) ---")]
    public GameObject crisis1QTE;          // Gameplay crise 1
    public GameObject crisis2Soldier;      // Gameplay crise 2
    public GameObject crisis3NearDeath;    // Gameplay crise 3

    [Header("--- CANVAS UI (PENSÉES / UI) ---")]
    public GameObject canvasCrisis1;       // Canvas UI hallucination 1
    public GameObject canvasCrisis2;       // Canvas UI hallucination 2
    public GameObject canvasCrisis3;       // Canvas UI hallucination 3

    void Start()
    {
        // 🔴 Au départ : tout est désactivé
        DisableAllCrises();
        DisableAllCanvases();
    }

    public void ActivateCrisis(int crisisID)
    {
        // 🔴 Toujours tout désactiver avant
        DisableAllCrises();
        DisableAllCanvases();

        switch (crisisID)
        {
            case 1:
                if (crisis1QTE) crisis1QTE.SetActive(true);
                if (canvasCrisis1) canvasCrisis1.SetActive(true);
                Debug.Log("Crise 1 activée");
                break;

            case 2:
                if (crisis2Soldier) crisis2Soldier.SetActive(true);
                if (canvasCrisis2) canvasCrisis2.SetActive(true);
                Debug.Log("Crise 2 activée");
                break;

            case 3:
                if (crisis3NearDeath) crisis3NearDeath.SetActive(true);
                if (canvasCrisis3) canvasCrisis3.SetActive(true);
                Debug.Log("Crise 3 activée");
                break;

            default:
                Debug.Log("Pas de crise définie");
                break;
        }
    }

    void DisableAllCrises()
    {
        if (crisis1QTE) crisis1QTE.SetActive(false);
        if (crisis2Soldier) crisis2Soldier.SetActive(false);
        if (crisis3NearDeath) crisis3NearDeath.SetActive(false);
    }

    void DisableAllCanvases()
    {
        if (canvasCrisis1) canvasCrisis1.SetActive(false);
        if (canvasCrisis2) canvasCrisis2.SetActive(false);
        if (canvasCrisis3) canvasCrisis3.SetActive(false);
    }
}
