using System;
using ScriptableObjects;
using UnityEngine;

// Token: 0x02000104 RID: 260
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class DefaultReferenceManager : MonoSingleton<DefaultReferenceManager>
{
	// Token: 0x040006D2 RID: 1746
	public GameObject wetParticle;

	// Token: 0x040006D3 RID: 1747
	public GameObject sandDrip;

	// Token: 0x040006D4 RID: 1748
	public GameObject blessingGlow;

	// Token: 0x040006D5 RID: 1749
	public GameObject sandificationEffect;

	// Token: 0x040006D6 RID: 1750
	public GameObject enrageEffect;

	// Token: 0x040006D7 RID: 1751
	public GameObject ineffectiveSound;

	// Token: 0x040006D8 RID: 1752
	public GameObject continuousSplash;

	// Token: 0x040006D9 RID: 1753
	public GameObject splash;

	// Token: 0x040006DA RID: 1754
	public GameObject smallSplash;

	// Token: 0x040006DB RID: 1755
	public GameObject bubbles;

	// Token: 0x040006DC RID: 1756
	public GameObject projectile;

	// Token: 0x040006DD RID: 1757
	public GameObject projectileExplosive;

	// Token: 0x040006DE RID: 1758
	public GameObject parryableFlash;

	// Token: 0x040006DF RID: 1759
	public GameObject unparryableFlash;

	// Token: 0x040006E0 RID: 1760
	public GameObject explosion;

	// Token: 0x040006E1 RID: 1761
	public GameObject superExplosion;

	// Token: 0x040006E2 RID: 1762
	public Material puppetMaterial;

	// Token: 0x040006E3 RID: 1763
	public GameObject puppetSpawn;

	// Token: 0x040006E4 RID: 1764
	public Material blankMaterial;

	// Token: 0x040006E5 RID: 1765
	public GameObject madnessEffect;

	// Token: 0x040006E6 RID: 1766
	public LineRenderer electricLine;

	// Token: 0x040006E7 RID: 1767
	public GameObject zapImpactParticle;

	// Token: 0x040006E8 RID: 1768
	public FootstepSet footstepSet;

	// Token: 0x040006E9 RID: 1769
	public GameObject radianceEffect;

	// Token: 0x040006EA RID: 1770
	public Shader masterShader;
}
