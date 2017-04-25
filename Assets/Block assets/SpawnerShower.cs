using UnityEngine;
using System.Collections;

public class SpawnerShower : MonoBehaviour {

	public GameObject objToSpawn;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Instantiate (objToSpawn, gameObject.transform.position + gameObject.transform.forward * 4, Quaternion.Euler (0, 0, 0));
	}
}
