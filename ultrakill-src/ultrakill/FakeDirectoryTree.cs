using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x0200014A RID: 330
public class FakeDirectoryTree<T> : IDirectoryTree<T>, IDirectoryTree
{
	// Token: 0x17000091 RID: 145
	// (get) Token: 0x0600067D RID: 1661 RVA: 0x0002C484 File Offset: 0x0002A684
	// (set) Token: 0x0600067E RID: 1662 RVA: 0x0002C48C File Offset: 0x0002A68C
	public string name { get; private set; }

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x0600067F RID: 1663 RVA: 0x0002C495 File Offset: 0x0002A695
	// (set) Token: 0x06000680 RID: 1664 RVA: 0x0002C49D File Offset: 0x0002A69D
	public IEnumerable<IDirectoryTree<T>> children { get; private set; }

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x06000681 RID: 1665 RVA: 0x0002C4A6 File Offset: 0x0002A6A6
	// (set) Token: 0x06000682 RID: 1666 RVA: 0x0002C4AE File Offset: 0x0002A6AE
	public IEnumerable<T> files { get; private set; }

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x06000683 RID: 1667 RVA: 0x0002C4B7 File Offset: 0x0002A6B7
	// (set) Token: 0x06000684 RID: 1668 RVA: 0x0002C4BF File Offset: 0x0002A6BF
	public IDirectoryTree<T> parent { get; set; }

	// Token: 0x06000685 RID: 1669 RVA: 0x0002C4C8 File Offset: 0x0002A6C8
	public FakeDirectoryTree(string name, IEnumerable<T> files = null, IEnumerable<IDirectoryTree<T>> children = null, IDirectoryTree<T> parent = null)
	{
		this.name = name;
		this.children = children ?? new List<IDirectoryTree<T>>();
		this.files = files ?? new List<T>();
		this.parent = parent;
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x0002C500 File Offset: 0x0002A700
	public FakeDirectoryTree(string name, List<T> files = null, List<FakeDirectoryTree<T>> children = null, IDirectoryTree<T> parent = null)
	{
		this.name = name;
		this.children = children ?? new List<FakeDirectoryTree<T>>();
		this.files = files ?? new List<T>();
		this.parent = parent;
		foreach (IDirectoryTree<T> directoryTree in this.children)
		{
			directoryTree.parent = this;
		}
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x00004AE3 File Offset: 0x00002CE3
	public void Refresh()
	{
	}

	// Token: 0x06000688 RID: 1672 RVA: 0x0002C580 File Offset: 0x0002A780
	public IEnumerable<T> GetFilesRecursive()
	{
		return this.children.SelectMany((IDirectoryTree<T> child) => child.GetFilesRecursive()).Concat(this.files);
	}
}
