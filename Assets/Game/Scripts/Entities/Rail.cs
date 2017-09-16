	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : Track {

	Station _stationFrom;
	Station _stationTo;

	List<Train> wagons;

	public void AddWagon()
	{
		var newGO = GameObject.CreatePrimitive (PrimitiveType.Cube);
		newGO.name = "train";
		newGO.GetComponent<Renderer> ().material.color = Color.red;
		newGO.transform.position = transform.position;
		newGO.AddComponent<Train> ();
	}

	public void SetEndPoints(Station stationFrom, Station stationTo)
	{
		_stationFrom = stationFrom;
		_stationTo = stationTo;


		var fromTo = _stationTo.StationData.Position - _stationFrom.StationData.Position;
		var distance = fromTo.magnitude;
		transform.localScale = new Vector3(0.015f, 1f, distance);
		transform.position = _stationFrom.StationData.Position + (fromTo / 2f);
		transform.LookAt(_stationTo.StationData.Position);

		var material = GetComponent<Renderer> ().material;
		material= new Material(Shader.Find("Unlit/Color"));
		material.color = Color.black;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
