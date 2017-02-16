using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampMovement : MonoBehaviour {
	
	public float movementDelay;
	public int size = 1;
	public Grid gameGrid;

	private float movementTimer;
	public StampPlayer playerHandler;

	public int X { get; private set; }
	public int Y { get; private set; }
	public StampSpace[] CurrentSpaces { get; private set; }

	private Gamepad Gamepad { get { return playerHandler.Gamepad; } }

	// Use this for initialization
	void Start () {
		playerHandler = gameObject.GetComponent<StampPlayer> ();
		while(!MoveTo (Random.Range (gameGrid.Left, gameGrid.Right - size), Random.Range (gameGrid.Bottom, gameGrid.Top - size)));
	}
	
	// Update is called once per frame
	void Update () {
		if ((movementTimer > 0 ? movementTimer -= Time.deltaTime : movementTimer) <= 0) {
			if (Gamepad == null);
			else if (Gamepad.GetInputActive(Gamepad.InputCode.LeftStickUp)){
				MoveTo(X, Y - 1);
			}else if (Gamepad.GetInputActive(Gamepad.InputCode.LeftStickLeft)){
				MoveTo(X - 1, Y);
			}else if (Gamepad.GetInputActive(Gamepad.InputCode.LeftStickDown)){
				MoveTo(X, Y + 1);
			}else if (Gamepad.GetInputActive(Gamepad.InputCode.LeftStickRight)){
				MoveTo(X + 1, Y);
			}
		}
	}

	public bool MoveTo(int x, int y)	{
		StampSpace[] targetSpaces = new StampSpace[size * size];
		for (int i = 0; i < size * size; i++) {
			GameObject space;
			if (!gameGrid.TryGetGridObject (x + i % size, 0, y + i / size, out space))
				return false;
			targetSpaces [i] = space.GetComponent<StampSpace> ();
			if (!playerHandler.IsGhost && (targetSpaces [i].IsWall || (targetSpaces [i].Owner != null && targetSpaces [i].Owner != this)))
				return false;
		}
		this.X = x;
		this.Y = y;
		this.movementTimer = movementDelay;

		//block to remove the currently occupying player
		if (CurrentSpaces != null) {
			foreach (StampSpace square in CurrentSpaces)
				if (playerHandler.IsGhost)
					square.SetGhost (false);
				else
					square.SetOccupyingPlayer (null);
		}

		//update currentSpaces to be the spaces we are moving to
		CurrentSpaces = targetSpaces;

		//set the player on the square, and set the owner/color
		foreach (StampSpace square in CurrentSpaces){
			if (playerHandler.IsGhost)
				square.SetGhost(true);
			else {
				square.SetOccupyingPlayer(playerHandler);
			}
		}
		return true;
	}
}
