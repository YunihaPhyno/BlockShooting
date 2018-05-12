using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug : MonoBehaviour {

	public bool spawnFlag = false;
	public GameObject spawnTargetObject;
	public string spawnInvokeMethod = "SpawnRandomBlocks";

	private void Update()
	{
		Spawn();
	}

	private void Spawn()
	{
		if(!spawnFlag)
		{
			return;
		}
		spawnTargetObject.SendMessage(spawnInvokeMethod);

		spawnFlag = false;
	}
}
