using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListAddr : MonoBehaviour {

	public GameObject comp;
	public Transform parent;
	public List<GameObject> compList;

	public void addComp() {
		GameObject n = Instantiate(comp);
		n.transform.SetParent(parent);
		N s = n.GetComponent<N>();
		s.parent = this;
		compList.Add(n);
	}

	public void remComp(int indx) {
		for(int i = compList.Count - 1; i > indx; i--) {
			N num = compList[i].GetComponent<N>();
			num.indx--;
		}
		compList.RemoveAt(indx);
	}

}
