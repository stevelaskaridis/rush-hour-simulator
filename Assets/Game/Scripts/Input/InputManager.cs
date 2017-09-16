using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
	private Camera _camera;
	private float _mouseEdgeSensitivity = 0.1f;
	private float _mouseScrollSensitivity = 10.0f;
	private float _minScrollIn = 100.0f;
	private float _maxScrollOut = 650.0f;

	// Use this for initialization
	void Start () {
		_camera = GetComponent<Camera> ();

		_camera.gameObject.AddComponent<PhysicsRaycaster>();
	}

	// Update is called once per frame
	void Update () {
		var pos = _camera.transform.position;

		if (Input.mousePosition.x > Screen.width - 10 && Input.mousePosition.x < Screen.width + 1) {
			pos.x += _mouseEdgeSensitivity;
		}
		else if (Input.mousePosition.x < 10 && Input.mousePosition.y > -1) {
			pos.x -= _mouseEdgeSensitivity;
		}

		if (Input.mousePosition.y > Screen.height - 10 && Input.mousePosition.y < Screen.height + 1) {
			pos.z += _mouseEdgeSensitivity;
		}
		else if (Input.mousePosition.y < 10 && Input.mousePosition.y > - 1) {
			pos.z -= _mouseEdgeSensitivity;
		}

		pos.y += _mouseScrollSensitivity * Input.mouseScrollDelta.y;

		// todo: restrict x and z
		pos.y = Mathf.Min (pos.y, _maxScrollOut);
		pos.y = Mathf.Max (pos.y, _minScrollIn);

		_camera.transform.position = pos;
	}
}