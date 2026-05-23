using System.Collections;
using UnityEngine;

public class RecurringThought : MonoBehaviour
{
    public float interval = 180f; // 3 minutes = 180 secondes
    public string recurringMessage = "Ce grésillement me stress trop... Je dois garder le contrôle.";

    private ThoughtBubble thoughtBubble;

    void Start()
    {
        thoughtBubble = GetComponent<ThoughtBubble>();
        if (thoughtBubble == null)
        {
            Debug.LogWarning("Pas de ThoughtBubble trouvé !");
            return;
        }

        StartCoroutine(RecurringThoughtRoutine());
    }

    private IEnumerator RecurringThoughtRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            thoughtBubble.ShowThought(recurringMessage);
        }
    }
}
