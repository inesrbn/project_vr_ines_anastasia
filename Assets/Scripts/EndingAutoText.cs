using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndingAutoText : MonoBehaviour
{
    public Text dialogueUI;

    [TextArea(3, 10)]
    public string[] endingLines;

    public float typingSpeed = 0.04f;
    public float delayBetweenLines = 1.8f;
    public float delayBeforeStart = 1f;

    void Start()
    {
        dialogueUI.text = "";
        StartCoroutine(PlayEnding());
    }

    IEnumerator PlayEnding()
    {
        yield return new WaitForSeconds(delayBeforeStart);

        foreach (string line in endingLines)
        {
            yield return StartCoroutine(TypeLine(line));
            yield return new WaitForSeconds(delayBetweenLines);
        }
    }

    IEnumerator TypeLine(string line)
    {
        dialogueUI.text = "";

        foreach (char c in line)
        {
            dialogueUI.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
