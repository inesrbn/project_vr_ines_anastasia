using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToPsy : MonoBehaviour
{
    public string sceneToLoad = "PsyScene"; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
