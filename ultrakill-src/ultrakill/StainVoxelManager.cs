using System;
using System.Collections.Generic;
using System.Linq;
using plog;
using SettingsMenu.Components.Pages;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Rendering;

// Token: 0x0200043F RID: 1087
public class StainVoxelManager : MonoSingleton<StainVoxelManager>
{
	// Token: 0x0600187A RID: 6266 RVA: 0x000C76AC File Offset: 0x000C58AC
	private void UpdateStainVisuals()
	{
		if (!this.jobHandle.IsCompleted)
		{
			return;
		}
		this.jobHandle.Complete();
		this.CleanupStaleTransforms();
		int num = this.currentStainCount;
		this.argsData[1] = (uint)num;
		this.argsBuffer.SetData(this.argsData);
		this.propBuffer.SetData<StainVoxelManager.InstanceProperties>(this.instanceProps, 0, 0, num);
		this.stainBuffer.SetData<float4x4>(this.gasolineTransforms, 0, 0, num);
		StainVoxelManager.UpdateMatrixJob updateMatrixJob = new StainVoxelManager.UpdateMatrixJob
		{
			OutMatrices = this.gasolineTransforms
		};
		this.jobHandle = updateMatrixJob.Schedule(this.stainTransforms, default(JobHandle));
	}

	// Token: 0x0600187B RID: 6267 RVA: 0x000C7754 File Offset: 0x000C5954
	private void Start()
	{
		this.usedComputeShadersAtStart = !SettingsMenu.Components.Pages.GraphicsSettings.disabledComputeShaders;
		if (this.usedComputeShadersAtStart)
		{
			this.stainTransforms = new TransformAccessArray(10000, -1);
			this.gasolineTransforms = new NativeArray<float4x4>(this.stainTransforms.capacity, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.instanceProps = new NativeArray<StainVoxelManager.InstanceProperties>(this.stainTransforms.capacity, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.stainBuffer = new ComputeBuffer(this.stainTransforms.capacity, 64, ComputeBufferType.Structured);
			this.propBuffer = new ComputeBuffer(this.stainTransforms.capacity, 8, ComputeBufferType.Structured);
			this.argsData[0] = this.gasStainMesh.GetIndexCount(0);
			this.argsData[1] = (uint)this.currentStainCount;
			this.argsData[2] = this.gasStainMesh.GetIndexStart(0);
			this.argsData[3] = this.gasStainMesh.GetBaseVertex(0);
			this.argsData[4] = 0U;
			this.argsBuffer = new ComputeBuffer(1, this.argsData.Length * 4, ComputeBufferType.DrawIndirect);
			this.argsBuffer.SetData(this.argsData);
			this.gasolineTransforms[0] = float4x4.identity;
		}
	}

	// Token: 0x0600187C RID: 6268 RVA: 0x000C7880 File Offset: 0x000C5A80
	public void AddGasolineStain(Transform stain, bool shouldClipToSurface)
	{
		int num = this.currentStainCount;
		this.stainTransforms.Add(stain);
		this.instanceProps[num] = new StainVoxelManager.InstanceProperties
		{
			index = this.currentStainCount,
			clipToSurface = (shouldClipToSurface ? 1 : 0)
		};
		this.stainIndices.Add(stain, num);
		this.currentStainCount = (this.currentStainCount + 1) % this.instanceProps.Length;
	}

	// Token: 0x0600187D RID: 6269 RVA: 0x000C78F8 File Offset: 0x000C5AF8
	private void CleanupStaleTransforms()
	{
		for (int i = this.currentStainCount - 1; i >= 0; i--)
		{
			if (this.stainTransforms[i] == null)
			{
				this.RemoveStainAtIndex(i);
			}
		}
	}

	// Token: 0x0600187E RID: 6270 RVA: 0x000C7934 File Offset: 0x000C5B34
	private void RemoveStainAtIndex(int removeIndex)
	{
		int num = this.currentStainCount - 1;
		if (removeIndex != num)
		{
			this.instanceProps[removeIndex] = this.instanceProps[num];
			this.gasolineTransforms[removeIndex] = this.gasolineTransforms[num];
			this.gasolineTransforms[num] = float4x4.zero;
			Transform transform = this.stainTransforms[num];
			this.stainIndices[transform] = removeIndex;
		}
		this.stainTransforms.RemoveAtSwapBack(removeIndex);
		this.currentStainCount--;
	}

	// Token: 0x0600187F RID: 6271 RVA: 0x000C79C3 File Offset: 0x000C5BC3
	public void RemoveAllStains()
	{
		this.removeLate = true;
	}

	// Token: 0x06001880 RID: 6272 RVA: 0x000C79CC File Offset: 0x000C5BCC
	private void LateUpdate()
	{
		if (!this.usedComputeShadersAtStart)
		{
			return;
		}
		if (!this.removeLate)
		{
			return;
		}
		this.jobHandle.Complete();
		for (int i = 0; i < this.stainTransforms.capacity; i++)
		{
			this.gasolineTransforms[i] = float4x4.zero;
		}
		this.stainBuffer.SetData<float4x4>(this.gasolineTransforms, 0, 0, this.currentStainCount);
		this.removeLate = false;
	}

	// Token: 0x06001881 RID: 6273 RVA: 0x000C7A40 File Offset: 0x000C5C40
	public void SetupStainCommandBuffer(Camera mainCam, RenderTexture mainTex, RenderTexture stainCopy, RenderTexture depth, RenderTexture viewNormal)
	{
		if (this.cb == null)
		{
			this.cb = new CommandBuffer();
			this.cb.name = "Gasoline Stain";
		}
		if (this.gasolineCompositeMaterial == null)
		{
			this.gasolineCompositeMaterial = new Material(this.gasolineCompositeShader);
		}
		this.gasStainMat.SetTexture("_DepthBuffer", depth);
		this.gasStainMat.SetBuffer("stainMatrices", this.stainBuffer);
		this.gasStainMat.SetBuffer("instanceBuffer", this.propBuffer);
		this.gasStainMat.SetTexture("_ViewNormal", viewNormal);
		this.cb.Clear();
		this.cb.SetRenderTarget(stainCopy.colorBuffer);
		this.cb.ClearRenderTarget(false, true, new Color(1f, 1f, 1f, 0f));
		this.cb.DrawMeshInstancedIndirect(this.gasStainMesh, 0, this.gasStainMat, 0, this.argsBuffer, 0, null);
		this.cb.Blit(stainCopy.colorBuffer, mainTex.colorBuffer, this.gasolineCompositeMaterial);
		mainCam.AddCommandBuffer(CameraEvent.BeforeForwardAlpha, this.cb);
	}

	// Token: 0x06001882 RID: 6274 RVA: 0x000C7B7C File Offset: 0x000C5D7C
	public void AcknowledgeNewStain(StainVoxel voxel)
	{
		voxel.AcknowledgeNewStain();
		if (voxel.isBurning)
		{
			return;
		}
		foreach (Vector3Int vector3Int in this.GetShapeIterator(voxel.VoxelPosition, VoxelCheckingShape.Box, 3).ToArray<Vector3Int>())
		{
			StainVoxel stainVoxel;
			if (this.stainVoxels.TryGetValue(vector3Int, out stainVoxel) && stainVoxel.isBurning)
			{
				voxel.TryIgnite();
			}
		}
	}

	// Token: 0x06001883 RID: 6275 RVA: 0x000C7BE4 File Offset: 0x000C5DE4
	private void Update()
	{
		if (this.usedComputeShadersAtStart)
		{
			this.UpdateStainVisuals();
		}
		if (this.lastPropagationTick != null)
		{
			TimeSince? timeSince = this.lastPropagationTick;
			float? num = ((timeSince != null) ? new float?(timeSince.GetValueOrDefault()) : null);
			float num2 = 0.05f;
			if (!((num.GetValueOrDefault() < num2) & (num != null)) && this.pendingIgnitions != null && this.pendingIgnitions.Count != 0)
			{
				this.lastPropagationTick = new TimeSince?(-global::UnityEngine.Random.Range(0f, 0.005f));
				List<StainVoxel> list = new List<StainVoxel>();
				foreach (Vector3Int vector3Int in this.pendingIgnitions)
				{
					StainVoxel stainVoxel = this.CreateOrGetVoxel(StainVoxelManager.VoxelToWorldPosition(vector3Int), true);
					if (stainVoxel != null && !stainVoxel.isBurning)
					{
						list.Add(stainVoxel);
					}
				}
				this.pendingIgnitions.Clear();
				bool flag = false;
				foreach (StainVoxel stainVoxel2 in list)
				{
					if (stainVoxel2.TryIgnite())
					{
						flag = true;
						this.ScheduleFirePropagation(stainVoxel2);
					}
				}
				if (flag)
				{
					return;
				}
				this.explodedVoxels.Clear();
				this.lastPropagationTick = null;
				return;
			}
		}
	}

	// Token: 0x06001884 RID: 6276 RVA: 0x000C7D6C File Offset: 0x000C5F6C
	private new void OnDestroy()
	{
		if (this.usedComputeShadersAtStart)
		{
			this.jobHandle.Complete();
			this.stainTransforms.Dispose();
			this.gasolineTransforms.Dispose();
			this.instanceProps.Dispose();
			this.stainBuffer.Release();
			this.propBuffer.Release();
			this.argsBuffer.Release();
		}
	}

	// Token: 0x06001885 RID: 6277 RVA: 0x000C7DD0 File Offset: 0x000C5FD0
	public StainVoxel CreateOrGetVoxel(Vector3 worldPosition, bool dontCreate = false)
	{
		Vector3Int vector3Int = StainVoxelManager.WorldToVoxelPosition(worldPosition);
		StainVoxel stainVoxel;
		if (this.stainVoxels.TryGetValue(vector3Int, out stainVoxel))
		{
			return stainVoxel;
		}
		if (dontCreate)
		{
			return null;
		}
		stainVoxel = new StainVoxel(vector3Int);
		this.stainVoxels.Add(vector3Int, stainVoxel);
		return stainVoxel;
	}

	// Token: 0x06001886 RID: 6278 RVA: 0x000C7E10 File Offset: 0x000C6010
	public void RefreshVoxel(StainVoxel voxel)
	{
		if (!voxel.isEmpty)
		{
			return;
		}
		this.stainVoxels.Remove(voxel.VoxelPosition);
		voxel.DestroySelf();
	}

	// Token: 0x06001887 RID: 6279 RVA: 0x000C7E34 File Offset: 0x000C6034
	public void UpdateProxyPosition(VoxelProxy proxy, Vector3Int newPosition)
	{
		Vector3Int voxelPosition = proxy.voxel.VoxelPosition;
		if (voxelPosition == newPosition)
		{
			return;
		}
		StainVoxel stainVoxel;
		if (!this.stainVoxels.TryGetValue(voxelPosition, out stainVoxel))
		{
			StainVoxelManager.Log.Warning(string.Format("Failed to find voxel at {0}", voxelPosition), null, null, null);
			return;
		}
		stainVoxel.RemoveProxy(proxy, false);
		this.CreateOrGetVoxel(StainVoxelManager.VoxelToWorldPosition(newPosition), false).AddProxy(proxy);
	}

	// Token: 0x06001888 RID: 6280 RVA: 0x000C7EA0 File Offset: 0x000C60A0
	public bool ShouldExplodeAt(Vector3Int voxelPosition)
	{
		if (this.explodedVoxels.Count == 0)
		{
			this.explodedVoxels.Add(voxelPosition);
			return true;
		}
		foreach (Vector3Int vector3Int in this.explodedVoxels)
		{
			if (Mathf.Abs(voxelPosition.x - vector3Int.x) <= 2 && Mathf.Abs(voxelPosition.y - vector3Int.y) <= 2 && Mathf.Abs(voxelPosition.z - vector3Int.z) <= 2)
			{
				return false;
			}
		}
		this.explodedVoxels.Add(voxelPosition);
		return true;
	}

	// Token: 0x06001889 RID: 6281 RVA: 0x000C7F64 File Offset: 0x000C6164
	public bool TryIgniteAt(Vector3 worldPosition, int checkSize = 3)
	{
		if (this.stainVoxels.Count == 0)
		{
			return false;
		}
		Vector3Int vector3Int = StainVoxelManager.WorldToVoxelPosition(worldPosition);
		return this.TryIgniteAt(vector3Int, checkSize);
	}

	// Token: 0x0600188A RID: 6282 RVA: 0x000C7F90 File Offset: 0x000C6190
	public bool TryIgniteAt(Vector3Int voxelPosition, int checkSize = 3)
	{
		StainVoxelManager.Log.Info(string.Format("TryIgniteAt {0}", voxelPosition), null, null, null);
		if (this.stainVoxels.Count == 0)
		{
			return false;
		}
		List<StainVoxel> list;
		if (!this.TryGetVoxels(voxelPosition, out list, checkSize, VoxelCheckingShape.Box, false))
		{
			return false;
		}
		bool flag = false;
		using (List<StainVoxel>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.TryIgnite())
				{
					flag = true;
				}
			}
		}
		return flag;
	}

	// Token: 0x0600188B RID: 6283 RVA: 0x000C8020 File Offset: 0x000C6220
	public void ScheduleFirePropagation(StainVoxel voxel)
	{
		if (voxel == null)
		{
			StainVoxelManager.Log.Warning("ScheduleFirePropagation called with null voxel", null, null, null);
			return;
		}
		if (this.lastPropagationTick == null)
		{
			this.lastPropagationTick = new TimeSince?(0f);
		}
		Vector3Int voxelPosition = voxel.VoxelPosition;
		foreach (Vector3Int vector3Int in StainVoxelManager.IterateBox(voxelPosition, 5).ToArray<Vector3Int>())
		{
			StainVoxel stainVoxel;
			if (!(vector3Int == voxelPosition) && !this.pendingIgnitions.Contains(vector3Int) && this.stainVoxels.TryGetValue(vector3Int, out stainVoxel) && !stainVoxel.isBurning)
			{
				this.pendingIgnitions.Add(vector3Int);
			}
		}
		if (this.explodedVoxels.Count != 0 && this.pendingIgnitions.Count == 0)
		{
			this.explodedVoxels.Clear();
		}
	}

	// Token: 0x0600188C RID: 6284 RVA: 0x000C80F1 File Offset: 0x000C62F1
	public void DoneBurning(VoxelProxy proxy)
	{
		if (proxy == null)
		{
			return;
		}
		StainVoxel voxel = proxy.voxel;
		if (voxel == null)
		{
			return;
		}
		voxel.RemoveProxy(proxy, true);
	}

	// Token: 0x0600188D RID: 6285 RVA: 0x000C8110 File Offset: 0x000C6310
	public bool TryGetVoxelsWorld(Vector3 worldPosition, out List<StainVoxel> voxels, int checkSize = 3, VoxelCheckingShape shape = VoxelCheckingShape.Box, bool returnOnHit = false)
	{
		Vector3Int vector3Int = StainVoxelManager.WorldToVoxelPosition(worldPosition);
		return this.TryGetVoxels(vector3Int, out voxels, checkSize, shape, returnOnHit);
	}

	// Token: 0x0600188E RID: 6286 RVA: 0x000C8134 File Offset: 0x000C6334
	public bool TryGetVoxels(Vector3Int voxelPosition, out List<StainVoxel> voxels, int checkSize = 3, VoxelCheckingShape shape = VoxelCheckingShape.Box, bool returnOnHit = false)
	{
		voxels = new List<StainVoxel>();
		if (checkSize > 1)
		{
			foreach (Vector3Int vector3Int in this.GetShapeIterator(voxelPosition, shape, checkSize))
			{
				StainVoxel stainVoxel;
				if (this.stainVoxels.TryGetValue(vector3Int, out stainVoxel))
				{
					voxels.Add(stainVoxel);
					StainVoxelManager.DrawVoxel(vector3Int, true);
					if (returnOnHit)
					{
						return true;
					}
				}
				else
				{
					StainVoxelManager.DrawVoxel(vector3Int, false);
				}
			}
			return voxels.Count > 0;
		}
		StainVoxel stainVoxel2;
		if (this.stainVoxels.TryGetValue(voxelPosition, out stainVoxel2))
		{
			voxels.Add(stainVoxel2);
			StainVoxelManager.DrawVoxel(voxelPosition, true);
			return true;
		}
		StainVoxelManager.DrawVoxel(voxelPosition, false);
		return false;
	}

	// Token: 0x0600188F RID: 6287 RVA: 0x000C81F0 File Offset: 0x000C63F0
	public bool HasProxiesAt(Vector3Int voxelPosition, int checkSize = 3, VoxelCheckingShape shape = VoxelCheckingShape.Box, ProxySearchMode searchMode = ProxySearchMode.Any, bool returnOnHit = true)
	{
		if (this.stainVoxels.Count == 0)
		{
			return false;
		}
		if (checkSize <= 1)
		{
			return this.ProxyExistsAt(voxelPosition, searchMode);
		}
		foreach (Vector3Int vector3Int in this.GetShapeIterator(voxelPosition, shape, checkSize))
		{
			if (this.ProxyExistsAt(vector3Int, searchMode))
			{
				StainVoxelManager.DrawVoxel(vector3Int, true);
				if (returnOnHit)
				{
					return true;
				}
			}
			else
			{
				StainVoxelManager.DrawVoxel(vector3Int, false);
			}
		}
		return false;
	}

	// Token: 0x06001890 RID: 6288 RVA: 0x000C827C File Offset: 0x000C647C
	private IEnumerable<Vector3Int> GetShapeIterator(Vector3Int center, VoxelCheckingShape shape, int size)
	{
		switch (shape)
		{
		case VoxelCheckingShape.Box:
			return StainVoxelManager.IterateBox(center, size);
		case VoxelCheckingShape.VerticalBox:
			return StainVoxelManager.IterateVerticalBox(center, size, 2);
		case VoxelCheckingShape.Pole:
			return StainVoxelManager.IteratePole(center, size);
		case VoxelCheckingShape.Cross:
			return StainVoxelManager.IterateCross(center, size, 2);
		default:
			return null;
		}
	}

	// Token: 0x06001891 RID: 6289 RVA: 0x000C82BC File Offset: 0x000C64BC
	private bool ProxyExistsAt(Vector3Int voxelPosition, ProxySearchMode searchMode = ProxySearchMode.Any)
	{
		StainVoxel stainVoxel;
		return this.stainVoxels.Count != 0 && this.stainVoxels.ContainsKey(voxelPosition) && this.stainVoxels.TryGetValue(voxelPosition, out stainVoxel) && stainVoxel.HasStains(searchMode);
	}

	// Token: 0x06001892 RID: 6290 RVA: 0x000C8301 File Offset: 0x000C6501
	private static IEnumerable<Vector3Int> IterateBox(Vector3Int center, int size)
	{
		int halfSize = size / 2;
		int num;
		for (int x = -halfSize; x <= halfSize; x = num + 1)
		{
			for (int y = -halfSize; y <= halfSize; y = num + 1)
			{
				for (int z = -halfSize; z <= halfSize; z = num + 1)
				{
					yield return new Vector3Int(center.x + x, center.y + y, center.z + z);
					num = z;
				}
				num = y;
			}
			num = x;
		}
		yield break;
	}

	// Token: 0x06001893 RID: 6291 RVA: 0x000C8318 File Offset: 0x000C6518
	private static IEnumerable<Vector3Int> IterateVerticalBox(Vector3Int center, int size, int height)
	{
		int halfSize = size / 2;
		int num;
		for (int x = -halfSize; x <= halfSize; x = num + 1)
		{
			for (int z = -halfSize; z <= halfSize; z = num + 1)
			{
				for (int y = 0; y < height; y = num + 1)
				{
					yield return new Vector3Int(center.x + x, center.y + y, center.z + z);
					num = y;
				}
				num = z;
			}
			num = x;
		}
		yield break;
	}

	// Token: 0x06001894 RID: 6292 RVA: 0x000C8336 File Offset: 0x000C6536
	private static IEnumerable<Vector3Int> IterateCross(Vector3Int center, int size, int height)
	{
		int halfSize = size / 2;
		int num;
		for (int y = 0; y < height; y = num + 1)
		{
			for (int x = -halfSize; x <= halfSize; x = num + 1)
			{
				yield return new Vector3Int(center.x + x, center.y + y, center.z);
				num = x;
			}
			for (int x = -halfSize; x <= halfSize; x = num + 1)
			{
				if (x != 0)
				{
					yield return new Vector3Int(center.x, center.y + y, center.z + x);
				}
				num = x;
			}
			num = y;
		}
		yield break;
	}

	// Token: 0x06001895 RID: 6293 RVA: 0x000C8354 File Offset: 0x000C6554
	private static IEnumerable<Vector3Int> IteratePole(Vector3Int center, int size)
	{
		int halfSize = size / 2;
		int num;
		for (int i = -halfSize; i <= halfSize; i = num + 1)
		{
			yield return new Vector3Int(center.x, center.y + i, center.z);
			num = i;
		}
		yield break;
	}

	// Token: 0x06001896 RID: 6294 RVA: 0x000C836C File Offset: 0x000C656C
	public static Vector3Int WorldToVoxelPosition(Vector3 position)
	{
		int num = Mathf.RoundToInt(position.x / 2.75f);
		int num2 = Mathf.RoundToInt(position.y / 2.75f);
		int num3 = Mathf.RoundToInt(position.z / 2.75f);
		return new Vector3Int(num, num2, num3);
	}

	// Token: 0x06001897 RID: 6295 RVA: 0x000C83B5 File Offset: 0x000C65B5
	public static Vector3 VoxelToWorldPosition(Vector3Int position)
	{
		return new Vector3((float)position.x * 2.75f, (float)position.y * 2.75f, (float)position.z * 2.75f);
	}

	// Token: 0x06001898 RID: 6296 RVA: 0x00004AE3 File Offset: 0x00002CE3
	private static void DrawVoxel(Vector3Int voxelPosition, bool success)
	{
	}

	// Token: 0x06001899 RID: 6297 RVA: 0x00004AE3 File Offset: 0x00002CE3
	private static void DrawVoxel(Vector3 roundedWorldPosition, bool success)
	{
	}

	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x0600189A RID: 6298 RVA: 0x000C83E6 File Offset: 0x000C65E6
	private StainVoxel[] debugVoxels
	{
		get
		{
			return this.stainVoxels.Values.ToArray<StainVoxel>();
		}
	}

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x0600189B RID: 6299 RVA: 0x000C83F8 File Offset: 0x000C65F8
	private Vector3Int[] debugPendingIgnitions
	{
		get
		{
			return this.pendingIgnitions.ToArray<Vector3Int>();
		}
	}

	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x0600189C RID: 6300 RVA: 0x000C8405 File Offset: 0x000C6605
	private Vector3Int[] debugExplodedVoxels
	{
		get
		{
			return this.explodedVoxels.ToArray<Vector3Int>();
		}
	}

	// Token: 0x0600189D RID: 6301 RVA: 0x000C8414 File Offset: 0x000C6614
	public void IgniteAll()
	{
		foreach (StainVoxel stainVoxel in this.stainVoxels.Values)
		{
			stainVoxel.TryIgnite();
		}
	}

	// Token: 0x0600189E RID: 6302 RVA: 0x000C846C File Offset: 0x000C666C
	public void ClearAll()
	{
		foreach (StainVoxel stainVoxel in this.stainVoxels.Values)
		{
			stainVoxel.DestroySelf();
		}
		this.stainVoxels.Clear();
		this.pendingIgnitions.Clear();
		this.explodedVoxels.Clear();
	}

	// Token: 0x0400224F RID: 8783
	private static readonly global::plog.Logger Log = new global::plog.Logger("StainVoxelManager");

	// Token: 0x04002250 RID: 8784
	private const int FireSpreadDistance = 5;

	// Token: 0x04002251 RID: 8785
	public const float VoxelSize = 2.75f;

	// Token: 0x04002252 RID: 8786
	public const float VoxelOverlapSphereRadius = 1.375f;

	// Token: 0x04002253 RID: 8787
	public const int ExplosionMargin = 2;

	// Token: 0x04002254 RID: 8788
	private readonly Dictionary<Vector3Int, StainVoxel> stainVoxels = new Dictionary<Vector3Int, StainVoxel>();

	// Token: 0x04002255 RID: 8789
	private readonly HashSet<Vector3Int> pendingIgnitions = new HashSet<Vector3Int>();

	// Token: 0x04002256 RID: 8790
	private TimeSince? lastPropagationTick;

	// Token: 0x04002257 RID: 8791
	private readonly HashSet<Vector3Int> explodedVoxels = new HashSet<Vector3Int>();

	// Token: 0x04002258 RID: 8792
	public readonly HurtCooldownCollection SharedHurtCooldownCollection = new HurtCooldownCollection();

	// Token: 0x04002259 RID: 8793
	public Shader gasolineCompositeShader;

	// Token: 0x0400225A RID: 8794
	private Material gasolineCompositeMaterial;

	// Token: 0x0400225B RID: 8795
	public Mesh gasStainMesh;

	// Token: 0x0400225C RID: 8796
	public Material gasStainMat;

	// Token: 0x0400225D RID: 8797
	public NativeArray<float4x4> gasolineTransforms;

	// Token: 0x0400225E RID: 8798
	private NativeArray<StainVoxelManager.InstanceProperties> instanceProps;

	// Token: 0x0400225F RID: 8799
	private ComputeBuffer propBuffer;

	// Token: 0x04002260 RID: 8800
	private ComputeBuffer stainBuffer;

	// Token: 0x04002261 RID: 8801
	private CommandBuffer cb;

	// Token: 0x04002262 RID: 8802
	private Dictionary<Transform, int> stainIndices = new Dictionary<Transform, int>();

	// Token: 0x04002263 RID: 8803
	private int currentStainCount;

	// Token: 0x04002264 RID: 8804
	public TransformAccessArray stainTransforms;

	// Token: 0x04002265 RID: 8805
	private JobHandle jobHandle;

	// Token: 0x04002266 RID: 8806
	private bool removeLate;

	// Token: 0x04002267 RID: 8807
	private ComputeBuffer argsBuffer;

	// Token: 0x04002268 RID: 8808
	private uint[] argsData = new uint[5];

	// Token: 0x04002269 RID: 8809
	public bool usedComputeShadersAtStart = true;

	// Token: 0x02000440 RID: 1088
	public struct InstanceProperties
	{
		// Token: 0x0400226A RID: 8810
		public int index;

		// Token: 0x0400226B RID: 8811
		public int clipToSurface;

		// Token: 0x0400226C RID: 8812
		public const int SIZE = 8;
	}

	// Token: 0x02000441 RID: 1089
	[BurstCompile]
	private struct UpdateMatrixJob : IJobParallelForTransform
	{
		// Token: 0x060018A1 RID: 6305 RVA: 0x000C8552 File Offset: 0x000C6752
		public void Execute(int index, TransformAccess transform)
		{
			if (!transform.isValid)
			{
				this.OutMatrices[index] = float4x4.zero;
				return;
			}
			this.OutMatrices[index] = transform.localToWorldMatrix;
		}

		// Token: 0x0400226D RID: 8813
		public NativeArray<float4x4> OutMatrices;
	}
}
