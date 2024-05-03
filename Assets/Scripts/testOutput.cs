using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EEG;
using TMPro;

public class testOutput : MonoBehaviour
{

    public GameObject player1;
    public EEGport p1;
    public TextMeshProUGUI stat;

    void Awake(){
         player1 = GameObject.Find("PlayerObject1");
         stat.text = "Testing";
    }
    // Start is called before the first frame update
    void Start()
    {
       if (player1 != null)
        {
        //Access the Player component attached to the player GameObject
            EEGport playerComponent = player1.GetComponent<EEGport>();

            if (playerComponent != null)
            {
                // Access properties or methods of the Player component
                p1 = playerComponent;
            }
        } 
    }

    // Update is called once per frame
    void Update()
    {
       stat.text = p1.getAttention().ToString();
       Debug.Log(p1.getAttention());
    }
}
