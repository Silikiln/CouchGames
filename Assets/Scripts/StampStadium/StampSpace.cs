using UnityEngine;
using System.Collections;

public class StampSpace : MonoBehaviour {
	private SpriteRenderer spriteRenderer;
    public bool isWall = false;

	public void SetColor(Color color) {
		if (spriteRenderer == null)
			spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		spriteRenderer.color = color;
	}

    public void InvokeWall(int wallDuration){
        StartCoroutine(ActiveWall(wallDuration));
    }

    IEnumerator ActiveWall(int wallDuration)
    {
        isWall = true;
        float startTime = Time.time;
        Color startColor = (spriteRenderer != null) ? spriteRenderer.color:Color.black;
        SetColor(Color.black);
        while (Time.time - startTime < wallDuration){
            yield return new WaitForFixedUpdate();
        }
        if(startColor != Color.black)
            SetColor(startColor);
        else
            SetColor(Color.white);


        isWall = false;
    }
}
