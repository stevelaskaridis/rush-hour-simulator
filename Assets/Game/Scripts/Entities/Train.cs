using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : TransportationVehicle {

	Vector3 startPoint;
	Vector3 endPoint;
	float startTime;
	float duration = 10.0f;
	float movementStopEpsilon = 0.5f;

	// Use this for initialization
	void Start () {
		startPoint = transform.position;
		endPoint = transform.position;
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp(startPoint, endPoint, (Time.time - startTime) / duration);
	}

	// move to position with an animation over the next frames
	public void MoveToPosition(Vector3 position){
		startPoint = transform.position;
		endPoint = position;
	}

	public void TeleportToPosition(Vector3 position){
		transform.position = position;
	}
}
