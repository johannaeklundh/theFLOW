using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using EEG;

public class LevelConnected : MonoBehaviour
{
    public int howConnected = 4;
    public Material MaterialLow;
    public Material MaterialMid;
    public Material MaterialHigh;
    public GameObject[] objectsToShow;

    public GameObject[] playersGameObject;
    public EEGport[] players;
    
    void Awake(){
        
        playersGameObject = new GameObject[4];
        players = new EEGport[4];
        
        for (int index = 0; index < playersGameObject.Length; index++)
        {
            playersGameObject[index] = GameObject.Find("PlayerObject" + (index + 1).ToString());
            if (playersGameObject[index] != null)
            {
                EEGport playerComponent = playersGameObject[index].GetComponent<EEGport>();
                if (playerComponent != null)
                {
                    players[index] = playerComponent;
                }
            }
        }
    }


    void Start()
    {
        HideObjects(); // Ensure object is hidden at the start
    }

    void Update()
    {
        // Call the function to show objects based on the updated value of ConnectedPlayers
        ShowObjects();

    }

    void HideObjects()
    {
        foreach (GameObject obj in objectsToShow)
        {
            obj.SetActive(false);
        }
    }

    // Function to show objects based on the value of ConnectedPlayers
    void ShowObjects()
    {
        // fungerade tidigare
        // foreach(EEGport e in players)
        // {
        //     for(int i = 0; i < objectsToShow.Length; i++){

        //     objectsToShow[i].SetActive(i < e.barStatus);

        //         // Set the material to each object
        //         SetMaterialForObject(objectsToShow[i], e);
        //     }
        // }

        calcBars();
        //ALLA FÅR SAMMA
        // for (int i = 0; i < objectsToShow.Length; i++)
        // {
        //     GameObject obj = objectsToShow[i];

        //     // Iterate over each player to determine the bar status for the current object
        //     float maxBarStatus = 0; // Initialize max bar status
        //     foreach (EEGport player in players)
        //     {
        //         // Update maxBarStatus if the player's barStatus is higher
        //         maxBarStatus = Mathf.Max(maxBarStatus, player.barStatus);
        //     }

        //     // Set the active state of the object based on maxBarStatus
        //     obj.SetActive(i < maxBarStatus);

        //     // Set the material for the object based on maxBarStatus
        //     SetMaterialForObject(obj, maxBarStatus);
        // }


        // for(int playerIndex = 0; playerIndex < players.Length; playerIndex++){
        //     EEGport player = players[playerIndex];


        //     for(int i = 0; i < objectsToShow.Length; i++){
        //         GameObject obj = objectsToShow[i];

        //         // Set the active status of the bar based on barStatus
        //         obj.SetActive(i < player.barStatus);

        //         // Set the material for the bar
        //         SetMaterialForObject(obj, player);
        //     }
        // }
    }

    void calcBars(){
        float attention;
        float meditation;
       
        for(int index = 0; index < players.Length; index++){
            attention = players[index].getAttention();
            meditation = players[index].getMeditation();
            
            if ((70 < attention && attention <= 100) || (70 < meditation && meditation <= 100)) {
                players[index].barStatus = 4;
            }
            else if ((40 < attention && attention <= 70) || (40 < meditation && meditation <= 70)) {
                players[index].barStatus = 3;
            }
            else if ((20 < attention && attention <= 40) || (25 < meditation && meditation <= 40)) {
                players[index].barStatus = 2;
            }
            else {
                players[index].barStatus = 1;
            }
        }
        
    }

    // Function to set material for a given object
    void SetMaterialForObject(GameObject obj, EEGport player)
    {
        // Check if the object has a Renderer component
            Renderer rendererComponent = obj.GetComponent<Renderer>();
            if (rendererComponent != null)
            {
                if (player.barStatus == 1)
                {
                    // Assign the material to the object's renderer component
                    rendererComponent.material = MaterialLow;
                }
                else if (player.barStatus < 4)
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

