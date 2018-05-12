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

		private Rigidbody m_rigidbody;
		private void Awake()
		{
			m_groupId = IncrimentalBlockGroupIndex++;
			m_rigidbody = GetComponent<Rigidbody>();
		}

		private bool m_stoppedFlag;
		private float m_speed = 6;
		private void Update()
		{
			m_stoppedFlag = false;
			m_rigidbody.velocity = new Vector3(0, - m_speed, 0);
		}

		#region Block
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

		public void Mapping(ref int[][] map)
		{
			for(int i = 0; i < blockList.Count; i++)
			{
				Block block = blockList[i];

				int x = (int)block.transform.position.x;
				int y = (int)block.transform.position.y;
				if(y >= map.Length || x >= map[0].Length)
				{
					return;
				}

				map[y][x] = m_groupId;
			}
		}
		#endregion // Block

		private void OnTriggerEnter(Collider other)
		{
			AmendPosition();
		}

		private void OnTriggerStay(Collider other)
		{
			AmendPosition();
		}

		private void AmendPosition()
		{
			Vector3 pos = transform.position;
			pos.y = (int)pos.y;
			transform.position = pos;
			m_stoppedFlag = true;
		}

		public bool IsStopped()
		{
			return m_stoppedFlag;
		}
	}
}
