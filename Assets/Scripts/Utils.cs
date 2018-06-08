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

	public static T[][] CreateMatrix<T>(int rows, int cols)
	{
		T[][] mat = new T[rows][];
		for(int r = 0; r < mat.Length; r++)
		{
			mat[r] = new T[cols];
		}
		return mat;
	}

	public static void SafeDestroy(ref Object obj)
	{
		if(obj == null)
		{
			return;
		}

		GameObject.Destroy(obj);
		obj = null;
	}

	public static void SafeDestroy(ref Transform transform)
	{
		if(transform == null)
		{
			return;
		}
		GameObject.Destroy(transform.gameObject);
		transform = null;
	}
}
