using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour {

	private static readonly string BLOCK_PREFAB_PATH = "Block";

	private static GameObject blockPrefab;

	[SerializeField]
	private Transform m_blocksParent;

	private void Awake()
	{
		blockPrefab = Resources.Load<GameObject>(BLOCK_PREFAB_PATH);
	}



	// Use this for initialization
	void Start ()
	{
	}



	// Update is called once per frame


	private static Block MakeBlock(Vector2 pos, Transform parent = null)
	{
		GameObject obj = Instantiate<GameObject>(blockPrefab);
		obj.transform.parent = parent;
		obj.transform.localPosition = (Vector3)pos;
		return obj.GetComponent<Block>();
	}
}
