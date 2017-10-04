using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

	static public FollowCam 	S;		// A FollowCam singleton

	// Fields set in the Unity Inspector Pane
	public float 				easing = 0.05f;
	public Vector2 minXY;
	public bool					____________________________________;

	// Fields set dynamically
	public GameObject			poi; 	// The Point of Interest
	public float 				camZ;	// The desired z position of the camera

	// Use this for initialization
	void Start () {
		S = this;
		camZ = this.transform.position.z;
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 destination;
		// If there is no poi, return to position [0, 0, 0]
		// poi is set in Slingshot.cs
		if (poi == null)
		{
			destination = Vector3.zero;
		}
		else
		{
			// Get the position of poi. poi is set from Slingshot script. It refers to the projectile
			destination = poi.transform.position;
			// If poi is a Projectile, check to see if it's at rest
			if (poi.tag == "Projectile")
			{
				// If it is sleeping (not moving)
				if (poi.GetComponent<Rigidbody> ().IsSleeping())
				{
					// return to default view
					poi = null;
					// in the next update
					return;
				}
			}
		}

		// Limit the X & Y to minimum values. This fixes the camera from following the projectile from
		// slightly behind the slingshot base when fired. 
		destination.x = Mathf.Max (minXY.x, destination.x);
		destination.y = Mathf.Max (minXY.y, destination.y);
		// Interpolate from the current Camera position toward destination
		destination = Vector3.Lerp (transform.position, destination, easing);
		// Retain a destination.z of camZ
		destination.z = camZ;
		// Set the camera to the destination
		transform.position = destination;

		// Set the orthographic Size of the Camera to keep the Ground in view
		this.GetComponent<Camera>().orthographicSize = destination.y + 10;

		
	}
}
