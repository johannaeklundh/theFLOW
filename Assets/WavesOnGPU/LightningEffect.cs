using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class LightningEffect : MonoBehaviour
{
    public Transform target;
    //public AddForce players;
    public float maxJitter = 0.2f;
    private LineRenderer lineRenderer;
    public KeyCode triggerKey = KeyCode.Space; // Key to trigger the lightning

    public Transform[] players;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;  // Initially hide the line renderer
    }

    void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            StartCoroutine(TriggerLightning());
        }
    }

    IEnumerator TriggerLightning()
    {
        lineRenderer.enabled = true;
        DrawLightning();
        yield return new WaitForSeconds(0.3f); // Display the lightning for 0.3 seconds
        lineRenderer.enabled = false;
    }

    public void ActivateLightning()
    {
        StartCoroutine(TriggerLightning());
    }

    public void ActivateLightning(int playerID)
    {
        // Access the player using the static array in AddForce
        //if (playerID > 0 && playerID < AddForce.players.Length)
        //{
        //    target = AddForce.players[playerID].transform;
        //    StartCoroutine(TriggerLightning());
        //}

        target = players[playerID];
        StartCoroutine(TriggerLightning());
    }

    void DrawLightning()
    {
        if (target == null) return;
        int segments = 10;
        Vector3 startPosition = Vector3.zero;
        Vector3 endPosition = target.position;
        Vector3 direction = (endPosition - startPosition) / segments;
        Vector3 currentPos = startPosition;
        lineRenderer.positionCount = segments + 1;

        for (int i = 0; i <= segments; i++)
        {
            if (i == segments)
            {
                lineRenderer.SetPosition(i, endPosition);
            }
            else
            {
                float offsetAmount = Random.Range(-maxJitter, maxJitter);
                Vector3 offset = new Vector3(offsetAmount, offsetAmount, offsetAmount);
                currentPos += direction;
                lineRenderer.SetPosition(i, currentPos + offset);
            }
        }
    }
}