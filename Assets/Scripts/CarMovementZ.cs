using UnityEngine;

public class CarLoop : MonoBehaviour
{
    public float speed = 10f;
    public float distance = 50f;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (Vector3.Distance(startPosition, transform.position) >= distance)
        {
            transform.position = startPosition;
        }
    }
}
