using System;
using Unity.Burst;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000080 RID: 128
public class Bloodsplatter : MonoBehaviour
{
	// Token: 0x17000052 RID: 82
	// (set) Token: 0x06000258 RID: 600 RVA: 0x0000D19E File Offset: 0x0000B39E
	[HideInInspector]
	public EnemyIdentifier eid
	{
		set
		{
			if (value != null)
			{
				this.eidID = value.GetInstanceID();
			}
		}
	}

	// Token: 0x06000259 RID: 601 RVA: 0x0000D1B8 File Offset: 0x0000B3B8
	private void Awake()
	{
		if (this.propertyBlock == null)
		{
			this.propertyBlock = new MaterialPropertyBlock();
		}
		if (!this.part)
		{
			this.part = base.GetComponent<ParticleSystem>();
		}
		if (this.part == null)
		{
			this.part = base.GetComponentInChildren<ParticleSystem>();
		}
		if (!this.aud)
		{
			this.aud = base.GetComponent<AudioSource>();
		}
		if (!this.col)
		{
			this.col = base.GetComponent<SphereCollider>();
		}
		this.cdatabase = MonoSingleton<ComponentsDatabase>.Instance;
		this.part.main.stopAction = ParticleSystemStopAction.Callback;
		this.part.AddListener(new UnityAction(this.Repool));
	}

	// Token: 0x0600025A RID: 602 RVA: 0x0000D274 File Offset: 0x0000B474
	private void OnEnable()
	{
		if (this.beenPlayed)
		{
			if (this.col && this.col.enabled)
			{
				if (this.underwater)
				{
					base.Invoke("DisableCollider", 2.5f);
					return;
				}
				base.Invoke("DisableCollider", 0.25f);
			}
			return;
		}
		this.beenPlayed = true;
		if (this.bsm == null)
		{
			this.bsm = MonoSingleton<BloodsplatterManager>.Instance;
		}
		bool flag = this.bsm.forceOn || MonoSingleton<PrefsManager>.Instance.GetBoolLocal("bloodEnabled", false);
		if (this.part)
		{
			this.part.Clear();
			if (flag)
			{
				this.part.Play();
			}
		}
		this.canCollide = true;
		if (this.aud != null)
		{
			this.aud.pitch = Random.Range(0.75f, 1.5f);
			this.aud.Play();
		}
		if (this.col)
		{
			this.col.enabled = true;
		}
		if (this.underwater)
		{
			base.Invoke("DisableCollider", 2.5f);
			return;
		}
		base.Invoke("DisableCollider", 0.25f);
	}

	// Token: 0x0600025B RID: 603 RVA: 0x0000D3B0 File Offset: 0x0000B5B0
	private void OnDisable()
	{
		if (this.col)
		{
			this.col.enabled = false;
		}
		base.CancelInvoke("DisableCollider");
		this.ready = false;
	}

	// Token: 0x0600025C RID: 604 RVA: 0x0000D3DD File Offset: 0x0000B5DD
	private void OnTriggerEnter(Collider other)
	{
		this.Collide(other);
	}

	// Token: 0x0600025D RID: 605 RVA: 0x0000D3E8 File Offset: 0x0000B5E8
	private void Collide(Collider other)
	{
		if (this.ready)
		{
			if (this.bsm == null)
			{
				return;
			}
			BloodFiller bloodFiller;
			if (this.bsm.hasBloodFillers && ((this.bsm.bloodFillers.Contains(other.gameObject) && other.gameObject.TryGetComponent<BloodFiller>(out bloodFiller)) || (other.attachedRigidbody && this.bsm.bloodFillers.Contains(other.attachedRigidbody.gameObject) && other.attachedRigidbody.TryGetComponent<BloodFiller>(out bloodFiller))))
			{
				bloodFiller.FillBloodSlider((float)this.hpAmount, base.transform.position, this.eidID);
				return;
			}
			if (this.canCollide && other.gameObject.CompareTag("Player"))
			{
				MonoSingleton<NewMovement>.Instance.GetHealth(this.hpAmount, false, this.fromExplosion, true);
				this.DisableCollider();
			}
		}
	}

	// Token: 0x0600025E RID: 606 RVA: 0x0000D4D4 File Offset: 0x0000B6D4
	public void Repool()
	{
		if (this.bloodSplatterType == BSType.dontpool)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		if (this.bloodSplatterType == BSType.unknown)
		{
			GameObject gameObject = base.gameObject;
			Debug.LogWarning(((gameObject != null) ? gameObject.ToString() : null) + "has an unknown BSType, this shouldn't happen!");
			Object.Destroy(base.gameObject);
			return;
		}
		base.CancelInvoke("DisableCollider");
		this.gz = null;
		this.eid = null;
		this.fromExplosion = false;
		this.ready = false;
		this.beenPlayed = false;
		base.transform.localScale = Vector3.one;
		if (this.bsm)
		{
			this.bsm.RepoolGore(this, this.bloodSplatterType);
		}
	}

	// Token: 0x0600025F RID: 607 RVA: 0x0000D58C File Offset: 0x0000B78C
	private void PlayBloodSound(Vector3 position)
	{
		if (Random.value < 0.1f)
		{
			this.bsm.splatterClip.PlayClipAtPoint(this.bsm.goreAudioGroup, position, 256, 1f, 1f, 0.5f, AudioRolloffMode.Logarithmic, 1f, 100f);
		}
	}

	// Token: 0x06000260 RID: 608 RVA: 0x0000D5E4 File Offset: 0x0000B7E4
	[BurstCompile]
	public void CreateBloodstain(ref RaycastHit hit, BloodsplatterManager bsman)
	{
		this.bsm = bsman;
		Collider collider = hit.collider;
		if (collider == null)
		{
			return;
		}
		Rigidbody rigidbody = hit.rigidbody;
		GameObject gameObject = (rigidbody ? rigidbody.gameObject : collider.gameObject);
		IBloodstainReceiver bloodstainReceiver;
		if (StockMapInfo.Instance.continuousGibCollisions && gameObject.TryGetComponent<IBloodstainReceiver>(out bloodstainReceiver) && bloodstainReceiver.HandleBloodstainHit(ref hit))
		{
			this.PlayBloodSound(hit.point);
			return;
		}
		if (this.ready && this.hpOnParticleCollision && gameObject.CompareTag("Player"))
		{
			MonoSingleton<NewMovement>.Instance.GetHealth(3, false, this.fromExplosion, true);
			return;
		}
		Transform transform = gameObject.transform;
		float num = this.bsm.GetBloodstainChance();
		num = (this.halfChance ? (num / 2f) : num);
		if ((float)Random.Range(0, 100) < num)
		{
			bool flag = gameObject.CompareTag("Wall");
			bool flag2 = gameObject.CompareTag("Floor");
			bool flag3 = gameObject.CompareTag("Moving");
			bool flag4 = gameObject.CompareTag("Glass");
			bool flag5 = gameObject.CompareTag("GlassFloor");
			if (flag || flag2 || flag3 || flag4 || flag5)
			{
				Vector3 normal = hit.normal;
				if (!this.gz)
				{
					this.gz = GoreZone.ResolveGoreZone(base.transform);
				}
				Vector3 vector = hit.point;
				this.PlayBloodSound(vector);
				bool flag6 = true;
				MeshRenderer meshRenderer;
				if ((flag || flag2) && gameObject.TryGetComponent<MeshRenderer>(out meshRenderer))
				{
					Material sharedMaterial = meshRenderer.sharedMaterial;
					if (sharedMaterial != null && sharedMaterial.IsKeywordEnabled(Bloodsplatter.VERTEX_DISPLACEMENT))
					{
						vector += normal * 0.2f;
						flag6 = false;
					}
				}
				bool flag7 = flag3 || flag4 || flag5;
				if (!flag7)
				{
					flag7 |= this.cdatabase && this.cdatabase.scrollers.Contains(transform);
				}
				if (flag7)
				{
					ScrollingTexture scrollingTexture;
					BloodstainParent bloodstainParent = (gameObject.TryGetComponent<ScrollingTexture>(out scrollingTexture) ? scrollingTexture.parent : gameObject.GetOrAddComponent<BloodstainParent>());
					if (MonoSingleton<BloodsplatterManager>.Instance.usedComputeShadersAtStart)
					{
						bloodstainParent.CreateChild(vector, normal, flag6, false);
						return;
					}
				}
				else
				{
					this.gz.stains.CreateChild(vector, normal, flag6, true);
				}
			}
		}
	}

	// Token: 0x06000261 RID: 609 RVA: 0x0000D81B File Offset: 0x0000BA1B
	private void DisableCollider()
	{
		this.canCollide = false;
		if (!this.part.isPlaying)
		{
			this.Repool();
		}
	}

	// Token: 0x06000262 RID: 610 RVA: 0x0000D837 File Offset: 0x0000BA37
	public void GetReady()
	{
		this.ready = true;
	}

	// Token: 0x040002B6 RID: 694
	public BSType bloodSplatterType;

	// Token: 0x040002B7 RID: 695
	[HideInInspector]
	public ParticleSystem part;

	// Token: 0x040002B8 RID: 696
	private int i;

	// Token: 0x040002B9 RID: 697
	private AudioSource aud;

	// Token: 0x040002BA RID: 698
	private int eidID;

	// Token: 0x040002BB RID: 699
	private SpriteRenderer sr;

	// Token: 0x040002BC RID: 700
	private MeshRenderer mr;

	// Token: 0x040002BD RID: 701
	private NewMovement nmov;

	// Token: 0x040002BE RID: 702
	public int hpAmount;

	// Token: 0x040002BF RID: 703
	private SphereCollider col;

	// Token: 0x040002C0 RID: 704
	public bool hpOnParticleCollision;

	// Token: 0x040002C1 RID: 705
	[HideInInspector]
	public bool beenPlayed;

	// Token: 0x040002C2 RID: 706
	public bool halfChance;

	// Token: 0x040002C3 RID: 707
	public bool ready;

	// Token: 0x040002C4 RID: 708
	private GoreZone gz;

	// Token: 0x040002C5 RID: 709
	public bool underwater;

	// Token: 0x040002C6 RID: 710
	private MaterialPropertyBlock propertyBlock;

	// Token: 0x040002C7 RID: 711
	private bool canCollide = true;

	// Token: 0x040002C8 RID: 712
	public BloodsplatterManager bsm;

	// Token: 0x040002C9 RID: 713
	[HideInInspector]
	public bool fromExplosion;

	// Token: 0x040002CA RID: 714
	private ComponentsDatabase cdatabase;

	// Token: 0x040002CB RID: 715
	private static string VERTEX_DISPLACEMENT = "VERTEX_DISPLACEMENT";
}
