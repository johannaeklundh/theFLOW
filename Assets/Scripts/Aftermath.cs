using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Aftermath: MonoBehaviour
{
    public int ConnectedPlayers = 0;

    public GameObject[] objectsToShow;

    public bool win = false;
    public GameObject[] HeaderToShow;

    public gamePlay GP;

    void Start()
    {
        win = gamePlay.playersWon(GP);
        ShowHeader(); // Ensure object is hidden at the start
        ShowObjects(); // Ensure object is hidden at the start
    }

    void Update()
    {
        // Call the function to show objects based on the updated value of ConnectedPlayers
        ShowObjects();
        ShowHeader();

    }

    // Function to show objects based on the value of ConnectedPlayers
    void ShowHeader()
    {
        // Loop through all the objects in the objectsToShow array
        if (win)
        {
            //set Victory to true
            HeaderToShow[0].SetActive(true);
            //set Lost to false
            HeaderToShow[1].SetActive(false);
        }
        else
        {
            //set Victory to false
            HeaderToShow[0].SetActive(false);
            //set Lost to true
            HeaderToShow[1].SetActive(true);

        }
    }

    // Function to show objects based on the value of ConnectedPlayers
    void ShowObjects()
    {
        // Loop through all the objects in the objectsToShow array
        for (int i = 0; i < objectsToShow.Length; i++)
        {
            // Check if the index is less than the value of ConnectedPlayers
            // If so, activate the game object
            if (i < ConnectedPlayers)
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



    public void StartMenu()
    {
        SceneManager.LoadScene(0); // Can also use 1;
        
    }

    public void ConnectMenu()
    {
        SceneManager.LoadScene(1); // Can also use 1;
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(5); // Can also use 1;
       
    }

}