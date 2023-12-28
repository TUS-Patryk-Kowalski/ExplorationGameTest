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

    public void UpdateDirection(CardinalDirection direction)
    {
        switch (direction)
        {
            case CardinalDirection.North:
                transform.eulerAngles = new Vector3(0, 0, 0);
                break;
            case CardinalDirection.East:
                transform.eulerAngles = new Vector3(0, 90, 0);
                break;
            case CardinalDirection.South:
                transform.eulerAngles = new Vector3(0, 180, 0);
                break;
            case CardinalDirection.West:
                transform.eulerAngles = new Vector3(0, 270, 0);
                break;
        }
    }
}
