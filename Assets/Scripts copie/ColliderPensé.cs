using UnityEngine;

public class ThoughtTrigger : MonoBehaviour
{
    public string message = "Ah, j'ai reçu une lettre. Je devrais la lire...";

    public ThoughtBubble thoughtBubble; // assigné dans l'inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (thoughtBubble != null)
            {
                thoughtBubble.ShowThought(message);
            }
        }
    }
}