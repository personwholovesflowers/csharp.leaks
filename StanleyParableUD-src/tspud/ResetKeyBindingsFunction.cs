using System;
using UnityEngine;

// Token: 0x0200011E RID: 286
public class ResetKeyBindingsFunction : MonoBehaviour
{
	// Token: 0x060006D5 RID: 1749 RVA: 0x00024A50 File Offset: 0x00022C50
	private void Start()
	{
		this.keybindingString.Init();
		this.LoadKeyBindings();
	}

	// Token: 0x060006D6 RID: 1750 RVA: 0x00024A63 File Offset: 0x00022C63
	public void ResetKeyBindings()
	{
		Singleton<GameMaster>.Instance.stanleyActions.ResetKeyBindings(this.keybindingString);
	}

	// Token: 0x060006D7 RID: 1751 RVA: 0x00024A7A File Offset: 0x00022C7A
	public void LoadKeyBindings()
	{
		Singleton<GameMaster>.Instance.stanleyActions.LoadCustomKeyBindings(this.keybindingString);
	}

	// Token: 0x04000728 RID: 1832
	public StringConfigurable keybindingString;
}
