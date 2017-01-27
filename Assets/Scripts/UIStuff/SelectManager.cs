using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour {
    public Text gameTitle;
    public GameObject ghostPanel;
    public GameObject hidenPeepsPanel;
    void Start () {
        //get the selected game
        if (LoadSceneOnClick.chosenSceneIndex == 1){
            gameTitle.text = "Hide'n peeps";
            hidenPeepsPanel.SetActive(true);
        }
        else if(LoadSceneOnClick.chosenSceneIndex == 2){
            gameTitle.text = "Ghosp taint";
            ghostPanel.SetActive(true);
        }
        else if(LoadSceneOnClick.chosenSceneIndex == 3){
            gameTitle.text = "not aknockff";
        }
        
        //need to assign text objects to players
	}

    //function that takes the calling players text object and tries to move it to the location text if able
    private void textMover(Text t1, Text t2){
        //check if the location text is empty
        if (t2.text == "") {
            t2.text = t1.text;
            t1.text = "";
        }
    }
}
