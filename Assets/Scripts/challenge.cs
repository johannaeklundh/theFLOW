using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

[ExecuteInEditMode]
public class challenge : MonoBehaviour
{
   
    public Image timeBar; // Reference to the time bar Image
    public float timeLimit = 60f; // Time limit for the challenge
    public GameObject[] objectsToShow;
    public int numOfPlayers; 
    public progress_bar1[] progressBars; // Declare the array

    // This method will be called to finish the challenge
    public void Finish()
    {
        SceneManager.LoadSceneAsync(2); // Load scene 2 asynchronously
    }

  

    void Start()
    {
       
        SendIntegerToProgressBars(); 
        ShowObjects();
    }

    // Method to send the integer value to all progress bars
   private void SendIntegerToProgressBars()
{
    if (progressBars == null)
    {
        Debug.LogError("Progress bars array not initialized.");
        return;
    }

    foreach (progress_bar1 progressBar in progressBars)
    {
        if (progressBar != null)
        {
            progressBar.getNumOfPlayers(numOfPlayers);
        }
        else
        {
            Debug.LogError("Progress bar reference not set in the array.");
        }
    }
}


    public void StartGame()
    {
       
        if (timeBar != null)
        {
            StartCoroutine(StartTimeBar());
            Debug.Log("Time bar starting"); 
        }
        else
        {
            Debug.LogError("Time bar Image is not assigned in the inspector!");
        }
    }


    public IEnumerator StartTimeBar()
    {
        float elapsed = 0f;

        Debug.Log("Timebar called"); 

        while (elapsed < timeLimit)
        {
            elapsed += Time.deltaTime;
            timeBar.fillAmount = 1 - (elapsed / timeLimit);
            yield return null;
        }

        timeBar.fillAmount = 0;
        Finish(); // End the challenge when the time runs out
    }

    // Function to show objects based on the value of ConnectedPlayers
    void ShowObjects()
    {
        Debug.Log(numOfPlayers);
        // Loop through all the objects in the objectsToShow array
        for (int i = 0; i < objectsToShow.Length; i++)
        {
            // Check if the index is less than the value of ConnectedPlayers
            // If so, activate the game object
            if (i < numOfPlayers)
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

}