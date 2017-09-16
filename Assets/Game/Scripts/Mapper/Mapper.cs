using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapper : MonoBehaviour {

	private IBridge bridge = new BridgeImplementation();

	public Transform BaselNew;
	public Transform GenevaNew;
	private Vector2 _baselOld = new Vector2 (7.589551f, 47.54741f);
	private Vector2 _genevaOld = new Vector2 (6.142453f, 46.2102f);

	// Use this for initialization
	void Start ()
	{
		bridge.GetCitiesNamesAndCoords(Callback);
	}

	public void Callback(Dictionary<int, StationData> stations)
	{
		Vector2 _baselReference = new Vector2 (BaselNew.transform.position.x, BaselNew.transform.position.z);
		Vector2 _genevaNew = new Vector2 (GenevaNew.transform.position.x, GenevaNew.transform.position.z);

		float scale = (_genevaNew - _baselReference).magnitude / (_genevaOld - _baselOld).magnitude;
		Vector2 offset = _genevaNew - _genevaOld;

		foreach (var station in stations)
		{
			GameObject temp = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			temp.transform.localScale = Vector3.one * 10.0f;

			var genevaOldToStationOld = station.Value.Position - _genevaOld;
			var genevaNewToStationNew = genevaOldToStationOld * scale;

			var stationNew = _genevaNew + genevaNewToStationNew;
			temp.transform.position = new Vector3 (stationNew.x, 0, stationNew.y);
		}
			
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
