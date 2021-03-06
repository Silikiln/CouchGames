﻿using UnityEngine;
using System.Collections;

public class StampSpace : MonoBehaviour {
	private int ghostCount = 0;

	public bool IsWall { get; private set; }

	public bool Occupied { get; private set; }
	public bool Ghosted { get { return ghostCount > 0; } }

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

	public void UpdateColor() {
		Color toColor; 
		if (IsWall)
			toColor = Color.black;
		else if (Occupied && Owner != null && Owner.Highlight)
			toColor = Color.gray;
		else
			toColor = defaultColor;
		
		if (Ghosted && !Occupied)
			toColor.a = .3f;
		
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
		ghostCount = Mathf.Clamp(ghostCount + (ghostOn ? 1 : -1), 0, 4);
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

    public void InvokeSwingColor(float colorDuration, Color colorToUse){
        StartCoroutine(GhostSwingColor(colorDuration, colorToUse));
    }

    private IEnumerator GhostSwingColor(float colorDuration, Color colorToUse)
    {
        Color startColor = defaultColor;
        defaultColor = colorToUse;
        UpdateColor();
        yield return new WaitForSeconds(colorDuration);
        defaultColor = startColor;
        UpdateColor();
    }
}
