using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AmplifyColor
{
	// Token: 0x02000323 RID: 803
	[Serializable]
	public class VolumeEffectFlags
	{
		// Token: 0x0600144E RID: 5198 RVA: 0x0006CC0C File Offset: 0x0006AE0C
		public VolumeEffectFlags()
		{
			this.components = new List<VolumeEffectComponentFlags>();
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0006CC20 File Offset: 0x0006AE20
		public void AddComponent(Component c)
		{
			VolumeEffectComponentFlags volumeEffectComponentFlags;
			if ((volumeEffectComponentFlags = this.components.Find((VolumeEffectComponentFlags s) => s.componentName == string.Concat(c.GetType()))) != null)
			{
				volumeEffectComponentFlags.UpdateComponentFlags(c);
				return;
			}
			this.components.Add(new VolumeEffectComponentFlags(c));
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x0006CC78 File Offset: 0x0006AE78
		public void UpdateFlags(VolumeEffect effectVol)
		{
			using (List<VolumeEffectComponent>.Enumerator enumerator = effectVol.components.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					VolumeEffectComponent comp = enumerator.Current;
					VolumeEffectComponentFlags volumeEffectComponentFlags;
					if ((volumeEffectComponentFlags = this.components.Find((VolumeEffectComponentFlags s) => s.componentName == comp.componentName)) == null)
					{
						this.components.Add(new VolumeEffectComponentFlags(comp));
					}
					else
					{
						volumeEffectComponentFlags.UpdateComponentFlags(comp);
					}
				}
			}
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x0006CD10 File Offset: 0x0006AF10
		public static void UpdateCamFlags(AmplifyColorBase[] effects, AmplifyColorVolumeBase[] volumes)
		{
			foreach (AmplifyColorBase amplifyColorBase in effects)
			{
				amplifyColorBase.EffectFlags = new VolumeEffectFlags();
				for (int j = 0; j < volumes.Length; j++)
				{
					VolumeEffect volumeEffect = volumes[j].EffectContainer.FindVolumeEffect(amplifyColorBase);
					if (volumeEffect != null)
					{
						amplifyColorBase.EffectFlags.UpdateFlags(volumeEffect);
					}
				}
			}
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x0006CD74 File Offset: 0x0006AF74
		public VolumeEffect GenerateEffectData(AmplifyColorBase go)
		{
			VolumeEffect volumeEffect = new VolumeEffect(go);
			foreach (VolumeEffectComponentFlags volumeEffectComponentFlags in this.components)
			{
				if (volumeEffectComponentFlags.blendFlag)
				{
					Component component = go.GetComponent(volumeEffectComponentFlags.componentName);
					if (component != null)
					{
						volumeEffect.AddComponent(component, volumeEffectComponentFlags);
					}
				}
			}
			return volumeEffect;
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x0006CDF0 File Offset: 0x0006AFF0
		public VolumeEffectComponentFlags FindComponentFlags(string compName)
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				if (this.components[i].componentName == compName)
				{
					return this.components[i];
				}
			}
			return null;
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x0006CE3C File Offset: 0x0006B03C
		public string[] GetComponentNames()
		{
			return (from r in this.components
				where r.blendFlag
				select r.componentName).ToArray<string>();
		}

		// Token: 0x04001057 RID: 4183
		public List<VolumeEffectComponentFlags> components;
	}
}
