using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
	public class Bullet : MonoBehaviour
	{

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void FixedUpdate()
		{
			Move();
			if(transform.position.y >= GameManager.MAX_ROWS)
			{
				Destroy(gameObject);
			}
		}

		float deltaPosition = 0;

		private void Move()
		{
			deltaPosition += Time.fixedDeltaTime * 4;
			if(deltaPosition >= 1)
			{
				Vector3 pos = transform.position + Vector3.up;
				GetComponent<Rigidbody>().MovePosition(pos);
				deltaPosition = 0;
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			switch(other.gameObject.tag)
			{
				case "Block":
					Destroy(gameObject);
					Destroy(other.gameObject);
					break;
			}

		}
	}
}