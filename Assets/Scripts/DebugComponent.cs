using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugComponent : MonoBehaviour {

	[SerializeField]
	BlockManager m_blockManager;

	[SerializeField]
	Vector2[] m_spawnPositions;

	[SerializeField]
	bool m_doneFlag = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!m_doneFlag)
		{
			return;
		}

		if(m_spawnPositions == null || m_spawnPositions.Length <= 0)
		{
			return;
		}

		if(m_blockManager == null)
		{
			return;
		}

		m_blockManager.Spawn(m_spawnPositions);

		m_doneFlag = false;
	}
}
