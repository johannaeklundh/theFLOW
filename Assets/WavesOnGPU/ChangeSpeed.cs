using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpeed : MonoBehaviour
{
    public AddForce changeForce;
    private float step = 10f;
    private float radiusChange = 1f;

    void Start()
    {
        StartCoroutine(delayUpdate());
    }

    private bool canUpdate = false; // Decides weather a function can update in update()

    IEnumerator delayUpdate()
    {
        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Allow updates to happen
        canUpdate = true;
    }

    void Update()
    {
        if (canUpdate)
        {
            UnityEngine.Debug.Log("Speed>!!!");
            //Press M to increase speed
            if (Input.GetKeyDown(KeyCode.M))
            {
                changeForce.targetCircularSpeed += step;
            }// Press L to decrease speed
            if (Input.GetKeyDown(KeyCode.L))
            {
                changeForce.targetCircularSpeed -= step;
            }
            if (changeForce.behaviorID == 0)
            {
                //Press S for smaller radius
                if (Input.GetKeyDown(KeyCode.S))
                {
                    changeForce.targetRadius -= radiusChange;
                }// Press B for bigger radius
                if (Input.GetKeyDown(KeyCode.B))
                {
                    changeForce.targetRadius += radiusChange;
                }

            }
        }
    }
}
