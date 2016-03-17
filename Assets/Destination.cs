using UnityEngine;
using System.Collections;

public class Destination : MonoBehaviour {
    public LocationInfo location;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetLocation(LocationInfo location, LocationInfo origin)
    {
        this.location = location;
        transform.position = new Vector3(location.latitude, 0, location.longitude) - new Vector3(origin.latitude, 0, origin.longitude);
    }
}
