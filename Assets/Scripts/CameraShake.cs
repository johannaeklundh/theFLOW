using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // All these values can be changed to give another effect to the camerashake
    public float shakeDuration = 0.3f;
    public float shakeMagnitude = 0.1f;
    public float dampingSpeed = 1.0f;
    Vector3 initialPosition;

    void Awake()
    {
        if (transform == null)
        {
            Debug.LogError("Transform is NULL");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))  // Use Space key to test shake
        {
            TriggerShake();
        }
    }

    void OnEnable()
    {
        initialPosition = transform.localPosition;
    }

    public void TriggerShake()
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float z = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition = new Vector3(x, initialPosition.y, z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Dampen back to the initial position
        while (transform.localPosition != initialPosition)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, dampingSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
