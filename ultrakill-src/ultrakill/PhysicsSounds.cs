using System;
using UnityEngine;

// Token: 0x02000334 RID: 820
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class PhysicsSounds : MonoSingleton<PhysicsSounds>
{
	// Token: 0x060012EE RID: 4846 RVA: 0x00096A88 File Offset: 0x00094C88
	public AudioClip ResolveSound(PhysicsSounds.PhysMaterial material)
	{
		switch (material)
		{
		case PhysicsSounds.PhysMaterial.Plastic:
			return this.sounds.plastic;
		case PhysicsSounds.PhysMaterial.Wood:
			return this.sounds.wood;
		case PhysicsSounds.PhysMaterial.Stone:
			return this.sounds.stone;
		case PhysicsSounds.PhysMaterial.Metal:
			return this.sounds.metal;
		case PhysicsSounds.PhysMaterial.Fleshy:
			return this.sounds.fleshy;
		case PhysicsSounds.PhysMaterial.Glass:
			return this.sounds.glass;
		case PhysicsSounds.PhysMaterial.Grass:
			return this.sounds.grass;
		default:
			return this.sounds.plastic;
		}
	}

	// Token: 0x060012EF RID: 4847 RVA: 0x00096B18 File Offset: 0x00094D18
	public void ImpactAt(Vector3 point, float magnitude, PhysicsSounds.PhysMaterial material)
	{
		if (magnitude < 3.5f)
		{
			return;
		}
		AudioSource audioSource = Object.Instantiate<AudioSource>(this.template);
		audioSource.transform.position = point;
		audioSource.clip = this.ResolveSound(material);
		audioSource.volume = Mathf.Lerp(0.2f, 1f, magnitude / 60f);
		audioSource.pitch = Mathf.Lerp(0.65f, 2.2f, (60f - magnitude) / 60f);
		audioSource.gameObject.SetActive(true);
		audioSource.Play();
	}

	// Token: 0x040019E9 RID: 6633
	[SerializeField]
	private PhysicsSounds.PhysSounds sounds;

	// Token: 0x040019EA RID: 6634
	[SerializeField]
	private AudioSource template;

	// Token: 0x02000335 RID: 821
	[Serializable]
	public struct PhysSounds
	{
		// Token: 0x040019EB RID: 6635
		public AudioClip plastic;

		// Token: 0x040019EC RID: 6636
		public AudioClip wood;

		// Token: 0x040019ED RID: 6637
		public AudioClip stone;

		// Token: 0x040019EE RID: 6638
		public AudioClip metal;

		// Token: 0x040019EF RID: 6639
		public AudioClip fleshy;

		// Token: 0x040019F0 RID: 6640
		public AudioClip glass;

		// Token: 0x040019F1 RID: 6641
		public AudioClip grass;
	}

	// Token: 0x02000336 RID: 822
	public enum PhysMaterial
	{
		// Token: 0x040019F3 RID: 6643
		Plastic,
		// Token: 0x040019F4 RID: 6644
		Wood,
		// Token: 0x040019F5 RID: 6645
		Stone,
		// Token: 0x040019F6 RID: 6646
		Metal,
		// Token: 0x040019F7 RID: 6647
		Fleshy,
		// Token: 0x040019F8 RID: 6648
		Glass,
		// Token: 0x040019F9 RID: 6649
		Grass
	}
}
