using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class connectionBar : MonoBehaviour
{
    public float width = 10f;
    public float offsetX = -600f; // Offset in the x-direction
    public float offsetY = -200f; // Offset in the y-direction
    LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.endWidth = width;
        lineRenderer.startWidth = width;

        // Calculate the positions of the pentagon vertices
        Vector3[] pentagonVertices = CalculatePentagonVertices();

        // Set the positions for the LineRenderer
        SetLinePositions(pentagonVertices);
    }

    // Calculate the positions of the vertices of a regular pentagon
    Vector3[] CalculatePentagonVertices()
    {
        Vector3[] vertices = new Vector3[2];

        float factor = 1;
        float yHeight = 0;
        if (gameObject.CompareTag("1"))
        {
            factor = 1;
            yHeight = 10;
        } else if (gameObject.CompareTag("2"))
        {
            factor = 2;
            yHeight = 30;
        } else if (gameObject.CompareTag("3"))
        {
            factor = 3;
            yHeight = 50;
        } else if (gameObject.CompareTag("4"))
        {
            factor = 4;
            yHeight = 70;
        }
        //vertices[0] = new Vector3(0, yHeight, 0);
        vertices[0] = new Vector3(20 * factor + offsetX, yHeight + offsetY , 0);
        vertices[1] = new Vector3(-20 * factor+ offsetX, yHeight + offsetY, 0);

        return vertices;
    }

    // Set the positions for the LineRenderer
    void SetLinePositions(Vector3[] pentagonVertices)
    {
       
        // Draw line from middle point to each corner of the pentagon
      //  lineRenderer.SetPosition(0, pentagonVertices[0]);
        lineRenderer.SetPosition(0, pentagonVertices[0]);
        lineRenderer.SetPosition(1, pentagonVertices[1]);

        // Close the loop by connecting the last point to the first point
        //lineRenderer.SetPosition(10, middlePoint);
    }
}
