using UnityEngine;
using System.Collections;

public class RouteController : MonoBehaviour {
    public DestinationManager dm;
    public GameObject StartButton;
    public GameObject StopButton;
    public GameObject EditThingsButton;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartRoute()
    {
        StopButton.SetActive(true);
        StartButton.SetActive(false);
        EditThingsButton.SetActive(false);
        dm.BeginFollowingRoute();
    }

    public void StopRoute()
    {
        StopButton.SetActive(false);
        StartButton.SetActive(true);
        EditThingsButton.SetActive(true);
        dm.FinishFollowingRoute();
    }
}
