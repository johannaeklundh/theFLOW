using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class alpha_bar : MonoBehaviour
{
    public int maxVal = 100;
    public int[]playerAlphaData;
    public Image alphaMask; // Each GameObject should have this set in the inspector to its unique Image component
    public float updateInterval = 0.1f;
    [HideInInspector] public int numPlayers;
    private bool runChallenge = false;
    public bool alphaSuccess = false; 

   void Start() {

    GenerateFakeData();
    runChallenge = true;
    StartCoroutine(UpdateFillAmounts());
        
}

   void GenerateFakeData()
{
  

      playerAlphaData = new int[10]; // Declare an integer array with 10 elements


        for (int j = 0; j < 10; j++)
        {
            playerAlphaData[j] = Random.Range(0, 101); // Random value between 0 and 100
            Debug.Log("Player " + " Alpha Data[" + j + "]: " + playerAlphaData[j]);
        }
    
}


 

IEnumerator UpdateFillAmounts()
{
    while (runChallenge)

    {
       
            for (int j = 0; j < 10; j++)
            {
    
                    float alphaTargetFillAmount = (float)playerAlphaData[j] / maxVal;

                    // Check if the value is 60 or greater
                    if (playerAlphaData[j] >= 60)
                    {
                        alphaMask.fillAmount = alphaTargetFillAmount;
                        runChallenge = false; 
                        alphaSuccess = true; 
                         StopAllCoroutines();
                         break; 

                    }
                    else
                    {
                        // Perform interpolation if the value is less than 60
                        yield return StartCoroutine(InterpolateFillAmount(alphaMask, alphaTargetFillAmount, j));
                        
                    }
            }
        
        yield return new WaitForSeconds(updateInterval);
    }
}

IEnumerator InterpolateFillAmount(Image mask, float targetFillAmount, int dataIndex)
{
    //Debug.Log("Interpolation start"); 
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

    mask.fillAmount = targetFillAmount;  // Ensure final fill amount is set accurately
}

public void ResetSuccess() {
    alphaSuccess = false;
}




}
