using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {

	public Camera _camera;
	public Button RailTool;
	public Button WagonTool;

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
		// unhighlight both button
		_railEndPoint = null;
	}

	public void OnSelectRailTool()
	{
		CancleTool ();
		// todo: highlight button
		_toolState = ToolState.RAIL;
	}

	public void OnSelectWagonTool()
	{
		CancleTool ();
		// todo: highlight button
		_toolState = ToolState.WAGON;
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
						} else if (_railEndPoint != station){
							CreateNewRail (_railEndPoint, station);
							_railEndPoint = null;
						}
					}
				} else if (_toolState == ToolState.WAGON) {
					var rail = info.transform.GetComponent<Rail> ();
					if (rail != null) {
						CreateNewWagon (rail);
					}
				}
			}
		}
	}

	void CreateNewWagon(Rail rail)
	{
		// TODO: check cost

		var newGO = GameObject.CreatePrimitive (PrimitiveType.Cube);
		newGO.GetComponent<Renderer> ().material.color = Color.red;
		newGO.transform.position = rail.transform.position;
	}

	void CreateNewRail(Station stationFrom, Station stationTo)
	{
		// TODO: check cost

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
