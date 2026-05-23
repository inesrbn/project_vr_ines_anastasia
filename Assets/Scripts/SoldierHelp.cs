using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SoldierHelp : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI helpText;

    [Header("SETTINGS")]
    public float helpDuration = 4f;
    public float failTextDuration = 2f;

    [Header("AUDIO")]
    public AudioSource soldierAudio;

    [Header("INPUT XR")]
    public InputActionProperty submitAction; // 👉 remplace E

    private bool isHelping = false;
    private bool playerInZone = false;
    private float timer = 0f;
    private Animator playerAnim;

    void Start()
    {
        this.enabled = false;
    }

    void Update()
    {
        if (!playerInZone) return;

        // 🎮 SUBMIT ACTION (VR / XR)
        if (!isHelping &&
            submitAction.action != null &&
            submitAction.action.WasPressedThisFrame())
        {
            StartHelping();
        }

        if (isHelping)
        {
            timer += Time.deltaTime;

            if (timer >= helpDuration)
            {
                StopHelping();
            }
        }
    }

    void StartHelping()
    {
        isHelping = true;
        timer = 0f;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            var move = player.GetComponent<PlayerMovementRotation>();
            if (move != null) move.enabled = false;

            playerAnim = player.GetComponent<Animator>();
            if (playerAnim != null)
                playerAnim.Play("CPR");
        }

        if (helpText != null)
            helpText.text = "Réanimation en cours...";

        if (soldierAudio != null)
            soldierAudio.Play();
    }

    void StopHelping()
    {
        isHelping = false;

        if (playerAnim != null)
            playerAnim.Play("Idle");

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            var move = player.GetComponent<PlayerMovementRotation>();
            if (move != null) move.enabled = true;
        }

        if (helpText != null)
            helpText.text = "Vous n'avez pas réussi à réanimer le soldat !";

        Invoke(nameof(EndCrisis), failTextDuration);
    }

    void EndCrisis()
    {
        PlayerPrefs.SetInt("ReturnSpawnID", 2);
        PlayerPrefs.SetInt("ReturnFromHallucination", 1);
        SceneManager.LoadScene("Exterieur_Scene");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            this.enabled = true;

            if (helpText != null)
                helpText.text = "Appuie sur le bouton (Submit)";
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;

            if (helpText != null)
                helpText.text = "";

            this.enabled = false;
        }
    }
}