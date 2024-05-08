using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class alpha_bar : MonoBehaviour
{
    public int maxVal = 100;
    public int[] playerAlphaData;
    public Image alphaMask; // Each GameObject should have this set in the inspector to its unique Image component
    public float updateInterval = 0.1f;
    public RectTransform lineIndicator; // Add a reference to the RectTransform of the line indicator
    public float currentVal; 
    [HideInInspector]
    public int numPlayers;
    private bool runChallenge = false;
    public bool alphaSuccess = false;

    void Start()
    {
        //GenerateFakeData();
        runChallenge = true;
        StartCoroutine(UpdateFillAmounts());
         SetLineIndicatorPosition();
    }

    //ÄNDRA
    /*void GenerateFakeData()
    {
        playerAlphaData = new int[10]; // Declare an integer array with 10 elements

        for (int j = 0; j < 10; j++)
        {
            playerAlphaData[j] = Random.Range(0, 50); // Random value between 0 and 100
            Debug.Log("Player " + " Alpha Data[" + j + "]: " + playerAlphaData[j]);
        }
    }

    */

    // Get alpha data 
    public void getAlphaData(float value) {

       currentVal = value; 

        Debug.Log("Alpha value updated: " + value);
    
    }

    // Update the bars
    IEnumerator UpdateFillAmounts()
    {
        while (runChallenge)
        {
            
                float alphaTargetFillAmount = currentVal / maxVal;

                // Check if the value is 60 or greater
                if (currentVal >= 100)
                {
                    alphaMask.fillAmount = alphaTargetFillAmount;
                    runChallenge = false;
                    alphaSuccess = true;
                    StopAllCoroutines();
                    yield break;
                }
                else
                {
                    // Perform interpolation if the value is less than 60
                    yield return StartCoroutine(
                        InterpolateFillAmount(alphaMask, alphaTargetFillAmount)
                    );
                }
            }

            yield return new WaitForSeconds(updateInterval);
        }
    


    //Interpolating 

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

      
    // Set the position of the line indicator at 60% horizontally across the parent container
void SetLineIndicatorPosition()
{
    if (lineIndicator != null)
    {
        // Calculate 60% of the bar's height for vertical positioning
        float barHeight = lineIndicator.parent.GetComponent<RectTransform>().rect.height;
        float sixtyPercentHeight = barHeight * 0.6f;

        // Set the top and bottom offsets to move the line to the 60% height mark
        // Since the line is stretching, we're effectively moving its central line to 60%
        // by equally increasing the top and decreasing the bottom offsets
        float offset = (1.0f - 0.6f) * barHeight; // Calculate the offset needed to move the line to 60%
        lineIndicator.offsetMin = new Vector2(lineIndicator.offsetMin.x, offset);
        lineIndicator.offsetMax = new Vector2(lineIndicator.offsetMax.x, -offset);

        // Center the line horizontally, assuming that the pivot.x is 0.5
        // This would mean the offset left and right should be the same
        // to keep the line in the center
        float width = lineIndicator.rect.width; // Assuming this is the thickness of your line
        float horizontalOffset = (lineIndicator.parent.GetComponent<RectTransform>().rect.width - width) * 0.5f;
        lineIndicator.offsetMin = new Vector2(horizontalOffset, lineIndicator.offsetMin.y);
        lineIndicator.offsetMax = new Vector2(-horizontalOffset, lineIndicator.offsetMax.y);
    }
    else
    {
        Debug.LogError("Line Indicator is not assigned.");
    }
}

    public void ResetSuccess()
    {
        alphaSuccess = false;
    }
}
