using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstrainWithoutParenting : MonoBehaviour
{
    public Transform constrainedTo;

    private void Update()
    {
        transform.position = constrainedTo.position;
    }
}
