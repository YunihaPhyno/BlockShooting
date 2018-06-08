using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround
{
	private readonly string NAME_PARENT_OBJECT = "BackGround";
	private readonly string BACKGROUND_BLOCK1_PREFAB_PATH = "Prefabs/Ingame/BackGroundBlock1";
	private readonly string BACKGROUND_BLOCK2_PREFAB_PATH = "Prefabs/Ingame/BackGroundBlock2";

	Transform m_parentObject;

	public BackGround(int rows, int cols, float z_pos, Transform parent = null)
	{
		m_parentObject = CreateParentObject(NAME_PARENT_OBJECT, z_pos, parent);

		GameObject block1, block2;
		LoadPrefabs(out block1, out block2);
		CreateBackGround(rows, cols, block1, block2, m_parentObject);
	}

	private Transform CreateParentObject(string name, float z_pos, Transform parent)
	{
		GameObject obj = new GameObject(name);
		obj.transform.parent = parent;
		obj.transform.localPosition = new Vector3(0, 0, z_pos);
		return obj.transform;
	}

	private void LoadPrefabs(out GameObject block1, out GameObject block2)
	{
		block1 = Resources.Load<GameObject>(BACKGROUND_BLOCK1_PREFAB_PATH);
		block2 = Resources.Load<GameObject>(BACKGROUND_BLOCK2_PREFAB_PATH);
	}

	private static void CreateBackGround(int rows, int cols, GameObject block1, GameObject block2, Transform parent)
	{
		for(int r = 0; r < rows; r++)
		{
			for(int c = 0; c < cols; c++)
			{
				GameObject prefab = (c % 2 == 0) ? block1 : block2;
				GameObject obj = GameObject.Instantiate<GameObject>(prefab);
				obj.transform.parent = parent;
				obj.transform.localPosition = new Vector3(c, r, 0);
			}
		}
	}

	~BackGround()
	{
		Utils.SafeDestroy(ref m_parentObject);
	}
}
