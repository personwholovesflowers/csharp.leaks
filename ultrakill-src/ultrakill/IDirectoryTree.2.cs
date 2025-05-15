using System;
using System.Collections.Generic;

// Token: 0x02000147 RID: 327
public interface IDirectoryTree<T> : IDirectoryTree
{
	// Token: 0x17000088 RID: 136
	// (get) Token: 0x06000662 RID: 1634
	string name { get; }

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x06000663 RID: 1635
	// (set) Token: 0x06000664 RID: 1636
	IDirectoryTree<T> parent { get; set; }

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x06000665 RID: 1637
	IEnumerable<IDirectoryTree<T>> children { get; }

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x06000666 RID: 1638
	IEnumerable<T> files { get; }

	// Token: 0x06000667 RID: 1639
	IEnumerable<T> GetFilesRecursive();

	// Token: 0x06000668 RID: 1640
	void Refresh();
}
