using UnityEngine;
using System.Collections;
using System;

public class Destination : MonoBehaviour {
    public LocationInfo location;
    public int RouteIndex = -1;

    public GameObject Ring;

    /// <summary>
    /// All work and no play makes sandwiches for the wealthy
    /// </summary>
    public void SetSize(float size)
    {
        Ring.transform.localScale = new Vector3(size, 1, size);
    }
}
