using UnityEngine;
using System.Collections;

public class Pointer : MonoBehaviour {
    public Transform Target;
    
	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.position;
        Vector3 target = Target.position;

        Quaternion r = Quaternion.FromToRotation(Vector3.forward, target - pos);

        transform.rotation = r;
	}
}
