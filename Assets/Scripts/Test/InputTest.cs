using UnityEngine;
using System.Collections;

public class InputTest : MonoBehaviour {
	private static InputTest nextFree = null;

	public int order = 0;
	public GamepadInput gamepad = null;

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
				gamepad = GamepadInput.AllGamepads.Find (controller => !controller.InUse && controller.Start);
				if (gamepad != null && gamepad.Start) {
					nextFree = null;
					gamepad.Lock ();
					spriteRender.enabled = true;
				}
			}
		}

		if (gamepad != null) {
			Color targetColor = Color.white;
			if (gamepad.A)
				targetColor = Color.green;
			else if (gamepad.X)
				targetColor = Color.blue;
			else if (gamepad.Y)
				targetColor = Color.yellow;
			else if (gamepad.B)
				targetColor = Color.red;
			
			spriteRender.color = targetColor;
			transform.position = startPosition + new Vector3 (gamepad.Left_X, -gamepad.Left_Y);

			if (gamepad.Back) {
				gamepad.Free ();
				spriteRender.enabled = false;
				gamepad = null;
			}
		}
	}
}
