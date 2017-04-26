using UnityEngine;
using System.Collections;

public class manageBlockHierarchy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		findSource ();
	}

	void findSource(){
		ConnectorScript[] objects = FindObjectsOfType<ConnectorScript> ();
		for (int i = 0; i < objects.Length; i++) {
			objects [i].attach ();
			objects[i].source = null;
		}
		for( int i = 0; i<objects.Length;i++){
			if (objects [i].block != null) {
				objects [i].block.GetComponent<ConnectorScript> ().source = objects[i].gameObject;
			}
		}
	}

}
