using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000060 RID: 96
[Serializable]
public class MB_AtlasesAndRects
{
	// Token: 0x04000266 RID: 614
	public Texture2D[] atlases;

	// Token: 0x04000267 RID: 615
	[NonSerialized]
	public List<MB_MaterialAndUVRect> mat2rect_map;

	// Token: 0x04000268 RID: 616
	public string[] texPropertyNames;
}
