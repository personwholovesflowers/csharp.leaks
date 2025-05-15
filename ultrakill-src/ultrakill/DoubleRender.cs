using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020003F0 RID: 1008
public class DoubleRender : MonoBehaviour
{
	// Token: 0x060016A4 RID: 5796 RVA: 0x000B5DA8 File Offset: 0x000B3FA8
	private void Awake()
	{
		this.cc = MonoSingleton<CameraController>.Instance;
		this.currentCam = this.cc.cam;
		this.radiantMat = new Material(MonoSingleton<PostProcessV2_Handler>.Instance.radiantBuff);
		this.thisRend = base.GetComponent<Renderer>();
		this.cb = new CommandBuffer
		{
			name = "BuffRender"
		};
		Mesh mesh = null;
		if (this.thisRend is SkinnedMeshRenderer)
		{
			mesh = (this.thisRend as SkinnedMeshRenderer).sharedMesh;
		}
		else if (this.thisRend is MeshRenderer)
		{
			mesh = this.thisRend.GetComponent<MeshFilter>().sharedMesh;
		}
		if (mesh != null)
		{
			for (int i = 0; i < mesh.subMeshCount; i++)
			{
				if (!this.subMeshesToIgnore.Contains(i))
				{
					this.cb.DrawRenderer(this.thisRend, this.radiantMat, i);
				}
			}
		}
		this.radiantMat.SetFloat("_ForceOutline", 1f);
		PostProcessV2_Handler instance = MonoSingleton<PostProcessV2_Handler>.Instance;
		instance.onReinitialize = (Action<bool>)Delegate.Combine(instance.onReinitialize, new Action<bool>(this.Reinitialize));
	}

	// Token: 0x060016A5 RID: 5797 RVA: 0x000B5EC4 File Offset: 0x000B40C4
	public void Reinitialize(bool forceReinitialize = false)
	{
		if (!this.thisRend.enabled && (forceReinitialize || this.isActive))
		{
			this.currentCam.RemoveCommandBuffer(CameraEvent.BeforeForwardAlpha, this.cb);
			this.isActive = false;
		}
		if (this.thisRend.enabled && (forceReinitialize || !this.isActive))
		{
			this.currentCam.AddCommandBuffer(CameraEvent.BeforeForwardAlpha, this.cb);
			this.isActive = true;
		}
		this.radiantMat.SetFloat("_Outline", (float)this.shouldOutline);
		if (this.currentCam != this.cc.cam)
		{
			this.currentCam.RemoveCommandBuffer(CameraEvent.BeforeForwardAlpha, this.cb);
			this.currentCam = this.cc.cam;
			this.currentCam.AddCommandBuffer(CameraEvent.BeforeForwardAlpha, this.cb);
		}
	}

	// Token: 0x060016A6 RID: 5798 RVA: 0x000B5F9B File Offset: 0x000B419B
	private void LateUpdate()
	{
		this.Reinitialize(false);
	}

	// Token: 0x060016A7 RID: 5799 RVA: 0x000B5FA4 File Offset: 0x000B41A4
	public void OnDisable()
	{
		if (this.currentCam != null)
		{
			this.currentCam.RemoveCommandBuffer(CameraEvent.BeforeForwardAlpha, this.cb);
		}
		this.isActive = false;
		if (MonoSingleton<PostProcessV2_Handler>.Instance != null)
		{
			PostProcessV2_Handler instance = MonoSingleton<PostProcessV2_Handler>.Instance;
			instance.onReinitialize = (Action<bool>)Delegate.Remove(instance.onReinitialize, new Action<bool>(this.Reinitialize));
		}
	}

	// Token: 0x060016A8 RID: 5800 RVA: 0x000B600C File Offset: 0x000B420C
	public void RemoveEffect()
	{
		this.currentCam.RemoveCommandBuffer(CameraEvent.BeforeForwardAlpha, this.cb);
		if (MonoSingleton<PostProcessV2_Handler>.Instance != null)
		{
			PostProcessV2_Handler instance = MonoSingleton<PostProcessV2_Handler>.Instance;
			instance.onReinitialize = (Action<bool>)Delegate.Remove(instance.onReinitialize, new Action<bool>(this.Reinitialize));
		}
		Object.Destroy(this);
		this.isActive = false;
	}

	// Token: 0x060016A9 RID: 5801 RVA: 0x000B606C File Offset: 0x000B426C
	public void SetOutline(int showOultine)
	{
		this.shouldOutline = showOultine;
	}

	// Token: 0x04001F68 RID: 8040
	public List<int> subMeshesToIgnore = new List<int>();

	// Token: 0x04001F69 RID: 8041
	public Material radiantMat;

	// Token: 0x04001F6A RID: 8042
	public Renderer thisRend;

	// Token: 0x04001F6B RID: 8043
	private CommandBuffer cb;

	// Token: 0x04001F6C RID: 8044
	private CameraController cc;

	// Token: 0x04001F6D RID: 8045
	private Camera currentCam;

	// Token: 0x04001F6E RID: 8046
	public int shouldOutline;

	// Token: 0x04001F6F RID: 8047
	private bool isActive;
}
