using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ButtonTrigger : MonoBehaviour
{
    public GameObject buttonUI;
    public InputActionProperty submitAction;

    private bool playerInZone = false;

    void OnEnable()
    {
        if (submitAction.action != null)
            submitAction.action.Enable();
    }

    void OnDisable()
    {
        if (submitAction.action != null)
            submitAction.action.Disable();
    }

    void Start()
    {
        if (buttonUI != null)
            buttonUI.SetActive(false);
    }

    void Update()
    {
        if (!playerInZone)
            return;

        if (submitAction.action != null &&
            submitAction.action.WasPressedThisFrame())
        {
            Debug.Log("BUTTON TRIGGER");
            LoadLettreScene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;

            if (buttonUI != null)
                buttonUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;

            if (buttonUI != null)
                buttonUI.SetActive(false);
        }
    }

    public void LoadLettreScene()
    {
        SceneManager.LoadScene("LettreScene");
    }
}