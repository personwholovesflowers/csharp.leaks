using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

// Token: 0x0200047C RID: 1148
public class ThreadedParticleCollision : MonoBehaviour
{
	// Token: 0x1400000D RID: 13
	// (add) Token: 0x06001A45 RID: 6725 RVA: 0x000D88C8 File Offset: 0x000D6AC8
	// (remove) Token: 0x06001A46 RID: 6726 RVA: 0x000D8900 File Offset: 0x000D6B00
	public event Action<NativeSlice<RaycastHit>> collisionEvent;

	// Token: 0x06001A47 RID: 6727 RVA: 0x000D8938 File Offset: 0x000D6B38
	private void Awake()
	{
		LayerMask layerMask = LayerMaskDefaults.Get(LMD.Environment);
		if (StockMapInfo.Instance.continuousGibCollisions)
		{
			layerMask |= 16;
		}
		QueryTriggerInteraction queryTriggerInteraction = (StockMapInfo.Instance.continuousGibCollisions ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore);
		this.commandJob.parameters = new QueryParameters(layerMask, false, queryTriggerInteraction, false);
		this.particles.SetCustomParticleData(this.customData, ParticleSystemCustomData.Custom1);
		this.results = new NativeArray<RaycastHit>(this.particles.main.maxParticles, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.raycasts = new NativeArray<RaycastCommand>(this.particles.main.maxParticles, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.commandJob.raycasts = this.raycasts;
		this.commandJob.lastFrameHits = this.results;
	}

	// Token: 0x06001A48 RID: 6728 RVA: 0x000D8A05 File Offset: 0x000D6C05
	private void OnEnable()
	{
		this.bsm = MonoSingleton<BloodsplatterManager>.Instance;
		MonoSingleton<BloodsplatterManager>.Instance.ParticleCollisionStep += this.Step;
	}

	// Token: 0x06001A49 RID: 6729 RVA: 0x000D8A28 File Offset: 0x000D6C28
	private void OnDisable()
	{
		if (MonoSingleton<BloodsplatterManager>.Instance)
		{
			MonoSingleton<BloodsplatterManager>.Instance.ParticleCollisionStep -= this.Step;
		}
	}

	// Token: 0x06001A4A RID: 6730 RVA: 0x000D8A4C File Offset: 0x000D6C4C
	private unsafe void Step(float dt)
	{
		if (!this.handle.IsCompleted)
		{
			return;
		}
		this.handle.Complete();
		if (this.results.IsCreated)
		{
			int particleCount = this.particles.particleCount;
			RaycastHit* unsafeBufferPointerWithoutChecks = (RaycastHit*)NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<RaycastHit>(this.results);
			for (int i = 0; i < particleCount; i++)
			{
				RaycastHit raycastHit = unsafeBufferPointerWithoutChecks[i];
				if (raycastHit.colliderInstanceID != 0)
				{
					this.bloodsplatter.CreateBloodstain(ref raycastHit, this.bsm);
				}
			}
		}
		Transform transform = this.particles.transform;
		if (transform.hasChanged)
		{
			transform.hasChanged = false;
			if (this.particles.main.simulationSpace == ParticleSystemSimulationSpace.Local)
			{
				this.commandJob.transform = transform.localToWorldMatrix;
				this.commandJob.worldSpace = false;
			}
			else
			{
				this.commandJob.transform = ThreadedParticleCollision.identityMatrix;
				this.commandJob.worldSpace = true;
				this.commandJob.center = transform.position;
			}
		}
		JobHandle jobHandle = this.commandJob.Schedule(this.particles, 128, default(JobHandle));
		this.handle = RaycastCommand.ScheduleBatch(this.raycasts, this.results, 128, jobHandle);
	}

	// Token: 0x06001A4B RID: 6731 RVA: 0x000D8B9B File Offset: 0x000D6D9B
	private void OnDestroy()
	{
		this.raycasts.Dispose(this.handle);
		this.results.Dispose(this.handle);
	}

	// Token: 0x040024D0 RID: 9424
	public ParticleSystem particles;

	// Token: 0x040024D1 RID: 9425
	public Bloodsplatter bloodsplatter;

	// Token: 0x040024D2 RID: 9426
	public NativeArray<RaycastCommand> raycasts;

	// Token: 0x040024D3 RID: 9427
	public NativeArray<RaycastHit> results;

	// Token: 0x040024D4 RID: 9428
	private CommandJob commandJob;

	// Token: 0x040024D5 RID: 9429
	private JobHandle handle;

	// Token: 0x040024D6 RID: 9430
	private List<Vector4> customData = new List<Vector4>();

	// Token: 0x040024D7 RID: 9431
	private BloodsplatterManager bsm;

	// Token: 0x040024D8 RID: 9432
	private static Matrix4x4 identityMatrix = Matrix4x4.identity;
}
