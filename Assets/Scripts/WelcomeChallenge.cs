using System.Collections;
using TMPro;
using UnityEngine;
using System;
using System.IO;

public class WelcomeChallenge : MonoBehaviour
{
    public TextMeshProUGUI welcomeText;
    public TextMeshProUGUI tipsText;
    public TextMeshProUGUI countdownText;
    public GameObject gameUI; // Reference to the GameUI GameObject
    public string tipsFilePath = "Assets/Tips/Tips.txt"; // Path to the text file containing tips
    private string[] tips;


    private float countdown = 20f; // 10 or 20 seconds countdown

    void Start()

    {
        // Read tips from the text file
        if (!string.IsNullOrEmpty(tipsFilePath) && File.Exists(tipsFilePath))
        {
                tips = File.ReadAllLines(tipsFilePath);
                Debug.Log("Tips loaded successfully. Number of tips: " + tips.Length);

              if (tips != null && tips.Length > 0)
            {
                string randomTip = tips[UnityEngine.Random.Range(0, tips.Length)];
                tipsText.text = randomTip;
            }
           
            else

            {
                Debug.LogError("No tips found.");
            }
        }
        
        else
        {
            Debug.LogError("Tips file not found or path is not provided.");
    
        }

        
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
           
           
        }
        else
        {
            Debug.LogError("gameUI is not assigned in the Inspector");
        }
    }
}
