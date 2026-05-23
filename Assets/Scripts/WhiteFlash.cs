using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WhiteFlash : MonoBehaviour
{
    public Image flashImage;
    public float flashDuration = 0.15f;

    void Awake()
    {
        flashImage.color = new Color(1, 1, 1, 0);
    }

    public void TriggerFlash()
    {
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        // Apparition instantanée
        flashImage.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(flashDuration);

        // Disparition
        flashImage.color = new Color(1, 1, 1, 0);
    }
}
