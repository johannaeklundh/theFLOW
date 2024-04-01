using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameIsOver : MonoBehaviour
{

    public bool win = false;
    public GameObject[] objectsToShow;

    

    void Start()
    {
        ShowObjects(); // Ensure object is hidden at the start
        // Start the coroutine to switch scenes after 15 seconds
        StartCoroutine(SwitchSceneAfterDelay());
    }

    void Update()
    {
        // Call the function to show objects based on the updated value of ConnectedPlayers
        ShowObjects();
    }

    IEnumerator SwitchSceneAfterDelay()
    {
        // Wait for 15 seconds
        yield return new WaitForSeconds(15f);

        // Load the next scene
        SceneManager.LoadSceneAsync(4);
    }

    // Function to show objects based on the value of ConnectedPlayers
    void ShowObjects()
    {
        // Loop through all the objects in the objectsToShow array
       if(win)
        {
            //set Victory to true
            objectsToShow[0].SetActive(true);
            //set Lost to false
            objectsToShow[1].SetActive(false);
        }
        else
        {
            //set Victory to false
            objectsToShow[0].SetActive(false);
            //set Lost to true
            objectsToShow[1].SetActive(true);

        }
    }

    public void ToAfterMath()
    {
        SceneManager.LoadSceneAsync(4); // Can also use 1;
    }
    


}
