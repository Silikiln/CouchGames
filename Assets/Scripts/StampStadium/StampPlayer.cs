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
		for (int i = 0; i < size * size; i++) {
            GameObject space;
            if (!gameGrid.TryGetGridObject(x + i % size, 0, y + i / size, out space) || (space.GetComponent<StampSpace>().isWall && !isGhost))
				return;
			targetSpaces [i] = space.GetComponent<StampSpace>();
		}
		this.x = x;
		this.y = y;
		this.delayTimer = movementDelay;

        foreach (StampSpace square in targetSpaces)
            square.SetColor(playerColor);

	}
    
    //function that generates a wall of random length in random positions
    void BossWall(){
        //values that represents the total number of walls to create and the max possible size of said walls.
        int wallsToCreate = 4;
        int wallsMaxSize = 7;
        int wallsMinSize = 2;

        //values that represents the 'tendency' for the rng to lean towards a particular end of the range of wallMaxSize
        //the chance to re-roll if the max wall size falls into the half of the bound designated by the value. -1 = lower, 1 = upper, 0 = none 
        float wallSizeRollChance = 0.2f;
        float wallSizeRollValue = 0;

        //value that represents the duration of each wall
        int wallDuration = 10;
        Random.InitState(System.DateTime.Now.Millisecond);
        for (int i = 0; i < wallsToCreate; i++)
        {
            Debug.Log("Creating wall: " + (i + 1) + " of " + wallsToCreate);
            bool wallGenerated = false;
            while (!wallGenerated)
            {
                Vector3 startingPoint = new Vector3(Random.Range((gameGrid.Height / -2), (gameGrid.Height/2)), 0, Random.Range((gameGrid.Width / -2), (gameGrid.Width / 2))); //is there a better way to check points on grid?
                Debug.Log("Trying StartingPoint: " + startingPoint + " for wall Number: " + (i + 1));
                //check if that point is already a wall
                GameObject tempPointCheck = null;
                if (gameGrid.TryGetGridObject(startingPoint, out tempPointCheck) && !tempPointCheck.GetComponent<StampSpace>().isWall)
                {
                    Debug.Log("Valid: " + startingPoint + " for wall Number: " + (i + 1));
                    int chosenDirection = 0;
                    List<int> directionsChecked = new List<int>();
                    while (chosenDirection == 0)
                    {
                        bool failedMinWallTest = false;
                        int tempDirection = Random.Range(1, 4);
                        if (!directionsChecked.Contains(tempDirection))
                        {
                            Debug.Log("Direction: " + tempDirection + " For StartingPoint: " + startingPoint + " for wall Number: " + (i + 1) + " VALID");
                            directionsChecked.Add(tempDirection);
                            for (int j = 0; j < wallsMinSize; j++)
                            {
                                if (!failedMinWallTest)
                                {
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
                                    if (!gameGrid.TryGetGridObject(tempWallPoint, out tempMinWall)) { 
                                        failedMinWallTest = true;
                                        Debug.Log("Testing min wall: " + (j + 1) + " of " + wallsMinSize + " in Direction: " + tempDirection + " FAILED");
                                    } else
                                        Debug.Log("Testing min wall: " + (j + 1) + " of " + wallsMinSize + " in Direction: " + tempDirection + " PASSED");
                                }
                            }
                        }
                        else {
                            //probably a better way to do this, but this ensures directions that are not valid will not be marked true.
                            //for the time being, don't make this an else if or stuff breaks
                            if (directionsChecked.Count == 4) { chosenDirection = -1; }
                        }

                        if (!failedMinWallTest)
                        {
                            //if we made it this far, then we know the current direction is valid
                            chosenDirection = tempDirection;
                            Debug.Log("Chosen Direcetion: " + chosenDirection);
                        }
                        else
                        {
                            Debug.Log("We failed with Direcetion: " + chosenDirection);
                        }

                    }

                    if(chosenDirection != 0 && chosenDirection != -1)
                    {
                        //generate the wallSize
                        int wallSize = wallsMaxSize / 2;
                        bool pickAgain = false;
                        do
                        {
                            Debug.Log("Generating wall size...");
                            pickAgain = false;
                            wallSize = Random.Range(wallsMinSize, wallsMaxSize);
                            //if the value is in the middle always use it
                            if (wallSizeRollValue == 1 && wallSize > (wallsMaxSize / 2) && Random.value <= wallSizeRollChance)
                                pickAgain = true;
                            if (wallSizeRollValue == -1 && wallSize < (wallsMaxSize / 2) && Random.value <= wallSizeRollChance)
                                pickAgain = true;
                        } while (pickAgain);

                        Debug.Log("Wall Size to try: " + wallSize);

                        List<StampSpace> wallSpaces = new List<StampSpace>();
                        for (int w = 0; w < wallSize; w++)
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
                            Debug.Log("Getting Final wall point: " + tempWallPoint + " Of Wall: " + (i + 1));
                            //ensure that we can build a wall in the temporary grid space, if not try a different direction
                            GameObject tempMinWall = null;
                            if (gameGrid.TryGetGridObject(tempWallPoint, out tempMinWall) && !tempMinWall.GetComponent<StampSpace>().isWall)
                            {
                                wallSpaces.Add(tempMinWall.GetComponent<StampSpace>());
                                Debug.Log("Wall point: " + tempWallPoint + " Of Wall: " + (i + 1) + "ADDED");
                            }
                            else
                            {
                                Debug.Log("Wall point: " + tempWallPoint + " Of Wall: " + (i + 1) + "NOT ADDED");
                            }

                        }

                        //loop over our list of wall spaces and turn each into a wall
                        foreach (StampSpace square in wallSpaces)
                            square.InvokeWall(wallDuration);

                        //wall has been generated    
                        wallGenerated = true;
                        Debug.Log("Wall: " + (i + 1) + " of " + wallsToCreate + " BUILT Final Size: " + wallSpaces.Count);
                    }
                }
            }
        }
    }
}
