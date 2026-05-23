using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string sceneName; // Le nom de la scène à charger

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ton perso doit avoir le tag "Player"
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}