using UnityEngine;

public class UIFollowCamera : MonoBehaviour
{
    public float distance = 2f;
    public float heightOffset = -0.2f;

    void Update()
    {
        if (Camera.main == null) return;

        Transform cam = Camera.main.transform;

        Vector3 targetPosition = cam.position + cam.forward * distance;
        targetPosition.y += heightOffset;

        transform.position = targetPosition;

        transform.LookAt(cam);
        transform.Rotate(0, 180f, 0); // pour faire face correctement
    }
}