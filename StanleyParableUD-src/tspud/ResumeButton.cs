using System;
using UnityEngine;

// Token: 0x02000120 RID: 288
public class ResumeButton : MenuButton
{
	// Token: 0x060006DF RID: 1759 RVA: 0x00024AFC File Offset: 0x00022CFC
	protected override void Update()
	{
		base.Update();
		if (Singleton<GameMaster>.Instance.stanleyActions.JumpAction.WasPressed)
		{
			this.OnClick(default(Vector3));
		}
	}

	// Token: 0x060006E0 RID: 1760 RVA: 0x00024B34 File Offset: 0x00022D34
	public override void OnClick(Vector3 point = default(Vector3))
	{
		base.OnClick(point);
		Singleton<GameMaster>.Instance.ClosePauseMenu(true);
	}
}
