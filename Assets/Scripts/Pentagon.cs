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
    
    void Update()
    {
        polygonRenderer.startWidth = width;
        polygonRenderer.endWidth = width;
        DrawLooped();
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
}
