using UnityEngine;
using System.Collections;

public class VRSpawnPoint : MonoBehaviour
{
    public Transform xrOrigin;
    public Transform mainCamera;

    IEnumerator Start()
    {
        yield return null;
        yield return new WaitForEndOfFrame();

        CharacterController cc =
            xrOrigin.GetComponent<CharacterController>();

        if (cc != null)
            cc.enabled = false;

        MovePlayerToSpawn();

        yield return null;

        if (cc != null)
            cc.enabled = true;
    }

    void MovePlayerToSpawn()
    {
        // Rotation
        xrOrigin.rotation =
            Quaternion.Euler(
                0f,
                transform.eulerAngles.y,
                0f);

        // Offset VR réel
        Vector3 offset =
            transform.position
            - mainCamera.position;

        // Déplacement
        xrOrigin.position += offset;

        Debug.Log("Spawn Psy appliqué");
    }
}