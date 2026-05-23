using UnityEngine;
using System.Collections;

public class RestorePortalPosition : MonoBehaviour
{
    [Header("XR References")]
    public Transform xrOrigin;
    public Transform mainCamera;

    IEnumerator Start()
    {
        // Attendre juste le chargement XR
        yield return null;
        yield return new WaitForEndOfFrame();

        // Désactive temporairement le CharacterController
        CharacterController cc =
            xrOrigin.GetComponent<CharacterController>();

        if (cc != null)
            cc.enabled = false;

        // Position cible = position de cet objet
        Vector3 targetCameraPosition = transform.position;

        // Rotation cible
        float targetRotationY = transform.eulerAngles.y;

        // Déplacement VR correct
        MoveCameraToTarget(
            targetCameraPosition,
            targetRotationY
        );

        // Attendre 1 frame
        yield return null;

        // Réactive le CharacterController
        if (cc != null)
            cc.enabled = true;

        Debug.Log("Spawn VR appliqué");
    }

    void MoveCameraToTarget(
        Vector3 targetCameraPosition,
        float targetYRotation)
    {
        // Rotation XR Origin
        xrOrigin.rotation =
            Quaternion.Euler(0f, targetYRotation, 0f);

        // Calcul offset réel VR
        Vector3 cameraOffset =
            targetCameraPosition - mainCamera.position;

        // Déplacement XR Origin
        xrOrigin.position += cameraOffset;

        Debug.Log("Camera déplacée vers : "
            + targetCameraPosition);
    }
}