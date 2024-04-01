using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Aftermath : MonoBehaviour
{
    //public gameObject TransparentBackground;

    void Start()
    {
        //TransparentBackground = Get
    }

    public void StartMenu ()
    {
         SceneManager.LoadSceneAsync(0); // Can also use 1;
    }

    public void ConnectMenu()
    {
        SceneManager.LoadSceneAsync(1); // Can also use 1;
    }

    public void ReStart()
    {
        SceneManager.LoadSceneAsync(2); // Can also use 1;
    }

}