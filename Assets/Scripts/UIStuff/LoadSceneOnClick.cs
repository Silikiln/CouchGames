using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneOnClick : MonoBehaviour {
    public static int chosenSceneIndex = 0;
    public void LoadByIndex(int sceneIndex){
        chosenSceneIndex = sceneIndex;
        SceneManager.LoadScene("PlayerSelect");
    }
    public void LoadOtherByIndex(int sceneIndex){
        chosenSceneIndex = 0;
        SceneManager.LoadScene(sceneIndex);
    }
}
