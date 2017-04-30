using UnityEngine;
using System.Collections;

public class PickupObject : MonoBehaviour {
	GameObject mainCamera;
	bool carrying;
	GameObject carriedObject;
	public float distance = 3f;
	public float smooth;
	public float rSmooth;
	public float scrollSensitivity;
	public GameObject toSpawnWith1;
	public GameObject toSpawnWith2;
	public GameObject toSpawnWith3;
	public GameObject toSpawnWith4;
	float yOffset;
	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindWithTag("Head");
	}
	
	// Update is called once per frame
	void Update () {
		if(carrying){
			carry (carriedObject);
			checkDrop ();
		}else{
			pickup();
		}
	}
	//TODO: line 33 works for now, but this dual-rotation bug must be fixed (parent y rotation + local y rotation) since local y rot should always be 0
	void carry(GameObject o){
		o.transform.position = Vector3.Lerp(o.transform.position, mainCamera.transform.position + transform.worldToLocalMatrix.MultiplyVector(mainCamera.transform.forward) * distance , Time.deltaTime * smooth);
		o.transform.rotation = Quaternion.Lerp(o.transform.rotation, Quaternion.Euler( new Vector3(0,gameObject.transform.eulerAngles.y + yOffset,0)), Time.deltaTime * rSmooth);
		yOffset += scrollSensitivity * Input.GetAxis("Mouse ScrollWheel");
		//Debug.Log(gameObject.transform.eulerAngles.y);
	} //o.transform.rotation.x, mainCamera.transform.forward.y, o.transform.rotation.z

	void spawnObject(Object objToSpawn){
		carrying = true;
		// the weird angling just gives it a cool animation
		carriedObject = Instantiate(objToSpawn, mainCamera.transform.position + mainCamera.transform.forward * distance, Quaternion.Euler(0,gameObject.transform.eulerAngles.y + 90,90)) as GameObject;
		carriedObject.GetComponent<Rigidbody> ().isKinematic = true;
		yOffset = 0f;
	}

	void pickup(){
		// create new block in hand
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			spawnObject (toSpawnWith1);
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			spawnObject (toSpawnWith2);
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			spawnObject (toSpawnWith3);
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			spawnObject (toSpawnWith4);
		}
		// pickup existing block
		if (Input.GetKeyDown (KeyCode.E)) {
			int x = Screen.width / 2;
			int y = Screen.height / 2;

			Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x,y));
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit)){
				pickupable p = hit.collider.GetComponent<pickupable>();
				if(p != null){
					carrying = true;
					carriedObject = p.gameObject;
					p.gameObject.GetComponent<Rigidbody> ().isKinematic = true;
					yOffset = 0f;
				}
			}
		}
	}

	void checkDrop(){
		if (Input.GetKeyDown (KeyCode.E)) {
			dropObject ();
		}
	}

	void dropObject(){
		carrying = false;
		carriedObject.GetComponent<Rigidbody> ().isKinematic = false;
		carriedObject = null;
	}
}
