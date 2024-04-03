using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class AddForce : MonoBehaviour
{

    private List<GameObject> gameObjects = new List<GameObject>();
    private SpawnBalls Spawn;
    public Transform centerPoint; // The center point around which the balls will circle.
    public float inwardForce = 10f; // The strength of the force pulling the balls towards the center.
    public float tangentialForce = 10f; // The strength of the force pushing the balls tangentially.


    // Start is called before the first frame update
    void Start()
    {
        Spawn = GetComponent<SpawnBalls>();
        gameObjects = Spawn.balls;


    }

    // Update is called once per frame
    //void Update()
    //{

    //    foreach (GameObject go in gameObjects) 
    //    {
    //        go.GetComponent<Rigidbody>().AddForce(0.0f, 0.1f, 0.0f, ForceMode.Impulse);

    //    }

    //}
    void FixedUpdate() //Better for physics
    {
        foreach (GameObject ball in gameObjects)
        {
            Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
            Vector3 noiseVec = new Vector3(Random.Range(-1, 1), 0.0f, Random.Range(-1, 1)).normalized;

            // Calculate direction towards the center.
            Vector3 towardsCenter = (centerPoint.position - ball.transform.position).normalized;
            Vector3 forceDirection = Vector3.Cross(towardsCenter, Vector3.up).normalized; // Cross product to get tangential direction.

            // Apply forces.
            ballRigidbody.AddForce(towardsCenter * inwardForce + noiseVec);
            ballRigidbody.AddForce(forceDirection * tangentialForce);
        }
    }
}
