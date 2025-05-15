using System;
using UnityEngine;

// Token: 0x02000331 RID: 817
public class ParticleCluster : MonoBehaviour
{
	// Token: 0x060012E1 RID: 4833 RVA: 0x00096494 File Offset: 0x00094694
	private void Awake()
	{
		this.emissionModules = new ParticleSystem.EmissionModule[this.particles.Length];
		for (int i = 0; i < this.particles.Length; i++)
		{
			this.emissionModules[i] = this.particles[i].emission;
		}
	}

	// Token: 0x060012E2 RID: 4834 RVA: 0x000964E0 File Offset: 0x000946E0
	public void EmissionOn()
	{
		for (int i = 0; i < this.particles.Length; i++)
		{
			this.emissionModules[i].enabled = true;
		}
	}

	// Token: 0x060012E3 RID: 4835 RVA: 0x00096514 File Offset: 0x00094714
	public void EmissionOff()
	{
		for (int i = 0; i < this.particles.Length; i++)
		{
			this.emissionModules[i].enabled = false;
		}
	}

	// Token: 0x040019D7 RID: 6615
	[SerializeField]
	private ParticleSystem[] particles;

	// Token: 0x040019D8 RID: 6616
	private ParticleSystem.EmissionModule[] emissionModules;
}
