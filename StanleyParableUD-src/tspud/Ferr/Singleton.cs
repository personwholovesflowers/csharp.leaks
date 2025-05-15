using System;
using UnityEngine;

namespace Ferr
{
	// Token: 0x020002F6 RID: 758
	public abstract class Singleton<T> : MonoBehaviour where T : Component
	{
		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060013B5 RID: 5045 RVA: 0x00068B59 File Offset: 0x00066D59
		public static T Instance
		{
			get
			{
				if (Singleton<T>._instance == null)
				{
					Singleton<T>.EnsureInstantiated();
				}
				return Singleton<T>._instance;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060013B6 RID: 5046 RVA: 0x00068B77 File Offset: 0x00066D77
		public static bool Instantiated
		{
			get
			{
				return Singleton<T>._instance != null;
			}
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x00068B8C File Offset: 0x00066D8C
		public static void EnsureInstantiated()
		{
			if (Singleton<T>._instance == null)
			{
				Singleton<T>._instance = Object.FindObjectOfType<T>();
				if (Singleton<T>._instance == null)
				{
					GameObject gameObject = Resources.Load("Singletons/" + typeof(T).Name) as GameObject;
					if (gameObject != null)
					{
						Singleton<T>._instance = Object.Instantiate<GameObject>(gameObject).GetComponent<T>();
					}
				}
				if (Singleton<T>._instance == null)
				{
					Singleton<T>._instance = new GameObject("_" + typeof(T).Name).AddComponent(typeof(T)) as T;
				}
				if (Singleton<T>._instance == null)
				{
					Debug.LogErrorFormat("Couldn't connect or create singleton {0}!", new object[] { typeof(T).Name });
				}
			}
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x00068C8C File Offset: 0x00066E8C
		protected virtual void Awake()
		{
			if (Singleton<T>._instance != null && Singleton<T>._instance != this)
			{
				Debug.LogErrorFormat(base.gameObject, "Creating a new instance of a singleton [{0}] when one already exists!", new object[] { typeof(T).Name });
				base.gameObject.SetActive(false);
				return;
			}
			Singleton<T>._instance = base.GetComponent<T>();
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x00068CFD File Offset: 0x00066EFD
		protected virtual void OnDestroy()
		{
			Singleton<T>._instance = default(T);
		}

		// Token: 0x04000F54 RID: 3924
		private static T _instance;
	}
}
