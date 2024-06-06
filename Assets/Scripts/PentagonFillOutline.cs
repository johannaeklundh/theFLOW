using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PentagonFillOutline : MonoBehaviour
{
    public int NumberPlayer;
    public float radius = 1f; // Radius of the pentagon
    public float offsetX = 0f; // Offset in the x-direction
    public float offsetY = 0f; // Offset in the y-direction
    public float width = 0.5f; // width of the line
    [Range(0f, 100f)] // set range of the input below to 0 to 100
    public float[] inputs = new float[] { 100.0f, 100.0f, 100.0f, 100.0f, 100.0f }; // were used before connected to real data

    LineRenderer lineRenderer;

    void Start()
    {
        // Get the LineRenderer component attached to this GameObject
        lineRenderer = GetComponent<LineRenderer>();

        // Set the number of positions for the LineRenderer
        lineRenderer.positionCount = 11; // 5 corners + 5 lines + 1 to close the loop

        inputs[0] = gamePlay.Instance.players[NumberPlayer].balance;
        inputs[1] = gamePlay.Instance.players[NumberPlayer].meanAlpha;
        inputs[2] = gamePlay.Instance.players[NumberPlayer].unbothered;
        inputs[3] = gamePlay.Instance.players[NumberPlayer].consistency;
        inputs[4] = gamePlay.Instance.players[NumberPlayer].meanTheta;

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
            vertices[i] = new Vector3(Mathf.Cos(angleInRadians) * radius * inputs[i] / 100 + offsetX, Mathf.Sin(angleInRadians) * radius * inputs[i] / 100 + offsetY, 0);
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
        for (int i = 0; i < 10; i += 2)
        {
            // Draw line from middle point to each corner of the pentagon
            lineRenderer.SetPosition(i, middlePoint);
            lineRenderer.SetPosition(i + 1, pentagonVertices[i / 2]);
        }

        // Close the loop by connecting the last point to the first point
        lineRenderer.SetPosition(10, middlePoint);
    }
}
