using System;
using UnityEngine;

// Token: 0x020003E7 RID: 999
[Serializable]
public class ShaderProperty
{
	// Token: 0x06001685 RID: 5765 RVA: 0x000B51A4 File Offset: 0x000B33A4
	public void Set(Material material)
	{
		if (this.setFloat)
		{
			material.SetFloat(this.name, this.floatValue);
		}
		if (this.setInt)
		{
			material.SetInt(this.name, this.intValue);
		}
		if (this.setVector)
		{
			material.SetVector(this.name, this.vectorValue);
		}
		if (this.setColor)
		{
			material.SetColor(this.name, this.colorValue);
		}
	}

	// Token: 0x04001F15 RID: 7957
	public string name;

	// Token: 0x04001F16 RID: 7958
	public bool setFloat = true;

	// Token: 0x04001F17 RID: 7959
	public float floatValue;

	// Token: 0x04001F18 RID: 7960
	public bool setInt = true;

	// Token: 0x04001F19 RID: 7961
	public int intValue;

	// Token: 0x04001F1A RID: 7962
	public bool setVector = true;

	// Token: 0x04001F1B RID: 7963
	public Vector4 vectorValue = Vector4.zero;

	// Token: 0x04001F1C RID: 7964
	public bool setColor = true;

	// Token: 0x04001F1D RID: 7965
	public Color colorValue = Color.black;
}
