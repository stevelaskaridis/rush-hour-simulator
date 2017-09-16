using UnityEngine;
using UnityEngine.SceneManagement;

public class Losing : MonoBehaviour {


	void Start()
	{
		stopAll();
		
	}

	public void stopAll()
	{
		GameObject go = GameObject.Find("VerticalLayout");
		go.SetActive(false);
		
		go = GameObject.Find("Main Camera");
		go.GetComponent<CameraController>().Interactable = false;
		
	}

}
