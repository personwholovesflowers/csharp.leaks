using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Token: 0x02000148 RID: 328
public class FileDirectoryTree : IDirectoryTree<FileInfo>, IDirectoryTree
{
	// Token: 0x1700008C RID: 140
	// (get) Token: 0x06000669 RID: 1641 RVA: 0x0002C2F5 File Offset: 0x0002A4F5
	// (set) Token: 0x0600066A RID: 1642 RVA: 0x0002C2FD File Offset: 0x0002A4FD
	public string name { get; private set; }

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x0600066B RID: 1643 RVA: 0x0002C306 File Offset: 0x0002A506
	// (set) Token: 0x0600066C RID: 1644 RVA: 0x0002C30E File Offset: 0x0002A50E
	public DirectoryInfo realDirectory { get; private set; }

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x0600066D RID: 1645 RVA: 0x0002C317 File Offset: 0x0002A517
	// (set) Token: 0x0600066E RID: 1646 RVA: 0x0002C31F File Offset: 0x0002A51F
	public IDirectoryTree<FileInfo> parent { get; set; }

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x0600066F RID: 1647 RVA: 0x0002C328 File Offset: 0x0002A528
	// (set) Token: 0x06000670 RID: 1648 RVA: 0x0002C330 File Offset: 0x0002A530
	public IEnumerable<IDirectoryTree<FileInfo>> children { get; private set; }

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x06000671 RID: 1649 RVA: 0x0002C339 File Offset: 0x0002A539
	// (set) Token: 0x06000672 RID: 1650 RVA: 0x0002C341 File Offset: 0x0002A541
	public IEnumerable<FileInfo> files { get; private set; }

	// Token: 0x06000673 RID: 1651 RVA: 0x0002C34A File Offset: 0x0002A54A
	public FileDirectoryTree(string path, IDirectoryTree<FileInfo> parent = null)
	{
		this.realDirectory = new DirectoryInfo(path);
		this.parent = parent;
		this.Refresh();
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x0002C36B File Offset: 0x0002A56B
	public FileDirectoryTree(DirectoryInfo realDirectory, IDirectoryTree<FileInfo> parent = null)
	{
		this.realDirectory = realDirectory;
		this.parent = parent;
		this.Refresh();
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x0002C388 File Offset: 0x0002A588
	public void Refresh()
	{
		this.realDirectory.Create();
		this.name = this.realDirectory.Name;
		this.children = from dir in this.realDirectory.GetDirectories()
			select new FileDirectoryTree(dir, this);
		this.files = this.realDirectory.GetFiles();
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x0002C3E4 File Offset: 0x0002A5E4
	public override bool Equals(object obj)
	{
		return obj != null && !(base.GetType() != obj.GetType()) && this.realDirectory.FullName.ToUpperInvariant() == (obj as DirectoryInfo).FullName.ToUpperInvariant();
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x0002C423 File Offset: 0x0002A623
	public override int GetHashCode()
	{
		return this.realDirectory.GetHashCode();
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x0002C430 File Offset: 0x0002A630
	public IEnumerable<FileInfo> GetFilesRecursive()
	{
		return this.children.SelectMany((IDirectoryTree<FileInfo> child) => child.GetFilesRecursive()).Concat(this.files);
	}
}
