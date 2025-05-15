using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001F3 RID: 499
public class Flammable : MonoBehaviour
{
	// Token: 0x06000A28 RID: 2600 RVA: 0x000463F8 File Offset: 0x000445F8
	private void Start()
	{
		EnemyIdentifierIdentifier enemyIdentifierIdentifier;
		if (base.gameObject.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
		{
			this.enemy = true;
			this.eidid = enemyIdentifierIdentifier;
		}
		Breakable breakable;
		if (base.gameObject.TryGetComponent<Breakable>(out breakable))
		{
			this.breakable = breakable;
		}
		this.alwaysSimpleFire = MonoSingleton<PrefsManager>.Instance.GetBoolLocal("simpleFire", false);
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x0004644E File Offset: 0x0004464E
	private void OnEnable()
	{
		if (this.burning)
		{
			this.Pulse();
		}
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x00046460 File Offset: 0x00044660
	private bool IsLossyScaleInvalid()
	{
		if (base.transform == null)
		{
			return true;
		}
		Vector3 lossyScale = base.transform.lossyScale;
		return float.IsNaN(lossyScale.x) || float.IsNaN(lossyScale.y) || float.IsNaN(lossyScale.z) || float.IsInfinity(lossyScale.x) || float.IsInfinity(lossyScale.y) || float.IsInfinity(lossyScale.z) || lossyScale.x == 0f || lossyScale.y == 0f || lossyScale.z == 0f;
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x00046500 File Offset: 0x00044700
	public void Burn(float newHeat, bool noInstaDamage = false)
	{
		if (this.markedForDestroy)
		{
			return;
		}
		if (this.fuelOnly && this.fuel <= 0f)
		{
			return;
		}
		if (this.IsLossyScaleInvalid())
		{
			this.MarkForDestroy();
			return;
		}
		if (this.specialFlammable)
		{
			UnityEvent unityEvent = this.onSpecialActivate;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
			return;
		}
		else
		{
			if (this.wet || (this.enemy && this.eidid && this.eidid.eid && this.eidid.eid.blessed))
			{
				return;
			}
			if (this.col == null)
			{
				this.col = base.GetComponent<Collider>();
			}
			if (this.col == null)
			{
				this.MarkForDestroy();
				return;
			}
			if (this.fuelOnly)
			{
				this.heat = 0.1f;
			}
			else if (newHeat > this.heat)
			{
				this.heat = newHeat;
			}
			if (this.currentFire == null)
			{
				Bounds bounds = this.col.bounds;
				this.currentFire = MonoSingleton<FireObjectPool>.Instance.GetFire(this.secondary || this.alwaysSimpleFire);
				this.currentFire.transform.SetParent(base.transform);
				this.currentFire.transform.position = bounds.center;
				Vector3 lossyScale = base.transform.lossyScale;
				this.currentFire.transform.localScale = new Vector3(bounds.size.x / lossyScale.x, bounds.size.y / lossyScale.y, bounds.size.z / lossyScale.z);
				this.currentFireAud = this.currentFire.GetComponentInChildren<AudioSource>();
				if (!this.secondary && !this.alwaysSimpleFire)
				{
					this.currentFireLight = this.currentFire.GetComponent<Light>();
					this.currentFireLight.enabled = true;
				}
			}
			if (this.eidid && this.eidid.eid && !this.eidid.eid.burners.Contains(this))
			{
				this.eidid.eid.burners.Add(this);
			}
			if (this.enemy)
			{
				this.burning = true;
				if (this.eidid.eid.burners.Count == 1)
				{
					this.eidid.eid.Burn();
				}
			}
			if (this.breakable != null)
			{
				this.breakable.Burn();
			}
			if (!this.secondary)
			{
				this.flammables = base.GetComponentsInChildren<Flammable>();
				foreach (Flammable flammable in this.flammables)
				{
					if (flammable != this)
					{
						flammable.secondary = true;
						flammable.Burn(this.heat, false);
						flammable.Pulse();
					}
				}
			}
			this.burning = true;
			return;
		}
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x000467E8 File Offset: 0x000449E8
	public void Pulse()
	{
		if (this.markedForDestroy)
		{
			return;
		}
		if (this.IsLossyScaleInvalid())
		{
			this.MarkForDestroy();
			return;
		}
		if (this.burning)
		{
			if (this.fuel >= 0.175f)
			{
				this.fuel -= 0.175f;
			}
			else if (this.fuel > 0f)
			{
				this.heat = 0.175f - this.fuel;
				this.fuel = 0f;
			}
			else
			{
				this.heat -= 0.25f;
			}
			if (this.heat <= 0f)
			{
				this.burning = false;
				this.fading = true;
				base.Invoke("Pulse", Random.Range(0.25f, 0.5f));
				return;
			}
			if (!this.enemy)
			{
				base.Invoke("Pulse", 0.5f);
				this.TryIgniteGasoline();
				return;
			}
		}
		else if (this.fading && this.currentFire != null)
		{
			if (this.fuel > 0f)
			{
				this.Burn(0.1f, false);
				base.CancelInvoke("Pulse");
				this.currentFire.transform.localScale = new Vector3(this.col.bounds.size.x / base.transform.lossyScale.x, this.col.bounds.size.y / base.transform.lossyScale.y, this.col.bounds.size.z / base.transform.lossyScale.z);
				return;
			}
			if (this.currentFire != null)
			{
				this.currentFire.transform.localScale *= 0.75f;
				if (this.currentFireAud == null)
				{
					this.currentFireAud = this.currentFire.GetComponentInChildren<AudioSource>();
				}
				this.currentFireAud.volume *= 0.75f;
				if (!this.secondary && this.currentFireLight != null)
				{
					this.currentFireLight.range *= 0.75f;
				}
			}
			if (this.currentFire.transform.localScale.x < 0.1f)
			{
				this.fading = false;
				this.ReturnToQueue();
				return;
			}
			base.Invoke("Pulse", Random.Range(0.25f, 0.5f));
		}
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x00033EF0 File Offset: 0x000320F0
	private void TryIgniteGasoline()
	{
		MonoSingleton<StainVoxelManager>.Instance.TryIgniteAt(base.transform.position, 3);
	}

	// Token: 0x06000A2E RID: 2606 RVA: 0x00046A7C File Offset: 0x00044C7C
	public void PutOut(bool getWet = true)
	{
		this.wet = getWet;
		if (this.currentFire)
		{
			this.heat = 0f;
			this.burning = false;
			this.fading = false;
			this.ReturnToQueue();
		}
		if (!this.secondary && this.flammables != null)
		{
			foreach (Flammable flammable in this.flammables)
			{
				if (flammable != this)
				{
					flammable.PutOut(true);
				}
			}
		}
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x00046AF8 File Offset: 0x00044CF8
	public void MarkForDestroy()
	{
		this.markedForDestroy = true;
		this.ReturnToQueue();
		if (this.fuelOnly)
		{
			Object.Destroy(this, Random.Range(0.001f, 0.01f));
			return;
		}
		EnemyIdentifierIdentifier enemyIdentifierIdentifier;
		if (base.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
		{
			enemyIdentifierIdentifier.Invoke("DestroyLimb", Random.Range(0.001f, 0.01f));
			return;
		}
		Object.Destroy(base.gameObject, Random.Range(0.001f, 0.01f));
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x00046B6F File Offset: 0x00044D6F
	public void ReturnToQueue()
	{
		if (this.currentFire != null)
		{
			MonoSingleton<FireObjectPool>.Instance.ReturnFire(this.currentFire, this.secondary || this.alwaysSimpleFire);
			this.currentFire = null;
		}
	}

	// Token: 0x04000D3D RID: 3389
	public float heat;

	// Token: 0x04000D3E RID: 3390
	public float fuel;

	// Token: 0x04000D3F RID: 3391
	private GameObject currentFire;

	// Token: 0x04000D40 RID: 3392
	private AudioSource currentFireAud;

	// Token: 0x04000D41 RID: 3393
	private Light currentFireLight;

	// Token: 0x04000D42 RID: 3394
	public bool burning;

	// Token: 0x04000D43 RID: 3395
	private bool fading;

	// Token: 0x04000D44 RID: 3396
	public bool secondary;

	// Token: 0x04000D45 RID: 3397
	public bool fuelOnly;

	// Token: 0x04000D46 RID: 3398
	private bool enemy;

	// Token: 0x04000D47 RID: 3399
	private EnemyIdentifierIdentifier eidid;

	// Token: 0x04000D48 RID: 3400
	private Flammable[] flammables;

	// Token: 0x04000D49 RID: 3401
	public bool wet;

	// Token: 0x04000D4A RID: 3402
	private Breakable breakable;

	// Token: 0x04000D4B RID: 3403
	public bool playerOnly;

	// Token: 0x04000D4C RID: 3404
	public bool enemyOnly;

	// Token: 0x04000D4D RID: 3405
	public bool specialFlammable;

	// Token: 0x04000D4E RID: 3406
	public UnityEvent onSpecialActivate;

	// Token: 0x04000D4F RID: 3407
	private Collider col;

	// Token: 0x04000D50 RID: 3408
	private bool alwaysSimpleFire;

	// Token: 0x04000D51 RID: 3409
	private bool markedForDestroy;
}
