using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    public WaveManager waveManager;
    public Transform centerPoint; // The center point around which the objects will circle.
    public float circularSpeed = 30f; // The speed of the circular movement.
    public float radius = 3f; // The radius of the circular movement
    public float startAngle; // The initial angle for this object
    private Vector3 randomNoise;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure waveManager is assigned
        if (waveManager == null)
        {
            waveManager = GetComponent<WaveManager>();
            if (waveManager == null)
            {
                Debug.LogError("WaveManager is not assigned and could not be found on the same GameObject.");
                return;
            }
        }

        // Initialize centerPoint to origin
        if (centerPoint == null)
        {
            centerPoint = new GameObject("CenterPoint").transform;
            centerPoint.position = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        // Check if waveManager is still null
        if (waveManager == null)
        {
            Debug.LogError("WaveManager is null. Make sure it is assigned or found on the same GameObject.");
            return;
        }

        // Calculate circular movement
        RotateAroundCenter();
    }

    void RotateAroundCenter()
    {
        // Calculate the rotation angle based on time and speed, adjusted by startAngle
        float angle = (Time.time * circularSpeed + startAngle) % 360f;
        float noiseX = RandomGen();
        float noiseZ = RandomGen();

        randomNoise = new Vector3(noiseX, 0f, noiseZ);

        // Calculate the new position based on the center point and radius
        Vector3 newPosition = centerPoint.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * radius;

        // Rotate around the center point
        transform.position = newPosition +randomNoise;
        transform.RotateAround(centerPoint.position, Vector3.up, circularSpeed * Time.deltaTime);
    }

    float RandomGen()
    {
        return UnityEngine.Random.Range(-0.05f, 0.05f);
    }
}
