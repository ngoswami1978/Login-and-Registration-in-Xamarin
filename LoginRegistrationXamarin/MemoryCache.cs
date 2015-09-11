
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Android.Graphics;


namespace MyFirstApp
{
		internal class MemoryCache
		{
			private IDictionary<String, Bitmap> m_Cache = new ConcurrentDictionary<String, Bitmap>();

			private List<String> m_CacheList = new List<String>();

			public void PopCache(int max)
			{
				if (max == 0)
					return;

				if (m_CacheList.Count >= max)
				{
					if (m_Cache.ContainsKey(m_CacheList[0]))
						m_Cache.Remove(m_CacheList[0]);

					m_CacheList.RemoveAt(0);
				}


			}

			public Bitmap Get(String id)
			{
				if (!m_Cache.ContainsKey(id))
					return null;

				return m_Cache[id];
			}

			public void Put(string id, Bitmap bitmap)
			{
				if (!m_Cache.ContainsKey(id))
					m_Cache.Add(id, bitmap);

				if(!m_CacheList.Contains(id))
					m_CacheList.Add(id);

				//if(m_CacheList.Count == 60)
				//{
				//    for(int i = 30; i >=0; i--)
				//    {
				//        m_Cache.Remove(m_CacheList[i]);
				//        m_CacheList.RemoveAt(i);
				//    }
				//}

			}

			public void Clear()
			{
				m_Cache.Clear();
				m_CacheList.Clear();
			}
	}
}


