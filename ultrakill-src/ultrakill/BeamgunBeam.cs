using System;
using UnityEngine;

// Token: 0x02000072 RID: 114
public class BeamgunBeam : MonoBehaviour
{
	// Token: 0x0600021D RID: 541 RVA: 0x0000B013 File Offset: 0x00009213
	private void Start()
	{
		this.line.widthMultiplier = this.beamWidth;
		this.hitParticle.transform.localScale = Vector3.one * this.beamWidth * 5f;
	}

	// Token: 0x0600021E RID: 542 RVA: 0x0000B050 File Offset: 0x00009250
	private void FixedUpdate()
	{
		if (this.beamCheckTime > 0f)
		{
			this.beamCheckTime = Mathf.MoveTowards(this.beamCheckTime, 0f, Time.deltaTime * this.beamCheckSpeed * 15f);
		}
		if (this.beamCheckTime <= 0f)
		{
			this.beamCheckTime = 1f;
			if (this.hitTarget)
			{
				this.hitTarget.eid.DeliverDamage(this.hitTarget.transform.gameObject, (this.hitPosition - base.transform.position).normalized * 10f, this.hitPosition, 0.15f, false, 0.5f, null, false, false);
			}
		}
	}

	// Token: 0x0600021F RID: 543 RVA: 0x0000B114 File Offset: 0x00009314
	private void Update()
	{
		this.hitTarget = null;
		if (this.playerDamageCooldown > 0f)
		{
			this.playerDamageCooldown = Mathf.MoveTowards(this.playerDamageCooldown, 0f, Time.deltaTime);
		}
		if (this.active)
		{
			this.line.enabled = true;
			if (this.line.widthMultiplier != this.beamWidth)
			{
				this.line.widthMultiplier = Mathf.MoveTowards(this.line.widthMultiplier, this.beamWidth, Time.deltaTime * 25f);
				this.hitParticle.transform.localScale = Vector3.one * this.line.widthMultiplier * 5f;
			}
			if (this.fakeStartPoint != Vector3.zero)
			{
				this.line.SetPosition(0, this.fakeStartPoint);
			}
			else
			{
				this.line.SetPosition(0, this.line.transform.position);
			}
			this.hitPosition = base.transform.position + base.transform.forward * 9999f;
			Vector3 vector = base.transform.forward * -1f;
			LayerMask layerMask = LayerMaskDefaults.Get(LMD.EnemiesAndEnvironment);
			if (this.canHitPlayer && this.playerDamageCooldown <= 0f)
			{
				layerMask = LayerMaskDefaults.Get(LMD.EnemiesEnvironmentAndPlayer);
			}
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit, float.PositiveInfinity, layerMask, QueryTriggerInteraction.Ignore))
			{
				Breakable breakable;
				if (!LayerMaskDefaults.IsMatchingLayer(raycastHit.transform.gameObject.layer, LMD.Environment))
				{
					if (raycastHit.collider.gameObject == MonoSingleton<NewMovement>.Instance.gameObject)
					{
						MonoSingleton<NewMovement>.Instance.GetHurt(10, true, 1f, false, false, 0.35f, false);
						this.playerDamageCooldown = 1f;
					}
					else if (raycastHit.transform.gameObject.layer == 10 || raycastHit.transform.gameObject.layer == 11)
					{
						EnemyIdentifierIdentifier enemyIdentifierIdentifier;
						if (raycastHit.transform.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
						{
							this.hitTarget = enemyIdentifierIdentifier;
						}
						else
						{
							Grenade componentInParent = raycastHit.transform.GetComponentInParent<Grenade>();
							if (componentInParent)
							{
								componentInParent.Explode(true, false, false, 1f, false, null, false);
							}
						}
					}
				}
				else if (raycastHit.transform.gameObject.CompareTag("Breakable") && raycastHit.transform.TryGetComponent<Breakable>(out breakable) && !breakable.precisionOnly && !breakable.specialCaseOnly)
				{
					breakable.Break();
				}
				this.hitPosition = raycastHit.point;
				vector = raycastHit.normal;
			}
			this.line.SetPosition(1, this.hitPosition);
			this.hitParticle.transform.position = this.hitPosition;
			this.hitParticle.transform.forward = vector;
			if (!this.hitParticle.isPlaying)
			{
				this.hitParticle.Play();
				return;
			}
		}
		else
		{
			if (this.line.enabled)
			{
				this.line.enabled = false;
			}
			if (this.hitParticle.isPlaying)
			{
				this.hitParticle.Stop();
			}
		}
	}

	// Token: 0x04000255 RID: 597
	public bool active = true;

	// Token: 0x04000256 RID: 598
	[SerializeField]
	private LineRenderer line;

	// Token: 0x04000257 RID: 599
	[HideInInspector]
	public Vector3 fakeStartPoint;

	// Token: 0x04000258 RID: 600
	[SerializeField]
	private ParticleSystem hitParticle;

	// Token: 0x04000259 RID: 601
	private EnemyIdentifierIdentifier hitTarget;

	// Token: 0x0400025A RID: 602
	private Vector3 hitPosition;

	// Token: 0x0400025B RID: 603
	public bool canHitPlayer;

	// Token: 0x0400025C RID: 604
	private float beamCheckTime;

	// Token: 0x0400025D RID: 605
	public float beamCheckSpeed = 1f;

	// Token: 0x0400025E RID: 606
	private float playerDamageCooldown;

	// Token: 0x0400025F RID: 607
	public float beamWidth = 0.1f;
}
