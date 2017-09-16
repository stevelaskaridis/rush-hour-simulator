using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BridgeImplementation : IBridge {

	public Vector2 To2DCoord(Vector2 input)
	{

		var R = 6371000;
		var x = R * Mathf.Cos (input.x) * Mathf.Cos (input.y);
		var y = R * Mathf.Cos (input.x) * Mathf.Sin (input.y);
		//z=...
		return new Vector2 (x, y);

	}

	public void GetCitiesNamesAndCoords(Action<Dictionary<int, StationData>> callback) {
		var go = new GameObject ();
		var bar = go.AddComponent<StationFetcher> ();
		bar.QueryForCities (callback);
	}
}
