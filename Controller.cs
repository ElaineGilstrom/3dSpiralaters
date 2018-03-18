using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
	
	public GameObject cam;

	public float scrollSpeed;
	public float sensitivity;
	public Vector3 lastPos;

	private void Start() {
		lastPos = Input.mousePosition;
	}
	
	void Update () {
		if(!lastPos.Equals(Input.mousePosition)) {
			if(!Input.GetMouseButton(1) && !Input.GetMouseButton(0) && Input.GetMouseButton(2)) {
				var forward = cam.transform.forward;
				forward.x = 0f; forward.y = 0f;
				forward.Normalize();
				cam.transform.Translate(forward * (lastPos - Input.mousePosition)[1] * scrollSpeed * -1);
			} else if(!Input.GetMouseButton(1) && Input.GetMouseButton(0) && !Input.GetMouseButton(2)) {
				var right = cam.transform.right;
				right.y = 0f; right.Normalize();
				var up = cam.transform.up;
				up.x = 0f; up.Normalize();
				Vector3 dif = (right * (lastPos - Input.mousePosition)[0] + up * (lastPos - Input.mousePosition)[1]) * scrollSpeed;
				cam.transform.Translate(dif);
			} else if(Input.GetMouseButton(1) && !Input.GetMouseButton(0) && !Input.GetMouseButton(2)) {
				Vector3 dif = (lastPos - Input.mousePosition) * sensitivity;
				cam.transform.Rotate(new Vector3(dif[1], -1 * dif[0], 0));
			}
			lastPos = Input.mousePosition;
		}
	}
}
