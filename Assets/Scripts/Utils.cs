using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
	public static bool TryGetComponent<T>(GameObject obj, out T component) where T : Component
	{
		component = obj.GetComponent<T>();
		if(component != null)
		{
			return true;
		}

		component = obj.GetComponentInParent<T>();
		if(component != null)
		{
			return true;
		}

		component = obj.GetComponentInChildren<T>();
		if(component != null)
		{
			return true;
		}

		return false;
	}
}
