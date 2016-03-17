using UnityEngine;
using System.Collections;

public class RotateChildren : MonoBehaviour {

	// Use this for initialization
	void Start () {
        for (int i = transform.childCount - 1; i >=0; i--) {
                transform.GetChild(i).gameObject.AddComponent<Rotator>();
        }
	}

    public class Rotator : MonoBehaviour {
        private Quaternion rotation;
        void Start()
        {
            rotation = Quaternion.AngleAxis(Random.Range(1, 5), new Vector3(Random.Range(-1f, 1), Random.Range(-1f, 1), Random.Range(-1f, 1)).normalized);
        }

        void Update()
        {
            transform.localRotation *= rotation;
        }
    }
}
