using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float score = 100;

	public void  UpdateCash(int served, int nonServed)
	{
		const float cashPerServedClient = 10;
		const float lossPerClient = 5;
		float cash = cashPerServedClient * served - lossPerClient * nonServed;
		score += cash;
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
