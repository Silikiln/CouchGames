using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {
    private Color ghostColor = Color.black;
    private bool canBeHit = false;
    public float fadeInDuration;
    private SpriteRenderer ghostRenderer;
    private GhostColor gcHandler;
    void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        ghostRenderer = transform.GetComponent<SpriteRenderer>();
        //init the gcHandler and set the ghost color
        gcHandler = new GhostColor(randomColor());
        ghostColor = gcHandler.toColor();
        ghostRenderer.color = ghostColor;

        //kick off the fade in coroutine
        StartCoroutine(GhostFadeIn(fadeInDuration));
    }

    public void CheckColor(Color beamColor){
        if(beamColor == ghostColor && canBeHit) { Destroy(gameObject); }
    }

    //a function to create a random color for the ghost to use, toDo; add a way to scale based on level
    public string randomColor() {
        string randomColor = "";
        int[] bry = new int[3];

        for(int i = 0; i < bry.Length; i++) {
            bry[i] = Random.Range(0, 2);
            if(bry[i] != 0) {
                for(int c =0; c < Random.Range(1, 3); c++) {
                    if(i == 0) { randomColor += "B"; } 
                    else if(i == 1) {randomColor += "R";} 
                    else { randomColor += "Y"; }
                }
            }
        }
        return randomColor;
    }

    void OnDestroy() {
        //call gameManager function to signal that a ghost was destroyed.
    }

    void Update() {
        //determine ghost movement
    }

    IEnumerator GhostFadeIn(float fadeInDuration) {
        //set the alpha to 0
        ghostColor.a = 0;
        ghostRenderer.color = ghostColor;
        float progress = 0.0f;
        while(progress < 1) {
            //change the alpha to fade in the ghost
            progress += Time.deltaTime / fadeInDuration;
            ghostColor.a = progress;
            ghostRenderer.color = ghostColor;
            yield return null;
        }
        //enable the ghost to be hitable
        canBeHit = true;
    }
}
