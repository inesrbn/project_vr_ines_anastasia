using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class WarCrisis3Controller : MonoBehaviour
{
    [Header("--- PLAYER AUTO-SETUP ---")]
    public GameObject player;

    [Header("--- INPUT XR ---")]
    public InputActionProperty submitAction; // ✅ remplace Space

    [Header("--- CONFIGURATION UI ---")]
    public GameObject eyesCanvas;
    public RectTransform topEyelid;
    public RectTransform bottomEyelid;

    [Header("--- CAMERA ---")]
    public Camera playerCamera;
    public Transform headTransform;
    public float headBobAmplitude = 0.5f;
    public float headBobSpeed = 1.0f;

    [Header("Blur & Vision")]
    public Image vignetteImage;
    public float maxVignetteAlpha = 0.6f;

    [Header("--- WARNING TEXT ---")]
    public TextMeshProUGUI warningText;

    [Header("--- DIFFICULTÉ & LUTTE ---")]
    [Range(0f, 1.5f)]
    public float currentFatigue = 0f;

    public float fatigueIncreaseRate = 0.2f;
    public float recoveryPerPress = 0.25f;
    public bool harderWhenTired = true;

    [Header("--- MICRO-SIESTES ---")]
    public float minBlinkInterval = 2f;
    public float maxBlinkInterval = 5f;
    public float blinkCloseSpeed = 8f;
    public float blinkReopenSpeed = 1.5f;

    [Header("--- CONTROLE DES YEUX ---")]
    public float fatigueNoBlinkThreshold = 0.35f;

    [Header("--- GAMEPLAY ---")]
    public PlayerMovementRotation playerMovement;

    private float currentEyeOpenness = 1f;
    private float targetEyeOpenness = 1f;
    private float blinkTimer;
    private bool isBlinking = false;

    private Vector3 startCamPos;
    private Quaternion startCamRot;
    private float perlinTime;

    void OnEnable()
    {
        if (eyesCanvas)
            eyesCanvas.SetActive(true);

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            if (!playerCamera)
                playerCamera = player.GetComponentInChildren<Camera>();

            if (!headTransform)
                headTransform = player.transform.Find("Head");

            if (!playerMovement)
                playerMovement = player.GetComponent<PlayerMovementRotation>();
        }

        if (!playerCamera || !headTransform)
        {
            Debug.LogError("Impossible de trouver la Camera ou le Head !");
            this.enabled = false;
            return;
        }

        startCamPos = playerCamera.transform.localPosition;
        startCamRot = playerCamera.transform.localRotation;

        currentFatigue = 0.3f;

        blinkTimer = Random.Range(minBlinkInterval, maxBlinkInterval);

        if (warningText != null)
            warningText.gameObject.SetActive(false);
    }

    void Start()
    {
        if (warningText != null)
            StartCoroutine(ShowWarningLoop());
    }

    void Update()
    {
        float dt = Time.deltaTime;

        perlinTime += dt * headBobSpeed;

        // 📈 Fatigue augmente
        currentFatigue += fatigueIncreaseRate * dt;

        // 🎮 INPUT XR SUBMIT ACTION
        if (submitAction.action != null &&
            submitAction.action.WasPressedThisFrame())
        {
            float recovery = recoveryBasePower();

            currentFatigue -= recovery;

            currentEyeOpenness = Mathf.Max(
                currentEyeOpenness,
                1f - currentFatigue
            );

            ApplyHeadJolt();
        }

        currentFatigue = Mathf.Clamp(currentFatigue, 0f, 1.5f);

        // 👁️ Gestion des clignements
        bool allowBlink = currentFatigue > fatigueNoBlinkThreshold;

        if (allowBlink)
        {
            float blinkSpeedFactor =
                Mathf.Lerp(0.2f, 1.5f, currentFatigue);

            blinkTimer -= dt * blinkSpeedFactor;

            if (blinkTimer <= 0)
            {
                StartCoroutine(DoHeavyBlink());

                blinkTimer =
                    Random.Range(minBlinkInterval, maxBlinkInterval);
            }
        }
        else
        {
            isBlinking = false;
        }

        float eyeOpenFactor =
            Mathf.Clamp01(1f - currentFatigue);

        float blinkFactor =
            isBlinking ? 0.2f : 1f;

        targetEyeOpenness =
            Mathf.Max(eyeOpenFactor,
            eyeOpenFactor * blinkFactor);

        if (currentFatigue <= fatigueNoBlinkThreshold)
        {
            targetEyeOpenness =
                Mathf.Max(targetEyeOpenness, currentEyeOpenness);
        }

        // 👁️ Animation ouverture/fermeture
        if (targetEyeOpenness > currentEyeOpenness)
        {
            currentEyeOpenness =
                Mathf.Lerp(
                    currentEyeOpenness,
                    targetEyeOpenness,
                    dt * blinkReopenSpeed
                );
        }
        else if (currentFatigue > fatigueNoBlinkThreshold)
        {
            currentEyeOpenness =
                Mathf.Lerp(
                    currentEyeOpenness,
                    targetEyeOpenness,
                    dt * blinkCloseSpeed
                );
        }

        UpdateEyelidsUI(currentEyeOpenness);
        UpdateVisionEffects();
        UpdateHeadMovement(dt);
    }

    float recoveryBasePower()
    {
        if (harderWhenTired)
        {
            float resistance =
                Mathf.Clamp01(currentFatigue * 0.5f);

            return recoveryPerPress * (1f - resistance);
        }

        return recoveryPerPress;
    }

    IEnumerator DoHeavyBlink()
    {
        isBlinking = true;

        float closedDuration =
            Random.Range(0.1f, 0.3f) +
            (currentFatigue * 0.4f);

        yield return new WaitForSeconds(closedDuration);

        isBlinking = false;
    }

    void UpdateEyelidsUI(float openness)
    {
        float closingVal = 1f - openness;

        if (topEyelid && bottomEyelid)
        {
            topEyelid.localScale =
                new Vector3(1, closingVal, 1);

            bottomEyelid.localScale =
                new Vector3(1, closingVal, 1);
        }
    }

    void UpdateVisionEffects()
    {
        if (vignetteImage == null)
            return;

        float fatigue01 =
            Mathf.Clamp01(currentFatigue);

        Color c = vignetteImage.color;

        c.a =
            Mathf.Lerp(
                0f,
                maxVignetteAlpha,
                fatigue01
            );

        vignetteImage.color = c;
    }

    void UpdateHeadMovement(float dt)
    {
        float trembleAmount =
            Mathf.InverseLerp(0f, 1.2f, currentFatigue);

        trembleAmount =
            Mathf.Lerp(0.1f, 1f, trembleAmount);

        if (trembleAmount <= 0.001f)
        {
            playerCamera.transform.localPosition =
                Vector3.Lerp(
                    playerCamera.transform.localPosition,
                    startCamPos,
                    dt * 5f
                );

            playerCamera.transform.localRotation =
                Quaternion.Slerp(
                    playerCamera.transform.localRotation,
                    startCamRot,
                    dt * 5f
                );

            return;
        }

        float swayX =
            (Mathf.PerlinNoise(perlinTime, 0) - 0.5f) *
            2f *
            headBobAmplitude *
            trembleAmount;

        float swayY =
            (Mathf.PerlinNoise(0, perlinTime) - 0.5f) *
            2f *
            headBobAmplitude *
            trembleAmount;

        float tilt =
            Mathf.Lerp(0f, 15f, trembleAmount) *
            Mathf.Sin(Time.time * (0.5f + trembleAmount));

        playerCamera.transform.localPosition =
            Vector3.Lerp(
                playerCamera.transform.localPosition,
                startCamPos + new Vector3(swayX, swayY, 0),
                dt * 2f
            );

        playerCamera.transform.localRotation =
            Quaternion.Slerp(
                playerCamera.transform.localRotation,
                startCamRot *
                Quaternion.Euler(
                    swayY * 12f,
                    swayX * 12f,
                    tilt
                ),
                dt * 2f
            );
    }

    void ApplyHeadJolt()
    {
        perlinTime += 3f;

        currentEyeOpenness =
            Mathf.Min(currentEyeOpenness + 0.2f, 1f);
    }

    IEnumerator ShowWarningLoop()
    {
        while (true)
        {
            if (warningText != null)
            {
                warningText.text =
                    "Attention ! Tu dois te reprendre en main !";

                warningText.gameObject.SetActive(true);

                yield return new WaitForSeconds(2f);

                warningText.gameObject.SetActive(false);

                yield return new WaitForSeconds(3f);
            }
            else
            {
                yield return null;
            }
        }
    }
}