using System;
using Newtonsoft.Json;
using UnityEngine;

// Token: 0x020003AC RID: 940
[Serializable]
public class SavedAlterOption
{
	// Token: 0x04001DC7 RID: 7623
	public string Key;

	// Token: 0x04001DC8 RID: 7624
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public float? FloatValue;

	// Token: 0x04001DC9 RID: 7625
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public bool? BoolValue;

	// Token: 0x04001DCA RID: 7626
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public Vector3? VectorData;

	// Token: 0x04001DCB RID: 7627
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public int? IntValue;
}
