using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotorSpin : MonoBehaviour
{
    [Tooltip("Vitesse en tours par minute (RPM)")]
    public float rpm = 600f;
    [Tooltip("Si vrai, rotation en localSpace")]
    public bool useLocal = true;
    public Vector3 axis = Vector3.up;

    void Update()
    {
        // convertir rpm -> degrés par seconde : rpm * 360 / 60
        float degPerSec = rpm * 6f; // 360/60 = 6
        float delta = degPerSec * Time.deltaTime;
        if (useLocal)
            transform.Rotate(axis, delta, Space.Self);
        else
            transform.Rotate(axis, delta, Space.World);
    }
}
