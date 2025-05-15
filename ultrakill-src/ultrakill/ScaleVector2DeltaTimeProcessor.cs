using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x020003C7 RID: 967
public sealed class ScaleVector2DeltaTimeProcessor : InputProcessor<Vector2>
{
	// Token: 0x060015FE RID: 5630 RVA: 0x000B22B3 File Offset: 0x000B04B3
	static ScaleVector2DeltaTimeProcessor()
	{
		ScaleVector2DeltaTimeProcessor.Initialize();
	}

	// Token: 0x060015FF RID: 5631 RVA: 0x000B22BA File Offset: 0x000B04BA
	public override Vector2 Process(Vector2 value, InputControl control)
	{
		return value * Time.deltaTime;
	}

	// Token: 0x06001600 RID: 5632 RVA: 0x000B22C7 File Offset: 0x000B04C7
	[RuntimeInitializeOnLoadMethod]
	private static void Initialize()
	{
		InputSystem.RegisterProcessor<ScaleVector2DeltaTimeProcessor>(null);
	}
}
