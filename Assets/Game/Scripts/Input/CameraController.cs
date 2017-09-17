using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
	public InputManager InputManager;
	public bool Interactable;

	private Camera _camera;
	private float _mouseEdgeSensitivity = 2.0f;
	private float _mouseScrollSensitivity = 5.0f;
	private float _minScrollIn = -10.0f;
	private float _maxScrollOut = -800.0f;
	const float screenBorder = 30.0f;

	// Use this for initialization
	void Start () {
		_camera = GetComponent<Camera> ();

		_camera.gameObject.AddComponent<PhysicsRaycaster>();
	}

	public float GetZoomCoefficient()
	{
		return (- _camera.transform.position.z / 40);
	}

	// Update is called once per frame
	void Update () {

		if (!Interactable) {
			return;
		}

		var pos = _camera.transform.position;

		if (Input.mousePosition.x > Screen.width - screenBorder && Input.mousePosition.x < Screen.width + 1) {
			pos.x += _mouseEdgeSensitivity * GetZoomCoefficient() * (Input.mousePosition.x - (Screen.width - screenBorder))/screenBorder;
		}
		else if (Input.mousePosition.x < screenBorder && Input.mousePosition.x > -1) {
			pos.x -= _mouseEdgeSensitivity * GetZoomCoefficient() * (screenBorder - Input.mousePosition.x) / screenBorder;
		}

		if (Input.mousePosition.y > Screen.height - screenBorder && Input.mousePosition.y < Screen.height + 1) {
			pos.y += _mouseEdgeSensitivity * GetZoomCoefficient() * (Input.mousePosition.y - (Screen.height - screenBorder))/screenBorder;
		}
		else if (Input.mousePosition.y < screenBorder && Input.mousePosition.y > - 1) {
			pos.y -= _mouseEdgeSensitivity * GetZoomCoefficient() * (screenBorder - Input.mousePosition.y) / screenBorder;
		}
		pos.z += _mouseScrollSensitivity * Input.mouseScrollDelta.y * GetZoomCoefficient();

		// todo: restrict x and z
		pos.x = Mathf.Max (pos.x, -0.5f * Screen.width);
		pos.x = Mathf.Min (pos.x, 0.5f * Screen.width);
		pos.y = Mathf.Max (pos.y, -0.5f * Screen.height);
		pos.y = Mathf.Min (pos.y, 0.5f * Screen.height);
		pos.z = Mathf.Max (pos.z, _maxScrollOut);
		pos.z = Mathf.Min (pos.z, _minScrollIn);

		_camera.transform.position = pos;

		if (Mathf.Abs(Input.mouseScrollDelta.y) > 0) {
			InputManager.ResizeStations (pos.z);
		}
	}
}