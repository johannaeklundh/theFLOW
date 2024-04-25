using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic; 

[ExecuteInEditMode]
public class challenge : MonoBehaviour
{
   
    public Image timeBar; // Reference to the time bar Image
    public float timeLimit = 60f; // Time limit for the challenge
    public GameObject[] objectsToShow;
    public int numOfPlayers; 
    public GameObject[] progressBars;
    public int count = 0; 
      private HashSet<GameObject> countedBars = new HashSet<GameObject>();

    // This method will be called to finish the challenge
    public void Finish()
    {
        SceneManager.LoadSceneAsync(2); // Load scene 2 asynchronously
    }


    void Start()
    {
         
       
        //SendIntegerToProgressBars(); 
        ShowObjects();
    }

    void Update () {

       CheckSuccess(); 

    }

private void CheckSuccess() {
    Debug.Log($"Checking progress bars; Current count: {count}");

    foreach (GameObject progressBar in progressBars) {
        if (progressBar != null) {
            if (progressBar.CompareTag("alpha")) {
                alpha_bar alphaBarScript = progressBar.GetComponent<alpha_bar>();
                if (alphaBarScript.alphaSuccess && !countedBars.Contains(progressBar)) {
                    count++;
                    countedBars.Add(progressBar);
                    Debug.Log($"Counted Alpha bar: {progressBar.name}, Total count: {count}");
                    alphaBarScript.ResetSuccess(); // Reset the success flag
                }
            } else if (progressBar.CompareTag("theta")) {
                theta_bar thetaBarScript = progressBar.GetComponent<theta_bar>();
                if (thetaBarScript.thetaSuccess && !countedBars.Contains(progressBar)) {
                    count++;
                    Debug.Log($"Counted Theta bar: {progressBar.name}, Total count: {count}");
                    countedBars.Add(progressBar);
                    thetaBarScript.ResetSuccess(); // Reset the success flag
                }
            }
        }
    }

    if(count == numOfPlayers * 2) {
        Debug.Log("Challenge completed successfully with count = " + count);
        //Finish();
    }
}




    
   



    void StartGame()
    {
        count = 0;  // Reset count at the start of the game
    countedBars.Clear();  // Clear previously counted bars if needed
       
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


   IEnumerator StartTimeBar()
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






