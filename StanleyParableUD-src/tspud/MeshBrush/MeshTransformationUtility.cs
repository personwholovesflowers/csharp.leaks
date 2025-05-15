using System;
using UnityEngine;

namespace MeshBrush
{
	// Token: 0x02000257 RID: 599
	public static class MeshTransformationUtility
	{
		// Token: 0x06000E44 RID: 3652 RVA: 0x00044228 File Offset: 0x00042428
		public static void ApplyRandomScale(Transform targetTransform, Vector2 range)
		{
			float num = Mathf.Abs(Random.Range(range.x, range.y));
			targetTransform.localScale = new Vector3(num, num, num);
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x0004425C File Offset: 0x0004245C
		public static void ApplyRandomScale(Transform targetTransform, Vector4 scaleRanges)
		{
			float num = Random.Range(scaleRanges.x, scaleRanges.y);
			float num2 = Random.Range(scaleRanges.z, scaleRanges.w);
			targetTransform.localScale = new Vector3
			{
				x = Mathf.Abs(num),
				y = Mathf.Abs(num2),
				z = Mathf.Abs(num)
			};
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x000442C4 File Offset: 0x000424C4
		public static void ApplyRandomScale(Transform targetTransform, Vector2 rangeX, Vector2 rangeY, Vector2 rangeZ)
		{
			targetTransform.localScale = new Vector3
			{
				x = Mathf.Abs(Random.Range(rangeX.x, rangeX.y)),
				y = Mathf.Abs(Random.Range(rangeY.x, rangeY.y)),
				z = Mathf.Abs(Random.Range(rangeZ.x, rangeZ.y))
			};
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x00044338 File Offset: 0x00042538
		public static void AddConstantScale(Transform targetTransform, Vector2 range)
		{
			float num = Random.Range(range.x, range.y);
			Vector3 vector = targetTransform.localScale + new Vector3(num, num, num);
			vector.x = Mathf.Abs(vector.x);
			vector.y = Mathf.Abs(vector.y);
			vector.z = Mathf.Abs(vector.z);
			targetTransform.localScale = vector;
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x000443A8 File Offset: 0x000425A8
		public static void AddConstantScale(Transform targetTransform, float x, float y, float z)
		{
			Vector3 vector = targetTransform.localScale + new Vector3
			{
				x = Mathf.Abs(x),
				y = Mathf.Abs(y),
				z = Mathf.Abs(z)
			};
			targetTransform.localScale = vector;
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x000443F8 File Offset: 0x000425F8
		public static void ApplyRandomRotation(Transform targetTransform, float randomRotationIntensityPercentageX, float randomRotationIntensityPercentageY, float randomRotationIntensityPercentageZ)
		{
			float num = Random.Range(0f, 3.6f * randomRotationIntensityPercentageX);
			float num2 = Random.Range(0f, 3.6f * randomRotationIntensityPercentageY);
			float num3 = Random.Range(0f, 3.6f * randomRotationIntensityPercentageZ);
			targetTransform.Rotate(new Vector3(num, num2, num3));
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x00044449 File Offset: 0x00042649
		public static void ApplyMeshOffset(Transform targetTransform, float offset, Vector3 direction)
		{
			targetTransform.Translate(direction.normalized * offset * 0.01f, Space.World);
		}
	}
}
