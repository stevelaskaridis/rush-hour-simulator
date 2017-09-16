using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationData
{
	public string name;
	public int id;
	public int load;
	public Vector3 Position;
}

public class Station : MonoBehaviour {


	public void DisplayCashAnimation(int served, int nonServed)
	{
		//  spawn a particle-like thing
	}

	public StationData StationData;
	public List<Rail> connections;

	// Use this for initialization
	void Start () {
		StationData.load = 2;
		connections = new List<Rail> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
