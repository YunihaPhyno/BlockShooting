using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
	public class Bullet : MonoBehaviour
	{
		private float m_speed = 10.0f;

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

		public void Initialize(Vector2 position)
		{
			this.transform.position = (Vector3)position;
			this.gameObject.SetActive(true);
		}

		private void Awake()
		{
			m_rigidBody = GetComponent<Rigidbody2D>();
		}
		
		private void Update()
		{
			Move();
		}

		private void Move()
		{
			Vector2 nextPosition = (Vector2)this.transform.position + GetMoveVector();
			m_rigidBody.MovePosition(nextPosition);
		}

		private Vector2 GetMoveVector()
		{
			return Vector2.up * m_speed * Consts.SECONDS_PER_FRAME;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			Block block;
			if(Utils.TryGetComponent<Block>(other.gameObject, out block))
			{
				block.OnTriggerBullet();
				OnTriggerAction();
				return;
			}		
		}

		private void OnTriggerAction()
		{
			this.gameObject.SetActive(false);
		}
	}
}