using UnityEngine;
using UnityEngine.SceneManagement;

public class Losing : MonoBehaviour {


	void Start()
	{
		stopAll();
		
	}

	public void stopAll()
	{
		return;
		GameObject.Find("PlaceWagonButton").SetActive(false);
		GameObject.Find("RunSimulation").SetActive(false);
		GameObject.Find("PlaceRailRoadButton").SetActive(false);
		
		GameObject.Find("Main Camera").GetComponent<CameraController>().Interactable = false;
	}
}
