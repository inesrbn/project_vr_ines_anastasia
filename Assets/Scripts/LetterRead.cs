using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneLettreTrigger : MonoBehaviour
{
    public string sceneLettre = "LettreScene";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneLettre);
        }
    }
}

