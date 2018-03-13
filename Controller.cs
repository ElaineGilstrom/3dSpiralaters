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
			if(Input.GetMouseButton(1) && !Input.GetMouseButton(0) && !Input.GetMouseButton(2)) {
				cam.transform.position += new Vector3(0, 0, -1 * (lastPos - Input.mousePosition)[1] * scrollSpeed);
			} else if(!Input.GetMouseButton(1) && Input.GetMouseButton(0) && !Input.GetMouseButton(2)) {
				Vector3 dif = (lastPos - Input.mousePosition) * scrollSpeed;
				cam.transform.position += new Vector3(dif[0],dif[1], 0);
			} else if(!Input.GetMouseButton(1) && !Input.GetMouseButton(0) && Input.GetMouseButton(2)) {
				Vector3 dif = (lastPos - Input.mousePosition) * sensitivity;
				cam.transform.Rotate(new Vector3(dif[1], -1 * dif[0], 0));
			}
			lastPos = Input.mousePosition;
		}
	}
}
