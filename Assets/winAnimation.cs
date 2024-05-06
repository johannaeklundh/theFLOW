using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class IntensityAnimation : MonoBehaviour
{
    public Material emissiveMaterial;   //player material

    //original colors 
    public Color originalBaseColor;     //player color
    public Color originalEmissionColor; //player emission color
    public float initialIntensity = 5.0f;      //intensity factor

    // Current colors and properties
    private Color currentBaseColor;
    private Color currentEmissionColor;
    public float currentIntensity;

    // Duration of the intensity animation in seconds
    public float animationDuration = 2.0f;

    // Time when the animation started
    private float animationStartTime;

    void Start()
    {
        // Create a new material instance from the original material
        emissiveMaterial = new Material(emissiveMaterial);

        // Store the original base color and emission color
        originalBaseColor = emissiveMaterial.color;
        originalEmissionColor = emissiveMaterial.GetColor("_EmissionColor");

        // Set initial appearance
        currentBaseColor = originalBaseColor;
        currentEmissionColor = originalEmissionColor;
        currentIntensity = initialIntensity;
    }

    void Update()
    {
       // changeIntensity();

        // Check if animation is in progress
        if (Time.time - animationStartTime < animationDuration)
        {
            // Calculate the progress of the animation
            float progress = (Time.time - animationStartTime) / animationDuration;

            // Interpolate the intensity
            currentIntensity = Mathf.Lerp(initialIntensity, 10.0f, progress);

            // Update the emissive color based on the interpolated intensity
            Color finalEmissiveColor = originalEmissionColor * currentIntensity;
            emissiveMaterial.SetColor("_EmissionColor", finalEmissiveColor);
            emissiveMaterial.EnableKeyword("_EMISSION");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            // Call the StartIntensityAnimation method of the IntensityAnimation script
            StartIntensityAnimation();
        }

    }

    public void StartIntensityAnimation()
    {
        // Start the intensity animation
        animationStartTime = Time.time;
    }

    //public void changeIntensity()
    //{

    //    Color finalEmissiveColor;

    //    currentIntensity = 10.0f;

    //    finalEmissiveColor = originalEmissionColor * currentIntensity;

    //    // Set the emissive color of the material
    //    emissiveMaterial.SetColor("_EmissionColor", finalEmissiveColor);

    //    // Enable emission on the material (required for emission to be visible)
    //    emissiveMaterial.EnableKeyword("_EMISSION");

    //}

    public void ResetAppearance()
    {
        currentIntensity = initialIntensity;//intensity factor

        // Reset emissive color to its original value
        emissiveMaterial.SetColor("_EmissionColor", originalEmissionColor * initialIntensity);

        // Enable emission on the material (required for emission to be visible)
        emissiveMaterial.EnableKeyword("_EMISSION");
    }


    // Reset appearance when the script is disabled (game ends or resets)
    void OnDisable()
    {
        Debug.Log("Script disabled; resetting appearance.");
        ResetAppearance();
    }
}