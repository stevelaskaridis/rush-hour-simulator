using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapper : MonoBehaviour {

	private IBridge bridge;// = new Bridge();

	public Transform BaselReference;
	public Transform GenevaReference;
	private Vector2 _baselOld = new Vector2 (47.54741f, 7.589551f);
	private Vector2 _genevaOld = new Vector2 (46.2102f ,6.142453f);

	// Use this for initialization
	void Start ()
	{
		bridge.GetCitiesNamesAndCoords(Callback);
	}

	public void Callback(Dictionary<string, Vector2>)
	{
		Vector2 _baselReference = new Vector2 (BaselReference.transform.position.x, BaselReference.transform.position.z);
		Vector2 _genevaNew = new Vector2 (GenevaReference.transform.position.x, GenevaReference.transform.position.z);

		float scale = (_genevaNew - _baselReference).magnitude / (_genevaOld - _baselOld).magnitude;
		Vector2 offset = _genevaNew - _genevaOld;

		foreach (var station in stations)
		{
			GameObject temp = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			temp.transform.localScale = Vector3.one * 10.0f;

			var genevaOldToStationOld = station.Value - _genevaOld;
			var genevaNewToStationNew = genevaOldToStationOld * scale;

			var stationNew = _genevaNew + genevaNewToStationNew;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
