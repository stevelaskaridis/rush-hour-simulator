using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : Track {

	Station _stationFrom;
	Station _stationTo;

	List<Train> wagons1;
	List<Train> wagons2;
	// Vector2 wagonSize = new Vector2(1f,1f); // should have ratio 1:1 (x:y)
	public GameObject trainObject;

	float wagonXDimension = 1;
	float wagonScale = 1;

	Vector3 wagonStartPos1;
	Vector3 wagonStartPos2;

	public int capacityPerWagon = 1000;

	public void AddWagon()
	{	
		// new wagon for wagons1
		GameObject newGO = (Instantiate(Resources.Load("TrainPrefab"))) as GameObject;
		newGO.name = "train";
		newGO.layer = SortingLayer.NameToID ("Train");
		//newGO.transform.localScale = new Vector3 (wagonSize.x,wagonSize.y,1);
		//newGO.transform.parent = transform;
		wagonXDimension = newGO.transform.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
		wagonScale = newGO.transform.localScale.x;

		Vector3 newPos = wagonStartPos1 + new Vector3(0,0,-0.5f);
		newGO.transform.position = newPos;
		newGO.transform.rotation = GetProperRotationToForwardVector(wagonStartPos2, wagonStartPos1);
		wagons1.Add(newGO.GetComponent<Train>());

		// new wagon for wagons2
		GameObject newGO2 = (Instantiate(Resources.Load("TrainPrefab"))) as GameObject;
		newGO2.name = "train";
		//newGO2.transform.parent = transform;
		//newGO2.transform.localScale = new Vector3 (wagonSize.x,wagonSize.y,1);
		newPos = wagonStartPos2 + new Vector3(0,0,-0.5f);
		newGO2.transform.position = newPos;
		newGO2.transform.rotation = GetProperRotationToForwardVector(wagonStartPos1, wagonStartPos2);
		wagons2.Add(newGO2.GetComponent<Train>());

		// reset the wagon positions
		ResetWagonPositions ();
	}

	Quaternion GetProperRotationToForwardVector(Vector3 targetLocation, Vector3 startLocation){
		Vector3 diff = targetLocation-startLocation;
		diff.Normalize();
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		return Quaternion.Euler(0f, 0f, rot_z + 180);
	}

	public int Capacity(){
		return wagons1.Count * capacityPerWagon; // wagons1 and wagons2 have same length
	}

	public int CurrentNumberOfWagons(){
		return wagons1.Count; // wagons1 and wagons2 have same length
	}

	// the maximum number of wagons this track allows based on the track length
	public int MaxNrOfWagons(){
		// return some really smart calculations <3
		int maxNrWagons = Mathf.CeilToInt((_stationTo.StationData.load + _stationFrom.StationData.load) / capacityPerWagon);
		return maxNrWagons;
	}

	public void StartSimulation(){
		ResetWagonPositions ();
		/// wagons1
		// start moving the wagons to the other side of the rail
		Vector3 positionIncrementPerWagon = (wagonStartPos2 - wagonStartPos1).normalized * (wagonXDimension*wagonScale);
		// start from endpos and then subtract from it for each wagon which is later in the list
		Vector3 endWagonPosition = wagonStartPos2 - positionIncrementPerWagon * wagons1.Count; // looks better in the long run if it actually reaches the station
		foreach (var wagon in wagons1){
			endWagonPosition.z = -0.5f;
			wagon.MoveToPosition(endWagonPosition);
			endWagonPosition = endWagonPosition + positionIncrementPerWagon;
		}

		/// wagons2
		// start moving the wagons to the other side of the rail
		positionIncrementPerWagon = (wagonStartPos1 - wagonStartPos2).normalized * (wagonXDimension*wagonScale);
		// start from endpos and then subtract from it for each wagon which is later in the list
		endWagonPosition = wagonStartPos1 - positionIncrementPerWagon * wagons2.Count; // looks better in the long run if it actually reaches the station
		foreach (var wagon in wagons2){
			endWagonPosition.z = -0.5f;
			wagon.MoveToPosition(endWagonPosition);
			endWagonPosition = endWagonPosition + positionIncrementPerWagon;
		}
	}

	public void ResetWagonPositions(){
		// reset all wagons to initial positions
		/// wagons1
		Vector3 currentWagonPosition = wagonStartPos1; // start from startpos and then add onto it for each wagon
		Vector3 positionIncrementPerWagon = (wagonStartPos2 - wagonStartPos1).normalized * (wagonXDimension*wagonScale);
		foreach (var wagon in wagons1){
			currentWagonPosition.z = -0.5f;
			wagon.TeleportToPosition (currentWagonPosition);
			currentWagonPosition = currentWagonPosition + positionIncrementPerWagon;
		}

		/// wagons2
		currentWagonPosition = wagonStartPos2; // start from startpos and then add onto it for each wagon
		positionIncrementPerWagon = (wagonStartPos1 - wagonStartPos2).normalized * (wagonXDimension*wagonScale);
		foreach (var wagon in wagons2){
			currentWagonPosition.z = -0.5f;
			wagon.TeleportToPosition (currentWagonPosition);
			currentWagonPosition = currentWagonPosition + positionIncrementPerWagon;
		}
	}

	public void SetEndPoints(Station stationFrom, Station stationTo)
	{
		var railGO = GameObject.CreatePrimitive (PrimitiveType.Cube);
		gameObject.layer = SortingLayer.NameToID ("Rail");
		railGO.name = "view";
		railGO.layer = SortingLayer.NameToID ("RailView");
		railGO.transform.SetParent (transform);
		railGO.transform.position = (stationTo.StationData.Position + stationFrom.StationData.Position) * 0.5f;

		_stationFrom = stationFrom;
		_stationTo = stationTo;

		var fromTo = _stationTo.StationData.Position - _stationFrom.StationData.Position;
		var distance = fromTo.magnitude;
		railGO.transform.localScale = new Vector3(0.015f, 1f, distance);
		railGO.transform.position = _stationFrom.StationData.Position + (fromTo / 2f);
		railGO.transform.LookAt(_stationTo.StationData.Position);

		var material = railGO.GetComponent<Renderer> ().material;
		material= new Material(Shader.Find("Unlit/Color"));
		material.color = Color.black;

		/*
		// add collider from visualization also to the rail
		var collider = gameObject.AddComponent<BoxCollider> ();
		var railCol = railGO.GetComponent<BoxCollider> ();

		//var bounds = GetComponent<BoxCollider> ().bounds;
		//collider.center = Vector3.zero;
		//collider.size = railCol.bounds.size;
		

		collider.transform.SetPositionAndRotation(railCol.transform.position, railCol.transform.rotation);
		collider.transform.localScale = railCol.transform.localScale;
		railCol.transform.position += new Vector3 (0, 0, 200);
		//GameObject.DestroyImmediate (railCol);
		//*/

		// start at two wagons offset from the station
		Vector3 initialWagonOffsetFromStations = new Vector3(0,0,0);//(_stationFrom.StationData.Position - _stationTo.StationData.Position).normalized * wagonSize.x * 2;
		wagonStartPos1 = _stationFrom.StationData.Position + initialWagonOffsetFromStations;
		wagonStartPos2 = _stationTo.StationData.Position - initialWagonOffsetFromStations;
	}

	// Use this for initialization
	void Start () {
		wagons1 = new List<Train> ();
		wagons2 = new List<Train> ();
	}
	
	// Update is called once per frame
	void Update () {

	}
}
