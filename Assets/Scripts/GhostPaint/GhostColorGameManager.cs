using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostColorGameManager : MonoBehaviour {
    public static int waveNum = 1;
    public GameObject ghostPrefab;

    public GameObject[] players;
    public static GameObject[] playersStatic;

    // Use this for initialization
    void Start () {
		for(int i = 0; i < 10; i++) {
            SpawnGhost();
        }
        playersStatic = players;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private Vector3 SpawnPosition() {return new Vector3(Random.Range(-9f, 9f), Random.Range(-4f, 4f), 0f);}
    void SpawnGhost() {
        bool validSpawnPoint = false;
        Vector3 ghostSpawnLocation = Vector3.zero;
        while(!validSpawnPoint) {
            ghostSpawnLocation = SpawnPosition();
            int validDistanceChecks = 0;
            foreach(GameObject player in players) {
                //check if the point is atleast a certain distance away
                float distFromPlayer = Vector3.Distance(player.transform.position, ghostSpawnLocation);
                if(System.Math.Abs(distFromPlayer) > 2) {validDistanceChecks++;}
            }
            if(validDistanceChecks == 3) {validSpawnPoint = true;}
        }
        Instantiate(ghostPrefab, ghostSpawnLocation, Quaternion.identity).name = "Ghost" + gIndex++;
    }

    private int gIndex = 0;
}
