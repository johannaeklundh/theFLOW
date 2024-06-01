using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class LightSourceBehaviour : MonoBehaviour
{

    public Material emissiveMaterial;   //player material

    //original colors 
    public Color originalBaseColor;             //shpere color
    public Color originalEmissionColor;         //emission color
    public float initialIntensity = 4.0f;       //intensity factor
    public Vector3 initialSize = new Vector3(0.3f, 0.3f, 0.3f); // Size of the sphere

    public int playerID;
    public gamePlay GP;

    // Current colors and properties
    private Color currentBaseColor;
    private Color currentEmissionColor;
    public float currentIntensity;
    private Vector3 currentSize;

    [Range(0f, 100f)]
    public float testAlpha = 50f;

    [Range(0f, 100f)]
    public float testTheta = 50f;

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
        currentSize = initialSize;
    }

    void Update()
    {
        //storing new alpha and theta values
        testAlpha = GP.players[getPlayerID(playerID)].alpha;
        testTheta = GP.players[getPlayerID(playerID)].theta;

        //changing appearence for feedback purposes
        changeIntensity(testAlpha);
        changeSize(testTheta);
    }

    //checking which player is being evaluated
    int getPlayerID(int id)
    {
        switch (id)
        {
            case 1: return 0;
            case 2: return 1;
            case 3: return 2;
            case 4: return 3;
            default: return 0;
        }
    }

    public void changeIntensity(float alpha)
    {
        Color finalEmissiveColor;

        if (alpha > 80)
        {
            currentIntensity = 10.0f;
        }
        else if (60 < alpha && alpha > 80)
        {
            currentIntensity = 8.0f;
        }
        else if (40 < alpha && alpha > 60)
        {
            currentIntensity = 6.0f;
        }
        else
        { //under 40
            currentIntensity = 4.0f;
        }

        finalEmissiveColor = originalEmissionColor * currentIntensity;

        // Set the emissive color of the material
        emissiveMaterial.SetColor("_EmissionColor", finalEmissiveColor);

        // Enable emission on the material (required for emission to be visible)
        emissiveMaterial.EnableKeyword("_EMISSION");
    }

    public void changeSize(float theta)
    {
        if (theta > 80)
        {
            currentSize = new Vector3(0.35f, 0.35f, 0.35f);
        }
        else if (60 < theta && theta > 80)
        {
            currentSize = new Vector3(0.3f, 0.3f, 0.3f);
        }
        else if (40 < theta && theta > 60)
        {
            currentSize = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else
        { //under 40
            currentSize = new Vector3(0.2f, 0.2f, 0.2f);
        }
        
        transform.localScale = currentSize;
    }

    public void ResetAppearance()
    {
        currentIntensity = initialIntensity; 
        currentSize = initialSize;

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