using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ReturnToMainScene : MonoBehaviour
{
    [Header("Input")]
    public InputActionProperty submitAction;

    void Update()
    {
        if (submitAction.action != null &&
            submitAction.action.WasPressedThisFrame())
        {
            LoadMainScene();
        }
    }

    public void LoadMainScene()
    {
        PlayerPrefs.SetInt("ReturnFromLetter", 1);

        Debug.Log("Retour vers SampleScene");

        SceneManager.LoadScene("SampleScene");
    }
}