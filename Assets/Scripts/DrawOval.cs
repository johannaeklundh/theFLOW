using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawOval : MonoBehaviour
{

    public int resolution = 50; // Number of points to draw the oval
    public float widthLine = 2f;
    public float width = 3f; // Width of the oval
    public float height = 1f; // Height of the oval
    public float offsetX = -6.4f; // Offset in the x-direction
    public float offsetY = 1f; // Offset in the y-direction
    LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = resolution + 1;

        // Calculate the positions of the oval points
        SetOvalPositions();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.endWidth = widthLine;
        lineRenderer.startWidth = widthLine;
        SetOvalPositions();
    }

    void SetOvalPositions()
    {
        Vector3[] positions = new Vector3[resolution + 1];

        float angleIncrement = 360f / resolution;

        for (int i = 0; i <= resolution; i++)
        {
            float angle = i * angleIncrement;
            float radians = angle * Mathf.Deg2Rad;

            float x = Mathf.Cos(radians) * width + offsetX;
            float y = Mathf.Sin(radians) * height + offsetY;

            positions[i] = new Vector3(x * 20, y * 20, 0f);
        }

        lineRenderer.SetPositions(positions);
    }

 
}