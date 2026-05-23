using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToCity : MonoBehaviour
{
    public string sceneToLoad = "Exterieur_Scene";
    public Transform player; // à assigner dans l'inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
