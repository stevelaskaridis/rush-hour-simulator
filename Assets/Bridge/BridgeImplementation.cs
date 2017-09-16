using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BridgeImplementation : IBridge {
	public void GetCitiesNamesAndCoords(Action<Dictionary<string, Vector2>> callback) {
		var go = new GameObject ();
		var bar = go.AddComponent<foo> ();
		bar.QueryForCities (callback);
	}
}
