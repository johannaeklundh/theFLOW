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
    //public ChangeSpeed changeSpeed; 

    /***********************************************************/

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(delayUpdate(3.0f)); // Delay Start() by 3 seconds
    }

    // Update is called once per frame
    void Update()
    {  

        if(canUpdate && GP.isBoosted == false){
            calculatePower(this);   // Caculates and set the power of AI
            setState(this);   // Sets the state of the AI
            isHit(this);

            canUpdate = false;  // Makes it so that each function doesn't update every frame

            // Start the coroutine (allows to delay update or execute over several frames) to enable updates after 3 seconds
            StartCoroutine(delayUpdate(delay));

        }
        
    }
    

    /************INFO concerning the following code************
    
    RULES:
    1. Varible-names for constants will always begin with a big letter.
    2. Function-names will always begin with a small letter
    3. Regular variable-names will begin with a small letter
    4. Struct-names always begin with a big letter


    Necessary inputs from other code:
    - Power from the player-team
    - radius of each player

    Necessary outputs from this code:
    - Power from the AI
    - Answers for these questions concerning lightning:
        1. WHO got hit by the lighning?
        2. HOW hard did they get hit?
        3. DID they even get hit at all? (This one may be added to other code)

    *********************************************************/


    /************Things used obly to control update(), not relevant for behaviour************/
    public bool canUpdate = false; // Decides weather a function can update in update()

    public float delay = 0.25f;
    
    public IEnumerator delayUpdate(float d){

        // Wait for d seconds
        yield return new WaitForSeconds(d);

        // Allow updates to happen
        canUpdate = true;

        delay = 0.25f;   // Reset delay

       // Debug.Log("Reached delayUpdate!");
    }

    /***********************************************************/


    





    /**********************Public varables******************/
    public int state = 0; // State of the AI, dephends on players performance and how "threatened" it feels, 0 = NEUTRAL, 1 = SLIGHTLY THREATNED, 2 = THREATENED, 3 = GREATLY THREATENED
    public float power = 50; // The AIs restitance against the players, starts at 50


    /***********Constants*********/


    // Lightning Constants, how many steps a player is thrown back when struck by lightning dephening on AI-state
    public const float LIGHT = 0.15f;
    public const float MEDIUM = 0.25f;
    public const float HARD = 0.35f;

    // Percentages of getting hit by lightning based on state, no lighning in NEUTRAL-state
    public const int PerState1 = 10; // 4% chance of getting hit per second when state = 1
    public const int PerState2 = 14; // 8% chance of getting hit per second when state = 2
    public const int PerState3 = 18; // 16% chance of getting hit per second when state = 3


    // Other
    private const float LightningRadious = 2.0f;    // The radius a player have to be within to be able to get hit by lightning


    /**********************Functions************************/

    // Calcutes the AIs power
    public static void calculatePower(AIScript instance){   // FIX so that it works for 1 or 2 players
        
        float maxIncrease = 0.0f;    // Maximum amount of increase in power for the AI
        float minDecrease = 0.0f;    // Minimum amount of decrease in the power for the AI

        // How far apart the player placed in 1:st place is from the player placed in last
        float diff1_last = Mathf.Abs(placementPlayer(instance, 1).radius - placementPlayer(instance, instance.GP.players.Length).radius);

        // How far apart the player placed in 1:st place is from the player placed in 2:th place, small diffrence can result in big boost to the others
        float diff1_2 = Mathf.Abs(placementPlayer(instance, 1).radius - placementPlayer(instance, 2).radius);


        // Assign max and min distance
        float currentTeamP = calculateTeamPower(instance);

        // Check state, metric on how close the player placed clostest to center is to winning 
        if(instance.state == 3){    // AI is more likely to get higher numbers and less likely to get lower values

            // Check if teamPower is over or under 50
            if(currentTeamP > 50){  

                if(currentTeamP > 80){  // Players are doing really good
                    maxIncrease = 20;
                    minDecrease = -20;
                }
                else{   // Players are is doing good
                    maxIncrease = 30;
                    minDecrease = -10;
                }
            }
            else{   
                if(currentTeamP < 20){  // Players are is doing really bad
                    maxIncrease = 30;
                    minDecrease = -5;
                }
                else{   // Players are doing bad
                    maxIncrease = 30;
                    minDecrease = -15;
                }
            }
        }
        else if(instance.state == 2){

            // Check if teamPower is over or under 50
            if(currentTeamP > 50){  

                if(currentTeamP > 80){  // Players are doing really good
                    maxIncrease = 10;
                    minDecrease = -20;
                }
                else{   // Players are doing good
                    maxIncrease = 20;
                    minDecrease = -10;
                }
            }
            else{   
                if(currentTeamP < 20){  // Players are doing really bad
                    maxIncrease = 30;
                    minDecrease = -10;
                }
                else{   // Players are doing bad
                    maxIncrease = 20;
                    minDecrease = -15;
                }
            }
        }
        else if(instance.state == 1){

            // Check if teamPower is over or under 50
            if(currentTeamP > 50){  

                if(currentTeamP > 80){  // Players are doing really good
                    maxIncrease = 5;
                    minDecrease = -40;
                }
                else{   // Players are doing good
                    maxIncrease = 10;
                    minDecrease = -30;
                }
            }
            else{   
                if(currentTeamP < 20){  // Players are doing really bad
                    maxIncrease = 20;
                    minDecrease = -10;
                }
                else{   // Players are doing bad
                    maxIncrease = 20;
                    minDecrease = -15;
                }
            }
        }
        else{   // state = 0

            // Check if teamPower is over or under 50
            if(currentTeamP > 50){  

                if(currentTeamP > 80){  // Players are doing really good
                    maxIncrease = 5;
                    minDecrease = -50;
                }
                else{   // Player is doing good
                    maxIncrease = 10;
                    minDecrease = -40;
                }
            }
            else{   

                if(currentTeamP < 20){  // Players are doing really bad
                    maxIncrease = 15;
                    minDecrease = -15;
                }
                else{   // Players are doing bad
                    maxIncrease = 10;
                    minDecrease = -20;
                }
            }
        }

        
        float avgMeanAlpha = 0.0f;  // Average for each active players meanAlpha
        float avgMeanTheta = 0.0f;  // Average for each active players meanTheta

        foreach(var player in instance.GP.players){
            
            if(player.update){
                avgMeanAlpha += player.meanAlpha;
                avgMeanTheta += player.meanTheta;
            }
        }

        avgMeanAlpha = avgMeanAlpha/instance.GP.numberOfActivePlayers();
        avgMeanTheta = avgMeanTheta/instance.GP.numberOfActivePlayers();
        
        // Bonus for the players
        maxIncrease -= avgMeanAlpha/15;
        minDecrease -= avgMeanTheta/15;


        // Generate random integer inbetween min and max, decides how much the AI:s power will increase/decrease
        float inc_dec = Random.Range(minDecrease, maxIncrease);

        instance.power = inc_dec + calculateTeamPower(instance); // Adds onto the player-teams average power

        //UnityEngine.Debug.Log("AI Power: " + instance.power);
    }

    // Calculates the player-teams averge power of the currenlty active players
    public static float calculateTeamPower(AIScript instance){

        float sum = 0.0f;   // Sum of all active players power
        int nrOfActivePlayers = 0;

        for(int i = 0; i < instance.GP.players.Length; i++){    // Loop through all players

            if(instance.GP.players[i].update){  // Only update if the player is active
                sum += instance.GP.players[i].power;
                nrOfActivePlayers++;
            }
        }

        //instance.changeSpeed.leading(sum / nrOfActivePlayers, instance.power);

        // UnityEngine.Debug.Log("TEAM Power: " + sum / nrOfActivePlayers + "\nAI Power: " + instance.power);
        return sum/nrOfActivePlayers;
    }




    // Find a what player holds a certain placement
    public static gamePlay.PlayerData placementPlayer(AIScript instance, int place){

        // Test weather place is possible
        if(place > instance.GP.players.Length){
             throw new System.Exception("Invalid placement entered, try 1-4");
        }

        foreach(var player in instance.GP.players){

            if(player.placement == place){
                return player;
            }
        }
        throw new System.Exception("Could not find player at placement " + place);
    }





    // Placeholder to set the state of the AI dephending on the player radiused the closest to the center (change to include other players and update once every 3 sec)
    public static void setState(AIScript instance, int s = 4){  // FIX for the new closest

        if(s == 4){
            //switch(placementPlayer(instance, 1).radius)   // Uses the player whose placement is 1:s radius
            switch(placementPlayer(instance, 1).radius)
            {
                case float n when n <= 0.6f:
                    instance.state = 3;
                    break;
                case float n when n <= 0.9f:
                    instance.state = 2;
                    break;
                case float n when n <= 1.5f:
                    instance.state = 1;
                    break;
                default:
                    instance.state = 0;
                    break;
            }
        }
        else{
            instance.state = s; // FIX
        }
    }

    // Answers if a player DID get hit by lightning
    public static void isHit(AIScript instance){

        int chance = 0; // The chance of getting hit based on the state of th AI

        // Chance of getting hit is generated based on the AIs state where higher state equals higher chance of hit
        switch(instance.state)
        {
            case int n when n == 3:
                chance = PerState3;
                break;
            case int n when n == 2:
                chance = PerState2;
                break;
            case int n when n == 1:
                chance = PerState1;
                break;
            default:
                chance = 0;
                break;
        }

        // Genrate random integer inbetween 0-100
        int randomInt = Random.Range(0, 100);

        if(randomInt <= chance){    // There is a chance% chance of getting hit
            playerHit(instance);    // If hit, calls upon the function playerHit that return the id of a hit player and how hard they got hit.
        }
    }





    // Answers WHO got hit by lightning and HOW hard they got hit
    public static void playerHit(AIScript instance){    // Only the players placed 1 to next last can get hit to not bully last player <--LOVE THIS

        // Generate random integer inbetween 1 and the next last placement, decides what player at the generated placement got hit
        
        int secondLastPlayer = instance.GP.players.Length;
        int placement = 1;

        if(secondLastPlayer > 1){   // Check if more than 1 player
            placement = Random.Range(1, secondLastPlayer);
        }

        Debug.Log("The placement who shall be hit is " + placement);

        gamePlay.PlayerData player = placementPlayer(instance, placement);  // Find player

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

            Debug.Log("playerHit:   Player " + player.id  + " whose placement is " + player.placement + " and who = " + who + " and how = " + how);
            // player.displayPlayerInfo();

            // Moving the player in question back "how" many steps
            var field = typeof(gamePlay.PlayerData).GetField("radius"); 
            field.SetValueDirect(__makeref(instance.GP.players[who-1]), (instance.GP.players[who-1].radius + how)); 

            // Test if possible value
            if(instance.GP.players[who-1].radius > 3.0f){    // If larger than 2, set to 2 (min-value)
                field.SetValueDirect(__makeref(instance.GP.players[who-1]), 3.0f);
            }
            else if(instance.GP.players[who-1].radius < 0.0f){   // If lesser than 0, set to 0 (max-value)
                field.SetValueDirect(__makeref(instance.GP.players[who-1]), 0.0f);
            }

            // Visual effect of getting hit by lightning (add sound)
            if (instance.lightningEffect != null) //Lightning when player gets hit
            {
                instance.lightningEffect.ActivateLightning(player.id);

            }

            //gamePlay.PlayerData player = gamePlay.idPlayer(instance.GP, who);
            gamePlay.setUnbothered(instance.GP, who);   // Updates unbothered of the player that got hit in gamePlay-class
        }
    }

}

