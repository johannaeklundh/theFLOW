using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenDarkener : MonoBehaviour
{
    public float duration = 2.0f; // Duration of the fade
    private Image darkeningImage;
    private float timeElapsed;
    private bool shouldDarken = false; // Controls when the fade starts

    void Start()
    {
        darkeningImage = GetComponent<Image>();
        darkeningImage.color = new Color(0, 0, 0, 0); //transparent
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) //replace this with when players have lost.
        {
            shouldDarken = true; 
            timeElapsed = 0; 
        }

        // Process darkening if shouldDarken is true
        if (shouldDarken && timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(timeElapsed / duration)/2;
            darkeningImage.color = new Color(0, 0, 0, alpha);
        }
    }
}
