using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StressManager : MonoBehaviour
{
    [Header("UI")]
    public Slider stressSlider;
    public Text stressBarTitle;

    [Header("Audio")]
    public AudioSource coffeeMachineSound;

    [Header("Stress Settings")]
    public bool stressIncreaseEnabled = true;

    public float maxStress = 100f;
    public float currentStress = 0f;

    public float baseStressGain = 1f;

    public float mediumNoiseThreshold = 0.2f;
    public float highNoiseThreshold = 0.5f;

    public float mediumStressGain = 5f;
    public float highStressGain = 10f;

    public float stressDecreaseSpeed = 3f;
    public float calmDownSpeed = 50f;

    [Header("Scenes")]
    [SerializeField] private string warSceneName = "WarScene";
    [SerializeField] private string citySceneName = "Exterieur_Scene";
    [SerializeField] private string psySceneName = "PsyScene";

    private bool isRelaxing = false;
    private bool isTriggered = false;
    private bool isHallucination3Active = false;
    private bool hallucinationTriggered = false;

    // =========================
    // START
    // =========================
    void Start()
    {
        int crisisID = PlayerPrefs.GetInt("WarCrisisID", 0);
        string currentScene = SceneManager.GetActiveScene().name;

        isHallucination3Active =
            (currentScene == warSceneName && crisisID == 3);

        // =========================
        // RETOUR HALLUCINATION 1
        // =========================
        if (PlayerPrefs.GetInt("ReturnFromHallucination", 0) == 1 &&
            currentScene != psySceneName &&
            currentScene != citySceneName)
        {
            currentStress = 0f;
        }

        // =========================
        // RETOUR HALLUCINATION 3
        // =========================
        if (currentScene == psySceneName &&
            PlayerPrefs.GetInt("ReturnedFromHallucination3", 0) == 1)
        {
            currentStress = 0f;

            baseStressGain = 0.05f;
            mediumStressGain = 0.1f;
            highStressGain = 0.2f;
        }

        // =========================
        // RETOUR HALLUCINATION 2
        // =========================
        if (currentScene == citySceneName &&
            PlayerPrefs.GetInt("ReturnedFromHallucination2", 0) == 1)
        {
            currentStress = 0f;

            stressIncreaseEnabled = false;

            Debug.Log("Retour Hallucination 2 : Stress désactivé");
        }

        // =========================
        // UI
        // =========================
        if (stressSlider != null)
        {
            stressSlider.maxValue = maxStress;
            stressSlider.value = currentStress;
        }
    }

    // =========================
    // UPDATE
    // =========================
    void Update()
    {
        float maxStressThisScene = maxStress;

        if (SceneManager.GetActiveScene().name == psySceneName)
            maxStressThisScene = 99f;

        // =========================
        // MODE HALLUCINATION 3
        // =========================
        if (isHallucination3Active)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                currentStress -= calmDownSpeed * Time.deltaTime;
            }

            currentStress =
                Mathf.Clamp(currentStress, 0f, maxStressThisScene);

            UpdateUI();

            // Retour scène psy
            if (currentStress <= 0f)
            {
                PlayerPrefs.SetInt("ReturnSpawnID", 3);
                PlayerPrefs.SetInt("ReturnFromHallucination", 1);
                PlayerPrefs.SetInt("ReturnedFromHallucination3", 1);

                PlayerPrefs.SetInt("WarCrisisID", 0);

                StartCoroutine(LoadSceneWithFade(psySceneName));
            }

            return;
        }

        // =========================
        // DECLENCHEMENT HALLU 3
        // =========================
        if (SceneManager.GetActiveScene().name == psySceneName &&
            !hallucinationTriggered &&
            currentStress >= 99f)
        {
            if (PlayerPrefs.GetInt("ReturnedFromHallucination3", 0) == 0)
            {
                TriggerHallucination3();
            }
        }

        // =========================
        // DIMINUTION STRESS CAFÉ
        // =========================
        if (isRelaxing &&
            coffeeMachineSound != null &&
            coffeeMachineSound.isPlaying)
        {
            currentStress -= stressDecreaseSpeed * Time.deltaTime;
        }
        else if (stressIncreaseEnabled)
        {
            float noise = GetAudioLoudness();

            float stressGain = baseStressGain;

            if (noise > highNoiseThreshold)
                stressGain = highStressGain;
            else if (noise > mediumNoiseThreshold)
                stressGain = mediumStressGain;

            currentStress += stressGain * Time.deltaTime;
        }

        currentStress =
            Mathf.Clamp(currentStress, 0f, maxStressThisScene);

        UpdateUI();

        // =========================
        // STRESS MAX
        // =========================
        if (currentStress >= maxStress &&
            !isTriggered &&
            SceneManager.GetActiveScene().name != warSceneName)
        {
            isTriggered = true;

            string currentScene =
                SceneManager.GetActiveScene().name;

            // =========================
            // HALLUCINATION 2
            // =========================
            if (currentScene == citySceneName)
            {
                PlayerPrefs.SetInt("WarCrisisID", 2);
                PlayerPrefs.SetInt("WarSpawnIndex", 1);

                Debug.Log("Hallucination 2 déclenchée");
            }

            // =========================
            // HALLUCINATION 3
            // =========================
            else if (currentScene == psySceneName)
            {
                PlayerPrefs.SetInt("WarCrisisID", 3);
                PlayerPrefs.SetInt("WarSpawnIndex", 2);
            }

            // =========================
            // HALLUCINATION 1
            // =========================
            else
            {
                PlayerPrefs.SetInt("WarCrisisID", 1);
                PlayerPrefs.SetInt("WarSpawnIndex", 0);

                Debug.Log("Hallucination 1 déclenchée");
            }

            StartCoroutine(StartCrisis());
        }
    }

    // =========================
    // UPDATE UI
    // =========================
    void UpdateUI()
    {
        if (stressSlider != null)
        {
            stressSlider.value = currentStress;
        }
    }

    // =========================
    // HALLUCINATION 3
    // =========================
    void TriggerHallucination3()
    {
        hallucinationTriggered = true;

        PlayerPrefs.SetInt("WarCrisisID", 3);
        PlayerPrefs.SetInt("WarSpawnIndex", 2);

        Debug.Log("Hallucination 3 déclenchée");

        StartCoroutine(LoadSceneWithFade(warSceneName));
    }

    // =========================
    // AUDIO LOUDNESS
    // =========================
    float GetAudioLoudness()
    {
        float total = 0f;

        AudioSource[] sources =
            FindObjectsOfType<AudioSource>();

        foreach (AudioSource src in sources)
        {
            if (src.isPlaying && src.clip != null)
            {
                total += src.volume;
            }
        }

        return sources.Length > 0
            ? Mathf.Clamp01(total / sources.Length)
            : 0f;
    }

    // =========================
    // CAFÉ
    // =========================
    public void StartCoffeeMaking()
    {
        isRelaxing = true;
    }

    public void StopCoffeeMaking()
    {
        isRelaxing = false;
    }

    // =========================
    // START CRISIS
    // =========================
    IEnumerator StartCrisis()
    {
        yield return StartCoroutine(
            LoadSceneWithFade(warSceneName)
        );
    }

    // =========================
    // LOAD SCENE + FADE VR
    // =========================
    IEnumerator LoadSceneWithFade(string sceneName)
    {
        FadeManager fade = FindObjectOfType<FadeManager>();

        if (fade != null)
        {
            yield return StartCoroutine(
                fade.FadeAndLoadScene(sceneName)
            );
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}