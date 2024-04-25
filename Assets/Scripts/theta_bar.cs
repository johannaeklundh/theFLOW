using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class theta_bar : MonoBehaviour
{
    public int maxVal = 100;
    public int[] playerThetaData;
    public Image thetaMask; // Each GameObject should have this set in the inspector to its unique Image component
    public float updateInterval = 0.1f;
    [HideInInspector] public int numPlayers = 1; // Adjust as necessary if managing multiple players per GameObject
    private bool runChallenge = false;
    public bool thetaSuccess = false;

    void Start()
    {
        GenerateFakeData();
        runChallenge = true; 
        StartCoroutine(UpdateFillAmounts());
    }

   

    // Fake data för att testa
    void GenerateFakeData()
    {
        // Initialize arrays for players
        playerThetaData = new int[10];
        int[] specificValues = {0, 1, 3, 10, 19, 39, 20, 22, 90, 2}; // Preset values for theta

        for (int i = 0; i < 10; i++)
        {
        
             playerThetaData[i]= Random.Range(0, 101); // Assign the specific values instead of random values
            
        }
    }

   

    
    IEnumerator UpdateFillAmounts()
    {
        while (runChallenge) // This will keep updating in a loop
        {
           
                for (int i = 0; i < 10; i++)
                {
                     float thetaTargetFillAmount = (float)playerThetaData[i] / (float)maxVal;

                    if (playerThetaData[i] >= 60)
                    {
                       
                       thetaMask.fillAmount = thetaTargetFillAmount;
                        runChallenge = false; 
                        thetaSuccess = true; 
                         StopAllCoroutines();
                         break; 

                    } else {

                        
                        // Perform interpolation if the value is less than 60
                        yield return StartCoroutine(InterpolateFillAmount(thetaMask, thetaTargetFillAmount, i));
                    }
                }
            
            yield return new WaitForSeconds(updateInterval); // Wait before updating again
        }
    }


    IEnumerator InterpolateFillAmount(Image mask, float targetFillAmount, int dataIndex)
    {
        float currentFillAmount = mask.fillAmount;
        float elapsedTime = 0f;
        float duration = 0.5f; // Duration over which the fill amount is interpolated

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            mask.fillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, t);
            yield return null;
        }

        mask.fillAmount = targetFillAmount;
    }

    public void ResetSuccess() {
    thetaSuccess = false;
}

}
