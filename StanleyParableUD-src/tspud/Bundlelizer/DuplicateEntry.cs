using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bundlelizer
{
	// Token: 0x020002F9 RID: 761
	[Serializable]
	public class DuplicateEntry : IComparable
	{
		// Token: 0x060013D1 RID: 5073 RVA: 0x00068F81 File Offset: 0x00067181
		public DuplicateEntry(string name)
		{
			this.DuplicateName = name;
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x00068F9B File Offset: 0x0006719B
		public void AddDuplicatePath(AssetEntry newEntry)
		{
			this.DuplicateEntries.Add(newEntry);
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x00068FA9 File Offset: 0x000671A9
		public int CompareTo(object obj)
		{
			return this.DuplicateName.CompareTo((obj as DuplicateEntry).DuplicateName);
		}

		// Token: 0x04000F58 RID: 3928
		[SerializeField]
		public List<AssetEntry> DuplicateEntries = new List<AssetEntry>();

		// Token: 0x04000F59 RID: 3929
		[SerializeField]
		public string DuplicateName;
	}
}
