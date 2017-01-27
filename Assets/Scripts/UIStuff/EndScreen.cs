using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour {
    public Text[] losers = new Text[3];
    public Text[] winners = new Text[3];

    void screenToggle() { gameObject.SetActive(!gameObject.activeSelf); }
    public void UpdateEnd(string playerWinner, string[] playerLosers)
    {
        for(int i=0; i < winners.Length; i++){ winners[i].text = (i == 1) ? playerWinner : "";}
        for (int i = 0; i < playerLosers.Length; i++) { losers[i].text = playerLosers[i]; }
        screenToggle();
    }
    public void UpdateEnd(string[] playerWinners, string playerLoser)
    {
        for (int i = 0; i < losers.Length; i++) { losers[i].text = (i == 1) ? playerLoser : ""; }
        for (int i = 0; i < playerWinners.Length; i++) { winners[i].text = playerWinners[i]; }
        screenToggle();
    }


}
