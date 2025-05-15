using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000237 RID: 567
public class GroundCheck : MonoBehaviour
{
	// Token: 0x06000C20 RID: 3104 RVA: 0x00055D6C File Offset: 0x00053F6C
	private void Start()
	{
		this.nmov = MonoSingleton<NewMovement>.Instance;
		this.pmov = base.transform.parent.GetComponent<PlayerMovementParenting>();
		if (this.pmov == null)
		{
			this.pmov = this.nmov.GetComponent<PlayerMovementParenting>();
		}
		this.waterMask = LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies);
		this.waterMask |= 4;
	}

	// Token: 0x06000C21 RID: 3105 RVA: 0x00055DDD File Offset: 0x00053FDD
	private void OnEnable()
	{
		base.transform.parent.parent = null;
	}

	// Token: 0x06000C22 RID: 3106 RVA: 0x00055DF0 File Offset: 0x00053FF0
	private void OnDisable()
	{
		this.touchingGround = false;
		if (MonoSingleton<NewMovement>.Instance)
		{
			MonoSingleton<NewMovement>.Instance.groundProperties = null;
		}
		this.cols.Clear();
		this.canJump = false;
	}

	// Token: 0x06000C23 RID: 3107 RVA: 0x00055E24 File Offset: 0x00054024
	private void Update()
	{
		if (this.forcedOff > 0)
		{
			this.onGround = false;
		}
		else if (this.onGround != this.touchingGround)
		{
			this.onGround = this.touchingGround;
		}
		if (!this.heavyFall)
		{
			this.hitEnemies.Clear();
		}
		if (this.onGround)
		{
			this.sinceLastGrounded = 0f;
		}
		MonoSingleton<PlayerAnimations>.Instance.onGround = this.onGround;
		if (this.superJumpChance > 0f)
		{
			this.superJumpChance = Mathf.MoveTowards(this.superJumpChance, 0f, Time.deltaTime);
			if (this.superJumpChance == 0f)
			{
				if (this.shockwave != null && this.nmov.stillHolding)
				{
					Object.Instantiate<GameObject>(this.shockwave, base.transform.position, Quaternion.identity).GetComponent<PhysicalShockwave>().force *= this.nmov.slamForce * 2.25f;
					this.nmov.cc.CameraShake(0.75f);
					MonoSingleton<SceneHelper>.Instance.CreateEnviroGibs(base.transform.position, Vector3.down, 5f, 10, 1f);
				}
				this.extraJumpChance = 0.15f;
				this.nmov.stillHolding = false;
			}
		}
		if (this.extraJumpChance > 0f)
		{
			this.extraJumpChance = Mathf.MoveTowards(this.extraJumpChance, 0f, Time.deltaTime);
			if (this.extraJumpChance <= 0f && this.superJumpChance <= 0f && this.bounceChance <= 0f)
			{
				this.nmov.slamForce = 0f;
			}
		}
		if (this.bounceChance > 0f)
		{
			this.bounceChance = Mathf.MoveTowards(this.bounceChance, 0f, Time.deltaTime);
			if (!GameStateManager.Instance.PlayerInputLocked && MonoSingleton<InputManager>.Instance.InputSource.Jump.IsPressed)
			{
				this.bounceChance = 0f;
				this.Bounce(this.bouncePosition);
			}
			if (!this.heavyFall && this.extraJumpChance <= 0f && this.superJumpChance <= 0f && this.bounceChance <= 0f)
			{
				this.nmov.slamForce = 0f;
			}
		}
		else
		{
			this.hasImpacted = false;
		}
		if (this.cols.Count > 0)
		{
			for (int i = this.cols.Count - 1; i >= 0; i--)
			{
				if (!this.ColliderIsStillUsable(this.cols[i]))
				{
					this.cols.RemoveAt(i);
				}
			}
		}
		if (this.touchingGround && this.cols.Count == 0)
		{
			this.touchingGround = false;
			MonoSingleton<NewMovement>.Instance.groundProperties = null;
		}
		if (this.canJump && (this.currentEnemyCol == null || !this.currentEnemyCol.gameObject.activeInHierarchy || Vector3.Distance(base.transform.position, this.currentEnemyCol.transform.position) > 40f))
		{
			this.canJump = false;
		}
	}

	// Token: 0x06000C24 RID: 3108 RVA: 0x00056150 File Offset: 0x00054350
	private void FixedUpdate()
	{
		if (this.heavyFall)
		{
			Collider[] array = RaycastAssistant.TrueSphereCastAll(base.transform.position, 1.25f, Vector3.down, 3f, LayerMaskDefaults.Get(LMD.Enemies));
			if (array != null)
			{
				foreach (Collider collider in array)
				{
					if ((collider.gameObject.layer == 10 || collider.gameObject.layer == 11) && !Physics.Raycast(base.transform.position + Vector3.up, collider.bounds.center - base.transform.position + Vector3.up, Vector3.Distance(base.transform.position + Vector3.up, collider.bounds.center), LayerMaskDefaults.Get(LMD.Environment)))
					{
						EnemyIdentifierIdentifier component = collider.gameObject.GetComponent<EnemyIdentifierIdentifier>();
						if (component && component.eid && !this.hitEnemies.Contains(component.eid))
						{
							bool dead = component.eid.dead;
							this.hitEnemies.Add(component.eid);
							component.eid.hitter = "ground slam";
							component.eid.DeliverDamage(collider.gameObject, Vector3.down * 50000f, collider.transform.position, (float)(this.instakillStomp ? 99999 : 2), true, 0f, null, false, false);
							if (!dead)
							{
								if (!GameStateManager.Instance.PlayerInputLocked && MonoSingleton<InputManager>.Instance.InputSource.Jump.IsPressed)
								{
									this.Bounce(base.transform.position);
								}
								else if (this.bounceChance <= 0f)
								{
									this.bouncePosition = base.transform.position;
									this.bounceChance = 0.15f;
								}
							}
						}
					}
				}
			}
		}
		if (MonoSingleton<UnderwaterController>.Instance.inWater || this.slopeCheck || MonoSingleton<PlayerTracker>.Instance.GetPlayerVelocity(false).y >= 0f || (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.FPS && !MonoSingleton<NewMovement>.Instance.sliding) || (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer && !MonoSingleton<PlatformerMovement>.Instance.sliding))
		{
			return;
		}
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position, Vector3.down, out raycastHit, Mathf.Abs(MonoSingleton<PlayerTracker>.Instance.GetPlayerVelocity(false).y), this.waterMask, QueryTriggerInteraction.Collide) && raycastHit.transform.gameObject.layer == 4)
		{
			this.BounceOnWater(raycastHit.collider);
		}
	}

	// Token: 0x06000C25 RID: 3109 RVA: 0x00056424 File Offset: 0x00054624
	private void Bounce(Vector3 position)
	{
		this.heavyFall = false;
		Vector3 position2 = MonoSingleton<CameraController>.Instance.transform.position;
		this.nmov.transform.position = position;
		MonoSingleton<CameraController>.Instance.transform.position = position2;
		MonoSingleton<CameraController>.Instance.defaultPos = MonoSingleton<CameraController>.Instance.transform.localPosition;
		this.nmov.Jump();
		this.nmov.EnemyStepResets();
		this.nmov.slamCooldown = 0.25f;
		if (!this.hasImpacted)
		{
			this.nmov.LandingImpact();
		}
	}

	// Token: 0x06000C26 RID: 3110 RVA: 0x000564BC File Offset: 0x000546BC
	private void OnTriggerExit(Collider other)
	{
		if (this.ColliderIsCheckable(other) && this.cols.Contains(other))
		{
			if (this.cols.IndexOf(other) == this.cols.Count - 1)
			{
				this.cols.Remove(other);
				if (this.cols.Count > 0)
				{
					for (int i = this.cols.Count - 1; i >= 0; i--)
					{
						if (this.ColliderIsStillUsable(this.cols[i]))
						{
							MonoSingleton<NewMovement>.Instance.groundProperties = (this.cols[i].attachedRigidbody ? this.cols[i].attachedRigidbody.GetComponent<CustomGroundProperties>() : this.cols[i].GetComponent<CustomGroundProperties>());
							break;
						}
						this.cols.RemoveAt(i);
					}
				}
			}
			else
			{
				this.cols.Remove(other);
			}
			if (this.cols.Count == 0)
			{
				this.touchingGround = false;
				MonoSingleton<NewMovement>.Instance.groundProperties = null;
			}
			if (!this.slopeCheck && (other.gameObject.CompareTag("Moving") || other.gameObject.layer == 11 || other.gameObject.layer == 26) && this.pmov.IsObjectTracked(other.transform))
			{
				this.pmov.DetachPlayer(other.transform);
				return;
			}
		}
		else if (!other.gameObject.CompareTag("Slippery") && other.gameObject.layer == 12)
		{
			this.canJump = false;
		}
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x0005665C File Offset: 0x0005485C
	private void OnTriggerEnter(Collider other)
	{
		if (this.ColliderIsCheckable(other) && !this.cols.Contains(other))
		{
			this.cols.Add(other);
			this.touchingGround = true;
			CustomGroundProperties customGroundProperties;
			if ((!other.attachedRigidbody && other.TryGetComponent<CustomGroundProperties>(out customGroundProperties)) || (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent<CustomGroundProperties>(out customGroundProperties)))
			{
				MonoSingleton<NewMovement>.Instance.groundProperties = customGroundProperties;
			}
			else
			{
				MonoSingleton<NewMovement>.Instance.groundProperties = null;
			}
			if (!this.slopeCheck && (other.gameObject.CompareTag("Moving") || other.gameObject.layer == 11 || other.gameObject.layer == 26) && other.attachedRigidbody != null && !this.pmov.IsObjectTracked(other.transform))
			{
				this.pmov.AttachPlayer(other.transform);
			}
		}
		else if (!other.gameObject.CompareTag("Slippery") && other.gameObject.layer == 12)
		{
			this.currentEnemyCol = other;
			this.canJump = true;
		}
		if (this.heavyFall)
		{
			if ((other.gameObject.layer == 10 || other.gameObject.layer == 11) && !Physics.Raycast(base.transform.position + Vector3.up, other.bounds.center - base.transform.position + Vector3.up, Vector3.Distance(base.transform.position + Vector3.up, other.bounds.center), LayerMaskDefaults.Get(LMD.Environment)))
			{
				EnemyIdentifierIdentifier component = other.gameObject.GetComponent<EnemyIdentifierIdentifier>();
				if (component && component.eid && !this.hitEnemies.Contains(component.eid))
				{
					bool dead = component.eid.dead;
					this.hitEnemies.Add(component.eid);
					component.eid.hitter = "ground slam";
					component.eid.DeliverDamage(other.gameObject, (base.transform.position - other.transform.position) * 5000f, other.transform.position, (float)(this.instakillStomp ? 99999 : 2), true, 0f, null, false, false);
					if (!dead)
					{
						if (!GameStateManager.Instance.PlayerInputLocked && MonoSingleton<InputManager>.Instance.InputSource.Jump.IsPressed)
						{
							this.Bounce(base.transform.position);
						}
						else if (this.bounceChance <= 0f)
						{
							this.bouncePosition = base.transform.position;
							this.bounceChance = 0.15f;
						}
					}
				}
			}
			else if (!other.gameObject.CompareTag("Slippery") && LayerMaskDefaults.IsMatchingLayer(other.gameObject.layer, LMD.Environment))
			{
				Breakable component2 = other.gameObject.GetComponent<Breakable>();
				if (component2 != null && ((component2.weak && !component2.precisionOnly) || component2.forceGroundSlammable) && !component2.unbreakable && !component2.specialCaseOnly)
				{
					component2.Break();
				}
				else
				{
					this.heavyFall = false;
				}
				Bleeder bleeder;
				if (other.gameObject.TryGetComponent<Bleeder>(out bleeder))
				{
					bleeder.GetHit(other.transform.position, GoreType.Body, false);
				}
				Idol idol;
				if (other.transform.TryGetComponent<Idol>(out idol))
				{
					idol.Death();
				}
				this.superJumpChance = 0.1f;
			}
		}
		if (!this.slopeCheck && other.gameObject.layer == 4 && ((MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.FPS && this.nmov.sliding && this.nmov.rb.velocity.y < 0f) || (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer && MonoSingleton<PlatformerMovement>.Instance.sliding && MonoSingleton<PlatformerMovement>.Instance.rb.velocity.y < 0f)))
		{
			Vector3 vector = other.ClosestPoint(base.transform.position);
			RaycastHit raycastHit;
			if (!MonoSingleton<UnderwaterController>.Instance.inWater && ((Vector3.Distance(vector, base.transform.position) < 0.1f && other.Raycast(new Ray(base.transform.position + Vector3.up * 1f, Vector3.down), out raycastHit, 1.1f)) || !Physics.Raycast(base.transform.position, Vector3.down, Vector3.Distance(vector, base.transform.position), LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies), QueryTriggerInteraction.Collide)))
			{
				this.BounceOnWater(other);
			}
		}
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x00056B54 File Offset: 0x00054D54
	private void BounceOnWater(Collider other)
	{
		if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.FPS)
		{
			this.nmov.rb.velocity = new Vector3(this.nmov.rb.velocity.x, 0f, this.nmov.rb.velocity.z);
			this.nmov.rb.AddForce(Vector3.up * 10f, ForceMode.VelocityChange);
		}
		else
		{
			MonoSingleton<PlatformerMovement>.Instance.rb.velocity = new Vector3(MonoSingleton<PlatformerMovement>.Instance.rb.velocity.x, 0f, MonoSingleton<PlatformerMovement>.Instance.rb.velocity.z);
			MonoSingleton<PlatformerMovement>.Instance.rb.AddForce(Vector3.up * 10f, ForceMode.VelocityChange);
		}
		Water componentInParent = other.GetComponentInParent<Water>();
		if (componentInParent)
		{
			GameObject gameObject = componentInParent.SpawnBasicSplash(Water.WaterGOType.small);
			gameObject.transform.SetPositionAndRotation(base.transform.position, Quaternion.LookRotation(Vector3.up));
			gameObject.GetComponent<AudioSource>().volume = 0.65f;
			ChallengeTrigger component = componentInParent.GetComponent<ChallengeTrigger>();
			if (component)
			{
				component.Entered();
			}
		}
	}

	// Token: 0x06000C29 RID: 3113 RVA: 0x00056C8D File Offset: 0x00054E8D
	public void ForceOff()
	{
		this.onGround = false;
		this.forcedOff++;
	}

	// Token: 0x06000C2A RID: 3114 RVA: 0x00056CA4 File Offset: 0x00054EA4
	public void StopForceOff()
	{
		this.forcedOff--;
		if (this.forcedOff <= 0)
		{
			this.onGround = this.touchingGround;
		}
	}

	// Token: 0x06000C2B RID: 3115 RVA: 0x00056CCC File Offset: 0x00054ECC
	public bool ColliderIsCheckable(Collider col)
	{
		return !col.isTrigger && !col.gameObject.CompareTag("Slippery") && (LayerMaskDefaults.IsMatchingLayer(col.gameObject.layer, LMD.Environment) || col.gameObject.layer == 11 || col.gameObject.layer == 26 || (col.gameObject.layer == 18 && col.gameObject.CompareTag("Floor")));
	}

	// Token: 0x06000C2C RID: 3116 RVA: 0x00056D4C File Offset: 0x00054F4C
	public bool ColliderIsStillUsable(Collider col)
	{
		return !(col == null) && col.enabled && !col.isTrigger && col.gameObject.activeInHierarchy && col.gameObject.layer != 17 && col.gameObject.layer != 10;
	}

	// Token: 0x04000FF0 RID: 4080
	public bool slopeCheck;

	// Token: 0x04000FF1 RID: 4081
	public bool onGround;

	// Token: 0x04000FF2 RID: 4082
	public bool touchingGround;

	// Token: 0x04000FF3 RID: 4083
	public bool canJump;

	// Token: 0x04000FF4 RID: 4084
	public bool heavyFall;

	// Token: 0x04000FF5 RID: 4085
	public bool instakillStomp;

	// Token: 0x04000FF6 RID: 4086
	public GameObject shockwave;

	// Token: 0x04000FF7 RID: 4087
	public float superJumpChance;

	// Token: 0x04000FF8 RID: 4088
	public float extraJumpChance;

	// Token: 0x04000FF9 RID: 4089
	public float bounceChance;

	// Token: 0x04000FFA RID: 4090
	private Vector3 bouncePosition;

	// Token: 0x04000FFB RID: 4091
	[HideInInspector]
	public bool hasImpacted;

	// Token: 0x04000FFC RID: 4092
	public TimeSince sinceLastGrounded;

	// Token: 0x04000FFD RID: 4093
	private NewMovement nmov;

	// Token: 0x04000FFE RID: 4094
	private PlayerMovementParenting pmov;

	// Token: 0x04000FFF RID: 4095
	private Collider currentEnemyCol;

	// Token: 0x04001000 RID: 4096
	public int forcedOff;

	// Token: 0x04001001 RID: 4097
	private LayerMask waterMask;

	// Token: 0x04001002 RID: 4098
	public List<Collider> cols = new List<Collider>();

	// Token: 0x04001003 RID: 4099
	private List<EnemyIdentifier> hitEnemies = new List<EnemyIdentifier>();
}
