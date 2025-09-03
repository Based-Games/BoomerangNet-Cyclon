using System;
using UnityEngine;

// Token: 0x0200022C RID: 556
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	// Token: 0x1700024E RID: 590
	// (get) Token: 0x06001001 RID: 4097 RVA: 0x00072EFC File Offset: 0x000710FC
	public static T instance
	{
		get
		{
			if (Singleton<T>.applicationIsQuitting)
			{
				return (T)((object)null);
			}
			object @lock = Singleton<T>._lock;
			T instance;
			lock (@lock)
			{
				if (Singleton<T>._instance == null)
				{
					Singleton<T>._instance = (T)((object)UnityEngine.Object.FindObjectOfType(typeof(T)));
					if (UnityEngine.Object.FindObjectsOfType(typeof(T)).Length > 1)
					{
						return Singleton<T>._instance;
					}
					if (Singleton<T>._instance == null)
					{
						GameObject gameObject = new GameObject();
						Singleton<T>._instance = gameObject.AddComponent<T>();
						gameObject.name = typeof(T).ToString();
						UnityEngine.Object.DontDestroyOnLoad(gameObject);
					}
				}
				instance = Singleton<T>._instance;
			}
			return instance;
		}
	}

	// Token: 0x06001002 RID: 4098 RVA: 0x0000DAAA File Offset: 0x0000BCAA
	public void OnDestroy()
	{
		Singleton<T>.applicationIsQuitting = true;
	}

	// Token: 0x0400119A RID: 4506
	private static T _instance;

	// Token: 0x0400119B RID: 4507
	private static object _lock = new object();

	// Token: 0x0400119C RID: 4508
	private static bool applicationIsQuitting = false;
}
