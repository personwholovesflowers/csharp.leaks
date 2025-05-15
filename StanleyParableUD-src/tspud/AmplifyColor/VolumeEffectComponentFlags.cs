using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AmplifyColor
{
	// Token: 0x02000322 RID: 802
	[Serializable]
	public class VolumeEffectComponentFlags
	{
		// Token: 0x06001448 RID: 5192 RVA: 0x0006C99D File Offset: 0x0006AB9D
		public VolumeEffectComponentFlags(string name)
		{
			this.componentName = name;
			this.componentFields = new List<VolumeEffectFieldFlags>();
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x0006C9B8 File Offset: 0x0006ABB8
		public VolumeEffectComponentFlags(VolumeEffectComponent comp)
			: this(comp.componentName)
		{
			this.blendFlag = true;
			foreach (VolumeEffectField volumeEffectField in comp.fields)
			{
				if (VolumeEffectField.IsValidType(volumeEffectField.fieldType))
				{
					this.componentFields.Add(new VolumeEffectFieldFlags(volumeEffectField));
				}
			}
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x0006CA38 File Offset: 0x0006AC38
		public VolumeEffectComponentFlags(Component c)
			: this(string.Concat(c.GetType()))
		{
			foreach (FieldInfo fieldInfo in c.GetType().GetFields())
			{
				if (VolumeEffectField.IsValidType(fieldInfo.FieldType.FullName))
				{
					this.componentFields.Add(new VolumeEffectFieldFlags(fieldInfo));
				}
			}
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x0006CA98 File Offset: 0x0006AC98
		public void UpdateComponentFlags(VolumeEffectComponent comp)
		{
			using (List<VolumeEffectField>.Enumerator enumerator = comp.fields.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					VolumeEffectField field = enumerator.Current;
					if (this.componentFields.Find((VolumeEffectFieldFlags s) => s.fieldName == field.fieldName) == null && VolumeEffectField.IsValidType(field.fieldType))
					{
						this.componentFields.Add(new VolumeEffectFieldFlags(field));
					}
				}
			}
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x0006CB30 File Offset: 0x0006AD30
		public void UpdateComponentFlags(Component c)
		{
			FieldInfo[] fields = c.GetType().GetFields();
			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo pi = fields[i];
				if (!this.componentFields.Exists((VolumeEffectFieldFlags s) => s.fieldName == pi.Name) && VolumeEffectField.IsValidType(pi.FieldType.FullName))
				{
					this.componentFields.Add(new VolumeEffectFieldFlags(pi));
				}
			}
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x0006CBAC File Offset: 0x0006ADAC
		public string[] GetFieldNames()
		{
			return (from r in this.componentFields
				where r.blendFlag
				select r.fieldName).ToArray<string>();
		}

		// Token: 0x04001054 RID: 4180
		public string componentName;

		// Token: 0x04001055 RID: 4181
		public List<VolumeEffectFieldFlags> componentFields;

		// Token: 0x04001056 RID: 4182
		public bool blendFlag;
	}
}
