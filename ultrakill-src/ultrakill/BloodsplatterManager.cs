using System;
using System.Collections;
using System.Collections.Generic;
using SettingsMenu.Components.Pages;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

// Token: 0x02000083 RID: 131
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class BloodsplatterManager : MonoSingleton<BloodsplatterManager>
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000266 RID: 614 RVA: 0x0000DB64 File Offset: 0x0000BD64
	// (remove) Token: 0x06000267 RID: 615 RVA: 0x0000DB9C File Offset: 0x0000BD9C
	public event Action<int> reuseParentIndex;

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000268 RID: 616 RVA: 0x0000DBD4 File Offset: 0x0000BDD4
	// (remove) Token: 0x06000269 RID: 617 RVA: 0x0000DC0C File Offset: 0x0000BE0C
	public event Action<int> reuseStainIndex;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x0600026A RID: 618 RVA: 0x0000DC44 File Offset: 0x0000BE44
	// (remove) Token: 0x0600026B RID: 619 RVA: 0x0000DC7C File Offset: 0x0000BE7C
	public event Action StainsCleared;

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x0600026C RID: 620 RVA: 0x0000DCB4 File Offset: 0x0000BEB4
	// (remove) Token: 0x0600026D RID: 621 RVA: 0x0000DCEC File Offset: 0x0000BEEC
	public event Action<float> ParticleCollisionStep;

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x0600026E RID: 622 RVA: 0x0000DD24 File Offset: 0x0000BF24
	// (remove) Token: 0x0600026F RID: 623 RVA: 0x0000DD5C File Offset: 0x0000BF5C
	public event Action PostCollisionStep;

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x06000270 RID: 624 RVA: 0x0000DD91 File Offset: 0x0000BF91
	public bool goreOn
	{
		get
		{
			return this.forceOn || this.forceGibs || MonoSingleton<PrefsManager>.Instance.GetBoolLocal("bloodEnabled", false);
		}
	}

	// Token: 0x06000271 RID: 625 RVA: 0x0000DDB5 File Offset: 0x0000BFB5
	public void SaveBloodstains()
	{
		this.checkpointPropIndex = this.propIndex;
		this.checkpointProps.CopyFrom(this.props);
	}

	// Token: 0x06000272 RID: 626 RVA: 0x0000DDD4 File Offset: 0x0000BFD4
	public void LoadBloodstains()
	{
		this.props.CopyFrom(this.checkpointProps);
		this.propIndex = this.checkpointPropIndex;
		if (this.usedComputeShadersAtStart)
		{
			this.instanceBuffer.SetData<BloodsplatterManager.InstanceProperties>(this.props);
			return;
		}
		this.meshDirty = true;
	}

	// Token: 0x06000273 RID: 627 RVA: 0x0000DE14 File Offset: 0x0000C014
	public int CreateBloodstain(Vector3 pos, Vector3 norm, bool clipToSurface, int parent = 0)
	{
		this.propIndex = (this.propIndex + 1) % this.props.Length;
		this.currentBloodCount = math.min(this.currentBloodCount + 1, this.props.Length);
		this.props[this.propIndex] = new BloodsplatterManager.InstanceProperties
		{
			pos = pos,
			norm = -norm,
			parentIndex = parent,
			clipToSurface = (clipToSurface ? 1 : 0)
		};
		Action<int> action = this.reuseStainIndex;
		if (action != null)
		{
			action(this.propIndex);
		}
		if (this.usedComputeShadersAtStart)
		{
			this.instanceBuffer.SetData<BloodsplatterManager.InstanceProperties>(this.props, this.propIndex, this.propIndex, 1);
		}
		else
		{
			this.meshDirty = true;
		}
		return this.propIndex;
	}

	// Token: 0x06000274 RID: 628 RVA: 0x0000DEF8 File Offset: 0x0000C0F8
	public void DeleteBloodstain(int index)
	{
		this.props[index] = default(BloodsplatterManager.InstanceProperties);
		Action<int> action = this.reuseStainIndex;
		if (action != null)
		{
			action(index);
		}
		if (this.usedComputeShadersAtStart)
		{
			this.instanceBuffer.SetData<BloodsplatterManager.InstanceProperties>(this.props, index, index, 1);
			return;
		}
		this.meshDirty = true;
	}

	// Token: 0x06000275 RID: 629 RVA: 0x0000DF50 File Offset: 0x0000C150
	public int CreateParent(Matrix4x4 initialMatrix)
	{
		int num = this.parentIndex;
		this.parentIndex = num + 1;
		int num2 = num;
		if (num2 >= this.parents.Length)
		{
			num2 = (this.parentIndex = 1);
		}
		Action<int> action = this.reuseParentIndex;
		if (action != null)
		{
			action(num2);
		}
		this.parents[num2] = initialMatrix;
		return num2;
	}

	// Token: 0x06000276 RID: 630 RVA: 0x0000DFA8 File Offset: 0x0000C1A8
	public float GetBloodstainChance()
	{
		if (this.overrideBloodstainChance)
		{
			return this.bloodstainChance;
		}
		return this.opm.bloodstainChance;
	}

	// Token: 0x06000277 RID: 631 RVA: 0x0000DFC4 File Offset: 0x0000C1C4
	private void Start()
	{
		this.totalStainMesh = new Mesh();
		this.totalStainMesh.MarkDynamic();
		this.usedComputeShadersAtStart = !SettingsMenu.Components.Pages.GraphicsSettings.disabledComputeShaders;
		Shader.SetGlobalFloat("_StainWarping", (float)MonoSingleton<PrefsManager>.Instance.GetInt("vertexWarping", 0));
		this.bloodCompositeMaterial = new Material(this.bloodCompositeShader);
		this.props = new NativeArray<BloodsplatterManager.InstanceProperties>((int)MonoSingleton<PrefsManager>.Instance.GetFloatLocal("bloodStainMax", 100000f), Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.checkpointProps = new NativeArray<BloodsplatterManager.InstanceProperties>(this.props.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.parents = new NativeArray<Matrix4x4>(1024, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this.parents[0] = Matrix4x4.identity;
		if (this.usedComputeShadersAtStart)
		{
			this.instanceBuffer = new ComputeBuffer(this.props.Length, 32, ComputeBufferType.Structured);
			this.instanceBuffer.SetData<BloodsplatterManager.InstanceProperties>(this.props);
			this.argsData[0] = this.stainMesh.GetIndexCount(0);
			this.argsData[1] = (uint)this.props.Length;
			this.argsData[2] = this.stainMesh.GetIndexStart(0);
			this.argsData[3] = this.stainMesh.GetBaseVertex(0);
			this.argsData[4] = 0U;
			this.argsBuffer = new ComputeBuffer(1, this.argsData.Length * 4, ComputeBufferType.DrawIndirect);
			this.argsBuffer.SetData(this.argsData);
			this.parentBuffer = new ComputeBuffer(this.parents.Length, 64, ComputeBufferType.Structured);
		}
		this.goreStore = base.transform.GetChild(0);
		float num = 0f;
		foreach (object obj in Enum.GetValues(typeof(BSType)))
		{
			BSType bstype = (BSType)obj;
			if (bstype != BSType.dontpool && bstype != BSType.unknown)
			{
				this.gorePool.Add(bstype, new Queue<GameObject>());
				num += 1f;
			}
		}
		this.opm = MonoSingleton<OptionsManager>.Instance;
		this.generateBloodMeshJob = new GenerateBloodMeshJob
		{
			props = this.props
		};
		this.InitPools();
	}

	// Token: 0x06000278 RID: 632 RVA: 0x0000E20C File Offset: 0x0000C40C
	private void Update()
	{
		Shader.SetGlobalFloat("_NormalForgiveness", this.normalForgiveness);
		if (this.sinceLastStep >= 0.128f)
		{
			this.sinceLastStep = 0f;
			Action<float> particleCollisionStep = this.ParticleCollisionStep;
			if (particleCollisionStep != null)
			{
				particleCollisionStep(0.128f);
			}
			Action postCollisionStep = this.PostCollisionStep;
			if (postCollisionStep != null)
			{
				postCollisionStep();
			}
		}
		if (this.goreOn && !this.usedComputeShadersAtStart && this.meshDirty)
		{
			this.totalStainMesh.Clear();
			VertexAttributeDescriptor vertexAttributeDescriptor = new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 4, 0);
			VertexAttributeDescriptor vertexAttributeDescriptor2 = new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3, 0);
			VertexAttributeDescriptor vertexAttributeDescriptor3 = new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2, 0);
			VertexAttributeDescriptor vertexAttributeDescriptor4 = new VertexAttributeDescriptor(VertexAttribute.TexCoord1, VertexAttributeFormat.Float32, 2, 0);
			VertexAttributeDescriptor vertexAttributeDescriptor5 = new VertexAttributeDescriptor(VertexAttribute.TexCoord2, VertexAttributeFormat.Float32, 3, 0);
			VertexAttributeDescriptor vertexAttributeDescriptor6 = new VertexAttributeDescriptor(VertexAttribute.TexCoord3, VertexAttributeFormat.Float32, 3, 0);
			this.meshDataArray = Mesh.AllocateWritableMeshData(1);
			Mesh.MeshData meshData = this.meshDataArray[0];
			int num = 4 * this.currentBloodCount;
			int num2 = 6 * this.currentBloodCount;
			meshData.SetVertexBufferParams(num, new VertexAttributeDescriptor[] { vertexAttributeDescriptor, vertexAttributeDescriptor2, vertexAttributeDescriptor3, vertexAttributeDescriptor4, vertexAttributeDescriptor5, vertexAttributeDescriptor6 });
			meshData.SetIndexBufferParams(num2, IndexFormat.UInt32);
			this.generateBloodMeshJob.meshData = meshData;
			this.generateBloodMeshJobHandle = this.generateBloodMeshJob.Schedule(this.currentBloodCount, 256, default(JobHandle));
		}
	}

	// Token: 0x06000279 RID: 633 RVA: 0x0000E38C File Offset: 0x0000C58C
	private void LateUpdate()
	{
		if (this.usedComputeShadersAtStart)
		{
			this.parentBuffer.SetData<Matrix4x4>(this.parents);
			this.argsData[1] = (uint)this.currentBloodCount;
			this.argsBuffer.SetData(this.argsData);
			return;
		}
		this.generateBloodMeshJobHandle.Complete();
		if (this.meshDirty && this.meshDataArray.Length == 1)
		{
			this.meshDirty = false;
			Mesh.ApplyAndDisposeWritableMeshData(this.meshDataArray, this.totalStainMesh, MeshUpdateFlags.Default);
			this.totalStainMesh.subMeshCount = 1;
			this.totalStainMesh.SetSubMesh(0, new SubMeshDescriptor(0, 6 * this.currentBloodCount, MeshTopology.Triangles), MeshUpdateFlags.Default);
			this.totalStainMesh.RecalculateBounds();
		}
	}

	// Token: 0x0600027A RID: 634 RVA: 0x0000E43F File Offset: 0x0000C63F
	public void ClearStains()
	{
		this.propIndex = 0;
		Action stainsCleared = this.StainsCleared;
		if (stainsCleared == null)
		{
			return;
		}
		stainsCleared();
	}

	// Token: 0x0600027B RID: 635 RVA: 0x0000E458 File Offset: 0x0000C658
	public void SetupBloodCommandBuffer(Camera mainCam, RenderTexture mainTex, RenderTexture bloodCopy, RenderTexture depth, RenderTexture viewNormal)
	{
		if (this.cb == null)
		{
			this.cb = new CommandBuffer();
			this.cb.name = "Bloodstain";
		}
		this.stainMat.SetTexture("_DepthBuffer", depth);
		this.stainMat.SetTexture("_ViewNormal", viewNormal);
		if (this.usedComputeShadersAtStart)
		{
			this.stainMat.SetBuffer("instanceBuffer", this.instanceBuffer);
			this.stainMat.SetBuffer("parentBuffer", this.parentBuffer);
		}
		this.cb.Clear();
		this.cb.SetRenderTarget(bloodCopy.colorBuffer);
		this.cb.ClearRenderTarget(false, true, Color.clear);
		if (this.usedComputeShadersAtStart)
		{
			this.cb.DrawMeshInstancedIndirect(this.stainMesh, 0, this.stainMat, 0, this.argsBuffer, 0, null);
		}
		else
		{
			this.cb.DrawMesh(this.totalStainMesh, float4x4.identity, this.stainMat);
		}
		this.cb.Blit(bloodCopy.colorBuffer, mainTex.colorBuffer, this.bloodCompositeMaterial);
		mainCam.AddCommandBuffer(CameraEvent.BeforeForwardAlpha, this.cb);
	}

	// Token: 0x0600027C RID: 636 RVA: 0x0000E594 File Offset: 0x0000C794
	protected override void OnEnable()
	{
		base.OnEnable();
	}

	// Token: 0x0600027D RID: 637 RVA: 0x0000E59C File Offset: 0x0000C79C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		NativeArray<BloodsplatterManager.InstanceProperties> nativeArray = this.checkpointProps;
		if (this.checkpointProps.IsCreated)
		{
			this.checkpointProps.Dispose();
		}
		NativeArray<BloodsplatterManager.InstanceProperties> nativeArray2 = this.props;
		if (this.props.IsCreated)
		{
			this.props.Dispose();
		}
		NativeArray<Matrix4x4> nativeArray3 = this.parents;
		if (this.parents.IsCreated)
		{
			this.parents.Dispose();
		}
		if (this.instanceBuffer != null)
		{
			this.instanceBuffer.Release();
		}
		if (this.parentBuffer != null)
		{
			this.parentBuffer.Release();
		}
	}

	// Token: 0x0600027E RID: 638 RVA: 0x0000E634 File Offset: 0x0000C834
	private GameObject GetPrefabByBSType(BSType bloodType)
	{
		switch (bloodType)
		{
		case BSType.head:
			return this.head;
		case BSType.limb:
			return this.limb;
		case BSType.body:
			return this.body;
		case BSType.small:
			return this.small;
		case BSType.smallest:
			return this.smallest;
		case BSType.splatter:
			return this.splatter;
		case BSType.underwater:
			return this.underwater;
		case BSType.sand:
			return this.sand;
		case BSType.blessing:
			return this.blessing;
		case BSType.brainChunk:
			return this.brainChunk;
		case BSType.skullChunk:
			return this.skullChunk;
		case BSType.eyeball:
			return this.eyeball;
		case BSType.jawChunk:
			return this.jawChunk;
		case BSType.gib:
			return this.gib[global::UnityEngine.Random.Range(0, this.gib.Length)];
		case BSType.chestExplosion:
			return this.chestExplosion;
		default:
			return null;
		}
	}

	// Token: 0x0600027F RID: 639 RVA: 0x0000E700 File Offset: 0x0000C900
	private void InitPools()
	{
		this.InitPool(BSType.head);
		this.InitPool(BSType.limb);
		this.InitPool(BSType.body);
		this.InitPool(BSType.small);
		this.InitPool(BSType.splatter);
		this.InitPool(BSType.underwater);
		this.InitPool(BSType.smallest);
		this.InitPool(BSType.sand);
		this.InitPool(BSType.blessing);
		this.InitPool(BSType.brainChunk);
		this.InitPool(BSType.skullChunk);
		this.InitPool(BSType.eyeball);
		this.InitPool(BSType.jawChunk);
		this.InitPool(BSType.gib);
		this.InitPool(BSType.chestExplosion);
	}

	// Token: 0x06000280 RID: 640 RVA: 0x0000E77C File Offset: 0x0000C97C
	private void InitPool(BSType bloodSplatterType)
	{
		base.StartCoroutine(this.AsyncInit(bloodSplatterType));
	}

	// Token: 0x06000281 RID: 641 RVA: 0x0000E78C File Offset: 0x0000C98C
	private IEnumerator AsyncInit(BSType bloodSplatterType)
	{
		Queue<GameObject> queue = this.gorePool[bloodSplatterType];
		GameObject prefabByBSType = this.GetPrefabByBSType(bloodSplatterType);
		Bloodsplatter bloodsplatter;
		if (prefabByBSType.TryGetComponent<Bloodsplatter>(out bloodsplatter))
		{
			this.defaultHPValues.Add(bloodSplatterType, bloodsplatter.hpAmount);
			bloodsplatter.bsm = this;
			prefabByBSType.SetActive(false);
		}
		int amount = ((bloodSplatterType == BSType.body) ? 200 : 100);
		if (bloodSplatterType == BSType.gib || bloodSplatterType == BSType.brainChunk || bloodSplatterType == BSType.skullChunk || bloodSplatterType == BSType.eyeball || bloodSplatterType == BSType.jawChunk)
		{
			amount = 200;
		}
		AsyncInstantiateOperation<GameObject> asyncOp = Object.InstantiateAsync<GameObject>(prefabByBSType, amount, this.goreStore);
		while (!asyncOp.isDone)
		{
			yield return null;
		}
		GameObject[] result = asyncOp.Result;
		for (int i = 0; i < amount; i++)
		{
			GameObject gameObject = result[i];
			queue.Enqueue(gameObject);
		}
		yield break;
	}

	// Token: 0x06000282 RID: 642 RVA: 0x0000E7A4 File Offset: 0x0000C9A4
	public void RepoolGore(Bloodsplatter bs, BSType type)
	{
		int num;
		if (type != BSType.dontpool && this.defaultHPValues.TryGetValue(type, out num))
		{
			bs.hpAmount = num;
		}
		this.RepoolGore(bs.gameObject, type);
	}

	// Token: 0x06000283 RID: 643 RVA: 0x0000E7DA File Offset: 0x0000C9DA
	public void RepoolGore(GameObject go, BSType type)
	{
		if (!go)
		{
			return;
		}
		if (type != BSType.dontpool)
		{
			this.ReturnToQueue(go, type);
			return;
		}
		Object.Destroy(go);
	}

	// Token: 0x06000284 RID: 644 RVA: 0x0000E7FC File Offset: 0x0000C9FC
	private void ReturnToQueue(GameObject go, BSType type)
	{
		if (type == BSType.unknown || type == BSType.dontpool)
		{
			Object.Destroy(go);
		}
		go.SetActive(false);
		this.gorePool[type].Enqueue(go);
		go.transform.SetParent(this.goreStore);
		go.transform.localScale = Vector3.one;
	}

	// Token: 0x06000285 RID: 645 RVA: 0x0000E854 File Offset: 0x0000CA54
	public GameObject GetFromQueue(BSType type)
	{
		GameObject gameObject = null;
		Queue<GameObject> queue = this.gorePool[type];
		while (gameObject == null && queue.Count > 0)
		{
			gameObject = queue.Dequeue();
		}
		if (gameObject == null)
		{
			gameObject = Object.Instantiate<GameObject>(this.GetPrefabByBSType(type), this.goreStore);
		}
		if (gameObject == null)
		{
			return null;
		}
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06000286 RID: 646 RVA: 0x0000E8BA File Offset: 0x0000CABA
	public GameObject GetGore(GoreType got, EnemyIdentifier eid, bool fromExplosion = false)
	{
		return this.GetGore(got, eid.underwater, eid.sandified, eid.blessed, eid, fromExplosion);
	}

	// Token: 0x06000287 RID: 647 RVA: 0x0000E8D8 File Offset: 0x0000CAD8
	public GameObject GetGore(GoreType got, bool isUnderwater = false, bool isSandified = false, bool isBlessed = false, EnemyIdentifier eid = null, bool fromExplosion = false)
	{
		if (isBlessed)
		{
			GameObject gameObject = this.GetFromQueue(BSType.blessing);
			AudioSource component = gameObject.GetComponent<AudioSource>();
			float splatterWeight = this.GetSplatterWeight(got);
			component.pitch = 1.15f + global::UnityEngine.Random.Range(-0.15f, 0.15f);
			component.volume = splatterWeight * 0.9f + 0.1f;
			gameObject.transform.localScale *= splatterWeight * splatterWeight * 3f;
			return gameObject;
		}
		if (isSandified)
		{
			GameObject gameObject = this.GetFromQueue(BSType.sand);
			if (got == GoreType.Head)
			{
				return gameObject;
			}
			AudioSource component2 = gameObject.GetComponent<AudioSource>();
			AudioSource component3 = gameObject.transform.GetChild(0).GetComponent<AudioSource>();
			AudioSource originalAudio = this.GetOriginalAudio(got);
			if (originalAudio)
			{
				component2.clip = originalAudio.clip;
				component2.volume = originalAudio.volume - 0.35f;
				component3.volume = originalAudio.volume - 0.2f;
			}
			return gameObject;
		}
		else
		{
			switch (got)
			{
			case GoreType.Head:
			{
				GameObject gameObject;
				if (isUnderwater)
				{
					gameObject = this.GetFromQueue(BSType.underwater);
					this.PrepareGore(gameObject, -1, eid, fromExplosion);
					return gameObject;
				}
				gameObject = this.GetFromQueue(BSType.head);
				this.PrepareGore(gameObject, -1, eid, fromExplosion);
				return gameObject;
			}
			case GoreType.Limb:
			{
				GameObject gameObject;
				if (isUnderwater)
				{
					gameObject = this.GetFromQueue(BSType.underwater);
					gameObject.transform.localScale *= 0.75f;
					this.PrepareGore(gameObject, 20, eid, fromExplosion);
					AudioSource component4 = gameObject.GetComponent<AudioSource>();
					AudioSource component5 = this.limb.GetComponent<AudioSource>();
					component4.clip = component5.clip;
					component4.volume = component5.volume;
					return gameObject;
				}
				gameObject = this.GetFromQueue(BSType.limb);
				this.PrepareGore(gameObject, -1, eid, fromExplosion);
				return gameObject;
			}
			case GoreType.Body:
			{
				GameObject gameObject;
				if (isUnderwater)
				{
					gameObject = this.GetFromQueue(BSType.underwater);
					gameObject.transform.localScale *= 0.5f;
					this.PrepareGore(gameObject, 10, eid, fromExplosion);
					AudioSource component6 = gameObject.GetComponent<AudioSource>();
					AudioSource component7 = this.body.GetComponent<AudioSource>();
					component6.clip = component7.clip;
					component6.volume = component7.volume;
					return gameObject;
				}
				gameObject = this.GetFromQueue(BSType.body);
				this.PrepareGore(gameObject, -1, eid, fromExplosion);
				return gameObject;
			}
			case GoreType.Small:
			{
				GameObject gameObject;
				if (isUnderwater)
				{
					gameObject = this.GetFromQueue(BSType.underwater);
					gameObject.transform.localScale *= 0.25f;
					this.PrepareGore(gameObject, 10, eid, fromExplosion);
					AudioSource component8 = gameObject.GetComponent<AudioSource>();
					AudioSource component9 = this.small.GetComponent<AudioSource>();
					component8.clip = component9.clip;
					component8.volume = component9.volume;
					return gameObject;
				}
				gameObject = this.GetFromQueue(BSType.small);
				this.PrepareGore(gameObject, -1, eid, fromExplosion);
				return gameObject;
			}
			case GoreType.Splatter:
			{
				GameObject gameObject;
				if (isUnderwater)
				{
					gameObject = this.GetFromQueue(BSType.underwater);
					this.PrepareGore(gameObject, -1, eid, fromExplosion);
					AudioSource component10 = gameObject.GetComponent<AudioSource>();
					AudioSource component11 = this.splatter.GetComponent<AudioSource>();
					component10.clip = component11.clip;
					component10.volume = component11.volume;
					return gameObject;
				}
				gameObject = this.GetFromQueue(BSType.splatter);
				this.PrepareGore(gameObject, -1, eid, fromExplosion);
				return gameObject;
			}
			case GoreType.Smallest:
			{
				GameObject gameObject;
				if (isUnderwater)
				{
					gameObject = this.GetFromQueue(BSType.underwater);
					gameObject.transform.localScale *= 0.15f;
					this.PrepareGore(gameObject, 5, eid, fromExplosion);
					AudioSource component12 = gameObject.GetComponent<AudioSource>();
					AudioSource component13 = this.smallest.GetComponent<AudioSource>();
					component12.clip = component13.clip;
					component12.volume = component13.volume;
					return gameObject;
				}
				gameObject = this.GetFromQueue(BSType.smallest);
				this.PrepareGore(gameObject, -1, eid, fromExplosion);
				return gameObject;
			}
			default:
				return null;
			}
		}
	}

	// Token: 0x06000288 RID: 648 RVA: 0x0000EC48 File Offset: 0x0000CE48
	private void PrepareGore(GameObject gob, int healthChange = -1, EnemyIdentifier eid = null, bool fromExplosion = false)
	{
		if (healthChange < 0 && eid == null && !fromExplosion)
		{
			return;
		}
		Bloodsplatter bloodsplatter;
		if (!gob.TryGetComponent<Bloodsplatter>(out bloodsplatter))
		{
			return;
		}
		if (healthChange >= 0)
		{
			bloodsplatter.hpAmount = healthChange;
		}
		if (eid)
		{
			bloodsplatter.eid = eid;
		}
		if (fromExplosion)
		{
			bloodsplatter.fromExplosion = true;
		}
	}

	// Token: 0x06000289 RID: 649 RVA: 0x0000EC98 File Offset: 0x0000CE98
	public GameObject GetGib(BSType type)
	{
		Queue<GameObject> queue = this.gorePool[type];
		GameObject gameObject = null;
		while (queue.Count > 0 && gameObject == null)
		{
			gameObject = queue.Dequeue();
		}
		if (gameObject == null)
		{
			gameObject = Object.Instantiate<GameObject>(this.GetPrefabByBSType(type));
		}
		return gameObject;
	}

	// Token: 0x0600028A RID: 650 RVA: 0x0000ECE8 File Offset: 0x0000CEE8
	private AudioSource GetOriginalAudio(GoreType got)
	{
		switch (got)
		{
		case GoreType.Limb:
			return this.limb.GetComponent<AudioSource>();
		case GoreType.Body:
			return this.body.GetComponent<AudioSource>();
		case GoreType.Small:
			return this.small.GetComponent<AudioSource>();
		case GoreType.Smallest:
			return this.smallest.GetComponent<AudioSource>();
		}
		return null;
	}

	// Token: 0x0600028B RID: 651 RVA: 0x0000ED44 File Offset: 0x0000CF44
	private float GetSplatterWeight(GoreType got)
	{
		switch (got)
		{
		case GoreType.Limb:
			return 0.75f;
		case GoreType.Body:
			return 0.5f;
		case GoreType.Small:
			return 0.125f;
		case GoreType.Smallest:
			return 0.075f;
		}
		return 1f;
	}

	// Token: 0x040002D4 RID: 724
	public float normalForgiveness = 10f;

	// Token: 0x040002D5 RID: 725
	public bool forceOn;

	// Token: 0x040002D6 RID: 726
	public bool forceGibs;

	// Token: 0x040002D7 RID: 727
	public bool neverFreezeGibs;

	// Token: 0x040002D8 RID: 728
	public bool overrideBloodstainChance;

	// Token: 0x040002D9 RID: 729
	public float bloodstainChance;

	// Token: 0x040002DA RID: 730
	public GameObject head;

	// Token: 0x040002DB RID: 731
	public GameObject limb;

	// Token: 0x040002DC RID: 732
	public GameObject body;

	// Token: 0x040002DD RID: 733
	public GameObject small;

	// Token: 0x040002DE RID: 734
	public GameObject smallest;

	// Token: 0x040002DF RID: 735
	public GameObject splatter;

	// Token: 0x040002E0 RID: 736
	public GameObject underwater;

	// Token: 0x040002E1 RID: 737
	public GameObject sand;

	// Token: 0x040002E2 RID: 738
	public GameObject blessing;

	// Token: 0x040002E3 RID: 739
	public GameObject chestExplosion;

	// Token: 0x040002E4 RID: 740
	public GameObject brainChunk;

	// Token: 0x040002E5 RID: 741
	public GameObject skullChunk;

	// Token: 0x040002E6 RID: 742
	public GameObject eyeball;

	// Token: 0x040002E7 RID: 743
	public GameObject jawChunk;

	// Token: 0x040002E8 RID: 744
	public GameObject[] gib;

	// Token: 0x040002E9 RID: 745
	private int currentBloodCount;

	// Token: 0x040002EA RID: 746
	public GameObject bloodStain;

	// Token: 0x040002EB RID: 747
	public Shader bloodCompositeShader;

	// Token: 0x040002EC RID: 748
	private Material bloodCompositeMaterial;

	// Token: 0x040002ED RID: 749
	public Mesh stainMesh;

	// Token: 0x040002EE RID: 750
	public Material stainMat;

	// Token: 0x040002EF RID: 751
	public NativeArray<BloodsplatterManager.InstanceProperties> checkpointProps;

	// Token: 0x040002F0 RID: 752
	public NativeArray<BloodsplatterManager.InstanceProperties> props;

	// Token: 0x040002F1 RID: 753
	public NativeArray<Matrix4x4> parents;

	// Token: 0x040002F2 RID: 754
	public ComputeBuffer instanceBuffer;

	// Token: 0x040002F3 RID: 755
	public ComputeBuffer parentBuffer;

	// Token: 0x040002F4 RID: 756
	private ComputeBuffer argsBuffer;

	// Token: 0x040002F5 RID: 757
	private uint[] argsData = new uint[5];

	// Token: 0x040002F6 RID: 758
	private int checkpointPropIndex;

	// Token: 0x040002F7 RID: 759
	private int propIndex;

	// Token: 0x040002F8 RID: 760
	private int parentIndex = 1;

	// Token: 0x040002FB RID: 763
	private Dictionary<BSType, Queue<GameObject>> gorePool = new Dictionary<BSType, Queue<GameObject>>();

	// Token: 0x040002FC RID: 764
	private Dictionary<BSType, int> defaultHPValues = new Dictionary<BSType, int>();

	// Token: 0x040002FD RID: 765
	private int order;

	// Token: 0x040002FE RID: 766
	private Transform goreStore;

	// Token: 0x040002FF RID: 767
	public bool hasBloodFillers;

	// Token: 0x04000300 RID: 768
	public HashSet<GameObject> bloodFillers = new HashSet<GameObject>();

	// Token: 0x04000301 RID: 769
	public AudioMixerGroup goreAudioGroup;

	// Token: 0x04000302 RID: 770
	public AudioClip splatterClip;

	// Token: 0x04000303 RID: 771
	[HideInInspector]
	public int bloodDestroyers;

	// Token: 0x04000304 RID: 772
	[HideInInspector]
	public int bloodAbsorbers;

	// Token: 0x04000305 RID: 773
	[HideInInspector]
	public int bloodAbsorberChildren;

	// Token: 0x04000307 RID: 775
	public const float PARTICLE_COLLISION_STEP_DT = 0.128f;

	// Token: 0x04000308 RID: 776
	public TimeSince sinceLastStep;

	// Token: 0x0400030B RID: 779
	private OptionsManager opm;

	// Token: 0x0400030C RID: 780
	private CommandBuffer cb;

	// Token: 0x0400030D RID: 781
	public bool usedComputeShadersAtStart = true;

	// Token: 0x0400030E RID: 782
	public bool meshDirty;

	// Token: 0x0400030F RID: 783
	private Mesh totalStainMesh;

	// Token: 0x04000310 RID: 784
	public GenerateBloodMeshJob generateBloodMeshJob;

	// Token: 0x04000311 RID: 785
	private JobHandle generateBloodMeshJobHandle;

	// Token: 0x04000312 RID: 786
	private Mesh.MeshDataArray meshDataArray;

	// Token: 0x02000084 RID: 132
	public struct InstanceProperties
	{
		// Token: 0x04000313 RID: 787
		public float3 pos;

		// Token: 0x04000314 RID: 788
		public float3 norm;

		// Token: 0x04000315 RID: 789
		public int parentIndex;

		// Token: 0x04000316 RID: 790
		public int clipToSurface;

		// Token: 0x04000317 RID: 791
		public const int SIZE = 32;
	}
}
