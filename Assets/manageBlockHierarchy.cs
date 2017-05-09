using UnityEngine;
using System.Collections;

public class manageBlockHierarchy : MonoBehaviour {

	public PickupObject toGetObjList;
	// Update is called once per frame
	void Update () {
		ConnectorScript.findSource (toGetObjList.objects);
		// findSource ();
	}

}
