using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
	public class Block : MonoBehaviour
	{
		private const int NUM_HIT_ARRAY_MAX = 5;
		private const float SEARCH_NEIGHBOR_BLOCK = 0.5f;

		private float m_speed = 3.0f;

		private Rigidbody2D m_rigidBody;

		List<Collider2D> m_colliderList = new List<Collider2D>();
		private Vector2 m_moveVector = Vector2.down;

		private bool m_isGrounded = false;
		private bool m_isGroundedLastFrame = false;
		public bool IsGrounded { get { return m_isGrounded; } }
		private bool m_alreadyUpdatedGroundedUpdate = false;
		private bool m_canMoveThisFrame = false;

		enum Side
		{
			TOP,
			BOTTOM,
			LEFT,
			RIGHT
		}

		private void Awake()
		{
			m_colliderList.AddRange(gameObject.GetComponentsInChildren<Collider2D>());
			m_rigidBody = GetComponent<Rigidbody2D>();
		}

		public void Initialize(float x, float y)
		{
			Initialize(new Vector2(x, y));
		}

		public void Initialize(Vector2 localPosition)
		{
			this.transform.localPosition = (Vector3)localPosition;
			this.gameObject.SetActive(true);
			ResetFlags();
		}

		private void ResetFlags()
		{
			m_isGrounded = false;
			m_isGroundedLastFrame = false;
			m_alreadyUpdatedGroundedUpdate = false;
		}

		private void Update()
		{
			if(!m_isGrounded)
			{
				MoveUpdate();
			}
		}

		private void LateUpdate()
		{
			m_isGroundedLastFrame = m_isGrounded;
			m_isGrounded = false;
		}

		#region MoveUpdate
		private void MoveUpdate()
		{
			Vector2 moveVector = CalculateMoveVector();

			if(moveVector.sqrMagnitude < Vector2.kEpsilon)
			{
				OnGrounded();
			}

			else
			{
				InvokeMove(moveVector);
			}
		}

		// 移動ベクトルの計算
		private Vector2 CalculateMoveVector()
		{
			// 移動を試みる距離
			float requestMoveDistance = m_speed * Consts.SECONDS_PER_FRAME;

			return TryMove(requestMoveDistance);
		}

		// 何処まで移動できるか
		private Vector2 TryMove(float moveDistance, Side side = Side.BOTTOM)
		{
			// Rayを発射する場所の位置を計算
			Vector2 origin = (Vector2)transform.position + GetObjectSideVectorFromCenter(side);

			// デバッグ用Ray表示
			Debug.DrawRay(origin, GetObjectSideDirectionFromCenter(side) * moveDistance, Color.red, Consts.SECONDS_PER_FRAME);

			RaycastHit2D[] hitArray = new RaycastHit2D[NUM_HIT_ARRAY_MAX];

			// triggerになってるコライダを対象に含める
			ContactFilter2D filter = new ContactFilter2D();
			filter.useTriggers = true;

			// Rayを飛ばす
			int numHits = Physics2D.Raycast(origin, GetObjectSideDirectionFromCenter(side), filter, hitArray, moveDistance);

			// Rayの範囲内にある全てのオブジェクトについて
			for(int i = 0; i < numHits && i < hitArray.Length; i++)
			{
				RaycastHit2D hit = hitArray[i];

				// コライダーが自分のものだったらスキップ
				if(hit.collider == null || m_colliderList.Contains(hit.collider))
				{
					continue;
				}

				// ブロックの場合
				Block block;
				if(Utils.TryGetComponent<Block>(hit.transform.gameObject, out block))
				{
					// まだ移動中のブロックだったらスキップ
					if(!block.m_isGrounded)
					{
						continue;
					}
				}

				// バレットもスキップ
				Bullet bullet;
				if(Utils.TryGetComponent<Bullet>(hit.transform.gameObject, out bullet))
				{
					continue;
				}

				// プレイヤーもスキップ
				Player player;
				if(Utils.TryGetComponent<Player>(hit.transform.gameObject, out player))
				{
					continue;
				}

				// スキップ対象でないオブジェクトに当たった場合(floorかm_isGrounded==trueのBlockを想定)
				// 移動できる距離を計算
				return hit.point - origin;
			}

			// 対象のオブジェクトが無ければmoveDistanceいっぱいいっぱい移動する
			return Vector2.down * moveDistance;
		}

		private void InvokeMove(Vector2 moveVector)
		{
			Vector2 nextPosition = (Vector2)transform.position + moveVector;
			m_rigidBody.MovePosition(nextPosition);
		}

		private void OnGrounded()
		{
			if(m_isGrounded)
			{
				return;
			}

			m_isGrounded = true;
			NotifyGroundedAllNeighborBlocks();
			AlignPosition();
		}

		private void AlignPosition()
		{
			Vector3 pos = this.transform.localPosition;
			pos.y = Mathf.RoundToInt(pos.y);
			this.transform.localPosition = pos;
		}

		private void NotifyGroundedAllNeighborBlocks()
		{
			List<Block> neighborBlockList = GetNeighborBlockList();
			for(int i = 0, max = neighborBlockList.Count; i < max; i++)
			{
				Block current = neighborBlockList[i];
				if(current.m_isGrounded)
				{
					continue;
				}

				if(m_isGroundedLastFrame && !current.m_isGroundedLastFrame)
				{
					continue;
				}
				current.ReceivedGroundedNotify();
			}
		}

		public void ReceivedGroundedNotify()
		{
			OnGrounded();
		}
		#endregion // MoveUpdate

		#region GroundedUpdate
		private void GroundedUpdate()
		{
			if(m_alreadyUpdatedGroundedUpdate)
			{
				return;
			}
			m_alreadyUpdatedGroundedUpdate = true;

			if(!CanMove())
			{
				return;
			}
			m_isGrounded = false;
		}

		private bool CanMove()
		{
			List<Block> blockList = GetNeighborBlockList();
			return false;
		}

		#endregion

		private static bool CanMoveAllNeighborBlock(List<Block> neighborBlockList)
		{
			for(int i = 0, max = neighborBlockList.Count; i < max; i++)
			{
				Block current = neighborBlockList[i];
				if(!current.IsGrounded)
				{
					continue;
				}

				if(!current.CanMove())
				{
					return false;
				}
			}
			return true;
		}

		#region GetNeighborBlockList
		private List<Block> GetNeighborBlockList()
		{
			var neighborList = new List<Block>();

			foreach(Side side in Enum.GetValues(typeof(Side)))
			{
				Block block = SearchBlock(side, SEARCH_NEIGHBOR_BLOCK);
				if(block == null)
				{
					continue;
				}

				if(neighborList.Contains(block))
				{
					continue;
				}

				neighborList.Add(block);
			}

			return neighborList;
		}

		private Block SearchBlock(Side side, float distance)
		{
			Vector2 origin = (Vector2)transform.position + GetObjectSideVectorFromCenter(side);

			RaycastHit2D[] hitArray = new RaycastHit2D[NUM_HIT_ARRAY_MAX];

			ContactFilter2D filter = new ContactFilter2D();
			filter.useTriggers = true;

			Debug.DrawRay(origin, GetObjectSideDirectionFromCenter(side) * distance, Color.blue, Consts.SECONDS_PER_FRAME);
			int numHits = Physics2D.Raycast(origin, GetObjectSideDirectionFromCenter(side), filter, hitArray, distance);

			for(int i = 0; i < numHits; i++)
			{
				RaycastHit2D hit = hitArray[i];
				if(hit.collider == null || m_colliderList.Contains(hit.collider))
				{
					continue;
				}

				Block block = hit.transform.GetComponent<Block>();
				if(block != null)
				{
					return block;
				}

				block = hit.transform.GetComponentInParent<Block>();
				if(block != null)
				{
					return block;
				}
			}

			return null;
		}

		private Vector2 GetObjectSideVectorFromCenter(Side side)
		{
			return GetObjectSideDirectionFromCenter(side) * GetObjectSideDistanceFromCenter(side);
		}

		private float GetObjectSideDistanceFromCenter(Side side)
		{
			float value = 0;
			switch(side)
			{
				case Side.TOP:
				case Side.BOTTOM:
					value = transform.lossyScale.y / 2.0f;
					break;

				case Side.RIGHT:
				case Side.LEFT:
					value = transform.lossyScale.x / 2.0f;
					break;
			}
			return value;
		}

		private Vector2 GetObjectSideDirectionFromCenter(Side side)
		{
			switch(side)
			{
				case Side.TOP:
					return Vector2.up;

				case Side.BOTTOM:
					return Vector2.down;

				case Side.LEFT:
					return Vector2.left;

				case Side.RIGHT:
					return Vector2.right;
			}

			return Vector2.zero;
		}

		#endregion //GetNeighborBlockList

		public void OnTriggerBullet()
		{
			this.gameObject.SetActive(false);
		}
	}
}