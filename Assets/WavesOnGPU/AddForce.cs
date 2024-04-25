using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    public gamePlay GP;

    public WaveManager waveManager;
    public Transform centerPoint; // The center point around which the objects will circle.
    public float circularSpeed = 0f; // The speed of the circular movement start at 0.
    public float radius; // The radius of the circular movement
    public float targetRadius;
    public float startAngle; // The initial angle for this object
    private float currentAngle; // Current angle of rotation around the center
    private Vector3 randomNoise;
    public float targetCircularSpeed; // New target speed variable
    public float speedAdjustmentRate; // Rate of speed change per second
    private float radiusAdjustmentRate = 0.5f;

    public int speedID;
    public int behaviorID;

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
        //transform.localScale = new Vector3(1f, 1f, 1f); //THIS TO CHANGE SIZE

        // Initialize centerPoint to origin
        if (centerPoint == null)
        {
            centerPoint = new GameObject("CenterPoint").transform;
            centerPoint.position = Vector3.zero;
        }

        currentAngle = startAngle;
        targetRadius = radius;
        targetCircularSpeed = DetermineSpeedBySpeedID(speedID);
    }

    float DetermineSpeedBySpeedID(int id)
    {
        switch (id)
        {
            case 1: return 25f;
            case 2: return 50f;
            default: return 10f;
        }
    }

    //Called every frame
    void FixedUpdate()
    {
        if (waveManager == null)
        {
            Debug.LogError("WaveManager is null. Make sure it is assigned or found on the same GameObject.");
            return;
        }

        if (circularSpeed < DetermineSpeedBySpeedID(speedID))
        {
            circularSpeed = Mathf.MoveTowards(circularSpeed, targetCircularSpeed, DetermineSpeedBySpeedID(speedID) * Time.deltaTime);
        }

        if (Mathf.Abs(targetCircularSpeed) < DetermineSpeedBySpeedID(speedID)) //Change direction quickly
        {
            if (behaviorID == 1) { circularSpeed = -1 * circularSpeed; }

            if (targetCircularSpeed < 0) { targetCircularSpeed += DetermineSpeedBySpeedID(speedID) * 2; } //100 change in circularspeed for a better effect
            else if (targetCircularSpeed > 0) { targetCircularSpeed -= DetermineSpeedBySpeedID(speedID) * 2; }
        }
        else
        {
            //Change direction slowly
            circularSpeed = Mathf.MoveTowards(circularSpeed, targetCircularSpeed, speedAdjustmentRate * Time.deltaTime);
        }

        radius = Mathf.MoveTowards(radius, targetRadius, radiusAdjustmentRate * Time.deltaTime);

        // Calculate circular movement
        RotateAroundCenter();
    }

    void RotateAroundCenter()
    {
        // Increment current angle based on circularSpeed
        currentAngle += circularSpeed * Time.deltaTime;
        currentAngle %= 360; // Ensure the angle stays within 0-360 degrees

        // Calculate the new position based on the center point, radius, and current angle
        Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward;
        Vector3 newPosition = centerPoint.position + direction * radius;

        // Apply the new position (and noise)

        if (Mathf.Abs(circularSpeed) > DetermineSpeedBySpeedID(speedID) + 20f && behaviorID ==1)
        {
            float noiseX = RandomGen();
            float noiseZ = RandomGen();

            randomNoise = new Vector3(noiseX, 0, noiseZ);
            transform.position = newPosition + randomNoise;
        }
        else { transform.position = newPosition; }

    }

    float RandomGen()
    {
        return UnityEngine.Random.Range(-0.03f, 0.03f);
    }
}