using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
	public class GameManager : MonoBehaviour
	{
		// 1フレームの時間を固定
		public const float SECOND_PER_FRAME = 0.016f;
		public const int MAX_ROWS = 30;
		public const int MAX_COLUMNS = 15;

		private BlockManager m_blockManager;
		BlockManager BlockManager { get { return m_blockManager; } }

		// Use this for initialization
		void Awake()
		{
			CreateBlockManager();
			DebugBackground();
		}

		private void CreateBlockManager()
		{
			m_blockManager = gameObject.AddComponent<BlockManager>();
			m_blockManager.transform.parent = this.transform;
		}

		void DebugBackground()
		{
			GameObject bg = new GameObject("BackGround");
			bg.transform.parent = this.transform;
			bg.transform.position = Vector3.zero;
			for(int r = 0; r < MAX_ROWS; r++)
			{
				for(int c = 0; c < MAX_COLUMNS; c++)
				{
					float color = ((r + c) % 2 == 0) ? 0.8f : 1.0f;
					GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
					Material material = cube.GetComponent<Renderer>().material;
					material.color = new Color(color,color,color,0.3f);

					cube.transform.parent = bg.transform;

					cube.transform.localPosition = new Vector3(c, r, 0);
				}
			}
			bg.transform.position = new Vector3(0, 0, 10);
		}
	}
}