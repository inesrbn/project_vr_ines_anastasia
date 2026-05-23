using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CoffeeMachine : MonoBehaviour
{
    public GameObject coffeeButtonUI;
    public AudioSource coffeeSound;
    public GameObject coffeeCup;

    public StressManager stressManager;

    [Header("Input")]
    public InputActionProperty submitAction;

    private bool playerInZone = false;
    private bool hasPressed = false;

    // ✅ Réactive correctement l'input après changement de scène
    void OnEnable()
    {
        if (submitAction.action != null)
        {
            submitAction.action.Enable();
        }

        hasPressed = false;
    }

    void OnDisable()
    {
        if (submitAction.action != null)
        {
            submitAction.action.Disable();
        }
    }

    void Start()
    {
        if (coffeeButtonUI != null)
            coffeeButtonUI.SetActive(false);
    }

    void Update()
    {
        if (!playerInZone)
            return;

        if (submitAction.action != null &&
            submitAction.action.WasPressedThisFrame() &&
            !hasPressed)
        {
            hasPressed = true;

            Debug.Log("COFFEE BUTTON PRESSED");

            MakeCoffee();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;

            // ✅ reset interaction quand on revient dans la zone
            hasPressed = false;

            if (coffeeButtonUI != null)
                coffeeButtonUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;

            if (coffeeButtonUI != null)
                coffeeButtonUI.SetActive(false);
        }
    }

    public void MakeCoffee()
    {
        if (coffeeSound != null)
        {
            coffeeSound.Play();

            if (stressManager != null)
            {
                stressManager.StartCoffeeMaking();
            }

            StartCoroutine(ShowCoffeeAfterSound());
        }

        if (coffeeButtonUI != null)
            coffeeButtonUI.SetActive(false);
    }

    IEnumerator ShowCoffeeAfterSound()
    {
        yield return new WaitForSeconds(coffeeSound.clip.length);

        if (stressManager != null)
        {
            stressManager.StopCoffeeMaking();
        }

        if (coffeeCup != null)
        {
            coffeeCup.SetActive(true);
        }

        // ✅ permet de refaire un café plus tard
        hasPressed = false;
    }
}