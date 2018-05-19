using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
	public class BlockManager : MonoBehaviour
	{
		private static GameObject blockPrefab;
		private static GameObject floorPrefab;
		private static GameObject floorBlockPrefab;

		private Transform m_blockParent;
		private Transform BlockParent { get { return m_blockParent; } }

		#region BlockList
		private List<Block> m_blockList = new List<Block>();
		public List<Block> BlockList { get { return m_blockList; } }
		#endregion // BlockList

		#region SpawnRows
		private int m_spawnRows = 5;
		public void SetSpawnRows(int value) { m_spawnRows = value; }
		public int SpawnRows { get { return m_spawnRows; } }
		#endregion //SpawnRows

		#region Awake
		private void Awake()
		{
			LoadPrefab();

			CreateBlockParent();
		}

		private void CreateBlockParent()
		{
			GameObject obj = new GameObject("Blocks");
			obj.transform.parent = transform;
			m_blockParent = obj.transform;
		}

		private static void LoadPrefab()
		{
			blockPrefab = CheckLoad<GameObject>("Block");
			floorPrefab = CheckLoad<GameObject>("Floor");
			floorBlockPrefab = CheckLoad<GameObject>("FloorBlock");
		}

		private static T CheckLoad<T>(string path) where T : Object
		{
			T obj = Resources.Load<T>(path);
			if(obj == null)
			{
				obj.ToString(); // とりあえず無かったらexeption吐かせる(乱暴)
			}
			return obj;
		}
		#endregion //Awake

		#region Start
		private void Start()
		{
			CreateFloor();
			SetState(State.SPAWN);
		}

		private void CreateFloor()
		{
			GameObject floor = GameObject.Instantiate<GameObject>(floorPrefab);
			for(int c = 0; c < GameManager.MAX_COLUMNS; c++)
			{
				GameObject block = GameObject.Instantiate<GameObject>(floorBlockPrefab);
				block.transform.parent = floor.transform;
				block.transform.localPosition = new Vector3(c, -1, 0);
			}
			floor.transform.localPosition = Vector3.zero;
		}
		#endregion //Start

		#region Update
		// Update is called once per frame
		private void Update()
		{
			UpdateState();
		}

		#region State
		public enum State
		{
			SUSPEND,
			SPAWN,
			FALL_DOWN,
		}
		private State m_state;
		private void SetState(State state) { m_state = state; }
		public State state { get { return m_state; } }
		#endregion // State

		private void UpdateState()
		{
			switch(state)
			{
				case State.SUSPEND:
					// nothing to do
					break;

				case State.SPAWN:
					StateSpawn();
					break;

				case State.FALL_DOWN:
					StateFallDown();
					break;
			}
		}

		#region StateSpawn
		private void StateSpawn()
		{
			SpawnRandomBlocks();
			SetState(State.FALL_DOWN);
		}

		#region SpawnRandomBlocks

		

		#region Threshold
		// 生成される確率(1.0f = 100%)
		float m_threshold = 0.7f;
		public void SetThreshold(float value) { m_threshold = value; }
		public float Threshold { get { return m_threshold; } }
		#endregion // Threshold

		public void SpawnRandomBlocks()
		{
			Block[] blocks = MakeRandomBlocks(SpawnRows, GameManager.MAX_COLUMNS, Threshold, BlockParent);
			BlockList.AddRange(blocks);
		}
		
		private static Block[] MakeRandomBlocks(int rows, int cols, float threshold, Transform parent)
		{
			Block[] blocks = new Block[rows * cols];
			for(int r = 0; r < rows; r++)
			{
				for(int c = 0; c < cols; c++)
				{
					if(Random.Range(0.0f, 1.0f) <= threshold)
					{
						blocks[r * cols + c] = SpawnBlock(new Vector3(c, GameManager.MAX_ROWS + r, 0), parent);
					}
				}
			}
			return blocks;
		}

		private static Block SpawnBlock(Vector3 spawnPos, Transform parent = null)
		{
			GameObject gobj = GameObject.Instantiate<GameObject>(blockPrefab);
			gobj.transform.parent = parent;
			gobj.transform.localPosition = spawnPos;

			Block block = gobj.GetComponent<Block>();
			return block;
		}
		#endregion // SpawnRandomBlocks

		#endregion // StateSpawn

		#region StateFallDown
		private void StateFallDown()
		{
			if(IsAllBlocksStopped())
			{
				SetState(State.SPAWN);
			}
			//Mapping();
		}

		private bool IsAllBlocksStopped()
		{
			return false;
			/*
			for(int i = 0; i < BlockList.Count; i++)
			{
				if(!BlockList[i].IsStopped())
				{
					return false;
				}
			}
			return true;
			*/
		}

		#region Mapping
		/*
		private int[][] m_blockMap;
		private void Mapping()
		{
			m_blockMap = MappingAllBlock(m_blockGroupList, GameManager.MAX_ROWS + m_spawnRows, GameManager.MAX_COLUMNS);
		}

		private static int[][] MappingAllBlock(List<BlockGroup> blockGroupList, int rows, int columns)
		{
			int[][] map = CreateBlockMap(rows, columns);
			for(int i = 0; i < blockGroupList.Count; i++)
			{
				BlockGroup blockGroup = blockGroupList[i];
				blockGroup.Mapping(ref map);
			}
			return map;
		}

		private static int[][] CreateBlockMap(int rows, int columns)
		{
			int[][] map = new int[rows][];

			for(int r = 0; r < map.Length; r++)
			{
				map[r] = new int[columns];
				for(int c = 0; c < map[r].Length; c++)
				{
					map[r][c] = -1;
				}
			}
			return map;
		}
		*/
		#endregion // Mapping

		#region Move

		#endregion // Move

		#endregion // StateFallDown
		#endregion //Update





















	}
}
