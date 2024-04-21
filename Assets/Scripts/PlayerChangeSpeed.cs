using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangeSpeed : MonoBehaviour
{
    public PlayerAddForce addforce;
    private float step = 10f;

    void Update()
    {
        //Press "M" to make the speed faster
        if (Input.GetKeyDown(KeyCode.M))
        {
            addforce.targetCircularSpeed += step;
        }
        //Press "L" to make it slower
        if (Input.GetKeyDown(KeyCode.L))
        {
            addforce.targetCircularSpeed -= step;
        }

        //Can also add a script for changing the radius the same way!
    }
}
