using UnityEngine;
using System.Collections.Generic;
using System;

public class DestinationManager : MonoBehaviour {
    public Destination PointerObject;
    private List<Destination> destinations = new List<Destination>();
    public Pointer[] Pointers;
    private LocationInfo? initialLocation = null;

    public TextMesh LatitudeReadout;
    public TextMesh LongditudeReadout;
    public TextMesh AccuracyReadout;
    public TextMesh DirectionReadout2;

    public CardboardHead head;

    public int DestinationCount { get { return destinations.Count; } }

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
                ResetLocation();
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

    public void RemoveMarker(int index)
    {
        if (destinations[index] != null)
            Destroy(destinations[index].gameObject);
        destinations.RemoveAt(index);
    }

    public void PointTo(int index)
    {
        foreach (var ptr in Pointers)
        {
            ptr.Target = destinations[index] != null ?destinations[index].transform : null;
        }
    }

    public void ResetLocation()
    {
        if (Input.location.status != LocationServiceStatus.Running)
            return;
        var loc = Input.location.lastData;

        initialLocation = loc;


        float phoneDirection = (90 - Input.compass.trueHeading);
        float cardboardDirection = Mathf.Atan2(transform.GetChild(0).forward.z, transform.GetChild(0).forward.x) * Mathf.Rad2Deg;
        float delta = cardboardDirection - phoneDirection;

        //transform.rotation = Quaternion.AngleAxis(delta + Mathf.PI, Vector3.up);

        //delta = Input.GetAxis("Horizontal") + 0.5f;
        transform.Rotate(Vector3.up, delta);
        DirectionReadout2.text = "" + delta;
        head.trackRotation = true;
    }


    public void AddDestination()
    {
        Vector3 pos;
        if (initialLocation == null)
        {
            pos = new Vector3(UnityEngine.Random.Range(-2f, 2), 0, UnityEngine.Random.Range(-2f, 2));
        } else { 
            LocationInfo loc = Input.location.lastData;

            pos = new Vector3(loc.latitude, 0, loc.longitude) - new Vector3(initialLocation.Value.latitude, 0, initialLocation.Value.longitude);
        }

        Destination destination = GameObject.Instantiate(PointerObject);
        destination.transform.position = pos;
        destinations.Add(destination);
    }
}
