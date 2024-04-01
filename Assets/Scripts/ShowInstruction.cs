using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideObject : MonoBehaviour
{
    public GameObject objectToShowHide;

    void Start()
    {
        HideObject(); // Ensure object is hidden at the start
    }

    public void ToggleObjectVisibility()
    {
        objectToShowHide.SetActive(!objectToShowHide.activeSelf); // Toggle object visibility
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Check if the click was outside the object
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);
            if (hit.collider == null)
            {
                HideObject(); // Hide object if clicked outside
            }
        }
    }

    void HideObject()
    {
        if (objectToShowHide.activeSelf)
        {
            objectToShowHide.SetActive(false);
        }
    }
}
