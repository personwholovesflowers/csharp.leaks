using System;
using UnityEngine;

// Token: 0x020000A4 RID: 164
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class CanvasController : MonoSingleton<CanvasController>
{
	// Token: 0x1700005B RID: 91
	// (get) Token: 0x06000336 RID: 822 RVA: 0x00013C54 File Offset: 0x00011E54
	// (set) Token: 0x06000337 RID: 823 RVA: 0x00013C5C File Offset: 0x00011E5C
	public Crosshair crosshair { get; private set; }

	// Token: 0x06000338 RID: 824 RVA: 0x00013C65 File Offset: 0x00011E65
	protected override void Awake()
	{
		if (MonoSingleton<CanvasController>.Instance && MonoSingleton<CanvasController>.Instance != this)
		{
			Object.DestroyImmediate(base.gameObject);
			return;
		}
		base.Awake();
		this.crosshair = base.GetComponentInChildren<Crosshair>(true);
	}

	// Token: 0x06000339 RID: 825 RVA: 0x00013C9F File Offset: 0x00011E9F
	protected override void OnEnable()
	{
		base.OnEnable();
		base.transform.SetParent(null);
	}
}
