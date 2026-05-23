using UnityEngine;
using System.Collections; // pour IEnumerator

public class PlayerSpawnManager : MonoBehaviour
{
    public Transform defaultSpawnPoint;
    public Transform returnFromLetterPoint;
    public Transform returnFromWar;
    public string thoughtAtReturn = "Oh non, j'avais oublié ce rendez-vous chez la psy...";
    public string coffeeThought = "Je me ferais bien un café pour me détendre..."; // Nouvelle pensée pour le café
    public string thoughtPsy = "Mince il est bientôt 14h, je dois aller au cabinet";
    public float thoughtDelay = 0.7f;
    public float coffeeThoughtDelay = 10f; // Temps avant la pensée du café (30 secondes)
    public float psyThoughtDelay = 10f;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Joueur introuvable !");
            return;
        }

        bool isReturningFromLetter = PlayerPrefs.GetInt("ReturnFromLetter", 0) == 1;
        bool isReturningFromWar = PlayerPrefs.GetInt("ReturnFromWar", 0) == 1;

        if (isReturningFromLetter)
        {
            player.transform.position = returnFromLetterPoint.position;
            PlayerPrefs.SetInt("ReturnFromLetter", 0); // reset
            StartCoroutine(ShowSpawnThought(player));  // Affiche la pensée au spawn
            StartCoroutine(ShowCoffeeThought(player)); // Affiche la pensée du café après 30 secondes
        }
        else
        {
            player.transform.position = defaultSpawnPoint.position;
        }
        if(isReturningFromWar)
        {
            player.transform.position = returnFromWar.position;
            PlayerPrefs.SetInt("ReturnFromWar", 0); // reset
            StartCoroutine(ShowSpawnThoughtPsy(player));  // Affiche la pensée au spawn
        }
    }

    private IEnumerator ShowSpawnThought(GameObject player)
    {
        yield return new WaitForSeconds(thoughtDelay);

        ThoughtBubble thoughtBubble = player.GetComponent<ThoughtBubble>();
        if (thoughtBubble != null)
        {
            thoughtBubble.ShowThought(thoughtAtReturn); // Pensée après spawn
        }
        else
        {
            Debug.LogWarning("Le joueur n'a pas de composant ThoughtBubble.");
        }
    }

    private IEnumerator ShowCoffeeThought(GameObject player)
    {
        yield return new WaitForSeconds(coffeeThoughtDelay); // Attendre 30 secondes

        ThoughtBubble thoughtBubble = player.GetComponent<ThoughtBubble>();
        if (thoughtBubble != null)
        {
            thoughtBubble.ShowThought(coffeeThought); // Pensée après 30 secondes
        }
        else
        {
            Debug.LogWarning("Le joueur n'a pas de composant ThoughtBubble.");
        }
    }

    private IEnumerator ShowSpawnThoughtPsy(GameObject player)
    {
        yield return new WaitForSeconds(psyThoughtDelay); // Attendre 30 secondes

        ThoughtBubble thoughtBubble = player.GetComponent<ThoughtBubble>();
        if (thoughtBubble != null)
        {
            thoughtBubble.ShowThought(thoughtPsy); // Pensée après 30 secondes
        }
        else
        {
            Debug.LogWarning("Le joueur n'a pas de composant ThoughtBubble.");
        }
    }
}
