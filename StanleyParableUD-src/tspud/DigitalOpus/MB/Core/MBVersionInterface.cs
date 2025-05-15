using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000277 RID: 631
	public interface MBVersionInterface
	{
		// Token: 0x06000ED4 RID: 3796
		string version();

		// Token: 0x06000ED5 RID: 3797
		int GetMajorVersion();

		// Token: 0x06000ED6 RID: 3798
		int GetMinorVersion();

		// Token: 0x06000ED7 RID: 3799
		bool GetActive(GameObject go);

		// Token: 0x06000ED8 RID: 3800
		void SetActive(GameObject go, bool isActive);

		// Token: 0x06000ED9 RID: 3801
		void SetActiveRecursively(GameObject go, bool isActive);

		// Token: 0x06000EDA RID: 3802
		Object[] FindSceneObjectsOfType(Type t);

		// Token: 0x06000EDB RID: 3803
		bool IsRunningAndMeshNotReadWriteable(Mesh m);

		// Token: 0x06000EDC RID: 3804
		Vector2[] GetMeshUV3orUV4(Mesh m, bool get3, MB2_LogLevel LOG_LEVEL);

		// Token: 0x06000EDD RID: 3805
		void MeshClear(Mesh m, bool t);

		// Token: 0x06000EDE RID: 3806
		void MeshAssignUV3(Mesh m, Vector2[] uv3s);

		// Token: 0x06000EDF RID: 3807
		void MeshAssignUV4(Mesh m, Vector2[] uv4s);

		// Token: 0x06000EE0 RID: 3808
		Vector4 GetLightmapTilingOffset(Renderer r);

		// Token: 0x06000EE1 RID: 3809
		Transform[] GetBones(Renderer r);

		// Token: 0x06000EE2 RID: 3810
		void OptimizeMesh(Mesh m);

		// Token: 0x06000EE3 RID: 3811
		int GetBlendShapeFrameCount(Mesh m, int shapeIndex);

		// Token: 0x06000EE4 RID: 3812
		float GetBlendShapeFrameWeight(Mesh m, int shapeIndex, int frameIndex);

		// Token: 0x06000EE5 RID: 3813
		void GetBlendShapeFrameVertices(Mesh m, int shapeIndex, int frameIndex, Vector3[] vs, Vector3[] ns, Vector3[] ts);

		// Token: 0x06000EE6 RID: 3814
		void ClearBlendShapes(Mesh m);

		// Token: 0x06000EE7 RID: 3815
		void AddBlendShapeFrame(Mesh m, string nm, float wt, Vector3[] vs, Vector3[] ns, Vector3[] ts);
	}
}
