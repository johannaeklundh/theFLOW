using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGame : MonoBehaviour
{
    public void Finish()
    {
        SceneManager.LoadSceneAsync(3); // Can also use 1;
    }
}
