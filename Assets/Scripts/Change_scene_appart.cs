using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToAppartment : MonoBehaviour
{
    public string sceneToLoad = "SampleScene";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}

