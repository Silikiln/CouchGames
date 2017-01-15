using UnityEngine;
using System.Collections.Generic;

public class StampManager : MonoBehaviour {
	public static List<StampPlayer> players = new List<StampPlayer> ();
	public static StampPlayer ghost;

	public Grid gameGrid;

	private static int totalSpaces, stampedSpaces;

	void Start() {
		totalSpaces = gameGrid.Width * gameGrid.Height;
		stampedSpaces = 0;
	}

	public static void PlayerKilled(StampPlayer player) {
		players.Remove (player);
		if (players.Count == 0) {
			Debug.Log ("Ghost wins");
		}
	}

	public static void SpaceStamped() {
		if (++stampedSpaces == totalSpaces) {
			Debug.Log ("Players win");
		}
		Debug.Log (stampedSpaces + "/" + totalSpaces);
	}
}
