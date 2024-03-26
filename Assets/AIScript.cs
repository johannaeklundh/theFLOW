using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Added
/*using UnityEngine.UI;         Uncomment to display text ingame concerning data here*/

public class AIScript : MonoBehaviour
{
    
    //Default functions, may be of use later...
    // Start is called before the first frame update
    void Start()
    {
        //calculatePower(this);   // Runs this function on start, see changes to the right
        //setState(this);
        setPlacements(this);
        player1.DisplayPlayerInfo();
        player2.DisplayPlayerInfo();
        player3.DisplayPlayerInfo();
        player4.DisplayPlayerInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    /************INFO concerning the following code************
    
    RULES:
    1. Varible-names for constants will always begin with a big letter.
    2. Function-names will always begin with a small letter
    3. Regular variable-names will begin with a small letter


    Necessary inputs from other code:
    - Power from the player-team
    - Placements of each player

    Necessary outputs from this code:
    - Power from the AI
    - Answers for these questions concerning lightning:
        1. WHO got hit by the lighning?
        2. HOW hard did they get hit?
        3. DID they even get hit at all? (This one may be added to other code)

    *********************************************************/








    /**********************Public varables******************/
    public int state = 0; // State of the AI, dephends on players performance and how "threatened" it feels, 0 = NEUTRAL, 1 = SLIGHTLY THREATNED, 2 = THREATENED, 3 = GREATLY THREATENED
    public int power = 50; // The AIs restitance against the players, starts at 50

    public bool gotHit = false; // Decides weather a player got hit by lightning based on chance




    /************Inputs(placeholders)*********/
    
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


    /************Constants*********/
    
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

    // Placeholder to decide the placement of the players
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
        instance.power = (int)instance.teamPower;
    }

    // Placeholder to set the state of the AI dephending on performance (change to case)
    public static void setState(AIScript instance){

        switch(instance.pos1)
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

    /*/ Placeholder to answer WHO got hit by lightning
    public static int playerHit(AIScript instance){

    }*/

    /*/ Placeholder to answer if a player DID get hit by lightning
    public static void isHit(AIScript instance){

    }*/

}
