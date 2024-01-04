using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonController : MonoBehaviour
{
    public int roomsToSpawn;
    public int roomToCorridorPercentage = 50;
    public int bonusRoomsSpawn;
    public bool readyToGenerate;
    public bool generatedDungeon;

    private DungeonEntrance dungeonEntranceScript;
    private DungeonRenderer dungeonRendererScript;

    public void OnEnable()
    {
        dungeonEntranceScript = GetComponentInParent<DungeonEntrance>();
        dungeonRendererScript = GetComponent<DungeonRenderer>();
    }

    public void GenerateDungeon()
    {
        int randomIndex;
        int randomChance;
        GameObject room;
        Quaternion randomRotation;

        List<Transform> floatingPointList = new List<Transform>();
        List<Transform> staticPointList = new List<Transform>();

        // Code for generating the dungeon

        // A. Select a random Starting Room from the StartingRoom List
        randomIndex = Random.Range(0, GameManager.Instance.DungeonPrefabSets[dungeonEntranceScript.dungeonRarity.GetHashCode()].StartingRooms.Count);

        //    Instantiate the selected StartingRoom
        //    Move it to the dungeon's dungeonStart GameObject's position
        //    Make the room a child of the dungeonStart object
        room = Instantiate(GameManager.Instance.DungeonPrefabSets[dungeonEntranceScript.dungeonRarity.GetHashCode()].StartingRooms[randomIndex], transform.position, transform.rotation, transform);
        //    Go to step B

        // B. Rotate the StartingRoom randomly,
        //    Snap it to the closest 90 degree angle
        randomRotation = Quaternion.Euler(0, Random.Range(0, 4) * 90.0f, 0);
        room.transform.rotation = randomRotation;
        //    Go to step C

        // C. Update the cardinalDirection value for each of the room's connection points
        //    Loop through all the child Objects of the room (might update later if the room will have decorations parented to it too)
        foreach (Transform snapPoint in room.transform)
        {
            // Get the ConnectionPointData script in the object
            ConnectionPointData dataScript = snapPoint.GetComponent<ConnectionPointData>();

            // If the object doesnt have the script then move on to the next object
            if (!dataScript)
            {
                continue;
            }
            // Else if it has the script, tell the script to update the cardinalDirection variable
            dataScript.UpdateDirection();
            // Add the points to the grounded points list (since starting room is always grounded)
            staticPointList.Add(snapPoint);
        }
        //    Go to step D

        // D. Pick step D1 or D2 at random
        randomChance = Random.Range(0, 100);

        if (randomChance <= roomToCorridorPercentage)
        {
            // - D1. Create a Room from the Room List
            //       Make the room a child of the dungeonStart object
            //       set the Room to a GameObject variable
            randomIndex = Random.Range(0, GameManager.Instance.DungeonPrefabSets[dungeonEntranceScript.dungeonRarity.GetHashCode()].NormalRooms.Count);

            room = Instantiate(GameManager.Instance.DungeonPrefabSets[dungeonEntranceScript.dungeonRarity.GetHashCode()].NormalRooms[randomIndex], transform.position, transform.rotation, transform);
        }
        else
        {
            // - D2. Create a Corridor from the Corridor List
            //       Make the Corridor a child of the dungeonStart object
            //       Set the Corridor to a GameObject variable
            randomIndex = Random.Range(0, GameManager.Instance.DungeonPrefabSets[dungeonEntranceScript.dungeonRarity.GetHashCode()].Corridors.Count);

            room = Instantiate(GameManager.Instance.DungeonPrefabSets[dungeonEntranceScript.dungeonRarity.GetHashCode()].Corridors[randomIndex], transform.position, transform.rotation, transform);
        }
        //    Go to step E

        // E. Rotate the Room/Corridor randomly
        //    Snap it to the closest 90 degree angle
        randomRotation = Quaternion.Euler(0, Random.Range(0, 4) * 90.0f, 0);
        room.transform.rotation = randomRotation;
        //    Go to step F

        // F. Update the cardinalDirection value for each of the room's connection points
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
            // Add the point to the ungrounded points list for use in the next step
            floatingPointList.Add(snapPoint);
        }
        //    Go to step G

        // G. Move the room to connect with a grounded room
        //    Go through the list and make sure the local positions of the poinst are set
        foreach (Transform floatingPoint in floatingPointList)
        {
            // Grab a reference to the ConncetionPointData
            ConnectionPointData pointData = floatingPoint.GetComponent<ConnectionPointData>();

            if (pointData.localOffset == Vector3.zero)
            {
                pointData.UpdateOffset();
            }
            else
            {
                continue;
            }
        }

        //    Pick a random point from both the floating room, and any of the unfinished static points

        //    Move the Room's/Corridor's point so that both selected points are in the same global position
        //    Move the floating room so that the point is back to its original local position
        //    Go to step H

        // H. OverlapBox to ensure the room doesnt intersect another room in its current position,
        //    Go to step I

        // I. if the room Overlaps, do I1, if it doesn't overlap, do I2
        //
        // - I1. Destroy the Room/Corridor, and go back to step D instead
        //
        // - I2. Set all of the room's connections as grounded
        //       Reduce roomsToSpawn by 1 if the prefab is a room, not a corridor
        //       Destroy the point that got connected since they are no longer needed
        //       Go back to Step D if roomsToSpawn is not 0

        // J. Create the specified number of bonus rooms

        // K. create a boss room

        // L. create a return portal room connected to the boss room

        dungeonRendererScript.DungeonRenderUpdate();
        generatedDungeon = true;
        readyToGenerate = false;

        Debug.Log("Dungeon has completed generation");
    }
}
