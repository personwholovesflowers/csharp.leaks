using System;
using UnityEngine;

// Token: 0x02000085 RID: 133
[CreateAssetMenu(fileName = "New Reverb Profile", menuName = "Post Effects Control/Create new Reverb Profile")]
public class ReverbProfile : ProfileBase
{
	// Token: 0x06000331 RID: 817 RVA: 0x00015D9C File Offset: 0x00013F9C
	public ReverbProfile(AudioReverbZone zone)
	{
		this.Data.Room = zone.room;
		this.Data.RoomHF = zone.roomHF;
		this.Data.RoomLF = zone.roomLF;
		this.Data.DecayTime = zone.decayTime;
		this.Data.DecayHFRatio = zone.decayHFRatio;
		this.Data.Reflections = (float)zone.reflections;
		this.Data.ReflectionsDelay = zone.reflectionsDelay;
		this.Data.Reverb = (float)zone.reverb;
		this.Data.ReverbDelay = zone.reverbDelay;
		this.Data.HFReference = zone.HFReference;
		this.Data.LFReference = zone.LFReference;
		this.Data.Diffusion = zone.diffusion;
		this.Data.Density = zone.density;
	}

	// Token: 0x0400032E RID: 814
	[SerializeField]
	public ReverbProfile.ReverbData Data;

	// Token: 0x02000386 RID: 902
	[Serializable]
	public struct ReverbData
	{
		// Token: 0x040012D4 RID: 4820
		[Range(-10000f, 0f)]
		public int Room;

		// Token: 0x040012D5 RID: 4821
		[Range(-10000f, 0f)]
		public int RoomHF;

		// Token: 0x040012D6 RID: 4822
		[Range(-10000f, 0f)]
		public int RoomLF;

		// Token: 0x040012D7 RID: 4823
		[Range(-10000f, 20f)]
		public float DecayTime;

		// Token: 0x040012D8 RID: 4824
		[Range(-10000f, 2f)]
		public float DecayHFRatio;

		// Token: 0x040012D9 RID: 4825
		[Range(-10000f, 1000f)]
		public float Reflections;

		// Token: 0x040012DA RID: 4826
		[Range(-10000f, 0.3f)]
		public float ReflectionsDelay;

		// Token: 0x040012DB RID: 4827
		[Range(-10000f, 2000f)]
		public float Reverb;

		// Token: 0x040012DC RID: 4828
		[Range(-10000f, 0.1f)]
		public float ReverbDelay;

		// Token: 0x040012DD RID: 4829
		[Range(-10000f, 20000f)]
		public float HFReference;

		// Token: 0x040012DE RID: 4830
		[Range(-10000f, 1000f)]
		public float LFReference;

		// Token: 0x040012DF RID: 4831
		[Range(-10000f, 100f)]
		public float Diffusion;

		// Token: 0x040012E0 RID: 4832
		[Range(-10000f, 100f)]
		public float Density;
	}
}
