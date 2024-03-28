using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu2 : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(2); // Can also use 1;
    }

    public void Back()
    {
        SceneManager.LoadSceneAsync(0); // Can also use 1;
    }
}
