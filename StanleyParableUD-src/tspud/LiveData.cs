using System;

// Token: 0x02000031 RID: 49
[Serializable]
public class LiveData
{
	// Token: 0x06000101 RID: 257 RVA: 0x00008B05 File Offset: 0x00006D05
	public LiveData(string _key, ConfigurableTypes _type)
	{
		this.key = _key;
		this.configureableType = _type;
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00008B1B File Offset: 0x00006D1B
	public void CopyValuesFrom(LiveData data)
	{
		this.IntValue = data.IntValue;
		this.FloatValue = data.FloatValue;
		this.BooleanValue = data.BooleanValue;
		this.StringValue = data.StringValue;
	}

	// Token: 0x04000171 RID: 369
	public string key;

	// Token: 0x04000172 RID: 370
	public ConfigurableTypes configureableType;

	// Token: 0x04000173 RID: 371
	public int IntValue;

	// Token: 0x04000174 RID: 372
	public float FloatValue;

	// Token: 0x04000175 RID: 373
	public bool BooleanValue;

	// Token: 0x04000176 RID: 374
	public string StringValue;
}
