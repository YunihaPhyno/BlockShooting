using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
	public class Player : MonoBehaviour
	{
		private const float GUIDE_LINE_WIDTH = 0.02f;

		private Rigidbody2D m_rigidbody;

		private float m_speed = 4.0f;

		private void Awake()
		{
			m_rigidbody = GetComponent<Rigidbody2D>();
		}

		private void Update()
		{
			Move();
			DrawGuideLine();
		}

		private void Move()
		{
			Vector2 moveVector = GetMoveVector();
			Vector2 nextPosition = (Vector2)transform.position + moveVector;
			m_rigidbody.MovePosition(nextPosition);
		}

		private Vector2 GetMoveVector()
		{
			Vector2 moveVector = Vector2.zero;
			if(PlayerInput.IsInputUp())
			{
				moveVector += Vector2.up;
			}

			if(PlayerInput.IsInputDown())
			{
				moveVector += Vector2.down;
			}

			if(PlayerInput.IsInputLeft())
			{
				moveVector += Vector2.left;
			}

			if(PlayerInput.IsInputRight())
			{
				moveVector += Vector2.right;
			}

			return moveVector * Consts.SECONDS_PER_FRAME * m_speed;
		}

		private void DrawGuideLine()
		{
			LineRenderer renderer = gameObject.GetComponent<LineRenderer>();
			// 線の幅
			renderer.startWidth = GUIDE_LINE_WIDTH;
			renderer.endWidth = GUIDE_LINE_WIDTH;
			// 頂点の数
			renderer.numCapVertices = 2;
			// 頂点を設定
			Vector3 start = this.transform.position;
			Vector3 end = new Vector3(start.x, BlockManager.MAX_ROWS);
			renderer.SetPosition(0, start);
			renderer.SetPosition(1, end);
		}
	}
}

