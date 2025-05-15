using System;
using Newtonsoft.Json;

// Token: 0x020003A8 RID: 936
[Serializable]
public class SavedGeneric
{
	// Token: 0x04001DBD RID: 7613
	public string ObjectIdentifier;

	// Token: 0x04001DBE RID: 7614
	public SavedVector3 Position;

	// Token: 0x04001DBF RID: 7615
	public SavedQuaternion Rotation;

	// Token: 0x04001DC0 RID: 7616
	public SavedVector3 Scale;

	// Token: 0x04001DC1 RID: 7617
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public SavedAlterData[] Data;

	// Token: 0x04001DC2 RID: 7618
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
	public bool DisallowManipulation;

	// Token: 0x04001DC3 RID: 7619
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
	public bool DisallowFreezing;
}
