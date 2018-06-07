using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
	public abstract class ObjectManagerBase<T> where T : MonoBehaviour
	{
		private Transform m_objectParent;
		private RingBuffer<T> m_objectRingBuffer;

		public ObjectManagerBase(Transform parent = null)
		{
			m_objectParent = CreateParentObject(GetParentObjectName(), parent);
			m_objectRingBuffer = new RingBuffer<T>(GetBufferSize(), Resources.Load<GameObject>(GetObjectPrefabPath()), m_objectParent);
		}

		private static Transform CreateParentObject(string name, Transform parent = null)
		{
			Transform transform = new GameObject(name).transform;
			transform.transform.parent = parent;
			return transform;
		}

		protected abstract string GetParentObjectName();
		protected abstract int GetBufferSize();
		protected abstract string GetObjectPrefabPath();

		#region GetObject
		
		protected T GetObject()
		{
			return m_objectRingBuffer.Get();
		}

		protected T[] GetObjects(int length)
		{
			T[] objArray = new T[length];
			for(int i = 0; i < objArray.Length; i++)
			{
				objArray[i] = GetObject();
			}
			return objArray;
		}
		#endregion // GetObject
	}
}