using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Added
/*using UnityEngine.UI;         Uncomment to display text ingame concerning data here*/

public class AIScript : MonoBehaviour
{
    /********Refrence instances to other classes/scripts********/
    public gamePlay GP;

    /***********************************************************/
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(delayUpdate()); // Delay Start() by 3 seconds
    }

    // Update is called once per frame
    void Update()
    {  
        if(canUpdate){
            calculatePower(this);   // Runs this function on start, see changes to the right
            setState(this);   // Test to set the state of the AI
            isHit(this);

            canUpdate = false;  // Makes it so that each function doesn't update every frame

            // Start the coroutine (allows to delay update or execute over several frames) to enable updates after 3 seconds
            StartCoroutine(delayUpdate());

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
    private bool canUpdate = false; // Decides weather a function can update in update()
    
    IEnumerator delayUpdate(){

        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Allow updates to happen
        canUpdate = true;
    }

    /***********************************************************/


    





    /**********************Public varables******************/
    public int state = 0; // State of the AI, dephends on players performance and how "threatened" it feels, 0 = NEUTRAL, 1 = SLIGHTLY THREATNED, 2 = THREATENED, 3 = GREATLY THREATENED
    public float power = 50; // The AIs restitance against the players, starts at 50


    /***********Constants*********/

    
    // Lightning Constants, how many steps a player is thrown back when struck by lightning dephening on AI-state
    public const float LIGHT = 0.1f;
    public const float MEDIUM = 0.2f;
    public const float HARD = 0.3f;

    // Percentages of getting hit by lightning based on state, no lighning in NEUTRAL-state
    public const int PerState1 = 50; // 4% chance of getting hit per second when state = 1
    public const int PerState2 = 75; // 8% chance of getting hit per second when state = 2
    public const int PerState3 = 100; // 16% chance of getting hit per second when state = 3




    /**********************Functions************************/

    // Placeholder to calcute the AIs power
    //[ContextMenu("CalculatePower")] // Makes it so that we can run the function on command when playing the game by pressing the 3 dot beside the script on the right (only work on non-static)
    public static void calculatePower(AIScript instance){   
        
        int maxIncrease = 5;    // Maximum amount of increase in power for the AI
        int minDecrease = -5;   // Minimum amount of decrease in the power for the AI

        // How far apart the player placed in 1:st place is from the player placed in 4:th place
        float diffrencePlacement1_4 = instance.GP.players[0].radius - instance.GP.players[3].radius;

        // How far apart the player placed in 1:st place is from the player placed in 2:th place, small diffrence can result in big boost to the others
        float diffrencePlacement1_2 = instance.GP.players[0].radius - instance.GP.players[1].radius;

        if(instance.state == 3 && diffrencePlacement1_2 < 10.0f){  // If the AI seem to be losing and there is 2 players near finishing, they AI may push harder
            maxIncrease = 10;
            if(diffrencePlacement1_4 > 30.0f){ // If there is a super huge gap between the first and the last placement, the AI may not push as hard
                minDecrease = -7;
            }
            else{   // Otherwise it may push harder to not give a sudden boost that makes everyone finish.
                minDecrease = -3; 
            }
        }
        else if(instance.state == 2){
            maxIncrease = 7;
            minDecrease = -5;
        }
        else if (instance.state == 1){
            maxIncrease = 6;
            minDecrease = -4;
        }
        else{
            maxIncrease = 5;
            minDecrease = -5;
        }

        // Generate random integer inbetween min and max, decides how much the AI:s power will increase/decrease
        int inc_dec = Random.Range(minDecrease, maxIncrease);

        instance.power = (float)inc_dec + calculateTeamPower(instance); // Adds onto the player-teams average power
    }

    // Calculates the player-teams averge power
    public static float calculateTeamPower(AIScript instance){
        return (instance.GP.players[0].power + instance.GP.players[1].power + instance.GP.players[2].power + instance.GP.players[3].power)/4;
    }




    // Find a what player holds a certain placement
    public static gamePlay.PlayerData placementPlayer(AIScript instance, int place){

        // Test weather place is possible
        if(place > 4){
             throw new System.Exception("Invalid placement entered, try 1-4");
        }

        foreach(var player in instance.GP.players){

            if(player.placement == place){
                return player;
            }
        }
        throw new System.Exception("Could not find player at placement " + place);
    }





    // Placeholder to set the state of the AI dephending on the player radiused the closest to the center (change to include other players and update once every 5 sec)
    public static void setState(AIScript instance){

        //switch(placementPlayer(instance, 1).radius)   // Uses the player whose placement is 1:s radius
        switch(placementPlayer(instance, 1).radius)
        {
            case float n when n >= 0.46f:
                instance.state = 3;
                break;
            case float n when n >= 0.40f:
                instance.state = 2;
                break;
            case float n when n >= 0.33f:
                instance.state = 1;
                break;
            default:
                instance.state = 0;
                break;
        }
    }

    // Placeholder to answer if a player DID get hit by lightning
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





    // Placeholder to answer WHO got hit by lightning and HOW hard they got hit
    public static void playerHit(AIScript instance){

        // Only the players placed 1-3 can get hit

        // Generate random integer inbetween 1-3, decides what player at the generated placement got got hit
        int placement = Random.Range(1, 4);
        //Debug.Log("Random placement between 1 and 3 is " + placement);
        gamePlay.PlayerData player = placementPlayer(instance, placement);

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
        gamePlay.calculateRadius(instance.GP, player, how);   // Updates unbothered of the player that got hit in gamePlay-class


        //gamePlay.PlayerData player = gamePlay.idPlayer(instance.GP, who);
        gamePlay.setUnbothered(instance.GP, who);   // Updates unbothered of the player that got hit in gamePlay-class
    }

}

