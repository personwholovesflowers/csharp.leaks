using System;
using UnityEngine;

// Token: 0x020004AA RID: 1194
[ExecuteInEditMode]
public class LimboSkybox : MonoBehaviour
{
	// Token: 0x06001B79 RID: 7033 RVA: 0x000E3EC0 File Offset: 0x000E20C0
	private void OnEnable()
	{
		this.fakeCam = base.GetComponent<Camera>();
		this.playerStartPos = this.playerStart.position;
		this.cc = MonoSingleton<CameraController>.Instance;
	}

	// Token: 0x06001B7A RID: 7034 RVA: 0x000E3EEA File Offset: 0x000E20EA
	private void Update()
	{
		this.UpdateCamera();
		this.InitializeRT();
	}

	// Token: 0x06001B7B RID: 7035 RVA: 0x000E3EF8 File Offset: 0x000E20F8
	private void OnRenderObject()
	{
		this.UpdateCamera();
	}

	// Token: 0x06001B7C RID: 7036 RVA: 0x000E3F00 File Offset: 0x000E2100
	private void OnPreRender()
	{
		if (!RenderSettings.fog)
		{
			return;
		}
		this.oldFogStart = RenderSettings.fogStartDistance;
		this.oldFogEnd = RenderSettings.fogEndDistance;
		this.oldFogColor = RenderSettings.fogColor;
		if (!this.useFogOverrides)
		{
			return;
		}
		if (this.overrideFogDistance)
		{
			RenderSettings.fogStartDistance = this.fogStart;
			RenderSettings.fogEndDistance = this.fogEnd;
		}
		if (this.overrideFogColor)
		{
			RenderSettings.fogColor = this.fogColor;
		}
	}

	// Token: 0x06001B7D RID: 7037 RVA: 0x000E3F70 File Offset: 0x000E2170
	private void OnPostRender()
	{
		if (!RenderSettings.fog)
		{
			return;
		}
		RenderSettings.fogStartDistance = this.oldFogStart;
		RenderSettings.fogEndDistance = this.oldFogEnd;
		RenderSettings.fogColor = this.oldFogColor;
	}

	// Token: 0x06001B7E RID: 7038 RVA: 0x000E3F9C File Offset: 0x000E219C
	private void UpdateCamera()
	{
		if (Application.isPlaying)
		{
			this.playerCam = this.cc.cam;
		}
		if (this.playerCam == null)
		{
			return;
		}
		Vector3 vector = (this.playerCam.transform.position - this.playerStartPos) / 16f;
		float num = (this.lockMinimumHeight ? Mathf.Max(vector.y, 0f) : vector.y);
		this.fakeCam.transform.position = this.fakeCamStart.position + new Vector3(vector.x, num, vector.z);
		this.fakeCam.transform.rotation = this.playerCam.transform.rotation;
		this.fakeCam.cullingMask = this.playerCam.cullingMask;
		this.fakeCam.fieldOfView = this.playerCam.fieldOfView;
	}

	// Token: 0x06001B7F RID: 7039 RVA: 0x000E4098 File Offset: 0x000E2298
	private void InitializeRT()
	{
		int num = (int)((float)Screen.width / this.downscaleFactor);
		int num2 = (int)((float)Screen.height / this.downscaleFactor);
		if (this.lastWidth != num || this.lastHeight != num2)
		{
			if (this.skybox)
			{
				this.fakeCam.targetTexture = null;
				this.skybox.Release();
				if (Application.isPlaying)
				{
					Object.Destroy(this.skybox);
				}
			}
			this.lastWidth = num;
			this.lastHeight = num2;
			this.skybox = new RenderTexture(this.lastWidth, this.lastHeight, 24, RenderTextureFormat.ARGB32);
			this.fakeCam.targetTexture = this.skybox;
			Shader.SetGlobalTexture("_LimboSky", this.skybox);
			Shader.SetGlobalFloat("_LimboSkyWidth", (float)num);
			Shader.SetGlobalFloat("_LimboSkyHeight", (float)num2);
		}
	}

	// Token: 0x040026B1 RID: 9905
	public bool lockMinimumHeight = true;

	// Token: 0x040026B2 RID: 9906
	public float downscaleFactor = 2f;

	// Token: 0x040026B3 RID: 9907
	public Transform playerStart;

	// Token: 0x040026B4 RID: 9908
	public Transform fakeCamStart;

	// Token: 0x040026B5 RID: 9909
	private RenderTexture skybox;

	// Token: 0x040026B6 RID: 9910
	private Camera fakeCam;

	// Token: 0x040026B7 RID: 9911
	private int lastWidth;

	// Token: 0x040026B8 RID: 9912
	private int lastHeight;

	// Token: 0x040026B9 RID: 9913
	private CameraController cc;

	// Token: 0x040026BA RID: 9914
	private Camera playerCam;

	// Token: 0x040026BB RID: 9915
	private Vector3 playerStartPos;

	// Token: 0x040026BC RID: 9916
	[Header("Fog Settings")]
	public bool useFogOverrides;

	// Token: 0x040026BD RID: 9917
	public bool overrideFogDistance = true;

	// Token: 0x040026BE RID: 9918
	public float fogStart;

	// Token: 0x040026BF RID: 9919
	public float fogEnd;

	// Token: 0x040026C0 RID: 9920
	public bool overrideFogColor = true;

	// Token: 0x040026C1 RID: 9921
	public Color fogColor = Color.black;

	// Token: 0x040026C2 RID: 9922
	private float oldFogStart;

	// Token: 0x040026C3 RID: 9923
	private float oldFogEnd;

	// Token: 0x040026C4 RID: 9924
	private Color oldFogColor;
}
