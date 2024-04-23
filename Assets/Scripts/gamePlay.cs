using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
//using System.Runtime.Remoting.Channels;
using UnityEngine;

public class gamePlay : MonoBehaviour
{
    /********Refrence instances to other classes/scripts********/
    public AIScript AI;


    /***********************************************************/

    // Start is called before the first frame update
    void Start()
    {
        updatePrevAndCurrent(this); // Update brainwves
        updatePlayerPosition(this); // Update positions
        setPlacements(this);        // Set placements

    }

    // Update is called once per frame
    void Update()
    {
   
        // Put functions inside this to delay the update to every 3 sec
        if(canUpdate){

            updatePrevAndCurrent(this);
            updatePlayerPosition(this);
            setPlacements(this);


            setConsistency(this);
            setMean(this);
            setBalance(this);
            // setUnbothered is called upon in AI 

            // player1.displayPlayerInfo();    // Displays all info stored in the PlayerData struct
            player2.displayPlayerInfo();
            // player3.displayPlayerInfo();
            // player4.displayPlayerInfo();
                 

            canUpdate = false;  // Makes it so that each function doesn't update every frame

            // Start the coroutine (allows to delay update or execute over several frames) to enable updates after 3 seconds
            StartCoroutine(delayUpdate());

        }
    }





    /************Things used obly to control update(), not relevant for behaviour************/
    private bool canUpdate = true; // Decides weather a function can update in update()

    IEnumerator delayUpdate(){

        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Allow updates to happen
        canUpdate = true;
    }
    /**************************************************************************************/






    /*************************************Structs****************************************/
    public struct PlayerData{
        public int id;
        public float position;
        public int placement;
        public float power;
        public float prevPower;   // Mathf.Abs(previous - current)
        public int alpha;
        public int prevAlpha;
        public int theta;
        public int prevTheta;
        
        
        public float meanAlpha;
        public float meanTheta;
        public float consistency;
        public float unbothered;
        public float balance;
        public float largestDistance;
        public float smallestDistance;

        // Constructor
        public PlayerData(int playerID, float playerPosition = 0.0f, int playerPlacement = 0, float playerPower = 50.0f, float playerAlphaMean = 0.0f,
        float playerThetaMean = 0.0f, float playerConsistency = 0.0f, float playerUnbothered = 0.0f, float playerBalance = 0.0f)
        {
            id = playerID;
            position = playerPosition;
            placement = playerPlacement;
            power = playerPower;
            prevPower = 0;
            alpha = 50;
            prevAlpha = 0;
            theta = 50;
            prevTheta = 0;
            meanAlpha = playerAlphaMean;
            meanTheta = playerThetaMean;
            consistency = playerConsistency;
            unbothered = playerUnbothered;
            balance = playerBalance;
            largestDistance = 0;
            smallestDistance = 100;
    }

        // A way to display each player info in the console
        public void displayPlayerInfo()
        {
            UnityEngine.Debug.Log("Player ID: " + id);
            UnityEngine.Debug.Log("Player Position: " + position);
            UnityEngine.Debug.Log("Player Placement: " + placement);
            // UnityEngine.Debug.Log("Player Prev Power: " + prevPower);
            UnityEngine.Debug.Log("Player Power: " + power);
            // UnityEngine.Debug.Log("Player Alpha: " + alpha);
            // UnityEngine.Debug.Log("Player Theta: " + theta);
            // UnityEngine.Debug.Log("Player Mean of Alpha: " + meanAlpha);
            // UnityEngine.Debug.Log("Player Mean of Theta: " + meanTheta);
            // UnityEngine.Debug.Log("Player Consistency: " + consistency);
            // UnityEngine.Debug.Log("Player Unbothered: " + unbothered);
            // UnityEngine.Debug.Log("Player Balance: " + balance);
        }
    }

    /************************************************************************************/

    
    
    
    
    
    /***********************************Variables****************************************/
    
    
    // Create the 4 players (may change to function due to the number of players beign flexible)
    public PlayerData player1 = new PlayerData(1);
    public PlayerData player2 = new PlayerData(2);
    public PlayerData player3 = new PlayerData(3);
    public PlayerData player4 = new PlayerData(4);

    /************************************************************************************/

    
    
    /**********************************Useful Functions**********************************/
    
    // Set the players alpha, theta and power and all the previous ones
    public static void updatePrevAndCurrent(gamePlay instance){
        // Player 1
        instance.player1.prevAlpha = instance.player1.alpha;    // Set prev to the current
        instance.player1.alpha = (int)Mathf.Abs(50*Mathf.Sin(Time.time));       // Update current, for now random number
        instance.player1.prevTheta = instance.player1.theta;    
        instance.player1.theta = (int)Mathf.Abs(30*Mathf.Cos(Time.time));   
        instance.player1.prevPower = instance.player1.power;    
        instance.player1.power = calculatePower(instance.player1.alpha, instance.player1.theta);  

        // Player 2
        instance.player2.prevAlpha = instance.player2.alpha;    // Set prev to the current
        instance.player2.alpha = (int)Mathf.Abs(44*Mathf.Sin(Time.time));       // Update current, for now random number
        instance.player2.prevTheta = instance.player2.theta;    
        instance.player2.theta = (int)Mathf.Abs(66*Mathf.Cos(Time.time));   
        instance.player2.prevPower = instance.player2.power;    
        instance.player2.power = calculatePower(instance.player2.alpha, instance.player2.theta);  

        // Player 3
        instance.player3.prevAlpha = instance.player3.alpha;    // Set prev to the current
        instance.player3.alpha = (int)Mathf.Abs(28*Mathf.Sin(Time.time));       // Update current, for now random number
        instance.player3.prevTheta = instance.player3.theta;    
        instance.player3.theta = (int)Mathf.Abs(49*Mathf.Cos(Time.time));   
        instance.player3.prevPower = instance.player3.power;    
        instance.player3.power = calculatePower(instance.player3.alpha, instance.player3.theta);

        // Player 4
        instance.player4.prevAlpha = instance.player4.alpha;    // Set prev to the current
        instance.player4.alpha = (int)Mathf.Abs(52*Mathf.Sin(Time.time));       // Update current, for now random number
        instance.player4.prevTheta = instance.player4.theta;    
        instance.player4.theta = (int)Mathf.Abs(31*Mathf.Cos(Time.time));   
        instance.player4.prevPower = instance.player4.power;    
        instance.player4.power = calculatePower(instance.player4.alpha, instance.player4.theta);  
    }
    
    // Calculates change
    public static float change(float current, float previous){
        float change = current - previous;
        return change;
    }

    // Calculates derivate
    public static float derivatePower(PlayerData player){
        float derivate = change(player.power, player.prevPower) / Time.deltaTime;
        return derivate;
    }

    // Placeholder to decide the placement of the players, updates every 3 sec
    public static void setPlacements(gamePlay instance){
        
        // Create an array containing each player (put this in a sepperate create-player funtion for when you can add less than 4 players)
        PlayerData[] players = {(PlayerData)instance.player1, (PlayerData)instance.player2, (PlayerData)instance.player3, (PlayerData)instance.player4};
        
        // Create vector storing the current position of each player
        Vector4 positions = new Vector4(instance.player1.position, instance.player2.position, instance.player3.position, instance.player4.position);

        // Convert to arry for easier sorting
        float[] sortedPositions = {(float)positions.x, (float)positions.y, (float)positions.z, (float)positions.w};

        // Sort new array
        System.Array.Sort(sortedPositions, (x,y) => y.CompareTo(x));

        // Update placements
        for(int i = 0; i < players.Length; i++)
        {
            for(int n = 0; n < sortedPositions.Length; n++)
            {
                if(players[i].position == sortedPositions[n]){
                    players[i].placement = n + 1;
                }
            }
        }

        instance.player1.placement = players[0].placement;
        instance.player2.placement = players[1].placement;
        instance.player3.placement = players[2].placement;
        instance.player4.placement = players[3].placement;

    }

    // Find a what player whose id is searched for
    public static PlayerData idPlayer(gamePlay instance, int ID){

        // Test weather ID is possible
        if(ID > 4){
             throw new System.Exception("Invalid placement entered, try 1-4");
        }

        // Create an array containing each player (put this in a sepperate create-player funtion for when you can add less than 4 players)
        PlayerData[] players = {(PlayerData)instance.player1, (PlayerData)instance.player2, (PlayerData)instance.player3, (PlayerData)instance.player4};

        // Search for player whose placement matches the searched place and return the player
        for(int i = 0; i < players.Length; i++)
        {
            if(players[i].id == ID){
                return players[i];
            }
        }

        //return players[0];
        throw new System.Exception("Could not find player whose id is " + ID);
    }



    // Returns the new calculated power (a mean of alpha and theta)
    public static float calculatePower(float alpha, float theta)
    {
        float power = (alpha + theta) / 2;
        return power;
    }


    /**********************************Main Functions*************************************/

    // Set consistency of each player
    public static void setConsistency(gamePlay instance){
        // Player 1
        float consistency = derivatePower(instance.player1); // Calculate consistency for player
        if(instance.player1.consistency > consistency){ // Only keep the largest decending derivative (want player to get better)
            instance.player1.consistency = consistency; // Update
        }

        // Player 2
        consistency = derivatePower(instance.player2); // Calculate consistency for player
        if(instance.player2.consistency > consistency){ // Only keep the largest decending derivative
            instance.player2.consistency = consistency; // Update
        }
        
        // Player 3
        consistency = derivatePower(instance.player3); // Calculate consistency for player
        if(instance.player3.consistency > consistency){ // Only keep the largest decending derivative
            instance.player3.consistency = consistency; // Update
        }
        
        // Player 4
        consistency = derivatePower(instance.player4); // Calculate consistency for player
        if(instance.player4.consistency > consistency){ // Only keep the largest decending derivative
            instance.player4.consistency = consistency; // Update
        }
    }

    // Set unbothered of each player, is utilized in AI-class
    public static void setUnbothered(gamePlay instance, int id){

        PlayerData p = idPlayer(instance, id);
        float unbothered = derivatePower(p);

        if(unbothered < p.unbothered){ // Update to the largest decending value
            if(id == 1){
                instance.player1.unbothered = unbothered;
            }
            else if(id == 2){
                instance.player2.unbothered = unbothered;
            }
            else if(id == 3){
                instance.player3.unbothered = unbothered;
            }
            else{
                instance.player4.unbothered = unbothered;
            }
        }

        //player.displayPlayerInfo();
    }


    // Return the new calculated the mean of input BrainWave with the latest input
    static float calculateMean(float mean, float brainWave)
    {
        mean = (mean + brainWave) / 2;
        return mean;
    }

   // Set mean of both alpha and theta of each player
    static void setMean(gamePlay instance)
    {

        instance.player1.meanAlpha = calculateMean(instance.player1.meanAlpha, instance.player1.alpha);
        instance.player1.meanTheta = calculateMean(instance.player1.meanTheta, instance.player1.theta);
                                                     
        instance.player2.meanAlpha = calculateMean(instance.player1.meanAlpha, instance.player2.alpha);
        instance.player2.meanTheta = calculateMean(instance.player1.meanTheta, instance.player2.theta);
                                                     
        instance.player3.meanAlpha = calculateMean(instance.player1.meanAlpha, instance.player2.alpha);
        instance.player3.meanTheta = calculateMean(instance.player1.meanTheta, instance.player3.theta);
                                                    
        instance.player4.meanAlpha = calculateMean(instance.player1.meanAlpha, instance.player2.alpha);
        instance.player4.meanTheta = calculateMean(instance.player1.meanTheta, instance.player4.theta);
    }

    // Returns the new calculated balnce (how close alpha and theta is)
    static float calculateBalance(ref PlayerData playerTemp)
    {
        float diff = Math.Abs(playerTemp.alpha - playerTemp.theta);
        if(diff > playerTemp.largestDistance)
        {
            //change playerTemp largest distance
            playerTemp.largestDistance = diff;

        } else if(diff < playerTemp.smallestDistance)
        {
            //change playerTemp smallest distance
            playerTemp.smallestDistance = diff;
        }   
        // Calculate new balance
        float balance = (playerTemp.largestDistance + playerTemp.smallestDistance) / 2;
        return balance;
    }

    // Set balance of all players
    public static void setBalance(gamePlay instance)
    {
        instance.player1.balance = calculateBalance(ref instance.player1);
        instance.player2.balance = calculateBalance(ref instance.player2);
        instance.player3.balance = calculateBalance(ref instance.player3);
        instance.player4.balance = calculateBalance(ref instance.player4);
    }

    
    
    
    // returns a radious for each player, goes from 0 to 2
    public static float calculatePosition(gamePlay instance, PlayerData player, float lightning = 0.0f){
        
        float position = player.position;

        // Normal increase/decrease dephending on power
        if(player.position < 2.0f){    // Must be less than 2
            if(player.power > instance.AI.power + 10){
                position = position + 0.1f;
            }
            else if(player.power > instance.AI.power + 5){
                position = position + 0.05f;
            }
            else if(player.power >= instance.AI.power){
            position = position + 0.01f;
            }
        }
        else if(player.position > 0.0f){   // Must be more than 0
            if(player.power < instance.AI.power - 5){
                position = position - 0.05f;
            }
            else if(player.power < instance.AI.power -10){
                position = position -0.1f;
            }
        }

        // Lightning from AI 
        position = position - lightning;
        

        // Keep positions inbetween possible values
        if(player.position > 2.0f){
            position = 2.0f;
        }
        if(player.position < 0.0f){
            position = 0.0f;
        }

        return position;
    }
    
    // Updates the players current position (placeholder)
    public static void updatePlayerPosition(gamePlay instance){
        instance.player1.position = calculatePosition(instance, instance.player1);
        instance.player2.position = calculatePosition(instance, instance.player2);
        instance.player3.position = calculatePosition(instance, instance.player3);
        instance.player4.position = calculatePosition(instance, instance.player4);
    }


    // Returns the power in which the vortex will spin in, positive means players are winning, negative means Ai is winning
    public static int whoseWinning(gamePlay instance, int teamPower, int aiPower){
        
        int rotate = 50;

        return rotate;
    }
     
    /************************************************************************************/
}