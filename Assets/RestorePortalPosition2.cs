using UnityEngine;
using System.Collections;

public class RestorePortalPosition2 : MonoBehaviour
{
    [Header("XR References")]
    public Transform xrOrigin;
    public Transform mainCamera;

    IEnumerator Start()
    {
        // IMPORTANT :
        // Ne rien faire si on ne revient pas de la guerre
        if (PlayerPrefs.GetInt("ReturnFromWar", 0) != 1)
            yield break;

        // Reset du flag
        PlayerPrefs.SetInt("ReturnFromWar", 0);

        // Attendre XR
        yield return null;
        yield return new WaitForEndOfFrame();

        CharacterController cc =
            xrOrigin.GetComponent<CharacterController>();

        if (cc != null)
            cc.enabled = false;

        Vector3 targetPosition =
            transform.position;

        float targetRotationY =
            transform.eulerAngles.y;

        MoveXRPlayerToSpawn(
            targetPosition,
            targetRotationY);

        yield return null;

        if (cc != null)
            cc.enabled = true;

        Debug.Log("Retour guerre appliqué");
    }

    void MoveXRPlayerToSpawn(
        Vector3 targetCameraPosition,
        float targetYRotation)
    {
        xrOrigin.rotation =
            Quaternion.Euler(
                0f,
                targetYRotation,
                0f);

        Vector3 offset =
            targetCameraPosition
            - mainCamera.position;

        xrOrigin.position += offset;
    }
}