using System;
using UnityEngine;

namespace LucasMeshCombine
{
	// Token: 0x0200057C RID: 1404
	public class MeshCombineData
	{
		// Token: 0x06001FD9 RID: 8153 RVA: 0x00102447 File Offset: 0x00100647
		public MeshCombineData(GameObject parent, MeshRenderer meshRenderer, MeshFilter meshFilter, Texture2D texture, int subMeshIndex)
		{
			this.Parent = parent;
			this.MeshRenderer = meshRenderer;
			this.MeshFilter = meshFilter;
			this.Texture = texture;
			this.SubMeshIndex = subMeshIndex;
		}

		// Token: 0x04002C15 RID: 11285
		public readonly GameObject Parent;

		// Token: 0x04002C16 RID: 11286
		public readonly MeshRenderer MeshRenderer;

		// Token: 0x04002C17 RID: 11287
		public readonly MeshFilter MeshFilter;

		// Token: 0x04002C18 RID: 11288
		public readonly Texture2D Texture;

		// Token: 0x04002C19 RID: 11289
		public readonly int SubMeshIndex;
	}
}
