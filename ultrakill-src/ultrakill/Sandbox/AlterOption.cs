using System;

namespace Sandbox
{
	// Token: 0x0200055E RID: 1374
	[Serializable]
	public class AlterOption<T>
	{
		// Token: 0x04002B4B RID: 11083
		public string name;

		// Token: 0x04002B4C RID: 11084
		public string key;

		// Token: 0x04002B4D RID: 11085
		public string tooltip;

		// Token: 0x04002B4E RID: 11086
		public T value;

		// Token: 0x04002B4F RID: 11087
		public Type type;

		// Token: 0x04002B50 RID: 11088
		public Action<T> callback;

		// Token: 0x04002B51 RID: 11089
		public IConstraints constraints;
	}
}
