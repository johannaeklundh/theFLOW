using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpeed : MonoBehaviour
{
    public AddForce addforce;


    //First method
    //public float clockWiseFast = 120f;
    //public float clockWiseSlow = 60f;
    //public float counterClockWiseFast = -120f;
    //public float counterClockWiseSlow = -60f;

    //Second method
    private float step = 10f;

    void Update()
    {
        //First method
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    addforce.circularSpeed = clockWiseFast;
        //}
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    addforce.circularSpeed = clockWiseSlow;
        //}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    addforce.circularSpeed = counterClockWiseSlow;
        //}
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    addforce.circularSpeed = counterClockWiseFast;
        //}

        //Second method
        if (Input.GetKeyDown(KeyCode.M))
        {
            addforce.targetCircularSpeed += step;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            addforce.targetCircularSpeed -= step;
        }
    }
}
