using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : TransportationVehicle {

	Vector3 startPoint;
	Vector3 endPoint;
	float startTime;
	float duration = 10.0f;
	float movementStopEpsilon = 1f;
	bool isMoving = false;

	// Use this for initialization
	void Start () {
		startPoint = transform.position;
		endPoint = transform.position;
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (isMoving) {
			transform.position = Vector3.Lerp (startPoint, endPoint, (Time.time - startTime) / duration);
			if ((transform.position - endPoint).sqrMagnitude < movementStopEpsilon) {
				isMoving = false;
			}
		}
	}

	// move to position with an animation over the next frames
	public void MoveToPosition(Vector3 position){
		startPoint = transform.position;
		endPoint = position;
		isMoving = true;
	}

	public void TeleportToPosition(Vector3 position){
		transform.position = position;
	}
}
