using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
//using Unity.VisualScripting;

public class SpawnBalls : MonoBehaviour
{
    int radius = 10;
    public GameObject bollPrefab;
    public GameObject bollPrefabSmall;
    private float angle = Mathf.PI / 12;
    public int antalBollar;
    public float z;
    public float x;
    public List<GameObject> balls;
    //Vector3 ballPos;
    //public Rigidbody force;


    // Start is called before the first frame update
    void Start()
    {
        spawnCircleOfBalls();
        spawnGridOfBalls();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void spawnCircleOfBalls()
    {
        for (int i = 0; i < antalBollar; ++i)
        {
            x = radius * Mathf.Cos(angle);
            z = radius * Mathf.Sin(angle);

            Vector3 position = new(x, 0.0f, z);

            GameObject newBoll = Instantiate(bollPrefab, position, Quaternion.identity);
            balls.Add(newBoll);

            angle += Mathf.PI / 12;
        }
    }

    void spawnGridOfBalls()
    {

        //for (int x = -10; x < 10; x = x + 2)
        //{

        //    for (int z = -10; z < 10; z = z + 2)
        //    {
        //        Vector3 position = new(x, 0.0f, z);

        //        GameObject newBoll = Instantiate(bollPrefabSmall, position, Quaternion.identity);

        //    }

        //}
    }
}
