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
