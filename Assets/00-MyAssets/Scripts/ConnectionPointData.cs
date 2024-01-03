using System;
using UnityEngine;
using Common.Enums;

[Serializable]
public class ConnectionSize
{
    public int connectionWidth;
    public int connectionHeight;
}

public class ConnectionPointData : MonoBehaviour
{
    public CardinalDirection cardinalDirection;
    public ConnectionSize connectionSize;
    public bool isPartOfGroundedRoom;

    public void UpdateDirection()
    {
        // Get the snapped Y rotation of the object
        Vector3 currentRotation = transform.rotation.eulerAngles;
        float snappedY = Mathf.Round(currentRotation.y / 90.0f) * 90.0f;

        // Determine the cardinal direction based on the snapped rotation using a switch statement
        switch (snappedY)
        {
            case 0.0f:
                cardinalDirection = CardinalDirection.North;
                break;
            case 90.0f:
                cardinalDirection = CardinalDirection.East;
                break;
            case 180.0f:
                cardinalDirection = CardinalDirection.South;
                break;
            case 270.0f:
                cardinalDirection = CardinalDirection.West;
                break;
            default:
                // Handle any other snappedY values here if needed
                break;
        }
    }
}
