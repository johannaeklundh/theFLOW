using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AdaptSize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        ResizeToWindowSize();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ResizeToWindowSize()
    {
        // Get the screen's width and height
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Set the size of your GameObject based on screen dimensions
        // Assuming your GameObject has a RectTransform component
        //RectTransform rectTransform = GetComponent<RectTransform>();

        //if (rectTransform != null)
        //{
        //    // Set the size of the RectTransform
            float convertedWidth = (float)(screenWidth * 0.2);
            float convertedHeight = (float)(screenHeight * 0.5);
        //    rectTransform.sizeDelta = new Vector2(convertedWidth, convertedHeight);
        //}
        //else
        //{
        //    UnityEngine.Debug.Log("RectTransform component not found on GameObject!");
        //}

        transform.localScale = new Vector3(convertedWidth, convertedHeight, 1f);
    }
}
