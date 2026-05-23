using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneChanger1 : MonoBehaviour
{
    public string sceneYes = "SampleSceneHappy";
    public string sceneNo = "SampleScene";

    public InputActionProperty yesAction; // A
    public InputActionProperty noAction;  // B

    void Update()
    {
        if (yesAction.action != null &&
            yesAction.action.WasPressedThisFrame())
        {
            LoadYes();
        }

        if (noAction.action != null &&
            noAction.action.WasPressedThisFrame())
        {
            LoadNo();
        }
    }

    public void LoadYes()
    {
        PlayerPrefs.SetInt("CHOICE_YES", 1);
        SceneManager.LoadScene(sceneYes);
    }

    public void LoadNo()
    {
        PlayerPrefs.SetInt("CHOICE_YES", 0);
        SceneManager.LoadScene(sceneNo);
    }
}