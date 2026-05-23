using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class IntroManager : MonoBehaviour
{
    public string nextSceneName;
    public InputActionProperty submitAction;

    void Update()
    {
        if (submitAction.action == null)
        {
            Debug.Log("Action NULL");
            return;
        }

        if (submitAction.action.WasPressedThisFrame())
        {
            Debug.Log("INPUT DETECTÉ");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}