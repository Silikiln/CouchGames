using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour {
    void screenToggle() { gameObject.SetActive(!gameObject.activeSelf); }
    public void UpdateEnd()
    {
        screenToggle();
    }
}
