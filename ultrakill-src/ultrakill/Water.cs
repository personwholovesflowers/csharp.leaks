using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020004BB RID: 1211
public class Water : MonoBehaviour
{
	// Token: 0x06001BBE RID: 7102 RVA: 0x000E6438 File Offset: 0x000E4638
	private void Start()
	{
		this.waterStore = MonoSingleton<PooledWaterStore>.Instance;
		this.lookUp = Quaternion.LookRotation(Vector3.up);
		this.waterColliders = base.GetComponentsInChildren<Collider>();
		if (this.fishDB)
		{
			this.fishDB.SetupWater(this);
		}
	}

	// Token: 0x06001BBF RID: 7103 RVA: 0x000E6485 File Offset: 0x000E4685
	private void OnEnable()
	{
		this.dzc = MonoSingleton<DryZoneController>.Instance;
		this.dzc.waters.Add(this);
	}

	// Token: 0x06001BC0 RID: 7104 RVA: 0x000E64A4 File Offset: 0x000E46A4
	private void OnDisable()
	{
		this.Cleanup();
	}

	// Token: 0x06001BC1 RID: 7105 RVA: 0x000E64A4 File Offset: 0x000E46A4
	private void OnDestroy()
	{
		this.Cleanup();
	}

	// Token: 0x06001BC2 RID: 7106 RVA: 0x000E64AC File Offset: 0x000E46AC
	private void Cleanup()
	{
		if (!base.gameObject.scene.isLoaded)
		{
			return;
		}
		this.toRemove.Clear();
		this.toRemove.UnionWith(this.tracked.Keys);
		foreach (Collider collider in this.toRemove)
		{
			this.RemoveFromWater(collider, false);
		}
		this.tracked.Clear();
		this.dzc = MonoSingleton<DryZoneController>.Instance;
		this.dzc.waters.Remove(this);
	}

	// Token: 0x06001BC3 RID: 7107 RVA: 0x000E6560 File Offset: 0x000E4760
	private void FixedUpdate()
	{
		if (this.doneThisFrame)
		{
			foreach (WaterObject waterObject in this.tracked.Values)
			{
				if (waterObject.IsPlayer && !this.IsCollidingWithWater(waterObject.Col))
				{
					this.toRemove.Add(waterObject.Col);
				}
				this.ApplyWaterForces(waterObject, true);
			}
			return;
		}
		this.gravity = Physics.gravity;
		this.scaledGravityY = this.gravity.y * 0.2f;
		this.waterColData.Clear();
		this.waterCount = this.waterColliders.Length;
		for (int i = 0; i < this.waterCount; i++)
		{
			Collider collider = this.waterColliders[i];
			if (collider)
			{
				Bounds bounds = collider.bounds;
				Water.WaterColData waterColData = new Water.WaterColData
				{
					maxHeight = bounds.max.y,
					minHeight = bounds.min.y,
					position = collider.transform.position,
					rotation = collider.transform.rotation
				};
				this.waterColData.Add(waterColData);
			}
		}
		foreach (WaterObject waterObject2 in this.tracked.Values)
		{
			if (!this.MarkIfStaleObject(waterObject2))
			{
				Vector3 vector;
				bool flag;
				if ((waterObject2.IsPlayer || waterObject2.IsEnemy) && this.TryGetSurfaceAndIsSubmerged(waterObject2.Col, out vector, out flag))
				{
					if (waterObject2.IsEnemy)
					{
						if (flag != waterObject2.EID.underwater)
						{
							waterObject2.EID.underwater = flag;
							waterObject2.BubbleEffect.SetActive(flag);
							for (int j = 0; j < waterObject2.LowPassFilters.Count; j++)
							{
								waterObject2.LowPassFilters[j].enabled = flag;
							}
						}
						if (flag)
						{
							this.KillStreetCleaner(waterObject2);
						}
					}
					this.UpdateContinuousSplash(waterObject2, vector, flag);
				}
				if (waterObject2.IsUWC && !this.IsCollidingWithWater(waterObject2.Col))
				{
					this.toRemove.Add(waterObject2.Col);
				}
				if (waterObject2.IsEnemy && waterObject2.EID.dead)
				{
					this.waterStore.ReturnToQueue(waterObject2.BubbleEffect, Water.WaterGOType.bubble);
				}
				this.ApplyWaterForces(waterObject2, false);
			}
		}
		foreach (Collider collider2 in this.toRemove)
		{
			this.RemoveFromWater(collider2, false);
		}
		this.toRemove.Clear();
		this.doneThisFrame = true;
	}

	// Token: 0x06001BC4 RID: 7108 RVA: 0x000E688C File Offset: 0x000E4A8C
	private bool MarkIfStaleObject(WaterObject wObj)
	{
		if (!wObj.Col || !wObj.Col.enabled || !wObj.Rb || !wObj.rbGO.activeInHierarchy)
		{
			this.toRemove.Add(wObj.Col);
			return true;
		}
		return false;
	}

	// Token: 0x06001BC5 RID: 7109 RVA: 0x000E68E2 File Offset: 0x000E4AE2
	private void Update()
	{
		this.doneThisFrame = false;
	}

	// Token: 0x06001BC6 RID: 7110 RVA: 0x000E68EC File Offset: 0x000E4AEC
	private void OnTriggerEnter(Collider other)
	{
		Rigidbody attachedRigidbody = other.attachedRigidbody;
		if (!other || !attachedRigidbody)
		{
			return;
		}
		if (this.dzc && this.dzc.colliderCalls.ContainsKey(other))
		{
			return;
		}
		if (this.tracked.ContainsKey(other))
		{
			return;
		}
		bool flag = false;
		if (other.isTrigger)
		{
			UnderwaterController underwaterController;
			if (!attachedRigidbody.CompareTag("Player") || !other.TryGetComponent<UnderwaterController>(out underwaterController))
			{
				return;
			}
			flag = true;
		}
		GameObject gameObject = attachedRigidbody.gameObject;
		int layer = other.gameObject.layer;
		WaterObject waterObject = new WaterObject
		{
			Rb = attachedRigidbody,
			rbGO = gameObject,
			Col = other,
			EnterVelocity = attachedRigidbody.velocity,
			IsUWC = flag,
			IsPlayer = other.CompareTag("Player"),
			IsEnemy = (layer == 12),
			Layer = layer,
			EID = attachedRigidbody.GetComponent<EnemyIdentifier>()
		};
		this.tracked.Add(other, waterObject);
		if (waterObject.IsPlayer)
		{
			this.isPlayerTouchingWater = true;
			MonoSingleton<NewMovement>.Instance.touchingWaters.Add(this);
		}
		else if (waterObject.IsUWC)
		{
			this.isPlayerUnderWater = true;
			UnderwaterController instance = MonoSingleton<UnderwaterController>.Instance;
			if (instance)
			{
				instance.EnterWater(this);
			}
			this.SpawnBubbles(waterObject);
		}
		else
		{
			if (waterObject.IsEnemy && waterObject.EID)
			{
				waterObject.EID.touchingWaters.Add(this);
				if (!this.simplifyWaterProcessing && !this.notWet)
				{
					this.SpawnBubbles(waterObject);
				}
			}
			if (!this.notWet)
			{
				this.AddLowPassFilters(waterObject);
				this.ExtinguishFires(other);
				this.MarkObjectWet(waterObject);
			}
		}
		this.TrySpawnSplash(waterObject);
	}

	// Token: 0x06001BC7 RID: 7111 RVA: 0x000E6AA7 File Offset: 0x000E4CA7
	private void OnTriggerExit(Collider other)
	{
		if (this.dzc && this.dzc.colliderCalls.ContainsKey(other))
		{
			return;
		}
		this.RemoveFromWater(other, true);
	}

	// Token: 0x06001BC8 RID: 7112 RVA: 0x000E6AD2 File Offset: 0x000E4CD2
	public void EnterDryZone(Collider other)
	{
		this.RemoveFromWater(other, false);
	}

	// Token: 0x06001BC9 RID: 7113 RVA: 0x000E6ADC File Offset: 0x000E4CDC
	public void ExitDryZone(Collider other)
	{
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		if (this.IsCollidingWithWater(other))
		{
			this.OnTriggerEnter(other);
		}
	}

	// Token: 0x06001BCA RID: 7114 RVA: 0x000E6AF8 File Offset: 0x000E4CF8
	public bool IsCollidingWithWater(Collider other)
	{
		Transform transform = other.transform;
		for (int i = 0; i < this.waterCount; i++)
		{
			Collider collider = this.waterColliders[i];
			Vector3 vector;
			float num;
			if (Physics.ComputePenetration(other, transform.position, transform.rotation, collider, this.waterColData[i].position, this.waterColData[i].rotation, out vector, out num))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001BCB RID: 7115 RVA: 0x000E6B64 File Offset: 0x000E4D64
	private void RemoveFromWater(Collider col, bool wasOnTriggerExit)
	{
		if (wasOnTriggerExit && this.waterCount > 1 && this.IsCollidingWithWater(col))
		{
			return;
		}
		WaterObject waterObject;
		if (!this.tracked.TryGetValue(col, out waterObject))
		{
			return;
		}
		if (wasOnTriggerExit && col && waterObject.Rb)
		{
			this.TrySpawnSplash(waterObject);
		}
		this.CleanupWaterEffects(waterObject);
		if (waterObject.IsPlayer)
		{
			if (this.isPlayerTouchingWater)
			{
				this.isPlayerTouchingWater = false;
				MonoSingleton<NewMovement>.Instance.touchingWaters.Remove(this);
			}
		}
		else if (waterObject.IsUWC)
		{
			this.isPlayerUnderWater = false;
			UnderwaterController instance = MonoSingleton<UnderwaterController>.Instance;
			if (instance)
			{
				instance.OutWater(this);
			}
		}
		else if (waterObject.IsEnemy && waterObject.EID)
		{
			waterObject.EID.underwater = false;
			waterObject.EID.touchingWaters.Remove(this);
		}
		this.tracked.Remove(col);
	}

	// Token: 0x06001BCC RID: 7116 RVA: 0x000E6C50 File Offset: 0x000E4E50
	private void ApplyWaterForces(WaterObject wObj, bool doBackupCheck)
	{
		if (wObj.IsUWC)
		{
			return;
		}
		Rigidbody rb = wObj.Rb;
		if (doBackupCheck && (!rb || !wObj.rbGO.activeInHierarchy))
		{
			this.toRemove.Add(wObj.Col);
			return;
		}
		if (rb.useGravity && !rb.isKinematic)
		{
			Vector3 velocity = rb.velocity;
			if (velocity.y < this.scaledGravityY)
			{
				Vector3 vector = new Vector3(velocity.x, this.scaledGravityY, velocity.z);
				rb.velocity = Vector3.MoveTowards(velocity, vector, Time.fixedDeltaTime * 10f * Mathf.Abs(velocity.y - this.scaledGravityY + 0.5f));
				return;
			}
			if (wObj.Layer == 10 || wObj.Layer == 9)
			{
				rb.AddForce(this.gravity * (rb.mass * -0.45f));
				return;
			}
			rb.AddForce(this.gravity * (rb.mass * -0.75f));
		}
	}

	// Token: 0x06001BCD RID: 7117 RVA: 0x000E6D60 File Offset: 0x000E4F60
	private void UpdateContinuousSplash(WaterObject wObj, Vector3 surfacePoint, bool isSubmerged)
	{
		if (wObj.ContinuousSplashEffect)
		{
			wObj.ContinuousSplashEffect.position = surfacePoint;
			if (isSubmerged)
			{
				wObj.ContinuousSplashEffect.GetComponent<SplashContinuous>().ReturnSoon();
				wObj.ContinuousSplashEffect = null;
				return;
			}
		}
		else
		{
			if (isSubmerged)
			{
				return;
			}
			GameObject fromQueue = this.waterStore.GetFromQueue(Water.WaterGOType.continuous);
			fromQueue.transform.SetPositionAndRotation(surfacePoint, this.lookUp);
			fromQueue.transform.localScale = 3f * wObj.Col.bounds.size.magnitude * Vector3.one;
			SplashContinuous component = fromQueue.GetComponent<SplashContinuous>();
			NavMeshAgent navMeshAgent;
			if (wObj.IsEnemy && wObj.Col.TryGetComponent<NavMeshAgent>(out navMeshAgent))
			{
				component.nma = navMeshAgent;
			}
			wObj.ContinuousSplashEffect = fromQueue.transform;
			wObj.ContinuousSplashEffect.position = surfacePoint;
		}
	}

	// Token: 0x06001BCE RID: 7118 RVA: 0x000E6E3C File Offset: 0x000E503C
	private bool TryGetSurfaceAndIsSubmerged(Collider col, out Vector3 surfacePoint, out bool isSubmerged)
	{
		surfacePoint = Vector3.zero;
		isSubmerged = false;
		Vector3 position = col.transform.position;
		for (int i = 0; i < this.waterCount; i++)
		{
			Collider collider = this.waterColliders[i];
			if (!(collider == null) && Vector3.Distance(collider.ClosestPoint(position), position) < 1f)
			{
				Water.WaterColData waterColData = this.waterColData[i];
				Vector3 vector = new Vector3(position.x, waterColData.maxHeight + 0.1f, position.z);
				float num = waterColData.maxHeight - waterColData.minHeight;
				RaycastHit raycastHit;
				if (Physics.Raycast(vector, Vector3.down, out raycastHit, num, 16, QueryTriggerInteraction.Collide))
				{
					surfacePoint = raycastHit.point;
					Bounds bounds = col.bounds;
					float y = bounds.max.y;
					float y2 = bounds.min.y;
					if (y - (y - y2) / 3f < surfacePoint.y)
					{
						isSubmerged = true;
					}
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06001BCF RID: 7119 RVA: 0x000E6F3C File Offset: 0x000E513C
	private void TrySpawnSplash(WaterObject wObj)
	{
		Rigidbody rb = wObj.Rb;
		int layer = wObj.Layer;
		Water.WaterGOType waterGOType = Water.WaterGOType.none;
		if ((rb.velocity.y < -25f || layer == 11) && rb.mass >= 1f && layer != 10 && layer != 9)
		{
			waterGOType = Water.WaterGOType.big;
		}
		else if (!rb.isKinematic)
		{
			waterGOType = Water.WaterGOType.small;
		}
		if (waterGOType == Water.WaterGOType.none)
		{
			return;
		}
		Vector3 position = rb.transform.position;
		Vector3 vector = Vector3.positiveInfinity;
		float num = float.PositiveInfinity;
		for (int i = 0; i < this.waterCount; i++)
		{
			Collider collider = this.waterColliders[i];
			Vector3 vector2 = new Vector3(position.x, this.waterColData[i].maxHeight, position.z);
			Vector3 vector3 = collider.ClosestPointOnBounds(vector2);
			float num2 = Vector3.Distance(vector3, position);
			if (num2 < num)
			{
				vector = vector3;
				num = num2;
			}
		}
		if (Vector3.Distance(vector, wObj.Col.ClosestPoint(vector)) < 1f)
		{
			GameObject fromQueue = this.waterStore.GetFromQueue(waterGOType);
			PooledSplash pooledSplash;
			if (wObj.IsPlayer && fromQueue.TryGetComponent<PooledSplash>(out pooledSplash) && wObj.IsPlayer)
			{
				pooledSplash.defaultPitch = 0.45f;
			}
			Transform transform = fromQueue.transform;
			transform.SetPositionAndRotation(vector, this.lookUp);
			transform.localScale = 3f * wObj.Col.bounds.size.magnitude * Vector3.one;
		}
	}

	// Token: 0x06001BD0 RID: 7120 RVA: 0x000E70AE File Offset: 0x000E52AE
	public GameObject SpawnBasicSplash(Water.WaterGOType type)
	{
		return this.waterStore.GetFromQueue(type);
	}

	// Token: 0x06001BD1 RID: 7121 RVA: 0x000E70BC File Offset: 0x000E52BC
	private void AddLowPassFilters(WaterObject wObj)
	{
		if (wObj.Layer == 11 || wObj.Layer == 12)
		{
			return;
		}
		GameObject rbGO = wObj.rbGO;
		BloodUnderwaterChecker bloodUnderwaterChecker;
		if (wObj.Layer == 0 && rbGO.TryGetComponent<BloodUnderwaterChecker>(out bloodUnderwaterChecker))
		{
			wObj.AudioSources = rbGO.transform.parent.GetComponentsInChildren<AudioSource>();
		}
		else
		{
			wObj.AudioSources = rbGO.GetComponentsInChildren<AudioSource>();
		}
		foreach (AudioSource audioSource in wObj.AudioSources)
		{
			if (audioSource)
			{
				AudioLowPassFilter audioLowPassFilter;
				if (!audioSource.TryGetComponent<AudioLowPassFilter>(out audioLowPassFilter))
				{
					audioLowPassFilter = audioSource.gameObject.AddComponent<AudioLowPassFilter>();
				}
				audioLowPassFilter.cutoffFrequency = 1000f;
				audioLowPassFilter.lowpassResonanceQ = 1f;
				if (wObj.IsEnemy)
				{
					audioLowPassFilter.enabled = false;
				}
				wObj.LowPassFilters.Add(audioLowPassFilter);
			}
		}
	}

	// Token: 0x06001BD2 RID: 7122 RVA: 0x000E718C File Offset: 0x000E538C
	private void RemoveLowPassFilters(WaterObject wObj)
	{
		for (int i = 0; i < wObj.LowPassFilters.Count; i++)
		{
			AudioLowPassFilter audioLowPassFilter = wObj.LowPassFilters[i];
			if (audioLowPassFilter)
			{
				Object.Destroy(audioLowPassFilter);
			}
		}
		wObj.LowPassFilters.Clear();
	}

	// Token: 0x06001BD3 RID: 7123 RVA: 0x000E71D8 File Offset: 0x000E53D8
	private void SpawnBubbles(WaterObject wObj)
	{
		GameObject fromQueue = this.waterStore.GetFromQueue(Water.WaterGOType.bubble);
		if (wObj.IsUWC)
		{
			fromQueue.transform.SetPositionAndRotation(wObj.Rb.transform.position, this.lookUp);
		}
		else
		{
			fromQueue.transform.SetPositionAndRotation(wObj.Col.bounds.center, this.lookUp);
			fromQueue.SetActive(false);
		}
		fromQueue.transform.SetParent(wObj.Rb.transform, true);
		wObj.BubbleEffect = fromQueue;
	}

	// Token: 0x06001BD4 RID: 7124 RVA: 0x000E7268 File Offset: 0x000E5468
	private void ExtinguishFires(Collider col)
	{
		Flammable[] componentsInChildren = col.GetComponentsInChildren<Flammable>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].PutOut(true);
		}
	}

	// Token: 0x06001BD5 RID: 7125 RVA: 0x000E7294 File Offset: 0x000E5494
	private void MarkObjectWet(WaterObject wObj)
	{
		if (wObj.Layer == 9 || wObj.Layer == 10)
		{
			return;
		}
		Wet wet;
		if (wObj.Col.TryGetComponent<Wet>(out wet))
		{
			wet.Refill();
			return;
		}
		wObj.Col.gameObject.AddComponent<Wet>();
	}

	// Token: 0x06001BD6 RID: 7126 RVA: 0x000E72E0 File Offset: 0x000E54E0
	private void KillStreetCleaner(WaterObject wObj)
	{
		EnemyIdentifier eid = wObj.EID;
		if (eid.enemyType == EnemyType.Streetcleaner && !eid.dead)
		{
			eid.InstaKill();
		}
	}

	// Token: 0x06001BD7 RID: 7127 RVA: 0x000E730C File Offset: 0x000E550C
	private void CleanupWaterEffects(WaterObject wObj)
	{
		if (wObj.BubbleEffect)
		{
			this.waterStore.ReturnToQueue(wObj.BubbleEffect, Water.WaterGOType.bubble);
		}
		if (wObj.ContinuousSplashEffect)
		{
			SplashContinuous splashContinuous;
			if (wObj.ContinuousSplashEffect.TryGetComponent<SplashContinuous>(out splashContinuous))
			{
				splashContinuous.ReturnSoon();
			}
			wObj.ContinuousSplashEffect = null;
		}
		this.RemoveLowPassFilters(wObj);
		if (!this.notWet && !wObj.IsPlayer && !wObj.IsUWC && wObj.Rb && wObj.Layer != 10 && wObj.Layer != 9)
		{
			wObj.rbGO.GetOrAddComponent<Wet>().Dry(wObj.Rb.centerOfMass);
		}
	}

	// Token: 0x06001BD8 RID: 7128 RVA: 0x000E73BC File Offset: 0x000E55BC
	public void UpdateColor(Color newColor)
	{
		this.clr = newColor;
		if (this.isPlayerUnderWater)
		{
			UnderwaterController instance = MonoSingleton<UnderwaterController>.Instance;
			if (instance)
			{
				instance.UpdateColor(newColor);
			}
		}
	}

	// Token: 0x04002714 RID: 10004
	private Dictionary<Collider, WaterObject> tracked = new Dictionary<Collider, WaterObject>();

	// Token: 0x04002715 RID: 10005
	[Header("Visual/FX")]
	public Color clr = new Color(0f, 0.5f, 1f);

	// Token: 0x04002716 RID: 10006
	public bool notWet;

	// Token: 0x04002717 RID: 10007
	public bool simplifyWaterProcessing;

	// Token: 0x04002718 RID: 10008
	[Header("References (Optional)")]
	public FishDB fishDB;

	// Token: 0x04002719 RID: 10009
	public Transform overrideFishingPoint;

	// Token: 0x0400271A RID: 10010
	public FishObject[] attractFish;

	// Token: 0x0400271B RID: 10011
	private Collider[] waterColliders;

	// Token: 0x0400271C RID: 10012
	private bool isPlayerUnderWater;

	// Token: 0x0400271D RID: 10013
	[HideInInspector]
	public bool isPlayerTouchingWater;

	// Token: 0x0400271E RID: 10014
	private DryZoneController dzc;

	// Token: 0x0400271F RID: 10015
	private HashSet<Collider> toRemove = new HashSet<Collider>();

	// Token: 0x04002720 RID: 10016
	private List<Water.WaterColData> waterColData = new List<Water.WaterColData>();

	// Token: 0x04002721 RID: 10017
	private int waterCount;

	// Token: 0x04002722 RID: 10018
	private Vector3 gravity;

	// Token: 0x04002723 RID: 10019
	private float scaledGravityY;

	// Token: 0x04002724 RID: 10020
	private Quaternion lookUp;

	// Token: 0x04002725 RID: 10021
	private bool doneThisFrame;

	// Token: 0x04002726 RID: 10022
	private PooledWaterStore waterStore;

	// Token: 0x020004BC RID: 1212
	public struct WaterColData
	{
		// Token: 0x04002727 RID: 10023
		public float maxHeight;

		// Token: 0x04002728 RID: 10024
		public float minHeight;

		// Token: 0x04002729 RID: 10025
		public Vector3 position;

		// Token: 0x0400272A RID: 10026
		public Quaternion rotation;
	}

	// Token: 0x020004BD RID: 1213
	public enum WaterGOType
	{
		// Token: 0x0400272C RID: 10028
		none,
		// Token: 0x0400272D RID: 10029
		small,
		// Token: 0x0400272E RID: 10030
		big,
		// Token: 0x0400272F RID: 10031
		continuous,
		// Token: 0x04002730 RID: 10032
		bubble,
		// Token: 0x04002731 RID: 10033
		wetparticle
	}
}
