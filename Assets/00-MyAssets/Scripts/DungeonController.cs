using UnityEngine;

public class DungeonController : MonoBehaviour
{
    public int roomsToSpawn;
    public bool readyToGenerate;
    private bool generatedDungeon;

    // Add 2 lists, one for the new room's connection data points, and one for the existing grounded connection data points

    private void OnEnable()
    {
        if (!generatedDungeon && readyToGenerate)
        {
            GenerateDungeon();
            GameManager.Instance.MovePlayer(transform, new Vector3(0,2,0));
        }
    }

    private void GenerateDungeon()
    {
        // Code for generating the dungeon

        // A. Select and Instantiate a random Starting Room from the StartingRoom List
        //    Move it to the dungeon's dungeonStart GameObject's position
        //    Make the room a child of the dungeonStart object
        //    Go to step B

        // B. Rotate the StartingRoom randomly,
        //    Snap it to the closest 90 degree angle
        //    Go to step C

        // C. Update the cardinalDirection value for each of the room's connection points
        //    Go to step D

        // D. Pick step D1 or D2 at random
        //
        // - D1. Create a Room from the Room List
        //       Make the room a child of the dungeonStart object
        //       set the Room to a GameObject variable
        //
        // - D2. Create a Corridor from the Corridor List
        //       Make the Corridor a child of the dungeonStart object
        //       Set the Corridor to a GameObject variable
        //
        //    Go to step E

        // E. Rotate the Room/Corridor randomly
        //    Snap it to the closest 90 degree angle
        //    Go to step F

        // F. Update the cardinalDirection value for each of the room's connection points
        //    Go to step G

        // G. Using the GameObject variable from step D find all the connection data points of the room
        //    Add them to a list (List1)
        //    Go to step H

        // H. Create a second list (List2)
        //    Add all the other incomplete but grounded connection points that also face the correct direction (opposite direction to the selected point from List1) and are also the right size
        //    Go to step I

        // I. Pick a random point from both lists
        //    Move the Room/Corridor so that both selected points are in the same global position
        //    Remove the point selected from List2
        //    Go to step J

        // J. OverlapBox to ensure the room doesnt intersect another room in its current position,
        //    Go to step K

        // K. if the room Overlaps, do K1, if it doesn't overlap, do K2
        //
        // - K2. Set all of the room's connections as grounded
        //       Reduce roomsToSpawn by 1 if the prefab is a room, not a corridor
        //       Go back to Step D if roomsToSpawn is not 0
        //
        // - K1. Return to step I
        //       but if it fails a set number of times;
        //          Destroy the Room/Corridor, and go back to step D instead

        generatedDungeon = true;
    }
}
