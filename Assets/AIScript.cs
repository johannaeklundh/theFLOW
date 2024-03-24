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
        CalculatePower(this);   // Runs this function on start, see changes to the right
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    /************INFO concerning the following code************
    
    RULES:
    1. Varible-names for constants will always begin with a big letter.
    2. Function-names will also always begin whith a big letter

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
    public int pos3 = 21;   // current position for player 3
    public int pos4 = 0;    // current position for player 4

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
    
    // Placeholder to calcute the AIs power
    [ContextMenu("CalculatePower")] // Makes it so that we can run the function on command when playing the game by pressing the 3 dot beside the script on the right
    public static void CalculatePower(AIScript instance){   
        instance.power = (int)instance.teamPower;
    }

}
