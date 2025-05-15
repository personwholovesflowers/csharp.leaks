using System;
using System.Collections.Generic;
using System.Reflection;
using NewBlood.Interop;
using SettingsMenu.Components.Pages;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Rendering;

// Token: 0x020004DB RID: 1243
[ExecuteInEditMode]
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class StaticSceneOptimizer : MonoSingleton<StaticSceneOptimizer>
{
	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x06001C90 RID: 7312 RVA: 0x000EFE8D File Offset: 0x000EE08D
	[HideInInspector]
	[SerializeField]
	private bool bakeCompleted
	{
		get
		{
			return !this.nothingBaked;
		}
	}

	// Token: 0x06001C91 RID: 7313 RVA: 0x000EFE98 File Offset: 0x000EE098
	public static void SetStaticBatchInfo(Renderer renderer, int firstSubMesh, int subMeshCount)
	{
		StaticSceneOptimizer.s_SetStaticBatchInfo(renderer, firstSubMesh, subMeshCount);
	}

	// Token: 0x06001C92 RID: 7314 RVA: 0x000EFEA8 File Offset: 0x000EE0A8
	private void SetupMaterial(bool isBaking = false)
	{
		if (!this.usedComputeShadersAtStart)
		{
			this.batchMaterialOutdoors.EnableKeyword("NO_COMPUTE");
			this.batchMaterialEnvironment.EnableKeyword("NO_COMPUTE");
		}
		if (isBaking || this.bakedDataAsset == null)
		{
			return;
		}
		if (this.bakedDataAsset.mainTexAtlas != null)
		{
			this.batchMaterialOutdoors.SetTexture("_MainTex", this.bakedDataAsset.mainTexAtlas);
			this.batchMaterialEnvironment.SetTexture("_MainTex", this.bakedDataAsset.mainTexAtlas);
		}
		if (this.bakedDataAsset.blendTexAtlas != null)
		{
			this.batchMaterialOutdoors.SetTexture("_BlendTex", this.bakedDataAsset.blendTexAtlas);
			this.batchMaterialEnvironment.SetTexture("_BlendTex", this.bakedDataAsset.blendTexAtlas);
			return;
		}
		Debug.LogWarning("No atlas texture exists for the scene!");
	}

	// Token: 0x06001C93 RID: 7315 RVA: 0x000EFF8C File Offset: 0x000EE18C
	private void Start()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		this.usedComputeShadersAtStart = !SettingsMenu.Components.Pages.GraphicsSettings.disabledComputeShaders;
		this.FixPosition();
		this.SetupMaterial(false);
		this.SetupMeshes();
		if (this.usedComputeShadersAtStart)
		{
			this.SetGlobalBufferData();
		}
	}

	// Token: 0x06001C94 RID: 7316 RVA: 0x000EFFC5 File Offset: 0x000EE1C5
	private void FixPosition()
	{
		base.transform.SetParent(null);
		base.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
		base.transform.localScale = Vector3.one;
	}

	// Token: 0x06001C95 RID: 7317 RVA: 0x000EFFF8 File Offset: 0x000EE1F8
	private void SetGlobalBufferData()
	{
		if (this.nothingBaked)
		{
			base.enabled = false;
			return;
		}
		this.globalLightData = new StaticSceneOptimizer.LightData[this.globalLights.Count];
		this.cbGlobalLightsData = new ComputeBuffer(this.globalLightData.Length, 64);
		this.cbGlobalLightsData.SetData(this.globalLightData);
		Shader.SetGlobalBuffer("_GlobalLightData", this.cbGlobalLightsData);
		this.cbMRLightIndices = new ComputeBuffer(this.bakedDataAsset.mrLightIndices.Count, 4);
		this.cbMRLightIndices.SetData<int>(this.bakedDataAsset.mrLightIndices);
		Shader.SetGlobalBuffer("_GlobalLightIndices", this.cbMRLightIndices);
		int num = this.globalLightData.Length;
		for (int i = 0; i < num; i++)
		{
			StaticSceneOptimizer.LightData lightData = this.globalLightData[i];
			Light light = this.globalLights[i];
			if (light == null)
			{
				this.globalLightData[i].lightColor = this.disabledLight;
			}
			else
			{
				LightType type = light.type;
				bool flag = type == LightType.Directional;
				bool flag2 = type == LightType.Spot;
				float num2 = light.spotAngle * 0.017453292f;
				float num3 = (flag2 ? Mathf.Cos(num2 / 2f) : (-1f));
				float num4 = (flag2 ? (1f / (Mathf.Cos(num2 / 4f) - Mathf.Cos(num2 / 2f))) : 1f);
				float num5 = (flag ? 0f : light.range);
				lightData.lightAtten = new Vector3(num3, num4, num5);
				Transform transform = light.transform;
				Vector3 vector = -transform.forward;
				lightData.lightPosition = (flag ? vector : transform.position);
				lightData.lightDir = vector;
				light.cullingMask &= ~((1 << this.enviroBakedLayer) | (1 << this.outdoorBakedLayer));
				Color color = light.color;
				lightData.lightColor = color * light.intensity;
				this.globalLightData[i] = lightData;
			}
		}
		this.cbGlobalLightsData.SetData(this.globalLightData);
		Shader.SetGlobalBuffer("_AllLightBuffer", this.cbGlobalLightsData);
	}

	// Token: 0x06001C96 RID: 7318 RVA: 0x000F023C File Offset: 0x000EE43C
	private void SetupMeshes()
	{
		for (int i = 0; i < this.staticMRends.Count; i++)
		{
			MeshRenderer meshRenderer = this.staticMRends[i];
			if (!(meshRenderer == null))
			{
				ProBuilderMesh proBuilderMesh;
				if (meshRenderer.TryGetComponent<ProBuilderMesh>(out proBuilderMesh))
				{
					Object.Destroy(proBuilderMesh);
				}
				Bounds bounds = meshRenderer.bounds;
				ushort num = this.bakedDataAsset.mrMeshIndices[i];
				meshRenderer.GetComponent<MeshFilter>().sharedMesh = this.bakedDataAsset.bakedMeshes[(int)num];
				int num2 = (int)this.bakedDataAsset.firstSubMesh[i];
				int num3 = (int)this.bakedDataAsset.subMeshCount[i];
				StaticSceneOptimizer.SetStaticBatchInfo(meshRenderer, num2, num3);
				GameObject gameObject = meshRenderer.gameObject;
				int layer = gameObject.layer;
				meshRenderer.GetMaterials(this.reusableMaterials);
				for (int j = 0; j < this.reusableMaterials.Count; j++)
				{
					this.reusableMaterials[j] = ((layer == 24) ? this.batchMaterialOutdoors : this.batchMaterialEnvironment);
				}
				meshRenderer.SetSharedMaterials(this.reusableMaterials);
				if (this.usedComputeShadersAtStart)
				{
					if (layer == this.enviroLayer)
					{
						gameObject.layer = this.enviroBakedLayer;
					}
					if (layer == this.outdoorLayer)
					{
						gameObject.layer = this.outdoorBakedLayer;
					}
				}
				meshRenderer.bounds = bounds;
			}
		}
	}

	// Token: 0x06001C97 RID: 7319 RVA: 0x000F0398 File Offset: 0x000EE598
	private unsafe StaticBatchInfo ApplyStaticBatchingInfo(int i, Renderer destination)
	{
		StaticBatchInfo staticBatchInfo = default(StaticBatchInfo);
		staticBatchInfo.firstSubMesh = this.bakedDataAsset.firstSubMesh[i];
		staticBatchInfo.subMeshCount = this.bakedDataAsset.subMeshCount[i];
		destination.GetNativeObject()->__BaseRenderer.m_RendererData.m_StaticBatchInfo = staticBatchInfo;
		return staticBatchInfo;
	}

	// Token: 0x06001C98 RID: 7320 RVA: 0x00004AE3 File Offset: 0x00002CE3
	private void Update()
	{
	}

	// Token: 0x06001C99 RID: 7321 RVA: 0x000F03F4 File Offset: 0x000EE5F4
	private void LateUpdate()
	{
		if (Application.isPlaying && this.usedComputeShadersAtStart)
		{
			this.UpdateLightBuffer();
		}
	}

	// Token: 0x06001C9A RID: 7322 RVA: 0x000F040B File Offset: 0x000EE60B
	public void UpdateRain(bool doEnable)
	{
		if (doEnable)
		{
			this.batchMaterialOutdoors.EnableKeyword("RAIN");
			return;
		}
		this.batchMaterialOutdoors.DisableKeyword("RAIN");
	}

	// Token: 0x06001C9B RID: 7323 RVA: 0x000F0434 File Offset: 0x000EE634
	private void UpdateLightBuffer()
	{
		int num = this.globalLightData.Length;
		for (int i = 0; i < num; i++)
		{
			Light light = this.globalLights[i];
			if (light == null || !light.isActiveAndEnabled)
			{
				this.globalLightData[i].lightColor = this.disabledLight;
			}
			else
			{
				StaticSceneOptimizer.LightData lightData = this.globalLightData[i];
				Color color = light.color;
				Transform transform = light.transform;
				Vector3 vector = -transform.forward;
				bool flag = light.type == LightType.Directional;
				lightData.lightDir = vector;
				lightData.lightPosition = (flag ? vector : transform.position);
				lightData.lightColor = color * light.intensity;
				this.globalLightData[i] = lightData;
			}
		}
		this.cbGlobalLightsData.SetData(this.globalLightData);
		Shader.SetGlobalBuffer("_AllLightBuffer", this.cbGlobalLightsData);
	}

	// Token: 0x06001C9C RID: 7324 RVA: 0x000F053C File Offset: 0x000EE73C
	public void GetSurfaceType(MeshRenderer mRend)
	{
		int num = this.staticMRends.IndexOf(mRend);
		Debug.Log(string.Format("found {0} at index {1}", mRend, num));
	}

	// Token: 0x06001C9D RID: 7325 RVA: 0x000F056C File Offset: 0x000EE76C
	private new void OnDestroy()
	{
		if (this.cbGlobalLightsData != null)
		{
			this.cbMRLightIndices.Release();
		}
		if (this.cbGlobalLightsData != null)
		{
			this.cbGlobalLightsData.Release();
		}
	}

	// Token: 0x04002865 RID: 10341
	public StaticSceneOptimizer.BakingMode bakeMode;

	// Token: 0x04002866 RID: 10342
	[Space(10f)]
	[SerializeField]
	private StaticSceneData bakedDataAsset;

	// Token: 0x04002867 RID: 10343
	[SerializeField]
	private List<Texture2D> devTexturesToSpot = new List<Texture2D>();

	// Token: 0x04002868 RID: 10344
	[SerializeField]
	public List<Type> ignoreTypes = new List<Type>
	{
		typeof(Skull),
		typeof(EnemyIdentifier),
		typeof(SpawnEffect),
		typeof(Animator),
		typeof(NewMovement),
		typeof(IgnoreSceneOptimizer),
		typeof(Torch),
		typeof(FinalDoor),
		typeof(FinalRoom),
		typeof(Joint),
		typeof(SimpleMeshCombiner),
		typeof(MovingPlatform),
		typeof(ChessPiece),
		typeof(BaitItem),
		typeof(ScrollingTexture),
		typeof(AnimatedTexture),
		typeof(BloodAbsorber)
	};

	// Token: 0x04002869 RID: 10345
	public bool warnNonStaticLights = true;

	// Token: 0x0400286A RID: 10346
	public bool warnLightInGoreZone;

	// Token: 0x0400286B RID: 10347
	public bool warnNonStaticObjects;

	// Token: 0x0400286C RID: 10348
	public bool warnWrongObjectLayers;

	// Token: 0x0400286D RID: 10349
	public bool warnNotUsingMasterShader;

	// Token: 0x0400286E RID: 10350
	public bool warnMismatchedShaderKeywords;

	// Token: 0x0400286F RID: 10351
	public bool warnMissingMeshFilter;

	// Token: 0x04002870 RID: 10352
	public bool warnMissingMesh;

	// Token: 0x04002871 RID: 10353
	public bool warnSubmeshIssues;

	// Token: 0x04002872 RID: 10354
	public bool warnOddNegativeScaling;

	// Token: 0x04002873 RID: 10355
	public bool randomColorAtlas;

	// Token: 0x04002874 RID: 10356
	[HideInInspector]
	[SerializeField]
	private StaticSceneOptimizer.BakingMode currentBakedMode;

	// Token: 0x04002875 RID: 10357
	[HideInInspector]
	[SerializeField]
	public List<Light> globalLights;

	// Token: 0x04002876 RID: 10358
	[HideInInspector]
	[SerializeField]
	public List<MeshRenderer> staticMRends = new List<MeshRenderer>();

	// Token: 0x04002877 RID: 10359
	[HideInInspector]
	[SerializeField]
	private int bakedTextureCount;

	// Token: 0x04002878 RID: 10360
	[HideInInspector]
	[SerializeField]
	private bool nothingBaked = true;

	// Token: 0x04002879 RID: 10361
	[HideInInspector]
	[SerializeField]
	private bool isDirty = true;

	// Token: 0x0400287A RID: 10362
	[HideInInspector]
	[SerializeField]
	private bool uv0Baked;

	// Token: 0x0400287B RID: 10363
	[HideInInspector]
	[SerializeField]
	private bool uv1Baked;

	// Token: 0x0400287C RID: 10364
	[HideInInspector]
	[SerializeField]
	public Material batchMaterialOutdoors;

	// Token: 0x0400287D RID: 10365
	[HideInInspector]
	[SerializeField]
	private Material batchMaterialEnvironment;

	// Token: 0x0400287E RID: 10366
	private int enviroLayer = 8;

	// Token: 0x0400287F RID: 10367
	private int enviroBakedLayer = 7;

	// Token: 0x04002880 RID: 10368
	private int outdoorLayer = 24;

	// Token: 0x04002881 RID: 10369
	private int outdoorBakedLayer = 6;

	// Token: 0x04002882 RID: 10370
	private Vector4 disabledLight = new Vector4(-1000f, -1000f, -1000f, 0f);

	// Token: 0x04002883 RID: 10371
	private List<Material> reusableMaterials = new List<Material>();

	// Token: 0x04002884 RID: 10372
	private HashSet<int> testHashes = new HashSet<int>();

	// Token: 0x04002885 RID: 10373
	private LocalKeyword[] matchKeywords;

	// Token: 0x04002886 RID: 10374
	private StaticSceneOptimizer.LightData[] globalLightData;

	// Token: 0x04002887 RID: 10375
	private ComputeBuffer cbMRLightIndices;

	// Token: 0x04002888 RID: 10376
	private ComputeBuffer cbGlobalLightsData;

	// Token: 0x04002889 RID: 10377
	private ComputeBuffer cbGlobalFullLightsData;

	// Token: 0x0400288A RID: 10378
	private RenderTexture directionalShadows;

	// Token: 0x0400288B RID: 10379
	private RenderTexture pointSpotShadows;

	// Token: 0x0400288C RID: 10380
	private List<MeshRenderer> tempMRends = new List<MeshRenderer>();

	// Token: 0x0400288D RID: 10381
	private List<Color> reusableColors = new List<Color>();

	// Token: 0x0400288E RID: 10382
	private List<Vector3> reusablePositions = new List<Vector3>();

	// Token: 0x0400288F RID: 10383
	private List<Vector3> reusableNormals = new List<Vector3>();

	// Token: 0x04002890 RID: 10384
	private string VERTEX_LIGHTING = "VERTEX_LIGHTING";

	// Token: 0x04002891 RID: 10385
	private string VERTEX_BLENDING = "VERTEX_BLENDING";

	// Token: 0x04002892 RID: 10386
	public bool usedComputeShadersAtStart = true;

	// Token: 0x04002893 RID: 10387
	private static readonly Action<Renderer, int, int> s_SetStaticBatchInfo = (Action<Renderer, int, int>)typeof(Renderer).GetMethod("SetStaticBatchInfo", BindingFlags.Instance | BindingFlags.NonPublic).CreateDelegate(typeof(Action<Renderer, int, int>));

	// Token: 0x020004DC RID: 1244
	public enum BakingMode
	{
		// Token: 0x04002895 RID: 10389
		Stationary
	}

	// Token: 0x020004DD RID: 1245
	private struct TestVertex
	{
		// Token: 0x04002896 RID: 10390
		public Vector3 position;

		// Token: 0x04002897 RID: 10391
		public Vector3 normal;

		// Token: 0x04002898 RID: 10392
		public Color32 color;

		// Token: 0x04002899 RID: 10393
		public half2 uv0;

		// Token: 0x0400289A RID: 10394
		public half4 uv1;

		// Token: 0x0400289B RID: 10395
		public half4 uv2;

		// Token: 0x0400289C RID: 10396
		public half4 uv3;

		// Token: 0x0400289D RID: 10397
		public ushort uv4X;

		// Token: 0x0400289E RID: 10398
		public ushort uv4Y;
	}

	// Token: 0x020004DE RID: 1246
	public struct LightData
	{
		// Token: 0x0400289F RID: 10399
		public Vector4 lightPosition;

		// Token: 0x040028A0 RID: 10400
		public Vector4 lightAtten;

		// Token: 0x040028A1 RID: 10401
		public Vector4 lightDir;

		// Token: 0x040028A2 RID: 10402
		public Vector4 lightColor;
	}

	// Token: 0x020004DF RID: 1247
	public struct FullLightData
	{
		// Token: 0x040028A3 RID: 10403
		public Vector4 lightPosition_shadowFormat;

		// Token: 0x040028A4 RID: 10404
		public Vector4 lightAtten_shadowIndex;

		// Token: 0x040028A5 RID: 10405
		public Vector4 lightDir_shadowStrength;

		// Token: 0x040028A6 RID: 10406
		public Vector4 lightColor;

		// Token: 0x040028A7 RID: 10407
		public Matrix4x4 viewMatrix;

		// Token: 0x040028A8 RID: 10408
		public Matrix4x4 projectionMatrix;
	}
}
