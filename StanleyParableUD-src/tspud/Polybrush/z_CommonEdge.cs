using System;
using System.Collections.Generic;

namespace Polybrush
{
	// Token: 0x02000223 RID: 547
	public struct z_CommonEdge : IEquatable<z_CommonEdge>
	{
		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000C40 RID: 3136 RVA: 0x00036A8C File Offset: 0x00034C8C
		public int x
		{
			get
			{
				return this.edge.x;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000C41 RID: 3137 RVA: 0x00036A99 File Offset: 0x00034C99
		public int y
		{
			get
			{
				return this.edge.y;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x00036AA6 File Offset: 0x00034CA6
		public int cx
		{
			get
			{
				return this.common.x;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000C43 RID: 3139 RVA: 0x00036AB3 File Offset: 0x00034CB3
		public int cy
		{
			get
			{
				return this.common.y;
			}
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x00036AC0 File Offset: 0x00034CC0
		public z_CommonEdge(int _x, int _y, int _cx, int _cy)
		{
			this.edge = new z_Edge(_x, _y);
			this.common = new z_Edge(_cx, _cy);
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x00036ADD File Offset: 0x00034CDD
		public bool Equals(z_CommonEdge b)
		{
			return this.common.Equals(b.common);
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x00036AF0 File Offset: 0x00034CF0
		public override bool Equals(object b)
		{
			return b is z_CommonEdge && this.common.Equals(((z_CommonEdge)b).common);
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x00036B12 File Offset: 0x00034D12
		public static bool operator ==(z_CommonEdge a, z_CommonEdge b)
		{
			return a.Equals(b);
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x00036B1C File Offset: 0x00034D1C
		public static bool operator !=(z_CommonEdge a, z_CommonEdge b)
		{
			return !a.Equals(b);
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x00036B29 File Offset: 0x00034D29
		public override int GetHashCode()
		{
			return this.common.GetHashCode();
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x00036B3C File Offset: 0x00034D3C
		public override string ToString()
		{
			return string.Format("{{ {{{0}:{1}}}, {{{2}:{3}}} }}", new object[]
			{
				this.edge.x,
				this.common.x,
				this.edge.y,
				this.common.y
			});
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x00036BA8 File Offset: 0x00034DA8
		public static List<int> ToList(IEnumerable<z_CommonEdge> edges)
		{
			List<int> list = new List<int>();
			foreach (z_CommonEdge z_CommonEdge in edges)
			{
				list.Add(z_CommonEdge.edge.x);
				list.Add(z_CommonEdge.edge.y);
			}
			return list;
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x00036C14 File Offset: 0x00034E14
		public static HashSet<int> ToHashSet(IEnumerable<z_CommonEdge> edges)
		{
			HashSet<int> hashSet = new HashSet<int>();
			foreach (z_CommonEdge z_CommonEdge in edges)
			{
				hashSet.Add(z_CommonEdge.edge.x);
				hashSet.Add(z_CommonEdge.edge.y);
			}
			return hashSet;
		}

		// Token: 0x04000BD1 RID: 3025
		public z_Edge edge;

		// Token: 0x04000BD2 RID: 3026
		public z_Edge common;
	}
}
