using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManagerDebut : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;

    [Range(1f, 10f)]
    public float fadeDuration = 3f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        float t = 0f;

        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;

        fadeImage.raycastTarget = false;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            color.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            fadeImage.color = color;

            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
    }
}