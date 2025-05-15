using System;
using UnityEngine;

// Token: 0x02000210 RID: 528
public static class GameObjectExtensions
{
	// Token: 0x06000B23 RID: 2851 RVA: 0x0005000C File Offset: 0x0004E20C
	public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
	{
		T t;
		if (gameObject.TryGetComponent<T>(out t))
		{
			return t;
		}
		return gameObject.AddComponent<T>();
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x0005002C File Offset: 0x0004E22C
	public static Component GetOrAddComponent(this GameObject gameObject, Type componentType)
	{
		Component component;
		if (gameObject.TryGetComponent(componentType, out component))
		{
			return component;
		}
		return gameObject.AddComponent(componentType);
	}
}
