using System;
using UnityEngine;

// Token: 0x0200011B RID: 283
public class MenuToggleButton : MenuButton
{
	// Token: 0x060006C7 RID: 1735 RVA: 0x000242F8 File Offset: 0x000224F8
	public override void OnClick(Vector3 point = default(Vector3))
	{
		base.OnClick(point);
		if (this.toShow != null && this.toHide != null)
		{
			this.toShow.SetActive(!this.toShow.activeInHierarchy);
			this.toHide.SetActive(!this.toHide.activeInHierarchy);
		}
	}

	// Token: 0x060006C8 RID: 1736 RVA: 0x0002435C File Offset: 0x0002255C
	protected override void Update()
	{
		base.Update();
		if (this.hijackCancel && Singleton<GameMaster>.Instance.stanleyActions.JumpAction.WasPressed)
		{
			this.OnClick(default(Vector3));
		}
	}

	// Token: 0x060006C9 RID: 1737 RVA: 0x0002439C File Offset: 0x0002259C
	public override void OnHover()
	{
		base.OnHover();
		if (this.useLeftRight && (Singleton<GameMaster>.Instance.stanleyActions.Left.WasPressed || Singleton<GameMaster>.Instance.stanleyActions.Right.WasPressed))
		{
			this.OnClick(default(Vector3));
		}
	}

	// Token: 0x04000715 RID: 1813
	public GameObject toHide;

	// Token: 0x04000716 RID: 1814
	public GameObject toShow;

	// Token: 0x04000717 RID: 1815
	[Space(5f)]
	public bool hijackCancel;

	// Token: 0x04000718 RID: 1816
	public bool useLeftRight;
}
