using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IBridge
{
	void GetCitiesNamesAndCoords(Action<Dictionary<int,StationData>> callback);
}