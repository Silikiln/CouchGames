using UnityEngine;
using System.Collections;

public class StampSpace : MonoBehaviour {
	private SpriteRenderer spriteRenderer;
	
	public void SetColor(Color color) {
		if (spriteRenderer == null)
			spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		spriteRenderer.color = color;
	}
}
