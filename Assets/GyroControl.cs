using UnityEngine;
using System.Collections;

public class GyroControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Gyroscope gyro = Input.gyro;
        Vector3 gravity = gyro.gravity;

        Quaternion rotation = Quaternion.FromToRotation(gravity, Vector3.down);
        transform.rotation = rotation;
	}
}
