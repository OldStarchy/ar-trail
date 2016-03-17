using UnityEngine;
using System.Collections.Generic;

public class DestinationManager : MonoBehaviour {
    public Destination PointerObject;
    private List<Destination> destinations = new List<Destination>();
    public Pointer[] Pointers;
    private LocationInfo? initialLocation = null;

    public TextMesh LatitudeReadout;
    public TextMesh LongditudeReadout;
    public TextMesh AccuracyReadout;

    public CardboardHead head;

    // Use this for initialization
	void Start () {
        Input.location.Start(0.5f, 0.5f);
        Input.compass.enabled = true;
    }

    void OnApplicationQuit()
    {
        Input.location.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            var loc = Input.location.lastData;
            GPSlocation gl = new GPSlocation(loc);

            LatitudeReadout.text = "Lat: " + loc.latitude;
            LongditudeReadout.text = "Long: " + loc.longitude;
            AccuracyReadout.text = "< " + loc.horizontalAccuracy + "m";

            if (initialLocation == null)
            {
                initialLocation = loc;
                float heading = Input.compass.trueHeading;
                transform.rotation = Quaternion.AngleAxis(Mathf.PI / 2 - heading, Vector3.down);
                head.trackRotation = true;
            } else
            {
                var ogl = new GPSlocation(initialLocation.Value);

                float dir =(float) GPSlocation.Direction(ogl, gl);
                float dist =(float) GPSlocation.Distance(ogl, gl);
                Vector3 vec = new Vector3(Mathf.Cos(dir), 0, Mathf.Sin(dir)) * dist;
                //Vector3 vec = new Vector3(loc.latitude - initialLocation.Value.latitude, 0, initialLocation.Value.longitude - loc.longitude);
                transform.position = vec;
            }
        }
    }


    public void AddDestination()
    {
        LocationInfo loc = Input.location.lastData;

        Destination destination = GameObject.Instantiate(PointerObject);
        destination.SetLocation(loc, initialLocation.Value);

        destinations.Add(destination);
    }
}
