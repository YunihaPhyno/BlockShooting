using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
	public class BlockGroup : MonoBehaviour
	{
		private static int IncrimentalBlockGroupIndex = 0;

		private int m_groupId;
		public int GroupId { get { return m_groupId; } }
		private void Awake()
		{
			m_groupId = IncrimentalBlockGroupIndex++;
		}

		private List<Block> blockList = new List<Block>();

		public void Add(Block block)
		{
			blockList.Add(block);
			block.transform.parent = this.transform;
		}

		public bool Contains(Block block)
		{
			return blockList.Contains(block);
		}
	}
}
