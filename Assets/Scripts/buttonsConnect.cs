using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EEG;

public class buttonsConnect : MonoBehaviour
{
    // public TextMeshProUGUI p1Stats;
    public TextMeshProUGUI p2Stats;
    // public TextMeshProUGUI p3Stats;
    // public TextMeshProUGUI p4Stats;
    // public Button p1Connect;
    // public Button p1Disconnect;
    // public Button p2Connect;
    // public Button p2Disconnect;
    // public Button p3Connect;
    // public Button p3Disconnect;
    // public Button p4Connect;

    // public Button deleteCon;
    // public Button p4Disconnect;
    // public GameObject player1;
    // public GameObject player2;
    // public GameObject player3;
    // public GameObject player4;
    // public EEGport p1;
    // public EEGport p2;
    // public EEGport p3;
    // public EEGport p4;

    //public TextMeshProUGUI[] stats;
    public Button[] connectButtons;
    public Button[] disconnectButtons;
    public GameObject[] playersGameObject;
    public EEGport[] players;
    public Button deleteCon;

    

    void Awake()
    {  
        
        // p1Disconnect.gameObject.SetActive(false);
        // p2Disconnect.gameObject.SetActive(false);
        // p3Disconnect.gameObject.SetActive(false);
        // p4Disconnect.gameObject.SetActive(false);
        // player1 = GameObject.Find("PlayerObject1");
        // player2 = GameObject.Find("PlayerObject2");
        // player3 = GameObject.Find("PlayerObject3");
        // player4 = GameObject.Find("PlayerObject4");

        // if (player1 != null)
        // {
        // // Access the Player component attached to the player GameObject
        //     EEGport playerComponent = player1.GetComponent<EEGport>();

        //     if (playerComponent != null)
        //     {
        //         // Access properties or methods of the Player component
        //         p1 = playerComponent;
        //     }
        // }
    
        // if (player2 != null)
        // {
        //     // Access the Player component attached to the player GameObject
        //     EEGport playerComponent = player2.GetComponent<EEGport>();

        //     if (playerComponent != null)
        //     {
        //             // Access properties or methods of the Player component
        //         p2 = playerComponent;
        //     }
        // }
        
        // if (player3 != null)
        // {
        //     // Access the Player component attached to the player GameObject
        //     EEGport playerComponent = player3.GetComponent<EEGport>();

        //     if (playerComponent != null)
        //     {
        //         // Access properties or methods of the Player component
        //         p3 = playerComponent;
        //     }
        // }
        
        // if (player4 != null)
        // {
        //     // Access the Player component attached to the player GameObject
        //     EEGport playerComponent = player4.GetComponent<EEGport>();

        //     if (playerComponent != null)
        //     {
        //             // Access properties or methods of the Player component
        //         p4 = playerComponent;
        //     }
        // }
        playersGameObject = new GameObject[4];
        players = new EEGport[4];
        
        for (int index = 0; index < playersGameObject.Length; index++)
        {
            disconnectButtons[index].gameObject.SetActive(false);
            playersGameObject[index] = GameObject.Find("PlayerObject" + (index + 1).ToString());
            if (playersGameObject[index] != null)
            {
                EEGport playerComponent = playersGameObject[index].GetComponent<EEGport>();
                if (playerComponent != null)
                {
                    players[index] = playerComponent;
                }
            }
        }
    }

    void Start(){
        
        // p1Stats.text = "";
        // p1Connect.onClick.AddListener(() => ConnectClicked()); 
        // p1Disconnect.onClick.AddListener(()=> DisconnectClicked());

        p2Stats.text = "";
        // p2Connect.onClick.AddListener(() => ConnectClicked()); 
        // p2Disconnect.onClick.AddListener(()=> DisconnectClicked());
        
        // p3Stats.text = "";
        // p3Connect.onClick.AddListener(() => ConnectClicked()); 
        // p3Disconnect.onClick.AddListener(()=> DisconnectClicked());
        
        // p4Stats.text = "";
        // p4Connect.onClick.AddListener(() => ConnectClicked()); 
        // p4Disconnect.onClick.AddListener(()=> DisconnectClicked());

        // deleteCon.onClick.AddListener(()=> deleteAllConnections());
        
        for (int index = 0; index < playersGameObject.Length; index++)
        {
            //stats[index].text = "";
            
            connectButtons[index].onClick.AddListener(() => ConnectClicked());
            disconnectButtons[index].onClick.AddListener(() => DisconnectClicked());
        }
        //deleteCon.onClick.AddListener(deleteAllConnections);
    }

    void ConnectClicked()
    {
        // if(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == p1Connect.gameObject){
        //     p1.connect();
        //     if(p1.errCodeConnect == 0){
        //         p1Connect.gameObject.SetActive(false);
        //         p1Disconnect.gameObject.SetActive(true);
        //     }
        //     else{
        //         //error meddelande. T.ex fel comport etc
        //     }
        // }
        // if(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == p2Connect.gameObject){
        //     p2.connect();
        //     if(p2.errCodeConnect == 0){
        //         p2Connect.gameObject.SetActive(false);
        //         p2Disconnect.gameObject.SetActive(true);
        //     }
        //     else{
        //         //error meddelande. T.ex fel comport etc
        //     }
        // }
        // if(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == p3Connect.gameObject){
        //     p3.connect();
        //     if(p3.errCodeConnect == 0){
        //         p3Connect.gameObject.SetActive(false);
        //         p3Disconnect.gameObject.SetActive(true);
        //     }
        //     else{
        //         //error meddelande. T.ex fel comport etc
        //     }
        // }
        // if(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == p4Connect.gameObject){
        //     p4.connect();
        //     if(p4.errCodeConnect == 0){
        //         p4Connect.gameObject.SetActive(false);
        //         p4Disconnect.gameObject.SetActive(true);
        //     }
        //     else{
        //         //error meddelande. T.ex fel comport etc
        //     }
        // }
        GameObject clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        
        for(int index = 0; index < connectButtons.Length; index++){

            if(clickedButton == connectButtons[index].gameObject){
            
                players[index].connect();
                if(players[index].errCodeConnect == 0)
                {
                    connectButtons[index].gameObject.SetActive(false);
                    disconnectButtons[index].gameObject.SetActive(true);
                }
            }
        }
    }

    void DisconnectClicked()
    {
        // if(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == p1Disconnect.gameObject){
        //     p1.disconnect();
        //     if(p1.errCodeConnect != 0){
        //         p1Disconnect.gameObject.SetActive(false);
        //         p1Connect.gameObject.SetActive(true);
        //         p1Stats.text = "";
        //     }   
        //     else{
        //         //error meddelande
        //     }
        // }
        // if(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == p2Disconnect.gameObject){
        //     p2.disconnect();
        //     if(p2.errCodeConnect != 0){
        //         p2Disconnect.gameObject.SetActive(false);
        //         p2Connect.gameObject.SetActive(true);
        //         p1Stats.text = "";
        //     }   
        //     else{
        //         //error meddelande
        //     }
        // }
        // if(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == p3Disconnect.gameObject){
        //     p3.disconnect();
        //     if(p3.errCodeConnect != 0){
        //         p3Disconnect.gameObject.SetActive(false);
        //         p3Connect.gameObject.SetActive(true);
        //         p3Stats.text = "";
        //     }   
        //     else{
        //         //error meddelande
        //     }
        // }
        // if(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == p4Disconnect.gameObject){
        //     p4.disconnect();
        //     if(p4.errCodeConnect != 0){
        //         p4Disconnect.gameObject.SetActive(false);
        //         p4Connect.gameObject.SetActive(true);
        //         p4Stats.text = "";
        //     }   
        //     else{
        //         //error meddelande
        //     }
        // }
        GameObject clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for(int index = 0; index < disconnectButtons.Length; index++){
            if(clickedButton == disconnectButtons[index].gameObject){
                players[index].disconnect();
                if (players[index].errCodeConnect == 2)
                {
                    disconnectButtons[index].gameObject.SetActive(false);
                    connectButtons[index].gameObject.SetActive(true);
                    //stats[index].text = "";
                }
            }
        }
    }

    void displayStats()
    {
        // if(!(p1Connect.gameObject.activeSelf)){
        //     p1Stats.text = p1.getAttention().ToString();
        // }
        // if(!(p2Connect.gameObject.activeSelf)){
        //     p2Stats.text = p2.getAttention().ToString();
        // }
        // if(!(p3Connect.gameObject.activeSelf)){
        //     p3Stats.text = p3.getAttention().ToString();
        // }
        // if(!(p4Connect.gameObject.activeSelf)){
        //     p4Stats.text = p4.getAttention().ToString();
        // }

        // for (int index = 0; index < stats.Length; index++)
        // {
        //     if (!connectButtons[index].gameObject.activeSelf)
        //     {
        //         stats[index].text = players[index].getAttention().ToString();
        //     }
        // }

            p2Stats.text = players[1].getAttention().ToString();
    
    }

    void deleteAllConnections()
    {
        // if(p1.errCodeConnect == 0 || p1.errCodeConnect == 2){
        //     p1Connect.gameObject.SetActive(true);
        //     p1Disconnect.gameObject.SetActive(false);
        //     p1.deleteConnection();  
        // }
        // if(p2.errCodeConnect == 0 || p1.errCodeConnect == 2){
        //     p2Connect.gameObject.SetActive(true);
        //     p2Disconnect.gameObject.SetActive(false);
        //     p2.deleteConnection();
        // }
        // if(p3.errCodeConnect == 0 || p1.errCodeConnect == 2){
        //     p3Connect.gameObject.SetActive(true);
        //     p3Disconnect.gameObject.SetActive(false);
        //     p3.deleteConnection();
        // }
        // if(p4.errCodeConnect == 0 || p1.errCodeConnect == 2){
        //     p4Connect.gameObject.SetActive(true);
        //     p4Disconnect.gameObject.SetActive(false);
        //     p4.deleteConnection();
        // }

        for (int index = 0; index < disconnectButtons.Length; index++)
        {
            if (players[index].errCodeConnect == 0 || players[index].errCodeConnect == 2)
            {
                connectButtons[index].gameObject.SetActive(true);
                disconnectButtons[index].gameObject.SetActive(false);
                players[index].deleteConnection();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {   

         if(!connectButtons[1].gameObject.activeSelf){
            displayStats();
         }
        // if(players[1].errCodeAutoRead == 0){
        // Debug.Log(players[1].getAttention());
        //}
        //displayStats();
    }
}
