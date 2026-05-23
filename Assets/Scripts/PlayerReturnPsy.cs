using UnityEngine;

public class PlayerReturnPsy : MonoBehaviour
{
    public Transform spawnPoint2; // Spawn de la scène Psy

    void Start()
    {
        // Vérifie si on revient d'une hallucination
        if (PlayerPrefs.GetInt("ReturnFromHallucination", 0) != 1)
            return;

        int spawnID = PlayerPrefs.GetInt("ReturnSpawnID", 0);

        // Seulement hallucination 3
        if (spawnID == 3 && spawnPoint2 != null)
        {
            transform.position = spawnPoint2.position;
            transform.rotation = spawnPoint2.rotation;
            Debug.Log("✅ Player téléporté au spawn Psy après hallucination 3");

            // 🔹 Désactive le trigger pour éviter qu'il se déclenche à nouveau
            GameObject crisisTrigger = GameObject.Find("CrisisTrigger2"); // ou le nom exact
            if (crisisTrigger != null)
                crisisTrigger.SetActive(false);
        }
        else
        {
            Debug.LogWarning("❌ Spawn Psy 2 non assigné ou mauvais ID");
        }

        // Nettoyage
        PlayerPrefs.DeleteKey("ReturnFromHallucination");
        PlayerPrefs.DeleteKey("ReturnSpawnID");
        PlayerPrefs.DeleteKey("WarCrisisID");
        PlayerPrefs.DeleteKey("WarSpawnIndex");
    }
}
