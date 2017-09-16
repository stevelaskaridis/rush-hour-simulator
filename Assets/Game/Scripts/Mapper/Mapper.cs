using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapper : MonoBehaviour {

	private IBridge bridge = new BridgeImplementation();

	public Transform BaselReference;
	public Transform GenevaReference;
	private Vector2 _baselOld = new Vector2 (-1518007f, -5606230f); 
	private Vector2 _genevaOld = new Vector2 (-3853121f, 545866.3f);

	// Use this for initialization
	void Start ()
	{
		var R = 6371000;
		var x = R * Mathf.Cos (_baselOld.x) * Mathf.Cos (_baselOld.y);
		var y = R * Mathf.Cos (_baselOld.x) * Mathf.Sin (_baselOld.y);
		Debug.Log(string.Format("Basel: {0}, {1}", x, y));

		x = R * Mathf.Cos (_genevaOld.x) * Mathf.Cos (_genevaOld.y);
		y = R * Mathf.Cos (_genevaOld.x) * Mathf.Sin (_genevaOld.y);
		Debug.Log(string.Format("Basel: {0}, {1}", x, y));


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
			GameObject temp = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			temp.transform.localScale = Vector3.one * 10.0f;

			var genevaOldToStationOld = station.Value - _genevaOld;
			var genevaNewToStationNew = genevaOldToStationOld * scale;

			var stationNew = _genevaNew + genevaNewToStationNew;
			temp.transform.position = new Vector3 (stationNew.x, 0, stationNew.y);
		}
			
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
