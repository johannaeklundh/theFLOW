using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
//using System.Runtime.Remoting.Channels;
using UnityEngine;
using UnityEngine.SceneManagement;
using EEG;

public class gamePlay : MonoBehaviour
{
    /********Refrence instances to other classes/scripts********/
    public AIScript AI;
    public GameObject[] playersObject;
    public EEGport[] EEGplayers;

    /********Variables********/
    public PlayerData[] players;    // Public array of all players

    /********Delete********/
    public float p1Power = 50.0f;
    public float p2Power = 51.0f;
    public float p3Power = 52.0f;
    public float p4Power = 53.0f;

    public float adjust = 0.0f;

    // Declare the singleton instance
    public static gamePlay Instance { get; private set; }

    /***********************************************************/

    // Awake is called before Start
    void Awake()
    {

        playersObject = new GameObject[4];
        EEGplayers = new EEGport[4];
        
        for (int index = 0; index < playersObject.Length; index++)
        {
           
            playersObject[index] = GameObject.Find("PlayerObject" + (index + 1).ToString());
            if (playersObject[index] != null)
            {
                EEGport playerComponent = playersObject[index].GetComponent<EEGport>();
                if (playerComponent != null)
                {
                    EEGplayers[index] = playerComponent;
                }
            }
        }
        // Ensure only one instance of gamePlay exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        createPlayers(4);   // Number of players given by EEGport

        StartCoroutine(delayUpdate(3.0f)); // Delay Start() by 3 seconds
        StartCoroutine(delayUpdate1Sec(3.0f)); // Delay Start() by 3 seconds
        StartCoroutine(delayUpdate10Sec(13.0f));

        updatePrevAndCurrent(this); // Update brainwves

    }

    // Update is called once per frame
    void Update()
    {


        // Put functions inside this to delay the update to every delay sec
        if(canUpdate){

            //Dont destroy causes problems when reloading game.....
            if(SceneManager.GetActiveScene().buildIndex==0) 
            { Destroy(gameObject); }
            
            // Main GamePlay-Functions
            updatePrevAndCurrent(this);
            updatePlayerRadius(this);
            setPlacements(this);

            // Aftermatch-Functions
            setConsistency(this);
            setMean(this);
            setBalance(this);
            // setUnbothered is called upon in AI 

            // Write out in console..
            // players[0].displayPlayerInfo();    // Displays all info stored in the PlayerData struct
            // players[1].displayPlayerInfo();
            // players[2].displayPlayerInfo();
            players[3].displayPlayerInfo();


            canUpdate = false;  // Makes it so that each function doesn't update every frame
            
            // Start the coroutine (allows to delay update or execute over several frames) to enable updates after delay seconds
            StartCoroutine(delayUpdate(delay));

        }

        // Functions that can update every 1 sec
        if(canUpdate1sec){

            canUpdate1sec = false;

            set10Vectors(this); // Updates the vectors to decide whose winning, AI or players

            // Start the coroutine (allows to delay update or execute over several frames) to enable updates after delay1 seconds
            StartCoroutine(delayUpdate1Sec(delay1));
        }

        // Functions that can update every 10 sec
        if(canUpdate10sec){

            // UnityEngine.Debug.Log("Enter if-10: ");

            // Stop update
            canUpdate10sec = false;

            whoseWinning(this); // Returns how the vortex spins, -1, 0 or 1 dephending on who is winning

            // Start the coroutine (allows to delay update or execute over several frames) to enable updates after delay10 seconds
            StartCoroutine(delayUpdate10Sec(delay10));
        }
    }





    /************Things used obly to control delays, not relevant for overall behaviour************/
    
    // Variables
    private bool canUpdate = false; // Decides weather a function can update in update()

    private bool canUpdate1sec = false;

    private bool canUpdate10sec = false;

    private float delay = 0.25f;

    private float delay1 = 1.0f;

    private float delay10 = 10.0f;

    public bool isBoosted = false; // Tells if boost is active
    
    
    // Functions
    IEnumerator delayUpdate(float d){

        // Wait for d seconds
        yield return new WaitForSeconds(d);

        // Allow updates to happen
        canUpdate = true;

        delay = 0.25f;   // Reset delay

        // UnityEngine.Debug.Log(delay + " seconds has passed!");

    }

    IEnumerator delayUpdate1Sec(float d){

        // Wait for d seconds
        yield return new WaitForSeconds(d);

        canUpdate1sec = true;

        delay1 = 1.0f;   // Reset delay1

        // UnityEngine.Debug.Log("1 seconds has passed!");

    }
    
    IEnumerator delayUpdate10Sec(float d){

        // Wait for d seconds
        yield return new WaitForSeconds(d);

        canUpdate10sec = true;

        delay10 = 10.0f;   // Reset delay10

        UnityEngine.Debug.Log("10 seconds has passed!");

    }

    // Coroutine to end the boost after 5 seconds
    private IEnumerator EndBoost(gamePlay instance) {

        yield return new WaitForSeconds(5.0f);

        instance.AI.canUpdate = true;  // Allow updates in AI to happen again

        isBoosted = false;  // Reset the flag
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
        public float alpha;
        public float prevAlpha;
        public float theta;
        public float prevTheta;
	
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
        float playerThetaMean = 0.0f, float playerConsistency = 100.0f, float playerUnbothered = 100.0f, float playerBalance = 0.0f)
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
            UnityEngine.Debug.Log("Player Mean of Alpha: " + meanAlpha);
            UnityEngine.Debug.Log("Player Mean of Theta: " + meanTheta);
            UnityEngine.Debug.Log("Player Consistency: " + consistency);
            UnityEngine.Debug.Log("Player Unbothered: " + unbothered);
            UnityEngine.Debug.Log("Player Balance: " + balance);
        }
    }

    /************************************************************************************/






    /******************************Class-unique Variables********************************/

    // Saves 10 latest power-values for players
    public List<float> players10Power = new List<float>();

    // Saves 10 latest power-values for AI
    public List<float> ai10Power = new List<float>();



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
        
        UnityEngine.Debug.Log("prevCurrent called!");
        
        // Assign prevAlpha
        float[] prevAlphaValues = {instance.players[0].alpha, instance.players[1].alpha, instance.players[2].alpha, instance.players[3].alpha};
        instance.assignValuesToField(prevAlphaValues, "prevAlpha");

        // Assign alpha
        // Sin-ver
        /*int[] alphaValues = {(int)Mathf.Abs(50*Mathf.Sin(Time.time)), (int)Mathf.Abs(44*Mathf.Sin(Time.time)),
        (int)Mathf.Abs(28*Mathf.Sin(Time.time)), (int)Mathf.Abs(52*Mathf.Sin(Time.time))};*/

        // EEG-ver
        /*float[] alphaValues = {instance.EEGplayers[0].med, instance.EEGplayers[1].med, instance.EEGplayers[2].med, instance.EEGplayers[3].med};*/

        // Test-ver
        float[] alphaValues = {instance.p1Power, instance.p2Power, instance.p3Power, instance.p4Power};
        instance.assignValuesToField(alphaValues, "alpha");
        // UnityEngine.Debug.Log(instance.EEGplayers[1].med);
        
        // Assign prevTheta
        float[] prevThetaValues = {instance.players[0].theta, instance.players[1].theta, instance.players[2].theta, instance.players[3].theta};
        instance.assignValuesToField(prevThetaValues, "prevTheta");

        // Assign theta

        // Cos-ver
        /*int[] thetaValues = {(int)Mathf.Abs(66*Mathf.Cos(Time.time)), (int)Mathf.Abs(28*Mathf.Cos(Time.time)),
        (int)Mathf.Abs(74*Mathf.Cos(Time.time)), (int)Mathf.Abs(12*Mathf.Cos(Time.time))};*/

        // EEG-ver
        /*float[] thetaValues = {instance.EEGplayers[0].att, instance.EEGplayers[1].att, instance.EEGplayers[2].att, instance.EEGplayers[3].att};*/

        // Test-ver
        float[] thetaValues = {instance.p1Power, instance.p2Power, instance.p3Power, instance.p4Power};
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

    // Placeholder to decide the placement of the players, updates every delay sec
    public static void setPlacements(gamePlay instance){

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

    // Gives boost to remaining players when ones finishes by nerfing AI for a while, no lightning, lesser power for 5 sec
    public static void boost(gamePlay instance){
        
        if (!instance.isBoosted) {
            instance.isBoosted = true; 

            instance.AI.canUpdate = false;  // Stop update() in AI

            instance.AI.state = 0;  // AI-state at 0, no lightning can occur
            instance.AI.power = AIScript.placementPlayer(instance.AI, instance.players.Length).power - 10;  // AI-power reduced to last placed player's power minus 10

            instance.StartCoroutine(instance.EndBoost(instance));
        }
    }


    // Triggers when a player finishes the game, aka radius = 0.0f
    public static void playerFinished(gamePlay instance, int place){

            // Set radius to 0.0f
            var field = typeof(PlayerData).GetField("radius");
            field.SetValueDirect(__makeref(instance.players[(place)]), 0.0f);
            
            instance.players[place].update = false; // Players stats can no longer update

            UnityEngine.Debug.Log("Player " + instance.players[(place)].id + " has finished!");
            
            boost(instance);    // Gives boost to remaining players by decreasing AI for 5 seconds
            playersWon(instance);   // Checks if game is over bc the players won
            
    }

    public int numberOfActivePlayers(){

        int activePlayers = 0;
        
        for (int i = 0; i < players.Length; i++) {
        
            if(players[i].update){
                activePlayers++;
            }
        }

        UnityEngine.Debug.Log("Number of remaining players are: " + activePlayers);

        return activePlayers;
    }


    /**********************************Main Functions*************************************/

    // Set consistency of each player
    public static void setConsistency(gamePlay instance){
        
        float[] consistencyValues = new float[instance.players.Length];

        for (int i = 0; i < instance.players.Length; i++) {
        
            float con = 100 - change(instance.players[i].power, instance.players[i].prevPower);

            if(con < instance.players[i].consistency){
                consistencyValues[i] = con;
            }
            else{
                consistencyValues[i] = instance.players[i].consistency;
            }
        }

        instance.assignValuesToField(consistencyValues, "consistency"); //  Assign consistencyValues to players
    }

    // Set unbothered of each player, is utilized in AI-class
    public static void setUnbothered(gamePlay instance, int id){

        float unbothered = 100 - change(instance.players[id-1].power, instance.players[id-1].prevPower);

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

    public static float calculateRadius(gamePlay instance, int place){ 

        if(instance.players[place].update == false){
             return 0.0f;
        }
        
        float addOn = 0.0f;
        
        // Normal increase (closer to center)
        if(instance.players[(place)].power > instance.AI.power){
            addOn = -((instance.players[(place)].power - instance.AI.power)/500 - instance.adjust);
        }

        // Normal decrease (further from center)
        if(instance.players[(place)].radius < 3.0f && instance.players[(place)].power < instance.AI.power){   // Must be less than radius of vortex
            addOn = (instance.AI.power - instance.players[(place)].power)/500 + instance.adjust;
        }

        /*if(instance.players[(place)].id == 4){  // Write out increase/decrease for player 4
            UnityEngine.Debug.Log("addOn = " + addOn);
        }*/

        float radius = instance.players[(place)].radius + addOn;    // Calculate the new radius

        // Keep radius inbetween possible values (aka 0 and 2)
        if(radius > 3.0f){
            radius = 3.0f;
        }
        else if(radius < 0.0f){
            radius = 0.0f;
            playerFinished(instance, place);    // Triggers boost effect
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
    
    public static void set10Vectors(gamePlay instance){

        instance.players10Power.Add(AIScript.calculateTeamPower(instance.AI));    // Add latest teamPower last
        instance.ai10Power.Add(instance.AI.power);    // Add latest AI-power last
    }
    
    // Decides who is currently winning return either 1, 0 or -1 based on status of the game, updates every 10 sec
    public static int whoseWinning(gamePlay instance){
        // 1 = players are winning

        // -1 = AI is winning

        // 0 = game is over
        
        int ai10Length = instance.ai10Power.Count;
        // UnityEngine.Debug.Log("ai10Length = " + ai10Length);
        int players10Length = instance.players10Power.Count;
        // UnityEngine.Debug.Log("players10Length = " + players10Length);
        
        if(ai10Length == players10Length && ai10Length != 0){
            // UnityEngine.Debug.Log("Entered sum!");

            if(instance.numberOfActivePlayers() == 0){
                return 0;
            }
            
            float aiPowerSum = 0.0f;
            float playerPowerSum = 0.0f;
            
            for(int i = ai10Length-1; i >= 0 ; i--){
                
                // Add to sums
                aiPowerSum += instance.ai10Power[i];
                playerPowerSum += instance.players10Power[i];

                // Remove last member of lists to reset them
                instance.ai10Power.RemoveAt(instance.ai10Power.Count - 1);
                instance.players10Power.RemoveAt(instance.players10Power.Count - 1);        
            }

            // UnityEngine.Debug.Log("Exit");

            // Calculate average
            aiPowerSum = aiPowerSum / ai10Length;
            playerPowerSum = playerPowerSum/ players10Length;

            if(aiPowerSum > playerPowerSum){
                UnityEngine.Debug.Log("Returned -1");
                return -1;
            }
            else if(aiPowerSum < playerPowerSum){
                UnityEngine.Debug.Log("Returned 1");
                return 1;
            }
        }

        UnityEngine.Debug.Log("Returned 1");
        return 1;
    }


    // Returns if all players have won
    public static void playersWon(gamePlay instance){


        if(instance.numberOfActivePlayers() == 0){ // Check if no players are active
            instance.canUpdate = false;
            instance.canUpdate1sec = false;
            instance.canUpdate10sec = false;

            UnityEngine.Debug.Log("Game is over, player have won!");
        }

    }
     
    /************************************************************************************/
}
