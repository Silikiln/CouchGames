using UnityEngine;
using System.Collections;

public class ControllerCount : MonoBehaviour {
	private TextMesh text;

	void Start() {
		text = GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {
		string newText = "";
		foreach (string gamepadName in Input.GetJoystickNames())
			newText += gamepadName + "\n";
		text.text = newText;
	}
}
