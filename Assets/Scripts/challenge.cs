using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        ShowObjects(); // Make sure this method is correctly activating/deactivating player objects
        SetUpProgressBars(); // A new method to set up progress bars
        StartGame();
    }

    // Set up active progress bars
    void SetUpProgressBars()
    {
        // Assuming each player should have one alpha and one theta bar.
        int barsToActivate = numOfPlayers * 2; // 2 bars per player

        for (int i = 0; i < progressBars.Length; i++)
        {
            if (i < barsToActivate)
            {
                progressBars[i].SetActive(true); // Activate only the required number of bars
            }
            else
            {
                progressBars[i].SetActive(false); // Deactivate the rest
            }
        }
    }

    void Update()
    {
        CheckSuccess();
    }

    // Check if the players have completed the challenge
    private void CheckSuccess()
    {
        Debug.Log("Checking active progress bars for success...");

        foreach (GameObject progressBar in progressBars)
        {
            // Check if the progressBar is active in the hierarchy
            if (progressBar != null && progressBar.activeSelf)
            {
                if (progressBar.CompareTag("alpha"))
                {
                    alpha_bar alphaBarScript = progressBar.GetComponent<alpha_bar>();
                    Debug.Log(
                        $"Checking Alpha bar {progressBar.name}, success status: {alphaBarScript.alphaSuccess}"
                    );
                    if (alphaBarScript.alphaSuccess && !countedBars.Contains(progressBar))
                    {
                        count++;
                        Debug.Log($"Counted Alpha bar: {progressBar.name}, Total count: {count}");
                        countedBars.Add(progressBar);
                        alphaBarScript.ResetSuccess(); // Reset the success flag after counting
                    }
                }
                else if (progressBar.CompareTag("theta"))
                {
                    theta_bar thetaBarScript = progressBar.GetComponent<theta_bar>();
                    Debug.Log(
                        $"Checking Theta bar {progressBar.name}, success status: {thetaBarScript.thetaSuccess}"
                    );
                    if (thetaBarScript.thetaSuccess && !countedBars.Contains(progressBar))
                    {
                        count++;
                        Debug.Log($"Counted Theta bar: {progressBar.name}, Total count: {count}");
                        countedBars.Add(progressBar);
                        thetaBarScript.ResetSuccess(); // Reset the success flag after counting
                    }
                }
            }
        }

        // Log a summary after each CheckSuccess() call
        Debug.Log($"Check complete. Total count: {count}. Counted bars: {countedBars.Count}");
        foreach (var bar in countedBars)
        {
            Debug.Log($"Counted bar: {bar.name}");
        }

        if (count == numOfPlayers * 2)
        {
            Debug.Log("Challenge completed successfully with count = " + count);
            Finish();
        }
    }

    // Starts timeBar and resets variables for CheckSuccess()
    void StartGame()
    {
        count = 0; // Reset count at the start of the game
        countedBars.Clear(); // Clear previously counted bars 

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

    // Starts the Time Bar
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

    // Function to show objects based on the value of numOfPlayers
    void ShowObjects()
    {
        Debug.Log(numOfPlayers);
        // Loop through all the objects in the objectsToShow array
        for (int i = 0; i < objectsToShow.Length; i++)
        {
            // Check if the index is less than the value of numOfPlayers
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
