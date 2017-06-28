using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PickupObject : MonoBehaviour {
	
	GameObject mainCamera;

	bool carrying;
	GameObject carriedObject;
	public float distance = 3f;
	public float smooth;
	public float rSmooth;
	public float scrollSensitivity;

	public GameObject[] objectToSpawn;
	static int SelectedCube = 0;
	float yOffset;
	float zOffset;

	public ConnectorScript[] objects;

	[SerializeField]
	private GameObject myTextgameObject;  // gameObject in Hierarchy
	private Text Txt_SelectedCube;        // Our reference to text component

	void Start () {
		mainCamera = GameObject.FindWithTag("MainCamera");
		Txt_SelectedCube = myTextgameObject.GetComponent<Text>();
	}

	void Update () {
		if(carrying){
			carry (carriedObject);
			checkDrop ();
		}else{
			pickup();
		}
	}

	void carry(GameObject o){
		o.transform.position = Vector3.Lerp(o.transform.position, mainCamera.transform.position + mainCamera.transform.forward * distance , Time.deltaTime * smooth);
		o.transform.rotation = Quaternion.Lerp(o.transform.rotation, Quaternion.Euler( new Vector3(zOffset,gameObject.transform.eulerAngles.y + yOffset,0)), Time.deltaTime * rSmooth);
		if (Input.GetKey (KeyCode.Tab)) {
			zOffset += scrollSensitivity * Input.GetAxis ("Mouse ScrollWheel");
		} else {
			yOffset += scrollSensitivity * Input.GetAxis ("Mouse ScrollWheel");
		}
	}

	void spawnObject(Object objToSpawn){
		carrying = true;
		// the weird angling just gives it a cool animation
		carriedObject = Instantiate(objToSpawn, mainCamera.transform.position + mainCamera.transform.forward * distance, Quaternion.Euler(0,gameObject.transform.eulerAngles.y + 90,90)) as GameObject;
		carriedObject.GetComponent<Rigidbody> ().isKinematic = true;
		yOffset = 0f;
		zOffset = 0f;
		objects = FindObjectsOfType<ConnectorScript> ();
	}

	void pickup(){
		// create new block in hand
		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			spawnObject (objectToSpawn[SelectedCube]);
		} // avoid out-of-bounds
		/*if (SelectedCube > objectToSpawn.Length - 1) {
			SelectedCube = 0;
		}
		if (SelectedCube < 0) {
			SelectedCube = objectToSpawn.Length - 1;
		}*/
		// change selected block

		SelectedCube += Mathf.RoundToInt( Input.GetAxis ("Mouse ScrollWheel") );
		Debug.Log (Mathf.RoundToInt( Input.GetAxis ("Mouse ScrollWheel") ) );
		//Txt_SelectedCube.text = SelectedCube.ToString();

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
					zOffset = 0f;
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
