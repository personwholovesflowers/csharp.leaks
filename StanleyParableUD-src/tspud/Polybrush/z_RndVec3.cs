using System;
using UnityEngine;

namespace Polybrush
{
	// Token: 0x02000229 RID: 553
	public struct z_RndVec3 : IEquatable<z_RndVec3>
	{
		// Token: 0x06000C68 RID: 3176 RVA: 0x00037618 File Offset: 0x00035818
		public z_RndVec3(Vector3 vector)
		{
			this.x = vector.x;
			this.y = vector.y;
			this.z = vector.z;
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x00037640 File Offset: 0x00035840
		public bool Equals(z_RndVec3 p)
		{
			return Mathf.Abs(this.x - p.x) < 0.0001f && Mathf.Abs(this.y - p.y) < 0.0001f && Mathf.Abs(this.z - p.z) < 0.0001f;
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x0003769C File Offset: 0x0003589C
		public bool Equals(Vector3 p)
		{
			return Mathf.Abs(this.x - p.x) < 0.0001f && Mathf.Abs(this.y - p.y) < 0.0001f && Mathf.Abs(this.z - p.z) < 0.0001f;
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x000376F6 File Offset: 0x000358F6
		public override bool Equals(object b)
		{
			return (b is z_RndVec3 && this.Equals((z_RndVec3)b)) || (b is Vector3 && this.Equals((Vector3)b));
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x00037726 File Offset: 0x00035926
		public override int GetHashCode()
		{
			return ((27 * 29 + this.round(this.x)) * 29 + this.round(this.y)) * 29 + this.round(this.z);
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x0003775A File Offset: 0x0003595A
		public override string ToString()
		{
			return string.Format("{{{0:F2}, {1:F2}, {2:F2}}}", this.x, this.y, this.z);
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x00037787 File Offset: 0x00035987
		private int round(float v)
		{
			return (int)(v / 0.0001f);
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x00037791 File Offset: 0x00035991
		public static implicit operator Vector3(z_RndVec3 p)
		{
			return new Vector3(p.x, p.y, p.z);
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x000377AA File Offset: 0x000359AA
		public static implicit operator z_RndVec3(Vector3 p)
		{
			return new z_RndVec3(p);
		}

		// Token: 0x04000BE2 RID: 3042
		public float x;

		// Token: 0x04000BE3 RID: 3043
		public float y;

		// Token: 0x04000BE4 RID: 3044
		public float z;

		// Token: 0x04000BE5 RID: 3045
		private const float resolution = 0.0001f;
	}
}
