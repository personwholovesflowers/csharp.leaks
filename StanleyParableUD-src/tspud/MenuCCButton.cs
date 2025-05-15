using System;
using UnityEngine;

// Token: 0x02000114 RID: 276
public class MenuCCButton : MenuButton
{
	// Token: 0x060006A6 RID: 1702 RVA: 0x00023ACB File Offset: 0x00021CCB
	private void OnEnable()
	{
		this.current = Singleton<GameMaster>.Instance.closedCaptionsOn;
		this.enabledText.SetActive(this.current);
		this.disabledText.SetActive(!this.current);
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x00023B04 File Offset: 0x00021D04
	public override void OnHover()
	{
		base.OnHover();
		if (Singleton<GameMaster>.Instance.stanleyActions.Right.WasPressed)
		{
			this.OnClick(default(Vector3));
			return;
		}
		if (Singleton<GameMaster>.Instance.stanleyActions.Left.WasPressed)
		{
			this.OnClick(default(Vector3));
		}
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x00023B64 File Offset: 0x00021D64
	public override void OnClick(Vector3 point = default(Vector3))
	{
		base.OnClick(point);
		this.current = Singleton<GameMaster>.Instance.closedCaptionsOn;
		this.enabledText.SetActive(!this.current);
		this.current = !this.current;
		this.disabledText.SetActive(!this.current);
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x00023BBF File Offset: 0x00021DBF
	public override void SaveChange()
	{
		base.SaveChange();
		if (this.original && this.original)
		{
			Singleton<GameMaster>.Instance.SetCaptionsActive(this.current);
		}
	}

	// Token: 0x040006FC RID: 1788
	public GameObject enabledText;

	// Token: 0x040006FD RID: 1789
	public GameObject disabledText;

	// Token: 0x040006FE RID: 1790
	private bool current = true;
}
