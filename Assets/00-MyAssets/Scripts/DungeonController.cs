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

    public List<Transform> floatingPointList = new List<Transform>();
    public List<Transform> staticPointList = new List<Transform>();

    public void OnEnable()
    {
        dungeonEntranceScript = GetComponentInParent<DungeonEntrance>();
        dungeonRendererScript = GetComponent<DungeonRenderer>();
    }

    public void GenerateDungeon()
    {
        int index = 0;
        int randomIndex;
        int randomChance;
        GameObject room;
        Quaternion randomRotation;

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

        // E. Update the cardinalDirection value for each of the room's connection points
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
        //    Go to step F

        // F. Move the room to connect with a grounded room
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

        //    Pick a random point from the static point list, and find a point with the opposite direction, and same size
        staticPointList.Shuffle();

        Transform staticPoint = staticPointList[index];
        ConnectionPointData staticPointData = staticPoint.GetComponent<ConnectionPointData>();

        floatingPointList.Shuffle();

        foreach (Transform floatingPoint in floatingPointList)
        {
            ConnectionPointData floatingPointData = floatingPoint.GetComponent<ConnectionPointData>();

            if (staticPointData.connectionSize == floatingPointData.connectionSize)
            {
            redoRotationCheck:
                if (staticPointData.transform.rotation != new Quaternion(0, floatingPointData.transform.rotation.y + 180, 0, 0))
                {
                    // rotate the room so the points can match up
                    room.transform.rotation = new Quaternion(0, room.transform.rotation.y + 90, 0, 0);

                    // Update connectionPointData to reflect this rotation change
                    floatingPointData.UpdateDirection();

                    goto redoRotationCheck;
                }
                else if(staticPointData.transform.rotation == new Quaternion(0, floatingPointData.transform.rotation.y + 180, 0, 0))
                {
                    floatingPointData.localOffset = floatingPoint.localPosition;

                    // Move the Room's/Corridor's floating point to the static point's global position
                    // floatingPoint.transform.position = staticPoint.transform.position;

                    // Move the floating room so that the floating point is back to its original local position
                    // room.GetComponentInParent<Transform>().position = floatingPoint.position + floatingPointData.localOffset;
                    break;
                }
            }
            else
            {
                continue;
            }
        }
        
        
        //    Go to step G

        // G. OverlapBox to ensure the room doesnt intersect another room in its current position,
        //    (The room's mesh might be useful for figuring out the size of the room for this step)
        //    Go to step H

        // H. if the room Overlaps, do H1, if it doesn't overlap, do H2
        //
        // - H1. Destroy the Room/Corridor, and go back to step D instead
        //
        // - H2. Set all of the room's connections as grounded
        //       Reduce roomsToSpawn by 1 if the prefab is a room, not a corridor
        //       Destroy the point that got connected since they are no longer needed
        //       Go back to Step D if roomsToSpawn is not 0

        // I. Create the specified number of bonus rooms dungeon rarity is 

        // J. create a boss room from the list of the dungeon's rarity

        // K. create a return portal room connected to the boss room from the list of the dungeon's rarity

        // L. Cap off any of the remianing incomplete connections using objects from the ConnectionCaps list for the specified Dungeon Rarity

        dungeonRendererScript.DungeonRenderUpdate();
        generatedDungeon = true;
        readyToGenerate = false;

        Debug.Log("Dungeon has completed generation");
    }
}
