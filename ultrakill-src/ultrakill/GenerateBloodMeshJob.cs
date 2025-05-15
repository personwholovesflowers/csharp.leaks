using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000081 RID: 129
[BurstCompile]
public struct GenerateBloodMeshJob : IJobParallelFor
{
	// Token: 0x06000265 RID: 613 RVA: 0x0000D85C File Offset: 0x0000BA5C
	public void Execute(int index)
	{
		NativeArray<GenerateBloodMeshJob.VertexData> vertexData = this.meshData.GetVertexData<GenerateBloodMeshJob.VertexData>(0);
		float3 pos = this.props[index].pos;
		float3 norm = this.props[index].norm;
		float3 @float;
		float3 float2;
		math.orthonormal_basis(norm, out @float, out float2);
		float4x4 float4x = float4x4.TRS(pos, math.mul(quaternion.LookRotation(norm, @float), quaternion.RotateZ((float)(index % 359))), new float3(1.28f, 1.28f, 1f));
		float4 float3 = math.mul(float4x, new float4(-1f, 1f, 0f, 1f));
		float4 float4 = math.mul(float4x, new float4(1f, 1f, 0f, 1f));
		float4 float5 = math.mul(float4x, new float4(1f, -1f, 0f, 1f));
		float4 float6 = math.mul(float4x, new float4(-1f, -1f, 0f, 1f));
		int num = index * 4;
		int num2 = num + 1;
		int num3 = num + 2;
		int num4 = num + 3;
		float2 float7 = new float2((float)index, 0f);
		vertexData[num] = new GenerateBloodMeshJob.VertexData
		{
			position = float3,
			normal = norm,
			uv = new float2(0f, 0f),
			offset = float7,
			center = pos,
			tangent = @float
		};
		vertexData[num2] = new GenerateBloodMeshJob.VertexData
		{
			position = float4,
			normal = norm,
			uv = new float2(1f, 0f),
			offset = float7,
			center = pos,
			tangent = @float
		};
		vertexData[num3] = new GenerateBloodMeshJob.VertexData
		{
			position = float5,
			normal = norm,
			uv = new float2(1f, 1f),
			offset = float7,
			center = pos,
			tangent = @float
		};
		vertexData[num4] = new GenerateBloodMeshJob.VertexData
		{
			position = float6,
			normal = norm,
			uv = new float2(0f, 1f),
			offset = float7,
			center = pos,
			tangent = @float
		};
		NativeArray<uint> indexData = this.meshData.GetIndexData<uint>();
		int num5 = index * 6;
		int num6 = num5 + 1;
		int num7 = num5 + 2;
		int num8 = num5 + 3;
		int num9 = num5 + 4;
		int num10 = num5 + 5;
		indexData[num5] = Convert.ToUInt32(num);
		indexData[num6] = Convert.ToUInt32(num2);
		indexData[num7] = Convert.ToUInt32(num3);
		indexData[num8] = Convert.ToUInt32(num);
		indexData[num9] = Convert.ToUInt32(num3);
		indexData[num10] = Convert.ToUInt32(num4);
	}

	// Token: 0x040002CC RID: 716
	[ReadOnly]
	public NativeArray<BloodsplatterManager.InstanceProperties> props;

	// Token: 0x040002CD RID: 717
	public Mesh.MeshData meshData;

	// Token: 0x02000082 RID: 130
	public struct VertexData
	{
		// Token: 0x040002CE RID: 718
		public float4 position;

		// Token: 0x040002CF RID: 719
		public float3 normal;

		// Token: 0x040002D0 RID: 720
		public float2 uv;

		// Token: 0x040002D1 RID: 721
		public float2 offset;

		// Token: 0x040002D2 RID: 722
		public float3 center;

		// Token: 0x040002D3 RID: 723
		public float3 tangent;
	}
}
