using System;
using UnityEngine;

// Token: 0x02000167 RID: 359
public class VibrationTest : MonoBehaviour
{
	// Token: 0x06000863 RID: 2147 RVA: 0x00027F28 File Offset: 0x00026128
	private void Update()
	{
		if (Singleton<GameMaster>.Instance.stanleyActions.Up.WasPressed)
		{
			this.strength += 0.05f;
		}
		if (Singleton<GameMaster>.Instance.stanleyActions.Down.WasPressed)
		{
			this.strength -= 0.05f;
		}
		if (Singleton<GameMaster>.Instance.stanleyActions.Right.WasPressed)
		{
			PlatformGamepad.PlayVibration(this.strength);
		}
		if (Singleton<GameMaster>.Instance.stanleyActions.Left.WasPressed)
		{
			PlatformGamepad.StopVibration();
		}
	}

	// Token: 0x04000836 RID: 2102
	private float strength;
}
