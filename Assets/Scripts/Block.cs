using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
	private const int NUM_HIT_ARRAY_MAX = 5;

	private float m_speed = 6.0f;

	private Rigidbody2D m_rigidBody;
	private Rigidbody2D GetRigidBody()
	{
		if(m_rigidBody == null)
		{
			m_rigidBody = GetComponent<Rigidbody2D>();
			if(m_rigidBody == null)
			{
				Debug.LogException(new System.Exception("Rigidbody2D is not attached exception."));
			}
		}
		return m_rigidBody;
	}

	List<Collider2D> m_colliderList = new List<Collider2D>();

	private Vector2 m_moveVector;

	private void Awake()
	{
		m_colliderList.AddRange(gameObject.GetComponentsInChildren<Collider2D>());	
	}

	// Use this for initialization
	private void Start () {
		
	}

	// Update is called once per frame
	private void Update () {
		// 移動ベクトルの計算
		CalculateMoveVector();
		SyncMoveVectorNeighborBlock();
	}

	private void LateUpdate()
	{
		Debug.Log("[" + Time.time + "]" + gameObject.name + m_moveVector.ToString());
		InvokeMove(m_moveVector);
		m_moveVector = Vector2.positiveInfinity;
	}

	// 移動ベクトルの計算
	private void CalculateMoveVector()
	{
		// 移動を試みる距離
		float requestMoveDistance = m_speed * Consts.SECONDS_PER_FRAME;

		Vector2 result = TryMove(requestMoveDistance);

		// 何処まで移動できるかを計算。結果をメンバに保存
		if(result.sqrMagnitude < m_moveVector.sqrMagnitude)
		{
			m_moveVector = result;
		}
	}

	// 何処まで移動できるか
	private Vector2 TryMove(float moveDistance, Side side = Side.BOTTOM)
	{
		Vector2 origin = (Vector2)transform.position + GetObjectSideVectorFromCenter(side);

		Debug.DrawRay(origin, GetObjectSideDirectionFromCenter(side) * moveDistance, Color.red, Consts.SECONDS_PER_FRAME);

		RaycastHit2D[] hitArray = new RaycastHit2D[NUM_HIT_ARRAY_MAX];
		ContactFilter2D filter = new ContactFilter2D();
		filter.useTriggers = true;

		int numHits = Physics2D.Raycast(origin, GetObjectSideDirectionFromCenter(side), filter, hitArray, moveDistance);
		for(int i = 0; i < numHits && i < hitArray.Length; i++)
		{
			RaycastHit2D hit = hitArray[i];
			if(hit.collider == null || m_colliderList.Contains(hit.collider))
			{
				continue;
			}

			if(hit.transform.tag == "Block")
			{
				continue;
			}

			return hit.point - origin;
		}

		return Vector2.down * moveDistance;
	}
	

	private void InvokeMove(Vector2 moveVector)
	{
		Vector2 nextPosition = (Vector2)transform.position + moveVector;
		GetRigidBody().MovePosition(nextPosition);
	}

	private void TrySyncMoveVector(Vector2 moveVector)
	{
		if(moveVector == m_moveVector)
		{
			return;
		}

		if(moveVector.sqrMagnitude < m_moveVector.sqrMagnitude)
		{
			Debug.Log("[" + Time.time + "]Sync " + gameObject.name);
			m_moveVector = moveVector;
		}
		SyncMoveVectorNeighborBlock();
	}

	enum Side
	{
		TOP,
		BOTTOM,
		LEFT,
		RIGHT
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

	private void SyncMoveVectorNeighborBlock()
	{
		List<Block> blockList = GetNeighborBlockList();
		blockList.ForEach(block => block.TrySyncMoveVector(m_moveVector));
	}

	private const float SEARCH_NEIGHBOR_BLOCK = 0.5f;
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

}
