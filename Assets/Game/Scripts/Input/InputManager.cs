using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour {

	public GameObject Map;
	private Dictionary<int, Station> _stations;
	public Camera _camera;
	public Button RailTool;
	public Button WagonTool;
	public Mapper Mapper;
	public Player Player;
	public Text ScoreText;
	public Text UserInformationText;
	public GameObject LoseScreen;
	public RawImage StationImage;
	public Text StationName;

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
		updateScore ();
		existingRails = new Dictionary<uint, Rail> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {

			if (_toolState == ToolState.RAIL) {
				RaycastHit info;
				Ray ray = _camera.ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast (ray, out info, 1 << LayerMask.NameToLayer("Station"))) {
					Debug.Log ("hit!");
					var station = info.transform.GetComponent<Station> ();
					if (station != null) {

						if (_railEndPoint == null) {
							_railEndPoint = station;
						} else if (_railEndPoint != station) {
							CreateNewRail (_railEndPoint, station);
							_railEndPoint = null;
						}
					}
				}
			} else if (_toolState == ToolState.WAGON) {

				RaycastHit info;
				Ray ray = _camera.ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast (ray, out info)) {
					Debug.Log ("hit!");
					var rail = info.transform.GetComponent<Rail> ();
					if (rail != null) {
						CreateNewWagon (rail); // this is actually unused right now, the rail does NOT have a collider anymore because it just wouldn't work..
					} else {
						// instead we use the box collider on its child, the visualization of the road to get to the rail if possible (the boxes (rail visualization)'s parent)
						rail = info.transform.parent.GetComponent<Rail> ();
						if (rail != null) {
							CreateNewWagon (rail);	
						}
					}
				}
			}
		}

		// get station imageRaycastHit info;
		RaycastHit info1;
		Ray ray1 = _camera.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray1, out info1, 1 << LayerMask.NameToLayer("Station"))) {
			Debug.Log ("hit!");
			var station = info1.transform.GetComponent<Station> ();
			if (station != null) {
				if (previouslyRendered == null || station.StationData.id != previouslyRendered.id) {
					Mapper.Bridge.GetCityImageTexture (Mapper.Stations, station.StationData, (texture) => {
						StationImage.texture = texture;
					});
				}
			} else {
				Debug.Log ("foo");
				StationImage.texture = null;
				previouslyRendered = null;
			}
		}
	}

	StationData previouslyRendered;

	void updateScore()
	{
		ScoreText.text = "Your Budget: " + (int)Player.score + ".- CHF";
	}

	void CreateNewWagon(Rail rail)
	{
		const float wagonCost = 10f;

		if (Player.score < wagonCost) { // check if enough money
			ScoreText.color = Color.red;
			UserInformationText.text = "You do not have enough money to place a train wagon.";
			return;
		}

		if (rail.CurrentNumberOfWagons() >= rail.MaxNrOfWagons()) { // also check if rail still has room for more trains
			// Debug.Log("Too many wagons already on this track, I'm not gonna put any more");
			UserInformationText.text = "Reached the maximum number of wagons on this track.";
			// have some player information here that its not possible
			return;
		}

		Player.score -= wagonCost;
		updateScore();

		UserInformationText.text = "You placed a train wagon. This costed you " + (int)wagonCost + ".- CHF.";
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

			const float costPerUnitDistance = 3f;
			var cost = distance * costPerUnitDistance;


			if (Player.score < cost) {
				ScoreText.color = Color.red;
				UserInformationText.text = "You can't build this track, you're " + (int)(cost - Player.score) + ".- CHF short on money.";
				return;
			}
			ScoreText.color = Color.green;

			Debug.Log ("player has " + Player.score + " rail costed " + cost);
			UserInformationText.text = "This track from " + stationFrom.name + " to " + stationTo.name + " costed you " + (int)cost + ".- CHF.";
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

		foreach (var station in _stations) {

			int nonServedClients = station.Value.StationData.load;

			if (nonServedClients > 0) {
				foreach (var connection in station.Value.connections) {
					//nonServedClients -= 3;
					nonServedClients -= connection.Capacity ();
					connection.StartSimulation ();
				}

				if (nonServedClients < 0) {
					nonServedClients = 0;
				}
				int servedClients = station.Value.StationData.load - nonServedClients;

				Player.UpdateCash (servedClients, nonServedClients);

				updateScore ();

				var g = servedClients / station.Value.StationData.load;
				var r = 1f - g;
				var color = new Color (r, g, 0, 1);

				station.Value.GetComponent<Renderer> ().material.color = color;
			}
		}

		if (Player.score < 0)
		{
			LoseScreen.gameObject.SetActive(true);
			LoseScreen.AddComponent<Losing>();
		}

		daysCounter++;

		GetClosestStation();
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

		ResizeStation (stationToAdd);
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
				.Where (d => d.load < 100000000);

			if(eligeable.Count() <1)
			{
				Debug.Log("do increase load acceptance for starting station");
				return;
			}

			// var additionalStations = new List<StationData> (); // what was the idea here?!
			int rand = UnityEngine.Random.Range (0, eligeable.Count()-1);
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
			ResizeStation (station.Value);
		}
	}

	public void ResizeStation(Station station)
	{
		station.transform.localScale = Vector3.one * _camera.GetComponent<CameraController> ().GetZoomCoefficient ();
		foreach (var rail in station.connections) {
			var scale = rail.transform.localScale;
			scale.y = transform.localScale.y;
			rail.transform.localScale = scale;
		}
	}
	
	public void reloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}