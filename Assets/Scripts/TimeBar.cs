using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class TimeBar : MonoBehaviour
{
    
    /********Refrence instances to other classes/scripts********/
    public gamePlay GP;
    public AIScript AI;

    /***********************************************************/

    public Image timeBar; // Reference to the time bar Image
    public float timeLimit = 4*60f; // Time limit for the challenge

    public void Finish()
    {
        
        /*/ Make all update-functions in gamePlay and AI unable to update
        GP.canUpdate = false;
        GP.canUpdate1sec = false;
        GP.canUpdate10sec = false;
        AI.canUpdate = false;*/

        gamePlay.playersWon(GP);

        //SceneManager.LoadSceneAsync(3); // Load scene 2 asynchronously
        SceneManager.LoadScene(4);
       
    }

   
    void Start()
    {
        StartCoroutine(StartTimeBar());
    }

 
    void Update() { }

    public IEnumerator StartTimeBar()
    {
        float elapsed = 0f;

        Debug.Log("Timebar coroutine started");

        while (elapsed < timeLimit)
        {
            elapsed += Time.deltaTime;
            float newFillAmount = 1 - (elapsed / timeLimit);
            timeBar.fillAmount = newFillAmount;
            // Debug.Log($"Timebar update: {newFillAmount}");
            yield return null;
        }

        timeBar.fillAmount = 0;
        Finish();
    }
}
