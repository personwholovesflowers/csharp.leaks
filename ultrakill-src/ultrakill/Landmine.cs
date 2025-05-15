using System;
using UnityEngine;

// Token: 0x02000291 RID: 657
public class Landmine : MonoBehaviour
{
	// Token: 0x06000E80 RID: 3712 RVA: 0x0006BF78 File Offset: 0x0006A178
	private void Start()
	{
		this.SetValues();
	}

	// Token: 0x06000E81 RID: 3713 RVA: 0x0006BF80 File Offset: 0x0006A180
	private void SetValues()
	{
		if (this.valuesSet)
		{
			return;
		}
		this.valuesSet = true;
		this.rb = base.GetComponent<Rigidbody>();
		this.aud = base.GetComponent<AudioSource>();
		this.lit = this.lightCylinder.GetComponentInChildren<Light>();
		this.rends = base.GetComponentsInChildren<Renderer>();
		this.sr = this.lightCylinder.GetComponentInChildren<SpriteRenderer>();
		this.block = new MaterialPropertyBlock();
		MonoSingleton<ObjectTracker>.Instance.AddLandmine(this);
	}

	// Token: 0x06000E82 RID: 3714 RVA: 0x0006BFF9 File Offset: 0x0006A1F9
	private void OnDestroy()
	{
		if (MonoSingleton<ObjectTracker>.Instance)
		{
			MonoSingleton<ObjectTracker>.Instance.RemoveLandmine(this);
		}
	}

	// Token: 0x06000E83 RID: 3715 RVA: 0x0006C014 File Offset: 0x0006A214
	private void OnTriggerEnter(Collider other)
	{
		if (this.activated)
		{
			return;
		}
		EnemyIdentifierIdentifier enemyIdentifierIdentifier;
		if (other.gameObject == MonoSingleton<NewMovement>.Instance.gameObject || (MonoSingleton<PlatformerMovement>.Instance && other.gameObject == MonoSingleton<PlatformerMovement>.Instance.gameObject) || (this.originEnemy != null && this.originEnemy.target != null && this.originEnemy.target.enemyIdentifier != null && other.gameObject == this.originEnemy.target.enemyIdentifier.gameObject) || ((other.gameObject.layer == 10 || other.gameObject.layer == 11) && other.gameObject.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier) && enemyIdentifierIdentifier.eid && !enemyIdentifierIdentifier.eid.dead && this.originEnemy != null && this.originEnemy.target != null && this.originEnemy.target.enemyIdentifier != null && this.originEnemy.target.enemyIdentifier == enemyIdentifierIdentifier.eid))
		{
			this.Activate(1f);
		}
	}

	// Token: 0x06000E84 RID: 3716 RVA: 0x0006C168 File Offset: 0x0006A368
	private void OnCollisionEnter(Collision collision)
	{
		if (!this.parried || this.exploded || collision.gameObject == MonoSingleton<NewMovement>.Instance.gameObject)
		{
			return;
		}
		EnemyIdentifier enemyIdentifier = null;
		EnemyIdentifierIdentifier enemyIdentifierIdentifier = null;
		ParryHelper parryHelper;
		if (collision.gameObject.layer == 26 && collision.gameObject.TryGetComponent<ParryHelper>(out parryHelper) && parryHelper.target)
		{
			parryHelper.target.TryGetComponent<EnemyIdentifier>(out enemyIdentifier);
		}
		if (enemyIdentifier || (collision.gameObject.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier) && enemyIdentifierIdentifier.eid) || collision.gameObject.TryGetComponent<EnemyIdentifier>(out enemyIdentifier))
		{
			if (enemyIdentifierIdentifier && enemyIdentifierIdentifier.eid)
			{
				enemyIdentifier = enemyIdentifierIdentifier.eid;
			}
			if (!enemyIdentifier.dead)
			{
				if (enemyIdentifier == this.originEnemy)
				{
					MonoSingleton<StyleHUD>.Instance.AddPoints(150, "ultrakill.landyours", null, enemyIdentifier, -1, "", "");
				}
				else
				{
					MonoSingleton<StyleHUD>.Instance.AddPoints(75, "ultrakill.serve", null, enemyIdentifier, -1, "", "");
				}
				Gutterman gutterman;
				if (enemyIdentifier.enemyType == EnemyType.Gutterman && enemyIdentifier.TryGetComponent<Gutterman>(out gutterman) && gutterman.hasShield)
				{
					gutterman.ShieldBreak(true, false);
				}
			}
		}
		this.Explode(true);
	}

	// Token: 0x06000E85 RID: 3717 RVA: 0x0006C2AE File Offset: 0x0006A4AE
	private void FixedUpdate()
	{
		if (this.parried)
		{
			this.rb.velocity = this.movementDirection * 250f;
		}
	}

	// Token: 0x06000E86 RID: 3718 RVA: 0x0006C2D4 File Offset: 0x0006A4D4
	public void Activate(float forceMultiplier = 1f)
	{
		this.rb.isKinematic = false;
		this.rb.useGravity = true;
		this.rb.AddForce(base.transform.up * 20f * forceMultiplier, ForceMode.VelocityChange);
		this.rb.AddRelativeTorque(Vector3.right * 360f * forceMultiplier, ForceMode.VelocityChange);
		if (this.activated)
		{
			return;
		}
		this.activated = true;
		this.aud.clip = this.activatedBeep;
		this.aud.pitch = 1.5f;
		this.aud.Play();
		this.parryZone.SetActive(true);
		this.SetColor(new Color(1f, 0.66f, 0f));
		base.Invoke("Explode", 1f);
	}

	// Token: 0x06000E87 RID: 3719 RVA: 0x0006C3B4 File Offset: 0x0006A5B4
	public void Parry()
	{
		base.CancelInvoke("Explode");
		this.parried = true;
		this.movementDirection = base.transform.forward;
		this.rb.useGravity = true;
		this.rb.AddRelativeTorque(Vector3.up * 36000f, ForceMode.VelocityChange);
		this.parryZone.SetActive(false);
		this.SetColor(new Color(0f, 1f, 1f));
		base.Invoke("Explode", 3f);
	}

	// Token: 0x06000E88 RID: 3720 RVA: 0x0006C441 File Offset: 0x0006A641
	private void Explode()
	{
		this.Explode(false);
	}

	// Token: 0x06000E89 RID: 3721 RVA: 0x0006C44C File Offset: 0x0006A64C
	private void Explode(bool super = false)
	{
		if (this.exploded)
		{
			return;
		}
		this.exploded = true;
		Object.Instantiate<GameObject>(super ? this.superExplosion : this.explosion, base.transform.position, Quaternion.identity);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06000E8A RID: 3722 RVA: 0x0006C49C File Offset: 0x0006A69C
	public void SetColor(Color newColor)
	{
		if (!this.valuesSet)
		{
			this.SetValues();
		}
		foreach (Renderer renderer in this.rends)
		{
			if (!(renderer == this.sr))
			{
				for (int j = 0; j < renderer.sharedMaterials.Length; j++)
				{
					renderer.GetPropertyBlock(this.block, j);
					this.block.SetColor(UKShaderProperties.EmissiveColor, newColor);
					renderer.SetPropertyBlock(this.block, j);
				}
			}
		}
		this.sr.color = new Color(newColor.r, newColor.g, newColor.b, this.sr.color.a);
		this.lit.color = newColor;
	}

	// Token: 0x04001345 RID: 4933
	private bool valuesSet;

	// Token: 0x04001346 RID: 4934
	private Rigidbody rb;

	// Token: 0x04001347 RID: 4935
	private AudioSource aud;

	// Token: 0x04001348 RID: 4936
	[SerializeField]
	private GameObject lightCylinder;

	// Token: 0x04001349 RID: 4937
	private Light lit;

	// Token: 0x0400134A RID: 4938
	private Renderer[] rends;

	// Token: 0x0400134B RID: 4939
	private SpriteRenderer sr;

	// Token: 0x0400134C RID: 4940
	private MaterialPropertyBlock block;

	// Token: 0x0400134D RID: 4941
	[SerializeField]
	private AudioClip activatedBeep;

	// Token: 0x0400134E RID: 4942
	[SerializeField]
	private GameObject explosion;

	// Token: 0x0400134F RID: 4943
	[SerializeField]
	private GameObject superExplosion;

	// Token: 0x04001350 RID: 4944
	[SerializeField]
	private GameObject parryZone;

	// Token: 0x04001351 RID: 4945
	private bool activated;

	// Token: 0x04001352 RID: 4946
	private bool parried;

	// Token: 0x04001353 RID: 4947
	private bool exploded;

	// Token: 0x04001354 RID: 4948
	private Vector3 movementDirection;

	// Token: 0x04001355 RID: 4949
	public EnemyIdentifier originEnemy;
}
