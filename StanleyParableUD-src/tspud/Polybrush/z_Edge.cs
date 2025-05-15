using System;
using System.Collections.Generic;

namespace Polybrush
{
	// Token: 0x02000224 RID: 548
	public struct z_Edge : IEquatable<z_Edge>
	{
		// Token: 0x06000C4D RID: 3149 RVA: 0x00036C80 File Offset: 0x00034E80
		public z_Edge(int _x, int _y)
		{
			this.x = _x;
			this.y = _y;
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x00036C90 File Offset: 0x00034E90
		public bool Equals(z_Edge p)
		{
			return (p.x == this.x && p.y == this.y) || (p.x == this.y && p.y == this.x);
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x00036CCE File Offset: 0x00034ECE
		public override bool Equals(object b)
		{
			return b is z_Edge && this.Equals((z_Edge)b);
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x00036CE6 File Offset: 0x00034EE6
		public static bool operator ==(z_Edge a, z_Edge b)
		{
			return a.Equals(b);
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x00036CF0 File Offset: 0x00034EF0
		public static bool operator !=(z_Edge a, z_Edge b)
		{
			return !a.Equals(b);
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x00036D00 File Offset: 0x00034F00
		public override int GetHashCode()
		{
			int num = 17;
			int hashCode = ((this.x < this.y) ? this.x : this.y).GetHashCode();
			int hashCode2 = ((this.x < this.y) ? this.y : this.x).GetHashCode();
			return (num * 29 + hashCode.GetHashCode()) * 29 + hashCode2.GetHashCode();
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x00036D6F File Offset: 0x00034F6F
		public override string ToString()
		{
			return string.Format("{{{{{0},{1}}}}}", this.x, this.y);
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x00036D94 File Offset: 0x00034F94
		public static List<int> ToList(IEnumerable<z_Edge> edges)
		{
			List<int> list = new List<int>();
			foreach (z_Edge z_Edge in edges)
			{
				list.Add(z_Edge.x);
				list.Add(z_Edge.y);
			}
			return list;
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x00036DF4 File Offset: 0x00034FF4
		public static HashSet<int> ToHashSet(IEnumerable<z_Edge> edges)
		{
			HashSet<int> hashSet = new HashSet<int>();
			foreach (z_Edge z_Edge in edges)
			{
				hashSet.Add(z_Edge.x);
				hashSet.Add(z_Edge.y);
			}
			return hashSet;
		}

		// Token: 0x04000BD3 RID: 3027
		public int x;

		// Token: 0x04000BD4 RID: 3028
		public int y;
	}
}
