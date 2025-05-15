using System;
using System.Collections.Generic;

namespace Ferr
{
	// Token: 0x020002ED RID: 749
	public class LambdaComparer<T> : IComparer<T>
	{
		// Token: 0x06001384 RID: 4996 RVA: 0x00067F59 File Offset: 0x00066159
		public LambdaComparer(Func<T, T, int> comparerFunc)
		{
			this.func = comparerFunc;
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x00067F68 File Offset: 0x00066168
		public int Compare(T x, T y)
		{
			return this.func(x, y);
		}

		// Token: 0x04000F40 RID: 3904
		private readonly Func<T, T, int> func;
	}
}
