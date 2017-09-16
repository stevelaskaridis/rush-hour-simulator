using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
	public bool Interactable;

	private Camera _camera;
	private float _mouseEdgeSensitivity = 1.0f;
	private float _mouseScrollSensitivity = 10.0f;
	private float _minScrollIn = -50.0f;
	private float _maxScrollOut = -1050.0f;

	// Use this for initialization
	void Start () {
		_camera = GetComponent<Camera> ();

		_camera.gameObject.AddComponent<PhysicsRaycaster>();
	}

	// Update is called once per frame
	void Update () {

		if (!Interactable) {
			return;
		}

		var pos = _camera.transform.position;

		if (Input.mousePosition.x > Screen.width - 10 && Input.mousePosition.x < Screen.width + 1) {
			pos.x += _mouseEdgeSensitivity;
		}
		else if (Input.mousePosition.x < 10 && Input.mousePosition.x > -1) {
			pos.x -= _mouseEdgeSensitivity;
		}

		if (Input.mousePosition.y > Screen.height - 10 && Input.mousePosition.y < Screen.height + 1) {
			pos.y += _mouseEdgeSensitivity;
		}
		else if (Input.mousePosition.y < 10 && Input.mousePosition.y > - 1) {
			pos.y -= _mouseEdgeSensitivity;
		}

		pos.z += _mouseScrollSensitivity * Input.mouseScrollDelta.y;

		// todo: restrict x and z
		pos.z = Mathf.Max (pos.z, _maxScrollOut);
		pos.z = Mathf.Min (pos.z, _minScrollIn);

		_camera.transform.position = pos;
	}
}