using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;

namespace Ingame
{
	public class Block : MonoBehaviour
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

		// Use this for initialization
		void Start()
		{
			gameObject.GetComponent<Renderer>().material.color = Color.green;
		}

		bool m_canMove = true;

		#region Update
		private void Update()
		{
			if(m_canMove)
			{
				CalcMoveVector();
			}
		}

		Vector3 m_moveVector;
		
		// これが立つと移動後にm_canMoveがfalseになる
		bool m_stopFlag = false;
		public bool CanMove { get { return m_canMove; } }
		private void CalcMoveVector()
		{
			m_moveVector = GetMoveVector();
		}

		private void LateUpdate()
		{
			if(m_canMove)
			{
				Move();
			}

			if(m_stopFlag)
			{
				StopMove();
			}
		}

		#region Move
		private float m_speed = 10.0f;
		private void Move()
		{
			m_rigidbody.MovePosition(this.transform.position + m_moveVector);
		}

		private Vector3 GetMoveVector()
		{
			// 何も無ければこのベクトルが返る
			Vector3 moveVector = new Vector3(0, -m_speed * GameManager.SECOND_PER_FRAME, 0);

			// 真下にレイを撃つ
			RaycastHit raycastHit;
			if(!RaycastDown(out raycastHit))
			{
				// 何にも当たらなければそのまま移動できる
				return moveVector;
			}

			// ブロックに当たったときの処理
			if(raycastHit.transform.gameObject.tag == GameManager.TAG_BLOCK)
			{
				return GetMoveVectorWithUnderBlock(raycastHit, moveVector);
			}

			// フロアに当たったときの処理
			else if(raycastHit.transform.gameObject.tag == GameManager.TAG_FLOOR)
			{
				return GetVectorWithFloor(raycastHit, moveVector);
			}

			return moveVector;
		}

		private Vector3 GetMoveVectorWithUnderBlock(RaycastHit raycastHit, Vector3 moveVector)
		{
			Block block = raycastHit.transform.gameObject.GetComponent<Block>();
			if(block == null)
			{
				Debug.LogError("Block component is not attached.");
				return moveVector;
			}

			// 移動中なら自分も移動できるはず
			if(block.CanMove)
			{
				return moveVector;
			}

			// それ以外の場合は床に接触したときと同じ判定にする
			return GetVectorWithFloor(raycastHit, moveVector);
		}

		//距離を計算して短い方を帰す
		private Vector3 GetVectorWithFloor(RaycastHit raycastHit, Vector3 moveVector)
		{
			Vector3 hitDist = CalcDistance(raycastHit.point);
			if(hitDist.magnitude < moveVector.magnitude)
			{
				m_stopFlag = true;
				return hitDist;
			}

			return moveVector;
		}

		// レイが当たったところから自分の下までの距離を計算
		private Vector3 CalcDistance(Vector3 hitPoint)
		{			
			return hitPoint - GetBottomPos();	
		}

		// 真下にレイを撃つ
		private bool RaycastDown(out RaycastHit raycastHit, float maxDistance = 1.0f)
		{
			// 底面位置を計算	
			Ray ray = new Ray(GetBottomPos(), Vector3.down);

			Debug.DrawRay(ray.origin, ray.direction, Color.red, 3.0f);
			return Physics.Raycast(ray, out raycastHit, maxDistance);
		}

		private Vector3 GetBottomPos()
		{
			Vector3 bottomPos = this.transform.position;
			bottomPos.y -= this.transform.lossyScale.y / 2.0f;
			return bottomPos;
		}
		#endregion // Move
		#endregion // Update

		private void StopMove()
		{
			SpreadMessage2NeighborBlocks(Vector3.left);
			SpreadMessage2NeighborBlocks(Vector3.up);
			SpreadMessage2NeighborBlocks(Vector3.right);
			SpreadMessage2NeighborBlocks(Vector3.down);
			m_canMove = false;
		}

		#region Move


		public bool IsStopped()
		{
			return (m_rigidbody.velocity.magnitude <= Vector3.kEpsilon);
		}

		private void SpreadMessage2NeighborBlocks(Vector3 direction)
		{
			Vector3 normalizedDirection = direction.normalized;
			float maxDistance = transform.lossyScale.x / 2.0f;
			Ray ray = new Ray(this.transform.position + normalizedDirection * maxDistance, normalizedDirection);
			RaycastHit raycastHit;
			bool isHit = Physics.Raycast(ray, out raycastHit, maxDistance);

			Debug.DrawRay(ray.origin, ray.direction, Color.red, 3.0f);

			if(!isHit)
			{
				return;
			}

			if(raycastHit.transform.tag != GameManager.TAG_BLOCK)
			{
				Debug.Log(raycastHit.transform.tag + "にぶつかった");
				return;
			}

			Block block = raycastHit.transform.GetComponent<Block>();
			if(block == null)
			{
				return;
			}

			if(!block.CanMove)
			{
				return;
			}

			block.m_stopFlag = true;
		}

		#endregion // Move
		#region Trigger
		/*
		private void OnTriggerEnter(Collider other)
		{
			OnTriggerAction(other);
		}

		private void OnTriggerAction(Collider other)
		{
			GameObject obj = other.gameObject;
			if(obj.tag == GameManager.TAG_BLOCK)
			{
				OnTriggerBlock(obj);
			}

			else if(obj.tag == GameManager.TAG_FLOOR)
			{
				OnTriggerFloor(obj);
			}
		}

		private void OnTriggerBlock(GameObject obj)
		{
			Block block = obj.GetComponent<Block>();
			if(block.IsStopped())
			{
				OnTriggerLandedBlock(block);
			}
		}

		private void OnTriggerLandedBlock(Block block)
		{
			AmendPosition(block.transform.position);
			StopMove();			
		}

		private void OnTriggerFloor(GameObject obj)
		{
			AmendPosition(obj.transform.position);
			StopMove();			
		}
		
		private void AmendPosition(Vector3 underObjectPos)
		{
			Vector3 pos = transform.position;
			pos.y = underObjectPos.y;
			transform.position = pos;
		}
		*/
		#endregion // Trigger

	}
}