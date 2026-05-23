using UnityEngine;

public class CarLoopX : MonoBehaviour
{
    public float speed = 10f; // Vitesse de la voiture
    public float distance = 50f; // Distance à parcourir avant de revenir
    private Vector3 startPosition;

    void Start()
    {
        // Enregistre la position de départ
        startPosition = transform.position;
    }

    void Update()
    {
        // Déplace la voiture selon son axe X (ou Z selon ton modèle)
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Si la voiture a parcouru la distance, on la remet au point de départ
        if (Vector3.Distance(startPosition, transform.position) >= distance)
        {
            transform.position = startPosition;
        }
    }
}
