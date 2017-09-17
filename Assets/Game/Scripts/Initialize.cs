using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Initialize : MonoBehaviour {

	public GameObject LoadingPage;
	public CameraController CameraController;
	public InputManager InputManager;
	public Text LoadingScreenMessage;
	int loadingScreenMessageState = 0;
	float timeBeforeMessageChange = 0.35f;
	float lastUpdateTime;

	private Mapper _mapper;

	// Use this for initialization
	void Start () {
		CameraController.Interactable = false;
		LoadingPage.SetActive(true);

		_mapper = new Mapper ();
		_mapper.InitializeMap (StartGame);

		lastUpdateTime = Time.time;
	}

	void StartGame()
	{
		LoadingPage.SetActive(false);
		CameraController.Interactable = true;

		if (_mapper.Stations.Count < 2) {
			Debug.LogWarning ("not enough train stations");
			return;
		}

		/*
		foreach (var station in _mapper.Stations) {
			GameObject stationGO = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			stationGO.transform.SetParent (Map.transform);
			stationGO.name = station.Value.name;
			stationGO.transform.localScale = Vector3.one * 5.0f;
			stationGO.transform.position = new Vector3 (station.Value.Position.x, station.Value.Position.y);
			var stationToAdd = stationGO.AddComponent<Station> ();
			stationToAdd.StationData = station.Value;

			InputManager.AddStation(stationToAdd);
		}
		InputManager.Mapper = _mapper;
		/*/

		InputManager.Mapper = _mapper;

		int numberOfAddStations = 4;
		for (int i = 0; i < numberOfAddStations; i++) {
			InputManager.GetClosestStation ();
		}
			
		//*/
	}
	
	// Update is called once per frame
	void Update () {
		if ((Time.time - lastUpdateTime) > timeBeforeMessageChange) {
			loadingScreenMessageState += 1;
			lastUpdateTime = Time.time;
			if (loadingScreenMessageState > 3) {
				loadingScreenMessageState = 0;
				LoadingScreenMessage.text = "Loading";
			}else{
				LoadingScreenMessage.text = LoadingScreenMessage.text + ".";
			}
		}
	}
}
