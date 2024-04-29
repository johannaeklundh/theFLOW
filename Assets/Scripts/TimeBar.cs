using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

[ExecuteInEditMode()]

public class TimeBar : MonoBehaviour
{

    public Image timeBar; // Reference to the time bar Image
    public float timeLimit = 60f; // Time limit for the challenge

 public void Finish()
    {
        SceneManager.LoadSceneAsync(2); // Load scene 2 asynchronously
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartTimeBar());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public IEnumerator StartTimeBar() {
       
        float elapsed = 0f;

         Debug.Log("Timebar coroutine started");

         while (elapsed < timeLimit) {
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
