using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BridgeImplementation : IBridge {

	StationFetcher _stationFetcherBuilder;
	private StationFetcher _stationFetcher
	{
		get{
			if (_stationFetcherBuilder == null) {
				var go = new GameObject ("StationFetcher");
				_stationFetcherBuilder = go.AddComponent<StationFetcher> ();
			}
			return _stationFetcherBuilder;	
		}
		set
		{
			_stationFetcherBuilder = value;
		}
	}

	public void GetCityImageTexture (Dictionary<int,StationData> stations, StationData station, Action<Texture2D> callback)
	{
		_stationFetcher.QueryForImageBinary(station, callback);
	}

	public void GetCitiesNamesAndCoords(Action<Dictionary<int, StationData>> callback) {

		Dictionary<int, StationData> dic1 = null;
		Dictionary<int, StationData> dic2 = null;

		int foo = 0;
		_stationFetcher.QueryForCitiesPopularity ((dic) => {
			foo++;
			dic1 = dic;
			if(foo >1)
			{
				callback2(dic1, dic2, callback);
			}
		});
		_stationFetcher.QueryForCities ((dic) => {
			foo++;
			dic2 = dic;
			if(foo >1)
			{
				callback2(dic1, dic2, callback);
			}
		});
	}

	private void callback2(Dictionary<int, StationData> dic1, Dictionary<int, StationData> dic2, Action<Dictionary<int, StationData>> callback)
	{
		var result = _stationFetcher.joinCitiesData (dic2, dic1);
		_stationFetcher.QueryForStationImages (result, (res) => callback(res));
	}
}
