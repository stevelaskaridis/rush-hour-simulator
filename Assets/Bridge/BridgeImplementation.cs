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
		var go = new GameObject ("StationFetcher");
		var bar = go.AddComponent<StationFetcher> ();

		Dictionary<int, StationData> dic1 = null;
		Dictionary<int, StationData> dic2 = null;

		int foo = 0;
		bar.QueryForCitiesPopularity ((dic) => {
			foo++;
			dic1 = dic;
			if(foo >1)
			{
				callback2(bar,dic1, dic2, callback);
			}
		});
		bar.QueryForCities ((dic) => {
			foo++;
			dic2 = dic;
			if(foo >1)
			{
				callback2(bar, dic1, dic2, callback);
			}
		});
	}

	private void callback2(StationFetcher stationFetcher ,Dictionary<int, StationData> dic1, Dictionary<int, StationData> dic2, Action<Dictionary<int, StationData>> callback)
	{
		callback(stationFetcher.joinCitiesData (dic2, dic1));
	}
}
