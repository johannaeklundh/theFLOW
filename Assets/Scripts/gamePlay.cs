using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
//using System.Runtime.Remoting.Channels;
using UnityEngine;

public class gamePlay : MonoBehaviour
{
    // Can change from Unity
    public float alphaChange = 50;
    public float thetaChange = 40;

    // Start is called before the first frame update
    void Start()
    {
        updatePlayerPosition(this);
        setPlacements(this);

    }

    // Update is called once per frame
    void Update()
    {
   
        // Put functions inside this to delay the update to every 3 sec
        if(canUpdate){

            updatePlayerPosition(this);
            setPlacements(this);
            setConsistency(this);
            setUnbothered(this);

            //float alphaTest = 30;
            //float thetaTest = 40;

            setMean(this, alphaChange,thetaChange);
            setBalance(this, alphaChange, thetaChange);
            setPower(this, alphaChange, thetaChange);  
                 

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
        public int position;
        public int placement;
        public float power;
        public float meanAlpha;
        public float meanTheta;
        public float consistency;
        public float unbothered;
        public float balance;
        public float largestDistance;
        public float smallestDistance;

        // Constructor
        public PlayerData(int playerID, int playerPosition = 0, int playerPlacement = 0, float playerPower = 50, float playerAlphaMean = 0.0f,
        float playerThetaMean = 0.0f, float playerConsistency = 0.0f, float playerUnbothered = 0.0f , float playerBalance = 0.0f)
        {
            id = playerID;
            position = playerPosition;
            placement = playerPlacement;
            power = playerPower;
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
            UnityEngine.Debug.Log("Player Power: " + power);
            UnityEngine.Debug.Log("Player Mean of Alpha: " + meanAlpha);
            UnityEngine.Debug.Log("Player Mean of Theta: " + meanTheta);
            UnityEngine.Debug.Log("Player Consistency: " + consistency);
            UnityEngine.Debug.Log("Player Unbothered: " + unbothered);
            UnityEngine.Debug.Log("Player Balance: " + balance);
        }
    }

    /************************************************************************************/

    
    
    
    
    
    /***********************************Variables****************************************/
    
    
    
    // Create the 4 players (may change to function due to the number of players beign flexible)
    PlayerData player1 = new PlayerData(1);
    PlayerData player2 = new PlayerData(2);
    PlayerData player3 = new PlayerData(3);
    PlayerData player4 = new PlayerData(4);


    // Current position of each player in x and y(placeholder)
    int pos1x = 20;
    int pos1y = 20;

    int pos2x = 30;
    int pos2y = 30;

    int pos3x = 40;
    int pos3y = 40;

    int pos4x = 50;
    int pos4y = 50;


    /************************************************************************************/




    



    /**********************************Functions*****************************************/
    
    
    // Algorithm for calculating consistency
    public static float calculateConsistency(gamePlay instance){
        float con = 5;

        return con;
    }

    // Set consistency of each player, utilizes calculateConsistency
    public static void setConsistency(gamePlay instance){
        //instance.player1.consistency = 5;
    }

    // Algorithm for calculating unbothered
    public static float calculateUnbothered(gamePlay instance){
        float unboth = 5;

        return unboth;
    }

    // Set unbothered of each player, utilizes calculateUnbothered
    public static void setUnbothered(gamePlay instance){
        //instance.player1.unbothered = 5;
    }
    
    
    // Takes in the position of a player in x- and y-direction and makes it a distance from center, assumes center of screen is (0,0)
    public static int calculatePosition(int x, int y){
        float position = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));    // Pythtagoras theorem to get distance from center

        return (int)position;
    }


    // Updates the players current position (placeholder)
    public static void updatePlayerPosition(gamePlay instance){
        instance.player1.position = calculatePosition(instance.pos1x, instance.pos1y);
        instance.player2.position = calculatePosition(instance.pos2x, instance.pos2y);
        instance.player3.position = calculatePosition(instance.pos3x, instance.pos3y);
        instance.player4.position = calculatePosition(instance.pos4x, instance.pos4y);
    }



    // Placeholder to decide the placement of the players, updates every 3 sec
    public static void setPlacements(gamePlay instance){
        
        // Create an array containing each player (put this in a sepperate create-player funtion for when you can add less than 4 players)
        PlayerData[] players = {(PlayerData)instance.player1, (PlayerData)instance.player2, (PlayerData)instance.player3, (PlayerData)instance.player4};
        
        // Create vector storing the current position of each player
        Vector4 positions = new Vector4(instance.player1.position, instance.player2.position, instance.player3.position, instance.player4.position);

        // Convert to arry for easier sorting
        int[] sortedPositions = {(int)positions.x, (int)positions.y, (int)positions.z, (int)positions.w};

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


    // Returns the power in which the vortex will spin in, positive means players are winning, negative means Ai is winning
    public static int whoseWinning(gamePlay instance, int teamPower, int aiPower){
        
        int rotate = 50;

        return rotate;
    }


    // Return the new calculated the mean of input BrainWave with the latest input
    static float calculateMean(float mean, float brainWave)
    {
        mean = (mean + brainWave) / 2;
        return mean;
    }

   // Set mean of both alpha and theta of each player
    static void setMean(gamePlay instance, float alpha, float theta)
    {

        instance.player1.meanAlpha = calculateMean(instance.player1.meanAlpha, alpha);
        instance.player1.meanTheta = calculateMean(instance.player1.meanTheta, theta);
                                                     
        instance.player2.meanAlpha = calculateMean(instance.player1.meanAlpha, alpha);
        instance.player2.meanTheta = calculateMean(instance.player1.meanTheta, theta);
                                                     
        instance.player3.meanAlpha = calculateMean(instance.player1.meanAlpha, alpha);
        instance.player3.meanTheta = calculateMean(instance.player1.meanTheta, theta);
                                                    
        instance.player4.meanAlpha = calculateMean(instance.player1.meanAlpha, alpha);
        instance.player4.meanTheta = calculateMean(instance.player1.meanTheta, theta);
    }
   
    // Returns the new calculated power (a mean of alpha and theta)
    public static float calculatePower(float alpha, float theta)
    {
        float power = (alpha + theta) / 2;
        return power;
    }
    // Set power to each instance of players
    static void setPower(gamePlay instance, float alpha, float theta)
    {
        // float at the end (50) should be replaced with brainWave input
        instance.player1.power = calculatePower(alpha, theta);
        instance.player2.power = calculatePower(alpha, theta);
        instance.player3.power = calculatePower(alpha, theta);
        instance.player4.power = calculatePower(alpha, theta);
    }

    // Returns the new calculated balnce (how close alpha and theta is)
    static float calculateBalance(ref PlayerData playerTemp, float alpha, float theta)
    {
        float diff = Math.Abs(alpha - theta);
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

    public static void setBalance(gamePlay instance, float alpha, float theta)
    {
        instance.player1.balance = calculateBalance(ref instance.player1, alpha, theta);
        instance.player2.balance = calculateBalance(ref instance.player2, alpha, theta);
        instance.player3.balance = calculateBalance(ref instance.player3, alpha, theta);
        instance.player4.balance = calculateBalance(ref instance.player4, alpha, theta);

    }
     
    /************************************************************************************/
}
