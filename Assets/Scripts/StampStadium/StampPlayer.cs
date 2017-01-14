using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StampPlayer : MonoBehaviour {

	public float movementDelay;
    
    public int size = 1;
	public Color playerColor = Color.cyan;
	public Grid gameGrid;
    public bool isGhost = false;
	private float delayTimer = 0;
	private int x, y;
    private float nextWall;
    public float wallDelay;

    // Update is called once per frame
    void Start () {
		MoveTo (Random.Range (gameGrid.Left, gameGrid.Right - size), Random.Range (gameGrid.Bottom, gameGrid.Top - size));
	}

	void Update() {
		if (delayTimer > 0)
			delayTimer -= Time.deltaTime;
		if (delayTimer <= 0) {
			if (Input.GetKey (KeyCode.W))
				MoveTo (x, y - 1);
			else if (Input.GetKey (KeyCode.A))
				MoveTo (x - 1, y);
			else if (Input.GetKey (KeyCode.S))
				MoveTo (x, y + 1);
			else if (Input.GetKey (KeyCode.D))
				MoveTo (x + 1, y);
		}

        //updates specific to boss player
        if (isGhost){
            if (Input.GetKey(KeyCode.X) && Time.time > nextWall){
                nextWall = Time.time + wallDelay;
                BossWall();
            }

        }
	}

	void MoveTo(int x, int y)	{
		StampSpace[] targetSpaces = new StampSpace[size * size];
		GameObject space;
		for (int i = 0; i < size * size; i++) {
			if (!gameGrid.TryGetGridObject(x + i % size, 0, y + i / size, out space))
				return;
			targetSpaces [i] = space.GetComponent<StampSpace>();
		}
		this.x = x;
		this.y = y;
		this.delayTimer = movementDelay;

        if (!isGhost){
            foreach (StampSpace square in targetSpaces)
                square.SetColor(playerColor);
        }
	}
    
    //function that generates a wall of random length in random positions
    void BossWall(){
        //values that represents the total number of walls to create and the max possible size of said walls.
        int wallsToCreate = 4;
        int wallsMaxSize = 6;
        int wallsMinSize = 2;

        //values that represents the 'tendency' for the rng to lean towards a particular end of the range of wallMaxSize
        //the chance to re-roll if the max wall size falls into the half of the bound designated by the value. -1 = lower, 1 = upper, 0 = none 
        float wallSizeRollChance = 0.2f;
        float wallSizeRollValue = 0;

        //value that represents the duration of each wall
        int wallDuration = 10;

        for (int i=0; i<wallsToCreate; i++){
Debug.Log("Creating wall: " + (i + 1) + " of " + wallsToCreate);
            bool wallGenerated = false;
            Random.InitState(System.DateTime.Now.Millisecond);
int whileBreaker1 = 0;
            while (!wallGenerated){
whileBreaker1++;
if(whileBreaker1 == 50) { break; }
                //First, pick a random point in the world within the bounds of the grid
                Vector3 startingPoint = new Vector3(Random.Range(0.0f,gameGrid.Height), 0.0f, Random.Range(0.0f, gameGrid.Width));
Debug.Log("Trying StartingPoint: " + startingPoint + " for wall Number: " + (i + 1));
                //check if that point is already a wall
                GameObject tempPointCheck = null;
                if (gameGrid.TryGetGridObject(startingPoint, out tempPointCheck) && !tempPointCheck.GetComponent<StampSpace>().isWall){
Debug.Log("StartingPoint: " + startingPoint + " for wall Number: " + (i + 1) + " VALID");
                    //we have a valid point that is not a wall.  Pick a random direction to wall off
                    int chosenDirection = 0;
                    List<int> directionsChecked = new List<int>();
int whileBreaker2 = 0;
                    while (chosenDirection == 0){
whileBreaker2++;
if (whileBreaker2 == 50) { break; }
                        int tempDirection = Random.Range(1, 4);
Debug.Log("Testing Direction: " + tempDirection + " For StartingPoint: " + startingPoint + " for wall Number: " + (i + 1));
                        if (!directionsChecked.Contains(tempDirection)){
Debug.Log("Direction: " + tempDirection + " For StartingPoint: " + startingPoint + " for wall Number: " + (i + 1) + " VALID");
                            directionsChecked.Add(tempDirection);
                            //check that we can actually build a minimum length wall in that direction
Debug.Log("Testing min wall in Direction: " + tempDirection + " For StartingPoint: " + startingPoint + " for wall Number: " + (i + 1));
                            for (int j = 0; j < wallsMinSize; j++){
                                //temporarily select a gridspace in the chose direction
                                Vector3 tempWallPoint;
                                if (tempDirection == 1)
                                    tempWallPoint = new Vector3(startingPoint.x, startingPoint.y, startingPoint.z + j); //up
                                else if (tempDirection == 2)
                                    tempWallPoint = new Vector3(startingPoint.x + j, startingPoint.y, startingPoint.z); //right
                                else if (tempDirection == 3)
                                    tempWallPoint = new Vector3(startingPoint.x, startingPoint.y, startingPoint.z - j); //down
                                else
                                    tempWallPoint = new Vector3(startingPoint.x - j, startingPoint.y, startingPoint.z); //left

                                //ensure that we can build a wall in the temporary grid space, if not try a different direction
                                GameObject tempMinWall = null;
                                if (!gameGrid.TryGetGridObject(tempWallPoint, out tempMinWall))
                                    break;
Debug.Log("Testing min wall: " + (j+1) + " of " + wallsMinSize +  " in Direction: " + tempDirection + " for wall Number: " + (i + 1) + " VALID");
                            }
                        }
                        else{
                            //probably a better way to do this, but this ensures directions that are not valid will not be marked true.
                            //for the time being, don't make this an else if or stuff breaks
                            if (directionsChecked.Count == 4) { break; }
                        }

                        //if we made it this far, then we know the current direction is valid
                        chosenDirection = tempDirection;
                    }

                    if (chosenDirection != 0)
                    {
Debug.Log("Wall pass checks, building wall: " + (i + 1) + " of " + wallsToCreate);
                        //generate the wallSize
                        int wallSize = wallsMaxSize / 2;
                        bool pickAgain = false;
int whileBreaker3 = 0;
                        do
                        {
whileBreaker3++;
if (whileBreaker3 == 50) { break; }
Debug.Log("Generating wall size...");
                            pickAgain = false;
                            wallSize = Random.Range(wallsMinSize, wallsMaxSize);
                            //if the value is in the middle always use it
                            if (wallSizeRollValue == 1 && wallSize > (wallsMaxSize / 2) && Random.value <= wallSizeRollChance)
                                pickAgain = true;
                            if (wallSizeRollValue == -1 && wallSize < (wallsMaxSize / 2) && Random.value <= wallSizeRollChance)
                                pickAgain = true;
                        } while (pickAgain);
Debug.Log("Final Wall Size: " + wallSize);
                        //build a list of all the spaces of the wall in the chosen direction
                        List<StampSpace> wallSpaces = new List<StampSpace>();
                        for (int w = 0; w < wallSize; i++)
                        {
                            //temporarily select a gridspace in the chose direction
                            Vector3 tempWallPoint;
                            if (chosenDirection == 1)
                                tempWallPoint = new Vector3(startingPoint.x, startingPoint.y, startingPoint.z + w); //up
                            else if (chosenDirection == 2)
                                tempWallPoint = new Vector3(startingPoint.x + w, startingPoint.y, startingPoint.z); //right
                            else if (chosenDirection == 3)
                                tempWallPoint = new Vector3(startingPoint.x, startingPoint.y, startingPoint.z - w); //down
                            else
                                tempWallPoint = new Vector3(startingPoint.x - w, startingPoint.y, startingPoint.z); //left
Debug.Log("Getting Final wall point: " + tempWallPoint + " Of Wall: " + (i+1));
                            //ensure that we can build a wall in the temporary grid space, if not try a different direction
                            GameObject tempMinWall = null;
                            if (gameGrid.TryGetGridObject(tempWallPoint, out tempMinWall))
                                wallSpaces.Add(tempMinWall.GetComponent<StampSpace>());
Debug.Log("Wall point: " + tempWallPoint + " Of Wall: " + (i + 1) + "ADDED to wallSpaces");
                        }

                        //loop over our list of wall spaces and turn each into a wall
                        foreach (StampSpace square in wallSpaces)
                            square.InvokeWall(wallDuration);
                        
                        //wall has been generated    
                        wallGenerated = true;
Debug.Log("Wall: " + (i + 1) + " of " + wallsToCreate + " BUILT");
                    }
                }
            }
        }
    }
}
