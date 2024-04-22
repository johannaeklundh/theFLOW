using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpeed : MonoBehaviour
{
    public AddForce addForce;
    private float step = 10f;
    private float radiusChange = 0.1f;

    void Update()
    {
        //Press M to increase speed
        if (Input.GetKeyDown(KeyCode.M))
        {
            addForce.targetCircularSpeed += step;
        }// Press L to decrease speed
        if (Input.GetKeyDown(KeyCode.L))
        {
            addForce.targetCircularSpeed -= step;
        }

        //Press S for smaller radius
        if (Input.GetKeyDown(KeyCode.S))
        {
            addForce.targetRadius -= radiusChange;
        }// Press B for bigger radius
        if (Input.GetKeyDown(KeyCode.B))
        {
            addForce.targetRadius += radiusChange;
        }
    }
}
