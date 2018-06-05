using UnityEngine;

public class RingBuffer<T> where T : Component
{
	T[] m_buffer;
	int tail = 0;

	public RingBuffer(int num, GameObject prefab, Transform parent = null)
	{
		m_buffer = new T[num];
		for(int i = 0; i < m_buffer.Length; i++)
		{
			GameObject obj = GameObject.Instantiate<GameObject>(prefab);
			obj.SetActive(false);
			obj.transform.parent = parent;

			T component = obj.GetComponent<T>();
			if(component == null)
			{
				Debug.LogException(new System.Exception("Component can not be got exception."));
			}

			m_buffer[i] = component;
		}
	}

	public T Get()
	{
		for(int i = 0; i < m_buffer.Length; i++, tail++)
		{
			if(tail >= m_buffer.Length)
			{
				tail = 0;
			}

			T obj = m_buffer[tail];
			if(!obj.gameObject.activeSelf)
			{
				return obj;
			}
		}

		return null;
	}

	public T[] GetAllBuffer()
	{
		return m_buffer;
	}
}
