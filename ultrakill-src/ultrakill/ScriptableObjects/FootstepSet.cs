using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
	// Token: 0x0200054E RID: 1358
	[CreateAssetMenu(fileName = "Footstep Set", menuName = "ULTRAKILL/FootstepSet")]
	public class FootstepSet : ScriptableObject
	{
		// Token: 0x06001EA5 RID: 7845 RVA: 0x000FCC29 File Offset: 0x000FAE29
		public bool TryGetFootstepClips(SurfaceType surfaceType, out AudioClip[] clips)
		{
			this.Initialize();
			return this.footstepsDictionary.TryGetValue(surfaceType, out clips) || this.footstepsDictionary.TryGetValue(SurfaceType.Generic, out clips);
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x000FCC52 File Offset: 0x000FAE52
		public bool TryGetEnviroGibs(SurfaceType surfaceType, out GameObject[] enviroGibs)
		{
			this.Initialize();
			return this.enviroGibsDictionary.TryGetValue(surfaceType, out enviroGibs) || this.enviroGibsDictionary.TryGetValue(SurfaceType.Generic, out enviroGibs);
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x000FCC7B File Offset: 0x000FAE7B
		public bool TryGetEnviroGibParticle(SurfaceType surface, out GameObject particle)
		{
			this.Initialize();
			return this.enviroGibParticleDictionary.TryGetValue(surface, out particle) || this.enviroGibParticleDictionary.TryGetValue(SurfaceType.Generic, out particle);
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x000FCCA4 File Offset: 0x000FAEA4
		public bool TryGetSlideParticle(SurfaceType surface, out GameObject particle)
		{
			this.Initialize();
			return this.slideParticlesDictionary.TryGetValue(surface, out particle) || this.slideParticlesDictionary.TryGetValue(SurfaceType.Generic, out particle);
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x000FCCCD File Offset: 0x000FAECD
		public bool TryGetWallScrapeParticle(SurfaceType surface, out GameObject particle)
		{
			this.Initialize();
			return this.wallScrapeParticlesDictionary.TryGetValue(surface, out particle) || this.wallScrapeParticlesDictionary.TryGetValue(SurfaceType.Generic, out particle);
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x000FCCF8 File Offset: 0x000FAEF8
		private void Initialize()
		{
			if (this.initialized)
			{
				return;
			}
			this.initialized = true;
			this.footstepsDictionary = new Dictionary<SurfaceType, AudioClip[]>();
			foreach (FootstepSet.Footsteps footsteps in this.footsteps)
			{
				this.footstepsDictionary[footsteps.SurfaceType] = footsteps.Clips;
			}
			this.enviroGibsDictionary = new Dictionary<SurfaceType, GameObject[]>();
			foreach (FootstepSet.EnviroGibs enviroGibs in this.enviroGibs)
			{
				this.enviroGibsDictionary[enviroGibs.SurfaceType] = enviroGibs.gibs;
			}
			this.enviroGibParticleDictionary = new Dictionary<SurfaceType, GameObject>();
			foreach (FootstepSet.EnviroGibParticles enviroGibParticles in this.enviroGibParticles)
			{
				this.enviroGibParticleDictionary[enviroGibParticles.SurfaceType] = enviroGibParticles.particle;
			}
			this.slideParticlesDictionary = new Dictionary<SurfaceType, GameObject>();
			foreach (FootstepSet.SlideParticles slideParticles in this.slideParticles)
			{
				this.slideParticlesDictionary[slideParticles.SurfaceType] = slideParticles.particle;
			}
			this.wallScrapeParticlesDictionary = new Dictionary<SurfaceType, GameObject>();
			foreach (FootstepSet.WallScrapeParticles wallScrapeParticles in this.wallScrapeParticles)
			{
				this.wallScrapeParticlesDictionary[wallScrapeParticles.SurfaceType] = wallScrapeParticles.particle;
			}
		}

		// Token: 0x04002B1D RID: 11037
		[SerializeField]
		private FootstepSet.Footsteps[] footsteps;

		// Token: 0x04002B1E RID: 11038
		[SerializeField]
		private FootstepSet.EnviroGibs[] enviroGibs;

		// Token: 0x04002B1F RID: 11039
		[SerializeField]
		private FootstepSet.EnviroGibParticles[] enviroGibParticles;

		// Token: 0x04002B20 RID: 11040
		[SerializeField]
		private FootstepSet.SlideParticles[] slideParticles;

		// Token: 0x04002B21 RID: 11041
		[SerializeField]
		private FootstepSet.WallScrapeParticles[] wallScrapeParticles;

		// Token: 0x04002B22 RID: 11042
		[NonSerialized]
		private Dictionary<SurfaceType, AudioClip[]> footstepsDictionary;

		// Token: 0x04002B23 RID: 11043
		[NonSerialized]
		private Dictionary<SurfaceType, GameObject[]> enviroGibsDictionary;

		// Token: 0x04002B24 RID: 11044
		[NonSerialized]
		private Dictionary<SurfaceType, GameObject> enviroGibParticleDictionary;

		// Token: 0x04002B25 RID: 11045
		[NonSerialized]
		private Dictionary<SurfaceType, GameObject> slideParticlesDictionary;

		// Token: 0x04002B26 RID: 11046
		[NonSerialized]
		private Dictionary<SurfaceType, GameObject> wallScrapeParticlesDictionary;

		// Token: 0x04002B27 RID: 11047
		[NonSerialized]
		private bool initialized;

		// Token: 0x0200054F RID: 1359
		[Serializable]
		public class Footsteps
		{
			// Token: 0x17000217 RID: 535
			// (get) Token: 0x06001EAC RID: 7852 RVA: 0x000FCE51 File Offset: 0x000FB051
			// (set) Token: 0x06001EAD RID: 7853 RVA: 0x000FCE59 File Offset: 0x000FB059
			public SurfaceType SurfaceType { get; private set; }

			// Token: 0x17000218 RID: 536
			// (get) Token: 0x06001EAE RID: 7854 RVA: 0x000FCE62 File Offset: 0x000FB062
			// (set) Token: 0x06001EAF RID: 7855 RVA: 0x000FCE6A File Offset: 0x000FB06A
			public AudioClip[] Clips { get; private set; }
		}

		// Token: 0x02000550 RID: 1360
		[Serializable]
		public class EnviroGibs
		{
			// Token: 0x17000219 RID: 537
			// (get) Token: 0x06001EB1 RID: 7857 RVA: 0x000FCE73 File Offset: 0x000FB073
			// (set) Token: 0x06001EB2 RID: 7858 RVA: 0x000FCE7B File Offset: 0x000FB07B
			public SurfaceType SurfaceType { get; private set; }

			// Token: 0x1700021A RID: 538
			// (get) Token: 0x06001EB3 RID: 7859 RVA: 0x000FCE84 File Offset: 0x000FB084
			// (set) Token: 0x06001EB4 RID: 7860 RVA: 0x000FCE8C File Offset: 0x000FB08C
			public GameObject[] gibs { get; private set; }
		}

		// Token: 0x02000551 RID: 1361
		[Serializable]
		public class EnviroGibParticles
		{
			// Token: 0x1700021B RID: 539
			// (get) Token: 0x06001EB6 RID: 7862 RVA: 0x000FCE95 File Offset: 0x000FB095
			// (set) Token: 0x06001EB7 RID: 7863 RVA: 0x000FCE9D File Offset: 0x000FB09D
			public SurfaceType SurfaceType { get; private set; }

			// Token: 0x1700021C RID: 540
			// (get) Token: 0x06001EB8 RID: 7864 RVA: 0x000FCEA6 File Offset: 0x000FB0A6
			// (set) Token: 0x06001EB9 RID: 7865 RVA: 0x000FCEAE File Offset: 0x000FB0AE
			public GameObject particle { get; private set; }
		}

		// Token: 0x02000552 RID: 1362
		[Serializable]
		public class SlideParticles
		{
			// Token: 0x1700021D RID: 541
			// (get) Token: 0x06001EBB RID: 7867 RVA: 0x000FCEB7 File Offset: 0x000FB0B7
			// (set) Token: 0x06001EBC RID: 7868 RVA: 0x000FCEBF File Offset: 0x000FB0BF
			public SurfaceType SurfaceType { get; private set; }

			// Token: 0x1700021E RID: 542
			// (get) Token: 0x06001EBD RID: 7869 RVA: 0x000FCEC8 File Offset: 0x000FB0C8
			// (set) Token: 0x06001EBE RID: 7870 RVA: 0x000FCED0 File Offset: 0x000FB0D0
			public GameObject particle { get; private set; }
		}

		// Token: 0x02000553 RID: 1363
		[Serializable]
		public class WallScrapeParticles
		{
			// Token: 0x1700021F RID: 543
			// (get) Token: 0x06001EC0 RID: 7872 RVA: 0x000FCED9 File Offset: 0x000FB0D9
			// (set) Token: 0x06001EC1 RID: 7873 RVA: 0x000FCEE1 File Offset: 0x000FB0E1
			public SurfaceType SurfaceType { get; private set; }

			// Token: 0x17000220 RID: 544
			// (get) Token: 0x06001EC2 RID: 7874 RVA: 0x000FCEEA File Offset: 0x000FB0EA
			// (set) Token: 0x06001EC3 RID: 7875 RVA: 0x000FCEF2 File Offset: 0x000FB0F2
			public GameObject particle { get; private set; }
		}
	}
}
