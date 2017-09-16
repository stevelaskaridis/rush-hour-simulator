using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapper : MonoBehaviour {

	private IBridge bridge = new BridgeImplementation();

	public Transform BaselReference;
	public Transform GenevaReference;
	private Vector2 _baselOld = new Vector2 (7.589551f, 47.54741f);
	private Vector2 _genevaOld = new Vector2 (6.142453f, 46.2102f);

	// Use this for initialization
	void Start ()
	{
		bridge.GetCitiesNamesAndCoords(Callback);
		Debug.Log ("query_started");
	}

	public void Callback(Dictionary<string, Vector2> stations)
	{
		Debug.Log ("callback_started");
		Vector2 _baselReference = new Vector2 (BaselReference.transform.position.x, BaselReference.transform.position.z);
		Vector2 _genevaNew = new Vector2 (GenevaReference.transform.position.x, GenevaReference.transform.position.z);

		float scale = (_genevaNew - _baselReference).magnitude / (_genevaOld - _baselOld).magnitude;
		Vector2 offset = _genevaNew - _genevaOld;

		foreach (var station in stations)
		{
			var stationn = station.Value;
			//var stationn = bridge.To2DCoord (station.Value);
			float tmp=stationn.x;
			stationn.x=stationn.y;
			stationn.y=tmp;
			GameObject temp = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			temp.transform.localScale = Vector3.one * 10.0f;

			var genevaOldToStationOld = stationn - _genevaOld;
			var genevaNewToStationNew = genevaOldToStationOld * scale;

			var stationNew = _genevaNew + genevaNewToStationNew;
			temp.transform.position = new Vector3 (stationNew.x, 0, stationNew.y);
		}
			
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
