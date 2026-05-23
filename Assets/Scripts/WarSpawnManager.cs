using UnityEngine;
using System.Collections;

public class WarSpawnManager : MonoBehaviour
{
    public Transform[] spawnPoints;

    public Transform xrOrigin;
    public Transform mainCamera;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);

        int crisisID = PlayerPrefs.GetInt("WarCrisisID", 0);
        int spawnIndex = PlayerPrefs.GetInt("WarSpawnIndex", 0);

        if (spawnIndex >= 0 && spawnIndex < spawnPoints.Length)
        {
            Transform spawn = spawnPoints[spawnIndex];

            Vector3 cameraOffset = xrOrigin.position - mainCamera.position;

            xrOrigin.position = spawn.position + cameraOffset;

            Debug.Log("Spawn appliqué : " + spawn.position);
        }

        WarCrisisManager crisisManager = FindObjectOfType<WarCrisisManager>();

        if (crisisManager != null)
        {
            crisisManager.ActivateCrisis(crisisID);
        }
    }
}