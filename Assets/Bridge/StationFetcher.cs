using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System;

public class StationFetcher : MonoBehaviour
{
    // Use this for initialization
    public void QueryForCities(Action<Dictionary<int, StationData>> callback)
	{
        // TODO: get station names, ids and xy coordinates
        // create list of stationData
        // complete info with station load

        StartCoroutine(GetStationPositionData(callback));
    }

    IEnumerator GetStationPositionData(Action<Dictionary<int, StationData>> callback)
    {
        string apikey = "e2e01b388967105bd0911fc98c1d8a5cf133c853e645a4a7f5b0f761";
        string rows = "rows=" + 10000;
        string endpoint =
            string.Format(
                "https://data.sbb.ch/api/records/1.0/search/?" + "apikey="+apikey + "&" + rows +
                "&dataset=didok-liste&q=zug&lang=en&facet=nummer&facet=abkuerzung&facet=tunummer&facet=tuabkuerzung&facet=betriebspunkttyp&facet=verkehrsmittel&facet=dst_abk&facet=didok&exclude.verkehrsmittel=Bus&exclude.verkehrsmittel=Luftseilbahn&exclude.verkehrsmittel=Schiff&exclude.verkehrsmittel=Tram&exclude.verkehrsmittel=Standseilbahn&exclude.verkehrsmittel=Zahnradbahn&exclude.verkehrsmittel=Metro&exclude.verkehrsmittel=Standseilbahn_Luftseilbahn&exclude.verkehrsmittel=Standseilbahn_Bus");

        UnityWebRequest www = UnityWebRequest.Get(endpoint);
        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log("logging error");
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            var downloadedJson = www.downloadHandler.text;
//			Debug.Log(getCitiesNamesAndCoords(getFieldsFromJsonString(downloadedJson)).Count);
            var results = myFoobar(downloadedJson);
            callback(results);
            //StartCoroutine(GetStationLoadData(10000, 0, "asc", "dtv", results, apiKey, callback));

        }
    }

    private Dictionary<int, StationData> myFoobar(string jsonString)
    {
        var parsed = JSON.Parse(jsonString);
        var theDictList = new Dictionary<int, StationData>();
        foreach (JSONObject records in parsed["records"].AsArray)
        {
            var station = new StationData();
            station.id = records["fields"]["nummer"];
            station.name = records["fields"]["name"];

			station.Position = new Vector2(records["fields"]["y_koord_ost"], records["fields"]["x_koord_nord"]);
            theDictList[station.id] = station;
        }
        return theDictList;
    }

    IEnumerator GetStationLoadData(int rowLimit, int startOffset, string sortOrder,
		Dictionary<int, StationData> stationData, string sortingField, string apiKey,
        Action<Dictionary<int, StationData>> callback)
    {
        string sortingPrefix = "";
        switch (sortOrder.ToLower())
        {
            case "asc":
                sortingPrefix = "-";
                break;
            case "desc":
                break;
            default:
                throw new KeyNotFoundException();
                break;
        }

        string endpoint = string.Format(
			"https://data.sbb.ch/api/records/1.0/search/?dataset=passenger-frequence&apikey={4}&rows={0}&start={1}&sort={2}{3}&facet=code&facet=bezugsjahr&facet=dtv&facet=dwv",
            rowLimit.ToString(),
            startOffset.ToString(),
            sortingPrefix,
            sortingField,
			apiKey
        );
        UnityWebRequest www = UnityWebRequest.Get(endpoint);
        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log("logging error");
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            var downloadedJson = www.downloadHandler.text;
//			Debug.Log(getCitiesNamesAndCoords(getFieldsFromJsonString(downloadedJson)).Count);
            var results = getCitiesNamesAndCoords(getFieldsFromJsonString(downloadedJson), stationData);
            Debug.Log(downloadedJson);


            callback(null);


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

    private Dictionary<string, Dictionary<string, string>> getFieldsFromJsonString(string jsonString)
    {
        var parsed = JSON.Parse(jsonString);
        var theDictList = new Dictionary<string, Dictionary<string, string>>();
        foreach (JSONObject record in parsed["records"].AsArray)
        {
            var theDict = new Dictionary<string, string>();
            theDict["code"] = record["fields"]["code"];
            theDict["name"] = record["fields"]["bahnhof_haltestelle"];
            theDict["dwv"] = record["fields"]["dwv"];
            theDict["dtv"] = record["fields"]["dtv"];
            theDict["timestamp"] = record["record_timestamp"];
            theDict["coord_x"] = record["fields"]["x_koord_nord"];
            theDict["coord_y"] = record["fields"]["y_koord_ost"];
            theDictList[theDict["code"]] = theDict;
        }
        return theDictList;
    }

    List<string> getCodesSortedByPopularity(string jsonString)
    {
        var parsed = JSON.Parse(jsonString);
        var theRankedList = new List<string>();
        foreach (JSONObject record in parsed["records"].AsArray)
        {
            theRankedList.Add(record["fields"]["code"]);
        }
        return theRankedList;
    }

    string getNameForSpecificCode(Dictionary<string, Dictionary<string, string>> records, string code)
    {
        return records[code]["name"];
    }

    float getNormalizedPopularityForSpecificCode(Dictionary<string, Dictionary<string, string>> records, string code)
    {
        int minPopularity = int.MaxValue;
        int maxPopularity = int.MinValue;
        foreach (KeyValuePair<string, Dictionary<string, string>> dict in records)
        {
            int currentPopularity = int.Parse(dict.Value["dwv"]);
            if (currentPopularity < minPopularity)
                minPopularity = currentPopularity;
            if (currentPopularity > maxPopularity)
                maxPopularity = currentPopularity;
        }
        return int.Parse(records[code]["dwv"]) - (float) minPopularity / maxPopularity - minPopularity;
    }

    float[] getCoordsForSpecificCode(Dictionary<string, Dictionary<string, string>> records, string code)
    {
        return new float[] {float.Parse(records[code]["coord_x"]), float.Parse(records[code]["coord_y"])};
    }

    Dictionary<string, Vector2> getCitiesNamesAndCoords(Dictionary<string, Dictionary<string, string>> records, Dictionary<int, StationData> stationData)
    {
        var theDict = new Dictionary<string, Vector2>();
        foreach (KeyValuePair<string, Dictionary<string, string>> dict in records)
        {
            var record = dict.Value;
            theDict[record["name"]] =
                new Vector2(float.Parse(record["coord_x"]),
                    float.Parse(record["coord_y"])); // TODO: replace 5 with load
        }
        return theDict;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = Quaternion.EulerRotation(new Vector3(0, Time.unscaledTime * (0.1f), 0));
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("clicked");
        }
    }
}