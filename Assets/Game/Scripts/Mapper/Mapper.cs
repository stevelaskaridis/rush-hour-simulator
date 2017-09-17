using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mapper
{
	static public float Epsilon = -0.1f;

	public Dictionary<int, StationData> Stations;
	public IBridge Bridge;

	private Vector3 _baselNew = new Vector3 (-130.3f, 275.1f, Epsilon);
	private Vector3 _genevaNew = new Vector3 (-529f, -226.8f, Epsilon);

	// TODO update these
	private Vector3 _baselOld = new Vector3 (612173.0f, 266858.0f, Epsilon);
	private Vector3 _genevaOld = new Vector3 (499744.0f, 117961.0f, Epsilon);
	float _scale;

	public Mapper()
	{
		_scale = (_genevaNew - _baselNew).magnitude / (_genevaOld - _baselOld).magnitude;
	}

	// Use this for initialization
	public void InitializeMap (Action StartGame)
	{
		Bridge = new BridgeImplementation();
		Bridge.GetCitiesNamesAndCoords
		((stations) => {
			Stations = stations;
			MapStations();
			StartGame ();
		});
	}

	public void MapStations()
	{
		foreach (var station in Stations)
		{
			var genevaOldToStationOld = station.Value.Position - _genevaOld;
			var genevaNewToStationNew = genevaOldToStationOld * _scale;

			var stationNew = _genevaNew + genevaNewToStationNew;
			station.Value.Position = stationNew;
		}
			
	}
}
