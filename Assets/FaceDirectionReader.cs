using UnityEngine;
using System.Collections;

public class FaceDirectionReader : MonoBehaviour {

    public TextMesh DirectionReadout;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        var t = (transform.rotation * Vector3.forward);
        DirectionReadout.text = ""+ Mathf.Atan2(t.z, t.x);
    }
}
