using System;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

// Token: 0x0200078B RID: 1931
[DOTSCompilerGenerated]
internal class __JobReflectionRegistrationOutput__1221673671587648887
{
	// Token: 0x0600298F RID: 10639 RVA: 0x00112A6C File Offset: 0x00110C6C
	public static void CreateJobReflectionData()
	{
		try
		{
			IJobParallelForExtensions.EarlyJobInit<GenerateBloodMeshJob>();
			IJobParallelForExtensions.EarlyJobInit<SeaBodies.VibrateSeaBodiesJob>();
			IJobParallelForTransformExtensions.EarlyJobInit<StainVoxelManager.UpdateMatrixJob>();
		}
		catch (Exception ex)
		{
			EarlyInitHelpers.JobReflectionDataCreationFailed(ex);
		}
	}

	// Token: 0x06002990 RID: 10640 RVA: 0x00112AAC File Offset: 0x00110CAC
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__1221673671587648887.CreateJobReflectionData();
	}
}
