﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
    private float radiusAdjustmentRate = 1.25f;  // Maybe remove
    public int direction = 1; //direction of vortex determined from gameplayScript

    public int speedID;
    public int behaviorID;
    public int playerID;

    // Start is called before the first frame update
    void Start()
    {
        if (behaviorID == 1) // keep the waves invisible in the beginning
        {
            transform.position = new Vector3(-2f, -2f, -2f);
        }
        if (behaviorID == 0)
        { //right place for players in the start

            transform.position = new Vector3(Mathf.Cos((float)Math.PI * startAngle / 180) * radius, 0,
                                             Mathf.Sin((float)Math.PI * startAngle / 180) * radius);

        }

        StartCoroutine(delayUpdate());

        // Ensure waveManager is assigned
        if (waveManager == null)
        {
            waveManager = GetComponent<WaveManager>();
            if (waveManager == null)
            {
                UnityEngine.Debug.LogError("WaveManager is not assigned and could not be found on the same GameObject.");
                return;
            }
        }

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


    private bool canUpdate = false; // Decides weather a function can update in update()

    IEnumerator delayUpdate()
    {
        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Allow updates to happen
        canUpdate = true;
    }

    float DetermineSpeedBySpeedID(int id) // Speed for players and vortex
    {
        switch (id)
        {
            case 1: return 20f; // Players speed
            case 2: return 40f; // Speed of vortex

            default: return 10f; 
        }
    }

    int DetermineRadiusByPlayerId(int id) // To distinguish players from eachother, here more players can be added if needed
    {
        switch (id)
        {
            case 1: return 0; // player 1
            case 2: return 1; // player 2
            case 3: return 2; // player 3
            case 4: return 3; // player 4
            default: return 0;
        }
    }

    //Called every frame
    void FixedUpdate()
    {
        if (canUpdate)
        {
            if (waveManager == null)
            {
                UnityEngine.Debug.LogError("WaveManager is null. Make sure it is assigned or found on the same GameObject.");
                return;
            }

            if (circularSpeed < DetermineSpeedBySpeedID(speedID))
            {
                circularSpeed = Mathf.MoveTowards(circularSpeed, targetCircularSpeed, DetermineSpeedBySpeedID(speedID) * Time.deltaTime);
            }

            direction = GP.getWhoIsWinning(); //-1 = AI leading, 0 = gameover , 1 = player leading

            if (direction == 1) { circularSpeed = DetermineSpeedBySpeedID(speedID); }
            else if (direction == -1) { circularSpeed = -1 * DetermineSpeedBySpeedID(speedID); }
            //else
            //{
            //   // if (behaviorID == 1) { radius = 100; } //out of screen if == 0 (NOT DONE)
            //}
            if (Input.GetKeyDown(KeyCode.D)) { radius = 100; } //PRESS D FOR THE LOSINGANIMATION other part is on screendarkener

            if (Mathf.Abs(targetCircularSpeed) < DetermineSpeedBySpeedID(speedID))
            {
                if (behaviorID == 1) { circularSpeed = -1 * circularSpeed; } //Change direction quickly

                if (targetCircularSpeed < 0) { targetCircularSpeed += DetermineSpeedBySpeedID(speedID) * 2; } //Change targetspeed quickly
                else if (targetCircularSpeed > 0) { targetCircularSpeed -= DetermineSpeedBySpeedID(speedID) * 2; }
            }
            else
            {
                //Change direction slowly
                circularSpeed = Mathf.MoveTowards(circularSpeed, targetCircularSpeed, speedAdjustmentRate * Time.deltaTime);
            }

            if (behaviorID == 0)
            {
                targetRadius = GP.players[DetermineRadiusByPlayerId(playerID)].radius;

            }

            radius = Mathf.MoveTowards(radius, targetRadius, radiusAdjustmentRate * Time.deltaTime);

            // Calculate circular movement
            RotateAroundCenter();

        }

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
        if (Mathf.Abs(circularSpeed) > DetermineSpeedBySpeedID(speedID) + 10f && behaviorID == 1)
        {
            float noiseX = RandomGen();
            float noiseZ = RandomGen();

            randomNoise = new Vector3(noiseX, 0, noiseZ);
            transform.position = newPosition + randomNoise;
        }
        else { transform.position = newPosition; }

    }

    float RandomGen() // Noise, can change values for more noise
    {
        return UnityEngine.Random.Range(-0.03f, 0.03f);
    }
}
