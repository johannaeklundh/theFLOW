using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brightener : MonoBehaviour
{
    public float duration = 2.0f; // Duration of the fade
    private Image brightImage;
    private float timeElapsed;
    private bool shouldBright = false; // Controls when the fade starts

    void Start()
    {
        brightImage = GetComponent<Image>();
        brightImage.color = new Color(0, 0, 0, 0); // Ensure it starts fully transparent
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            shouldBright = true; // Start darkening when 'W' is pressed
            timeElapsed = 0; // Reset the timer
        }

        if (shouldBright && timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(timeElapsed / duration) / 2;
            brightImage.color = new Color(255, 255, 255, alpha);
        }
    }
}


