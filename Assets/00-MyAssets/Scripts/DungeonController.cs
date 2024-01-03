using System.Collections.Generic;
using UnityEngine;

public class DungeonController : MonoBehaviour
{
    public int roomsToSpawn;
    public int roomToCorridorPercentage = 50;
    public int bonusRoomsSpawn;
    public bool readyToGenerate;
    public bool generatedDungeon;

    private DungeonEntrance dungeonEntranceScript;
    public void OnEnable()
    {
        dungeonEntranceScript = GetComponentInParent<DungeonEntrance>();
    }

    public void GenerateDungeon()
    {
        // Code for generating the dungeon

        // A. Select a random Starting Room from the StartingRoom List
        int randomIndex;
        randomIndex = Random.Range
            (
            0,
            GameManager.Instance.DungeonPrefabSets[dungeonEntranceScript.dungeonRarity.GetHashCode()].StartingRooms.Count
            );

        //    Instantiate the selected StartingRoom
        //    Move it to the dungeon's dungeonStart GameObject's position
        //    Make the room a child of the dungeonStart object
        GameObject room;
        room = Instantiate
            (
            GameManager.Instance.DungeonPrefabSets[dungeonEntranceScript.dungeonRarity.GetHashCode()].StartingRooms[randomIndex],
            transform.position,
            transform.rotation,
            transform
            );
        //    Go to step B

        // B. Rotate the StartingRoom randomly,
        Quaternion randomRotation;
        randomRotation = Quaternion.Euler(0, Random.Range(0, 4) * 90.0f, 0);

        room.transform.rotation = randomRotation;

        //    Snap it to the closest 90 degree angle
        SnapRoomRotation(room);
        //    Go to step C

        // C. Update the cardinalDirection value for each of the room's connection points
        //    Loop through all the child Objects of the room (might update later if the room will have decorations parented to it too)
        foreach(Transform snapPoint in room.transform)
        {
            // Get the ConnectionPointData script in the object
            ConnectionPointData dataScript = snapPoint.GetComponent<ConnectionPointData>();

            // If the object doesnt have the script then move on to the next object
            if (!dataScript)
            {
                continue;
            }
            // Else if it has the script, tell it to update the cardinalDirection variable
            dataScript.UpdateDirection();
        }
        //    Go to step D

        // D. Pick step D1 or D2 at random
        int random;
        random = Random.Range(0, 100);

        if (random <= roomToCorridorPercentage)
        {
            // - D1. Create a Room from the Room List
            //       Make the room a child of the dungeonStart object
            //       set the Room to a GameObject variable
            randomIndex = Random.Range
                (
                0,
                GameManager.Instance.DungeonPrefabSets[dungeonEntranceScript.dungeonRarity.GetHashCode()].NormalRooms.Count
                );

            room = Instantiate
                (
                GameManager.Instance.DungeonPrefabSets[dungeonEntranceScript.dungeonRarity.GetHashCode()].NormalRooms[randomIndex], 
                transform.position, 
                transform.rotation, 
                transform
                );
        }
        else
        {
            // - D2. Create a Corridor from the Corridor List
            //       Make the Corridor a child of the dungeonStart object
            //       Set the Corridor to a GameObject variable
            randomIndex = Random.Range(0, GameManager.Instance.DungeonPrefabSets[dungeonEntranceScript.dungeonRarity.GetHashCode()].Corridors.Count);
            room = Instantiate
                (
                GameManager.Instance.DungeonPrefabSets[dungeonEntranceScript.dungeonRarity.GetHashCode()].Corridors[randomIndex], 
                transform.position, 
                transform.rotation, 
                transform
                );
        }
        //    Go to step E

        // E. Rotate the Room/Corridor randomly
        randomRotation = Quaternion.Euler(0, Random.Range(0, 4) * 90.0f, 0);
        room.transform.rotation = randomRotation;

        //    Snap it to the closest 90 degree angle
        SnapRoomRotation(room);
        //    Go to step F

        // F. Update the cardinalDirection value for each of the room's connection points
        List<Transform> connectionPointList = new List<Transform>();
        foreach (Transform snapPoint in room.transform)
        {
            // Get the ConnectionPointData script in the object
            ConnectionPointData dataScript = snapPoint.GetComponent<ConnectionPointData>();

            // If the object doesnt have the script then move on to the next object
            if (!dataScript)
            {
                continue;
            }
            // Else if it has the script, tell it to update the cardinalDirection variable
            dataScript.UpdateDirection();
            // Add the point to the list for use in the next step
            connectionPointList.Add(snapPoint);
        }
        //    Go to step G

        // G. Using the GameObject variable, find all the connection data points of the room
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
        // - K1. Return to step I
        //       but if it fails a set number of times;
        //          Destroy the Room/Corridor, and go back to step D instead
        //
        // - K2. Set all of the room's connections as grounded
        //       Reduce roomsToSpawn by 1 if the prefab is a room, not a corridor
        //       Go back to Step D if roomsToSpawn is not 0

        // L. Create the specified number of bonus rooms

        // M. create a boss room

        // N. create a return portal room connected to the boss room

        generatedDungeon = true;
        readyToGenerate = false;
    }

    public static void SnapRoomRotation(GameObject targetObject)
    {
        if (targetObject.transform.rotation.y != 0 
            || targetObject.transform.rotation.y != 90 
            || targetObject.transform.rotation.y != 180 
            || targetObject.transform.rotation.y != 270)
        {
            // Get the current rotation of the target object
            Vector3 currentRotation = targetObject.transform.rotation.eulerAngles;

            // Round the Y Euler angle to the nearest multiple of 90 degrees
            float snappedY = Mathf.Round(currentRotation.y / 90.0f) * 90.0f;

            // Create a new Quaternion with the snapped Y rotation
            Quaternion snappedRotation = Quaternion.Euler(currentRotation.x, snappedY, currentRotation.z);

            // Apply the snapped rotation to the target object
            targetObject.transform.rotation = snappedRotation;
        }
    }
}
