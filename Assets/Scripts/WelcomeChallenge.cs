using System.Collections;
using TMPro;
using UnityEngine;

public class WelcomeChallenge : MonoBehaviour
{
    public TextMeshProUGUI welcomeText;
    public TextMeshProUGUI countdownText;
    public GameObject gameUI; // Reference to the GameUI GameObject

    private float countdown = 10f; // 10 seconds countdown

    void Start()
    {
        //welcomeText.text = "Welcome to the rematch! Try to get your alpha and theta values to..";
        countdownText.text = countdown.ToString("F0");
        StartCoroutine(CountdownAndStart());
    }

    IEnumerator CountdownAndStart()
    {
        while (countdown > 0)
        {
            yield return new WaitForSeconds(1f);
            countdown -= 1f;
            countdownText.text = countdown.ToString("F0");
        }

        // When countdown is over, hide the welcome panel and activate the GameUI
        gameObject.SetActive(false); // Deactivates the welcome panel
       
        if (gameUI != null)
        {
            gameUI.SetActive(true); // Activates the GameUI

            /*challenge challengeScript = gameUI.GetComponent<challenge>();
            if (challengeScript != null)
            {
                challengeScript.StartGame(); // Should call the public method
            }
            else
            {
                Debug.LogError("Challenge script not found on gameUI GameObject");
            } */
        }
        else
        {
            Debug.LogError("gameUI is not assigned in the Inspector");
        }
    }
}
