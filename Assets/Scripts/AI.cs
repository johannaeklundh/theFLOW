using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Added
/*using UnityEngine.UI;         Uncomment to display text ingame concerning data here*/

public class AIScript : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        calculatePower(this);   // Runs this function on start, see changes to the right
        setPlacements(this);    // Test to set the correct placements of the players, must always intialize or all placements will be wrong
        setState(this);   // Test to set the state of the AI
        player1.DisplayPlayerInfo();    // Displays all info stored in the PlayerData struct
        player2.DisplayPlayerInfo();
        player3.DisplayPlayerInfo();
        player4.DisplayPlayerInfo();
        //Debug.Log("The player whose placement is 1 is player nr: " + placementPlayer(this, 1).id);  // Tests the search for player at placement function by writing out who is currently in placement 1
        isHit(this);
    }

    // Update is called once per frame
    void Update()
    {  
        if(canUpdate){
            calculatePower(this);   // Runs this function on start, see changes to the right
            setPlacements(this);    // Test to set the correct placements of the players, must always intialize or all placements will be wrong
            setState(this);   // Test to set the state of the AI
            player1.DisplayPlayerInfo();    // Displays all info stored in the PlayerData struct
            player2.DisplayPlayerInfo();
            player3.DisplayPlayerInfo();
            player4.DisplayPlayerInfo();
            //Debug.Log("The player whose placement is 1 is player nr: " + placementPlayer(this, 1).id);  // Tests the search for player at placement function by writing out who is currently in placement 1
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
    - Position of each player

    Necessary outputs from this code:
    - Power from the AI
    - Answers for these questions concerning lightning:
        1. WHO got hit by the lighning?
        2. HOW hard did they get hit?
        3. DID they even get hit at all? (This one may be added to other code)

    *********************************************************/


    /************Things used obly to control update(), not relevant for behaviour************/
    private bool canUpdate = true; // Decides weather a function can update in update()

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




    /***********Inputs(placeholders)*********/
    
    // Current position of each player, 0 = outer edge, 51 = center (GOAL), placeholder taken from FLOW - EN VIRVEL
    public int pos1 = 35;   // current position for player 1
    public int pos2 = 26;   // current position for player 2
    public int pos3 = 41;   // current position for player 3 (diffrent from paper due to wanting to test setPlacements)
    public int pos4 = 0;    // current position for player 4

    // Handles all data unique to each player
    public struct PlayerData{
        public int id;
        public int position;
        public int placement;
        
        // Constructor
        public PlayerData(int playerID, int playerPosition, int playerPlacement)
        {
            id = playerID;
            position = playerPosition;
            placement = playerPlacement;
        }

        // A way to display each player info in the console
        public void DisplayPlayerInfo()
        {
            Debug.Log("Player ID: " + id);
            Debug.Log("Player Position: " + position);
            Debug.Log("Player Placement: " + placement);
        }
    }

    // Create the 4 players (may change due to the number of players beign flexible)
    PlayerData player1 = new PlayerData(1, 0, 1);
    PlayerData player2 = new PlayerData(2, 0, 2);
    PlayerData player3 = new PlayerData(3, 0, 3);
    PlayerData player4 = new PlayerData(4, 0, 4);


    public float teamPower = 47.5F; // current power of the player-team


    /***********Constants*********/
    
    // Max/Min Power Constants
    public const int MaxPower = 100;    // Maximum POWER-output by the AI dephending on players performance
    public const int MinPower = 0;      // Minimum POWER-output by the AI dephending on players performance

    
    // Lightning Constants, how many steps a player is thrown back when struck by lightning dephening on AI-state
    public const int LIGHT = 1;
    public const int MEDIUM = 2;
    public const int HARD = 3;

    // Percentages of getting hit by lightning based on state, no lighning in NEUTRAL-state
    public const int PerState1 = 4; // 4% chance of getting hit per second when state = 1
    public const int PerState2 = 8; // 8% chance of getting hit per second when state = 2
    public const int PerState3 = 16; // 16% chance of getting hit per second when state = 3




    /**********************Functions************************/


    // Takes in the position of a player in x- and y-direction and makes it a distance from center, assumes center of screen is (0,0) (maybe move to gameLogic)
    public static int setPosition(AIScript instance, int x, int y){
        float position = Mathf.Sqrt(Mathf.Abs(x) + Mathf.Abs(y));    // Pythtagoras theorem to get distance from center
        return (int)position;
    }   // Add more things dephending on overall performance based of of gameLogic



    // Placeholder to decide the placement of the players, change to update every 5 sec or smth
    public static void setPlacements(AIScript instance){

        instance.player1.position = instance.pos1;
        instance.player2.position = instance.pos2;
        instance.player3.position = instance.pos3;
        instance.player4.position = instance.pos4;
        
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
    


    
    // Placeholder to calcute the AIs power
    //[ContextMenu("CalculatePower")] // Makes it so that we can run the function on command when playing the game by pressing the 3 dot beside the script on the right (only work on non-static)
    public static void calculatePower(AIScript instance){   
        
        int maxIncrease = 5;    // Maximum amount of increase in power for the AI
        int minDecrease = -5;   // Minimum amount of decrease in the power for the AI

        // How far apart the player placed in 1:st place is from the player placed in 4:th place
        int diffrencePlacement1_4 = instance.player1.position - instance.player4.position;

        // How far apart the player placed in 1:st place is from the player placed in 2:th place, small diffrence can result in big boost to the others
        int diffrencePlacement1_2 = instance.player1.position - instance.player2.position;

        if(instance.state == 3 && diffrencePlacement1_2 < 10){  // If the AI seem to be losing and there is 2 players near finishing, they AI may push harder
            maxIncrease = 10;
            if(diffrencePlacement1_4 > 30){ // If there is a super huge gap between the first and the last placement, the AI may not push as hard
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

        instance.power = (float)inc_dec + instance.teamPower;
    }






    // Find a what player holds a certain placement
    public static PlayerData placementPlayer(AIScript instance, int place){

        // Test weather place is possible
        if(place > 4){
             throw new System.Exception("Invalid placement entered, try 1-4");
        }

        // Create an array containing each player (put this in a sepperate create-player funtion for when you can add less than 4 players)
        PlayerData[] players = {(PlayerData)instance.player1, (PlayerData)instance.player2, (PlayerData)instance.player3, (PlayerData)instance.player4};

        // Search for player whose placement matches the searched place and return the player
        for(int i = 0; i < players.Length; i++)
        {
            if(players[i].placement == place){
                return players[i];
            }
        }

        //return players[0];
        throw new System.Exception("Could not find player at placement " + place);
    }





    // Placeholder to set the state of the AI dephending on the player positioned the closest to the center (change to include other players and update once every 5 sec)
    public static void setState(AIScript instance){

        switch(placementPlayer(instance, 1).position)   // Uses the player whose placement is 1:s position
        {
            case int n when n >= 46:
                instance.state = 3;
                break;
            case int n when n >= 40:
                instance.state = 2;
                break;
            case int n when n >= 33:
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
    public static (int, int) playerHit(AIScript instance){

        // Only the players placed 1-3 can get hit

        // Generate random integer inbetween 1-3, decides what player at the generated placement got got hit
        int placement = Random.Range(1, 3);
        int who = placementPlayer(instance, placement).id;

        int how = 0;    // How hard the player got hit based on the state of the AI

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

        // Debug.Log("Player ID: " + who + ", how hard: " + how);
        return (who, how);
    }

}
