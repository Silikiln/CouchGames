using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamPanel : MonoBehaviour {
	public Text TeamTitle;
	public Text[] Players;

	// Use this for initialization
	void Start () {
		foreach (Text playerText in Players)
			playerText.text = "";
	}

	public bool Join(string name) {
		foreach (Text playerText in Players)
			if (playerText.text.Length == 0) {
				playerText.text = name;
				return true;
			}
		return false;
	}

	public void Leave(string name) {
		foreach (Text playerText in Players)
			if (playerText.text == name) {
				playerText.text = "";
				return;
			}
	}
}
