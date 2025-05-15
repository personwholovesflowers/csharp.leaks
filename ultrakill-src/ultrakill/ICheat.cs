using System;
using System.Collections;

// Token: 0x020000B6 RID: 182
public interface ICheat
{
	// Token: 0x1700005D RID: 93
	// (get) Token: 0x0600039E RID: 926
	string LongName { get; }

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x0600039F RID: 927
	string Identifier { get; }

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x060003A0 RID: 928
	string ButtonEnabledOverride { get; }

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x060003A1 RID: 929
	string ButtonDisabledOverride { get; }

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x060003A2 RID: 930
	string Icon { get; }

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x060003A3 RID: 931
	bool DefaultState { get; }

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x060003A4 RID: 932
	StatePersistenceMode PersistenceMode { get; }

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x060003A5 RID: 933
	bool IsActive { get; }

	// Token: 0x060003A6 RID: 934
	void Enable(CheatsManager manager);

	// Token: 0x060003A7 RID: 935
	void Disable();

	// Token: 0x060003A8 RID: 936 RVA: 0x00016DE5 File Offset: 0x00014FE5
	IEnumerator Coroutine(CheatsManager manager)
	{
		yield break;
	}
}
