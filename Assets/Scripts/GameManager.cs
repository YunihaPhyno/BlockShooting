using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
	public class GameManager : MonoBehaviour
	{

		public const int MAX_ROWS = 30;
		public const int MAX_COLUMNS = 15;

		private BlockManager m_blockManager;
		BlockManager BlockManager { get { return m_blockManager; } }

		// Use this for initialization
		void Awake()
		{
			m_blockManager = gameObject.AddComponent<BlockManager>();
			DebugBackground();

			SetState(State.INIT);
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

		// Update is called once per frame
		void Update()
		{
			UpdateState();
		}

		#region State
		enum State
		{
			SUSPEND,
			INIT,
		}
		State m_state;
		void SetState(State state) { m_state = state; }
		State state { get { return m_state; } }
		#endregion // State

		void UpdateState()
		{
			switch(state)
			{
				case State.SUSPEND:
					// nothing to do
					break;

				case State.INIT:
					StateInit();
					break;				
			}
		}

		#region StateInit
		void StateInit()
		{
			float tb = Time.realtimeSinceStartup;
			BlockManager.SpawnRandomBlocks();
			SetState(State.SUSPEND);
			float ta = Time.realtimeSinceStartup;
			Debug.Log(ta - tb);
		}
		#endregion // StateInit
	}
}