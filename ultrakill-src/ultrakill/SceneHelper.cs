using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Logic;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// Token: 0x02000047 RID: 71
[ConfigureSingleton(SingletonFlags.NoAutoInstance | SingletonFlags.PersistAutoInstance | SingletonFlags.DestroyDuplicates)]
public class SceneHelper : MonoSingleton<SceneHelper>
{
	// Token: 0x17000041 RID: 65
	// (get) Token: 0x0600014B RID: 331 RVA: 0x00006D64 File Offset: 0x00004F64
	private static LayerMask footstepLayerMask
	{
		get
		{
			LayerMask layerMask = SceneHelper._footstepLayerMask.GetValueOrDefault();
			if (SceneHelper._footstepLayerMask == null)
			{
				layerMask = LayerMask.GetMask(new string[] { "Environment", "Outdoors", "EnvironmentBaked", "OutdoorsBaked" });
				SceneHelper._footstepLayerMask = new LayerMask?(layerMask);
			}
			return SceneHelper._footstepLayerMask.Value;
		}
	}

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x0600014C RID: 332 RVA: 0x00006DCE File Offset: 0x00004FCE
	public static bool IsPlayingCustom
	{
		get
		{
			return GameStateManager.Instance.currentCustomGame != null;
		}
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x0600014D RID: 333 RVA: 0x00006DDD File Offset: 0x00004FDD
	public static bool IsSceneRankless
	{
		get
		{
			return MonoSingleton<SceneHelper>.Instance.embeddedSceneInfo.ranklessScenes.Contains(SceneHelper.CurrentScene);
		}
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x0600014E RID: 334 RVA: 0x00006DF8 File Offset: 0x00004FF8
	public static int CurrentLevelNumber
	{
		get
		{
			if (!SceneHelper.IsPlayingCustom)
			{
				return MonoSingleton<StatsManager>.Instance.levelNumber;
			}
			return GameStateManager.Instance.currentCustomGame.levelNumber;
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x0600014F RID: 335 RVA: 0x00006E1B File Offset: 0x0000501B
	// (set) Token: 0x06000150 RID: 336 RVA: 0x00006E22 File Offset: 0x00005022
	public static string CurrentScene { get; private set; }

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x06000151 RID: 337 RVA: 0x00006E2A File Offset: 0x0000502A
	// (set) Token: 0x06000152 RID: 338 RVA: 0x00006E31 File Offset: 0x00005031
	public static string LastScene { get; private set; }

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x06000153 RID: 339 RVA: 0x00006E39 File Offset: 0x00005039
	// (set) Token: 0x06000154 RID: 340 RVA: 0x00006E40 File Offset: 0x00005040
	public static string PendingScene { get; private set; }

	// Token: 0x06000155 RID: 341 RVA: 0x00006E48 File Offset: 0x00005048
	public bool TryGetSurfaceData(Vector3 pos, out SceneHelper.HitSurfaceData hitSurfaceData)
	{
		return this.TryGetSurfaceData(pos, Vector3.down, 5f, out hitSurfaceData);
	}

	// Token: 0x06000156 RID: 342 RVA: 0x00006E5C File Offset: 0x0000505C
	public bool TryGetSurfaceData(Vector3 pos, Vector3 direction, float distance, out SceneHelper.HitSurfaceData hitSurfaceData)
	{
		hitSurfaceData = new SceneHelper.HitSurfaceData(SurfaceType.Generic, Color.white);
		PhysicsScene physicsScene = this.footstepPhysicsScene;
		return this.footstepPhysicsScene.Raycast(pos, direction, out hitSurfaceData.hit, distance, SceneHelper.footstepLayerMask, QueryTriggerInteraction.Ignore) && this.TryGetRaycastHitSurfaceData(ref hitSurfaceData);
	}

	// Token: 0x06000157 RID: 343 RVA: 0x00006EAE File Offset: 0x000050AE
	private bool TryGetRaycastHitSurfaceData(ref SceneHelper.HitSurfaceData hitSurfaceData)
	{
		hitSurfaceData.surfaceType = SurfaceType.Generic;
		if (this.ResolveHitSurfaceData(ref hitSurfaceData))
		{
			this.ResolveSurfaceType(ref hitSurfaceData);
			return true;
		}
		return false;
	}

	// Token: 0x06000158 RID: 344 RVA: 0x00006ECC File Offset: 0x000050CC
	private bool ResolveHitSurfaceData(ref SceneHelper.HitSurfaceData hitSurfaceData)
	{
		hitSurfaceData.material = null;
		hitSurfaceData.mesh = null;
		hitSurfaceData.useSecondaryBlend = false;
		if (hitSurfaceData.hit.triangleIndex == -1)
		{
			return false;
		}
		Collider collider = hitSurfaceData.hit.collider;
		MeshRenderer meshRenderer;
		if (!collider || !collider.TryGetComponent<MeshRenderer>(out meshRenderer))
		{
			return false;
		}
		hitSurfaceData.mesh = ((MeshCollider)collider).sharedMesh;
		meshRenderer.GetSharedMaterials(this.reusableMaterials);
		Material material = this.reusableMaterials[0];
		int num = hitSurfaceData.hit.triangleIndex * 3;
		int num2 = 0;
		int num3 = 0;
		if (hitSurfaceData.mesh.subMeshCount <= 1 || material.IsKeywordEnabled("STATIONARY_LIGHTING") || material.IsKeywordEnabled("STATIC_LIGHTING"))
		{
			hitSurfaceData.material = material;
		}
		else
		{
			int num4 = 0;
			for (int i = meshRenderer.subMeshStartIndex; i < hitSurfaceData.mesh.subMeshCount; i++)
			{
				int num5 = num4 + hitSurfaceData.mesh.GetSubMesh(i).indexCount;
				if (num < num5)
				{
					num2 = i;
					num3 = num4;
					hitSurfaceData.material = this.reusableMaterials[i - meshRenderer.subMeshStartIndex];
					this.reusableMaterials.Clear();
					break;
				}
				num4 = num5;
			}
		}
		if (hitSurfaceData.material != null && hitSurfaceData.material.IsKeywordEnabled("VERTEX_BLENDING"))
		{
			this.reusableColors.Clear();
			hitSurfaceData.mesh.GetColors(this.reusableColors);
			if (this.reusableColors.Count > 0)
			{
				this.reusableTriangles.Clear();
				hitSurfaceData.mesh.GetTriangles(this.reusableTriangles, num2);
				int num6 = num - num3;
				float r = this.reusableColors[this.reusableTriangles[num6]].r;
				float r2 = this.reusableColors[this.reusableTriangles[num6 + 1]].r;
				float r3 = this.reusableColors[this.reusableTriangles[num6 + 2]].r;
				Vector3 barycentricCoordinate = hitSurfaceData.hit.barycentricCoordinate;
				float num7 = r * barycentricCoordinate.x + r2 * barycentricCoordinate.y + r3 * barycentricCoordinate.z;
				hitSurfaceData.useSecondaryBlend = num7 < 0.5f;
			}
			else
			{
				Debug.LogWarning("Material uses vertex blending but no vertex colors were found!", meshRenderer.gameObject);
			}
		}
		return hitSurfaceData.material != null;
	}

	// Token: 0x06000159 RID: 345 RVA: 0x00007134 File Offset: 0x00005334
	public void ResolveSurfaceType(ref SceneHelper.HitSurfaceData hitSurfaceData)
	{
		if (hitSurfaceData.material.IsKeywordEnabled("STATIONARY_LIGHTING") || hitSurfaceData.material.IsKeywordEnabled("STATIC_LIGHTING"))
		{
			Debug.Log("TODO for Victoria");
			int num = hitSurfaceData.hit.triangleIndex * 3;
			int num2 = hitSurfaceData.mesh.triangles[num];
			hitSurfaceData.mesh.uv7[num2];
		}
		if (hitSurfaceData.material.HasProperty(ShaderProperties.SurfaceType))
		{
			if (hitSurfaceData.useSecondaryBlend)
			{
				hitSurfaceData.surfaceType = (SurfaceType)Mathf.RoundToInt(hitSurfaceData.material.GetFloat(ShaderProperties.SecondarySurfaceType));
				hitSurfaceData.particleColor = hitSurfaceData.material.GetColor(ShaderProperties.SecondaryEnviroParticleColor);
				return;
			}
			hitSurfaceData.surfaceType = (SurfaceType)Mathf.RoundToInt(hitSurfaceData.material.GetFloat(ShaderProperties.SurfaceType));
			hitSurfaceData.particleColor = hitSurfaceData.material.GetColor(ShaderProperties.EnviroParticleColor);
		}
	}

	// Token: 0x0600015A RID: 346 RVA: 0x0000721F File Offset: 0x0000541F
	public void CreateEnviroGibs(RaycastHit hit, int gibAmount = 3, float sizeMultiplier = 1f)
	{
		this.CreateEnviroGibs(hit.point + hit.normal, -hit.normal, 5f, gibAmount, sizeMultiplier);
	}

	// Token: 0x0600015B RID: 347 RVA: 0x0000724D File Offset: 0x0000544D
	public void CreateEnviroGibs(ContactPoint hit, int gibAmount = 3, float sizeMultiplier = 1f)
	{
		this.CreateEnviroGibs(hit.point + hit.normal, -hit.normal, 5f, gibAmount, sizeMultiplier);
	}

	// Token: 0x0600015C RID: 348 RVA: 0x0000727C File Offset: 0x0000547C
	public void CreateEnviroGibs(Vector3 position, Vector3 direction, float distance, int gibAmount = 3, float sizeMultiplier = 1f)
	{
		if (sizeMultiplier <= 0f)
		{
			return;
		}
		if (!this.environmentalHitParticles)
		{
			return;
		}
		SceneHelper.HitSurfaceData hitSurfaceData;
		if (!this.TryGetSurfaceData(position, direction, distance, out hitSurfaceData))
		{
			return;
		}
		SurfaceType surfaceType = hitSurfaceData.surfaceType;
		FootstepSet footstepSet = MonoSingleton<DefaultReferenceManager>.Instance.footstepSet;
		RaycastHit hit = hitSurfaceData.hit;
		GameObject[] array;
		if (gibAmount > 0 && footstepSet.TryGetEnviroGibs(surfaceType, out array) && array != null && array.Length != 0)
		{
			for (int i = 0; i < gibAmount; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(array[Random.Range(0, array.Length)], hit.point + hit.normal, Random.rotation);
				gameObject.transform.localScale *= sizeMultiplier;
				MeshRenderer meshRenderer;
				Renderer renderer;
				if (gameObject.TryGetComponent<MeshRenderer>(out meshRenderer))
				{
					meshRenderer.material = hitSurfaceData.material;
					if (hitSurfaceData.useSecondaryBlend)
					{
						meshRenderer.material.SetFloat("_ForceSecondary", 1f);
					}
				}
				else if (gameObject.TryGetComponent<Renderer>(out renderer))
				{
					renderer.material.color = hitSurfaceData.particleColor;
				}
				Rigidbody rigidbody;
				if (gameObject.TryGetComponent<Rigidbody>(out rigidbody))
				{
					float num = 1.5f;
					if (i >= 3)
					{
						num /= (float)(i / 2);
					}
					Vector3 vector = hit.normal + new Vector3(Random.Range(-num, num), Random.Range(-num, num), Random.Range(-num, num));
					rigidbody.AddForce(vector.normalized * (1f + (sizeMultiplier - 1f) / 2f) * 30f, ForceMode.Impulse);
				}
			}
		}
		GameObject gameObject2;
		if (footstepSet.TryGetEnviroGibParticle(surfaceType, out gameObject2))
		{
			GameObject gameObject = Object.Instantiate<GameObject>(gameObject2, hit.point, Quaternion.LookRotation(hit.normal));
			EnviroGibModifier[] componentsInChildren = gameObject.GetComponentsInChildren<EnviroGibModifier>();
			if (componentsInChildren != null && componentsInChildren.Length != 0)
			{
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					ParticleSystem particleSystem;
					if (componentsInChildren[j] != null && componentsInChildren[j].increaseBurstEmission && componentsInChildren[j].TryGetComponent<ParticleSystem>(out particleSystem) && particleSystem.emission.burstCount > 0)
					{
						componentsInChildren[j].transform.localScale /= sizeMultiplier;
						ParticleSystem.MainModule main = particleSystem.main;
						main.startSpeed = this.MultiplyCurve(main.startSpeed, sizeMultiplier);
						ParticleSystem.Burst burst = particleSystem.emission.GetBurst(0);
						ParticleSystem.MinMaxCurve minMaxCurve = this.MultiplyCurve(burst.count, sizeMultiplier);
						burst.count = minMaxCurve;
						particleSystem.emission.SetBurst(0, burst);
					}
				}
			}
			gameObject.transform.localScale *= sizeMultiplier;
			this.SetParticlesColors(componentsInChildren, ref hitSurfaceData);
		}
	}

	// Token: 0x0600015D RID: 349 RVA: 0x00007556 File Offset: 0x00005756
	public void SetParticlesColors(GameObject target, ref SceneHelper.HitSurfaceData hitSurfaceData)
	{
		this.SetParticlesColors(target.GetComponentsInChildren<EnviroGibModifier>(), ref hitSurfaceData);
	}

	// Token: 0x0600015E RID: 350 RVA: 0x00007565 File Offset: 0x00005765
	public void SetParticlesColors(EnviroGibModifier[] modifiers, ref SceneHelper.HitSurfaceData hitSurfaceData)
	{
		this.SetParticlesColors(modifiers, hitSurfaceData.particleColor);
	}

	// Token: 0x0600015F RID: 351 RVA: 0x00007574 File Offset: 0x00005774
	public void SetParticlesColors(EnviroGibModifier[] modifiers, Color clr)
	{
		if (clr != Color.white)
		{
			foreach (EnviroGibModifier enviroGibModifier in modifiers)
			{
				if (enviroGibModifier.particleSystem)
				{
					ParticleSystem.MainModule main = enviroGibModifier.particleSystem.main;
					clr.a = main.startColor.color.a;
					main.startColor = clr;
				}
				if (enviroGibModifier.spriteRenderer != null)
				{
					clr.a = enviroGibModifier.spriteRenderer.color.a;
					enviroGibModifier.spriteRenderer.color = clr;
				}
			}
		}
	}

	// Token: 0x06000160 RID: 352 RVA: 0x0000761C File Offset: 0x0000581C
	private ParticleSystem.MinMaxCurve MultiplyCurve(ParticleSystem.MinMaxCurve curve, float multiplier)
	{
		if (curve.mode == ParticleSystemCurveMode.Curve || curve.mode == ParticleSystemCurveMode.TwoCurves)
		{
			curve.curveMultiplier *= multiplier;
		}
		else if (curve.mode == ParticleSystemCurveMode.TwoConstants)
		{
			curve.constantMin *= multiplier;
			curve.constantMax *= multiplier;
		}
		else if (curve.mode == ParticleSystemCurveMode.Constant)
		{
			curve.constant *= multiplier;
		}
		return curve;
	}

	// Token: 0x06000161 RID: 353 RVA: 0x00007694 File Offset: 0x00005894
	protected override void OnEnable()
	{
		base.OnEnable();
		Object.DontDestroyOnLoad(base.gameObject);
		SceneManager.sceneLoaded += this.OnSceneLoaded;
		this.OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
		if (string.IsNullOrEmpty(SceneHelper.CurrentScene))
		{
			SceneHelper.CurrentScene = SceneManager.GetActiveScene().name;
		}
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
		this.environmentalHitParticles = !MonoSingleton<PrefsManager>.Instance.GetBoolLocal("disableHitParticles", false);
	}

	// Token: 0x06000162 RID: 354 RVA: 0x00007726 File Offset: 0x00005926
	private void OnPrefChanged(string key, object value)
	{
		if (key == "disableHitParticles")
		{
			this.environmentalHitParticles = !(bool)value;
		}
	}

	// Token: 0x06000163 RID: 355 RVA: 0x00007744 File Offset: 0x00005944
	private void OnDisable()
	{
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06000164 RID: 356 RVA: 0x00007777 File Offset: 0x00005977
	public bool IsSceneSpecial(string sceneName)
	{
		sceneName = SceneHelper.SanitizeLevelPath(sceneName);
		return !(this.embeddedSceneInfo == null) && this.embeddedSceneInfo.specialScenes.Contains(sceneName);
	}

	// Token: 0x06000165 RID: 357 RVA: 0x000077A2 File Offset: 0x000059A2
	public static bool IsStaticEnvironment(RaycastHit hit)
	{
		return SceneHelper.IsStaticEnvironment(hit.collider);
	}

	// Token: 0x06000166 RID: 358 RVA: 0x000077B0 File Offset: 0x000059B0
	public static bool IsStaticEnvironment(Collider col)
	{
		if (col == null || col.isTrigger)
		{
			return false;
		}
		if (col.attachedRigidbody != null && !col.attachedRigidbody.isKinematic)
		{
			return false;
		}
		Renderer renderer;
		if (!col.TryGetComponent<Renderer>(out renderer) || !renderer.enabled)
		{
			return false;
		}
		LayerMask layerMask = LayerMaskDefaults.Get(LMD.Environment);
		return ((1 << col.gameObject.layer) & layerMask) != 0;
	}

	// Token: 0x06000167 RID: 359 RVA: 0x00007822 File Offset: 0x00005A22
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (EventSystem.current != null)
		{
			Object.Destroy(EventSystem.current.gameObject);
		}
		Object.Instantiate<GameObject>(this.eventSystem);
		if (mode == LoadSceneMode.Single)
		{
			GameStateManager.Instance.SceneReset();
			this.SetUpFootstepPhysicsScene(scene);
		}
	}

	// Token: 0x06000168 RID: 360 RVA: 0x00007860 File Offset: 0x00005A60
	public static bool IsValidForPhysicsScene(Transform transform, out MeshRenderer mr, out Collider col)
	{
		int layer = transform.gameObject.layer;
		mr = null;
		col = null;
		Rigidbody rigidbody;
		return (SceneHelper.footstepLayerMask & (1 << layer)) != 0 && transform.TryGetComponent<Collider>(out col) && !col.isTrigger && transform.TryGetComponent<MeshRenderer>(out mr) && (!transform.TryGetComponent<Rigidbody>(out rigidbody) || rigidbody.isKinematic);
	}

	// Token: 0x06000169 RID: 361 RVA: 0x000078C8 File Offset: 0x00005AC8
	private GameObject CreatePhysicsSceneObject(SceneHelper.PhysicsSceneObjectData data)
	{
		GameObject gameObject = new GameObject(data.Name);
		gameObject.layer = data.Layer;
		gameObject.transform.SetPositionAndRotation(data.Position, data.Rotation);
		gameObject.transform.localScale = data.Scale;
		MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
		meshCollider.sharedMesh = data.Mesh;
		meshCollider.convex = false;
		meshCollider.enabled = true;
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		meshRenderer.enabled = false;
		meshRenderer.sharedMaterials = data.Materials;
		return gameObject;
	}

	// Token: 0x0600016A RID: 362 RVA: 0x00007954 File Offset: 0x00005B54
	public GameObject AddToPhysicsScene(SceneHelper.PhysicsSceneObjectData data, GameObject sourceObject = null)
	{
		if (!this.footstepScene.isLoaded)
		{
			Debug.LogError("Footstep scene is not initialized!");
			return null;
		}
		GameObject gameObject = this.CreatePhysicsSceneObject(data);
		SceneManager.MoveGameObjectToScene(gameObject, this.footstepScene);
		if (sourceObject == null)
		{
			return gameObject;
		}
		sourceObject.AddComponent<PhysicsSceneStateEnforcer>().SetMatchingObject(gameObject);
		return gameObject;
	}

	// Token: 0x0600016B RID: 363 RVA: 0x000079A8 File Offset: 0x00005BA8
	public GameObject AddMeshToPhysicsScene(Mesh mesh, Material[] materials, Vector3 position, Quaternion rotation, Vector3 scale, int layer, GameObject sourceObject = null)
	{
		string text = ((sourceObject != null) ? sourceObject.name : "Physics Mesh");
		SceneHelper.PhysicsSceneObjectData physicsSceneObjectData = new SceneHelper.PhysicsSceneObjectData(mesh, materials, position, rotation, scale, layer, text);
		return this.AddToPhysicsScene(physicsSceneObjectData, sourceObject);
	}

	// Token: 0x0600016C RID: 364 RVA: 0x000079E8 File Offset: 0x00005BE8
	private void SetUpFootstepPhysicsScene(Scene scene)
	{
		Scene scene2 = this.footstepScene;
		if (this.footstepScene.isLoaded)
		{
			SceneManager.UnloadSceneAsync(this.footstepScene);
		}
		this.footstepScene = SceneManager.CreateScene(scene.name + " - Footsteps", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
		this.footstepPhysicsScene = this.footstepScene.GetPhysicsScene();
		GameObject[] rootGameObjects = scene.GetRootGameObjects();
		for (int i = 0; i < rootGameObjects.Length; i++)
		{
			Transform[] componentsInChildren = rootGameObjects[i].GetComponentsInChildren<Transform>(true);
			this.DuplicateColliders(componentsInChildren);
		}
	}

	// Token: 0x0600016D RID: 365 RVA: 0x00007A70 File Offset: 0x00005C70
	private void DuplicateColliders(Transform[] transforms)
	{
		foreach (Transform transform in transforms)
		{
			SceneHelper.PhysicsSceneObjectData physicsSceneObjectData;
			if (SceneHelper.PhysicsSceneObjectData.TryCreateFromObject(transform, out physicsSceneObjectData, false))
			{
				this.AddToPhysicsScene(physicsSceneObjectData, transform.gameObject);
			}
		}
	}

	// Token: 0x0600016E RID: 366 RVA: 0x00007AAC File Offset: 0x00005CAC
	public static string SanitizeLevelPath(string scene)
	{
		if (scene.StartsWith("Assets/Scenes/"))
		{
			scene = scene.Substring("Assets/Scenes/".Length);
		}
		if (scene.EndsWith(".unity"))
		{
			scene = scene.Substring(0, scene.Length - ".unity".Length);
		}
		return scene;
	}

	// Token: 0x0600016F RID: 367 RVA: 0x00007B00 File Offset: 0x00005D00
	public static void ShowLoadingBlocker()
	{
		MonoSingleton<SceneHelper>.Instance.loadingBlocker.SetActive(true);
	}

	// Token: 0x06000170 RID: 368 RVA: 0x00007B12 File Offset: 0x00005D12
	public static void DismissBlockers()
	{
		MonoSingleton<SceneHelper>.Instance.loadingBlocker.SetActive(false);
		MonoSingleton<SceneHelper>.Instance.loadingBar.gameObject.SetActive(false);
	}

	// Token: 0x06000171 RID: 369 RVA: 0x00007B39 File Offset: 0x00005D39
	public static void LoadScene(string sceneName, bool noBlocker = false)
	{
		MonoSingleton<SceneHelper>.Instance.StartCoroutine(MonoSingleton<SceneHelper>.Instance.LoadSceneAsync(sceneName, noBlocker));
	}

	// Token: 0x06000172 RID: 370 RVA: 0x00007B52 File Offset: 0x00005D52
	private IEnumerator LoadSceneAsync(string sceneName, bool noSplash = false)
	{
		if (SceneHelper.PendingScene != null)
		{
			yield break;
		}
		SceneHelper.PendingScene = sceneName;
		sceneName = SceneHelper.SanitizeLevelPath(sceneName);
		if (!(sceneName == "Main Menu") && !(sceneName == "Tutorial") && !(sceneName == "Credits") && !(sceneName == "Endless"))
		{
			sceneName == "Custom Content";
		}
		Debug.Log("(LoadSceneAsync) Loading scene " + sceneName);
		this.loadingBlocker.SetActive(!noSplash);
		yield return null;
		if (SceneHelper.CurrentScene != sceneName)
		{
			SceneHelper.LastScene = SceneHelper.CurrentScene;
		}
		SceneHelper.CurrentScene = sceneName;
		if (MonoSingleton<MapVarManager>.Instance != null)
		{
			MonoSingleton<MapVarManager>.Instance.ReloadMapVars();
		}
		yield return Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Single, true, 100);
		if (GameStateManager.Instance)
		{
			GameStateManager.Instance.currentCustomGame = null;
		}
		if (this.preloadingBadge)
		{
			this.preloadingBadge.SetActive(false);
		}
		if (this.loadingBlocker)
		{
			this.loadingBlocker.SetActive(false);
		}
		if (this.loadingBar)
		{
			this.loadingBar.gameObject.SetActive(false);
		}
		SceneHelper.PendingScene = null;
		yield break;
	}

	// Token: 0x06000173 RID: 371 RVA: 0x00007B70 File Offset: 0x00005D70
	public static void RestartScene()
	{
		foreach (MonoBehaviour monoBehaviour in Object.FindObjectsOfType<MonoBehaviour>())
		{
			if (!(monoBehaviour == null) && !(monoBehaviour.gameObject.scene.name == "DontDestroyOnLoad"))
			{
				monoBehaviour.CancelInvoke();
				monoBehaviour.enabled = false;
			}
		}
		if (string.IsNullOrEmpty(SceneHelper.CurrentScene))
		{
			SceneHelper.CurrentScene = SceneManager.GetActiveScene().name;
		}
		Addressables.LoadSceneAsync(SceneHelper.CurrentScene, LoadSceneMode.Single, true, 100).WaitForCompletion();
		if (MonoSingleton<MapVarManager>.Instance != null)
		{
			MonoSingleton<MapVarManager>.Instance.ReloadMapVars();
		}
	}

	// Token: 0x06000174 RID: 372 RVA: 0x00007C18 File Offset: 0x00005E18
	public static void LoadPreviousScene()
	{
		string text = SceneHelper.LastScene;
		if (string.IsNullOrEmpty(text))
		{
			text = "Main Menu";
		}
		SceneHelper.LoadScene(text, false);
	}

	// Token: 0x06000175 RID: 373 RVA: 0x00007C40 File Offset: 0x00005E40
	public static void SpawnFinalPitAndFinish()
	{
		FinalRoom finalRoom = Object.FindObjectOfType<FinalRoom>();
		if (finalRoom != null)
		{
			if (finalRoom.doorOpener)
			{
				finalRoom.doorOpener.SetActive(true);
			}
			MonoSingleton<NewMovement>.Instance.transform.position = finalRoom.dropPoint.position;
			return;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(AssetHelper.LoadPrefab(MonoSingleton<SceneHelper>.Instance.finalRoomPit));
		finalRoom = gameObject.GetComponent<FinalRoom>();
		gameObject.transform.position = new Vector3(50000f, -1000f, 50000f);
		MonoSingleton<NewMovement>.Instance.transform.position = finalRoom.dropPoint.position;
	}

	// Token: 0x06000176 RID: 374 RVA: 0x00007CE3 File Offset: 0x00005EE3
	public static void SetLoadingSubtext(string text)
	{
		if (!MonoSingleton<SceneHelper>.Instance.loadingBlocker)
		{
			return;
		}
		MonoSingleton<SceneHelper>.Instance.loadingBar.gameObject.SetActive(true);
		MonoSingleton<SceneHelper>.Instance.loadingBar.text = text;
	}

	// Token: 0x06000177 RID: 375 RVA: 0x00007D1C File Offset: 0x00005F1C
	public int? GetLevelIndexAfterIntermission(string intermissionScene)
	{
		if (this.embeddedSceneInfo == null)
		{
			return null;
		}
		foreach (IntermissionRelation intermissionRelation in this.embeddedSceneInfo.intermissions)
		{
			if (intermissionRelation.intermissionScene == intermissionScene)
			{
				return new int?(intermissionRelation.nextLevelIndex);
			}
		}
		return null;
	}

	// Token: 0x0400013F RID: 319
	private static LayerMask? _footstepLayerMask;

	// Token: 0x04000140 RID: 320
	private readonly List<Material> reusableMaterials = new List<Material>();

	// Token: 0x04000141 RID: 321
	[SerializeField]
	private AssetReference finalRoomPit;

	// Token: 0x04000142 RID: 322
	[SerializeField]
	private GameObject loadingBlocker;

	// Token: 0x04000143 RID: 323
	[SerializeField]
	private TMP_Text loadingBar;

	// Token: 0x04000144 RID: 324
	[SerializeField]
	private GameObject preloadingBadge;

	// Token: 0x04000145 RID: 325
	[SerializeField]
	private GameObject eventSystem;

	// Token: 0x04000146 RID: 326
	[Space]
	[SerializeField]
	private AudioMixerGroup masterMixer;

	// Token: 0x04000147 RID: 327
	[SerializeField]
	private AudioMixerGroup musicMixer;

	// Token: 0x04000148 RID: 328
	[SerializeField]
	private AudioMixer allSound;

	// Token: 0x04000149 RID: 329
	[SerializeField]
	private AudioMixer goreSound;

	// Token: 0x0400014A RID: 330
	[SerializeField]
	private AudioMixer musicSound;

	// Token: 0x0400014B RID: 331
	[SerializeField]
	private AudioMixer doorSound;

	// Token: 0x0400014C RID: 332
	[SerializeField]
	private AudioMixer unfreezeableSound;

	// Token: 0x0400014D RID: 333
	[Space]
	[SerializeField]
	private EmbeddedSceneInfo embeddedSceneInfo;

	// Token: 0x04000151 RID: 337
	private Scene footstepScene;

	// Token: 0x04000152 RID: 338
	private PhysicsScene footstepPhysicsScene;

	// Token: 0x04000153 RID: 339
	public bool environmentalHitParticles = true;

	// Token: 0x04000154 RID: 340
	private readonly List<Color> reusableColors = new List<Color>();

	// Token: 0x04000155 RID: 341
	private readonly List<int> reusableTriangles = new List<int>();

	// Token: 0x02000048 RID: 72
	public struct HitSurfaceData
	{
		// Token: 0x06000179 RID: 377 RVA: 0x00007DB6 File Offset: 0x00005FB6
		public HitSurfaceData(SurfaceType surfaceType = SurfaceType.Generic, Color surfaceColor = default(Color))
		{
			this.material = null;
			this.hit = default(RaycastHit);
			this.mesh = null;
			this.useSecondaryBlend = false;
			this.surfaceType = SurfaceType.Generic;
			this.particleColor = default(Color);
		}

		// Token: 0x04000156 RID: 342
		public Material material;

		// Token: 0x04000157 RID: 343
		public RaycastHit hit;

		// Token: 0x04000158 RID: 344
		public Mesh mesh;

		// Token: 0x04000159 RID: 345
		public bool useSecondaryBlend;

		// Token: 0x0400015A RID: 346
		public SurfaceType surfaceType;

		// Token: 0x0400015B RID: 347
		public Color particleColor;
	}

	// Token: 0x02000049 RID: 73
	public readonly struct PhysicsSceneObjectData
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00007DEC File Offset: 0x00005FEC
		public Mesh Mesh { get; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00007DF4 File Offset: 0x00005FF4
		public Material[] Materials { get; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00007DFC File Offset: 0x00005FFC
		public Vector3 Position { get; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00007E04 File Offset: 0x00006004
		public Quaternion Rotation { get; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00007E0C File Offset: 0x0000600C
		public Vector3 Scale { get; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00007E14 File Offset: 0x00006014
		public int Layer { get; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00007E1C File Offset: 0x0000601C
		public string Name { get; }

		// Token: 0x06000181 RID: 385 RVA: 0x00007E24 File Offset: 0x00006024
		public PhysicsSceneObjectData(Mesh mesh, Material[] materials, Vector3 position, Quaternion rotation, Vector3 scale, int layer, string name)
		{
			this.Mesh = mesh;
			this.Materials = materials;
			this.Position = position;
			this.Rotation = rotation;
			this.Scale = scale;
			this.Layer = layer;
			this.Name = name;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00007E5C File Offset: 0x0000605C
		public static bool TryCreateFromObject(Transform transform, out SceneHelper.PhysicsSceneObjectData data, bool ignorePhysicsChecks = false)
		{
			data = default(SceneHelper.PhysicsSceneObjectData);
			MeshRenderer meshRenderer = null;
			Collider collider;
			if (!ignorePhysicsChecks && !SceneHelper.IsValidForPhysicsScene(transform, out meshRenderer, out collider))
			{
				return false;
			}
			if (ignorePhysicsChecks && !transform.TryGetComponent<MeshRenderer>(out meshRenderer))
			{
				return false;
			}
			Mesh mesh = null;
			PreservedOriginalMesh preservedOriginalMesh;
			MeshCollider meshCollider;
			MeshFilter meshFilter;
			if (transform.TryGetComponent<PreservedOriginalMesh>(out preservedOriginalMesh))
			{
				mesh = preservedOriginalMesh.mesh;
			}
			else if (transform.TryGetComponent<MeshCollider>(out meshCollider))
			{
				mesh = meshCollider.sharedMesh;
			}
			else if (transform.TryGetComponent<MeshFilter>(out meshFilter))
			{
				mesh = meshFilter.sharedMesh;
			}
			if (mesh == null || meshRenderer == null)
			{
				return false;
			}
			data = new SceneHelper.PhysicsSceneObjectData(mesh, meshRenderer.sharedMaterials, transform.position, transform.rotation, transform.lossyScale, transform.gameObject.layer, transform.name);
			return true;
		}
	}
}
