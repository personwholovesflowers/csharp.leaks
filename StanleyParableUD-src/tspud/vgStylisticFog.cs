using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020000D1 RID: 209
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
internal class vgStylisticFog : vgCommandBufferBase
{
	// Token: 0x060004D7 RID: 1239 RVA: 0x0001C370 File Offset: 0x0001A570
	public override void VerifyResources()
	{
		base.VerifyResources();
		if (this.fogMaterial == null)
		{
			this.fogMaterial = new Material(this.fogShader);
			this.fogMaterial.hideFlags = HideFlags.DontSave;
		}
		if (this.mParamIdsList.Count == 0)
		{
			for (int i = 0; i < this.kSHADER_PASSES.Length; i++)
			{
				string text = this.kSHADER_PASSES[i];
				vgStylisticFog.ShaderParamIds shaderParamIds = new vgStylisticFog.ShaderParamIds();
				shaderParamIds.Link(text);
				this.mParamIdsList.Add(shaderParamIds);
			}
		}
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x0001C3F1 File Offset: 0x0001A5F1
	protected override CameraEvent GetPassCameraEvent()
	{
		return this.cameraQueueEvent;
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x0001C3F9 File Offset: 0x0001A5F9
	protected override string GetPassCommandBufferName()
	{
		return this.commandBufferName;
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x0001C401 File Offset: 0x0001A601
	protected override int GetPassSortingIndex()
	{
		return this.orderingIndex;
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x0001C409 File Offset: 0x0001A609
	private bool CheckFogData()
	{
		return true;
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x00005444 File Offset: 0x00003644
	[ContextMenu("Clear Command Buffers")]
	public void ClearCommandBuffers()
	{
	}

	// Token: 0x060004DD RID: 1245 RVA: 0x0001C40C File Offset: 0x0001A60C
	private void SetFogShaderParameters(vgStylisticFogData data, int pass)
	{
		if (data == null)
		{
			Debug.LogError("vgStylisticFog:: Data being set from is null, no parameters have been setup.", this);
			return;
		}
		vgStylisticFog.ShaderParamIds shaderParamIds = this.mParamIdsList[pass];
		Vector3 vector = default(Vector3);
		float num = Mathf.Clamp01(data.intensityScale + this.intensityOffset);
		float num2 = data.startDistance + this.startPointOffset;
		float num3 = data.endDistance + this.endPointOffset;
		float num4 = (num2 - this.cameraNear) / (this.cameraFar - this.cameraNear);
		float num5 = (num3 - this.cameraNear) / (this.cameraFar - this.cameraNear);
		if (data.transformObjectB != null && data.transformObjectA != null)
		{
			vector = data.transformObjectB.position - data.transformObjectA.position;
			vector.Normalize();
		}
		else
		{
			vector = new Vector3(0f, 0f, 1f);
		}
		data.offsetFromAToB = Mathf.Clamp(data.offsetFromAToB, -1f, 1f);
		this.fogMaterial.SetVector(shaderParamIds.StartDistance, new Vector4(1f / num2, this.cameraScale - num2, num4, 0f));
		this.fogMaterial.SetVector(shaderParamIds.EndDistance, new Vector4(1f / num3, this.cameraScale - num3, num5, 0f));
		this.fogMaterial.SetTexture(shaderParamIds.FogColorTextureFromAToB, data.fogColorTextureFromAToB);
		this.fogMaterial.SetTexture(shaderParamIds.FogColorTextureFromBToA, data.fogColorTextureFromBToA);
		this.fogMaterial.SetVector(shaderParamIds.FromAToBNormal, vector);
		this.fogMaterial.SetFloat(shaderParamIds.FromAToBOffset, data.offsetFromAToB);
		this.fogMaterial.SetFloat(shaderParamIds.IntensityScale, num);
	}

	// Token: 0x060004DE RID: 1246 RVA: 0x0001C5E0 File Offset: 0x0001A7E0
	private void PrepareFogParameters(Camera cam)
	{
		this.VerifyResources();
		this.cameraNear = cam.nearClipPlane;
		this.cameraFar = cam.farClipPlane;
		this.cameraFov = cam.fieldOfView;
		this.cameraAspectRatio = cam.aspect;
		Matrix4x4 identity = Matrix4x4.identity;
		float num = this.cameraFov * 0.5f;
		Vector3 vector = cam.transform.right * this.cameraNear * Mathf.Tan(num * 0.017453292f) * this.cameraAspectRatio;
		Vector3 vector2 = cam.transform.up * this.cameraNear * Mathf.Tan(num * 0.017453292f);
		Vector3 vector3 = cam.transform.forward * this.cameraNear - vector + vector2;
		this.cameraScale = vector3.magnitude * this.cameraFar / this.cameraNear;
		vector3.Normalize();
		vector3 *= this.cameraScale;
		Vector3 vector4 = cam.transform.forward * this.cameraNear + vector + vector2;
		vector4.Normalize();
		vector4 *= this.cameraScale;
		Vector3 vector5 = cam.transform.forward * this.cameraNear + vector - vector2;
		vector5.Normalize();
		vector5 *= this.cameraScale;
		Vector3 vector6 = cam.transform.forward * this.cameraNear - vector - vector2;
		vector6.Normalize();
		vector6 *= this.cameraScale;
		identity.SetRow(0, vector3);
		identity.SetRow(1, vector4);
		identity.SetRow(2, vector5);
		identity.SetRow(3, vector6);
		vgStylisticFog.ShaderParamIds shaderParamIds = this.mParamIdsList[0];
		this.fogMaterial.SetMatrix(shaderParamIds.FrustumCornersWS, identity);
		this.fogMaterial.SetVector(shaderParamIds.CameraWS, cam.transform.position);
		this.fogMaterial.SetFloat(shaderParamIds.OneOverFarMinusNearPlane, 1f / (cam.farClipPlane - cam.nearClipPlane));
		this.SetFogShaderParameters(this._customFogData, 0);
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x0001C845 File Offset: 0x0001AA45
	protected override void RefreshCommandBufferInfo(CommandBuffer buf, Camera cam)
	{
		this.VerifyResources();
		this.PrepareFogParameters(cam);
		buf.SetRenderTarget(BuiltinRenderTextureType.CurrentActive);
		buf.DrawMesh(vgCommandBufferBase.fullScreenQuadMesh, Matrix4x4.identity, this.fogMaterial, 0, 0);
	}

	// Token: 0x040004A2 RID: 1186
	public Shader fogShader;

	// Token: 0x040004A3 RID: 1187
	private Material fogMaterial;

	// Token: 0x040004A4 RID: 1188
	private float cameraNear = 0.5f;

	// Token: 0x040004A5 RID: 1189
	private float cameraFar = 50f;

	// Token: 0x040004A6 RID: 1190
	private float cameraFov = 60f;

	// Token: 0x040004A7 RID: 1191
	private float cameraAspectRatio = 1.333333f;

	// Token: 0x040004A8 RID: 1192
	private bool interpolateBetweenTwoFogs;

	// Token: 0x040004A9 RID: 1193
	private float dualFogInterpolationValue;

	// Token: 0x040004AA RID: 1194
	private static string shaderDualFogFeatureDefineOn = "INTERPOLATE_DUAL_FOG";

	// Token: 0x040004AB RID: 1195
	private static string shaderDualFogFeatureDefineOff = "DONT_INTERPOLATE_DUAL_FOG";

	// Token: 0x040004AC RID: 1196
	private float cameraScale;

	// Token: 0x040004AD RID: 1197
	private string commandBufferName = "Stylistic Fog Effect";

	// Token: 0x040004AE RID: 1198
	private CameraEvent cameraQueueEvent = CameraEvent.BeforeSkybox;

	// Token: 0x040004AF RID: 1199
	[Tooltip("Change this to render this before or after other command buffers - doesn't change in real time")]
	public int orderingIndex = 1;

	// Token: 0x040004B0 RID: 1200
	public float intensityOffset;

	// Token: 0x040004B1 RID: 1201
	public float startPointOffset;

	// Token: 0x040004B2 RID: 1202
	public float endPointOffset;

	// Token: 0x040004B3 RID: 1203
	private string[] kSHADER_PASSES = new string[] { "_", "_Second" };

	// Token: 0x040004B4 RID: 1204
	protected List<vgStylisticFog.ShaderParamIds> mParamIdsList = new List<vgStylisticFog.ShaderParamIds>();

	// Token: 0x040004B5 RID: 1205
	public vgStylisticFogData _customFogData;

	// Token: 0x020003A8 RID: 936
	[Serializable]
	protected class ShaderParamIds
	{
		// Token: 0x17000285 RID: 645
		// (get) Token: 0x060016AD RID: 5805 RVA: 0x000775E2 File Offset: 0x000757E2
		public bool valid
		{
			get
			{
				return this._valid;
			}
		}

		// Token: 0x060016AE RID: 5806 RVA: 0x000775EC File Offset: 0x000757EC
		public void Link(string prefix)
		{
			this.StartDistance = Shader.PropertyToID(prefix + "StartDistance");
			this.EndDistance = Shader.PropertyToID(prefix + "EndDistance");
			this.FogColorTextureFromAToB = Shader.PropertyToID(prefix + "FogColorTextureFromAToB");
			this.FogColorTextureFromBToA = Shader.PropertyToID(prefix + "FogColorTextureFromBToA");
			this.FromAToBNormal = Shader.PropertyToID(prefix + "FromAToBNormal");
			this.FromAToBOffset = Shader.PropertyToID(prefix + "FromAToBOffset");
			this.IntensityScale = Shader.PropertyToID(prefix + "IntensityScale");
			this.MainTex_TexelSize = Shader.PropertyToID("_MainTex_TexelSize");
			this.DualFogInterpolationValue = Shader.PropertyToID("_DualFogInterpolationValue");
			this.FrustumCornersWS = Shader.PropertyToID("_FrustumCornersWS");
			this.OneOverFarMinusNearPlane = Shader.PropertyToID("_OneOverFarMinusNearPlane");
			this.CameraWS = Shader.PropertyToID("_CameraWS");
			this._valid = true;
		}

		// Token: 0x0400136A RID: 4970
		public int StartDistance = -1;

		// Token: 0x0400136B RID: 4971
		public int EndDistance = -1;

		// Token: 0x0400136C RID: 4972
		public int FogColorTextureFromAToB = -1;

		// Token: 0x0400136D RID: 4973
		public int FogColorTextureFromBToA = -1;

		// Token: 0x0400136E RID: 4974
		public int FromAToBNormal = -1;

		// Token: 0x0400136F RID: 4975
		public int FromAToBOffset = -1;

		// Token: 0x04001370 RID: 4976
		public int IntensityScale = -1;

		// Token: 0x04001371 RID: 4977
		public int MainTex_TexelSize;

		// Token: 0x04001372 RID: 4978
		public int DualFogInterpolationValue;

		// Token: 0x04001373 RID: 4979
		public int FrustumCornersWS;

		// Token: 0x04001374 RID: 4980
		public int OneOverFarMinusNearPlane;

		// Token: 0x04001375 RID: 4981
		public int CameraWS;

		// Token: 0x04001376 RID: 4982
		private bool _valid;
	}
}
