using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System;

public class StationFetcher : MonoBehaviour
{
//	public void Start() {
//		QueryForCitiesPopularity (null);
//	}

	public void QueryForImageBinary(StationData theStation, Action<Texture2D> callback) {
		StartCoroutine(RetrieveBinaryImage(theStation, callback));
	}

	private IEnumerator RetrieveBinaryImage(StationData theStation, Action<Texture2D> callback) {

		if (theStation.imageUrl != "" && theStation.imageUrl != null) {
			var endpoint = theStation.imageUrl;
			UnityWebRequest www = UnityWebRequestTexture.GetTexture(endpoint);
			yield return www.Send();

			if (www.isNetworkError)
			{
				Debug.Log("logging error");
				Debug.Log(www.error);
			}
			else
			{
				// Show results as text
				var downloadedBinary = ((DownloadHandlerTexture)www.downloadHandler).texture;
				callback(downloadedBinary);
			}
		}
	}

	public void QueryForStationImages(Dictionary<int, StationData> data, Action<Dictionary<int, StationData>> callback) {
		StartCoroutine(AppendImageUrlsToStations(data, callback));
	}

	/**
	 * Appends the image url information to the station data passed.
	 */
	private IEnumerator AppendImageUrlsToStations(Dictionary<int, StationData> data, Action<Dictionary<int, StationData>> callback) {
		string apikey = "e2e01b388967105bd0911fc98c1d8a5cf133c853e645a4a7f5b0f761";
		string rows = "10000";
		string offset = "0";
		string sortkey = "nummer";
//		https://data.sbb.ch/api/records/1.0/search/?dataset=bilder-von-bahnhofen&rows=10000&start=0&sort=nummer
		string endpoint =
			string.Format(
				"https://data.sbb.ch/api/records/1.0/search/?apikey={0}&dataset=bilder-von-bahnhofen&rows={1}&start={2}&sort={3}", apikey, rows, offset, sortkey);
	
		UnityWebRequest www = UnityWebRequest.Get(endpoint);
		yield return www.Send();

		if (www.isNetworkError)
		{
			Debug.Log("logging error");
			Debug.Log(www.error);
		}
		else
		{

			var returnDic = new Dictionary<int, StationData>();

			var downloadedJson = www.downloadHandler.text;
			var results = extractCitiesImageUrls(downloadedJson);
			foreach (var result in results) {
				if (data.ContainsKey(result.Key)) {
					data[result.Key].imageUrl = result.Value.imageUrl;
					data[result.Key].imageWidth = result.Value.imageWidth;
					data[result.Key].imageHeight = result.Value.imageHeight;

					returnDic.Add(result.Key, data[result.Key]);
					Debug.Log("url: " + data[result.Key].imageUrl);
				}
			}
			callback(returnDic);
		}
	}

	/**
	 * Extracts the urls and returns dict of the form <id, url>
	 */
	private Dictionary<int, StationData> extractCitiesImageUrls(string jsonString) {
		var parsed = JSON.Parse(jsonString);
		var theDict = new Dictionary<int, StationData>();
		foreach (JSONObject record in parsed["records"].AsArray)
		{
			var station = new StationData ();
			var id = int.Parse(record["fields"]["nummer"]);
			var datasetid = record ["datasetid"];
			var datasetid1 = datasetid.Value.TrimStart ().TrimEnd ();
			var imageid = record ["fields"] ["file"] ["id"];
			var imageid1 = imageid.Value.TrimStart ().TrimEnd ();
			var url = string.Format("https://data.sbb.ch/explore/dataset/{0}/files/{1}/download", datasetid1, imageid1);
			station.id = id;
			station.imageUrl = url;
			station.imageWidth = record["fields"]["file"]["width"];
			station.imageHeight = record["fields"]["file"]["heigth"];
			theDict[id] = station;
		}
		return theDict;
	}

    public void QueryForCities(Action<Dictionary<int, StationData>> callback)
	{
		StartCoroutine (GetStationPositionData (callback));
    }

	public void QueryForCitiesPopularity(Action<Dictionary<int, StationData>> callback) {
		string apiKey = "e2e01b388967105bd0911fc98c1d8a5cf133c853e645a4a7f5b0f761";
		StartCoroutine (GetStationPopularityData (10000, 0, "asc", "dtv", apiKey, callback));
	}

	/**
	 * Gets cities and their positions on map and calls the callback on completion.
	 */
    IEnumerator GetStationPositionData(Action<Dictionary<int, StationData>> callback)
    {
        string apikey = "e2e01b388967105bd0911fc98c1d8a5cf133c853e645a4a7f5b0f761";
        string rows = "10000";
        string endpoint =
            string.Format(
                "https://data.sbb.ch/api/records/1.0/search/?" + "apikey="+apikey + "&" + "rows=" + rows +
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
            var results = extractCitiesCoordinates(downloadedJson);
            callback(results);
            //StartCoroutine(GetStationLoadData(10000, 0, "asc", "dtv", results, apiKey, callback));
        }
    }

	/**
	 * Method extracting coordinates for cities.
	 * Returns a Dictionary of <didok85, StationData> entries.
	 */
    private Dictionary<int, StationData> extractCitiesCoordinates(string jsonString)
    {
        var parsed = JSON.Parse(jsonString);
        var theDictList = new Dictionary<int, StationData>();
        foreach (JSONObject records in parsed["records"].AsArray)
        {
            var station = new StationData();
            station.id = records["fields"]["didok85"];
            station.name = records["fields"]["name"];

			station.Position = new Vector3(records["fields"]["y_koord_ost"], records["fields"]["x_koord_nord"], Mapper.Epsilon);
            theDictList[station.id] = station;
        }
        return theDictList;
    }

	/**
	 * Gets cities and their popularities and calls the callback on completion.
	 */
    IEnumerator GetStationPopularityData(int rowLimit, int startOffset, string sortOrder, string sortingField, string apiKey,
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
            var results = getCitiesNamesAndPopularity(getFieldsFromJsonString(downloadedJson));
//			foreach (var v in results.Values)
//				Debug.Log ("Load of " + v.id + " is "+ v.load);

            callback(results);
        }
    }

	/**
	 * Method extracting popularity for cities.
	 * Returns a Dictionary of <didok85, StationData> entries.
	 */
	Dictionary<int, StationData> getCitiesNamesAndPopularity(Dictionary<string, Dictionary<string, string>> records)
	{
		var theDict = new Dictionary<int, StationData>();
		foreach (KeyValuePair<string, Dictionary<string, string>> dict in records)
		{
			var record = dict.Value;
			var station = new StationData ();

			var lod_field = record ["lod"].Split (new char[]{'/'});
			var id = int.Parse (lod_field [lod_field.Length - 1]);

			station.id = id;
			station.load = int.Parse (record ["dtv"]);
			station.name = record ["name"];

			theDict [station.id] = station;
		}
		return theDict;
	}

	/**
	 * Joins the coordinated populated data with the popularity populated data
	 */
	public Dictionary<int, StationData> joinCitiesData(Dictionary<int, StationData> citiesWithCoords, Dictionary<int, StationData> citiesWithPopularity) {
		var lKeys = citiesWithCoords.Keys;
		var joinedCities = new Dictionary<int, StationData> ();
		foreach (var key in lKeys) {
			if (citiesWithPopularity.ContainsKey(key)) {
				joinedCities [key] = citiesWithPopularity [key];
				joinedCities [key].Position = citiesWithCoords [key].Position;
			}
		}

		return joinedCities;
	}

//	Dictionary<string, Vector2> getCitiesNamesAndCoords(Dictionary<string, Dictionary<string, string>> records, Dictionary<int, StationData> stationData)
//	{
//		var theDict = new Dictionary<string, Vector2>();
//		foreach (KeyValuePair<string, Dictionary<string, string>> dict in records)
//		{
//			var record = dict.Value;
//			theDict[record["name"]] =
//				new Vector2(float.Parse(record["coord_x"]),
//					float.Parse(record["coord_y"])); // TODO: replace 5 with load
//		}
//		return theDict;
//	}

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
			theDict["lod"] = record["fields"]["lod"];
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

    // Update is called once per frame
    void Update()
    {
//        gameObject.transform.rotation = Quaternion.EulerRotation(new Vector3(0, Time.unscaledTime * (0.1f), 0));
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("clicked");
        }
    }
}