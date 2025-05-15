using System;
using UnityEngine;

// Token: 0x02000052 RID: 82
public class Painter : MonoBehaviour
{
	// Token: 0x0600020A RID: 522 RVA: 0x0000EDD0 File Offset: 0x0000CFD0
	private void OnGUI()
	{
		GUI.skin = this.gskin;
		GUILayout.BeginArea(new Rect((float)Screen.width * this.uIOffsets.mainPainterBox.x, (float)Screen.height * this.uIOffsets.mainPainterBox.y, (float)Screen.width * 0.9f, (float)Screen.height * 0.9f), "", "PurpleBox");
		GUILayout.BeginArea(new Rect((float)Screen.width * this.uIOffsets.toolsbox.x, (float)Screen.height * this.uIOffsets.toolsbox.y, (float)Screen.width * 0.15f, (float)Screen.height));
		GUILayout.Space(10f);
		GUILayout.Label("Drawing Options", Array.Empty<GUILayoutOption>());
		this.tool = (Painter.Tool)GUILayout.Toolbar((int)this.tool, this.toolIcons, "Tool", Array.Empty<GUILayoutOption>());
		GUILayout.Space(10f);
		Painter.Tool tool = this.tool;
		if (tool != Painter.Tool.Brush)
		{
			if (tool == Painter.Tool.Eraser)
			{
				GUILayout.Label("Size " + Mathf.Round(this.eraser.width * 10f) / 10f, Array.Empty<GUILayoutOption>());
				this.eraser.width = GUILayout.HorizontalSlider(this.eraser.width, 0f, 50f, Array.Empty<GUILayoutOption>());
				GUILayout.Space(50f);
			}
		}
		else
		{
			GUILayout.Label("Size " + Mathf.Round(this.brush.width * 10f) / 10f, Array.Empty<GUILayoutOption>());
			this.brush.width = GUILayout.HorizontalSlider(this.brush.width, 0f, 40f, Array.Empty<GUILayoutOption>());
			GUILayout.Space(10f);
			this.brushColor = GUIControls.RGBCircle(this.brushColor, "Color Picker", this.colorCircleTex);
			GUILayout.Space(10f);
		}
		if (GUILayout.Button(this.clearDwatingIcon, "ClearDrawing", Array.Empty<GUILayoutOption>()))
		{
			this.OnEnable();
		}
		GUILayout.Space(10f);
		if (GUILayout.Button("Save", "Save", Array.Empty<GUILayoutOption>()))
		{
			Drawing.MergeTextures(ref this.baseTex, ref this.drawingTex, this.drawingTextureDownScalingRatio);
			base.enabled = false;
		}
		GUILayout.EndArea();
		GUI.DrawTexture(new Rect((float)Screen.width * this.uIOffsets.canvas.x, (float)Screen.height * this.uIOffsets.canvas.y, (float)Screen.width * this.canvasScaleRatio, (float)Screen.height * this.canvasScaleRatio), this.baseTex);
		GUI.DrawTexture(new Rect((float)Screen.width * this.uIOffsets.canvas.x, (float)Screen.height * this.uIOffsets.canvas.y, (float)Screen.width * this.canvasScaleRatio, (float)Screen.height * this.canvasScaleRatio), this.drawingTex);
		GUILayout.BeginArea(new Rect((float)Screen.width * (1f - this.uIOffsets.toolsbox.x) - (float)Screen.width * 0.15f, (float)Screen.height * this.uIOffsets.toolsbox.y, (float)Screen.width * 0.15f, (float)Screen.height));
		if (GUILayout.Button("", "Close", Array.Empty<GUILayoutOption>()))
		{
			base.enabled = false;
		}
		GUILayout.EndArea();
		GUILayout.EndArea();
	}

	// Token: 0x0600020B RID: 523 RVA: 0x0000F190 File Offset: 0x0000D390
	private void Update()
	{
		this.imgRect = new Rect((float)Screen.width * (this.uIOffsets.canvas.x + this.uIOffsets.mainPainterBox.x), (float)Screen.height * (this.uIOffsets.canvas.y + this.uIOffsets.mainPainterBox.y), (float)Screen.width * this.canvasScaleRatio, (float)Screen.height * this.canvasScaleRatio);
		Vector2 vector = Input.mousePosition;
		vector.y = (float)Screen.height - vector.y;
		if (Input.GetKeyDown("mouse 0"))
		{
			if (this.imgRect.Contains(vector))
			{
				this.dragStart = vector - new Vector2(this.imgRect.x, this.imgRect.y);
				this.dragStart.y = this.imgRect.height - this.dragStart.y;
				this.dragStart.x = Mathf.Round(this.dragStart.x * (this.canvasScaleRatio * (float)this.drawingTextureDownScalingRatio));
				this.dragStart.y = Mathf.Round(this.dragStart.y * (this.canvasScaleRatio * (float)this.drawingTextureDownScalingRatio));
				this.ClampCursor(vector, this.imgRect);
			}
			else
			{
				this.dragStart = Vector3.zero;
			}
		}
		if (Input.GetKey("mouse 0"))
		{
			if (this.dragStart == Vector2.zero)
			{
				return;
			}
			this.ClampCursor(vector, this.imgRect);
			if (this.tool == Painter.Tool.Brush)
			{
				this.Brush(this.dragEnd, this.preDrag);
			}
			if (this.tool == Painter.Tool.Eraser)
			{
				this.Eraser(this.dragEnd, this.preDrag);
			}
		}
		if (Input.GetKeyUp("mouse 0") && this.dragStart != Vector2.zero)
		{
			this.dragStart = Vector2.zero;
			this.dragEnd = Vector2.zero;
		}
		this.preDrag = this.dragEnd;
	}

	// Token: 0x0600020C RID: 524 RVA: 0x0000F3B0 File Offset: 0x0000D5B0
	private void ClampCursor(Vector2 mouse, Rect imgRect)
	{
		this.dragEnd = mouse - new Vector2(imgRect.x, imgRect.y);
		this.dragEnd.x = Mathf.Clamp(this.dragEnd.x, 0f, imgRect.width);
		this.dragEnd.y = imgRect.height - Mathf.Clamp(this.dragEnd.y, 0f, imgRect.height * 2f);
		this.dragEnd.x = Mathf.Round(this.dragEnd.x / (this.canvasScaleRatio * (float)this.drawingTextureDownScalingRatio));
		this.dragEnd.y = Mathf.Round(this.dragEnd.y / (this.canvasScaleRatio * (float)this.drawingTextureDownScalingRatio));
	}

	// Token: 0x0600020D RID: 525 RVA: 0x0000F48C File Offset: 0x0000D68C
	private void Brush(Vector2 p1, Vector2 p2)
	{
		if (p2 == Vector2.zero)
		{
			p2 = p1;
		}
		this.stroke.Set(p1, p2, this.brush.width, this.brush.hardness, this.brushColor);
		Drawing.PaintLine(this.stroke, ref this.drawingTex);
	}

	// Token: 0x0600020E RID: 526 RVA: 0x0000F4E4 File Offset: 0x0000D6E4
	private void Eraser(Vector2 p1, Vector2 p2)
	{
		if (p2 == Vector2.zero)
		{
			p2 = p1;
		}
		this.stroke.Set(p1, p2, this.eraser.width, this.eraser.hardness, Color.clear);
		Drawing.PaintLine(this.stroke, ref this.drawingTex);
	}

	// Token: 0x0600020F RID: 527 RVA: 0x0000F53A File Offset: 0x0000D73A
	public void OnDisable()
	{
		if (this._OnDisable != null)
		{
			this._OnDisable();
		}
	}

	// Token: 0x06000210 RID: 528 RVA: 0x0000F550 File Offset: 0x0000D750
	public void OnEnable()
	{
		if (this.drawingTex != null)
		{
			Object.Destroy(this.drawingTex);
		}
		this.drawingTex = new Texture2D(Screen.width / this.drawingTextureDownScalingRatio, Screen.height / this.drawingTextureDownScalingRatio, TextureFormat.RGBA32, false, true);
		Color[] array = new Color[Screen.width * Screen.height / this.drawingTextureDownScalingRatio];
		this.drawingTex.SetPixels(array, 0);
		this.drawingTex.Apply(false);
	}

	// Token: 0x0400021D RID: 541
	[Header("Drawing Toolset")]
	public Painter.Tool tool;

	// Token: 0x0400021E RID: 542
	public Texture[] toolIcons;

	// Token: 0x0400021F RID: 543
	public Texture2D colorCircleTex;

	// Token: 0x04000220 RID: 544
	public Texture2D clearDwatingIcon;

	// Token: 0x04000221 RID: 545
	public Color brushColor = Color.white;

	// Token: 0x04000222 RID: 546
	[Header("Canvas")]
	[Tooltip("A value of 2 would mean that the transparent texture to be drawn on top off is going to be half of the resolution of the screenshot taken, this saves a lot of perfomance but makes drawing look a bit more pixelated")]
	public int drawingTextureDownScalingRatio = 2;

	// Token: 0x04000223 RID: 547
	public GUISkin gskin;

	// Token: 0x04000224 RID: 548
	[Header("Organization")]
	public Painter.UIOffsets uIOffsets;

	// Token: 0x04000225 RID: 549
	[Tooltip("The scale of the canvas in proportion to the size of the screen")]
	public float canvasScaleRatio = 0.675f;

	// Token: 0x04000226 RID: 550
	public Action _OnDisable;

	// Token: 0x04000227 RID: 551
	[HideInInspector]
	public Texture2D baseTex;

	// Token: 0x04000228 RID: 552
	private Texture2D drawingTex;

	// Token: 0x04000229 RID: 553
	private Painter.BrushTool brush = new Painter.BrushTool();

	// Token: 0x0400022A RID: 554
	private Painter.EraserTool eraser = new Painter.EraserTool();

	// Token: 0x0400022B RID: 555
	private Vector2 dragStart;

	// Token: 0x0400022C RID: 556
	private Vector2 dragEnd;

	// Token: 0x0400022D RID: 557
	private Vector2 preDrag;

	// Token: 0x0400022E RID: 558
	private Rect imgRect;

	// Token: 0x0400022F RID: 559
	private Drawing.Stroke stroke = new Drawing.Stroke();

	// Token: 0x0200036A RID: 874
	public enum Tool
	{
		// Token: 0x04001265 RID: 4709
		Brush,
		// Token: 0x04001266 RID: 4710
		Eraser,
		// Token: 0x04001267 RID: 4711
		None
	}

	// Token: 0x0200036B RID: 875
	public class EraserTool
	{
		// Token: 0x04001268 RID: 4712
		public float width = 2f;

		// Token: 0x04001269 RID: 4713
		public float hardness = 50f;
	}

	// Token: 0x0200036C RID: 876
	public class BrushTool
	{
		// Token: 0x0400126A RID: 4714
		public float width = 1f;

		// Token: 0x0400126B RID: 4715
		public float hardness = 50f;

		// Token: 0x0400126C RID: 4716
		public float spacing = 10f;
	}

	// Token: 0x0200036D RID: 877
	[Serializable]
	public class UIOffsets
	{
		// Token: 0x17000251 RID: 593
		// (get) Token: 0x060015ED RID: 5613 RVA: 0x00075069 File Offset: 0x00073269
		// (set) Token: 0x060015EE RID: 5614 RVA: 0x00005444 File Offset: 0x00003644
		[HideInInspector]
		public Vector2 canvas
		{
			get
			{
				this._canvas.Set(this.canvasBox, this.canvasBox * 0.5f);
				return this._canvas;
			}
			set
			{
			}
		}

		// Token: 0x0400126D RID: 4717
		public Vector2 mainPainterBox = new Vector2(0.05f, 0.05f);

		// Token: 0x0400126E RID: 4718
		public Vector2 toolsbox = new Vector2(0.025f, 0.025f);

		// Token: 0x0400126F RID: 4719
		public float canvasBox = 0.2f;

		// Token: 0x04001270 RID: 4720
		private Vector2 _canvas;
	}
}
