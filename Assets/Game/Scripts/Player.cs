using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float score;

	public void  UpdateCash(int served, int nonServed)
	{
		const float cashPerServedClient = 30;
		const float lossPerClient = 1;
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
