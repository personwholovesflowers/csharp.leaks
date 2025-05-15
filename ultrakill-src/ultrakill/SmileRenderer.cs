using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004FE RID: 1278
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(RawImage))]
public class SmileRenderer : MonoBehaviour
{
	// Token: 0x06001D31 RID: 7473 RVA: 0x000F4D26 File Offset: 0x000F2F26
	private void OnValidate()
	{
		if (!Application.isPlaying)
		{
			this.Setup();
		}
	}

	// Token: 0x06001D32 RID: 7474 RVA: 0x000F4D35 File Offset: 0x000F2F35
	private void OnEnable()
	{
		if (Application.isPlaying)
		{
			base.StartCoroutine(this.WaitSetup());
		}
	}

	// Token: 0x06001D33 RID: 7475 RVA: 0x000F4D4B File Offset: 0x000F2F4B
	private IEnumerator WaitSetup()
	{
		HudOpenEffect opener = base.GetComponentInParent<HudOpenEffect>();
		this.displayImage = base.GetComponent<RawImage>();
		this.displayImage.color = Color.clear;
		if (opener != null)
		{
			if (!opener.animating)
			{
				yield return null;
			}
			while (opener.animating)
			{
				yield return null;
			}
		}
		this.Setup();
		this.CreateTex();
		this.displayImage.color = Color.white;
		yield break;
	}

	// Token: 0x06001D34 RID: 7476 RVA: 0x000F4D5C File Offset: 0x000F2F5C
	private void Setup()
	{
		this.cam = base.GetComponent<Camera>();
		this.cam.cullingMask = 1 << LayerMask.NameToLayer("Invisible");
		this.cam.clearFlags = CameraClearFlags.Color;
		this.cam.backgroundColor = Color.clear;
		this.cam.orthographic = true;
		this.cam.nearClipPlane = -1f;
		this.cam.farClipPlane = 1f;
		this.pixelScale = 1f;
		this.scaler = base.GetComponentInParent<CanvasScaler>();
		if (this.scaler != null)
		{
			this.pixelScale = this.scaler.referencePixelsPerUnit;
		}
		this.displayImage = base.GetComponent<RawImage>();
		this.displayImage.raycastTarget = false;
		Vector3[] array = new Vector3[4];
		this.displayImage.rectTransform.GetWorldCorners(array);
		this.width = Vector3.Distance(array[0], array[3]);
		this.height = Vector3.Distance(array[0], array[1]);
		this.pixelWidth = (int)(this.width * this.pixelScale);
		this.pixelHeight = (int)(this.height * this.pixelScale);
		this.cam.orthographicSize = Mathf.Max(this.width, this.height) / 2f;
	}

	// Token: 0x06001D35 RID: 7477 RVA: 0x000F4EBC File Offset: 0x000F30BC
	private void CreateTex()
	{
		if (this.pixelWidth <= 0 || this.pixelHeight <= 0)
		{
			return;
		}
		this.rt = new RenderTexture(this.pixelWidth, this.pixelHeight, 32, RenderTextureFormat.ARGB32);
		this.rt.filterMode = FilterMode.Point;
		this.cam.targetTexture = this.rt;
		this.displayImage.texture = this.rt;
	}

	// Token: 0x04002963 RID: 10595
	public float width;

	// Token: 0x04002964 RID: 10596
	public float height;

	// Token: 0x04002965 RID: 10597
	private Camera cam;

	// Token: 0x04002966 RID: 10598
	private RenderTexture rt;

	// Token: 0x04002967 RID: 10599
	private RawImage displayImage;

	// Token: 0x04002968 RID: 10600
	private CanvasScaler scaler;

	// Token: 0x04002969 RID: 10601
	private float pixelScale;

	// Token: 0x0400296A RID: 10602
	private int pixelWidth;

	// Token: 0x0400296B RID: 10603
	private int pixelHeight;
}
