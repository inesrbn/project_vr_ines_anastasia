using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public float typingSpeed = 0.05f;
    [TextArea(5, 10)]
    public string fullText = "De retour du front, un soldat tente de retrouver une vie normale. Mais ses souvenirs le hantent, refaisant surface sans prévenir.\r\nHallucinations, cauchemars, crises d’angoisse...\r\nCe qu’il a vécu continue de le poursuivre.\r\nPour espérer guérir, il devra faire face à ses démons,\r\nrevivre l’horreur… et tenter de la surmonter.";

    public AudioSource audioSource;
    public AudioClip typingSound;

    public TextMeshProUGUI pressEnterText;

    private string currentText = "";
    private TextMeshProUGUI textMeshPro;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();

        if (pressEnterText != null)
            pressEnterText.gameObject.SetActive(false);

        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        if (audioSource != null && typingSound != null)
        {
            audioSource.clip = typingSound;
            audioSource.loop = true;
            audioSource.Play();
        }

        foreach (char letter in fullText)
        {
            currentText += letter;
            textMeshPro.text = currentText;
            yield return new WaitForSeconds(typingSpeed);
        }

        if (audioSource != null)
            audioSource.Stop();

        if (pressEnterText != null)
        {
            pressEnterText.gameObject.SetActive(true);
            StartCoroutine(BlinkText(pressEnterText));
        }
    }

    IEnumerator BlinkText(TextMeshProUGUI text)
    {
        while (true)
        {
            text.enabled = !text.enabled;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
