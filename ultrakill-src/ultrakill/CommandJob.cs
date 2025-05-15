using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

// Token: 0x0200047B RID: 1147
[BurstCompile]
internal struct CommandJob : IJobParticleSystemParallelFor
{
	// Token: 0x06001A44 RID: 6724 RVA: 0x000D8758 File Offset: 0x000D6958
	public void Execute(ParticleSystemJobData jobData, int i)
	{
		ParticleSystemNativeArray4 customData = jobData.customData1;
		Vector4 vector = customData[i];
		if (this.worldSpace && vector == Vector4.zero)
		{
			vector = this.center;
		}
		int num = (int)vector.w;
		Vector3 point = this.lastFrameHits[num].point;
		if (point.x != 0f && point.y != 0f && point.z != 0f)
		{
			jobData.aliveTimePercent[i] = 100f;
		}
		float3 xyz = math.mul(this.transform, new float4(vector.x, vector.y, vector.z, 1f)).xyz;
		float3 @float = math.mul(this.transform, new float4(jobData.positions[i], 1f)).xyz - xyz;
		float num2 = math.length(@float);
		this.raycasts[i] = new RaycastCommand(xyz, @float / num2, this.parameters, num2);
		customData[i] = new float4(jobData.positions[i], (float)i);
	}

	// Token: 0x040024C8 RID: 9416
	public float4x4 transform;

	// Token: 0x040024C9 RID: 9417
	public NativeArray<RaycastCommand> raycasts;

	// Token: 0x040024CA RID: 9418
	[ReadOnly]
	public NativeArray<RaycastHit> lastFrameHits;

	// Token: 0x040024CB RID: 9419
	public QueryParameters parameters;

	// Token: 0x040024CC RID: 9420
	public float deltaTime;

	// Token: 0x040024CD RID: 9421
	public bool worldSpace;

	// Token: 0x040024CE RID: 9422
	public Vector3 center;
}
