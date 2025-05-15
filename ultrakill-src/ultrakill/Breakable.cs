using System;
using Sandbox;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x02000093 RID: 147
public class Breakable : MonoBehaviour, IAlter, IAlterOptions<bool>
{
	// Token: 0x060002D6 RID: 726 RVA: 0x00010A38 File Offset: 0x0000EC38
	private void Start()
	{
		this.defaultHeight = base.transform.localScale.y;
		this.originalBounceHealth = this.bounceHealth;
		if ((this.breakParticle == null || this.breakParticle.Equals(null) || SceneHelper.IsPlayingCustom) && this.breakParticleFallback != null && this.breakParticleFallback.RuntimeKeyIsValid())
		{
			this.breakParticle = this.breakParticleFallback.ToAsset();
		}
		if (this.breakOnThrown)
		{
			this.rb = base.GetComponent<Rigidbody>();
			this.itid = base.GetComponent<ItemIdentifier>();
		}
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x00010AD0 File Offset: 0x0000ECD0
	public void Bounce()
	{
		if (this.originalBounceHealth > 0 && this.crateCoin && (this.col || base.TryGetComponent<Collider>(out this.col)))
		{
			Object.Instantiate<GameObject>(this.crateCoin, this.col.bounds.center, Quaternion.identity, GoreZone.ResolveGoreZone(base.transform).transform);
		}
		if (this.bounceHealth > 1)
		{
			base.transform.localScale = new Vector3(base.transform.localScale.x, this.defaultHeight / 4f, base.transform.localScale.z);
			this.bounceHealth--;
			return;
		}
		this.Break();
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x00010B9C File Offset: 0x0000ED9C
	private void Update()
	{
		TimeSince? timeSince = this.timeSinceBurn;
		float? num = ((timeSince != null) ? new float?(timeSince.GetValueOrDefault()) : null);
		float num2 = 3f;
		if ((num.GetValueOrDefault() > num2) & (num != null))
		{
			this.Break();
		}
		if (!this.crate)
		{
			return;
		}
		if (base.transform.localScale.y != this.defaultHeight)
		{
			base.transform.localScale = new Vector3(base.transform.localScale.x, Mathf.MoveTowards(base.transform.localScale.y, this.defaultHeight, Time.deltaTime * 10f), base.transform.localScale.z);
		}
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x00010C70 File Offset: 0x0000EE70
	private void OnTriggerEnter(Collider other)
	{
		if (this.breakOnEnvironment && LayerMaskDefaults.IsMatchingLayer(other.gameObject.layer, LMD.Environment))
		{
			this.Break();
		}
		if (!this.breakOnTouch)
		{
			return;
		}
		int layer = other.gameObject.layer;
		if (other.gameObject == MonoSingleton<NewMovement>.Instance.gameObject || layer == 10 || layer == 11 || layer == 12 || layer == 14)
		{
			this.Break();
		}
	}

	// Token: 0x060002DA RID: 730 RVA: 0x00010CE4 File Offset: 0x0000EEE4
	private void OnCollisionEnter(Collision collision)
	{
		if (this.breakOnTouch)
		{
			int layer = collision.gameObject.layer;
			if (collision.gameObject == MonoSingleton<NewMovement>.Instance.gameObject || (!this.playerOnly && (layer == 10 || layer == 11 || layer == 12 || layer == 14)))
			{
				this.Break();
			}
		}
		if (this.breakOnEnvironment && LayerMaskDefaults.IsMatchingLayer(collision.gameObject.layer, LMD.Environment))
		{
			this.Break();
		}
		if (!this.breakOnThrown || !this.itid)
		{
			return;
		}
		if (!this.itid.pickedUp && this.itid.beenPickedUp && (this.rb == null || !this.rb.isKinematic) && collision.gameObject != MonoSingleton<NewMovement>.Instance.gameObject)
		{
			this.Break();
		}
	}

	// Token: 0x060002DB RID: 731 RVA: 0x00010DC8 File Offset: 0x0000EFC8
	private void HitWith(GameObject target)
	{
		if (!this.breakOnThrown || !this.itid)
		{
			return;
		}
		if (target.layer == 10 || target.layer == 11)
		{
			if (MonoSingleton<FistControl>.Instance.heldObject == this.itid)
			{
				MonoSingleton<FistControl>.Instance.currentPunch.ResetHeldState();
			}
			base.transform.position = target.transform.position;
			this.Break();
		}
	}

	// Token: 0x060002DC RID: 732 RVA: 0x00010E44 File Offset: 0x0000F044
	public void Burn()
	{
		if (this.weak)
		{
			this.Break();
			return;
		}
		if (this.unbreakable || this.broken)
		{
			return;
		}
		if (this.timeSinceBurn == null)
		{
			this.timeSinceBurn = new TimeSince?(0f);
		}
	}

	// Token: 0x060002DD RID: 733 RVA: 0x00010E93 File Offset: 0x0000F093
	public void ForceBreak()
	{
		this.unbreakable = false;
		this.Break();
	}

	// Token: 0x060002DE RID: 734 RVA: 0x00010EA4 File Offset: 0x0000F0A4
	public void Break()
	{
		this.timeSinceBurn = null;
		if (this.unbreakable || this.broken)
		{
			return;
		}
		SandboxProp sandboxProp;
		Rigidbody rigidbody;
		if (base.TryGetComponent<SandboxProp>(out sandboxProp) && base.TryGetComponent<Rigidbody>(out rigidbody) && rigidbody.isKinematic && MonoSingleton<SandboxNavmesh>.Instance)
		{
			MonoSingleton<SandboxNavmesh>.Instance.MarkAsDirty(sandboxProp);
		}
		this.broken = true;
		if (this.breakParticle != null)
		{
			Vector3 vector = base.transform.position;
			if (this.particleAtBoundsCenter && (this.col || base.TryGetComponent<Collider>(out this.col)))
			{
				vector = this.col.bounds.center;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(this.breakParticle, vector, base.transform.rotation);
			if (this.customPositionRotation != null)
			{
				gameObject.transform.SetPositionAndRotation(this.customPositionRotation.position, this.customPositionRotation.rotation);
			}
			if (this.applyScaleToParticle)
			{
				gameObject.transform.localScale = base.transform.lossyScale;
			}
		}
		if (this.crate)
		{
			MonoSingleton<CrateCounter>.Instance.AddCrate();
			if (this.crateCoin && this.coinAmount > 0 && (this.col || base.TryGetComponent<Collider>(out this.col)))
			{
				for (int i = 0; i < this.coinAmount; i++)
				{
					Object.Instantiate<GameObject>(this.crateCoin, this.col.bounds.center + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)), Quaternion.identity, GoreZone.ResolveGoreZone(base.transform).transform);
				}
			}
			if (this.protectorCrate && MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer)
			{
				MonoSingleton<PlatformerMovement>.Instance.AddExtraHit(1);
			}
		}
		Rigidbody[] componentsInChildren = base.GetComponentsInChildren<Rigidbody>();
		if (componentsInChildren.Length != 0)
		{
			foreach (Rigidbody rigidbody2 in componentsInChildren)
			{
				rigidbody2.transform.SetParent(base.transform.parent, true);
				rigidbody2.isKinematic = false;
				rigidbody2.useGravity = true;
			}
		}
		if (this.activateOnBreak.Length != 0)
		{
			foreach (GameObject gameObject2 in this.activateOnBreak)
			{
				if (gameObject2 != null)
				{
					gameObject2.SetActive(true);
				}
			}
		}
		if (this.destroyOnBreak.Length != 0)
		{
			foreach (GameObject gameObject3 in this.destroyOnBreak)
			{
				if (gameObject3 != null)
				{
					Object.Destroy(gameObject3);
				}
			}
		}
		this.destroyEvent.Invoke("");
		Object.Destroy(base.gameObject);
	}

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x060002DF RID: 735 RVA: 0x00011186 File Offset: 0x0000F386
	public string alterKey
	{
		get
		{
			return "breakable";
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x060002E0 RID: 736 RVA: 0x0001118D File Offset: 0x0000F38D
	public string alterCategoryName
	{
		get
		{
			return "Breakable";
		}
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x060002E1 RID: 737 RVA: 0x00011194 File Offset: 0x0000F394
	public AlterOption<bool>[] options
	{
		get
		{
			return new AlterOption<bool>[]
			{
				new AlterOption<bool>
				{
					name = "Weak",
					key = "weak",
					value = this.weak,
					callback = delegate(bool value)
					{
						this.weak = value;
					}
				},
				new AlterOption<bool>
				{
					name = "Unbreakable",
					key = "unbreakable",
					value = this.unbreakable,
					callback = delegate(bool value)
					{
						this.unbreakable = value;
					}
				}
			};
		}
	}

	// Token: 0x04000364 RID: 868
	public bool unbreakable;

	// Token: 0x04000365 RID: 869
	public bool weak;

	// Token: 0x04000366 RID: 870
	public bool precisionOnly;

	// Token: 0x04000367 RID: 871
	public bool interrupt;

	// Token: 0x04000368 RID: 872
	public bool breakOnThrown;

	// Token: 0x04000369 RID: 873
	public bool breakOnTouch;

	// Token: 0x0400036A RID: 874
	public bool breakOnEnvironment;

	// Token: 0x0400036B RID: 875
	[HideInInspector]
	public EnemyIdentifier interruptEnemy;

	// Token: 0x0400036C RID: 876
	public bool playerOnly;

	// Token: 0x0400036D RID: 877
	public bool specialCaseOnly;

	// Token: 0x0400036E RID: 878
	public bool accurateExplosionsOnly;

	// Token: 0x0400036F RID: 879
	public bool forceGroundSlammable;

	// Token: 0x04000370 RID: 880
	public bool forceSawbladeable;

	// Token: 0x04000371 RID: 881
	[Space(10f)]
	public GameObject breakParticle;

	// Token: 0x04000372 RID: 882
	public AssetReference breakParticleFallback;

	// Token: 0x04000373 RID: 883
	public bool particleAtBoundsCenter;

	// Token: 0x04000374 RID: 884
	public bool applyScaleToParticle;

	// Token: 0x04000375 RID: 885
	public Transform customPositionRotation;

	// Token: 0x04000376 RID: 886
	[Space(10f)]
	public bool crate;

	// Token: 0x04000377 RID: 887
	public int bounceHealth;

	// Token: 0x04000378 RID: 888
	[HideInInspector]
	public int originalBounceHealth;

	// Token: 0x04000379 RID: 889
	public GameObject crateCoin;

	// Token: 0x0400037A RID: 890
	public int coinAmount;

	// Token: 0x0400037B RID: 891
	private float defaultHeight;

	// Token: 0x0400037C RID: 892
	public bool protectorCrate;

	// Token: 0x0400037D RID: 893
	[Space(10f)]
	public GameObject[] activateOnBreak;

	// Token: 0x0400037E RID: 894
	public GameObject[] destroyOnBreak;

	// Token: 0x0400037F RID: 895
	public UltrakillEvent destroyEvent;

	// Token: 0x04000380 RID: 896
	private bool broken;

	// Token: 0x04000381 RID: 897
	private Collider col;

	// Token: 0x04000382 RID: 898
	private TimeSince? timeSinceBurn;

	// Token: 0x04000383 RID: 899
	private ItemIdentifier itid;

	// Token: 0x04000384 RID: 900
	private Rigidbody rb;
}
