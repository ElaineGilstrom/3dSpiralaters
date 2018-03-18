using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

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


	//Exporting stuffs

	public float expScaleWL;
	public float expScaleT;
	public string expFileName;

	public void updateScaleWL(string n) { expScaleWL = float.Parse(n); }
	public void updateScaleT(string n) { expScaleT = float.Parse(n); }
	public void updateFileName(string n) { expFileName = n; }

	public void exportOBJ() {
		List<Vector3> v = new List<Vector3>();//verticies are xyz coordinates
		List<int[]> f = new List<int[]>();//faces are 4 indxs of verticies in list
		genVFLists(ref v, ref f);
		
		FileStream of = File.Open(expFileName + ".obj", FileMode.Create);

		foreach(Vector3 vec in v) {
			string w = "v " + vec.x.ToString() + " " + vec.y.ToString() + " " + vec.z.ToString() + System.Environment.NewLine;
			byte[] b = strToByte(w);
			of.Write(b, 0, b.Length);
		}

		foreach(int[] face in f) {
			string w = "f";
			foreach(int i in face) w += " " + i.ToString();
			w += System.Environment.NewLine;
			byte[] b = strToByte(w);
			of.Write(b, 0, b.Length);
		}

		of.Close();
	}

	byte[] strToByte(string s) {
		char[] c = s.ToCharArray();
		byte[] b = new byte[c.Length];
		for(int i = 0; i < c.Length; i ++) b[i] = (byte) c[i];
		return b;
	}

	public void exportMap() {
			
	}

	private void genVFLists(ref List<Vector3> v, ref List<int[]> f) {
		foreach(Transform c in parent) {
			var pos = c.position;
			var size = c.GetComponent<Renderer>().bounds.size;
			int[][] faces = new int[6][];
			for(int i = 0; i < faces.Length; i++) faces[i] = new int[4];
			Vector3 vec = pos;
			if(!v.Contains(vec)) { v.Add(vec); Debug.Log("Added vec " + vecToStr(vec)); }

			int fn = find(v, vec);
			faces[0][0] = fn; faces[1][0] = fn; faces[2][0] = fn;

			for(int i = 0; i < 3; i++) {
				vec = pos;
				vec[i] += size[i];
				if(!v.Contains(vec)) { v.Add(vec); Debug.Log("Added vec " + vecToStr(vec)); }
				fn = find(v, vec);
				faces[i + 3][2] = fn; faces[i][1] = fn; faces[(2 + i) % 3][3] = fn;
				bool oneSet = false;
				for(int j = 0; j < 3; j++) {
					if(j == i) continue;
					vec[j] += size[i];
					if(!v.Contains(vec)) { v.Add(vec); Debug.Log("Added vec " + vecToStr(vec)); }
					fn = find(v, vec);
					if(oneSet) faces[i + 3][3] = fn;
					else { faces[i + 3][1] = fn; oneSet = true; }
					if((2 + i) % 3 == j) faces[i][2] = fn;
				}
			}
			vec = pos + size;
			if(!v.Contains(vec)) { v.Add(vec); Debug.Log("Added vec " + vecToStr(vec)); }

			fn = find(v, vec);
			faces[3][0] = fn; faces[4][0] = fn; faces[5][0] = fn;

			foreach(int[] fa in faces) {
				if(!intArrInList(f, fa)) { f.Add(fa); Debug.Log("Adding face: " + fa.ToString()); }
			}
		}
	}

	private int find(List<Vector3> lst, Vector3 vec) {
		for(int i = 0; i < lst.Count; i++) {
			if(lst[i] == vec) return i + 1;
		}
		return -1;
	}

	private bool intArrInList(List<int[]> l, int[] f) {
		foreach(int[] a in l) {
			if(a.Length != f.Length) continue;
			bool c = false;
			for(int i = 0; i < f.Length; i++) if(a[i] != f[i]) { c = true; break; }
			if(c) continue;
			return true;
		}
		return false;
	}

	private string vecToStr(Vector3 v) {
		string r = "{";
		for(int i = 0; i < 3; i++) r += " " + v[i].ToString();
		r += " }";
		return r;
	}
}
