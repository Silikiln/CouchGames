using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneOnClick : MonoBehaviour {
    public void LoadByIndex(int sceneIndex){
		SelectManager.chosenSceneIndex = sceneIndex;
        SceneManager.LoadScene("PlayerSelect");
    }
    public void LoadOtherByIndex(int sceneIndex){
		SelectManager.chosenSceneIndex = sceneIndex;
        SceneManager.LoadScene(sceneIndex);
    }
}
