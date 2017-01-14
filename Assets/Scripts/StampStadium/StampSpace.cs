using UnityEngine;
using System.Collections;

public class StampSpace : MonoBehaviour {
	private SpriteRenderer spriteRenderer;
    public bool isWall = false;

	void Start() {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	public void SetColor(Color color) {
		if (spriteRenderer == null)
			spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		spriteRenderer.color = color;
	}

	public void SetGhost(bool ghostOn) {
		Color currentColor = spriteRenderer.color;
		currentColor.a = ghostOn ? .4f : 1;
		spriteRenderer.color = currentColor;
	}

    public void InvokeWall(int wallDuration){
        StartCoroutine(ActiveWall(wallDuration));
    }

    IEnumerator ActiveWall(float wallDuration)
    {
        isWall = true;
        Color startColor = (spriteRenderer != null) ? spriteRenderer.color:Color.black;
        SetColor(Color.black);
		yield return new WaitForSeconds (wallDuration);
        if(startColor != Color.black)
            SetColor(startColor);
        else
            SetColor(Color.white);

        isWall = false;
    }
}
