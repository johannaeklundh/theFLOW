using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class LightSourceBehaviour : MonoBehaviour
{
    
    public Material emissiveMaterial;   //player material
    public Color originalBaseColor;     //player color
    public Color originalEmissionColor; //player emission color
    public float intensity = 2.0f;      //intensity factor

    /*
    [SerializeField] // Expose in the inspector
    private Vector3 sphereSize = new Vector3(0.3f, 0.3f, 0.3f); // Size of the sphere
    */

    public Vector3 sphereSize = new Vector3(0.3f, 0.3f, 0.3f); // Size of the sphere

    [Range(0f, 100f)]
    public float testAlpha = 50f;

    [Range(0f, 100f)]
    public float testTheta = 50f;


    void Start()
    {
        
        // Store the original base color and emission color
        originalBaseColor = emissiveMaterial.color;
        originalEmissionColor = emissiveMaterial.GetColor("_EmissionColor");

    }



    void Update()
    {

        changeIntensity(testAlpha);
        changeSize(testTheta);

/*
        // Calculate the final emissive color with the desired intensity
        Color finalEmissiveColor = originalEmissionColor * intensity;

        // Set the emissive color of the material
        emissiveMaterial.SetColor("_EmissionColor", finalEmissiveColor);

        // Enable emission on the material (required for emission to be visible)
        emissiveMaterial.EnableKeyword("_EMISSION");

        // Update the size of the sphere
        transform.localScale = sphereSize;
*/
    }


    public void changeIntensity(float alpha) {

        //int alphaTransform = alpha/100f + 1;
        Color finalEmissiveColor;

        if(alpha > 50){

            intensity = 3.0f;
            // Calculate the final emissive color with the desired intensity
            finalEmissiveColor = originalEmissionColor * intensity;

        }

        else {
            intensity = -3.0f;
            // Calculate the final emissive color with the desired intensity
            finalEmissiveColor = originalEmissionColor * intensity;

        }

        // Set the emissive color of the material
        emissiveMaterial.SetColor("_EmissionColor", finalEmissiveColor);

        // Enable emission on the material (required for emission to be visible)
        emissiveMaterial.EnableKeyword("_EMISSION");


    }

    public void changeSize(float theta) {

        if(theta > 50){
            sphereSize = new Vector3(0.4f, 0.4f, 0.4f);
            transform.localScale = sphereSize;
        }
        else {
            sphereSize = new Vector3(0.2f, 0.2f, 0.2f);
            transform.localScale = sphereSize;
        }
    }


}



