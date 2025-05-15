using System;
using UnityEngine;

namespace Ferr
{
	// Token: 0x020002DD RID: 733
	public interface IProceduralMesh
	{
		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06001323 RID: 4899
		Mesh MeshData { get; }

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06001324 RID: 4900
		MeshFilter MeshFilter { get; }

		// Token: 0x06001325 RID: 4901
		void Build(bool aFullBuild);
	}
}
