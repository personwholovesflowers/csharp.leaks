using System;
using UnityEngine;

// Token: 0x02000057 RID: 87
public class Ferr2DT_Material : ScriptableObject, IFerr2DTMaterial
{
	// Token: 0x17000022 RID: 34
	// (get) Token: 0x0600022F RID: 559 RVA: 0x000100E8 File Offset: 0x0000E2E8
	// (set) Token: 0x06000230 RID: 560 RVA: 0x000100F0 File Offset: 0x0000E2F0
	public Material fillMaterial
	{
		get
		{
			return this._fillMaterial;
		}
		set
		{
			this._fillMaterial = value;
		}
	}

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x06000231 RID: 561 RVA: 0x000100F9 File Offset: 0x0000E2F9
	// (set) Token: 0x06000232 RID: 562 RVA: 0x00010101 File Offset: 0x0000E301
	public Material edgeMaterial
	{
		get
		{
			return this._edgeMaterial;
		}
		set
		{
			this._edgeMaterial = value;
		}
	}

	// Token: 0x06000233 RID: 563 RVA: 0x0001010C File Offset: 0x0000E30C
	public Ferr2DT_Material()
	{
		for (int i = 0; i < this._descriptors.Length; i++)
		{
			this._descriptors[i] = new Ferr2DT_SegmentDescription();
		}
	}

	// Token: 0x06000234 RID: 564 RVA: 0x00010154 File Offset: 0x0000E354
	public Ferr2DT_SegmentDescription GetDescriptor(Ferr2DT_TerrainDirection aDirection)
	{
		this.ConvertToPercentage();
		for (int i = 0; i < this._descriptors.Length; i++)
		{
			if (this._descriptors[i].applyTo == aDirection)
			{
				return this._descriptors[i];
			}
		}
		if (this._descriptors.Length != 0)
		{
			return this._descriptors[0];
		}
		return new Ferr2DT_SegmentDescription();
	}

	// Token: 0x06000235 RID: 565 RVA: 0x000101AC File Offset: 0x0000E3AC
	public bool Has(Ferr2DT_TerrainDirection aDirection)
	{
		for (int i = 0; i < this._descriptors.Length; i++)
		{
			if (this._descriptors[i].applyTo == aDirection)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000236 RID: 566 RVA: 0x000101E0 File Offset: 0x0000E3E0
	public void Set(Ferr2DT_TerrainDirection aDirection, bool aActive)
	{
		if (aActive)
		{
			if (this._descriptors[(int)aDirection].applyTo != aDirection)
			{
				this._descriptors[(int)aDirection] = new Ferr2DT_SegmentDescription();
				this._descriptors[(int)aDirection].applyTo = aDirection;
				return;
			}
		}
		else if (this._descriptors[(int)aDirection].applyTo != Ferr2DT_TerrainDirection.Top)
		{
			this._descriptors[(int)aDirection] = new Ferr2DT_SegmentDescription();
		}
	}

	// Token: 0x06000237 RID: 567 RVA: 0x00010238 File Offset: 0x0000E438
	public Rect ToUV(Rect aNativeRect)
	{
		if (this.edgeMaterial == null)
		{
			return aNativeRect;
		}
		return new Rect(aNativeRect.x, 1f - aNativeRect.height - aNativeRect.y, aNativeRect.width, aNativeRect.height);
	}

	// Token: 0x06000238 RID: 568 RVA: 0x00010284 File Offset: 0x0000E484
	public Rect ToScreen(Rect aNativeRect)
	{
		this.edgeMaterial == null;
		return aNativeRect;
	}

	// Token: 0x06000239 RID: 569 RVA: 0x00010294 File Offset: 0x0000E494
	public Rect GetBody(Ferr2DT_TerrainDirection aDirection, int aBodyID)
	{
		return this.GetDescriptor(aDirection).body[aBodyID];
	}

	// Token: 0x0600023A RID: 570 RVA: 0x000102A8 File Offset: 0x0000E4A8
	private void ConvertToPercentage()
	{
		if (this.isPixel)
		{
			for (int i = 0; i < this._descriptors.Length; i++)
			{
				for (int j = 0; j < this._descriptors[i].body.Length; j++)
				{
					this._descriptors[i].body[j] = this.ToNative(this._descriptors[i].body[j]);
				}
				this._descriptors[i].leftCap = this.ToNative(this._descriptors[i].leftCap);
				this._descriptors[i].rightCap = this.ToNative(this._descriptors[i].rightCap);
			}
			this.isPixel = false;
		}
	}

	// Token: 0x0600023B RID: 571 RVA: 0x00010368 File Offset: 0x0000E568
	public Rect ToNative(Rect aPixelRect)
	{
		if (this.edgeMaterial == null)
		{
			return aPixelRect;
		}
		int num = ((this.edgeMaterial.mainTexture == null) ? 1 : this.edgeMaterial.mainTexture.width);
		int num2 = ((this.edgeMaterial.mainTexture == null) ? 1 : this.edgeMaterial.mainTexture.height);
		return new Rect(aPixelRect.x / (float)num, aPixelRect.y / (float)num2, aPixelRect.width / (float)num, aPixelRect.height / (float)num2);
	}

	// Token: 0x0600023C RID: 572 RVA: 0x00010400 File Offset: 0x0000E600
	public Rect ToPixels(Rect aNativeRect)
	{
		if (this.edgeMaterial == null)
		{
			return aNativeRect;
		}
		int num = ((this.edgeMaterial.mainTexture == null) ? 1 : this.edgeMaterial.mainTexture.width);
		int num2 = ((this.edgeMaterial.mainTexture == null) ? 1 : this.edgeMaterial.mainTexture.height);
		return new Rect(aNativeRect.x * (float)num, aNativeRect.y * (float)num2, aNativeRect.width * (float)num, aNativeRect.height * (float)num2);
	}

	// Token: 0x0600023D RID: 573 RVA: 0x00010498 File Offset: 0x0000E698
	string IFerr2DTMaterial.get_name()
	{
		return base.name;
	}

	// Token: 0x04000252 RID: 594
	[SerializeField]
	private Material _fillMaterial;

	// Token: 0x04000253 RID: 595
	[SerializeField]
	private Material _edgeMaterial;

	// Token: 0x04000254 RID: 596
	[SerializeField]
	private Ferr2DT_SegmentDescription[] _descriptors = new Ferr2DT_SegmentDescription[4];

	// Token: 0x04000255 RID: 597
	[SerializeField]
	private bool isPixel = true;
}
