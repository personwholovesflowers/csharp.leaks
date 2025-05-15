using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200049D RID: 1181
public class BloodAbsorber : MonoBehaviour, IBloodstainReceiver
{
	// Token: 0x06001B31 RID: 6961 RVA: 0x000E2458 File Offset: 0x000E0658
	private void Start()
	{
		this.bcm = MonoSingleton<BloodCheckerManager>.Instance;
		this.cb = new CommandBuffer
		{
			name = "PaintRenderer"
		};
		this.absorbers = base.GetComponentsInChildren<MeshRenderer>();
		this.absorberMFs = new MeshFilter[this.absorbers.Length];
		CombineInstance[] array = new CombineInstance[this.absorbers.Length];
		for (int i = 0; i < this.absorbers.Length; i++)
		{
			MeshRenderer meshRenderer = this.absorbers[i];
			MeshFilter meshFilter;
			if (meshRenderer.TryGetComponent<MeshFilter>(out meshFilter))
			{
				this.absorberMFs[i] = meshFilter;
				array[i].mesh = meshFilter.sharedMesh;
			}
			else
			{
				Debug.LogError("No mesh found for: " + meshRenderer.gameObject.name);
			}
		}
		Mesh mesh = new Mesh();
		mesh.CombineMeshes(array);
		int num = Math.Min(Mathf.CeilToInt(Mathf.Sqrt(mesh.GetUVDistributionMetric(1)) * (float)this.texelsPerWorldUnit), 4096);
		float num2 = Mathf.Log((float)num, 2f);
		num2 = Mathf.Round(num2);
		num2 = Mathf.Pow(2f, num2);
		int num3 = (int)num2;
		this.propBlock = new MaterialPropertyBlock();
		this.absMat = new Material(this.absorptionShader);
		this.paintBuffer = new RenderTexture(num, num, 0, RenderTextureFormat.R16, 0)
		{
			filterMode = FilterMode.Point
		};
		this.bloodMap = new RenderTexture(num, num, 0, RenderTextureFormat.R8, 0)
		{
			filterMode = FilterMode.Point
		};
		this.dilationMask = new RenderTexture(num, num, 0, RenderTextureFormat.R8, 0)
		{
			filterMode = FilterMode.Point
		};
		this.clampedMap = new RenderTexture(num3, num3, 0, RenderTextureFormat.RHalf)
		{
			filterMode = FilterMode.Point,
			useMipMap = true,
			autoGenerateMips = false
		};
		this.paintBuffer.Create();
		this.bloodMap.Create();
		this.clampedMap.Create();
		this.dilationMask.Create();
		this.InitializeRTs();
		this.InitializeDilationMask();
		this.propBlock.SetTexture("_BloodBuffer", this.paintBuffer);
		this.propBlock.SetTexture("_BloodTex", this.bloodMap);
		this.propBlock.SetTexture("_DilationMask", this.dilationMask);
		for (int j = 0; j < this.absorbers.Length; j++)
		{
			this.absorbers[j].SetPropertyBlock(this.propBlock);
		}
		if (this.visibilityMask == null)
		{
			this.visibilityMask = Texture2D.whiteTexture;
		}
		this.cbuff = new ComputeBuffer(100, 16);
		this.collisionDataList = new List<BloodAbsorber.CollisionData>(this.cbuff.count);
		this.baseAccuracy = this.forgivenessCutoff;
	}

	// Token: 0x06001B32 RID: 6962 RVA: 0x000E26EA File Offset: 0x000E08EA
	public void ToggleHigherAccuracy(bool isOn)
	{
		this.forgivenessCutoff = (isOn ? 0.0001f : this.baseAccuracy);
	}

	// Token: 0x06001B33 RID: 6963 RVA: 0x000E2704 File Offset: 0x000E0904
	private void OnEnable()
	{
		BloodsplatterManager instance = MonoSingleton<BloodsplatterManager>.Instance;
		if (instance != null)
		{
			instance.bloodAbsorbers++;
		}
	}

	// Token: 0x06001B34 RID: 6964 RVA: 0x000E2730 File Offset: 0x000E0930
	private void OnDisable()
	{
		BloodsplatterManager instance = MonoSingleton<BloodsplatterManager>.Instance;
		if (instance != null)
		{
			instance.bloodAbsorbers--;
		}
	}

	// Token: 0x06001B35 RID: 6965 RVA: 0x000E275A File Offset: 0x000E095A
	public void StartCheckingFill()
	{
		if (this.checkFillRoutine == null)
		{
			base.StartCoroutine(this.CheckFill());
		}
	}

	// Token: 0x06001B36 RID: 6966 RVA: 0x000E2771 File Offset: 0x000E0971
	public void StoreBloodCopy()
	{
		if (this.bloodMapCheckpoint == null)
		{
			this.bloodMapCheckpoint = new RenderTexture(this.bloodMap);
		}
		Graphics.CopyTexture(this.bloodMap, this.bloodMapCheckpoint);
	}

	// Token: 0x06001B37 RID: 6967 RVA: 0x000E27A3 File Offset: 0x000E09A3
	public void RestoreBloodCopy()
	{
		if (this.bloodMapCheckpoint != null)
		{
			Graphics.CopyTexture(this.bloodMapCheckpoint, this.bloodMap);
		}
		else
		{
			this.InitializeRTs();
		}
		if (this.isCompleted)
		{
			this.StartCheckingFill();
		}
	}

	// Token: 0x06001B38 RID: 6968 RVA: 0x000E27DC File Offset: 0x000E09DC
	private void OnCollisionEnter(Collision collision)
	{
		GoreSplatter goreSplatter;
		if (collision.transform.TryGetComponent<GoreSplatter>(out goreSplatter))
		{
			goreSplatter.bloodAbsorberCount++;
			this.bcm.AddGoreToRoom(this, goreSplatter);
			return;
		}
		EnemyIdentifierIdentifier enemyIdentifierIdentifier;
		if (collision.transform.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
		{
			enemyIdentifierIdentifier.bloodAbsorberCount++;
			if (enemyIdentifierIdentifier.eid == enemyIdentifierIdentifier.GetComponentInParent<EnemyIdentifier>())
			{
				foreach (EnemyIdentifierIdentifier enemyIdentifierIdentifier2 in enemyIdentifierIdentifier.eid.GetComponentsInChildren<EnemyIdentifierIdentifier>())
				{
					this.bcm.AddGibToRoom(this, enemyIdentifierIdentifier2);
				}
				return;
			}
			this.bcm.AddGibToRoom(this, enemyIdentifierIdentifier);
		}
	}

	// Token: 0x06001B39 RID: 6969 RVA: 0x000E2880 File Offset: 0x000E0A80
	private void OnCollisionExit(Collision collision)
	{
		GoreSplatter goreSplatter;
		EnemyIdentifierIdentifier enemyIdentifierIdentifier;
		if (collision.transform.TryGetComponent<GoreSplatter>(out goreSplatter))
		{
			goreSplatter.bloodAbsorberCount--;
			if (StockMapInfo.Instance.removeGibsWithoutAbsorbers)
			{
				goreSplatter.Invoke("RepoolIfNoAbsorber", StockMapInfo.Instance.gibRemoveTime);
				return;
			}
		}
		else if (collision.transform.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
		{
			enemyIdentifierIdentifier.bloodAbsorberCount--;
			if (StockMapInfo.Instance.removeGibsWithoutAbsorbers)
			{
				enemyIdentifierIdentifier.SetupForHellBath();
			}
		}
	}

	// Token: 0x06001B3A RID: 6970 RVA: 0x000E28FB File Offset: 0x000E0AFB
	private IEnumerator CheckFill()
	{
		this.UnclearedAbsorber();
		yield return new WaitForSeconds(3f);
		if (this.maxFill == 0f)
		{
			Debug.LogWarning("No max fill data for: " + base.gameObject.name);
			Graphics.Blit(this.bloodMap, this.clampedMap, this.absMat, 5);
			this.clampedMap.GenerateMips();
			AsyncGPUReadbackRequest request2 = AsyncGPUReadback.Request(this.clampedMap, this.clampedMap.mipmapCount - 1, new Action<AsyncGPUReadbackRequest>(this.AsyncGetFilledSpace));
			yield return new WaitUntil(() => request2.done);
		}
		this.fillAmount = 999f;
		int timesChecked = -1;
		while (this.fillAmount >= this.forgivenessCutoff)
		{
			timesChecked = Math.Min(2, timesChecked + 1);
			Graphics.Blit(this.bloodMap, this.clampedMap, this.absMat, 5);
			this.clampedMap.GenerateMips();
			AsyncGPUReadbackRequest request = AsyncGPUReadback.Request(this.clampedMap, this.clampedMap.mipmapCount - 1, new Action<AsyncGPUReadbackRequest>(this.AsyncGetCurrentFillAmount));
			yield return new WaitUntil(() => request.done);
		}
		if (timesChecked == 2)
		{
			Object.Instantiate<AudioSource>(this.cleanSuccess);
		}
		this.checkFillRoutine = null;
		base.StartCoroutine(this.ClearedAbsorber());
		this.isWashing = true;
		this.fillAmount = 1f;
		this.cb.Clear();
		this.cb.SetRenderTarget(this.bloodMap);
		this.cb.ClearRenderTarget(false, true, Color.black);
		Graphics.ExecuteCommandBuffer(this.cb);
		yield break;
	}

	// Token: 0x06001B3B RID: 6971 RVA: 0x000E290A File Offset: 0x000E0B0A
	private IEnumerator ClearedAbsorber()
	{
		this.isCompleted = true;
		float time = 0f;
		this.propBlock.SetTexture("_CubeTex", this.cleanedMap);
		while (time < 1f)
		{
			float num = Mathf.Lerp(0f, 0.06f, time);
			this.propBlock.SetFloat("_ReflectionStrength", num);
			Color color = Color.Lerp(new Color(1f, 0.9f, 0.5f), Color.white, time);
			this.propBlock.SetColor("_EmissiveColor", color);
			float num2 = Mathf.Lerp(2f, 1f, time);
			this.propBlock.SetFloat("_EmissiveIntensity", num2);
			int num3 = this.absorbers.Length;
			for (int i = 0; i < num3; i++)
			{
				this.absorbers[i].SetPropertyBlock(this.propBlock);
			}
			time += Time.deltaTime;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001B3C RID: 6972 RVA: 0x000E291C File Offset: 0x000E0B1C
	private void UnclearedAbsorber()
	{
		this.isCompleted = false;
		this.propBlock.SetFloat("_ReflectionStrength", 0f);
		this.propBlock.SetColor("_EmissiveColor", Color.white);
		this.propBlock.SetFloat("_EmissiveIntensity", 1f);
		int num = this.absorbers.Length;
		for (int i = 0; i < num; i++)
		{
			this.absorbers[i].SetPropertyBlock(this.propBlock);
		}
	}

	// Token: 0x06001B3D RID: 6973 RVA: 0x000E2998 File Offset: 0x000E0B98
	private void AsyncGetFilledSpace(AsyncGPUReadbackRequest request)
	{
		if (!this.clampedMap)
		{
			Debug.LogError("No blood map exists while attempting to calculate absorber max fill.");
		}
		this.maxFill = request.GetData<half>(0)[0];
		this.fillAmount = this.maxFill;
	}

	// Token: 0x06001B3E RID: 6974 RVA: 0x000E29E4 File Offset: 0x000E0BE4
	private void AsyncGetCurrentFillAmount(AsyncGPUReadbackRequest request)
	{
		if (!this.clampedMap)
		{
			return;
		}
		float num = request.GetData<half>(0)[0];
		this.fillAmount = num / this.maxFill;
	}

	// Token: 0x06001B3F RID: 6975 RVA: 0x000E2A24 File Offset: 0x000E0C24
	private void OnValidate()
	{
		if (this.clearBlood)
		{
			this.InitializeRTs();
			this.clearBlood = false;
		}
		if (this.fillBlood)
		{
			Graphics.Blit(this.visibilityMask, this.bloodMap, this.absMat, 7);
			this.fillBlood = false;
		}
		int num = this.visibilityMask.width * this.visibilityMask.height;
		float num2 = 0f;
		for (int i = 0; i < this.visibilityMask.width; i++)
		{
			for (int j = 0; j < this.visibilityMask.height; j++)
			{
				num2 += this.visibilityMask.GetPixel(i, j).r;
			}
		}
		this.maxFill = num2 / (float)num;
	}

	// Token: 0x06001B40 RID: 6976 RVA: 0x000E2AD8 File Offset: 0x000E0CD8
	private void InitializeRTs()
	{
		this.cb.Clear();
		this.cb.SetRenderTarget(this.paintBuffer);
		this.cb.ClearRenderTarget(true, true, Color.black);
		this.cb.SetRenderTarget(this.bloodMap);
		this.cb.ClearRenderTarget(true, true, Color.black);
		Graphics.ExecuteCommandBuffer(this.cb);
	}

	// Token: 0x06001B41 RID: 6977 RVA: 0x000E2B4C File Offset: 0x000E0D4C
	private void InitializeDilationMask()
	{
		this.cb.Clear();
		this.cb.SetRenderTarget(this.dilationMask);
		for (int i = 0; i < this.absorbers.Length; i++)
		{
			MeshRenderer meshRenderer = this.absorbers[i];
			int subMeshCount = this.absorberMFs[i].sharedMesh.subMeshCount;
			for (int j = 0; j < subMeshCount; j++)
			{
				this.cb.DrawRenderer(meshRenderer, this.absMat, j, 6);
			}
		}
		Graphics.ExecuteCommandBuffer(this.cb);
	}

	// Token: 0x06001B42 RID: 6978 RVA: 0x000E2BD4 File Offset: 0x000E0DD4
	private void Update()
	{
		if (!this.isSleeping && this.sleepTimer >= this.timeUntilSleep)
		{
			this.cb.Clear();
			this.cb.SetRenderTarget(this.paintBuffer);
			this.cb.ClearRenderTarget(true, true, Color.black);
			Graphics.ExecuteCommandBuffer(this.cb);
			this.clearBlood = false;
			this.isSleeping = true;
			return;
		}
		if (this.bloodTimer >= this.bloodUpdateRate)
		{
			Graphics.Blit(null, this.paintBuffer, this.absMat, 1);
			this.absMat.SetTexture("_VisibilityMask", this.visibilityMask);
			Graphics.Blit(this.paintBuffer, this.bloodMap, this.absMat, this.isWashing ? 3 : 2);
			this.bloodTimer = 0f;
		}
		this.bloodTimer += Time.deltaTime;
		this.sleepTimer += Time.deltaTime;
	}

	// Token: 0x06001B43 RID: 6979 RVA: 0x000E2CD0 File Offset: 0x000E0ED0
	public bool HandleBloodstainHit(ref RaycastHit hit)
	{
		if (this.isCompleted)
		{
			this.StartCheckingFill();
		}
		if (this.isWashing)
		{
			this.cb.Clear();
			this.cb.SetRenderTarget(this.paintBuffer);
			this.cb.ClearRenderTarget(false, true, Color.black);
		}
		this.isWashing = false;
		this.isSleeping = false;
		this.sleepTimer = 0f;
		Vector3 point = hit.point;
		Vector3 normal = hit.normal;
		Vector3 vector = normal * -1f;
		Quaternion quaternion = Quaternion.LookRotation(normal, Vector3.up);
		Quaternion quaternion2 = Quaternion.AngleAxis(global::UnityEngine.Random.Range(0f, 360f), normal);
		Matrix4x4 matrix4x = Matrix4x4.Rotate(quaternion * quaternion2);
		this.absMat.SetVector("_HitPos", point);
		this.absMat.SetVector("_HitNorm", vector);
		this.absMat.SetMatrix("_RotMat", matrix4x);
		this.absMat.SetTexture("_MainTex", this.paintBuffer);
		this.absMat.SetTexture("_NoiseTex", this.noiseTex);
		this.absMat.SetTexture("_VisibilityMask", this.visibilityMask);
		this.cb.Clear();
		this.cb.SetRenderTarget(this.paintBuffer);
		this.propBlock.SetFloat("_IsWashing", 0f);
		int num = this.absorbers.Length;
		for (int i = 0; i < num; i++)
		{
			MeshRenderer meshRenderer = this.absorbers[i];
			int subMeshCount = this.absorberMFs[i].sharedMesh.subMeshCount;
			for (int j = 0; j < subMeshCount; j++)
			{
				this.cb.DrawRenderer(meshRenderer, this.absMat, j, 0);
			}
			meshRenderer.SetPropertyBlock(this.propBlock);
		}
		Graphics.ExecuteCommandBuffer(this.cb);
		Graphics.Blit(this.paintBuffer, this.bloodMap, this.absMat, 2);
		return true;
	}

	// Token: 0x06001B44 RID: 6980 RVA: 0x000E2ED0 File Offset: 0x000E10D0
	public void ProcessWasherSpray(ref List<ParticleCollisionEvent> pEvents, Vector3 position, MeshRenderer hitChild)
	{
		if (!this.isWashing)
		{
			this.cb.Clear();
			this.cb.SetRenderTarget(this.paintBuffer);
			this.cb.ClearRenderTarget(false, true, Color.black);
		}
		this.isWashing = true;
		this.collisionDataList.Clear();
		int num = 0;
		foreach (ParticleCollisionEvent particleCollisionEvent in pEvents)
		{
			if (num >= this.cbuff.count)
			{
				break;
			}
			BloodAbsorber.CollisionData collisionData = new BloodAbsorber.CollisionData
			{
				position = particleCollisionEvent.intersection,
				distance = Vector3.Distance(particleCollisionEvent.intersection, position)
			};
			this.collisionDataList.Add(collisionData);
			num++;
		}
		this.cbuff.SetData<BloodAbsorber.CollisionData>(this.collisionDataList);
		this.isSleeping = false;
		this.sleepTimer = 0f;
		this.absMat.SetFloat("_HitCount", (float)num);
		this.absMat.SetBuffer("_HitData", this.cbuff);
		this.absMat.SetTexture("_MainTex", this.paintBuffer);
		this.absMat.SetTexture("_NoiseTex", this.noiseTex);
		this.cb.Clear();
		this.cb.SetRenderTarget(this.paintBuffer);
		this.propBlock.SetFloat("_IsWashing", 1f);
		if (hitChild != null)
		{
			int subMeshCount = this.absorberMFs[Array.IndexOf<MeshRenderer>(this.absorbers, hitChild)].sharedMesh.subMeshCount;
			for (int i = 0; i < subMeshCount; i++)
			{
				this.cb.DrawRenderer(hitChild, this.absMat, i, 4);
			}
			hitChild.SetPropertyBlock(this.propBlock);
		}
		else
		{
			int num2 = this.absorbers.Length;
			for (int j = 0; j < num2; j++)
			{
				MeshRenderer meshRenderer = this.absorbers[j];
				int subMeshCount2 = this.absorberMFs[j].sharedMesh.subMeshCount;
				for (int k = 0; k < subMeshCount2; k++)
				{
					this.cb.DrawRenderer(meshRenderer, this.absMat, k, 4);
				}
				meshRenderer.SetPropertyBlock(this.propBlock);
			}
		}
		Graphics.ExecuteCommandBuffer(this.cb);
		this.absMat.SetTexture("_VisibilityMask", this.visibilityMask);
		Graphics.Blit(this.paintBuffer, this.bloodMap, this.absMat, 3);
	}

	// Token: 0x04002661 RID: 9825
	public string painterName;

	// Token: 0x04002662 RID: 9826
	public Shader absorptionShader;

	// Token: 0x04002663 RID: 9827
	public Texture noiseTex;

	// Token: 0x04002664 RID: 9828
	public Texture2D visibilityMask;

	// Token: 0x04002665 RID: 9829
	public int texelsPerWorldUnit = 10;

	// Token: 0x04002666 RID: 9830
	public float forgivenessCutoff = 0.01f;

	// Token: 0x04002667 RID: 9831
	public float bloodUpdateRate = 0.02f;

	// Token: 0x04002668 RID: 9832
	public float timeUntilSleep = 4f;

	// Token: 0x04002669 RID: 9833
	public bool clearBlood;

	// Token: 0x0400266A RID: 9834
	public bool fillBlood;

	// Token: 0x0400266B RID: 9835
	[SerializeField]
	private float maxFill;

	// Token: 0x0400266C RID: 9836
	[HideInInspector]
	public float fillAmount = 999f;

	// Token: 0x0400266D RID: 9837
	public bool isCompleted;

	// Token: 0x0400266E RID: 9838
	private float sleepTimer;

	// Token: 0x0400266F RID: 9839
	private bool isWashing;

	// Token: 0x04002670 RID: 9840
	private float bloodTimer;

	// Token: 0x04002671 RID: 9841
	private bool isSleeping = true;

	// Token: 0x04002672 RID: 9842
	private CommandBuffer cb;

	// Token: 0x04002673 RID: 9843
	private Material absMat;

	// Token: 0x04002674 RID: 9844
	public RenderTexture paintBuffer;

	// Token: 0x04002675 RID: 9845
	public RenderTexture bloodMap;

	// Token: 0x04002676 RID: 9846
	public RenderTexture clampedMap;

	// Token: 0x04002677 RID: 9847
	public RenderTexture dilationMask;

	// Token: 0x04002678 RID: 9848
	public RenderTexture bloodMapCheckpoint;

	// Token: 0x04002679 RID: 9849
	private MeshRenderer[] absorbers;

	// Token: 0x0400267A RID: 9850
	private MeshFilter[] absorberMFs;

	// Token: 0x0400267B RID: 9851
	private MaterialPropertyBlock propBlock;

	// Token: 0x0400267C RID: 9852
	private ComputeBuffer cbuff;

	// Token: 0x0400267D RID: 9853
	[HideInInspector]
	public GameObject owningRoom;

	// Token: 0x0400267E RID: 9854
	private BloodCheckerManager bcm;

	// Token: 0x0400267F RID: 9855
	[SerializeField]
	private AudioSource cleanSuccess;

	// Token: 0x04002680 RID: 9856
	public Cubemap cleanedMap;

	// Token: 0x04002681 RID: 9857
	private List<BloodAbsorber.CollisionData> collisionDataList;

	// Token: 0x04002682 RID: 9858
	private Coroutine checkFillRoutine;

	// Token: 0x04002683 RID: 9859
	private float baseAccuracy;

	// Token: 0x0200049E RID: 1182
	private struct CollisionData
	{
		// Token: 0x04002684 RID: 9860
		public Vector3 position;

		// Token: 0x04002685 RID: 9861
		public float distance;
	}
}
