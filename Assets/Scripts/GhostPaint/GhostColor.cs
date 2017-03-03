using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script used for handling the various colors that are used in this game.
public class GhostColor{
    private string ghostColorString = "";
    
    //colorTable keys should always be alphabetical (BRY)
    private Hashtable colorTable = new Hashtable();

    public GhostColor(string startColor) {
        //init the hashTable of colors
        colorTable.Add("B", new Color(.16f, .373f, .6f));   //blue
        colorTable.Add("R", new Color(1f, 0, 0));           //red
        colorTable.Add("Y", new Color(1f, 1f, 0));          //yellow
        colorTable.Add("BR", new Color(.5f, 0, .5f));       //purple
        colorTable.Add("BY", new Color(0, .66f, .2f));      //green
        colorTable.Add("RY", new Color(1f, .5f, 0));        //orange
        colorTable.Add("BRY", new Color(1f, 1f, 1f));       //white
        colorTable.Add("BBR", new Color(.25f, 0, .5f));     //violet
        colorTable.Add("BRR", new Color(.5f, 0, .25f));     //magenta
        colorTable.Add("BBY", new Color(0, .5f, .5f));      //teal
        colorTable.Add("BYY", new Color(.5f, 1f, 0));       //chartruese
        colorTable.Add("RRY", new Color(1f, .25f, 0));      //Vermillion
        colorTable.Add("RYY", new Color(1f, .75f, 0));      //amber
        colorTable.Add("", new Color(.2f, .094f, 0));       //brown
        ghostColorString = startColor;
    }

    //function to add a color to the current color, red yellow or blue.
    public void addColor(string colorToAdd) {
        colorToAdd = colorToAdd.ToUpper();
        if(colorToAdd.Equals("B") || colorToAdd.Equals("BLUE")) {
            ghostColorString = "B" + ghostColorString;
        } else if(colorToAdd.Equals("R") || colorToAdd.Equals("RED")) {
            int lastBlueIndex = ghostColorString.LastIndexOf("B");
            ghostColorString = (lastBlueIndex == -1) ? "R" + ghostColorString : ghostColorString.Insert(lastBlueIndex, "R");
        } else if(colorToAdd.Equals("Y") || colorToAdd.Equals("YELLOW")) { ghostColorString = ghostColorString + "Y"; }
    }

    //function to remove a color from the current color, red yellow or blue.
    public void removeColor(string colorToRemove) {
        colorToRemove = colorToRemove.ToUpper();
        if(colorToRemove.Equals("B") || colorToRemove.Equals("R") || colorToRemove.Equals("Y")) {
            int lastIndexOfColor = ghostColorString.LastIndexOf(colorToRemove);
            if(lastIndexOfColor != -1) {ghostColorString = ghostColorString.Remove(lastIndexOfColor, 1);}
        }
    }

    //a function to remove unescessary duplicate colors (BBRR == BR)
    private void colorStringCleaner() {
        int blueCount = 0;
        int redCount = 0;
        int yellowCount = 0;
        for(int i = 0; i < ghostColorString.Length; i++) {
            if(ghostColorString[i].Equals('B')) {blueCount++;}
            else if(ghostColorString[i].Equals('R')) { redCount++; }
            else { yellowCount++; }
        }

        //removing this line breaks things...wtf...makes all ghost 1 color....what the hell...
        Debug.Log("STRING CLEANING INIT -- blue: " + blueCount + " red: " + redCount + " yellow: " + yellowCount);

        //remove all B occurrances that are higher than the allowed number (2)
        while(blueCount > 2) {
            removeColor("B");
            blueCount--;
        }
        //remove all R occurrances that are higher than the allowed number (2)
        while(redCount > 2) {
            removeColor("R");
            redCount--;
        }
        //remove all Y occurrances that are higher than the allowed number (2)
        while(yellowCount > 2) {
            removeColor("Y");
            yellowCount--;
        }

        
        //triple color check
        if(blueCount >= 1 &&  redCount >= 1 && yellowCount >= 1) {
            ghostColorString = "BRY";
            blueCount = 1;
            redCount = 1;
            yellowCount = 1;
        }

        //double color checks
        if(blueCount == 2 && blueCount == redCount) {
            removeColor("B");
            blueCount--;
            removeColor("R");
            redCount--;
        }
        if(blueCount == 2 && blueCount == yellowCount) {
            removeColor("B");
            blueCount--;
            removeColor("Y");
            yellowCount--;
        }
        if(redCount == 2 && redCount == yellowCount) {
            removeColor("R");
            redCount--;
            removeColor("Y");
            yellowCount--;
        }

        //single color checks
        if(blueCount == 2 && redCount == 0 && yellowCount == 0) {
            removeColor("B");
            blueCount--;
        }
        if(redCount == 2 && blueCount == 0 && yellowCount == 0) {
            removeColor("R");
            redCount--;
        }
        if(yellowCount == 2 && blueCount == 0 && redCount == 0) {
            removeColor("Y");
            yellowCount--;
        }
    }
    public Color toColor() {
        colorStringCleaner();
        return (Color)colorTable[ghostColorString];
    }
}
