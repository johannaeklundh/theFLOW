using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelConnected : MonoBehaviour
{
    public int howConnected = 4;
    public Material MaterialLow;
    public Material MaterialMid;
    public Material MaterialHigh;
    public GameObject[] objectsToShow;

    void Start()
    {
        ShowObjects(); // Ensure object is hidden at the start
    }

    void Update()
    {
        // Call the function to show objects based on the updated value of ConnectedPlayers
        ShowObjects();
    }

    // Function to show objects based on the value of ConnectedPlayers
    void ShowObjects()
    {
        
        
        foreach (GameObject obj in objectsToShow)
        {
            // Set the material to each object
            SetMaterialForObject(obj);
        }

        // Loop through all the objects in the objectsToShow array
        for (int i = 0; i < objectsToShow.Length; i++)
        {
            // Check if the index is less than the value of ConnectedPlayers
            // If so, activate the game object
            if (i < howConnected)
            {
                objectsToShow[i].SetActive(true);
            }
            // If not, deactivate the game object
            else
            {
                objectsToShow[i].SetActive(false);
            }
        }
    }

    // Function to set material for a given object
    void SetMaterialForObject(GameObject obj)
    {
        // Check if the object has a Renderer component
        Renderer rendererComponent = obj.GetComponent<Renderer>();
        if (rendererComponent != null)
        {
            if (howConnected == 1)
            {
                // Assign the material to the object's renderer component
                rendererComponent.material = MaterialLow;
            }
             else if (howConnected < 4)
            {
                rendererComponent.material = MaterialMid;
            }
            else
            {
                rendererComponent.material = MaterialHigh;
            }
            
        }
        else
        {
            // Log a warning if the object does not have a Renderer component
            UnityEngine.Debug.LogWarning("Renderer component not found on GameObject: " + obj.name);
        }
    }
}

