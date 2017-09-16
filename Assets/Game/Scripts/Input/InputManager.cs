using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {

	private List<Station> _stations;
	public Camera _camera;
	public Button RailTool;
	public Button WagonTool;
	public Mapper Mapper;
	public Player Player;

	public void AddStation(Station station)
	{
		_stations.Add(station);
	}

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
		existingRails = new Dictionary<uint, Rail> ();
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
				Debug.Log ("hit!");
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
		rail.AddWagon();
	}

	Dictionary<uint, Rail> existingRails;

	void CreateNewRail(Station stationFrom, Station stationTo)
	{
		// TODO: check cost

		if (stationTo.StationData.id < stationFrom.StationData.id) {
			var temp = stationTo;
			stationTo = stationFrom;
			stationFrom = temp;
		}

		uint connection = (uint)(stationTo.StationData.id) << 16 | (uint)(stationFrom.StationData.id);
		if (existingRails.ContainsKey (connection)) {
			return;
		} else {
			var railGO = GameObject.CreatePrimitive (PrimitiveType.Cube);
			railGO.name = "rail_" + stationFrom.name + "-" + stationTo.name;
			railGO.transform.position = (stationTo.StationData.Position + stationFrom.StationData.Position) * 0.5f;
			var rail = railGO.AddComponent<Rail> ();
			rail.SetEndPoints (stationFrom, stationTo);

			stationFrom.GetComponent<Renderer> ().material.color = Color.green;
			stationTo.GetComponent<Renderer> ().material.color = Color.green;

			existingRails.Add (connection, rail);

			stationFrom.connections.Add (rail);
			stationTo.connections.Add (rail);
		}
	}

	public void OnSimulate()
	{
		foreach (var station in _stations) {
			
			int nonServedClients = station.StationData.load;
			foreach (var connection in station.connections) {
				//load -= connection.capacity;
				//connection.StartSimulationRound()
			}

			if (nonServedClients < 0)
			{
				nonServedClients = 0;
			}
			int servedClients = station.StationData.load - nonServedClients;

			Player.UpdateCash (servedClients, nonServedClients);


		}
	}
}