using System;
using UnityEngine;

// Token: 0x02000062 RID: 98
[Serializable]
public class MB_MaterialAndUVRect
{
	// Token: 0x06000258 RID: 600 RVA: 0x00010503 File Offset: 0x0000E703
	public MB_MaterialAndUVRect(Material m, Rect destRect, Rect samplingRectMatAndUVTiling, Rect sourceMaterialTiling, Rect samplingEncapsulatinRect, string objName)
	{
		this.material = m;
		this.atlasRect = destRect;
		this.samplingRectMatAndUVTiling = samplingRectMatAndUVTiling;
		this.sourceMaterialTiling = sourceMaterialTiling;
		this.samplingEncapsulatinRect = samplingEncapsulatinRect;
		this.srcObjName = objName;
	}

	// Token: 0x06000259 RID: 601 RVA: 0x00010538 File Offset: 0x0000E738
	public override int GetHashCode()
	{
		return this.material.GetInstanceID() ^ this.samplingEncapsulatinRect.GetHashCode();
	}

	// Token: 0x0600025A RID: 602 RVA: 0x00010557 File Offset: 0x0000E757
	public override bool Equals(object obj)
	{
		return obj is MB_MaterialAndUVRect && this.material == ((MB_MaterialAndUVRect)obj).material && this.samplingEncapsulatinRect == ((MB_MaterialAndUVRect)obj).samplingEncapsulatinRect;
	}

	// Token: 0x0400026C RID: 620
	public Material material;

	// Token: 0x0400026D RID: 621
	public Rect atlasRect;

	// Token: 0x0400026E RID: 622
	public string srcObjName;

	// Token: 0x0400026F RID: 623
	public Rect samplingRectMatAndUVTiling;

	// Token: 0x04000270 RID: 624
	public Rect sourceMaterialTiling;

	// Token: 0x04000271 RID: 625
	public Rect samplingEncapsulatinRect;
}
