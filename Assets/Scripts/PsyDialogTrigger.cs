using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;

public class PsyDialogueTrigger : MonoBehaviour
{
    public VisionStressFX stressFX;

    public Text dialogueUI;          // Texte du dialogue
    public Text nextIndicator;       // Texte clignotant : "Appuie sur Entrée →"
    public Button yesButton;
    public Button noButton;
    public AudioClip voiceClip;

    public string[] dialogueLines;
    public float typingSpeed = 0.04f;

    private int currentLine = 0;
    private bool isTyping = false;
    private AudioSource audioSource;
    private bool hasSpoken = false;
    private Coroutine blinkCoroutine;

    public InputActionProperty submitAction;

    void Start()
    {
        dialogueUI.text = "";
        if (nextIndicator != null)
            nextIndicator.gameObject.SetActive(false);

        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (hasSpoken && !isTyping && submitAction.action.WasPressedThisFrame())
        {
            ShowNextLine();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger avec : " + other.name);
        if (!hasSpoken && other.CompareTag("Player"))
        {
            hasSpoken = true;
            StartCoroutine(TypeLine(dialogueLines[currentLine]));

            if (voiceClip != null && audioSource != null)
                audioSource.PlayOneShot(voiceClip);
        }
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueUI.text = "";

        if (nextIndicator != null)
            nextIndicator.gameObject.SetActive(false);

        foreach (char letter in line.ToCharArray())
        {
            dialogueUI.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        // Si ce n’est PAS la dernière ligne, afficher le "Appuie sur Entrée"
        if (currentLine < dialogueLines.Length - 1 && nextIndicator != null)
        {
            blinkCoroutine = StartCoroutine(BlinkNext());
        }
        // Si c’est la dernière ligne, afficher les boutons
        else
        {
            yesButton.gameObject.SetActive(true);
            noButton.gameObject.SetActive(true);

            if (stressFX != null)
                stressFX.StartStress(1f); // intensité (0.5f si tu veux plus léger)
        }
    }

    IEnumerator BlinkNext()
    {
        nextIndicator.gameObject.SetActive(true);
        while (!isTyping)
        {
            nextIndicator.text = "Appuie sur Entrée →";
            yield return new WaitForSeconds(0.5f);
            nextIndicator.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    void ShowNextLine()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            if (nextIndicator != null)
                nextIndicator.gameObject.SetActive(false);
        }

        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            StartCoroutine(TypeLine(dialogueLines[currentLine]));
        }
    }

    public void OnChoiceMade()
    {
        if (stressFX != null)
            stressFX.StopStress();
    }

    void OnEnable()
    {
        submitAction.action?.Enable();
    }

    void OnDisable()
    {
        submitAction.action?.Disable();
    }

}