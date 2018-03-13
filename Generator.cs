using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Generator : MonoBehaviour {

	enum face {
		top,	//0,0,0
		front,	//0,0,90
		left,	//90,0,0
		bottom,	//0,0,0
		back,	//0,0,90
		right	//90,0,0
	}

	private face curFace;

	Vector3 rot;
	Vector3 posOff;
	Vector3 pos;

	public List<float> set;
	private int indx;

	public bool drawing;

	public float time;
	float deltaT;

	public Transform parent;
	public GameObject unit;

	void Start () {
		time = 0.5f;
		drawing = false;
		set = new List<float>();
		curFace = face.top;
		pos = new Vector3(0,0,0);
		indx = 0;
	}
	
	void Update () {
		if(drawing && set.Count > 0) {
			if(deltaT >= time) {
				deltaT = 0f;
				updateVectors();
				for(int i = 0; i < set[indx]; i++) {
					GameObject g = Instantiate(unit);
					g.transform.SetParent(parent);
					g.transform.position = pos;
					pos += posOff;
					g.transform.rotation = Quaternion.Euler(rot);
				}
				if(curFace == face.right) curFace = face.top;
				else curFace += 1;
				indx++;
				if(indx >= set.Count) indx = 0;
			} else deltaT += Time.deltaTime;
		}
	}

	void updateVectors() {
		switch(curFace) {
			case face.top:
				rot = new Vector3(0,0,0);
				posOff = new Vector3(1,0,0);
				pos += new Vector3(0,-0.5f,0.5f);
				break;
			case face.front:
				rot = new Vector3(0,0,90);
				posOff = new Vector3(0,0,1);
				pos += new Vector3(-0.5f,-0.5f,0);
				break;
			case face.left:
				rot = new Vector3(90,0,0);
				posOff = new Vector3(0,-1,0);
				pos += new Vector3(-0.5f,0,-0.5f);
				break;
			case face.bottom:
				rot = new Vector3(0,0,0);
				posOff = new Vector3(-1,0,0);
				pos += new Vector3(0,0.5f,-0.5f);
				break;
			case face.back:
				rot = new Vector3(0,0,90);
				posOff = new Vector3(0,0,-1);
				pos += new Vector3(0.5f,0.5f,0);
				break;
			case face.right:
				rot = new Vector3(90,0,0);
				posOff = new Vector3(0,1,0);
				pos += new Vector3(0.5f,0,0.5f);
				break;
		}
	}

	public void clear() {
		foreach(Transform c in parent) {
			Destroy(c.gameObject);
		}
		pos = Vector3.zero;
		curFace = face.top;
	}

	public void updateTime(float newTime) {
		time = newTime;
	}

	public void addToSet(float n) {
		set.Add(n);
	}

	public void editSet(int indx, float newVal) {
		set.RemoveAt(indx);
		set.Insert(indx, newVal);
	}

	public void removeSet(int indx) {
		set.RemoveAt(indx);
	}

	public void toggleStart() {
		drawing = !drawing;
	}

}
