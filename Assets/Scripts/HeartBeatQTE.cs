using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HeartbeatQTE : MonoBehaviour
{
    [Header("REFERENCES")]
    public StressManager stressManager;

    [Header("INPUT XR")]
    public InputActionProperty submitAction; // 👉 remplace Space

    [Header("QTE SETTINGS")]
    public float decreasePerPress = 30f;
    public string returnScene = "SampleScene";

    [Header("RHYTHM UI")]
    public TextMeshProUGUI rhythmIndicator;
    public float beatInterval = 1f;
    public float inputWindow = 0.5f;

    private float beatTimer = 0f;
    private bool canPress = false;

    [Header("DELAY BEFORE START")]
    public float delayBeforeQTE = 4f;
    private float delayTimer = 0f;
    private bool qteActive = false;

    [Header("VISUALS")]
    public Image redFlashImage;

    [Header("THOUGHTS")]
    public TextMeshProUGUI thoughtText;
    public float thoughtShowDuration = 2f;
    public float thoughtPauseDuration = 3f;

    void Start()
    {
        if (rhythmIndicator != null)
            rhythmIndicator.text = "";

        if (redFlashImage != null)
            redFlashImage.enabled = false;

        if (stressManager != null)
            stressManager.currentStress = stressManager.maxStress;

        if (thoughtText != null)
            StartCoroutine(ThoughtLoop());
    }

    void Update()
    {
        // ⏳ DELAY AVANT QTE
        if (!qteActive)
        {
            delayTimer += Time.deltaTime;

            if (delayTimer >= delayBeforeQTE)
                qteActive = true;

            return;
        }

        // 📈 STRESS MONTE LÉGÈREMENT
        if (stressManager != null)
        {
            stressManager.currentStress += 0.5f * Time.deltaTime;
            stressManager.currentStress = Mathf.Clamp(
                stressManager.currentStress,
                0,
                stressManager.maxStress
            );
        }

        // 🫀 BEAT SYSTEM
        beatTimer += Time.deltaTime;

        if (beatTimer >= beatInterval)
        {
            canPress = true;
            beatTimer = 0f;

            beatInterval = Random.Range(0.3f, 1f);

            if (rhythmIndicator != null)
                rhythmIndicator.text = "APPUIE !";

            if (redFlashImage != null)
                redFlashImage.enabled = true;

            Invoke(nameof(HideIndicator), inputWindow);
        }

        // 🎮 INPUT XR (SUBMIT ACTION)
        if (submitAction.action != null &&
            submitAction.action.WasPressedThisFrame() &&
            canPress)
        {
            stressManager.currentStress -= decreasePerPress;
            stressManager.currentStress = Mathf.Clamp(
                stressManager.currentStress,
                0,
                stressManager.maxStress
            );

            canPress = false;

            if (stressManager.currentStress <= 0)
            {
                PlayerPrefs.SetInt("ReturnFromWar", 1);
                SceneManager.LoadScene(returnScene);
            }
        }
    }

    void HideIndicator()
    {
        canPress = false;

        if (rhythmIndicator != null)
            rhythmIndicator.text = "";

        if (redFlashImage != null)
            redFlashImage.enabled = false;
    }

    private IEnumerator ThoughtLoop()
    {
        while (true)
        {
            if (thoughtText != null)
            {
                thoughtText.text = "Oh non, je ne veux pas revivre ça...";
                thoughtText.gameObject.SetActive(true);

                yield return new WaitForSeconds(thoughtShowDuration);

                thoughtText.gameObject.SetActive(false);

                yield return new WaitForSeconds(thoughtPauseDuration);
            }
            else
            {
                yield return null;
            }
        }
    }
}