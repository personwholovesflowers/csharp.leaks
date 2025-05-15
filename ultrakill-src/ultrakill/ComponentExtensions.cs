using System;
using UnityEngine;

// Token: 0x020000D5 RID: 213
public static class ComponentExtensions
{
	// Token: 0x06000441 RID: 1089 RVA: 0x0001D420 File Offset: 0x0001B620
	public static T GetOrAddComponent<T>(this Component component) where T : Component
	{
		return component.gameObject.GetOrAddComponent<T>();
	}

	// Token: 0x06000442 RID: 1090 RVA: 0x0001D42D File Offset: 0x0001B62D
	public static Component GetOrAddComponent(this Component component, Type componentType)
	{
		return component.gameObject.GetOrAddComponent(componentType);
	}
}
