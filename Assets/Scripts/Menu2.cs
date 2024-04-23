using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu2 : MonoBehaviour
{
    public int ConnectedPlayers = 0;

    public GameObject[] objectsToShow;

    void Start()
    {
        //ShowObjects(); // Ensure object is hidden at the start
    }

    void Update()
    {
        // Call the function to show objects based on the updated value of ConnectedPlayers
        //ShowObjects();
    }

    // Function to show objects based on the value of ConnectedPlayers
    //void ShowObjects()
    //{
    //    // Loop through all the objects in the objectsToShow array
    //    for (int i = 0; i < objectsToShow.Length; i++)
    //    {
    //        // Check if the index is less than the value of ConnectedPlayers
    //        // If so, activate the game object
    //        if (i < ConnectedPlayers)
    //        {
    //            objectsToShow[i].SetActive(true);
    //        }
    //        // If not, deactivate the game object
    //        else
    //        {
    //            objectsToShow[i].SetActive(false);
    //        }
    //    }
    //}

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(2); 
    }

    public void Back()
    {
        SceneManager.LoadSceneAsync(0);
    }


}
