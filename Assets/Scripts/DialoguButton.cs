using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueButtons : MonoBehaviour
{
    public Text dialogueUI;
    public Button yesButton;
    public Button noButton;
    public float letterDelay = 0.03f;

    private string[] dialogueLines = new string[]
    {
        "Bonjour. Je suis ravie de te voir aujourd’hui.",
        "J’ai ton dossier de suivi psychologique depuis ton retour de mission.",
        "Il y a des mentions d’hallucinations visuelles et auditives persistantes.",
        "Tu n’as pas à affronter tout ça seul.",
        "Veux-tu que je t’aide à traverser cette étape ?"
    };

    private int currentLine = 0;
    private bool isTyping = false;
    private bool canProceed = false;

    void Start()
    {
        dialogueUI.text = "";
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        StartCoroutine(TypeLine(dialogueLines[currentLine]));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !isTyping && canProceed)
        {
            currentLine++;

            if (currentLine < dialogueLines.Length)
            {
                StartCoroutine(TypeLine(dialogueLines[currentLine]));
            }
            else
            {
                yesButton.gameObject.SetActive(true);
                noButton.gameObject.SetActive(true);
            }
        }
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        canProceed = false;
        dialogueUI.text = "";

        foreach (char letter in line.ToCharArray())
        {
            dialogueUI.text += letter;
            yield return new WaitForSeconds(letterDelay);
        }

        isTyping = false;
        canProceed = true;
    }

    public void OnYesClicked()
    {
        dialogueUI.text = "Tu as répondu oui.";
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
    }

    public void OnNoClicked()
    {
        dialogueUI.text = "Tu as répondu non.";
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
    }
}
