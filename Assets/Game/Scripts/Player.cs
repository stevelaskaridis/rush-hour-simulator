using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float score = 100;

	public void  UpdateCash(int served, int nonServed)
	{

		const float cachePerServedClient = 10;

		score += cachePerServedClient * served;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
