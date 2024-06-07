using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Added
/*using UnityEngine.UI;         Uncomment to display text ingame concerning data here*/

public class AIScript : MonoBehaviour
{
    /********Refrence instances to other classes/scripts********/
    public gamePlay GP;

    public LightningEffect lightningEffect;

    private AudioSource audioSource;    // Reference to the AudioSource component

    /***********************************************************/
    
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(delayUpdate(6.0f)); // Delay Start()
    }

    // Update is called once per frame
    void Update()
    {  

        if(canUpdateAI && !GP.isBoosted && !GP.gameOver){
            calculatePower(this);   // Caculates and set the power of AI
            setState();   // Sets the state of the AI
            isHit(this);

            canUpdateAI = false;  // Makes it so that each function doesn't update every frame

            // Start the coroutine (allows to delay update or execute over several frames) to enable updates after 3 seconds
            StartCoroutine(delayUpdate(delay));

        }
        
    }
    

    /************INFO concerning the following code************

    Necessary inputs from gamePlay.cs:
    - Each player's playerData
    - If game is currently boosted
    - If game is finished
    - Audio and visuals for the lightning


    Necessary outputs from this code:
    - Power from the AI
    - Answers for these questions concerning lightning:
        1. WHO got hit by the lighning?
        2. HOW hard did they get hit?
        3. DID they even get hit at all? 
        4. Audio and visuals for the lightning
        5. Trigger unBothered in gamePlay

    *********************************************************/


    /************Things used obly to control update(), not relevant for behaviour************/
    public bool canUpdateAI = false; // Decides weather a function can update in update()

    private float delay = 0.25f;    // The current update-rate for everything in update()
    
    public IEnumerator delayUpdate(float d){

        // Wait for d seconds
        yield return new WaitForSeconds(d);

        // Allow updates to happen
        canUpdateAI = true;

        delay = 0.25f;   // Reset delay

       // Debug.Log("Reached delayUpdate!");
    }

    /***********************************************************/


    





    /**********************Public varables******************/
    public int state = 0; // State of the AI, dephends on players performance and how "threatened" it feels, 0 = NEUTRAL, 1 = SLIGHTLY THREATNED, 2 = THREATENED, 3 = GREATLY THREATENED
    public float power = 50; // The AIs resistance against the players, starts at 50



    /**********************Constants**********************/

    
    // Zones (radius) for triggering diffent staes
    private const float ForState1 = 2.0f; // radius required to trigger state 1
    private const float ForState2 = 1.5f; // radius required to trigger state 2
    private const float ForState3 = 1.0f; // radius required to trigger state 3
    
    
    // Lightning Constants, how far a player is pushed back when struck by lightning dephening on AI-state (no lightning in state 0)
    private const float LIGHT = 0.15f;  // in state 1
    private const float MEDIUM = 0.25f; // in state 2
    private const float HARD = 0.35f;   // in state 3



    // Percentages of getting hit by lightning based on state, no lightning in NEUTRAL-state
    private const int PerState1 = 4; // chance of getting hit per delay when in state = 1
    private const int PerState2 = 7; // chance of getting hit per delay when in state = 2
    private const int PerState3 = 10; // chance of getting hit per delay when in state = 3


    // Power-limits constants
    private const float MaxPower = 80.0f;   // Maximum power the AI can achieve, rewards good performance 
    private const float MinPower = 30.0f;   // Minimum power the AI can achieve, discourages bad performance 
    
    
    // Performance thesholds (inbetweeen good and awful is bad performance)
    private const int WellPerformanceThreshold = 80;    // The threshold for considered weel performance
    private const int GoodPerformanceThreshold = 60;    // The threshold for considered good performance
    private const int AwfulPerformanceThreshold = 20;    // The lower threshold for considered awful performance
    
    
    // Other

    private const int meanDivider = 20;     // By what number meanAlpha and meanTheta is divied by to nerf AI

    private const float LightningRadious = 2.0f;    // The radius a player have to be within to be able to get hit by lightning

    private const int wellPerformersBonusMultiplier = 3;     // What number of well-perfomers get mulltiplied with to decrease maxIncrease


    /**********************Main Functions************************/
    // Contains the functions that gives the necesseray outputs needed in other parts of code


    // Calculates the AIs power
    public static void calculatePower(AIScript instance){ 
        
        float maxIncrease = 0.0f;    // Maximum amount of increase in power for the AI
        float maxDecrease = 0.0f;    // Minimum amount of decrease in the power for the AI


        // Curret team-average power
        float currentTeamP = instance.calculateTeamPower();

        // Check state, metric on how close the player placed clostest to center is to winning 
        if(instance.state == 3){    // AI is more likely to get higher numbers and less likely to get lower values

            // Check if teamPower is over or under 60
            if(currentTeamP > GoodPerformanceThreshold){  

                if(currentTeamP > WellPerformanceThreshold){  // Players are doing really well
                    maxIncrease = 22;
                    maxDecrease = -16;
                }
                else{   // Players are is doing good
                    maxIncrease = 30;
                    maxDecrease = -14;
                }
            }
            else{   
                if(currentTeamP < AwfulPerformanceThreshold){  // Players are is doing awful
                    maxIncrease = 30;
                    maxDecrease = -5;
                }
                else{   // Players are doing bad
                    maxIncrease = 30;
                    maxDecrease = -15;
                }
            }
        }
        else if(instance.state == 2){

            // Check if teamPower is over or under 60
            if(currentTeamP > 60){  

                if(currentTeamP > 80){  // Players are doing really good
                    maxIncrease = 20;
                    maxDecrease = -20;
                }
                else{   // Players are doing good
                    maxIncrease = 30;
                    maxDecrease = -10;
                }
            }
            else{   
                if(currentTeamP < 20){  // Players are doing really bad
                    maxIncrease = 40;
                    maxDecrease = -10;
                }
                else{   // Players are doing bad
                    maxIncrease = 20;
                    maxDecrease = -15;
                }
            }
        }
        else if(instance.state == 1){

            // Check if teamPower is over or under 60
            if(currentTeamP > 60){  

                if(currentTeamP > 80){  // Players are doing really good
                    maxIncrease = 10;
                    maxDecrease = -40;
                }
                else{   // Players are doing good
                    maxIncrease = 15;
                    maxDecrease = -30;
                }
            }
            else{   
                if(currentTeamP < 20){  // Players are doing really bad
                    maxIncrease = 25;
                    maxDecrease = -10;
                }
                else{   // Players are doing bad
                    maxIncrease = 25;
                    maxDecrease = -15;
                }
            }
        }
        else{   // state = 0

            // Check if teamPower is over or under 60
            if(currentTeamP > 60){  

                if(currentTeamP > 80){  // Players are doing really good
                    maxIncrease = 10;
                    maxDecrease = -50;
                }
                else{   // Player is doing good
                    maxIncrease = 15;
                    maxDecrease = -40;
                }
            }
            else{   

                if(currentTeamP < 20){  // Players are doing really bad
                    maxIncrease = 20;
                    maxDecrease = -15;
                }
                else{   // Players are doing bad
                    maxIncrease = 17;
                    maxDecrease = -20;
                }
            }
        }

        
        // Bonus for the players for doing well overall
        float avgMeanAlpha = 0.0f;  // Average for each active players meanAlpha
        float avgMeanTheta = 0.0f;  // Average for each active players meanTheta

        // Add toghter all meanAlpha and meanTheta from all players
        foreach(var player in instance.GP.players){
            
            if(player.update){
                avgMeanAlpha += player.meanAlpha;
                avgMeanTheta += player.meanTheta;
            }
        }

        int activePlayers = instance.GP.numberOfActivePlayers();    // Get number of active player (players that have not finished)

        avgMeanAlpha = avgMeanAlpha/activePlayers;
        avgMeanTheta = avgMeanTheta/activePlayers;
        
        // Give bonus based on averages on meanAlpha and meanTheta
        maxIncrease -= avgMeanAlpha/meanDivider;
        maxDecrease -= avgMeanTheta/meanDivider;


        // Modifiers based on how far the players are apart (only works when more than 1 active player)
        if(activePlayers > 1){

            gamePlay.PlayerData playerPlace1 = instance.closestPlayerToFinish(1);    // Closest player to finishing
            gamePlay.PlayerData playerPlace2 = instance.closestPlayerToFinish(2);    // Second closest player to finishing

            // How far apart the player closest to finishing and the players placed last are
            float diff1_last = Mathf.Abs(playerPlace1.radius - instance.placementPlayer(instance.GP.players.Length).radius);

            // How far apart the player closest to finishing is from the player placed in 2:th place
            float diff1_2 = Mathf.Abs(playerPlace1.radius - playerPlace2.radius);

            // Lesser chance of generating big numbers the further apart closest and last players are, to help last placed player, may even result in boost
            maxIncrease -= diff1_last;

            // More chance of generating low numbers the further apart closest and 2:nd closest players are to help the closest finishing to help big skillgaps
            maxDecrease -= diff1_2;
        }


        // Reward for multible players doing well, encourages team-effort but also lets some player carry others
        maxIncrease -= instance.numberOFWellperformances()*wellPerformersBonusMultiplier;



        // Generate random integer inbetween min and max, decides how much the AI:s power will increase/decrease
        float inc_dec = Random.Range(maxDecrease, maxIncrease);

        instance.power = inc_dec + currentTeamP; // Adds onto the player-teams average power

        // power should stay withing min- and max-Power, players performing over MaxPower should always be rewarded
        if(instance.power > MaxPower){
            instance.power = MaxPower;
        }
        else if(instance.power < MinPower){
            instance.power = MinPower;
        }

    }

    // Sets the state of the AI dephending on the radius of the player closest to winning
    public void setState(){

        if(GP.numberOfActivePlayers() == 1){
            state = 0;  // Makes it easier for one player to win
            return;
        }
        
        switch(closestPlayerToFinish(1).radius)
        {
            case float n when n <= ForState3:
                state = 3;
                break;
            case float n when n <= ForState2:
                state = 2;
                break;
            case float n when n <= ForState1:
                state = 1;
                break;
            default:
                state = 0;
                break;
        }
    }

    // Answers if a player DID get hit by lightning
    public static void isHit(AIScript instance){

        int chance = 0; // The chance of getting hit based on the state of th AI

        int activePlayers = instance.GP.numberOfActivePlayers();    // Chance of getting hit increases based of number of active players

        if(activePlayers > 1){  // Lightning is only active if there are more than 1 player
            
            // Chance of getting hit is generated based on the AIs state where higher state equals higher chance of hit
            switch(instance.state)
            {
                case int n when n == 3:
                    chance = PerState3 + activePlayers;
                    break;
                case int n when n == 2:
                    chance = PerState2 + activePlayers;
                    break;
                case int n when n == 1:
                    chance = PerState1 + activePlayers;
                    break;
                default:
                    chance = 0;
                    break;
            }

            // Genrate random integer inbetween 0-100
            int randomInt = Random.Range(0, 100);

            if(randomInt <= chance){    // There is a chance% chance of getting hit
                playerHit(instance);    // If hit, calls upon the function playerHit that triggers the hit-effects
            }
        }
    }



    // Answers WHO got hit by lightning and HOW hard they got hit
    public static void playerHit(AIScript instance){    // Only the players placed 1 to next last can get hit to not bully last player <--LOVE THIS

        // Generate random integer inbetween 1 and the next last placement, decides what player at the generated placement got hit
        int numberOfPlayers = instance.GP.players.Length;

        int placement = Random.Range(1, numberOfPlayers); 

        gamePlay.PlayerData player = instance.placementPlayer(placement);  // Find player at placement

        if(player.update && player.radius < LightningRadious){  // Update only if the player hasn't finished (for safety) 

            int who = player.id;    // Player whose id is the one who got hit

            float how = 0;    // How hard the player got hit based on the state of the AI

            switch(instance.state)
            {
                case int n when n == 3:
                    how = HARD;
                    break;
                case int n when n == 2:
                    how = MEDIUM;
                    break;
                default:
                    how = LIGHT;
                    break;
            }

            // Debug.Log("playerHit:   Player " + player.id  + " whose placement is " + player.placement + " and who = " + who + " and how = " + how);
            // player.displayPlayerInfo();
            
            // Moving the player in question back "how" many steps
            var field = typeof(gamePlay.PlayerData).GetField("radius"); 
            field.SetValueDirect(__makeref(instance.GP.players[who-1]), (instance.GP.players[who-1].radius + how)); 

            // Test if value is within possible radius-values
            if(instance.GP.players[who-1].radius > 3.0f){    // If larger than 3, set to 3 (max-value)
                field.SetValueDirect(__makeref(instance.GP.players[who-1]), 3.0f);
            }
            else if(instance.GP.players[who-1].radius < gamePlay.winRadius){   // If lesser than 0, set to 0 (min-value)
                field.SetValueDirect(__makeref(instance.GP.players[who-1]), gamePlay.winRadius);
            }


            // Visual effect of getting hit by lightning (add sound)
            if (instance.lightningEffect != null) //Lightning when player gets hit
            {
                instance.lightningEffect.ActivateLightning(player.id);

            }

            // Get the AudioSource component
            instance.audioSource = instance.GetComponent<AudioSource>();

            // Audio-effect of getting hit by lightning
            if (instance.audioSource != null) //Lightning-audio when player gets hit
            {
                // Play the audio clip
                instance.audioSource.Play();

            }

            //gamePlay.PlayerData player = gamePlay.idPlayer(instance.GP, who);
            gamePlay.setUnbothered(instance.GP, who);   // Updates unbothered of the player that got hit in gamePlay-class
        }
    }

    /*************************************************************/
    






    
    /**********************Other Functions************************/
    // Contains functions that gets used in the main functions or in some cases helps in gamePlay.cs

    
    // Calculates the player-teams averge power of the currenlty active players
    public float calculateTeamPower(){

        float sum = 0.0f;   // Sum of all active players power
        int nrOfActivePlayers = 0;

        for(int i = 0; i < GP.players.Length; i++){    // Loop through all players

            if(GP.players[i].update){  // Only update if the player is active
                sum += GP.players[i].power;
                nrOfActivePlayers++;
            }
        }

        //changeSpeed.leading(sum / nrOfActivePlayers, power);

        // UnityEngine.Debug.Log("TEAM Power: " + sum / nrOfActivePlayers + "\nAI Power: " + power);
        return sum/nrOfActivePlayers;
    }




    // Find player at entered placement
    public gamePlay.PlayerData placementPlayer(int place){

        // Test weather place is possible
        if(place > GP.players.Length){
             throw new System.Exception("Invalid placement entered, try 1-4");
        }

        foreach(var player in GP.players){

            if(player.placement == place){
                return player;
            }
        }
        throw new System.Exception("Could not find player at placement " + place);
    }

    // Returns the currently active player that is either closest or second closest to finishing
    public gamePlay.PlayerData closestPlayerToFinish(int placement){

        gamePlay.PlayerData p1 = new gamePlay.PlayerData(0);    // Will be currently active player closest to winning
        gamePlay.PlayerData p2 = new gamePlay.PlayerData(5);    // Will be currently second active player closest to winning

        p1.radius = 3.1f;    // Assign radius that will always be bigger than possible for other players
        p2.radius = 3.1f;

        foreach(var player in GP.players){

            if(player.update){  // If a player is active
                
                if(player.radius < p1.radius){

                    p2 = p1;        // p2 becomes the prevoiusly closest player
                    p1 = player;    // p1 becomes player is smaller radious
                }
                else if(player.radius < p2.radius){

                    p2 = player;
                }
            }
        }

        if(placement == 2){
            // Debug.Log("Currenly second closest player to finishing is " + p2.id);
            return p2;
        }
        else{
            // Debug.Log("Currenly closest player to finishing is " + p1.id);
            return p1;
        }
    }

    // Returns number of players with power over the threshold, the higher the number, the lower the AI-power and more reward for players
    public int numberOFWellperformances(){

        int counter = 0;

        foreach(var player in GP.players){

            if(player.update && player.power > WellPerformanceThreshold){  // If a player has a lesser radius than p, p becomes that player
                counter++;
            }
        }

        return counter;
    }


    /*************************************************************/

}

