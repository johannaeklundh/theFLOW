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

    public PlayerData[] players;    // Public array of all players

    public int p1Alpha = 35;
    public int p1Theta = 35;
    public int p2Alpha = 50;
    public int p2Theta = 50;
    public int p3Alpha = 55;
    public int p3Theta = 55;
    public int p4Alpha = 45;
    public int p4Theta = 45;

    /***********************************************************/

    // Start is called before the first frame update
    void Start()
    {

        createPlayers(4);   // Number of players given by EEGport

        StartCoroutine(delayUpdate()); // Delay Start() by 3 seconds

        updatePrevAndCurrent(this); // Update brainwves

    }

    // Update is called once per frame
    void Update()
    {
   
        // Put functions inside this to delay the update to every 3 sec
        if(canUpdate){

            updatePrevAndCurrent(this);
            updatePlayerRadius(this);
            setPlacements(this);

            setConsistency(this);
            setMean(this);
            setBalance(this);
            // setUnbothered is called upon in AI 

            players[0].displayPlayerInfo();    // Displays all info stored in the PlayerData struct
            // players[1].displayPlayerInfo();
            // players[2].displayPlayerInfo();
            // players[3].displayPlayerInfo();
                 
            canUpdate = false;  // Makes it so that each function doesn't update every frame
            
            // Start the coroutine (allows to delay update or execute over several frames) to enable updates after 3 seconds
            StartCoroutine(delayUpdate());

        }
    }





    /************Things used obly to control delays, not relevant for behaviour************/
    private bool canUpdate = false; // Decides weather a function can update in update()
    
    IEnumerator delayUpdate(){

        // Wait for 3 seconds
        yield return new WaitForSeconds(1.0f);

        // Allow updates to happen
        canUpdate = true;

    }
    /**************************************************************************************/






    /*************************************Structs****************************************/
    public struct PlayerData{
        
	// Values for gameplay
	public int id;
        public float radius;
        public int placement;
        public float power;
        public float prevPower;
        public int alpha;
        public int prevAlpha;
        public int theta;
        public int prevTheta;
	
	    // Functionality
	    public bool update;
        
        // Values for after game
        public float meanAlpha;
        public float meanTheta;
        public float consistency;
        public float unbothered;
        public float balance;
        public float largestDistance;
        public float smallestDistance;

        // Constructor
        public PlayerData(int playerID, float playerradius = 2.0f, int playerPlacement = 1, float playerPower = 50.0f, float playerAlphaMean = 0.0f,
        float playerThetaMean = 0.0f, float playerConsistency = 0.0f, float playerUnbothered = 0.0f, float playerBalance = 0.0f)
        {
            id = playerID;
            radius = playerradius;
            placement = playerPlacement;
            power = playerPower;
            prevPower = 0;
            alpha = 50;
            prevAlpha = 0;
            theta = 50;
            prevTheta = 0;

            update = true;

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
            UnityEngine.Debug.Log("Player radius: " + radius);
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

    
    
    /**********************************Useful Functions**********************************/

    // Function to create n (provided by EEGport) players and add them to the players array
    void createPlayers(int n) { 
        // Instantiate the players array with size n
        players = new PlayerData[n];

        // Loop to create n players and assign them to the array
        for (int i = 0; i < n; i++) {

            int id = i + 1; // Player ID starts from 1
            players[i] = new PlayerData(id);
        }
    }
    
    // Function that assign values to a specified field of PlayerData for each player in players
    void assignValuesToField(float[] values, string fieldName) {    // Must be called with an instance if within a function

        if (values.Length != players.Length) {  // Test if possible
            UnityEngine.Debug.LogError("Number of values should match the number of players.");
            return;
        }
        
        for (int i = 0; i < players.Length; i++) {  // Go through all players

            if(players[i].update){  // Test if player should update

                // Use reflection to set the value of the specified field
                var field = typeof(PlayerData).GetField(fieldName);

                if (field != null && field.FieldType == typeof(float)) {    // Test if field is possible and if it truly is a float
                    field.SetValueDirect(__makeref(players[i]), values[i]); // Set value 
                }
            }
        }
    }

    // Overloadeed version for int[]
    void assignValuesToField(int[] values, string fieldName) {    // Must be called with an instance if within a function

        if (values.Length != players.Length) {  // Test if possible
            UnityEngine.Debug.LogError("Number of values should match the number of players.");
            return;
        }
        
        for (int i = 0; i < players.Length; i++) {  // Go through all players

            if(players[i].update){  // Test if player should update

                // Use reflection to set the value of the specified field
                var field = typeof(PlayerData).GetField(fieldName);

                if (field != null && field.FieldType == typeof(int)) {  // Test if field is possible and if it truly is a float
                    field.SetValueDirect(__makeref(players[i]), values[i]); // Set value
                }
            }
        }
    }
    
    // Set the players alpha, theta and power and all the previous ones
    public static void updatePrevAndCurrent(gamePlay instance){
        
        // Assign prevAlpha
        int[] prevAlphaValues = {instance.players[0].alpha, instance.players[1].alpha, instance.players[2].alpha, instance.players[3].alpha};
        instance.assignValuesToField(prevAlphaValues, "prevAlpha");

        // Assign alpha
        /*int[] alphaValues = {(int)Mathf.Abs(50*Mathf.Sin(Time.time)), (int)Mathf.Abs(44*Mathf.Sin(Time.time)),
        (int)Mathf.Abs(28*Mathf.Sin(Time.time)), (int)Mathf.Abs(52*Mathf.Sin(Time.time))};*/
        int[] alphaValues = {instance.p1Alpha, instance.p2Alpha, instance.p3Alpha, instance.p4Alpha};
        instance.assignValuesToField(alphaValues, "alpha");
        
        // Assign prevTheta
        int[] prevThetaValues = {instance.players[0].theta, instance.players[1].theta, instance.players[2].theta, instance.players[3].theta};
        instance.assignValuesToField(prevThetaValues, "prevTheta");

        // Assign theta
        /*int[] thetaValues = {(int)Mathf.Abs(66*Mathf.Cos(Time.time)), (int)Mathf.Abs(28*Mathf.Cos(Time.time)),
        (int)Mathf.Abs(74*Mathf.Cos(Time.time)), (int)Mathf.Abs(12*Mathf.Cos(Time.time))};*/
        int[] thetaValues = {instance.p1Theta, instance.p2Theta, instance.p3Theta, instance.p4Theta};
        instance.assignValuesToField(thetaValues, "theta");

        // Assign prevPower
        float[] prevPowerValues = {instance.players[0].power, instance.players[1].power, instance.players[2].power, instance.players[3].power};
        instance.assignValuesToField(prevPowerValues, "prevPower");

        // Assign power
        float[] powerValues = {calculatePower(instance.players[0].alpha, instance.players[0].theta), calculatePower(instance.players[1].alpha, instance.players[1].theta),
         calculatePower(instance.players[2].alpha, instance.players[2].theta),calculatePower(instance.players[3].alpha, instance.players[3].theta)};
        instance.assignValuesToField(powerValues, "power");
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
    public static void setPlacements(gamePlay instance){    // FIX so always placements 1-3

         // Create a copy of the players array
        PlayerData[] sortedCopy = new PlayerData[instance.players.Length];
        Array.Copy(instance.players, sortedCopy, instance.players.Length);

        // Sort the copy based on their radius
        Array.Sort(sortedCopy, (a, b) => a.radius.CompareTo(b.radius)); // Sorting in ascending order
        
        int placement = 1;

        for (int i = 0; i < sortedCopy.Length; i++) {  // Loop through all players
            
            // Find the index of the player in the unsorted players array
            int playerIndex = Array.IndexOf(instance.players, sortedCopy[i]);
            
            // Assign placement to the player in the players array
            instance.players[playerIndex].placement = placement;

            placement++;
        }

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

        float[] consistencyValues = new float[instance.players.Length];

        for (int i = 0; i < instance.players.Length; i++) {
            
            float con = derivatePower(instance.players[i]);

            if(con > instance.players[i].consistency){
                consistencyValues[i] = con;
            }
            else{
                consistencyValues[i] = instance.players[i].consistency;
            }
        }

        instance.assignValuesToField(consistencyValues, "consistency"); //  Assign consistencyValues tto players
    }

    // Set unbothered of each player, is utilized in AI-class
    public static void setUnbothered(gamePlay instance, int id){

        float unbothered = derivatePower(instance.players[(id-1)]);

        if(unbothered < instance.players[(id-1)].unbothered){ // Update to the largest decending value
            instance.players[(id-1)].unbothered = unbothered;
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

        float[] meanAlphaValues = new float[instance.players.Length];
        float[] meanThetaValues = new float[instance.players.Length];

        for (int i = 0; i < instance.players.Length; i++) {
            
            float al = calculateMean(instance.players[i].meanAlpha, instance.players[i].alpha);
            float th = calculateMean(instance.players[i].meanTheta, instance.players[i].theta);

            meanAlphaValues[i] = al;
            meanThetaValues[i] = th;
        }

        instance.assignValuesToField(meanAlphaValues, "meanAlpha"); //  Assign meanAlphaValues to players
        instance.assignValuesToField(meanThetaValues, "meanTheta"); //  Assign meanThetaValues to players
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
        float[] balanceValues = new float[instance.players.Length];
       
        for (int i = 0; i < instance.players.Length; i++) {
            
            float bal = calculateBalance(ref instance.players[i]);

            balanceValues[i] = bal;
        }

        instance.assignValuesToField(balanceValues, "balance"); //  Assign balanceValues to players
    }

    public static float calculateRadius(gamePlay instance, int place){  // FIX so that other players get boost when winning

        if(instance.players[place].update == false){
             return 0.0f;
        }
        
        float addOn = 0.0f;
        
        // Normal increase (closer to center)
        if(instance.players[(place)].power > instance.AI.power + 15){
            addOn = -0.08f;
        }
        else if(instance.players[(place)].power > instance.AI.power + 10){
            addOn = -0.05f;
        }
        else if(instance.players[(place)].power > instance.AI.power + 7){
            addOn = -0.02f;
        }
        else if(instance.players[(place)].power >= instance.AI.power){
        addOn = -0.01f;
        }

        // Normal decrease (further from center)
        if(instance.players[(place)].radius < 2.0f){   // Must be less than 2

            if(instance.players[(place)].power < instance.AI.power - 15){
                addOn = 0.07f;
            }
            else if(instance.players[(place)].power < instance.AI.power - 12){
                addOn = 0.04f;
            }
            else if(instance.players[(place)].power < instance.AI.power -9){
                addOn = 0.02f;
            }
            else if(instance.players[(place)].power <= instance.AI.power){
            addOn = 0.005f;
            }
        }

        /*if(instance.players[(place)].id == 4){  // Write out increase/decrease for player 4
            UnityEngine.Debug.Log("addOn = " + addOn);
        }*/

        float radius = instance.players[(place)].radius + addOn;    // Calculate the new radius

        // Keep radiuss inbetween possible values (aka 0 and 2)
        if(radius > 2.0f){
            radius = 2.0f;
        }
        else if(radius < 0.0f){
            radius = 0.0f;
            instance.players[place].update = false;
            UnityEngine.Debug.Log("Player " + instance.players[(place)].id + " has finished!");
        }

        return radius;
    }
    
    // Updates the players current radius (placeholder)
    public static void updatePlayerRadius(gamePlay instance){

        float[] radiusValues = new float[instance.players.Length];
       
        for (int i = 0; i < instance.players.Length; i++) {
            
            float rad = calculateRadius(instance, i);

            radiusValues[i] = rad;
        }

        instance.assignValuesToField(radiusValues, "radius"); //  Assign radiusValues to players
    }


    // Returns the power in which the vortex will spin in, positive means players are winning, negative means Ai is winning
    public static int whoseWinning(gamePlay instance, int teamPower, int aiPower){
        
        int rotate = 50;

        return rotate;
    }
     
    /************************************************************************************/
}
