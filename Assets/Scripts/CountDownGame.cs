using System.Collections;
using UnityEngine;
using TMPro;

public class CountDownGame : MonoBehaviour
{
    public TextMeshProUGUI countdownText;

    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        int count = 3;

        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f); // Wait for 1 second
            count--;
        }

        // After the countdown finishes, start the game or perform any desired action
        countdownText.text = "Go!";
        yield return new WaitForSeconds(1f); // Wait for 1 second
        countdownText.text = "";
        StartGame(); // Replace this with your game starting logic
    }

    void StartGame()
    {
        // Add your game starting logic here
        UnityEngine.Debug.Log("Game started!");
    }
}
