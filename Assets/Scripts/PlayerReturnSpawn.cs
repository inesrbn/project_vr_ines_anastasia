using UnityEngine;

public class PlayerReturnExterior : MonoBehaviour
{
    public Transform spawnPoint; // Spawn de la scène exterieur

    void Awake()
    {
        // Vérifie si on revient d'une hallucination
        if (PlayerPrefs.GetInt("ReturnFromHallucination", 0) != 1)
            return;

        int spawnID = PlayerPrefs.GetInt("ReturnSpawnID", 0);

        // Seulement hallucination 2
        if (spawnID == 2 && spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
            Debug.Log("✅ Player téléporté au spawn H2");

            // 🔹 Désactive le trigger pour éviter qu'il se déclenche à nouveau
            GameObject crisisTrigger = GameObject.Find("CrisisTrigger"); // ou le nom exact
            if (crisisTrigger != null)
                crisisTrigger.SetActive(false);
        }
        else
        {
            Debug.LogWarning("❌ Spawn H2 non assigné ou mauvais ID");
        }

        // Nettoyage
        PlayerPrefs.DeleteKey("ReturnFromHallucination");
        PlayerPrefs.DeleteKey("ReturnSpawnID");
        PlayerPrefs.DeleteKey("WarCrisisID");
        PlayerPrefs.DeleteKey("WarSpawnIndex");
    }


}
