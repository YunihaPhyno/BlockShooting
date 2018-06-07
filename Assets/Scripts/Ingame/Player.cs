using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
	public class Player : MonoBehaviour
	{
		private Rigidbody2D m_rigidbody;

		private float m_speed = 4.0f;

		private void Awake()
		{
			m_rigidbody = GetComponent<Rigidbody2D>();
		}

		private void Update()
		{
			Move();
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
	}
}

