using System;
using UnityEngine;

// Token: 0x0200011D RID: 285
public class QuitButton : MenuButton
{
	// Token: 0x060006D3 RID: 1747 RVA: 0x00024A42 File Offset: 0x00022C42
	public override void OnClick(Vector3 point = default(Vector3))
	{
		base.OnClick(point);
		Application.Quit();
	}
}
