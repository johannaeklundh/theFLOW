using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Pentagon : MonoBehaviour
{
    public int sides = 5;
    public float radius = 1;
    public LineRenderer polygonRenderer;
    public int extraSteps = 2;
    public float moveX = 6;
    public float moveY = 2;
    public float width = 0.5f;

    public bool isLooped;

    // A material is set to object
    //void Start()
    //{
    //    polygonRenderer.startColor = Color.red;
    //    polygonRenderer.endColor = Color.blue;
    //}
    
    void Update()
    {
        polygonRenderer.startWidth = width;
        polygonRenderer.endWidth = width;
        if (isLooped)
        {
            DrawLooped();
        }
        else
        {
            DrawOverlapped();
        }

    }

    void DrawLooped()
    {
        polygonRenderer.positionCount = sides;
        float TAU = 2 * Mathf.PI;

        for (int currentPoint = 0; currentPoint < sides; currentPoint++)
        {
            float currentRadian = ((float)currentPoint / sides) * TAU;
            float x = Mathf.Cos(currentRadian) * radius;
            float y = Mathf.Sin(currentRadian) * radius;
            polygonRenderer.SetPosition(currentPoint, new Vector3(x + moveX, y + moveY, 0));
        }
        polygonRenderer.loop = true;
    }

    void DrawOverlapped()
    {
        DrawLooped();
        polygonRenderer.loop = false;
        polygonRenderer.positionCount += extraSteps;

        int positionCount = polygonRenderer.positionCount;
        for (int i = 0; i < extraSteps; i++)
        {
            polygonRenderer.SetPosition(positionCount - extraSteps + i, polygonRenderer.GetPosition(i));
        }
    }
    //public float radius = 1f; // Radius of the pentagon
    //public float offsetX = 0f; // Offset in the x-direction
    //public float offsetY = 0f; // Offset in the y-direction
    //public int sides = 5; // Corners of the object

    //LineRenderer lineRenderer;

    //public float alpha = 1f;
    //public float theta = 1f;
    //public float balance = 1f;
    //public float unbothered = 1f;
    //public float consistency = 1f;

    //public Color lineColor = Color.white; // Color of the lines

    //void Start()
    //{
    //    // Get the LineRenderer component attached to this GameObject
    //    lineRenderer = GetComponent<LineRenderer>();

    //    // Set the number of positions for the LineRenderer
    //    lineRenderer.positionCount = 5; // 5 corners + 5 lines + 1 to close the loop

    //    DrawLooped();

    //}

    //void DrawLooped()
    //{
    //    lineRenderer.positionCount = sides;
    //    float TAU = 2 * Mathf.PI;

    //    Vector3 middlePoint = Vector3.zero;

    //    for (int currentPoint = 0; currentPoint < sides; currentPoint++)
    //    {
    //        float currentRadian = ((float)currentPoint / sides) * TAU;
    //        float x = Mathf.Cos(currentRadian) * radius;
    //        float y = Mathf.Sin(currentRadian) * radius;
    //        lineRenderer.SetPosition(currentPoint, middlePoint);
    //    }

    //}


}

//[RequireComponent(typeof(LineRenderer))]
//public class Pentagon : MonoBehaviour
//{
//    public int sides = 5;
//    public float radius = 3;
//    public LineRenderer polygonRenderer;
//    public int extraSteps = 1;
//    public float moveX = 6;
//    public float moveY = 2;

//    public bool isLooped;

//    void Update()
//    {
//        if (isLooped)
//        {
//            DrawLooped();
//        }
//        else
//        {
//            DrawOverlapped();
//        }

//    }

//    void DrawLooped()
//    {
//        polygonRenderer.positionCount = sides;
//        float TAU = 2 * Mathf.PI;

//        for (int currentPoint = 0; currentPoint < sides; currentPoint++)
//        {
//            float currentRadian = ((float)currentPoint / sides) * TAU;
//            float x = Mathf.Cos(currentRadian) * radius;
//            float y = Mathf.Sin(currentRadian) * radius;
//            polygonRenderer.SetPosition(currentPoint, new Vector3(x + moveX, y + moveY, 0));
//        }
//        polygonRenderer.loop = true;
//    }

//    void DrawOverlapped()
//    {
//        DrawLooped();
//        polygonRenderer.loop = false;
//        polygonRenderer.positionCount += extraSteps;

//        int positionCount = polygonRenderer.positionCount;
//        for(int i = 0; i < extraSteps; i++)
//        {
//            polygonRenderer.SetPosition(positionCount-extraSteps, polygonRenderer.GetPosition(i));
//        }
//    }
//}



//    public float radius = 1f; // Radius of the pentagon
//    public float offsetX = 0f; // Offset in the x-direction
//    public float offsetY = 0f; // Offset in the y-direction

//    //public gamePlay GPintance = gamePla

//    LineRenderer lineRenderer;

//    public float alpha = 1f;
//    public float theta = 1f;
//    public float balance = 1f;
//    public float unbothered = 1f;
//    public float consistency = 1f;

//    public Color lineColor = Color.white; // Color of the lines

//    void Start()
//    {
//        // Get the LineRenderer component attached to this GameObject
//        lineRenderer = GetComponent<LineRenderer>();

//        // Set the number of positions for the LineRenderer
//        lineRenderer.positionCount = 10; // 5 corners + 5 lines + 1 to close the loop

//        // Calculate the positions of the pentagon vertices
//        Vector3[] pentagonVertices = CalculatePentagonVertices();

//        // Calculate the position of the middle point
//        Vector3 middlePoint = CalculateMiddlePoint(pentagonVertices);

//        // Set the positions for the LineRenderer
//        SetLinePositions(middlePoint, pentagonVertices);
//    }

//    // Calculate the positions of the vertices of a regular pentagon
//    Vector3[] CalculatePentagonVertices()
//    {
//        Vector3[] vertices = new Vector3[5];
//        float angle = 360f / 5;

//        for (int i = 0; i < 5; i++)
//        {
//            float angleInRadians = Mathf.Deg2Rad * (i * angle + 90); // Offset by 90 degrees to orient the pentagon correctly
//            vertices[i] = new Vector3(Mathf.Cos(angleInRadians) * radius, Mathf.Sin(angleInRadians) * radius, 0);
//        }

//        return vertices;
//    }

//    // Calculate the middle point of the pentagon
//    Vector3 CalculateMiddlePoint(Vector3[] vertices)
//    {
//        Vector3 middlePoint = Vector3.zero;

//        foreach (Vector3 vertex in vertices)
//        {
//            middlePoint += vertex;
//        }

//        return middlePoint / vertices.Length;
//    }

//    // Set the positions for the LineRenderer
//    void SetLinePositions(Vector3 middlePoint, Vector3[] pentagonVertices)
//    {
//        for (int i = 0; i < 5; i++)
//        {
//            // Draw line from middle point to each corner of the pentagon
//            lineRenderer.SetPosition(i, middlePoint);
//            lineRenderer.SetPosition(i + 5, pentagonVertices[i]);
//        }

//        // Close the loop by connecting the last point to the first point
//        lineRenderer.SetPosition(10, pentagonVertices[0]);
//    }



//    // Draw the outline of the pentagon
//    void OnDrawGizmos()
//    {
//        if (lineRenderer == null)
//            lineRenderer = GetComponent<LineRenderer>();

//        lineRenderer.startColor = lineColor;
//        lineRenderer.endColor = lineColor;

//        Vector3[] vertices = CalculatePentagonVertices();
//        SetLinePositions(transform.position, vertices);
//    }


//    //void catchValues()
//    //{
//    //    float alpha1 = gamePlay.player1.alphaMean;
//    //    float alpha2 = gamePlay.player2.alphaMean;
//    //    float alpha3 = gamePlay.player3.alphaMean;
//    //    float alpha4 = gamePlay.player4.alphaMean;

//    //    float theta1 = gamePlay.player1.thetaMean;
//    //    float theta2 = gamePlay.player2.thetaMean;
//    //    float theta3 = gamePlay.player3.thetaMean;
//    //    float theta4 = gamePlay.player4.thetaMean;

//    //    float balance1 = gamePlay.player1.balance;
//    //    float balance2 = gamePlay.player2.balance;
//    //    float balance3 = gamePlay.player3.balance;
//    //    float balance4 = gamePlay.player4.balance;

//    //    float unbothered1 = gamePlay.player1.unbothered;
//    //    float unbothered2 = gamePlay.player2.unbothered;
//    //    float unbothered3 = gamePlay.player3.unbothered;
//    //    float unbothered4 = gamePlay.player4.unbothered;

//    //    float consistency1 = gamePlay.player1.consistency;
//    //    float consistency2 = gamePlay.player2.consistency;
//    //    float consistency3 = gamePlay.player3.consistency;
//    //    float consistency4 = gamePlay.player4.consistency;

//    //}

//}
