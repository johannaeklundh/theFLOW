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

    private bool[][] alphaUpdated;
    private bool[][] thetaUpdated;
   
    //public Image mask; 
   public Image alphaMask;
   public Image thetaMask;
    [HideInInspector] public int numPlayers;


    public float updateInterval = 0.1f; 

    void Start()
    {
        // Generate fake data for player alpha and theta arrays
        GenerateFakeData();

   
       
    }

 void Update()
{
    if(alphaMask != null && thetaMask == null)
    {
        Debug.LogError(" Theta mask is not assigned.");
        return;
    } else if(alphaMask == null && thetaMask != null) {

        Debug.LogError("Alpha mask is not assigned.");
    } else if (alphaMask == null && thetaMask == null) {

         Debug.LogError("no mask assigned.");
    } else {

        StartCoroutine(UpdateFillAmounts());
    }

    
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
            playerAlphaData[i][j] = Random.Range(0, 50); // Random value between 0 and 100
            playerThetaData[i][j] = Random.Range(0, 101); // Random value between 0 and 100
             Debug.Log("Player " + (i + 1) + " Alpha Data[" + j + "]: " + playerAlphaData[i][j]);
           // Debug.Log("Player " + (i + 1) + " Theta Data[" + j + "]: " + playerThetaData[i][j]);
        
        }
    }
}

 

 public IEnumerator UpdateFillAmounts()
{
    for (int i = 0; i < numPlayers; i++)
    {
        GameObject playerObject = GameObject.Find("Player" + i); // Assuming players are named Player0, Player1, etc.
        string tag = playerObject.tag;

        for (int j = 0; j < 10; j++)
        {
            if (!alphaUpdated[i][j] && playerAlphaData[i][j] >= 60)
            {
                alphaUpdated[i][j] = true;
                if (tag == "alpha")
                    StartCoroutine(InterpolateFillAmount(alphaMask, 0.6f, "Alpha", i, j));
            }

            if (!thetaUpdated[i][j] && playerThetaData[i][j] >= 60)
            {
                thetaUpdated[i][j] = true;
                if (tag == "theta")
                    StartCoroutine(InterpolateFillAmount(thetaMask, 0.6f, "Theta", i, j));
            }

            // Start interpolation for alpha and theta bars if they haven't been permanently updated
            if (!alphaUpdated[i][j] && tag == "alpha")
            {
                float alphaTargetFillAmount = (float)playerAlphaData[i][j] / (float)maxVal;
                StartCoroutine(InterpolateFillAmount(alphaMask, alphaTargetFillAmount, "Alpha", i, j));
            }
            
            if (!thetaUpdated[i][j] && tag == "theta")
            {
                float thetaTargetFillAmount = (float)playerThetaData[i][j] / (float)maxVal;
                StartCoroutine(InterpolateFillAmount(thetaMask, thetaTargetFillAmount, "Theta", i, j));
            }
        }
        
        yield return new WaitForSeconds(updateInterval);
    }
}


    IEnumerator InterpolateFillAmount(Image mask, float targetFillAmount, string barName, int playerIndex, int dataIndex)
    {
        if ((barName == "Alpha" && alphaUpdated[playerIndex][dataIndex]) || (barName == "Theta" && thetaUpdated[playerIndex][dataIndex]))
        {
            mask.fillAmount = targetFillAmount;
            yield break;
        }

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


