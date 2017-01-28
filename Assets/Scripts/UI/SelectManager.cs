using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using GameInfo = GameTeams.GameInfo;

public class SelectManager : MonoBehaviour {
	public static int chosenGameIndex = -1;

	public GameInfo[] GameInformation;

    public Text GameTitle;

	public TeamPanel LeftPanel, CenterPanel, RightPanel;

	private Dictionary<int, TeamPanel> usedPanels = new Dictionary<int, TeamPanel> ();
	private GamepadInput[] playersJoined;
	private int startTeam;
	private GameInfo targetGame;

    void Start () {
		if (chosenGameIndex == -1)
			chosenGameIndex = 0;

		foreach (GamepadInput gamepad in GamepadInput.AllGamepads)
			gamepad.Free ();
		GameTeams.GamepadTeam.Clear ();
		targetGame = GameInformation [chosenGameIndex];
		GameTitle.text = targetGame.Title;

		playersJoined = new GamepadInput[targetGame.MaxPlayers];
		if (targetGame.TeamCount == 1) {
			startTeam = 0;
			usedPanels.Add (0, CenterPanel);
		}
		else {
			startTeam = -1;
			usedPanels.Add (-1, CenterPanel);
			CenterPanel.gameObject.SetActive (true);
			if (targetGame.TeamCount == 2) {
				usedPanels.Add (0, LeftPanel);
				usedPanels.Add (1, RightPanel);
			}
		}

		for (int i = 0; i < targetGame.TeamCount; i++) {
			usedPanels [i].gameObject.SetActive (true);
			usedPanels [i].TeamTitle.text = targetGame.Teams [i].TeamName;
		}
	}

	void Update() {
		for (int i = 0; i < playersJoined.Length; i++)
			if (playersJoined[i] != null) {
				if (playersJoined [i].Back) {
					playersJoined [i].Free ();
					usedPanels [GameTeams.GamepadTeam [playersJoined [i]]].Leave ("P" + (i + 1));
					GameTeams.GamepadTeam.Remove (playersJoined [i]);
					playersJoined [i] = null;
				} else if (playersJoined [i].Start && ValidTeams) {
					SceneManager.LoadScene ("stampTest");
				} else if (playersJoined [i].Left_X < 0) {
					MoveTeam (i, 0);
				} else if (playersJoined [i].Left_X > 0) {
					MoveTeam (i, 1);
				}
			}

		int index;
		if (!TryGetFreeIndex (out index))
			return;
		
		GamepadInput gamepad = GamepadInput.AllGamepads.Find (controller => !controller.InUse && controller.Start);
		if (gamepad == null)
			return;
		
		gamepad.Lock ();
		playersJoined [index] = gamepad;
		GameTeams.GamepadTeam.Add (gamepad, startTeam);
		CenterPanel.Join("P" + (index + 1));
	}

	private bool ValidTeams {
		get {
			if (GameTeams.GamepadTeam.Values.Contains (-1))
				return false;

			int[] teamSizes = GameTeams.TeamSizes;
			for (int i = 0; i < targetGame.TeamCount; i++)
				if (!targetGame.Teams [i].Valid (teamSizes [i]))
					return false;
			return true;
		}
	}

	private void MoveTeam(int playerIndex, int targetTeam) {
		GamepadInput gamepad = playersJoined [playerIndex];

		int currentTeam = GameTeams.GamepadTeam [gamepad];
		if (currentTeam == targetTeam)
			return;
		
		string playerName = "P" + (playerIndex + 1);
		
		usedPanels [currentTeam].Leave (playerName);
		usedPanels [targetTeam].Join (playerName);

		GameTeams.GamepadTeam [gamepad] = targetTeam;
	}

	private bool TryGetFreeIndex(out int index) {
		for (index = 0; index < playersJoined.Length; index++)
			if (playersJoined [index] == null) {
				return true;
			}

		return false;
	}
}
