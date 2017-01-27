using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTest : MonoBehaviour {
    void screenToggle(){gameObject.SetActive(!gameObject.activeSelf);}
    public void endGame()
    {
        screenToggle();
    }
}
