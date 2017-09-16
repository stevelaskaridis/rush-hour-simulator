using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationData
{
	public string name;
	public int id;
	public int load;
	public Vector3 Position;
	public string imageUrl;
	public int imageWidth;
	public int imageHeight;
}

public class Station : MonoBehaviour {


	public void DisplayCashAnimation(int served, int nonServed)
	{
		//  spawn a particle-like thing
	}

	public StationData StationData;
	public List<Rail> connections = new List<Rail> ();

	// Use this for initialization
	void Start () {
		gameObject.layer=LayerMask.NameToLayer("Station");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
