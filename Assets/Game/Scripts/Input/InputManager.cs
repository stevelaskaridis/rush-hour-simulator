using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InputManager : MonoBehaviour {

	public GameObject Map;
	private Dictionary<int, Station> _stations;
	public Camera _camera;
	public Button RailTool;
	public Button WagonTool;
	public Mapper Mapper;
	public Player Player;
	public Text ScoreText;

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

	void updateScore()
	{
		ScoreText.text = "your budget: " + Player.score + "$";
	}

	void CreateNewWagon(Rail rail)
	{
		const float wagonCost = 10f;

		if (Player.score < wagonCost) {
			ScoreText.color = Color.red;
			return;
		}
		Player.score -= wagonCost;
		updateScore();

		rail.AddWagon();
	}

	Dictionary<uint, Rail> existingRails;

	void CreateNewRail(Station stationFrom, Station stationTo)
	{
		

		if (stationTo.StationData.id < stationFrom.StationData.id) {
			var temp = stationTo;
			stationTo = stationFrom;
			stationFrom = temp;
		}

		uint connection = (uint)(stationTo.StationData.id) << 16 | (uint)(stationFrom.StationData.id);
		if (existingRails.ContainsKey (connection)) {
			return;
		} else {
			var distance = (stationTo.StationData.Position - stationFrom.StationData.Position).magnitude;

			const float costPerUnitDistance = 10f;
			var cost = distance * costPerUnitDistance;

			if (Player.score < cost) {
				ScoreText.color = Color.red;
				return;
			}
			ScoreText.color = Color.white;
			Player.score -= cost;
			updateScore ();

			var railGO = new GameObject("rail_" + stationFrom.name + "-" + stationTo.name);
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

	int daysCounter;

	public void OnSimulate()
	{
		daysCounter++;

		if (daysCounter % 7 == 0) {
			GetClosestStation();
		}

		foreach (var station in _stations) {
			
			int nonServedClients = station.Value.StationData.load;
			foreach (var connection in station.Value.connections) {
				//load -= connection.Capacity;
				//connection.StartSimulationRound()
			}

			if (nonServedClients < 0)
			{
				nonServedClients = 0;
			}
			int servedClients = station.Value.StationData.load - nonServedClients;

			Player.UpdateCash (servedClients, nonServedClients);

			updateScore ();
		}
	}

	void InstantiateStation(StationData station)
	{
		GameObject stationGO = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		stationGO.transform.SetParent (Map.transform);
		stationGO.name = station.name;
		stationGO.transform.localScale = Vector3.one * 5.0f;
		stationGO.transform.position = new Vector3 (station.Position.x, station.Position.y);
		var stationToAdd = stationGO.AddComponent<Station>();
		stationToAdd.StationData = station;

		_stations.Add(station.id, stationToAdd);
	}

	public void GetClosestStation()
	{
		var minDistance = Mathf.Infinity;
		StationData result = null;

		if (_stations == null) {
			_stations = new Dictionary<int, Station> ();
		}
		if (_stations.Count == 0) {

			var eligeable = Mapper.Stations.Select (i => i.Value)
				.Where (d => d.load < 10);

			var additionalStations = new List<StationData> ();
			int rand = UnityEngine.Random.Range (0, eligeable.Count());
			InstantiateStation(eligeable.ElementAt(rand));
			return;
		}

		foreach (var additionalStation in Mapper.Stations)
		{
			if (!_stations.ContainsKey(additionalStation.Value.id)) {
				foreach (var existingStation in _stations) {
					// ignore the input station itself
					var sqrDist = (additionalStation.Value.Position - existingStation.Value.StationData.Position).sqrMagnitude;
					if (sqrDist < minDistance) {
						minDistance = sqrDist;
						result = additionalStation.Value;
					}
				}
			}
		}

		InstantiateStation (result);
	}

	public void ResizeStations(float z)
	{
		foreach (var station in _stations) {
			station.Value.transform.localScale = Vector3.one * _camera.GetComponent<CameraController>().GetZoomCoefficient();
			foreach (var rail in station.Value.connections) {
				var scale = rail.transform.localScale;
				scale.y = transform.localScale.y;
				rail.transform.localScale = scale;
			}
		}
	}
}