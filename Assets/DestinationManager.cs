using UnityEngine;
using System.Collections.Generic;

public class DestinationManager : MonoBehaviour
{
    public Destination PointerObject;
    private List<Destination> destinations = new List<Destination>();
    public List<Destination> Route = new List<Destination>();
    public Pointer[] Pointers;
    private GPSlocation initialLocation = null;
    private GPSlocation currentLocation = null;
    public UIMachine takeItToTheLimit_this_week_in_esoteric_programming;
    public Vector3 startPos;
    public TextMesh LatitudeReadout;
    public TextMesh LongditudeReadout;
    public TextMesh AccuracyReadout;
    public TextMesh DirectionReadout2;
    public TextMesh CompassAccuracy;
    public TextMesh PathInstructionsThing;
    public float XXinit = 0;
    public float ZZinit = 0;
    public Transform AccuracyCircle;
    private Quaternion prevRotation;
    public CardboardHead head;
    public Vector3 initialPosition;
    [Header("Virtual GPS")]
    public bool EnableVirtualGPS;
    [Range(34.63213f - 0.0001f, 34.83213f + 0.0001f)]
    public double Latitude = 34.73213;
    [Range(135.6348f - 0.0001f, 135.8348f + 0.0001f)]
    public double Longditude = 135.7348;
    [Range(0.5f, 1000)]
    public double Accuracy = 1;

    public int DestinationCount { get { return destinations.Count; } }

    /// <summary>
    /// Literally not used for anything.
    /// </summary>
    public bool FollowingRoute
    {
        get; private set;
    }
    /// <summary>
    /// Is this comment even neccassary?
    /// </summary>
    private int _requiredVariable;
    public int PositionInRoute { get { return _requiredVariable; }
        set
        {
            _requiredVariable = value;
            PathInstructionsThing.text = "次のチェックポイントに\n行ってください\n" + (value + 1) + "/" + Route.Count;
        }
    }

    /// <summary>
    /// "7 is a good number..." said he "...for such a purpose as this."
    /// </summary>
    private float _waypointSize = 7;
    /// <summary>
    /// Can you spell _ _ _ _ _ _? No? it means radius. Its not there because this is a diameter.
    /// </summary>
    public float WaypointSize
    {
        get { return _waypointSize;
        }
        set
        {
            _waypointSize = value;
            foreach (var v in destinations) {
                v.SetSize(value);
            }
        }
    }

    void Start()
    {
        Input.location.Start(0.5f, 0.5f);
        Input.compass.enabled = !false;
        takeItToTheLimit_this_week_in_esoteric_programming.ShowBoringStuff();
    }

    public Destination GetDestination(int index)
    {
        return destinations[index];
    }

    void OnApplicationQuit()
    {
        Input.location.Stop();
    }

    public void BeginFollowingRoute()
    {
        if (Route.Count == 0)
            return;

        FollowingRoute = true;
        PositionInRoute = 0;
        PointTo(Route[0].transform);
        WaypointSize = 1+1//+1+1+1+1+1 //We need this line of code or else it won't look good
            ;
        takeItToTheLimit_this_week_in_esoteric_programming.ShowInstructions();
    }

    public void FinishFollowingRoute()
    {
        foreach (var v in Route) {
            v.gameObject.SetActive(true);
        }
        FollowingRoute = false;
        takeItToTheLimit_this_week_in_esoteric_programming.OmedetouToTheUserPlux();
    }

    // Update is called once per frame
    void Update()
    {
        float y = Cardboard.SDK.HeadPose.Orientation.eulerAngles.y;
        //        float phoneDirection = (-Input.compass.trueHeading);        float cardboardDirection = Mathf.Atan2(transform.GetChild(0).forward.z, transform.GetChild(0).forward.x) * Mathf.Rad2Deg;
        CompassAccuracy.text = "Input Dir: " + (prevRotation.eulerAngles.y-y) + " Act: " + y;
        if (Input.location.status == LocationServiceStatus.Running || EnableVirtualGPS) {
            var gl = getLatLong();

            LatitudeReadout.text = "Lat: " + gl.latitude;
            LongditudeReadout.text = "Long: " + gl.longitude;
            AccuracyReadout.text = "< " + gl.horizontalAccuracy + "m";

            AccuracyCircle.localScale = new Vector3((float)gl.horizontalAccuracy, 1, (float)gl.horizontalAccuracy);
            WaypointSize = (float)gl.horizontalAccuracy;
            if (initialLocation == null) {
                ResetLocation();
            } else {
                var ogl = initialLocation;
             //   float radius = 6371;
                float xCur = (float)gl.latitude;//radius * Mathf.Cos((float)gl.latitude) * Mathf.Cos((float)gl.longitude);
                float zCur = (float)gl.longitude;//radius * Mathf.Cos((float)gl.latitude) * Mathf.Sin((float)gl.longitude);
                float xInit = (float)ogl.latitude;//radius * Mathf.Cos((float)ogl.latitude) * Mathf.Cos((float)ogl.longitude);
                float zInit = (float)ogl.longitude;//radius * Mathf.Cos((float)ogl.latitude) * Mathf.Sin((float)ogl.longitude);
//                float dir = (float)transform.forward;//GPSlocation.Direction(ogl, gl);
//                float dist =(float) GPSlocation.Distance(ogl, gl);
//                Vector3 vec = new Vector3(Mathf.Cos(dir), 0, Mathf.Sin(dir)) * dist;
                //Vector3 vec = new Vector3(loc.latitude - initialLocation.Value.latitude, 0, initialLocation.Value.longitude - loc.longitude);

                XXinit = (xCur - xInit) * (40075.0f/360)*1000;
                ZZinit = (zCur - zInit) * (40075.0f/360)*1000;
                transform.position = initialPosition + new Vector3(ZZinit, 0, XXinit);//initialPosition + transform.forward * (xCur - xInit);// +transform.right * (zCur - zInit);
//                transform.position = initialPosition + ;//;, 0, z);
//                transform.position = new Vector3(x,0,z);
            }

            if (FollowingRoute) {
                if ((Route[PositionInRoute].transform.position - transform.position).magnitude < WaypointSize / 2) {
                    Route[PositionInRoute].gameObject.SetActive(false);
                    PositionInRoute++;
                    if (PositionInRoute >= Route.Count) {
                        FinishFollowingRoute();
                    } else {
                        PointTo(Route[PositionInRoute].transform);
                    }
                }
            }
        }
    }

    public void RemoveMarker(int index)
    {
        if (destinations[index] != null) {
            if (destinations[index].RouteIndex >= 0)
                ToggleInRoute(index);
            Destroy(destinations[index].gameObject);
        }
        destinations.RemoveAt(index);
    }

    public void PointTo(int index)
    {
        foreach (var ptr in Pointers) {
            ptr.Target = destinations[index] != null ? destinations[index].transform : null;
        }
    }

    public void PointTo(Transform tr)
    {
        foreach (var ptr in Pointers) {
            ptr.Target = tr;
        }
    }

    public int ToggleInRoute(int index)
    {
        var d = destinations[index];
        if (Route.Contains(d)) {
            Route.Remove(d);
            for (int i = d.RouteIndex; i < Route.Count; i++)
                Route[i].RouteIndex = i;
            d.RouteIndex = -1;
        } else {
            Route.Add(d);
            d.RouteIndex = Route.Count - 1;
        }
        return d.RouteIndex;
    }

    private GPSlocation getLatLong()
    {
        if (EnableVirtualGPS)
            return new GPSlocation(Latitude, Longditude, Accuracy);
        else
            return new GPSlocation(Input.location.lastData);
    }

    public void ResetLocation()
    {
        transform.position = new Vector3(0, 0, 0);
        if (Input.location.status != LocationServiceStatus.Running && !EnableVirtualGPS)
            return;
        var loc = getLatLong();

        initialLocation = loc;
        initialPosition = transform.position;
        prevRotation = Cardboard.SDK.HeadPose.Orientation;
//        float phoneDirection = (90-Input.compass.magneticHeading);
        float cardboardDirection = prevRotation.eulerAngles.y; //Mathf.Atan2(transform.GetChild(0).forward.z, transform.GetChild(0).forward.x) * Mathf.Rad2Deg;
        float delta = cardboardDirection;

        //transform.rotation = Quaternion.AngleAxis(delta + Mathf.PI, Vector3.up);

//        //delta = Input.GetAxis("Horizontal") + 0.5f;
   //       transform.Rotate(Vector3.up, delta);
      //  transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,cardboardDirection,transform.rotation.eulerAngles.z);
        DirectionReadout2.text = "" + delta;
        head.trackRotation = true;
    }


    public void AddDestination()
    {
        Destination destination = GameObject.Instantiate(PointerObject);
        destination.transform.position = transform.position;
        destinations.Add(destination);
    }

    public void SaveData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("destination count", destinations.Count);
        for (int i = 0; i < destinations.Count; i++) {
            var d = destinations[i];

            PlayerPrefs.SetFloat(i + "_x", d.transform.position.x);
            PlayerPrefs.SetFloat(i + "_z", d.transform.position.z);
            PlayerPrefs.SetInt(i + "_route", d.RouteIndex);
        }
        PlayerPrefs.SetInt("route size", Route.Count);
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        while (destinations.Count > 0)
            RemoveMarker(0);

        int c = PlayerPrefs.GetInt("destination count");
        Destination[] rt = new Destination[PlayerPrefs.GetInt("route size")];
        for (int i = 0; i < c; i++) {
            Destination destination = GameObject.Instantiate(PointerObject);
            destination.transform.position = new Vector3(PlayerPrefs.GetFloat(i + "_x"), 0, PlayerPrefs.GetFloat(i + "_z"));
            var r = PlayerPrefs.GetInt(i + "_route");
            if (r >= 0) {
                rt[r] = destination;
                destination.RouteIndex = r;
            }
            destinations.Add(destination);
        }
        Route = new List<Destination>(rt);
    }
}
