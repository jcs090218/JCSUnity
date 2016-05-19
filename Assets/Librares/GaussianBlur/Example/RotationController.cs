using UnityEngine;
using System.Collections;

public class RotationController : MonoBehaviour {

	public float speed = 1.0f;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Horizontal") != 0) {
			Vector3 angles = transform.eulerAngles;
			angles.y += Input.GetAxis("Horizontal") * speed;
			transform.eulerAngles = angles;
		}
	}
}
