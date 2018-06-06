﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
	public class BlockManager : ObjectManagerBase<Block>
	{
		public const int MAX_ROWS = 20;
		public const int MAX_COLUMNS = 15;
		public const int SPAWN_HEIGHT = 30;
		private readonly string BLOCK_PREFAB_PATH = "Prefabs/Ingame/Block";
		private readonly string BLOCK_PARENT_OBJECT_NAME = "Blocks";

		protected override int GetBufferSize()
		{
			return MAX_ROWS * MAX_COLUMNS;
		}

		protected override string GetObjectPrefabPath()
		{
			return BLOCK_PREFAB_PATH;
		}

		protected override string GetParentObjectName()
		{
			return BLOCK_PARENT_OBJECT_NAME;
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

		public void Spawn(Vector2 localPosision)
		{
			Block block = GetObject();
			block.Initialize(localPosision);
		}

		public void Spawn(Vector2[] localPositions)
		{
			Block[] blockArray = GetObjects(localPositions.Length);
			for(int i = 0; i < blockArray.Length && i < localPositions.Length; i++)
			{
				blockArray[i].Initialize(localPositions[i]);
			}
		}
	}
}
