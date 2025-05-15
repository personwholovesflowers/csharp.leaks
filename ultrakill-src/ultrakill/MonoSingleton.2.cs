using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000301 RID: 769
public abstract class MonoSingleton<T> : MonoSingleton where T : MonoSingleton<T>
{
	// Token: 0x0600117D RID: 4477 RVA: 0x000884C3 File Offset: 0x000866C3
	static MonoSingleton()
	{
		ConfigureSingletonAttribute customAttribute = typeof(T).GetCustomAttribute<ConfigureSingletonAttribute>();
		MonoSingleton<T>.flags = ((customAttribute != null) ? customAttribute.Flags : SingletonFlags.None);
	}

	// Token: 0x0600117E RID: 4478 RVA: 0x000884E8 File Offset: 0x000866E8
	private static T Initialize()
	{
		if (!MonoSingleton<T>.flags.HasFlag(SingletonFlags.NoAutoInstance))
		{
			GameObject gameObject = new GameObject(typeof(T).FullName);
			T t = gameObject.AddComponent<T>();
			if (MonoSingleton<T>.flags.HasFlag(SingletonFlags.HideAutoInstance))
			{
				gameObject.hideFlags = HideFlags.HideAndDontSave;
			}
			if (MonoSingleton<T>.flags.HasFlag(SingletonFlags.PersistAutoInstance))
			{
				Object.DontDestroyOnLoad(gameObject);
			}
			return t;
		}
		if (!SceneManager.GetActiveScene().isLoaded)
		{
			return Object.FindObjectOfType<T>();
		}
		return default(T);
	}

	// Token: 0x1700016B RID: 363
	// (get) Token: 0x0600117F RID: 4479 RVA: 0x00088582 File Offset: 0x00086782
	// (set) Token: 0x06001180 RID: 4480 RVA: 0x000885A6 File Offset: 0x000867A6
	public static T Instance
	{
		get
		{
			if (!MonoSingleton<T>.instance)
			{
				return MonoSingleton<T>.instance = MonoSingleton<T>.Initialize();
			}
			return MonoSingleton<T>.instance;
		}
		protected set
		{
			MonoSingleton<T>.instance = value;
		}
	}

	// Token: 0x06001181 RID: 4481 RVA: 0x000885B0 File Offset: 0x000867B0
	protected virtual void Awake()
	{
		if (MonoSingleton<T>.instance && MonoSingleton<T>.flags.HasFlag(SingletonFlags.DestroyDuplicates) && MonoSingleton<T>.instance != this)
		{
			Object.Destroy(this);
			return;
		}
		if (MonoSingleton<T>.flags.HasFlag(SingletonFlags.NoAwakeInstance))
		{
			return;
		}
		if (MonoSingleton<T>.instance && MonoSingleton<T>.instance.isActiveAndEnabled && !base.isActiveAndEnabled)
		{
			return;
		}
		MonoSingleton<T>.Instance = (T)((object)this);
	}

	// Token: 0x06001182 RID: 4482 RVA: 0x0008864D File Offset: 0x0008684D
	protected virtual void OnEnable()
	{
		MonoSingleton<T>.Instance = (T)((object)this);
	}

	// Token: 0x06001183 RID: 4483 RVA: 0x00004AE3 File Offset: 0x00002CE3
	protected virtual void OnDestroy()
	{
	}

	// Token: 0x040017C4 RID: 6084
	private static T instance;

	// Token: 0x040017C5 RID: 6085
	private static readonly SingletonFlags flags;
}
