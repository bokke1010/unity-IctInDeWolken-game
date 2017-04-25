using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConnectorScript : MonoBehaviour {

	GameObject block;
	bool attached;
	public float connectRange=4;
	GameObject source;
	public string blockFunction = "none";
	GameObject Ball;
	// Use this for initialization
	void Start () {
		Ball = GameObject.FindWithTag("ball");
	}
	
	// Update is called once per frame
	void Update () {
		attach ();
		findSource ();
		Connect (new Color (0, 1, 0));
	}

	void FixedUpdate() {
		if (blockFunction == "clock" && Time.fixedTime % 2 == 0 && source == null){
			activateNext();
		}
	}

	void activateNext(){
		if (attached) {
			Debug.Log (Time.fixedTime,block);
			block.GetComponent<ConnectorScript> ().runCode ();
		}
	}

	bool attach(){
		Ray ray = new Ray (gameObject.transform.position, gameObject.transform.forward);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, connectRange)) {
			ConnectorScript p = hit.collider.GetComponent<ConnectorScript> ();
			if (p != null) {
				attached = true;
				block = p.gameObject;
				return true;
			}
		}
		block = null;
		attached = false;
		return false;
	}

	void runCode(){

		switch (blockFunction) {
			case "code_block":
				Debug.Log ("code activated");
				activateNext ();
				moveBall (gameObject.transform.forward, 5);
				break;
			case "connector_block":
				activateNext ();
				break;
		}
	}

	void moveBall (Vector3 movement, float dist){
		Ball.GetComponent<Rigidbody> ().velocity += movement * dist;
	}

	void findSource(){
		GameObject target = block;
		ConnectorScript[] objects = FindObjectsOfType<ConnectorScript> ();
		for (int i = 0; i < objects.Length; i++) {
			objects [i].attach ();
			objects[i].source = null;
		}
		for( int i = 0; i<objects.Length;i++){
			if (objects [i].block != null) {
				objects [i].block.GetComponent<ConnectorScript> ().source = gameObject;
			}
		}
	}

	void Connect(Color color, float duration = 0.02f)
	{
		int vertexes = 1;
		if (source == null) {
			GameObject nxtBlock = gameObject;
			while (nxtBlock.GetComponent<ConnectorScript> ().block != null) {
				nxtBlock = nxtBlock.GetComponent<ConnectorScript> ().block;
				vertexes += 1;
			}
			GameObject myLine = new GameObject();
			myLine.transform.position = gameObject.transform.position;
			myLine.AddComponent<LineRenderer>();
			LineRenderer lr = myLine.GetComponent<LineRenderer>();
			lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
			lr.SetColors(color, color);
			lr.SetWidth(0.1f, 0.05f);
			lr.SetVertexCount(vertexes+1);
			lr.SetPosition (0, gameObject.transform.position);
			nxtBlock = gameObject;
			for (int n = 1; n< vertexes; n++) {
				nxtBlock = nxtBlock.GetComponent<ConnectorScript> ().block;
				lr.SetPosition(n, nxtBlock.transform.position);
			}
			lr.SetPosition(vertexes, nxtBlock.transform.forward * nxtBlock.GetComponent<ConnectorScript>().connectRange + nxtBlock.transform.position );
			GameObject.Destroy(myLine, duration);
		}
	}
}
