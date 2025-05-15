using System;

// Token: 0x02000399 RID: 921
public class RumbleKey
{
	// Token: 0x1700017C RID: 380
	// (get) Token: 0x06001525 RID: 5413 RVA: 0x000AD0B6 File Offset: 0x000AB2B6
	// (set) Token: 0x06001526 RID: 5414 RVA: 0x000AD0BE File Offset: 0x000AB2BE
	public string name { get; private set; }

	// Token: 0x1700017D RID: 381
	// (get) Token: 0x06001527 RID: 5415 RVA: 0x000AD0C7 File Offset: 0x000AB2C7
	// (set) Token: 0x06001528 RID: 5416 RVA: 0x000AD0CF File Offset: 0x000AB2CF
	public int hashKey { get; private set; }

	// Token: 0x06001529 RID: 5417 RVA: 0x000AD0D8 File Offset: 0x000AB2D8
	public RumbleKey(string name)
	{
		this.name = name;
		this.hashKey = name.GetHashCode();
	}

	// Token: 0x0600152A RID: 5418 RVA: 0x000AD0F3 File Offset: 0x000AB2F3
	public override string ToString()
	{
		return this.name;
	}

	// Token: 0x0600152B RID: 5419 RVA: 0x000AD0FC File Offset: 0x000AB2FC
	public override bool Equals(object obj)
	{
		RumbleKey rumbleKey = obj as RumbleKey;
		return rumbleKey != null && rumbleKey.hashKey == this.hashKey;
	}

	// Token: 0x0600152C RID: 5420 RVA: 0x000AD123 File Offset: 0x000AB323
	public override int GetHashCode()
	{
		return this.hashKey;
	}
}
