using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
	public class BlockManager : MonoBehaviour
	{
		private static GameObject blockPrefab;

		private Transform m_blockParent;
		private Transform BlockParent { get { return m_blockParent; } }

		void Awake()
		{
			LoadPrefab();

			m_blockParent = new GameObject("Blocks").transform;
		}

		private static void LoadPrefab()
		{
			blockPrefab = Resources.Load<GameObject>("Block");
			if(blockPrefab == null)
			{
				blockPrefab.ToString(); // とりあえず無かったらexeption吐かせる(乱暴)
			}
		}

		#region BlockList
		private List<BlockGroup> m_blockGroupList = new List<BlockGroup>();
		public List<BlockGroup> BlockGroupList { get { return m_blockGroupList; } }
		#endregion // BlockList

		#region SpawnRows
		private int m_spawnRows = 3;
		public void SetSpawnRows(int value) { m_spawnRows = value; }
		public int SpawnRows { get { return m_spawnRows; } }
		#endregion //SpawnRows

		#region Threshold
		// 生成される確率(1.0f = 100%)
		float m_threshold = 0.7f;
		public void SetThreshold(float value) { m_threshold = value; }
		public float Threshold { get { return m_threshold; } }
		#endregion // Threshold
		
		public void SpawnRandomBlocks()
		{
			Block[][] blocks = MakeRandomBlocks(SpawnRows, Threshold, BlockParent);
			MakeBlockGroups(blocks, BlockParent);
		}

		private static List<BlockGroup> MakeBlockGroups(Block[][] blocks, Transform parent)
		{
			var groupList = new List<BlockGroup>();

			// 全ての点を探索する
			for(int r = 0; r < blocks.Length; r++)
			{
				for(int c = 0; c < blocks[r].Length; c++)
				{
					// ブロックの存在チェック
					if(blocks[r][c] == null)
					{
						continue;
					}

					// そのブロックがグループに所属している？
					if(GetBlockGroup(groupList, blocks, r, c) != null)
					{
						continue;
					}
					
					// 所属が見つからなければ新たに作る
					BlockGroup newGroup = CreateBlockGroup(parent);
					groupList.Add(newGroup);

					// 再帰的に隣接するブロックを登録する
					AddAllNeighborBlock(newGroup, blocks, r, c, groupList);
				}
			}

			return groupList;
		}

		private static void AddAllNeighborBlock(BlockGroup group, Block[][] blocks, int row, int col, List<BlockGroup> groupList)
		{
			if(row < 0 || blocks.Length <= row || col < 0 || blocks[0].Length <= col)
			{
				return;
			}

			if(blocks[row][col] == null)
			{
				return;
			}

			if(GetBlockGroup(groupList, blocks, row, col) != null)
			{
				return;
			}

			// 自分自身を登録
			group.Add(blocks[row][col]);

			AddAllNeighborBlock(group, blocks, row + 1, col, groupList); // 上
			AddAllNeighborBlock(group, blocks, row, col + 1, groupList); // 右
			AddAllNeighborBlock(group, blocks, row - 1, col, groupList); // 下
			AddAllNeighborBlock(group, blocks, row, col - 1, groupList); // 左
		}

		private static BlockGroup CreateBlockGroup(Transform parent)
		{
			GameObject obj = new GameObject("BlockGroup");
			obj.transform.parent = parent;
			obj.transform.localPosition = Vector3.zero;

			BlockGroup group = obj.AddComponent<BlockGroup>();
			return group;
		}

		private static BlockGroup GetBlockGroup(List<BlockGroup> groupList, Block[][] blocks, int r, int c)
		{
			for(int i = 0; i < groupList.Count; i++)
			{
				if(!groupList[i].Contains(blocks[r][c]))
				{
					continue;
				}

				return groupList[i];
			}

			return null;
		}

		private static Block[][] MakeRandomBlocks(int rows, float threshold, Transform parent)
		{
			Block[][] blocks = new Block[rows][];
			for(int r = 0; r < rows; r++)
			{
				blocks[r] = new Block[GameManager.MAX_COLUMNS];
				for(int c = 0; c < GameManager.MAX_COLUMNS; c++)
				{
					if(Random.Range(0.0f, 1.0f) <= threshold)
					{
						blocks[r][c] = SpawnBlock(new Vector3(c, GameManager.MAX_ROWS + r, 0), parent);
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

			Block block = gobj.AddComponent<Block>();
			return block;
		}
	}
}
