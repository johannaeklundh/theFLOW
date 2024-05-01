using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightSourceBehaviour : MonoBehaviour
{
    public Material emissiveMaterial;
    public Color originalBaseColor;
    public Color originalEmissionColor;
    public float intensity = 1.0f;

    void Start()
    {
        // Store the original base color and emission color
        originalBaseColor = emissiveMaterial.color;
        originalEmissionColor = emissiveMaterial.GetColor("_EmissionColor");
    }

    void Update()
    {
        // Calculate the final emissive color with the desired intensity
        Color finalEmissiveColor = originalEmissionColor * intensity;

        // Set the emissive color of the material
        emissiveMaterial.SetColor("_EmissionColor", finalEmissiveColor);

        // Enable emission on the material (required for emission to be visible)
        emissiveMaterial.EnableKeyword("_EMISSION");
    }
}



/*
public class LightSourceBehaviour : MonoBehaviour
{
    public Material emissiveMaterial;
    public Color originalBaseColor;
    public Color originalEmissionColor;

    // Parameters for each player
    public float alpha = 50.0f; // Default value
    public float theta = 50.0f; // Default value

    // Mapping from alpha and theta to emission intensity and object size
    public AnimationCurve intensityCurve;
    public AnimationCurve sizeCurve;

    void Start()
    {
        // Store the original base color and emission color
        originalBaseColor = emissiveMaterial.color;
        originalEmissionColor = emissiveMaterial.GetColor("_EmissionColor");
    }

    void Update()
    {
        // Calculate emission intensity based on alpha value
        float emissionIntensity = intensityCurve.Evaluate(alpha / 100f);

        // Calculate object size based on theta value
        float objectSize = sizeCurve.Evaluate(theta / 100f);

        // Adjust the emissive intensity based on the desired intensity range
        float finalIntensity = Mathf.Lerp(5.0f, 10.0f, emissionIntensity);

        // Calculate the final emissive color with the desired intensity
        Color finalEmissiveColor = originalEmissionColor * finalIntensity;

        // Set the emissive color of the material
        emissiveMaterial.SetColor("_EmissionColor", finalEmissiveColor);

        // Enable emission on the material (required for emission to be visible)
        emissiveMaterial.EnableKeyword("_EMISSION");

        // Set the object size
        transform.localScale = new Vector3(objectSize, objectSize, objectSize);
    }

    // Method to update player performance parameters
    public void UpdatePerformance(float newAlpha, float newTheta)
    {
        // Update alpha and theta values
        alpha = newAlpha;
        theta = newTheta;
    }
}

*/