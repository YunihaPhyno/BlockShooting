using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField]
		private Player m_player;

		private BlockManager m_blockManager;
		private BulletManager m_bulletManager;
		private BackGround m_backGround;

		float m_blockCoolTime = 0;
		float m_blockCoolTimeMax = 7;

		float m_shootCoolTime = 0;
		float m_shootCoolTimeMax = 0.3f;

		private void Awake()
		{
			m_blockManager = new BlockManager(this.transform);
			m_bulletManager = new BulletManager(this.transform);
			m_backGround = new BackGround(BlockManager.MAX_ROWS, BlockManager.MAX_COLUMNS, 10, this.transform);
		}

		// Update is called once per frame
		void Update()
		{
			UpdateCoolTime();
			if(PlayerInput.IsInputShoot() && CanShoot())
			{
				Shoot();
				ResetShootCoolTime();
			}
			
			if(CanSpawnBlocks())
			{
				SpawnBlocks();
				ResetBlocksCoolTime();
			}

			CheckLine();
		}

		private void UpdateCoolTime()
		{
			m_blockCoolTime -= Consts.SECONDS_PER_FRAME;
			m_shootCoolTime -= Consts.SECONDS_PER_FRAME;
		}

		#region Shoot
		private bool CanShoot()
		{
			if(m_shootCoolTime > 0)
			{
				return false;
			}

			return true;
		}

		private void ResetShootCoolTime()
		{
			m_shootCoolTime = m_shootCoolTimeMax;
		}

		private void Shoot()
		{
			m_bulletManager.Spawn((Vector2)m_player.transform.position);
		}
		#endregion //Shoot

		#region SpawnBlocks
		private bool CanSpawnBlocks()
		{
			if(m_blockCoolTime > 0)
			{
				return false;
			}

			return true;			
		}

		private void ResetBlocksCoolTime()
		{
			m_blockCoolTime = m_blockCoolTimeMax;
		}

		private void SpawnBlocks()
		{
			m_blockManager.MakeRandomBlocks(1, BlockManager.MAX_COLUMNS);
		}
		#endregion //SpawnBlocks

		#region CheckLine
		private void CheckLine()
		{
			Block[][] map = m_blockManager.GetGroundedMap();
			int numVanishLines = 0;
			for(int r = 0, r_length = map.Length; r < r_length; r++)
			{
				Block[] line = map[r];
				if(!IsFilledLine(line))
				{
					continue;
				}

				VanishLine(line);
				numVanishLines++;
			}
		}

		private bool IsFilledLine(Block[] line)
		{
			for(int c = 0, c_length = line.Length; c < c_length; c++)
			{
				if(line[c] == null)
				{
					return false;
				}
			}
			return true;
		}

		private void VanishLine(Block[] line)
		{
			for(int c = 0, c_length = line.Length; c < c_length; c++)
			{
				if(line[c] == null)
				{
					continue;
				}
				line[c].Vanish();
			}
		}
		#endregion // CheckLine
	}
}
