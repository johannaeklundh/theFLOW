using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brightener : MonoBehaviour
{
    public float duration = 2.0f; // Duration of the fade
    private Image brighteningImage;
    private float timeElapsed;
    private bool shouldBrighten = false; // Controls when the fade starts

    void Start()
    {
        brighteningImage = GetComponent<Image>();
        brighteningImage.color = new Color(0, 0, 0, 0); // Ensure it starts fully transparent
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            shouldBrighten = true; // Start bright when 'W' is pressed
            timeElapsed = 0; // Reset the timer
        }

        if (shouldBrighten && timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(timeElapsed / duration) / 2;
            brighteningImage.color = new Color(255, 255, 255, alpha);
        }
    }
}

