using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StampPlayer : MonoBehaviour {

	public Color playerColor = Color.cyan;
    
	protected StampMovement movementHandler;
	protected int X { get { return movementHandler.X; } }
	protected int Y { get { return movementHandler.Y; } }
	protected int Size { get { return movementHandler.size; } }
	protected StampSpace[] CurrentSpaces { get { return movementHandler.CurrentSpaces; } }
	protected Grid GameGrid { get { return movementHandler.gameGrid; } }

	public GamepadInput Gamepad { get; set; }

	public virtual bool IsGhost { get { return false; } }

	public virtual bool Highlight { get { return Gamepad.Y || Input.GetKey (KeyCode.N); } }

	protected virtual void Start() {
		movementHandler = gameObject.GetComponent<StampMovement> ();
	}

	void Update() {
		foreach (StampSpace space in CurrentSpaces)
			space.UpdateColor ();
	}

	public void PlayerDeath(){
        Destroy(gameObject);
    }

	void OnDestroy() {
		foreach (StampSpace space in CurrentSpaces)
			space.SetOccupyingPlayer (null);
		StampManager.PlayerKilled (this);
	}
}
