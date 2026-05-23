using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortal : MonoBehaviour
{
    public string sceneToLoad;             // Nom de la scène à charger
    public Vector3 returnPosition;         // Position de retour dans cette scène

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Sauvegarde de la position de retour
            PortalMemory.savedReturnPosition = returnPosition;
            PortalMemory.hasSavedPosition = true;

            // Changement de scène
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
