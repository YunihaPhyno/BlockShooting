using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
	public class Block : MonoBehaviour
	{
		// Use this for initialization
		void Start()
		{
			gameObject.GetComponent<Renderer>().material.color = Color.green;
		}
	}
}