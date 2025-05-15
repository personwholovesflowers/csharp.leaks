using System;
using UnityEngine;

// Token: 0x020004B7 RID: 1207
public class VirtueInsignia : MonoBehaviour
{
	// Token: 0x06001BAE RID: 7086 RVA: 0x000E53B4 File Offset: 0x000E35B4
	private void Start()
	{
		this.offset = Random.Range(-0.1f, 0.1f);
		this.sprites = base.GetComponentsInChildren<SpriteRenderer>();
		this.explosionWidth = this.explosion.transform.localScale.x;
		if (this.parentDrone)
		{
			this.parentDrone.childVi.Add(this);
			this.hadParent = true;
		}
		if (this.otherParent)
		{
			this.hadParent = true;
		}
		for (int i = 1; i < this.sprites.Length; i++)
		{
			this.sprites[i].gameObject.SetActive(false);
			this.sprites[i].transform.localPosition = Vector3.zero;
		}
		if ((this.charges > 1 && this.multiVersion != null && !this.predictive) || (this.predictiveVersion != null && this.predictive))
		{
			foreach (SpriteRenderer spriteRenderer in this.sprites)
			{
				if (this.predictive)
				{
					spriteRenderer.sprite = this.predictiveVersion;
				}
				else if (this.charges > 1)
				{
					spriteRenderer.sprite = this.multiVersion;
				}
			}
		}
		this.lit = base.GetComponent<Light>();
		if (!this.noTracking)
		{
			base.Invoke("Activating", 2f / this.windUpSpeedMultiplier);
			return;
		}
		this.Activating();
	}

	// Token: 0x06001BAF RID: 7087 RVA: 0x000E5520 File Offset: 0x000E3720
	private void Update()
	{
		if (!this.activating)
		{
			if (this.activationTime < 1f)
			{
				if (this.hadParent && !this.parentDrone && !this.otherParent)
				{
					Object.Destroy(base.gameObject);
				}
				base.transform.position = Vector3.MoveTowards(base.transform.position, this.target.position + Vector3.up * this.offset, Time.deltaTime * 50f + Time.deltaTime * Vector3.Distance(base.transform.position, this.target.position) * 100f);
				base.transform.Rotate(Vector3.up, Time.deltaTime * 180f, Space.Self);
				return;
			}
			this.explosionLength = Mathf.MoveTowards(this.explosionLength, 0f, Time.deltaTime);
			if (this.explosionLength <= 1f)
			{
				this.explosion.transform.localScale = new Vector3(this.explosionLength * this.explosionWidth, this.explosion.transform.localScale.y, this.explosionLength * this.explosionWidth);
				this.explAud.pitch = this.explosionLength;
				if (this.explosionLength <= 0f)
				{
					Object.Destroy(base.gameObject);
					return;
				}
			}
		}
		else
		{
			if (this.noTracking && this.hadParent && !this.parentDrone && (!this.otherParent || !this.otherParent.gameObject.activeSelf))
			{
				Object.Destroy(base.gameObject);
			}
			base.transform.Rotate(Vector3.up, Time.deltaTime * 720f, Space.Self);
			this.activationTime = Mathf.MoveTowards(this.activationTime, 1f, Time.deltaTime * this.windUpSpeedMultiplier);
			this.currentDistance = Mathf.MoveTowards(this.currentDistance, 1f, Time.deltaTime * (this.windUpSpeedMultiplier * 2f * (1f - this.currentDistance)));
			for (int i = 1; i < this.sprites.Length; i++)
			{
				if (i % 2 == 0)
				{
					this.sprites[i].transform.localPosition = Vector3.up * 3f * Mathf.Lerp(0f, (float)(i / 2 * -1), this.currentDistance);
				}
				else
				{
					this.sprites[i].transform.localPosition = Vector3.up * 3f * Mathf.Lerp(0f, (float)((i + 1) / 2), this.currentDistance);
				}
			}
			if (this.activationTime >= 1f)
			{
				this.activating = false;
				this.explosionLength += 1f;
				this.Explode();
			}
		}
	}

	// Token: 0x06001BB0 RID: 7088 RVA: 0x000E581C File Offset: 0x000E3A1C
	private void Activating()
	{
		if (!this.noTracking && this.charges > 1)
		{
			this.charges--;
			Object.Instantiate<GameObject>(base.gameObject, base.transform.position, Quaternion.identity).GetComponent<VirtueInsignia>().target = this.target;
		}
		this.activating = true;
		if (this.predictive)
		{
			if (this.target != null)
			{
				float num = 1f / this.windUpSpeedMultiplier;
				Vector3 velocity = this.target.GetVelocity();
				if (this.target.isPlayer)
				{
					if (MonoSingleton<NewMovement>.Instance.ridingRocket)
					{
						base.transform.position = this.target.PredictTargetPosition(num, false);
					}
					else
					{
						base.transform.position = this.target.position + Vector3.up * this.offset + new Vector3(velocity.x, 0f, velocity.z).normalized * 16.45f * (num - (base.transform.localScale.x - 1f) / 20f * num);
					}
				}
				else
				{
					base.transform.position = this.target.position + Vector3.up * this.offset + new Vector3(velocity.x, 0f, velocity.z) * (num - (base.transform.localScale.x - 1f) / 20f * num);
				}
			}
			else
			{
				base.transform.position = this.target.position + Vector3.up * this.offset;
			}
		}
		for (int i = 1; i < this.sprites.Length; i++)
		{
			this.sprites[i].gameObject.SetActive(true);
			this.sprites[i].transform.localPosition = Vector3.zero;
		}
		this.currentDistance = 0f;
		this.activationTime = 0f;
	}

	// Token: 0x06001BB1 RID: 7089 RVA: 0x000E5A54 File Offset: 0x000E3C54
	private void Explode()
	{
		if (this.noTracking && this.charges > 1 && (!this.hadParent || this.parentDrone || (this.otherParent && this.otherParent.gameObject.activeSelf)))
		{
			this.charges--;
			VirtueInsignia component = Object.Instantiate<GameObject>(base.gameObject, base.transform.position, Quaternion.identity).GetComponent<VirtueInsignia>();
			component.target = this.target;
			component.hadParent = this.hadParent;
			if (this.parentDrone)
			{
				component.parentDrone = this.parentDrone;
			}
			if (this.otherParent)
			{
				component.otherParent = this.otherParent;
			}
		}
		SpriteRenderer[] array = this.sprites;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(false);
		}
		this.explAud = this.explosion.GetComponent<AudioSource>();
		this.explosion.SetActive(true);
		MonoSingleton<CameraController>.Instance.CameraShake(1f);
		if (this.lit)
		{
			this.lit.enabled = false;
		}
		if (this.parentDrone)
		{
			this.parentDrone.childVi.Remove(this);
		}
	}

	// Token: 0x06001BB2 RID: 7090 RVA: 0x000E5BB0 File Offset: 0x000E3DB0
	private void OnTriggerEnter(Collider other)
	{
		if (this.target != null && !this.hasHitTarget && (!this.target.isPlayer || other.gameObject.CompareTag("Player")))
		{
			if (this.target.isPlayer)
			{
				this.hasHitTarget = true;
				if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer)
				{
					MonoSingleton<PlatformerMovement>.Instance.Burn(false);
				}
				else
				{
					MonoSingleton<NewMovement>.Instance.LaunchFromPoint(MonoSingleton<NewMovement>.Instance.transform.position, 200f, 5f);
					MonoSingleton<NewMovement>.Instance.GetHurt(this.damage, true, 1f, false, false, 0.35f, false);
				}
			}
			else
			{
				EnemyIdentifier enemyIdentifier = other.GetComponent<EnemyIdentifier>();
				if (enemyIdentifier == null)
				{
					EnemyIdentifierIdentifier component = other.GetComponent<EnemyIdentifierIdentifier>();
					if (component != null)
					{
						enemyIdentifier = component.eid;
					}
				}
				Rigidbody rigidbody;
				if (enemyIdentifier != null && this.target.enemyIdentifier == enemyIdentifier && other.TryGetComponent<Rigidbody>(out rigidbody))
				{
					this.hasHitTarget = true;
					rigidbody.AddExplosionForce(1000f, base.transform.position, 10f);
					enemyIdentifier.SimpleDamage((float)this.damage);
				}
			}
		}
		Flammable component2 = other.GetComponent<Flammable>();
		if (component2 && !component2.playerOnly)
		{
			component2.Burn(10f, false);
		}
	}

	// Token: 0x040026E5 RID: 9957
	public EnemyTarget target;

	// Token: 0x040026E6 RID: 9958
	public GameObject explosion;

	// Token: 0x040026E7 RID: 9959
	public int damage;

	// Token: 0x040026E8 RID: 9960
	private bool hasHitTarget;

	// Token: 0x040026E9 RID: 9961
	private float offset;

	// Token: 0x040026EA RID: 9962
	private SpriteRenderer[] sprites;

	// Token: 0x040026EB RID: 9963
	private bool activating;

	// Token: 0x040026EC RID: 9964
	private float activationTime;

	// Token: 0x040026ED RID: 9965
	private float currentDistance;

	// Token: 0x040026EE RID: 9966
	public float windUpSpeedMultiplier = 1f;

	// Token: 0x040026EF RID: 9967
	public float explosionLength;

	// Token: 0x040026F0 RID: 9968
	public int charges;

	// Token: 0x040026F1 RID: 9969
	private float explosionWidth;

	// Token: 0x040026F2 RID: 9970
	private AudioSource explAud;

	// Token: 0x040026F3 RID: 9971
	private Light lit;

	// Token: 0x040026F4 RID: 9972
	[HideInInspector]
	public Drone parentDrone;

	// Token: 0x040026F5 RID: 9973
	[HideInInspector]
	public Transform otherParent;

	// Token: 0x040026F6 RID: 9974
	public bool hadParent;

	// Token: 0x040026F7 RID: 9975
	public bool predictive;

	// Token: 0x040026F8 RID: 9976
	public bool noTracking;

	// Token: 0x040026F9 RID: 9977
	public Sprite predictiveVersion;

	// Token: 0x040026FA RID: 9978
	public Sprite multiVersion;
}
