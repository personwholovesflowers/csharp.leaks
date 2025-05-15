using System;
using Sandbox;
using UnityEngine;

// Token: 0x0200028E RID: 654
public class JumpPad : MonoBehaviour, IAlter, IAlterOptions<float>
{
	// Token: 0x06000E6F RID: 3695 RVA: 0x0006B9D8 File Offset: 0x00069BD8
	private void Start()
	{
		this.aud = base.GetComponent<AudioSource>();
		if (this.aud)
		{
			this.origPitch = this.aud.pitch;
		}
	}

	// Token: 0x06000E70 RID: 3696 RVA: 0x0006BA04 File Offset: 0x00069C04
	private void OnTriggerEnter(Collider other)
	{
		if (!other.gameObject.isStatic)
		{
			float num = 1f;
			if (other.gameObject.CompareTag("Player"))
			{
				NewMovement instance = MonoSingleton<NewMovement>.Instance;
				if (instance && instance.gameObject.activeSelf)
				{
					if (instance.gc.heavyFall && !this.ignoreSlam)
					{
						num = 1.2f;
					}
					instance.LaunchFromPoint(other.transform.position, 0f, 1f);
					if (this.forceDirection)
					{
						if (instance.sliding)
						{
							instance.StopSlide();
						}
						if (instance.boost)
						{
							instance.boost = false;
						}
					}
				}
				else if (MonoSingleton<PlatformerMovement>.Instance)
				{
					if (this.forceDirection)
					{
						MonoSingleton<PlatformerMovement>.Instance.StopSlide();
						MonoSingleton<PlatformerMovement>.Instance.rb.velocity = Vector3.zero;
					}
					MonoSingleton<PlatformerMovement>.Instance.Jump(true, 0f);
				}
			}
			else if (other.gameObject.CompareTag("Enemy"))
			{
				EnemyIdentifier component = other.gameObject.GetComponent<EnemyIdentifier>();
				if (component != null && !component.dead)
				{
					if (component.unbounceable)
					{
						return;
					}
					component.DeliverDamage(other.gameObject, Vector3.up * 10f, other.transform.position, 0f, false, 0f, null, false, false);
				}
			}
			Rigidbody component2 = other.gameObject.GetComponent<Rigidbody>();
			if (component2 != null && !component2.isKinematic)
			{
				Vector3 velocity = component2.velocity;
				if (base.transform.up.x != 0f || this.forceDirection)
				{
					velocity.x = base.transform.up.x * this.force * num;
				}
				if (base.transform.up.y != 0f || this.forceDirection)
				{
					velocity.y = base.transform.up.y * this.force * num;
				}
				if (base.transform.up.z != 0f || this.forceDirection)
				{
					velocity.z = base.transform.up.z * this.force * num;
				}
				component2.velocity = velocity;
				int layer = other.gameObject.layer;
				if (layer == 14)
				{
					other.transform.LookAt(other.transform.position + component2.velocity);
				}
				if (this.aud)
				{
					if (layer == 11 || layer == 12 || layer == 2 || layer == 15)
					{
						this.aud.clip = this.launchSound;
					}
					else
					{
						this.aud.clip = this.lightLaunchSound;
					}
					this.aud.pitch = this.origPitch + Random.Range(-0.1f, 0.1f);
					this.aud.Play();
				}
			}
		}
	}

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x06000E71 RID: 3697 RVA: 0x0006BD0A File Offset: 0x00069F0A
	public string alterKey
	{
		get
		{
			return "jump-pad";
		}
	}

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x06000E72 RID: 3698 RVA: 0x0006BD11 File Offset: 0x00069F11
	public string alterCategoryName
	{
		get
		{
			return "Jump Pad";
		}
	}

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x06000E73 RID: 3699 RVA: 0x0006BD18 File Offset: 0x00069F18
	public AlterOption<float>[] options
	{
		get
		{
			return new AlterOption<float>[]
			{
				new AlterOption<float>
				{
					name = "Force",
					key = "force",
					value = this.force,
					callback = delegate(float value)
					{
						this.force = value;
					}
				}
			};
		}
	}

	// Token: 0x04001330 RID: 4912
	public float force;

	// Token: 0x04001331 RID: 4913
	private float origPitch;

	// Token: 0x04001332 RID: 4914
	private AudioSource aud;

	// Token: 0x04001333 RID: 4915
	public AudioClip launchSound;

	// Token: 0x04001334 RID: 4916
	public AudioClip lightLaunchSound;

	// Token: 0x04001335 RID: 4917
	public bool forceDirection;

	// Token: 0x04001336 RID: 4918
	public bool ignoreSlam;
}
