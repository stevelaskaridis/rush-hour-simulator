using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Initialize : MonoBehaviour {

	public GameObject Map;
	public GameObject LoadingPage;
	public CameraController CameraController;

	private Mapper _mapper;

	// Use this for initialization
	void Start () {
		CameraController.Interactable = false;
		LoadingPage.SetActive(true);

		_mapper = new Mapper ();
		_mapper.InitializeMap (StartGame);
	}

	void StartGame()
	{
		LoadingPage.SetActive(false);
		CameraController.Interactable = true;

		if (_mapper.Stations.Count < 2) {
			Debug.LogWarning ("not enough train stations");
			return;
		}

		//*
		foreach (var station in _mapper.Stations) {
			GameObject stationGO = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			stationGO.transform.SetParent (Map.transform);
			stationGO.name = station.Value.name;
			stationGO.transform.localScale = Vector3.one * 5.0f;
			stationGO.transform.position = new Vector3 (station.Value.Position.x, station.Value.Position.y);
			stationGO.AddComponent<Station> ().StationData = station.Value;
		}/*/

		var eligeable = _mapper.Stations.Select (i => i.Value)
			.Where (d => d.load < 10);

		var additionalStations = new List<StationData> ();
		int rand = UnityEngine.Random.Range (0, eligeable.Count());
		additionalStations.Add(eligeable.ElementAt(rand));

		int numberOfAddStations = 1;
		for (int i = 0; i < numberOfAddStations; i++) {
			additionalStations.Add (GetClosestStation (additionalStations));
		}

		foreach (var additionalStation in additionalStations) {
			InstantiateStation (additionalStation);
		}
		//*/
	}

	GameObject InstantiateStation(StationData station)
	{
		GameObject stationGO = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		stationGO.transform.SetParent (Map.transform);
		stationGO.name = station.name;
		stationGO.transform.localScale = Vector3.one * 5.0f;
		stationGO.transform.position = new Vector3 (station.Position.x, station.Position.y);
		return stationGO;
	}

	StationData GetClosestStation(List<StationData> existingStations)
	{
		var minDistance = Mathf.Infinity;
		StationData result = null;
		foreach (var additionalStation in _mapper.Stations)
		{
			foreach (var existingStation in existingStations) {
				// ignore the input station itself
				if (additionalStation.Value != existingStation) {
					var sqrDist = (additionalStation.Value.Position - existingStation.Position).sqrMagnitude;
					if (sqrDist < minDistance) {
						minDistance = sqrDist;
						result = additionalStation.Value;
					}
				}
			}
		}

		return result;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
