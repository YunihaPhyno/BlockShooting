using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInput
{
	public static bool IsInputUp()
	{
		if(Input.GetKey(KeyCode.W))
		{
			return true;
		}

		return false;
	}

	public static bool IsInputDown()
	{
		if(Input.GetKey(KeyCode.S))
		{
			return true;
		}

		return false;
	}

	public static bool IsInputLeft()
	{
		if(Input.GetKey(KeyCode.A))
		{
			return true;
		}

		return false;
	}

	public static bool IsInputRight()
	{
		if(Input.GetKey(KeyCode.D))
		{
			return true;
		}

		return false;
	}

	public static bool IsInputShoot()
	{
		if(Input.GetKey(KeyCode.Space))
		{
			return true;
		}

		return false;
	}
}
