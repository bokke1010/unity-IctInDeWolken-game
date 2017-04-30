using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Walking_script : MonoBehaviour {

	public int MaxSpeed = 50;
	public Text BlockCount;

	Transform theParent;
	Transform theChild;
	public float NewPos;

	public float forwardVel = 6;
	public float sidewaysVel = 6;

	float forwardInput;
	float sidewaysInput;

	Quaternion targetRotation;
	Vector3 velocity = Vector3.zero;
	Rigidbody rbody;

	public Quaternion TargetRotation
	{
		get { return targetRotation; }
	}

	// Use this for initialization
	void Start () {
		// FIXME this must not be red LOL
		// GameManager.instance.currentScore = BlockCount;
		theChild = GameObject.FindWithTag("Head").transform;
		theParent = gameObject.transform;
		targetRotation = transform.rotation;
		rbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		// TODO: add some working code

		forwardInput = Input.GetAxis("Vertical");
		sidewaysInput = Input.GetAxis ("Horizontal");
		//turnInput = Input.GetAxis("Horizontal");
		Turn();
	}

	void FixedUpdate()
	{
		Run ();

	}

	void Run()
	{
		if (Mathf.Abs(forwardInput) > 0)
		{
			velocity.z = forwardVel * forwardInput;
			// anim.SetBool("isWalking", true);
		}
		else
		{
			// anim.SetBool("isWalking", false);
			velocity.z = 0;
		}
		if (Mathf.Abs(sidewaysInput) > 0)
		{
			velocity.x = sidewaysVel * sidewaysInput;
			// anim.SetBool("isWalking", true);
		}
		else
		{
			// anim.SetBool("isWalking", false);
			velocity.x = 0;
		}
		rbody.velocity = transform.TransformDirection(velocity);
	}

	void Turn()
	{
		NewPos = theChild.localEulerAngles.y;
		theChild.localEulerAngles = new Vector3(theChild.localEulerAngles.x,0,theChild.localEulerAngles.z);
		theParent.localEulerAngles = new Vector3 (0, NewPos, 0);
	}
}
