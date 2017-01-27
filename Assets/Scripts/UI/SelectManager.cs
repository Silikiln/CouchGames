using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GameInfo = GameTeams.GameInfo;

public class SelectManager : MonoBehaviour {
	public static int chosenSceneIndex = 0;

	public GameInfo[] GameInformation;

    public Text GameTitle;

	public TeamPanel LeftPanel, CenterPanel, RightPanel;

	private GamepadInput[] playersJoined;

    void Start () {
		GameTeams.GamepadTeam.Clear ();
		GameInfo targetGame = GameInformation [chosenSceneIndex];
		playersJoined = new GamepadInput[targetGame.MaxPlayers];

		GameTitle.text = targetGame.Title;
	}

	void Update() {
		for (int i = 0; i < playersJoined.Length; i++)
			if (playersJoined[i] != null && playersJoined [i].Back) {
				playersJoined [i].Free ();
				playersJoined [i] = null;
				CenterPanel.Leave ("P" + (i + 1));
			}

		int index;
		if (!TryGetFreeIndex (out index))
			return;
		
		GamepadInput gamepad = GamepadInput.AllGamepads.Find (controller => !controller.InUse && controller.Start);
		if (gamepad == null)
			return;
		
		gamepad.Lock ();
		playersJoined [index] = gamepad;
		CenterPanel.Join("P" + (index + 1));
	}

	private bool TryGetFreeIndex(out int index) {
		for (index = 0; index < playersJoined.Length; index++)
			if (playersJoined [index] == null) {
				return true;
			}

		return false;
	}
}
