using System;
using UnityEngine;

namespace Nest.Addons
{
	// Token: 0x02000250 RID: 592
	public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000E02 RID: 3586 RVA: 0x0003EAF4 File Offset: 0x0003CCF4
		public static T Instance
		{
			get
			{
				if (!Singleton<T>.Instantiated)
				{
					Singleton<T>.CreateInstance();
				}
				return Singleton<T>._instance;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000E03 RID: 3587 RVA: 0x0003EB07 File Offset: 0x0003CD07
		// (set) Token: 0x06000E04 RID: 3588 RVA: 0x0003EB0E File Offset: 0x0003CD0E
		public static bool Instantiated { get; private set; }

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000E05 RID: 3589 RVA: 0x0003EB16 File Offset: 0x0003CD16
		// (set) Token: 0x06000E06 RID: 3590 RVA: 0x0003EB1D File Offset: 0x0003CD1D
		public static bool Destroyed { get; private set; }

		// Token: 0x06000E07 RID: 3591 RVA: 0x0003EB28 File Offset: 0x0003CD28
		private static void CreateInstance()
		{
			if (Singleton<T>.Destroyed)
			{
				return;
			}
			Type typeFromHandle = typeof(T);
			T[] array = Object.FindObjectsOfType<T>();
			if (array.Length != 0)
			{
				if (array.Length > 1)
				{
					Debug.LogWarning("There is more than one instance of Singleton of type \"" + typeFromHandle + "\". Keeping the first one. Destroying the others.");
					for (int i = 1; i < array.Length; i++)
					{
						Object.Destroy(array[i].gameObject);
					}
				}
				Singleton<T>._instance = array[0];
				Singleton<T>._instance.gameObject.SetActive(true);
				Singleton<T>.Instantiated = true;
				Singleton<T>.Destroyed = false;
				return;
			}
			Singleton<T>._instance = new GameObject
			{
				name = typeFromHandle.ToString()
			}.AddComponent<T>();
			Singleton<T>.Instantiated = true;
			Singleton<T>.Destroyed = false;
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x0003EBE8 File Offset: 0x0003CDE8
		protected virtual void Awake()
		{
			if (!(Singleton<T>._instance == null))
			{
				if (this.Persistent)
				{
					Object.DontDestroyOnLoad(base.gameObject);
				}
				if (base.GetInstanceID() != Singleton<T>._instance.GetInstanceID())
				{
					Object.Destroy(base.gameObject);
				}
				return;
			}
			if (!this.Persistent)
			{
				return;
			}
			Singleton<T>.CreateInstance();
			Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x0003EC56 File Offset: 0x0003CE56
		private void OnDestroy()
		{
			Singleton<T>.Destroyed = true;
			Singleton<T>.Instantiated = false;
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x00005444 File Offset: 0x00003644
		public void Touch()
		{
		}

		// Token: 0x04000C82 RID: 3202
		private static T _instance;

		// Token: 0x04000C83 RID: 3203
		public bool Persistent;
	}
}
