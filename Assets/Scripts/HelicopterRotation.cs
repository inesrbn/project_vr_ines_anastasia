using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HelicopterRotation : MonoBehaviour
{
    [Header("Patrol settings")]
    public Transform centerPoint;    // assigner dans l'inspector (empty au centre)
    public float radius = 50f;       // rayon du cercle
    public float height = 30f;       // hauteur au dessus du centerPoint
    [Tooltip("Vitesse angulaire en degrés/seconde")]
    public float angularSpeed = 30f; // degrés par seconde

    [Header("Look ahead")]
    public float lookAheadDegrees = 8f; // pour orienter l'hélico vers sa prochaine position

    // état interne
    float angleDeg = 0f;

    void Start()
    {
        if (centerPoint == null)
        {
            Debug.LogWarning("[HelicopterPatrol] Aucun centerPoint assigné. Créée un empty et assigne-le.", this);
        }
    }

    void Update()
    {
        if (centerPoint == null) return;

        // incrémente l'angle en degrés
        angleDeg += angularSpeed * Time.deltaTime;
        // garde l'angle dans une plage raisonnable
        if (angleDeg > 360f) angleDeg -= 360f;

        // converti en radians pour cos/sin
        float rad = angleDeg * Mathf.Deg2Rad;
        float x = Mathf.Cos(rad) * radius;
        float z = Mathf.Sin(rad) * radius;

        Vector3 targetPos = centerPoint.position + new Vector3(x, height, z);
        transform.position = targetPos;

        // calcule une position "future" légèrement en avant pour orienter le nez
        float futureRad = (angleDeg + lookAheadDegrees) * Mathf.Deg2Rad;
        Vector3 futurePos = centerPoint.position + new Vector3(Mathf.Cos(futureRad) * radius, height, Mathf.Sin(futureRad) * radius);

        Vector3 forward = (futurePos - transform.position).normalized;
        if (forward.sqrMagnitude > 0.0001f)
        {
            // orienter doucement
            Quaternion targetRot = Quaternion.LookRotation(forward, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 6f * Time.deltaTime);
        }
    }

    // visualiser le rayon dans l'éditeur
    void OnDrawGizmosSelected()
    {
        if (centerPoint == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(centerPoint.position + Vector3.up * height, radius);
    }
}
