using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

[ExecuteInEditMode()]
public class progress_bar1 : MonoBehaviour
{
    public int maxVal = 100;
    public int[][] playerAlphaData;
    public int[][] playerThetaData;
   

    public Image mask; 
    [HideInInspector] public int numPlayers;


    public float updateInterval = 0.1f; 

    void Start()
    {
        // Generate fake data for player alpha and theta arrays
        GenerateFakeData();
       
    }

    void Update()
    {
        // Update the fill amounts of the progress bars
        StartCoroutine(UpdateFillAmounts());
    }

    public int getNumOfPlayers (int value) {

        numPlayers = value; 

        return numPlayers;

    }
void GenerateFakeData()
{
    // Initialize arrays for four players
    playerAlphaData = new int[numPlayers][];
    playerThetaData = new int[numPlayers][];
  

    // Initialize data and masks for each player
    for (int i = 0; i < numPlayers; i++)
    {
        playerAlphaData[i] = new int[10]; //  10 elements in the array
        playerThetaData[i] = new int[10]; //  10 elements in the array
        
        for (int j = 0; j < 10; j++)
        {
            playerAlphaData[i][j] = Random.Range(0, 101); // Random value between 0 and 100
            playerThetaData[i][j] = Random.Range(0, 101); // Random value between 0 and 100
            // Debug.Log("Player " + (i + 1) + " Alpha Data[" + j + "]: " + playerAlphaData[i][j]);
           // Debug.Log("Player " + (i + 1) + " Theta Data[" + j + "]: " + playerThetaData[i][j]);
        
        }
    }
}

public IEnumerator UpdateFillAmounts()
{
    //Debug.Log(numPlayers);

    // Update fill amount for each player's alpha and theta bars
    for (int i = 0; i < numPlayers; i++)
    {
        for (int j = 0; j < 10; j++) // Assuming 10 elements in each player's data array
        {
           // Debug.Log("Player " + (i + 1) + ", Bar " + (j + 1));


            // Update fill amount of alpha progress bar
            float alphaTargetFillAmount = (float)playerAlphaData[i][j] / (float)maxVal;
            StartCoroutine(InterpolateFillAmount(mask, alphaTargetFillAmount));

            // Update fill amount of theta progress bar
            float thetaTargetFillAmount = (float)playerThetaData[i][j] / (float)maxVal;
            StartCoroutine(InterpolateFillAmount(mask, thetaTargetFillAmount));

            // Pause briefly between updates 
            yield return new WaitForSeconds(updateInterval);
        }
    }
}


    IEnumerator InterpolateFillAmount(Image mask, float targetFillAmount)
    {
        float currentFillAmount = mask.fillAmount;
        float elapsedTime = 0f;
        float duration = 0.5f; 

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            mask.fillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, t);
            yield return null;
        }

        mask.fillAmount = targetFillAmount; // Ensure final fill amount is set accurately
    }


}
