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

	private StampSpace[] currentSpaces;

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
			if (!gameGrid.TryGetGridObject(x + i % size, 0, y + i / size, out space))
				return;
			targetSpaces [i] = space.GetComponent<StampSpace>();
			if (targetSpaces [i].isWall && !isGhost)
				return;
		}
		this.x = x;
		this.y = y;
		this.delayTimer = movementDelay;

        //block to remove the currently occupying player
		if (isGhost && currentSpaces != null)
			foreach (StampSpace square in currentSpaces){
                square.SetGhost(false);
                square.SetOccupyingPlayer(gameObject);
            }
		if(!isGhost && currentSpaces != null)
            foreach (StampSpace square in currentSpaces){
                square.SetOccupyingPlayer(gameObject);
            }

        //update currentSpaces to be the spaces we are moving to
        currentSpaces = targetSpaces;

        //set the player on the square, and set the owner/color
        foreach (StampSpace square in currentSpaces){
            square.SetOccupyingPlayer(gameObject);
            if (isGhost)
                square.SetGhost(true);
            else {
                square.SetOwner(gameObject);
                square.SetColor(playerColor);
            }
        }
				
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
        for (int i = 0; i < wallsToCreate; i++){
            //while we have not actually created a wall
            bool wallGenerated = false;
            while (!wallGenerated){
                //generate a random starting point
                Vector3 startingPoint = new Vector3(Random.Range((gameGrid.Height / -2), (gameGrid.Height/2)), 0, Random.Range((gameGrid.Width / -2), (gameGrid.Width / 2))); //is there a better way to check points on grid?
                //check if that point is already a wall
                GameObject tempPointCheck = null;
                if (gameGrid.TryGetGridObject(startingPoint, out tempPointCheck) && !tempPointCheck.GetComponent<StampSpace>().isWall){
                    //create a list of directions we have already tried
                    int chosenDirection = 0;
                    List<int> directionsChecked = new List<int>();
                    //while we have not found a direction for our wall
                    while (chosenDirection == 0){
                        //generate a direction that we have not checked
                        bool failedMinWallTest = false;
                        int tempDirection = Random.Range(1, 4);
                        if (!directionsChecked.Contains(tempDirection)){
                            directionsChecked.Add(tempDirection);
                            //is this direction with this startingpoint valid?
                            for (int j = 0; j < wallsMinSize; j++){
                                if (!failedMinWallTest){
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
                                        failedMinWallTest = true;
                                }
                            }
                        }
                        else {
                            //this ensures directions that are not valid will not be marked true.
                            //for the time being, don't make this an else if or stuff breaks
                            if (directionsChecked.Count == 4) { chosenDirection = -1; }
                        }

                        //if we made it this far, then we know the current direction is valid
                        if (!failedMinWallTest)
                            chosenDirection = tempDirection;
                    }

                    //if our direction was good, then we know we can actually build the wall now
                    if(chosenDirection != 0 && chosenDirection != -1){
                        //generate the wallSize
                        int wallSize = wallsMaxSize / 2;
                        bool pickAgain = false;
                        do
                        {
                            pickAgain = false;
                            wallSize = Random.Range(wallsMinSize, wallsMaxSize);
                            //if the value is in the middle always use it
                            if (wallSizeRollValue == 1 && wallSize > (wallsMaxSize / 2) && Random.value <= wallSizeRollChance)
                                pickAgain = true;
                            if (wallSizeRollValue == -1 && wallSize < (wallsMaxSize / 2) && Random.value <= wallSizeRollChance)
                                pickAgain = true;
                        } while (pickAgain);

                        //build a list of spaces that will be turned into a wall
                        List<StampSpace> wallSpaces = new List<StampSpace>();
                        for (int w = 0; w < wallSize; w++){
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
                            //ensure that we can build a wall in the temporary grid space, if not try a different direction
                            GameObject tempMinWall = null;
                            if (gameGrid.TryGetGridObject(tempWallPoint, out tempMinWall) && !tempMinWall.GetComponent<StampSpace>().isWall)
                                wallSpaces.Add(tempMinWall.GetComponent<StampSpace>());
                        }

                        //loop over our list of wall spaces and turn each into a wall
                        foreach (StampSpace square in wallSpaces)
                            square.InvokeWall(wallDuration);

                        //wall has been generated    
                        wallGenerated = true;
                    }
                }
            }
        }
    }
}
