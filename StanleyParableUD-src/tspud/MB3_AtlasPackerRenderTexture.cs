using System;
using System.Collections.Generic;
using DigitalOpus.MB.Core;
using UnityEngine;

// Token: 0x02000076 RID: 118
[ExecuteInEditMode]
public class MB3_AtlasPackerRenderTexture : MonoBehaviour
{
	// Token: 0x060002DE RID: 734 RVA: 0x000139A8 File Offset: 0x00011BA8
	public Texture2D OnRenderAtlas(MB3_TextureCombiner combiner)
	{
		this.fastRenderer = new MB_TextureCombinerRenderTexture();
		this._doRenderAtlas = true;
		Texture2D texture2D = this.fastRenderer.DoRenderAtlas(base.gameObject, this.width, this.height, this.padding, this.rects, this.textureSets, this.indexOfTexSetToRender, this.texPropertyName, this.resultMaterialTextureBlender, this.isNormalMap, this.fixOutOfBoundsUVs, this.considerNonTextureProperties, combiner, this.LOG_LEVEL);
		this._doRenderAtlas = false;
		return texture2D;
	}

	// Token: 0x060002DF RID: 735 RVA: 0x00013A28 File Offset: 0x00011C28
	private void OnRenderObject()
	{
		if (this._doRenderAtlas)
		{
			this.fastRenderer.OnRenderObject();
			this._doRenderAtlas = false;
		}
	}

	// Token: 0x040002D5 RID: 725
	private MB_TextureCombinerRenderTexture fastRenderer;

	// Token: 0x040002D6 RID: 726
	private bool _doRenderAtlas;

	// Token: 0x040002D7 RID: 727
	public int width;

	// Token: 0x040002D8 RID: 728
	public int height;

	// Token: 0x040002D9 RID: 729
	public int padding;

	// Token: 0x040002DA RID: 730
	public bool isNormalMap;

	// Token: 0x040002DB RID: 731
	public bool fixOutOfBoundsUVs;

	// Token: 0x040002DC RID: 732
	public bool considerNonTextureProperties;

	// Token: 0x040002DD RID: 733
	public TextureBlender resultMaterialTextureBlender;

	// Token: 0x040002DE RID: 734
	public Rect[] rects;

	// Token: 0x040002DF RID: 735
	public Texture2D tex1;

	// Token: 0x040002E0 RID: 736
	public List<MB3_TextureCombiner.MB_TexSet> textureSets;

	// Token: 0x040002E1 RID: 737
	public int indexOfTexSetToRender;

	// Token: 0x040002E2 RID: 738
	public ShaderTextureProperty texPropertyName;

	// Token: 0x040002E3 RID: 739
	public MB2_LogLevel LOG_LEVEL = MB2_LogLevel.info;

	// Token: 0x040002E4 RID: 740
	public Texture2D testTex;

	// Token: 0x040002E5 RID: 741
	public Material testMat;
}
