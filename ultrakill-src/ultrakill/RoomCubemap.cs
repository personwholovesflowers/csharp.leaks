using System;
using UnityEngine;

// Token: 0x020004FC RID: 1276
public class RoomCubemap : MonoBehaviour
{
	// Token: 0x06001D2C RID: 7468 RVA: 0x000F4B44 File Offset: 0x000F2D44
	private void Awake()
	{
		this.room = base.transform.parent;
		this.roomObjects = this.room.GetComponentsInChildren<MeshRenderer>(true);
		this.cam = base.gameObject.AddComponent<Camera>();
		this.cam.enabled = false;
		if (this.automaticPosition)
		{
			this.roomBounds = this.roomObjects[0].bounds;
			for (int i = 1; i < this.roomObjects.Length; i++)
			{
				this.roomBounds.Encapsulate(this.roomObjects[i].bounds);
			}
			this.cam.transform.position = this.roomBounds.center;
		}
		int num = 1;
		num |= 64;
		num |= 128;
		num |= 256;
		num |= 16777216;
		num |= 33554432;
		this.cam.cullingMask = num;
		this.cubemap = new Cubemap(256, TextureFormat.ARGB32, false);
		this.cubemap.filterMode = FilterMode.Point;
		this.propertyBlock = new MaterialPropertyBlock();
	}

	// Token: 0x06001D2D RID: 7469 RVA: 0x000F4C4E File Offset: 0x000F2E4E
	private void OnEnable()
	{
		this.UpdateCubemap();
	}

	// Token: 0x06001D2E RID: 7470 RVA: 0x000F4C58 File Offset: 0x000F2E58
	public void UpdateCubemap()
	{
		this.cam.RenderToCubemap(this.cubemap);
		this.roomObjects = this.room.GetComponentsInChildren<MeshRenderer>(true);
		for (int i = 0; i < this.roomObjects.Length; i++)
		{
			MeshRenderer meshRenderer = this.roomObjects[i];
			meshRenderer.GetPropertyBlock(this.propertyBlock);
			this.propertyBlock.SetTexture("_CubeTex", this.cubemap);
			this.propertyBlock.SetFloat("_CubeMode", (float)this.cubemapMode);
			this.propertyBlock.SetFloat("_ReflectionStrength", this.cubemapStrength);
			meshRenderer.SetPropertyBlock(this.propertyBlock);
		}
	}

	// Token: 0x06001D2F RID: 7471 RVA: 0x000F4CFE File Offset: 0x000F2EFE
	public void DelayUpdate(float delayTime)
	{
		base.Invoke("UpdateCubemap", delayTime);
	}

	// Token: 0x04002957 RID: 10583
	public bool automaticPosition = true;

	// Token: 0x04002958 RID: 10584
	public CubemapMode cubemapMode;

	// Token: 0x04002959 RID: 10585
	public float cubemapStrength = 0.5f;

	// Token: 0x0400295A RID: 10586
	private Transform room;

	// Token: 0x0400295B RID: 10587
	private MeshRenderer[] roomObjects;

	// Token: 0x0400295C RID: 10588
	private Bounds roomBounds;

	// Token: 0x0400295D RID: 10589
	private Cubemap cubemap;

	// Token: 0x0400295E RID: 10590
	private Camera cam;

	// Token: 0x0400295F RID: 10591
	private MaterialPropertyBlock propertyBlock;
}
