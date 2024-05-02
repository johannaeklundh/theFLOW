using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    void Start()
    {

        // Ensure that this GameObject persists across scene changes
        DontDestroyOnLoad(gameObject);

        // Play the audio clip attached to the AudioSource component
        GetComponent<AudioSource>().Play();
    }
}
