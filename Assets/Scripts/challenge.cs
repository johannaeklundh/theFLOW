using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class challenge : MonoBehaviour
{
    public void Finish()
    {
        SceneManager.LoadSceneAsync(2); // Can also use 1;
    }
}
