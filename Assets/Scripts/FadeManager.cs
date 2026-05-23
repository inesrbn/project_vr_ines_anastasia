using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public Image fadeImage;

    public float fadeOutDuration = 3f;
    public float fadeInDuration = 2f;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    // =========================
    // FADE IN
    // =========================
    public IEnumerator FadeIn()
    {
        float t = 0f;

        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;

        while (t < fadeInDuration)
        {
            t += Time.deltaTime;

            color.a = Mathf.Lerp(1f, 0f, t / fadeInDuration);

            fadeImage.color = color;

            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
    }

    // =========================
    // FADE OUT
    // =========================
    public IEnumerator FadeOut()
    {
        float t = 0f;

        Color color = fadeImage.color;

        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;

            color.a = Mathf.Lerp(0f, 1f, t / fadeOutDuration);

            fadeImage.color = color;

            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
    }

    // =========================
    // LOAD SCENE
    // =========================
    public IEnumerator FadeAndLoadScene(string sceneName)
    {
        // Fade vers noir
        yield return StartCoroutine(FadeOut());

        // écran noir complet
        yield return new WaitForSeconds(1f);

        // charge scène
        SceneManager.LoadScene(sceneName);

        // attendre 1 frame
        yield return null;

        // fade retour
        yield return StartCoroutine(FadeIn());
    }
}