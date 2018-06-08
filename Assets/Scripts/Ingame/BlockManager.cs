using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
	public class BlockManager : ObjectManagerBase<Block>
	{
		public const int MAX_ROWS = 30;
		public const int MAX_COLUMNS = 15;
		public const int SPAWN_HEIGHT = 30;
		private readonly string BLOCK_PREFAB_PATH = "Prefabs/Ingame/Block";
		private readonly string BLOCK_PARENT_OBJECT_NAME = "Blocks";

		public BlockManager(Transform parent) : base(parent) { }

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
			if(block == null)
			{
				return;
			}

			block.Initialize(localPosision);
		}

		public void Spawn(Vector2[] localPositions)
		{
			Block[] blockArray = GetObjects(localPositions.Length);
			for(int i = 0; i < blockArray.Length && i < localPositions.Length; i++)
			{
				if(blockArray[i] == null)
				{
					continue;
				}
				blockArray[i].Initialize(localPositions[i]);
			}
		}

		public Block[][] GetGroundedMap()
		{
			Block[][] map;
			Mapping(out map, GetBuffer());
			return map;
		}

		private static void Mapping(out Block[][] map, Block[] buffer)
		{
			map = Utils.CreateMatrix<Block>(MAX_ROWS, MAX_COLUMNS);
			for(int i = 0, length = buffer.Length; i < length; i++)
			{
				Block block = buffer[i];
				if(!block.gameObject.activeSelf)
				{
					continue;
				}

				if(!block.IsGrounded)
				{
					continue;
				}

				int x = (int)block.transform.localPosition.x;
				int y = (int)block.transform.localPosition.y;
				if(y >= map.Length || x >= map[y].Length)
				{
					continue;
				}

				map[y][x] = block;
			}
		}

		
	}
}

