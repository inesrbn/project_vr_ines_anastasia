using UnityEngine;

public class AutoOpenDoor : MonoBehaviour
{
    private bool doorOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !doorOpen)
        {
            transform.Rotate(Vector3.up * 90);
            doorOpen = true;
        }
    }
}
