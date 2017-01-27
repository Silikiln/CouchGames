using UnityEngine;
using System.Collections.Generic;

public class StampManager : MonoBehaviour {
	public static StampManager Instance { get; private set; }
	public static List<StampPlayer> ActivePlayers = new List<StampPlayer> ();
	private static int totalSpaces, stampedSpaces;


	public Grid gameGrid;
	public EndScreen endScreen;
	public StampPlayer ghost;
	public StampPlayer[] players;



    //need to add in a call to endScreen.GetComponent(EndScreen).UpdateEnd(player names and such)

	void Start() {
		Instance = this;

		ghost.Gamepad = GameTeams.TeamMembers (0) [0];
		GamepadInput[] playerGamepads = GameTeams.TeamMembers (1);
		for (int i = 0; i < playerGamepads.Length; i++) {
			players [i].gameObject.SetActive (true);
			players [i].Gamepad = playerGamepads [i];
		}

		totalSpaces = gameGrid.Width * gameGrid.Height;
		stampedSpaces = 0;
	}

	public static void PlayerKilled(StampPlayer player) {
		ActivePlayers.Remove (player);
		Debug.Log (ActivePlayers.Count);
		if (ActivePlayers.Count == 0) {
			Instance.endScreen.UpdateEnd ("Ghost", new string[0]);
		}
	}

	public static void SpaceStamped() {
		if (++stampedSpaces == totalSpaces) {
			Instance.endScreen.UpdateEnd ("Players", new string[0]);
		}
	}
}
