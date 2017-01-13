using UnityEngine;
using System.Collections;

public class BallBuster : MonoBehaviour {
	public bool busting = false;

	private SpriteRenderer spriteRenderer;

	void Start() {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	void FixedUpdate() {
		busting = Input.GetKey (KeyCode.Space);
		spriteRenderer.color = !busting ? Color.white : Color.green;
	}
}
