using System;
using UnityEngine;

// Token: 0x02000060 RID: 96
public class AnimatedTexture : MonoBehaviour
{
	// Token: 0x060001D1 RID: 465 RVA: 0x0000983B File Offset: 0x00007A3B
	private void OnValidate()
	{
		this.Setup();
		this.SetTexture();
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x00009849 File Offset: 0x00007A49
	private void Awake()
	{
		this.Setup();
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x00009854 File Offset: 0x00007A54
	private void Setup()
	{
		TextureType textureType = this.textureType;
		if (textureType != TextureType.Main)
		{
			if (textureType == TextureType.Emissive)
			{
				this.texID = AnimatedTexture.EmissiveTexID;
			}
		}
		else
		{
			this.texID = AnimatedTexture.MainTexID;
		}
		this.block = new MaterialPropertyBlock();
		this.renderer = base.GetComponent<Renderer>();
		this.renderer.GetPropertyBlock(this.block);
		this.counter = 0f;
		if (this.arrayTex != null && (this.arrayIndexTexture == null || this.arrayIndexTexture.width != this.arrayTex.width || this.arrayIndexTexture.height != this.arrayTex.height || this.arrayIndexTexture.format != this.arrayTex.format))
		{
			this.arrayIndexTexture = new Texture2D(this.arrayTex.width, this.arrayTex.height, this.arrayTex.format, false);
			this.arrayIndexTexture.filterMode = FilterMode.Point;
		}
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x00009964 File Offset: 0x00007B64
	private void Update()
	{
		if (this.manualTrigger)
		{
			return;
		}
		if (this.counter > this.delay)
		{
			if (this.randomFrame)
			{
				this.SetArraySlice(Random.Range(0, this.arrayTex.depth));
			}
			else
			{
				this.SetTexture();
			}
			this.counter = 0f;
		}
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x000099C4 File Offset: 0x00007BC4
	private void SetTexture()
	{
		this.renderer.GetPropertyBlock(this.block);
		if (this.framePool.Length != 0)
		{
			if (this.selector >= this.framePool.Length)
			{
				this.selector = 0;
			}
			this.block.SetTexture(this.texID, this.framePool[this.selector]);
		}
		else
		{
			if (this.arrayTex == null)
			{
				Debug.Log("MISMIDMSIMFSIMFISMDS " + base.gameObject.name, base.gameObject);
			}
			if (this.selector >= this.arrayTex.depth)
			{
				this.selector = 0;
			}
			Graphics.CopyTexture(this.arrayTex, this.selector, this.arrayIndexTexture, 0);
			this.block.SetTexture(this.texID, this.arrayIndexTexture);
		}
		this.renderer.SetPropertyBlock(this.block);
		this.selector++;
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x00009AB8 File Offset: 0x00007CB8
	public void AddTime(float newTime)
	{
		newTime *= (float)(this.arrayTex.depth - 1);
		Graphics.CopyTexture(this.arrayTex, Mathf.RoundToInt(newTime), this.arrayIndexTexture, 0);
		this.renderer.GetPropertyBlock(this.block);
		this.block.SetTexture(this.texID, this.arrayIndexTexture);
		this.renderer.SetPropertyBlock(this.block);
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x00009B28 File Offset: 0x00007D28
	public void SetArraySlice(int slice)
	{
		Graphics.CopyTexture(this.arrayTex, slice, this.arrayIndexTexture, 0);
		this.renderer.GetPropertyBlock(this.block);
		this.block.SetTexture(this.texID, this.arrayIndexTexture);
		this.renderer.SetPropertyBlock(this.block);
	}

	// Token: 0x040001F3 RID: 499
	[SerializeField]
	private bool randomFrame;

	// Token: 0x040001F4 RID: 500
	[SerializeField]
	private bool manualTrigger;

	// Token: 0x040001F5 RID: 501
	[SerializeField]
	private float delay = 0.0666f;

	// Token: 0x040001F6 RID: 502
	[SerializeField]
	private Texture2D[] framePool;

	// Token: 0x040001F7 RID: 503
	[SerializeField]
	public Texture2DArray arrayTex;

	// Token: 0x040001F8 RID: 504
	[SerializeField]
	private TextureType textureType;

	// Token: 0x040001F9 RID: 505
	private TimeSince counter;

	// Token: 0x040001FA RID: 506
	private int selector;

	// Token: 0x040001FB RID: 507
	private MaterialPropertyBlock block;

	// Token: 0x040001FC RID: 508
	private Renderer renderer;

	// Token: 0x040001FD RID: 509
	private static readonly int MainTexID = Shader.PropertyToID("_MainTex");

	// Token: 0x040001FE RID: 510
	private static readonly int EmissiveTexID = Shader.PropertyToID("_EmissiveTex");

	// Token: 0x040001FF RID: 511
	private Texture2D arrayIndexTexture;

	// Token: 0x04000200 RID: 512
	private int texID;
}
