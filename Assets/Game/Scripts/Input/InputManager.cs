using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	private enum ToolState
	{
		NONE,
		RAIL,
		WAGON
	}

	private ToolState _toolState;

	private Station _railEndPoint;

	private void CancleTool()
	{
		_railEndPoint = null;
	}

	public void OnSelectRailTool(bool enabled)
	{
		_toolState = ToolState.NONE;
		if (enabled) {
			_toolState = ToolState.RAIL;
		}
	}

	public void OnSelectWagonTool()
	{
		_toolState = ToolState.WAGON;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
