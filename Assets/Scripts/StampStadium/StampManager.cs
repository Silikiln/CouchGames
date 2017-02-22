using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class StampManager : MonoBehaviour {
	public static StampManager Instance { get; private set; }
	public static List<StampPlayer> ActivePlayers = new List<StampPlayer> ();
	private static int totalSpaces, stampedSpaces;

    public GameObject playerUISquare;
    public GameObject playerUIPanel;
    private static GameObject staticPlayerUISquare;
    private static GameObject staticPlayerUIPanel;
    private static GameObject[] playerSquares;

    public Grid gameGrid;
	public EndScreen endScreen;
	public StampPlayer ghost;
	public StampPlayer[] players;

    
    
    //need to add in a call to endScreen.GetComponent(EndScreen).UpdateEnd(player names and such)

	void Start() {
		Instance = this;

		if (TeamSelection.chosenGameIndex == -1) {
			GameTeams.GamepadTeam.Clear ();
			GameTeams.GamepadTeam.Add (Gamepad.Get (0), 0);
			GameTeams.GamepadTeam.Add (Gamepad.Get (1), 1);
			GameTeams.GamepadTeam.Add (Gamepad.Get (2), 1);
		}
			
		ghost.Gamepad = GameTeams.TeamMembers (0) [0];
		Gamepad[] playerGamepads = GameTeams.TeamMembers (1);
		for (int i = 0; i < playerGamepads.Length; i++) {
			players [i].gameObject.SetActive (true);
			players [i].Gamepad = playerGamepads [i];
			ActivePlayers.Add (players [i]);
		}

        //set the size of the square array equals to the number of players...yay statics
        playerSquares = new GameObject[ActivePlayers.Count];
        staticPlayerUIPanel = playerUIPanel;
        staticPlayerUISquare = playerUISquare;
        updatePlayerUI();

        totalSpaces = gameGrid.Width * gameGrid.Height;
		stampedSpaces = 0;
	}

	public static void PlayerKilled(StampPlayer player) {
		ActivePlayers.Remove (player);
		Debug.Log (ActivePlayers.Count);
        if (ActivePlayers.Count == 0) {
			Instance.endScreen.UpdateEnd ("Ghost", new string[0]);
        }else{ updatePlayerUI(); }
	}

	public static void SpaceStamped() {
		if (++stampedSpaces == totalSpaces) {
			Instance.endScreen.UpdateEnd ("Players", new string[0]);
		}
	}

    private static void updatePlayerUI()
    {
        //reset ui elements
        foreach (GameObject uiSquare in playerSquares){Destroy(uiSquare);}
        int squaresIndex = 0;
        foreach (StampPlayer tempPlayer in ActivePlayers){
            if (!tempPlayer.IsGhost){
                //create a player in the ui 
                playerSquares[squaresIndex] = Instantiate(staticPlayerUISquare);
                playerSquares[squaresIndex].transform.SetParent(staticPlayerUIPanel.transform, false);
                playerSquares[squaresIndex].transform.GetChild(0).GetComponent<Image>().color = tempPlayer.playerColor;
            }
            squaresIndex++;
        }
    }
}
