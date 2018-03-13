using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N : MonoBehaviour {

	public GameObject self;
	public ListAddr parent;
	public GameObject spir;
	Generator m;

	public int indx;
	public float num;

	void Start () {
		spir = GameObject.Find("Spiralateral");
		m = spir.GetComponent<Generator>();
		indx = m.set.Count;
		num = 0f;
		m.set.Add(num);
	}
	
	public void updateNum(string s) {
		num = float.Parse(s);
		m.editSet(indx, num);
	}

	public void remove() {
		m.set.RemoveAt(indx);
		parent.remComp(indx);
		Destroy(self);
	}
}
