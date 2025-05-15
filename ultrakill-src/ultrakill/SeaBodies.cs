using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020003DC RID: 988
public class SeaBodies : MonoBehaviour
{
	// Token: 0x06001657 RID: 5719 RVA: 0x000B3E54 File Offset: 0x000B2054
	private void Start()
	{
		List<Transform> leafChildrenOfAllChunks = SeaBodies.GetLeafChildrenOfAllChunks(base.transform);
		this.seaBodyMaterial.SetFloat("_AtlasCount", (float)this.atlasCount);
		this.instanceCount = leafChildrenOfAllChunks.Count;
		this.originalPositions = new NativeArray<float3>(this.instanceCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.originalScales = new NativeArray<float3>(this.instanceCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.targetPositions = new NativeArray<float3>(this.instanceCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.speeds = new NativeArray<float>(this.instanceCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.randomStates = new NativeArray<Unity.Mathematics.Random>(this.instanceCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.currentPositions = new NativeArray<float3>(this.instanceCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.instanceMatricesNative = new NativeArray<Matrix4x4>(this.instanceCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.instanceAtlasOffset = new int[this.instanceCount];
		this.instanceColors = new Vector4[this.instanceCount];
		uint num = 12345U;
		for (int i = 0; i < this.instanceCount; i++)
		{
			SpriteRenderer component = leafChildrenOfAllChunks[i].GetComponent<SpriteRenderer>();
			this.instanceColors[i] = component.color;
			this.instanceAtlasOffset[i] = (component.sprite.name.Contains("1") ? 0 : 1);
			float3 @float = leafChildrenOfAllChunks[i].position;
			this.originalPositions[i] = @float;
			this.targetPositions[i] = @float;
			this.speeds[i] = global::UnityEngine.Random.Range(this.speedMin, this.speedMax);
			quaternion quaternion = leafChildrenOfAllChunks[i].rotation;
			this.currentPositions[i] = @float;
			float3 float2 = leafChildrenOfAllChunks[i].lossyScale * new float3(1f, 2f, 1f);
			this.originalScales[i] = float2;
			this.instanceMatricesNative[i] = Matrix4x4.TRS(@float, quaternion, float2);
			this.randomStates[i] = new Unity.Mathematics.Random((uint)((ulong)num + (ulong)((long)(i * 31))) | 1U);
			component.enabled = false;
		}
	}

	// Token: 0x06001658 RID: 5720 RVA: 0x000B408C File Offset: 0x000B228C
	private static List<Transform> FindAllChildrenContainingName(Transform parent, string substring)
	{
		List<Transform> list = new List<Transform>();
		foreach (object obj in parent)
		{
			Transform transform = (Transform)obj;
			if (transform.name.Contains(substring))
			{
				list.Add(transform);
			}
			list.AddRange(SeaBodies.FindAllChildrenContainingName(transform, substring));
		}
		return list;
	}

	// Token: 0x06001659 RID: 5721 RVA: 0x000B4104 File Offset: 0x000B2304
	public static List<Transform> GetAllLeafChildren(Transform parent)
	{
		List<Transform> list = new List<Transform>();
		foreach (object obj in parent)
		{
			Transform transform = (Transform)obj;
			if (transform.childCount == 0)
			{
				list.Add(transform);
			}
			else
			{
				list.AddRange(SeaBodies.GetAllLeafChildren(transform));
			}
		}
		return list;
	}

	// Token: 0x0600165A RID: 5722 RVA: 0x000B4178 File Offset: 0x000B2378
	public static List<Transform> GetLeafChildrenOfAllChunks(Transform root)
	{
		List<Transform> list = SeaBodies.FindAllChildrenContainingName(root, "Chunk");
		List<Transform> list2 = new List<Transform>();
		foreach (Transform transform in list)
		{
			list2.AddRange(SeaBodies.GetAllLeafChildren(transform));
		}
		return list2;
	}

	// Token: 0x0600165B RID: 5723 RVA: 0x000B41DC File Offset: 0x000B23DC
	private void Update()
	{
		CameraController instance = MonoSingleton<CameraController>.Instance;
		if (!instance)
		{
			return;
		}
		SeaBodies.VibrateSeaBodiesJob vibrateSeaBodiesJob = new SeaBodies.VibrateSeaBodiesJob
		{
			intensity = this.intensity,
			deltaTime = Time.deltaTime,
			cameraPosition = instance.cam.transform.position,
			originalPos = this.originalPositions,
			originalScale = this.originalScales,
			targetPos = this.targetPositions,
			speeds = this.speeds,
			currentPos = this.currentPositions,
			instanceMatrices = this.instanceMatricesNative,
			randomArray = this.randomStates
		};
		this.jobHandle = vibrateSeaBodiesJob.Schedule(this.instanceCount, 64, default(JobHandle));
	}

	// Token: 0x0600165C RID: 5724 RVA: 0x000B42B0 File Offset: 0x000B24B0
	private void LateUpdate()
	{
		this.jobHandle.Complete();
		if (this.mpb == null)
		{
			this.mpb = new MaterialPropertyBlock();
		}
		int num = 0;
		int num2;
		for (int i = this.instanceCount; i > 0; i -= num2)
		{
			num2 = Mathf.Min(1023, i);
			for (int j = 0; j < num2; j++)
			{
				int num3 = num + j;
				this.atlasOffsetBuffer[j] = (float)this.instanceAtlasOffset[num3];
				this.colorsBuffer[j] = this.instanceColors[num3];
				this.subMatrices[j] = this.instanceMatricesNative[num3];
			}
			this.mpb.Clear();
			this.mpb.SetFloatArray("_AtlasOffsetArray", this.atlasOffsetBuffer);
			this.mpb.SetVectorArray("_PerInstanceColor", this.colorsBuffer);
			Graphics.DrawMeshInstanced(this.seaBodyMesh, 0, this.seaBodyMaterial, this.subMatrices, num2, this.mpb, ShadowCastingMode.Off, false);
			num += num2;
		}
	}

	// Token: 0x0600165D RID: 5725 RVA: 0x000B43B4 File Offset: 0x000B25B4
	private void OnDestroy()
	{
		this.originalPositions.Dispose();
		this.originalScales.Dispose();
		this.targetPositions.Dispose();
		this.speeds.Dispose();
		this.currentPositions.Dispose();
		this.randomStates.Dispose();
		this.instanceMatricesNative.Dispose();
	}

	// Token: 0x04001EC3 RID: 7875
	public float intensity = 1f;

	// Token: 0x04001EC4 RID: 7876
	public float speedMin = 4f;

	// Token: 0x04001EC5 RID: 7877
	public float speedMax = 5f;

	// Token: 0x04001EC6 RID: 7878
	public Texture2D textureAtlas;

	// Token: 0x04001EC7 RID: 7879
	private NativeArray<float3> originalPositions;

	// Token: 0x04001EC8 RID: 7880
	private NativeArray<float3> originalScales;

	// Token: 0x04001EC9 RID: 7881
	private NativeArray<float3> targetPositions;

	// Token: 0x04001ECA RID: 7882
	private NativeArray<float> speeds;

	// Token: 0x04001ECB RID: 7883
	private NativeArray<float3> currentPositions;

	// Token: 0x04001ECC RID: 7884
	private NativeArray<Unity.Mathematics.Random> randomStates;

	// Token: 0x04001ECD RID: 7885
	private NativeArray<Matrix4x4> instanceMatricesNative;

	// Token: 0x04001ECE RID: 7886
	private JobHandle jobHandle;

	// Token: 0x04001ECF RID: 7887
	public Mesh seaBodyMesh;

	// Token: 0x04001ED0 RID: 7888
	public int atlasCount = 2;

	// Token: 0x04001ED1 RID: 7889
	public Material seaBodyMaterial;

	// Token: 0x04001ED2 RID: 7890
	private int[] instanceAtlasOffset;

	// Token: 0x04001ED3 RID: 7891
	private Vector4[] instanceColors;

	// Token: 0x04001ED4 RID: 7892
	private int instanceCount;

	// Token: 0x04001ED5 RID: 7893
	private MaterialPropertyBlock mpb;

	// Token: 0x04001ED6 RID: 7894
	private float[] atlasOffsetBuffer = new float[1023];

	// Token: 0x04001ED7 RID: 7895
	private Vector4[] colorsBuffer = new Vector4[1023];

	// Token: 0x04001ED8 RID: 7896
	private Matrix4x4[] subMatrices = new Matrix4x4[1023];

	// Token: 0x020003DD RID: 989
	[BurstCompile]
	public struct VibrateSeaBodiesJob : IJobParallelFor
	{
		// Token: 0x0600165F RID: 5727 RVA: 0x000B447C File Offset: 0x000B267C
		public void Execute(int i)
		{
			float3 @float = this.currentPos[i];
			float3 float2 = this.targetPos[i];
			float num = this.speeds[i];
			float3 float3 = this.originalPos[i];
			Unity.Mathematics.Random random = this.randomArray[i];
			math.distance(@float, float2);
			if (math.distance(@float, float2) < 0.001f)
			{
				@float = float2;
				float3 float4 = random.NextFloat3Direction() * this.intensity;
				float2 = float3 + float4;
				this.targetPos[i] = float2;
			}
			else
			{
				@float = Vector3.MoveTowards(@float, float2, num * this.deltaTime);
			}
			this.currentPos[i] = @float;
			float3 float5 = new float3(this.cameraPosition.x, @float.y, this.cameraPosition.z) - @float;
			quaternion quaternion = quaternion.identity;
			if (math.lengthsq(float5) > 0.0001f)
			{
				quaternion = quaternion.LookRotationSafe(float5, math.up());
			}
			this.randomArray[i] = random;
			this.instanceMatrices[i] = Matrix4x4.TRS(@float, quaternion, this.originalScale[i]);
		}

		// Token: 0x04001ED9 RID: 7897
		public float intensity;

		// Token: 0x04001EDA RID: 7898
		public float deltaTime;

		// Token: 0x04001EDB RID: 7899
		public float3 cameraPosition;

		// Token: 0x04001EDC RID: 7900
		public NativeArray<float3> originalPos;

		// Token: 0x04001EDD RID: 7901
		public NativeArray<float3> originalScale;

		// Token: 0x04001EDE RID: 7902
		public NativeArray<float3> targetPos;

		// Token: 0x04001EDF RID: 7903
		public NativeArray<float> speeds;

		// Token: 0x04001EE0 RID: 7904
		public NativeArray<float3> currentPos;

		// Token: 0x04001EE1 RID: 7905
		public NativeArray<Matrix4x4> instanceMatrices;

		// Token: 0x04001EE2 RID: 7906
		public NativeArray<Unity.Mathematics.Random> randomArray;
	}
}
