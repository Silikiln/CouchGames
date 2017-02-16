using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour {
	
    public void LoadGame(int gameIndex) {
		TeamSelection.chosenGameIndex = gameIndex;
        SceneManager.LoadScene("TeamSelect");
    }

    public void LoadScene(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }

	public void ExitApplication() {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
