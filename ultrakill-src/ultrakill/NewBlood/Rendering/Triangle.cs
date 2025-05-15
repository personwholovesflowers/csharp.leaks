using System;
using System.Runtime.CompilerServices;

namespace NewBlood.Rendering
{
	// Token: 0x020005E7 RID: 1511
	public struct Triangle<[IsUnmanaged] TIndex> where TIndex : struct, ValueType
	{
		// Token: 0x170002A2 RID: 674
		public TIndex this[int index]
		{
			get
			{
				if (index > 2)
				{
					Triangle<TIndex>.ThrowIndexOutOfRangeException();
				}
				if (index == 1)
				{
					return this.Index1;
				}
				if (index != 2)
				{
					return this.Index0;
				}
				return this.Index2;
			}
			set
			{
				if (index > 2)
				{
					Triangle<TIndex>.ThrowIndexOutOfRangeException();
				}
				if (index == 1)
				{
					this.Index1 = value;
					return;
				}
				if (index != 2)
				{
					this.Index0 = value;
					return;
				}
				this.Index2 = value;
			}
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x0010B1C7 File Offset: 0x001093C7
		public Triangle(TIndex index0, TIndex index1, TIndex index2)
		{
			this.Index0 = index0;
			this.Index1 = index1;
			this.Index2 = index2;
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x0010B1DE File Offset: 0x001093DE
		private static TIndex ThrowIndexOutOfRangeException()
		{
			throw new IndexOutOfRangeException();
		}

		// Token: 0x04002DB0 RID: 11696
		public TIndex Index0;

		// Token: 0x04002DB1 RID: 11697
		public TIndex Index1;

		// Token: 0x04002DB2 RID: 11698
		public TIndex Index2;
	}
}
