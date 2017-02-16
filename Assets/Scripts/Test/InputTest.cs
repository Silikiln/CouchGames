using UnityEngine;
using System.Collections;

public class InputTest : MonoBehaviour {
	private static InputTest nextFree = null;

	public int order = 0;
	public Gamepad gamepad = null;

	private SpriteRenderer spriteRender;
	private Vector3 startPosition;

	void Start() {
		if (nextFree == null || nextFree.order > order)
			nextFree = this;
		spriteRender = GetComponent<SpriteRenderer> ();
		spriteRender.enabled = false;
		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (gamepad == null) {
			if (nextFree == null || nextFree.order > order)
				nextFree = this;
			if (this == nextFree) {
				gamepad = Gamepad.AllGamepads.Find (controller => !controller.InUse && controller.GetInputActive(Gamepad.InputCode.Start));
				if (gamepad != null) {
					nextFree = null;
					gamepad.Lock ();
					spriteRender.enabled = true;
				}
			}
		}

		if (gamepad != null) {
			Color targetColor = Color.white;
			if (gamepad.GetInputActive(Gamepad.InputCode.A))
				targetColor = Color.green;
			else if (gamepad.GetInputActive(Gamepad.InputCode.X))
				targetColor = Color.blue;
			else if (gamepad.GetInputActive(Gamepad.InputCode.Y))
				targetColor = Color.yellow;
			else if (gamepad.GetInputActive(Gamepad.InputCode.B))
				targetColor = Color.red;
			
			spriteRender.color = targetColor;
			transform.position = startPosition + new Vector3 (Input.GetAxis(gamepad.LeftHorizontal_Axis), -Input.GetAxis(gamepad.LeftVertical_Axis));

			if (gamepad.GetInputActive(Gamepad.InputCode.Back)) {
				gamepad.Free ();
				spriteRender.enabled = false;
				gamepad = null;
			}
		}
	}
}
