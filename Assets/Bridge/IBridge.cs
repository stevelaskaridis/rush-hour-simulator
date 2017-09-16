using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IBridge
{
	Vector2 To2DCoord (Vector2 input);
	void GetCitiesNamesAndCoords(Action<Dictionary<string, Vector2>> callback);
}