using UnityEngine;
using TMPro;
using System.Collections;

public class ThoughtBubble : MonoBehaviour
{
    public TextMeshProUGUI thoughtText;
    public float displayTime = 5f;

    private Coroutine currentRoutine;

    void Start()
    {
        if (thoughtText != null)
            thoughtText.gameObject.SetActive(false);
    }

    public void ShowThought(string message)
    {
        if (thoughtText == null) return;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowThoughtCoroutine(message));
    }

    private IEnumerator ShowThoughtCoroutine(string message)
    {
        thoughtText.text = message;
        thoughtText.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        thoughtText.gameObject.SetActive(false);
    }

    public void HideThought()
    {
        if (thoughtText != null)
            thoughtText.gameObject.SetActive(false);
    }
}