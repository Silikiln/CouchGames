using UnityEngine;
using System.Collections;

public class StampPlayer : MonoBehaviour {

	public float movementDelay;
	public int size = 1;
	public Color playerColor = Color.cyan;
	public Grid gameGrid;

	private float delayTimer = 0;
	private int x, y;

	// Update is called once per frame
	void Start () {
		MoveTo (Random.Range (gameGrid.Left, gameGrid.Right - size), Random.Range (gameGrid.Bottom, gameGrid.Top - size));
	}

	void Update() {
		if (delayTimer > 0)
			delayTimer -= Time.deltaTime;
		if (delayTimer <= 0) {
			if (Input.GetKey (KeyCode.W))
				MoveTo (x, y - 1);
			else if (Input.GetKey (KeyCode.A))
				MoveTo (x - 1, y);
			else if (Input.GetKey (KeyCode.S))
				MoveTo (x, y + 1);
			else if (Input.GetKey (KeyCode.D))
				MoveTo (x + 1, y);
		}
	}

	void MoveTo(int x, int y)	{
		StampSpace[] targetSpaces = new StampSpace[size * size];
		GameObject space;
		for (int i = 0; i < size * size; i++) {
			if (!gameGrid.TryGetGridObject(x + i % size, 0, y + i / size, out space))
				return;
			targetSpaces [i] = space.GetComponent<StampSpace>();
		}
		this.x = x;
		this.y = y;
		this.delayTimer = movementDelay;
		foreach (StampSpace square in targetSpaces)
			square.SetColor (playerColor);
	}
}
