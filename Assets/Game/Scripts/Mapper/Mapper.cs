using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mapper
{
	public Dictionary<int, StationData> Stations;

	private Vector2 _baselNew = new Vector2 (-137.9f, 284f);
	private Vector2 _genevaNew = new Vector2 (-529f, -226.8f);

	// TODO update these
	private Vector2 _baselOld = new Vector2 (7.589551f, 47.54741f);
	private Vector2 _genevaOld = new Vector2 (6.142453f, 46.2102f);
	float _scale;
	Vector2 _offset;

	public Mapper()
	{
		_scale = (_genevaNew - _baselNew).magnitude / (_genevaOld - _baselOld).magnitude;
		_offset = _genevaNew - _genevaOld;
	}

	// Use this for initialization
	public void InitializeMap (Action StartGame)
	{
		IBridge bridge = new BridgeImplementation();
		bridge.GetCitiesNamesAndCoords
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
			station.Value.Position = new Vector3 (stationNew.x, 0, stationNew.y);
		}
			
	}
}
