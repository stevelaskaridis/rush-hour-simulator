using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBridge
{
	Dictionary<string, Vector2> GetCitiesNamesAndCoords();
}