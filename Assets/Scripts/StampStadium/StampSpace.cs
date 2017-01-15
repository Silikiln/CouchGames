using UnityEngine;
using System.Collections;

public class StampSpace : MonoBehaviour {
	public bool IsWall { get; private set; }

	public bool Occupied { get; private set; }
	public bool Ghosted { get; private set; }

	public Color defaultColor = Color.white;

	private SpriteRenderer _spriteRenderer;
	private SpriteRenderer spriteRenderer {
		get {
			if (_spriteRenderer == null)
				_spriteRenderer = GetComponent<SpriteRenderer> ();
			return _spriteRenderer;
		}
	}

	public StampPlayer Owner { get; private set; }

	private void UpdateColor() {
		Color toColor = IsWall ? Color.black : defaultColor;
		if (Ghosted && !Occupied)
			toColor.a = .4f;
		spriteRenderer.color = toColor;
	}

	public void SetOccupyingPlayer(StampPlayer player) {
		Occupied = player != null;
		if (Occupied && Owner == null) {
			Owner = player;
			defaultColor = player.playerColor;
			StampManager.SpaceStamped ();
		}
		UpdateColor ();
	}

	public void SetGhost(bool ghostOn) {
		Ghosted = ghostOn;
		UpdateColor ();
	}

    public void InvokeWall(int wallDuration){
        StartCoroutine(ActiveWall(wallDuration));
    }

    IEnumerator ActiveWall(float wallDuration)
    {
        IsWall = true;
		UpdateColor ();
		yield return new WaitForSeconds (wallDuration);
        IsWall = false;
		UpdateColor ();
    }
}
