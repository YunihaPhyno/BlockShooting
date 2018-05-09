using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
	public class Player : MonoBehaviour
	{

		GameObject m_bulletParent;

		// Use this for initialization
		void Start()
		{
			m_bulletParent = new GameObject("Bullets");
		}

		// Update is called once per frame
		void Update()
		{
			Move();

			if(Input.GetKeyDown(KeyCode.Space))
			{
				Shoot();
			}
		}

		private void Move()
		{
			if(Input.GetKeyDown(KeyCode.W))
			{
				transform.position += Vector3.up;
			}

			if(Input.GetKeyDown(KeyCode.S))
			{
				transform.position += Vector3.down;
			}

			if(Input.GetKeyDown(KeyCode.A))
			{
				transform.position += Vector3.left;
			}

			if(Input.GetKeyDown(KeyCode.D))
			{
				transform.position += Vector3.right;
			}
		}

		private void Shoot()
		{
			GameObject bullet = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Bullet"));
			bullet.transform.position = transform.position;
			bullet.transform.parent = m_bulletParent.transform;
		}
	}
}