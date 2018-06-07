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

		float m_blockCoolTime = 0;
		float m_blockCoolTimeMax = 10;

		float m_shootCoolTime = 0;
		float m_shootCoolTimeMax = 0.3f;

		private void Awake()
		{
			m_blockManager = new BlockManager(this.transform);
			m_bulletManager = new BulletManager(this.transform);
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
		}

		private void UpdateCoolTime()
		{
			m_blockCoolTime -= Consts.SECONDS_PER_FRAME;
			m_shootCoolTime -= Consts.SECONDS_PER_FRAME;
		}

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
			m_blockManager.MakeRandomBlocks(3, BlockManager.MAX_COLUMNS);
		}
	}
}
