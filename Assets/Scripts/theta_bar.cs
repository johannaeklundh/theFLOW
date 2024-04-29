using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class theta_bar : MonoBehaviour
{
    //Variables
    public int maxVal = 100;
    public int[] playerThetaData;
    public Image thetaMask; // Each GameObject should have this set in the inspector to its unique Image component
    public float updateInterval = 0.1f;

    [HideInInspector]
    public int numPlayers = 1; // Adjust in editor

    public RectTransform lineIndicator; // Add a reference to the RectTransform of the line indicator

    public bool runChallenge = true;
    public bool thetaSuccess = false;

    void Start()
    {
        GenerateFakeData();
        runChallenge = true;
        StartCoroutine(UpdateFillAmounts());
        SetLineIndicatorPosition();
    }

    // ÄNDRA
    void GenerateFakeData()
    {
        // Initialize arrays for players
        playerThetaData = new int[10];
        int[] specificValues = { 0, 1, 3, 10, 19, 39, 20, 22, 90, 2 }; // Preset values for theta

        for (int i = 0; i < 10; i++)
        {
            playerThetaData[i] = Random.Range(0, 101); // Assign the specific values instead of random values
        }
    }

    //Fill the bars depending on the playerData
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
                }
                else
                {
                    // Perform interpolation if the value is less than 60
                    yield return StartCoroutine(
                        InterpolateFillAmount(thetaMask, thetaTargetFillAmount, i)
                    );
                }
            }

            yield return new WaitForSeconds(updateInterval); // Wait before updating again
        }
    }

    //Interpolat the fillAmount for smooth trasition
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
        thetaSuccess = false;
    }

    
}
