using System;
using System.Collections.Generic;
using System.Linq;

namespace AmplifyColor
{
	// Token: 0x02000320 RID: 800
	[Serializable]
	public class VolumeEffectContainer
	{
		// Token: 0x06001440 RID: 5184 RVA: 0x0006C7F7 File Offset: 0x0006A9F7
		public VolumeEffectContainer()
		{
			this.volumes = new List<VolumeEffect>();
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x0006C80C File Offset: 0x0006AA0C
		public void AddColorEffect(AmplifyColorBase colorEffect)
		{
			VolumeEffect volumeEffect;
			if ((volumeEffect = this.FindVolumeEffect(colorEffect)) != null)
			{
				volumeEffect.UpdateVolume();
				return;
			}
			volumeEffect = new VolumeEffect(colorEffect);
			this.volumes.Add(volumeEffect);
			volumeEffect.UpdateVolume();
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x0006C844 File Offset: 0x0006AA44
		public VolumeEffect AddJustColorEffect(AmplifyColorBase colorEffect)
		{
			VolumeEffect volumeEffect = new VolumeEffect(colorEffect);
			this.volumes.Add(volumeEffect);
			return volumeEffect;
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x0006C868 File Offset: 0x0006AA68
		public VolumeEffect FindVolumeEffect(AmplifyColorBase colorEffect)
		{
			for (int i = 0; i < this.volumes.Count; i++)
			{
				if (this.volumes[i].gameObject == colorEffect)
				{
					return this.volumes[i];
				}
			}
			for (int j = 0; j < this.volumes.Count; j++)
			{
				if (this.volumes[j].gameObject != null && this.volumes[j].gameObject.SharedInstanceID == colorEffect.SharedInstanceID)
				{
					return this.volumes[j];
				}
			}
			return null;
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x0006C911 File Offset: 0x0006AB11
		public void RemoveVolumeEffect(VolumeEffect volume)
		{
			this.volumes.Remove(volume);
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x0006C920 File Offset: 0x0006AB20
		public AmplifyColorBase[] GetStoredEffects()
		{
			return this.volumes.Select((VolumeEffect r) => r.gameObject).ToArray<AmplifyColorBase>();
		}

		// Token: 0x04001050 RID: 4176
		public List<VolumeEffect> volumes;
	}
}
