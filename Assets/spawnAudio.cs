using UnityEngine;

public class SingletonLoader : MonoBehaviour
{
    public GameObject objectToLoad;

    void Awake()
    {
        //Creating many audioplayers is bad programming practice!!!!
        if (GameObject.FindWithTag(objectToLoad.tag) == null)
        {
            
            GameObject instance = Instantiate(objectToLoad);
        
            DontDestroyOnLoad(instance);
        }
        else
        {
            // If an object with this tag exists, we do nothing or destroy the current object if needed
            Debug.Log("lul");
        }
    }
}