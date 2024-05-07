using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Darkener : MonoBehaviour
{
    public float duration = 2.0f; // Duration of the fade
    private Image darkeningImage;
    private float timeElapsed;
    private bool shouldDarken = false; // Controls when the fade starts

    void Start()
    {
        darkeningImage = GetComponent<Image>();
        darkeningImage.color = new Color(0, 0, 0, 0); // Ensure it starts fully transparent
    }

    void Update()
    {
        // Check if the 'D' key is pressed
        if (Input.GetKeyDown(KeyCode.D))
        {
            shouldDarken = true; // Start darkening when 'D' is pressed
            timeElapsed = 0; // Reset the timer
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

