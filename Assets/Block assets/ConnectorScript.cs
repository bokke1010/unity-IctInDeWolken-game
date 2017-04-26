using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConnectorScript : MonoBehaviour {

	public GameObject block;
	public GameObject source;
	bool attached;

	public float connectRange=4;
	public string blockFunction = "none";
	GameObject Ball;
	ConnectorScript blockScript;
	// Use this for initialization
	void Start () {
		Ball = GameObject.FindWithTag("ball");
	}
	
	// Update is called once per frame
	void Update () {
		// findSource (); // now moved to the controller script for optimization
		Connect (new Color (0, 1, 0));
		if (blockFunction == "clock" && Time.fixedTime % 2 == 0 && source == null){
			activateNext(0);
		}
	}

	void FixedUpdate() {
		
	}

	void activateNext( int cycle){
		if (attached && cycle < 10) {
			// Debug.Log (Time.fixedTime,block);
			cycle ++;
			blockScript.runCode (cycle);
		}
	}

	public bool attach(){
		Ray ray = new Ray (gameObject.transform.position, gameObject.transform.forward);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, connectRange)) {
			ConnectorScript p = hit.collider.GetComponent<ConnectorScript> ();
			if (p != null) {
				attached = true;
				block = p.gameObject;
				blockScript = p;
				return true;
			}
		}
		block = null;
		attached = false;
		return false;
	}

	void runCode(int cycle){

		switch (blockFunction) {
			case "code_block":
				Debug.Log ("code activated");
				activateNext (cycle);
				moveBall (gameObject.transform.forward * 75);
				break;
			case "connector_block":
				activateNext (cycle);
				break;
		}
	}

	void moveBall (Vector3 movement){
		Ball.GetComponent<Rigidbody> ().AddForce (movement);
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

	void Connect(Color color, float duration = 0.02f)
	{
		int vertexes = 1;
		if (source == null) {
			GameObject nxtBlock = gameObject;
			ConnectorScript nxtBlockScript = nxtBlock.GetComponent<ConnectorScript> ();
			while (nxtBlockScript.block != null) {
				nxtBlock = nxtBlockScript.block;
				nxtBlockScript = nxtBlock.GetComponent<ConnectorScript> ();
				vertexes += 1;
				if (vertexes > 10) {
					break;
				}
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
			nxtBlockScript = nxtBlock.GetComponent<ConnectorScript> ();
			for (int n = 1; n< vertexes; n++) {
				nxtBlock = nxtBlockScript.block;
				nxtBlockScript = nxtBlock.GetComponent<ConnectorScript> ();
				lr.SetPosition(n, nxtBlock.transform.position);
			}
			lr.SetPosition(vertexes, nxtBlock.transform.forward * nxtBlockScript.connectRange + nxtBlock.transform.position );
			GameObject.Destroy(myLine, duration);
		}
	}
}
