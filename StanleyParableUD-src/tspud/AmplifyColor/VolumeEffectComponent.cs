using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AmplifyColor
{
	// Token: 0x0200031E RID: 798
	[Serializable]
	public class VolumeEffectComponent
	{
		// Token: 0x0600142D RID: 5165 RVA: 0x0006BC4C File Offset: 0x00069E4C
		public VolumeEffectComponent(string name)
		{
			this.componentName = name;
			this.fields = new List<VolumeEffectField>();
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x0006BC66 File Offset: 0x00069E66
		public VolumeEffectField AddField(FieldInfo pi, Component c)
		{
			return this.AddField(pi, c, -1);
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x0006BC74 File Offset: 0x00069E74
		public VolumeEffectField AddField(FieldInfo pi, Component c, int position)
		{
			VolumeEffectField volumeEffectField = (VolumeEffectField.IsValidType(pi.FieldType.FullName) ? new VolumeEffectField(pi, c) : null);
			if (volumeEffectField != null)
			{
				if (position < 0 || position >= this.fields.Count)
				{
					this.fields.Add(volumeEffectField);
				}
				else
				{
					this.fields.Insert(position, volumeEffectField);
				}
			}
			return volumeEffectField;
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x0006BCCF File Offset: 0x00069ECF
		public void RemoveEffectField(VolumeEffectField field)
		{
			this.fields.Remove(field);
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x0006BCE0 File Offset: 0x00069EE0
		public VolumeEffectComponent(Component c, VolumeEffectComponentFlags compFlags)
			: this(compFlags.componentName)
		{
			foreach (VolumeEffectFieldFlags volumeEffectFieldFlags in compFlags.componentFields)
			{
				if (volumeEffectFieldFlags.blendFlag)
				{
					FieldInfo field = c.GetType().GetField(volumeEffectFieldFlags.fieldName);
					VolumeEffectField volumeEffectField = (VolumeEffectField.IsValidType(field.FieldType.FullName) ? new VolumeEffectField(field, c) : null);
					if (volumeEffectField != null)
					{
						this.fields.Add(volumeEffectField);
					}
				}
			}
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x0006BD80 File Offset: 0x00069F80
		public void UpdateComponent(Component c, VolumeEffectComponentFlags compFlags)
		{
			using (List<VolumeEffectFieldFlags>.Enumerator enumerator = compFlags.componentFields.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					VolumeEffectFieldFlags fieldFlags = enumerator.Current;
					if (fieldFlags.blendFlag && !this.fields.Exists((VolumeEffectField s) => s.fieldName == fieldFlags.fieldName))
					{
						FieldInfo field = c.GetType().GetField(fieldFlags.fieldName);
						VolumeEffectField volumeEffectField = (VolumeEffectField.IsValidType(field.FieldType.FullName) ? new VolumeEffectField(field, c) : null);
						if (volumeEffectField != null)
						{
							this.fields.Add(volumeEffectField);
						}
					}
				}
			}
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x0006BE44 File Offset: 0x0006A044
		public VolumeEffectField FindEffectField(string fieldName)
		{
			for (int i = 0; i < this.fields.Count; i++)
			{
				if (this.fields[i].fieldName == fieldName)
				{
					return this.fields[i];
				}
			}
			return null;
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x0006BE90 File Offset: 0x0006A090
		public static FieldInfo[] ListAcceptableFields(Component c)
		{
			if (c == null)
			{
				return new FieldInfo[0];
			}
			return (from f in c.GetType().GetFields()
				where VolumeEffectField.IsValidType(f.FieldType.FullName)
				select f).ToArray<FieldInfo>();
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x0006BEE1 File Offset: 0x0006A0E1
		public string[] GetFieldNames()
		{
			return this.fields.Select((VolumeEffectField r) => r.fieldName).ToArray<string>();
		}

		// Token: 0x0400104C RID: 4172
		public string componentName;

		// Token: 0x0400104D RID: 4173
		public List<VolumeEffectField> fields;
	}
}
