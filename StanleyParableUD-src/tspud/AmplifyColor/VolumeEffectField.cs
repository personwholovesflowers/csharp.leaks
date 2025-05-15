using System;
using System.Reflection;
using UnityEngine;

namespace AmplifyColor
{
	// Token: 0x0200031D RID: 797
	[Serializable]
	public class VolumeEffectField
	{
		// Token: 0x06001429 RID: 5161 RVA: 0x0006BAEF File Offset: 0x00069CEF
		public VolumeEffectField(string fieldName, string fieldType)
		{
			this.fieldName = fieldName;
			this.fieldType = fieldType;
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x0006BB08 File Offset: 0x00069D08
		public VolumeEffectField(FieldInfo pi, Component c)
			: this(pi.Name, pi.FieldType.FullName)
		{
			object value = pi.GetValue(c);
			this.UpdateValue(value);
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x0006BB3C File Offset: 0x00069D3C
		public static bool IsValidType(string type)
		{
			return type == "System.Single" || type == "System.Boolean" || type == "UnityEngine.Color" || type == "UnityEngine.Vector2" || type == "UnityEngine.Vector3" || type == "UnityEngine.Vector4";
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x0006BB9C File Offset: 0x00069D9C
		public void UpdateValue(object val)
		{
			string text = this.fieldType;
			if (text == "System.Single")
			{
				this.valueSingle = (float)val;
				return;
			}
			if (text == "System.Boolean")
			{
				this.valueBoolean = (bool)val;
				return;
			}
			if (text == "UnityEngine.Color")
			{
				this.valueColor = (Color)val;
				return;
			}
			if (text == "UnityEngine.Vector2")
			{
				this.valueVector2 = (Vector2)val;
				return;
			}
			if (text == "UnityEngine.Vector3")
			{
				this.valueVector3 = (Vector3)val;
				return;
			}
			if (!(text == "UnityEngine.Vector4"))
			{
				return;
			}
			this.valueVector4 = (Vector4)val;
		}

		// Token: 0x04001044 RID: 4164
		public string fieldName;

		// Token: 0x04001045 RID: 4165
		public string fieldType;

		// Token: 0x04001046 RID: 4166
		public float valueSingle;

		// Token: 0x04001047 RID: 4167
		public Color valueColor;

		// Token: 0x04001048 RID: 4168
		public bool valueBoolean;

		// Token: 0x04001049 RID: 4169
		public Vector2 valueVector2;

		// Token: 0x0400104A RID: 4170
		public Vector3 valueVector3;

		// Token: 0x0400104B RID: 4171
		public Vector4 valueVector4;
	}
}
