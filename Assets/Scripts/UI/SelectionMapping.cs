using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionMapping : MonoBehaviour {
	public SelectionMapping UpTarget, LeftTarget, DownTarget, RightTarget;

	private Selectable parent;

	public void Start() {
		parent = GetComponent<Selectable> ();
	}

	public bool Selectable { get { return parent != null && parent.IsInteractable (); } }
}
