using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour {

	public const int MAX_ROWS = 20;
	public const int MAX_COLUMNS = 15;
	public const int SPAWN_HEIGHT = 30;
	private readonly string BLOCK_PREFAB_PATH = "Block";
	private readonly string BLOCK_PARENT_NAME = "Blocks";

	Transform m_blockParent;
	RingBuffer<Block> m_blockRingBuffer;

	private void Awake()
	{
		m_blockParent = new GameObject(BLOCK_PARENT_NAME).transform;
		m_blockParent.transform.parent = this.transform;

		m_blockRingBuffer = new RingBuffer<Block>(MAX_ROWS * MAX_COLUMNS, Resources.Load<GameObject>(BLOCK_PREFAB_PATH), m_blockParent);
	}

	float currentTime = 0;

	// Update is called once per frame
	void Update()
	{
		currentTime += Consts.SECONDS_PER_FRAME;
		if(currentTime >= 3)
		{
			MakeRandomBlocks(3, MAX_COLUMNS);
			currentTime = 0;
		}
	}

	public void MakeRandomBlocks(int rows, int cols)
	{
		for(int r = 0; r < rows; r++)
		{
			for(int c = 0; c < cols; c++)
			{
				if(Random.Range(0f, 1f) < 0.5f)
				{
					Spawn(new Vector2(c, SPAWN_HEIGHT + r));
				}
			}
		}
	}

	#region Spawn
	public void Spawn(float x, float y)
	{
		Spawn(new Vector2(x, y));
	}

	public void Spawn(Vector2 localPosition)
	{
		Block block = m_blockRingBuffer.Get();
		block.Initialize(localPosition);
	}

	public void Spawn(Vector2[] localPositionArray)
	{
		for(int i = 0; i < localPositionArray.Length; i++)
		{
			Spawn(localPositionArray[i]);
		}
	}
	#endregion // Spawn


}
