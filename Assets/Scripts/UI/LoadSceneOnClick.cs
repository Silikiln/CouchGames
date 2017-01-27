using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneOnClick : MonoBehaviour {
    public void LoadGame(int gameIndex){
		SelectManager.chosenGameIndex = gameIndex;
        SceneManager.LoadScene("PlayerSelect");
    }
    public void LoadScene(int sceneIndex){
        SceneManager.LoadScene(sceneIndex);
    }
}
