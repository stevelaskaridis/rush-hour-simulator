using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {

	public Camera _camera;
	public Toggle RailTool;
	public Toggle WagonTool;

	private enum ToolState
	{
		NONE,
		RAIL,
		WAGON
	}

	private ToolState _toolState;

	private Station _railEndPoint;

	private void CancleTool()
	{
		_toolState = ToolState.NONE;
		_railEndPoint = null;
	}

	public void OnSelectRailTool()
	{
		CancleTool ();
		WagonTool.isOn = false;
		if (RailTool.isOn) {
			_toolState = ToolState.RAIL;
		}
	}

	public void OnSelectWagonTool()
	{
		CancleTool ();
		RailTool.isOn = false;
		if (WagonTool.isOn) {
			_toolState = ToolState.WAGON;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(_toolState == ToolState.NONE) {
			return;
		}
			
		if (Input.GetMouseButtonDown (0)) {

			RaycastHit info;
			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast (ray, out info)) {
				
				if (_toolState == ToolState.RAIL) {
					var station = info.transform.GetComponent<Station> ();
					if (station != null) {

						if (_railEndPoint == null) {
							_railEndPoint = station;
						} else {
							// create new path
						}
					}
				} else if (_toolState == ToolState.WAGON) {
					var rail = info.transform.GetComponent<Rail> ();
					if (rail != null) {
						// create wagon
					}
				}
			}
		}
	}

	void CreateNewRail(Station stationFrom, Station stationTo)
	{
		if (stationTo.StationData.id < stationFrom.StationData.id) {
			var temp = stationTo;
			stationTo = stationFrom;
			stationFrom = temp;
		}

		// TODO: if not already existing

		var newGO = GameObject.CreatePrimitive (PrimitiveType.Cube);
		newGO.GetComponent<Renderer> ().material.color = Color.green;
		newGO.transform.position = (stationTo.StationData.Position + stationFrom.StationData.Position) * 0.5f;
	}
}
