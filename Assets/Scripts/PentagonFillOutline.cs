using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PentagonFillOutline : MonoBehaviour
{

    public float radius = 1f; // Radius of the pentagon
    public float offsetX = 0f; // Offset in the x-direction
    public float offsetY = 0f; // Offset in the y-direction
    public float width = 0.5f;
    [Range(0f, 1f)]
    public float[] inputs = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };

    LineRenderer lineRenderer;

    void Start()
    {
        // Get the LineRenderer component attached to this GameObject
        lineRenderer = GetComponent<LineRenderer>();

        // Set the number of positions for the LineRenderer
        lineRenderer.positionCount = 11; // 5 corners + 5 lines + 1 to close the loop

        
    }

    void Update()
    {
        lineRenderer.endWidth = width;
        lineRenderer.startWidth = width;
        // Calculate the positions of the pentagon vertices
        Vector3[] pentagonVertices = CalculatePentagonVertices();

        // Calculate the position of the middle point
        Vector3 middlePoint = CalculateMiddlePoint(pentagonVertices);

        // Set the positions for the LineRenderer
        SetLinePositions(middlePoint, pentagonVertices);
    }

    // Calculate the positions of the vertices of a regular pentagon
    Vector3[] CalculatePentagonVertices()
    {
        Vector3[] vertices = new Vector3[5];
        float angle = 360f / 5;

        for (int i = 0; i < 5; i++)
        {
            float angleInRadians = Mathf.Deg2Rad * (i * angle + 70); // Offset by 90 degrees to orient the pentagon correctly
            vertices[i] = new Vector3(Mathf.Cos(angleInRadians) * radius * inputs[i] + offsetX, Mathf.Sin(angleInRadians) * radius*inputs[i] + offsetY, 0);
        }

        return vertices;
    }

    // Calculate the middle point of the pentagon
    Vector3 CalculateMiddlePoint(Vector3[] vertices)
    {
        Vector3 middlePoint = Vector3.zero;

        foreach (Vector3 vertex in vertices)
        {
            middlePoint += vertex;
        }

        return middlePoint / vertices.Length;
    }

    // Set the positions for the LineRenderer
    void SetLinePositions(Vector3 middlePoint, Vector3[] pentagonVertices)
    {
        for (int i = 0; i < 10; i+=2)
        {
            // Draw line from middle point to each corner of the pentagon
            lineRenderer.SetPosition(i, middlePoint);
            lineRenderer.SetPosition(i + 1, pentagonVertices[i/2]);
        }

        // Close the loop by connecting the last point to the first point
        lineRenderer.SetPosition(10, middlePoint);
    }



    //// Draw the outline of the pentagon
    //void OnDrawGizmos()
    //{
    //    if (lineRenderer == null)
    //        lineRenderer = GetComponent<LineRenderer>();

    //    lineRenderer.startColor = lineColor;
    //    lineRenderer.endColor = lineColor;

    //    Vector3[] vertices = CalculatePentagonVertices();
    //    SetLinePositions(transform.position, vertices);
    //}


    //    [Range(0f, 1f)]
    //    public float[] inputs = new float[] { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };

    //    LineRenderer lineRenderer;

    //    void Start()
    //    {
    //        lineRenderer = GetComponent<LineRenderer>();
    //        lineRenderer.positionCount = 6;

    //        // Set the positions to form the outline of the pentagon
    //        SetPentagonOutline();
    //    }

    //    void SetPentagonOutline()
    //    {
    //        // Calculate the positions of the pentagon vertices
    //        Vector3[] vertices = CalculatePentagonVertices();

    //        // Set the positions for the LineRenderer
    //        for (int i = 0; i < 5; i++)
    //        {
    //            lineRenderer.SetPosition(i, vertices[i]);
    //        }

    //        // Close the loop by connecting the last point to the first point
    //        lineRenderer.SetPosition(5, vertices[0]);
    //    }

    //    Vector3[] CalculatePentagonVertices()
    //    {
    //        Vector3[] vertices = new Vector3[5];
    //        float angle = 360f / 5;

    //        for (int i = 0; i < 5; i++)
    //        {
    //            float angleInRadians = Mathf.Deg2Rad * (i * angle); // No need for offset here
    //            vertices[i] = new Vector3(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians), 0);
    //        }

    //        return vertices;
    //    }

    //    // Method to get the points of the pentagon outline
    //    public Vector2[] GetPentagonPoints()
    //    {
    //        Vector3[] vertices = CalculatePentagonVertices();
    //        Vector2[] points = new Vector2[vertices.Length];
    //        for (int i = 0; i < vertices.Length; i++)
    //        {
    //            points[i] = new Vector2(vertices[i].x, vertices[i].y);
    //        }
    //        return points;
    //    }

}
