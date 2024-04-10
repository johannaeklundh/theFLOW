using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamePlay : MonoBehaviour
{
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


            // Display all that is stored in playerData in console
            Debug.Log("\nPlayer1:");
            player1.displayPlayerInfo();
            Debug.Log("\nPlayer2:");
            player2.displayPlayerInfo();
            Debug.Log("\nPlayer3:");
            player3.displayPlayerInfo();
            Debug.Log("\nPlayer4:");
            player4.displayPlayerInfo();       
                 

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
    /********************************************************************************'******/






    /*************************************Structs****************************************/
    public struct PlayerData{
        public int id;
        public int position;
        public int placement;
        /*public float power;
        public float meanAlpha;
        public float meanTheta;
        public float consistency;
        public float unbothered;
        public float balance;*/
        
        // Constructor
        public PlayerData(int playerID, int playerPosition, int playerPlacement)
        {
            id = playerID;
            position = playerPosition;
            placement = playerPlacement;
        }

        // A way to display each player info in the console
        public void displayPlayerInfo()
        {
            Debug.Log("Player ID: " + id);
            Debug.Log("Player Position: " + position);
            Debug.Log("Player Placement: " + placement);
        }
    }

    /************************************************************************************/

    
    
    
    
    
    /***********************************Variables****************************************/
    
    
    
    // Create the 4 players (may change due to the number of players beign flexible)
    PlayerData player1 = new PlayerData(1, 0, 1);
    PlayerData player2 = new PlayerData(2, 0, 2);
    PlayerData player3 = new PlayerData(3, 0, 3);
    PlayerData player4 = new PlayerData(4, 0, 4);


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













    /************************************************************************************/
}
