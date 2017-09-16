using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
	// Use this for initialization
	void Start () {
		var _camera = GetComponent<Camera> ();

		_camera.gameObject.AddComponent<PhysicsRaycaster>();
	}

	// Update is called once per frame
	void Update () {

	}
}