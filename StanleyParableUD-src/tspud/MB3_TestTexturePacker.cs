using System;
using System.Collections.Generic;
using DigitalOpus.MB.Core;
using UnityEngine;

// Token: 0x02000074 RID: 116
public class MB3_TestTexturePacker : MonoBehaviour
{
	// Token: 0x060002CF RID: 719 RVA: 0x00012340 File Offset: 0x00010540
	[ContextMenu("Generate List Of Images To Add")]
	public void GenerateListOfImagesToAdd()
	{
		this.imgsToAdd = new List<Vector2>();
		for (int i = 0; i < this.numTex; i++)
		{
			Vector2 vector = new Vector2((float)Mathf.RoundToInt((float)Random.Range(this.min, this.max) * this.xMult), (float)Mathf.RoundToInt((float)Random.Range(this.min, this.max) * this.yMult));
			if (this.imgsMustBePowerOfTwo)
			{
				vector.x = (float)MB2_TexturePacker.RoundToNearestPositivePowerOfTwo((int)vector.x);
				vector.y = (float)MB2_TexturePacker.RoundToNearestPositivePowerOfTwo((int)vector.y);
			}
			this.imgsToAdd.Add(vector);
		}
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x000123F0 File Offset: 0x000105F0
	[ContextMenu("Run")]
	public void RunTestHarness()
	{
		this.texturePacker = new MB2_TexturePacker();
		this.texturePacker.doPowerOfTwoTextures = this.doPowerOfTwoTextures;
		this.texturePacker.LOG_LEVEL = this.logLevel;
		this.rs = this.texturePacker.GetRects(this.imgsToAdd, this.maxDim, this.padding, this.doMultiAtlas);
		if (this.rs != null)
		{
			Debug.Log("NumAtlas= " + this.rs.Length);
			for (int i = 0; i < this.rs.Length; i++)
			{
				for (int j = 0; j < this.rs[i].rects.Length; j++)
				{
					Rect rect = this.rs[i].rects[j];
					rect.x *= (float)this.rs[i].atlasX;
					rect.y *= (float)this.rs[i].atlasY;
					rect.width *= (float)this.rs[i].atlasX;
					rect.height *= (float)this.rs[i].atlasY;
					Debug.Log(rect.ToString("f5"));
				}
				Debug.Log("===============");
			}
			this.res = string.Concat(new object[]
			{
				"mxX= ",
				this.rs[0].atlasX,
				" mxY= ",
				this.rs[0].atlasY
			});
			return;
		}
		this.res = "ERROR: PACKING FAILED";
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x000125AC File Offset: 0x000107AC
	private void OnDrawGizmos()
	{
		if (this.rs != null)
		{
			for (int i = 0; i < this.rs.Length; i++)
			{
				Vector2 vector = new Vector2((float)i * 1.5f * (float)this.maxDim, 0f);
				AtlasPackingResult atlasPackingResult = this.rs[i];
				Vector2 vector2 = new Vector2(vector.x + (float)(atlasPackingResult.atlasX / 2), vector.y + (float)(atlasPackingResult.atlasY / 2));
				Vector2 vector3 = new Vector2((float)atlasPackingResult.atlasX, (float)atlasPackingResult.atlasY);
				Gizmos.DrawWireCube(vector2, vector3);
				for (int j = 0; j < this.rs[i].rects.Length; j++)
				{
					Rect rect = this.rs[i].rects[j];
					Gizmos.color = new Color(Random.value, Random.value, Random.value);
					Vector2 vector4 = new Vector2(vector.x + (rect.x + rect.width / 2f) * (float)this.rs[i].atlasX, vector.y + (rect.y + rect.height / 2f) * (float)this.rs[i].atlasY);
					Vector2 vector5 = new Vector2(rect.width * (float)this.rs[i].atlasX, rect.height * (float)this.rs[i].atlasY);
					Gizmos.DrawCube(vector4, vector5);
				}
			}
		}
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x00012740 File Offset: 0x00010940
	[ContextMenu("Test1")]
	private void Test1()
	{
		this.texturePacker = new MB2_TexturePacker();
		this.texturePacker.doPowerOfTwoTextures = true;
		List<Vector2> list = new List<Vector2>();
		list.Add(new Vector2(450f, 200f));
		list.Add(new Vector2(450f, 200f));
		list.Add(new Vector2(450f, 80f));
		this.texturePacker.LOG_LEVEL = this.logLevel;
		this.rs = this.texturePacker.GetRects(list, 512, 8, true);
		Debug.Log("Success! ");
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x000127E0 File Offset: 0x000109E0
	[ContextMenu("Test2")]
	private void Test2()
	{
		this.texturePacker = new MB2_TexturePacker();
		this.texturePacker.doPowerOfTwoTextures = true;
		List<Vector2> list = new List<Vector2>();
		list.Add(new Vector2(200f, 450f));
		list.Add(new Vector2(200f, 450f));
		list.Add(new Vector2(80f, 450f));
		this.texturePacker.LOG_LEVEL = this.logLevel;
		this.rs = this.texturePacker.GetRects(list, 512, 8, true);
		Debug.Log("Success! ");
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x00012880 File Offset: 0x00010A80
	[ContextMenu("Test3")]
	private void Test3()
	{
		this.texturePacker = new MB2_TexturePacker();
		this.texturePacker.doPowerOfTwoTextures = false;
		List<Vector2> list = new List<Vector2>();
		list.Add(new Vector2(450f, 200f));
		list.Add(new Vector2(450f, 200f));
		list.Add(new Vector2(450f, 80f));
		this.texturePacker.LOG_LEVEL = this.logLevel;
		this.rs = this.texturePacker.GetRects(list, 512, 8, true);
		Debug.Log("Success! ");
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x00012920 File Offset: 0x00010B20
	[ContextMenu("Test4")]
	private void Test4()
	{
		this.texturePacker = new MB2_TexturePacker();
		this.texturePacker.doPowerOfTwoTextures = false;
		List<Vector2> list = new List<Vector2>();
		list.Add(new Vector2(200f, 450f));
		list.Add(new Vector2(200f, 450f));
		list.Add(new Vector2(80f, 450f));
		this.texturePacker.LOG_LEVEL = this.logLevel;
		this.rs = this.texturePacker.GetRects(list, 512, 8, true);
		Debug.Log("Success! ");
	}

	// Token: 0x040002B7 RID: 695
	private MB2_TexturePacker texturePacker;

	// Token: 0x040002B8 RID: 696
	public int numTex = 32;

	// Token: 0x040002B9 RID: 697
	public int min = 126;

	// Token: 0x040002BA RID: 698
	public int max = 2046;

	// Token: 0x040002BB RID: 699
	public float xMult = 1f;

	// Token: 0x040002BC RID: 700
	public float yMult = 1f;

	// Token: 0x040002BD RID: 701
	public bool imgsMustBePowerOfTwo;

	// Token: 0x040002BE RID: 702
	public List<Vector2> imgsToAdd = new List<Vector2>();

	// Token: 0x040002BF RID: 703
	public int padding = 1;

	// Token: 0x040002C0 RID: 704
	public int maxDim = 4096;

	// Token: 0x040002C1 RID: 705
	public bool doPowerOfTwoTextures = true;

	// Token: 0x040002C2 RID: 706
	public bool doMultiAtlas;

	// Token: 0x040002C3 RID: 707
	public MB2_LogLevel logLevel;

	// Token: 0x040002C4 RID: 708
	public string res;

	// Token: 0x040002C5 RID: 709
	public AtlasPackingResult[] rs;
}
