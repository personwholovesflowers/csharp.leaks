using System;
using UnityEngine;

// Token: 0x0200010D RID: 269
public class BeginButton : MenuButton
{
	// Token: 0x0600068A RID: 1674 RVA: 0x0002335E File Offset: 0x0002155E
	public override void OnClick(Vector3 point = default(Vector3))
	{
		base.OnClick(point);
		Singleton<GameMaster>.Instance.TSP_Reload();
		Singleton<GameMaster>.Instance.ClosePauseMenu(true);
	}
}
