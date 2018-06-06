using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
	public class BulletManager : ObjectManagerBase<Bullet>
	{
		private const int MAX_BULLETS = 100; 
		private readonly string BULLET_PREFAB_PATH = "Prefabs/Ingame/Bullet";
		private readonly string BULLET_PARENT_OBJECT_NAME = "Bullets";

		protected override int GetBufferSize()
		{
			return MAX_BULLETS;
		}

		protected override string GetObjectPrefabPath()
		{
			return BULLET_PREFAB_PATH;
		}

		protected override string GetParentObjectName()
		{
			return BULLET_PARENT_OBJECT_NAME;
		}

		
	}
}