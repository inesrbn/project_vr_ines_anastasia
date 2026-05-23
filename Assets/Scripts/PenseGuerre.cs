using UnityEngine;
using System.Collections; // pour IEnumerator

public class RecurringThoughtGuerre : MonoBehaviour
{
    public string recurringMessage = "Oh non, je ne veux pas revivre ça...";  // Message à afficher
    public float thoughtDisplayTime = 5f; // Durée pendant laquelle le message s'affiche (en secondes)

    private ThoughtBubble thoughtBubble;

    void Start()
    {
        thoughtBubble = GetComponent<ThoughtBubble>();
        if (thoughtBubble == null)
        {
            Debug.LogWarning("Pas de ThoughtBubble trouvé !");
            return;
        }

        // Lancer la coroutine pour afficher le message une fois
        StartCoroutine(ShowThoughtOnce());
    }

    private IEnumerator ShowThoughtOnce()
    {
        // Affiche le message
        thoughtBubble.ShowThought(recurringMessage);

        // Attend pendant la durée spécifiée avant de cacher le message
        yield return new WaitForSeconds(thoughtDisplayTime);

        // Cache le message après le délai
        thoughtBubble.HideThought();  // Assurez-vous que cette méthode existe dans ton script ThoughtBubble pour cacher le texte
    }
}
