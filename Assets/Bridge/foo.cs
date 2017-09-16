using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System;

public class foo : MonoBehaviour {
	private float x = 0;
	// Use this for initialization
	public void QueryForCities (Action<Dictionary<string, Vector2>> callback) {
		StartCoroutine (GetText (10000, 0, "asc", "dtv", callback));
	}

	IEnumerator GetText(int rowLimit, int startOffset, string sortOrder, string sortingField, Action<Dictionary<string, Vector2>> callback) {
		string sortingPrefix = "";
		switch (sortOrder.ToLower()) {
		case "asc":
			sortingPrefix = "-";
			break;
		case "desc":
			break;
		default:
			throw new KeyNotFoundException ();
			break;
		}
//		"https://data.sbb.ch/api/records/1.0/search/?dataset=passenger-frequence&rows=10000&start=0&sort=-dtv&facet=code&facet=bezugsjahr&facet=dtv&facet=dwv"
		string endpoint = string.Format ("https://data.sbb.ch/api/records/1.0/search/?dataset=passenger-frequence&rows={0}&start={1}&sort={2}{3}&facet=code&facet=bezugsjahr&facet=dtv&facet=dwv", 
			                  rowLimit.ToString (),
			                  startOffset.ToString (),
			                  sortingPrefix,
			                  sortingField
		                  );
		UnityWebRequest www = UnityWebRequest.Get(endpoint);
		yield return www.Send();

		if(www.isNetworkError) {
			Debug.Log ("logging error");
			Debug.Log(www.error);
		}
		else {
			// Show results as text
			Debug.Log ("logging success");
			var downloadedJson = www.downloadHandler.text;
//			Debug.Log(getCitiesNamesAndCoords(getFieldsFromJsonString(downloadedJson)).Count);
			var results = getCitiesNamesAndCoords (getFieldsFromJsonString (downloadedJson));

			callback (results);




//			foreach (KeyValuePair<string, Vector2> dict in ) {
//				if (dict.Key.StartsWith("Gen") || dict.Key.StartsWith("Basel"))
//					Debug.Log (string.Format ("{0}: {1},{2}", dict.Key, dict.Value.x, dict.Value.y));
//			}
//			Debug.Log(downloadedJson);
//			foreach (KeyValuePair<string, Dictionary<string, string>> dict in getFieldsFromJsonString(downloadedJson)) {
//				Debug.Log (string.Format ("Key: {0}", dict.Key));
//				foreach (KeyValuePair<string, string> entry in dict.Value) {
//					Debug.Log (string.Format("{0}: {1}", entry.Key, entry.Value));
//				}
//			}
		}
	}

	private Dictionary<string, Dictionary<string, string>> getFieldsFromJsonString(string jsonString) {
		var parsed = JSON.Parse (jsonString);
		var theDictList = new Dictionary<string, Dictionary<string, string>>();
		foreach (JSONObject record in parsed ["records"].AsArray) {
			var theDict = new Dictionary<string, string> ();
			theDict ["code"] = record ["fields"] ["code"];
			theDict["name"] = record["fields"]["bahnhof_haltestelle"];
			theDict ["dwv"] = record ["fields"] ["dwv"];
			theDict ["dtv"] = record ["fields"] ["dtv"];
			theDict ["timestamp"] = record ["record_timestamp"];
			theDict ["coord_x"] = record ["fields"] ["geopos"] [0];
			theDict ["coord_y"] = record ["fields"] ["geopos"] [1];
			theDictList [theDict ["code"]] = theDict;
		}
		return theDictList;
	}

	List<string> getCodesSortedByPopularity(string jsonString) {
		var parsed = JSON.Parse (jsonString);
		var theRankedList = new List<string>();
		foreach (JSONObject record in parsed ["records"].AsArray) {
			theRankedList.Add(record["fields"]["code"]);
		}
		return theRankedList;
	}

	string getNameForSpecificCode(Dictionary<string, Dictionary<string, string>> records, string code) {
		return records [code] ["name"];
	}

	float getNormalizedPopularityForSpecificCode(Dictionary<string, Dictionary<string, string>> records, string code) {
		int minPopularity = int.MaxValue;
		int maxPopularity = int.MinValue;
		foreach (KeyValuePair<string, Dictionary<string, string>> dict in records) {
			int currentPopularity = int.Parse(dict.Value["dwv"]);
			if (currentPopularity < minPopularity)
				minPopularity = currentPopularity;
			if (currentPopularity > maxPopularity)
				maxPopularity = currentPopularity;
		}
		return int.Parse (records [code] ["dwv"]) - (float)minPopularity / maxPopularity - minPopularity;
	}

	float[] getCoordsForSpecificCode(Dictionary<string, Dictionary<string, string>> records, string code) {
		return new float[] {float.Parse (records [code] ["coord_x"]), float.Parse (records [code] ["coord_y"])};
	}

	Dictionary<string, Vector2> getCitiesNamesAndCoords(Dictionary<string, Dictionary<string, string>> records) {
		var theDict = new Dictionary<string, Vector2> ();
		foreach (KeyValuePair<string, Dictionary<string, string>> dict in records) {
			var record = dict.Value;
			var R = 6371000;
			var x = R * Mathf.Cos (float.Parse(record ["coord_x"])) * Mathf.Cos (float.Parse(record ["coord_y"]));
			var y = R * Mathf.Cos (float.Parse(record ["coord_x"])) * Mathf.Sin (float.Parse(record ["coord_y"]));

			theDict [record["name"]] = new Vector2 (x,y);
		}
		return theDict;
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.rotation = Quaternion.EulerRotation (new Vector3 (0, Time.unscaledTime * (0.1f), 0));
		if (Input.GetKeyDown (KeyCode.Space)) {
			Debug.Log ("clicked");
		}
	}
}
