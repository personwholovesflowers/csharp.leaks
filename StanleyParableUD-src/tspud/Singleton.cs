using System;
using UnityEngine;

// Token: 0x02000190 RID: 400
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	// Token: 0x170000BD RID: 189
	// (get) Token: 0x06000939 RID: 2361 RVA: 0x0002B6D7 File Offset: 0x000298D7
	public static T Instance
	{
		get
		{
			if (Singleton<T>._instance == null)
			{
				Singleton<T>._instance = (T)((object)Object.FindObjectOfType(typeof(T)));
			}
			return Singleton<T>._instance;
		}
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x0002B70C File Offset: 0x0002990C
	protected virtual void Awake()
	{
		if (Singleton<T>._instance == null)
		{
			Singleton<T>._instance = this as T;
		}
		if (Singleton<T>._instance != this as T)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x04000911 RID: 2321
	private static T _instance;
}
