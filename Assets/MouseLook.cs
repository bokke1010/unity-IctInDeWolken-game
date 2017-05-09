using UnityEngine;
using System.Collections;
using UnityEngine.VR;


/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation


/// To make an FPS style character:
/// - Create a capsule.
/// - Add a rigid body to the capsule
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSWalker script to the capsule


/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {


	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	public float VRsensitivityX = 1F;
	public float VRsensitivityY = 1F;
	public float minimumX = -360F;
	public float maximumX = 360F;
	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationX = 0F;
	float rotationY = 0F;

	Quaternion originalRotation;
	Quaternion originalVRRotation;

	public float maxVrOffset = 0.5f;
	Vector3 baseOffset;
	Vector3 basePos;

	void Update ()
	{
		// Read the mouse input axis
		rotationX += Input.GetAxis("Mouse X") * sensitivityX;
		rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
		rotationX = ClampAngle (rotationX, minimumX, maximumX);
		rotationY = ClampAngle (rotationY, minimumY, maximumY);
		Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
		Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, -Vector3.right);
		transform.localRotation = originalRotation * xQuaternion * yQuaternion * VRRotation();
		gameObject.transform.localPosition = basePos - Vector3.ClampMagnitude (InputTracking.GetLocalPosition(VRNode.Head) - baseOffset, maxVrOffset);
	}
	void Start ()
	{
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
		originalRotation = transform.localRotation;
		originalVRRotation = InputTracking.GetLocalRotation(VRNode.Head);
		basePos = this.transform.localPosition;
		baseOffset = InputTracking.GetLocalPosition(VRNode.Head);
	}
	public static float ClampAngle (float angle, float min, float max)
	{
		if(angle < -360F)
			angle += 360F;
		if(angle > 360F)
			angle -= 360F;
		return Mathf.Clamp (angle, min, max);
	}

	Quaternion VRRotation(){
		Quaternion absoluteRotation = UnityEngine.VR.InputTracking.GetLocalRotation (VRNode.Head);
		return originalVRRotation * Quaternion.Inverse( absoluteRotation);
	}

}