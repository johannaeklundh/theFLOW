using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class LightningEffect : MonoBehaviour
{
    public Transform target;
    public float updateFrequency = 0.5f;
    public float maxJitter = 0.2f;
    private LineRenderer lineRenderer;
    private float timer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (timer <= 0)
        {
            DrawLightning();
            timer = updateFrequency;
        }
        timer -= Time.deltaTime;
    }

    void DrawLightning()
    {
        if (target == null) return;

        Vector3 startPosition = Vector3.zero; // Center of the scene
        Vector3 endPosition = target.position;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPosition);

        // Add jitter to simulate the chaotic nature of lightning
        Vector3 jitter = new Vector3(Random.Range(-maxJitter, maxJitter), Random.Range(-maxJitter, maxJitter), Random.Range(-maxJitter, maxJitter));
        lineRenderer.SetPosition(1, endPosition + jitter);
    }
}
