using System;
using System.Collections.Generic;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Token: 0x02000315 RID: 789
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class NewMovement : MonoSingleton<NewMovement>
{
	// Token: 0x06001208 RID: 4616 RVA: 0x0008E8C8 File Offset: 0x0008CAC8
	protected override void Awake()
	{
		base.Awake();
		this.rb = base.GetComponent<Rigidbody>();
		this.aud = base.GetComponent<AudioSource>();
		this.wc = base.GetComponentInChildren<WallCheck>();
		this.aud2 = this.gc.GetComponent<AudioSource>();
		this.aud3 = this.wc.GetComponent<AudioSource>();
		this.cc = base.GetComponentInChildren<CameraController>();
		this.playerCollider = base.GetComponent<CapsuleCollider>();
		this.frictionlessSurfaceMask = LayerMaskDefaults.Get(LMD.Environment);
		this.frictionlessSurfaceMask |= 1;
	}

	// Token: 0x06001209 RID: 4617 RVA: 0x0008E960 File Offset: 0x0008CB60
	private void OnDisable()
	{
		if (this.sliding)
		{
			this.StopSlide();
		}
		if (this.currentFallParticle)
		{
			Object.Destroy(this.currentFallParticle);
		}
		if (this.wallScrape)
		{
			Object.Destroy(this.wallScrape);
		}
		Physics.IgnoreLayerCollision(2, 12, false);
	}

	// Token: 0x0600120A RID: 4618 RVA: 0x0008E9B4 File Offset: 0x0008CBB4
	private void OnPrefChanged(string key, object value)
	{
		if (key == "weaponHoldPosition" || key == "hudType")
		{
			int @int = MonoSingleton<PrefsManager>.Instance.GetInt("weaponHoldPosition", 0);
			int int2 = MonoSingleton<PrefsManager>.Instance.GetInt("hudType", 0);
			this.quakeJump = @int == 1 && int2 >= 2;
		}
	}

	// Token: 0x0600120B RID: 4619 RVA: 0x0008EA14 File Offset: 0x0008CC14
	private void Start()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
		this.inman = MonoSingleton<InputManager>.Instance;
		this.asscon = MonoSingleton<AssistController>.Instance;
		if (this.hurtScreen)
		{
			this.hurtColor = this.hurtScreen.color;
			this.currentColor = this.hurtColor;
			this.currentColor.a = 0f;
			this.hurtScreen.color = this.currentColor;
			this.hurtScreen.enabled = false;
			Shader.SetGlobalColor("_HurtScreenColor", this.currentColor);
			this.hurtAud = this.hurtScreen.GetComponent<AudioSource>();
			this.fullHud = this.hurtScreen.GetComponentInParent<Canvas>();
		}
		this.hudOriginalPos = this.screenHud.transform.localPosition;
		this.camOriginalPos = this.hudCam.transform.localPosition;
		MonoSingleton<TimeController>.Instance.SetAllPitch(1f);
		this.defaultRBConstraints = this.rb.constraints;
		this.rb.solverIterations *= 5;
		this.rb.solverVelocityIterations *= 5;
		this.groundCheckPos = this.gc.transform.localPosition;
		this.scalc = MonoSingleton<StyleCalculator>.Instance;
		this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		this.normalSlideGradient = this.slideParticle.GetComponent<ParticleSystem>().trails.colorOverLifetime;
		if (this.difficulty == 0 && this.hp == 100)
		{
			this.hp = 200;
		}
	}

	// Token: 0x0600120C RID: 4620 RVA: 0x0008EBC4 File Offset: 0x0008CDC4
	protected override void OnDestroy()
	{
		base.OnDestroy();
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x0600120D RID: 4621 RVA: 0x0008EBEC File Offset: 0x0008CDEC
	public AudioSource DuplicateDetachWhoosh()
	{
		if (!this.aud3)
		{
			return null;
		}
		float time = this.aud3.time;
		this.aud3.enabled = false;
		GameObject gameObject = Object.Instantiate<GameObject>(this.aud3.gameObject, this.aud3.transform.parent, true);
		Object.Destroy(gameObject.GetComponent<WallCheck>());
		AudioSource component = gameObject.GetComponent<AudioSource>();
		component.time = time;
		component.Play();
		return component;
	}

	// Token: 0x0600120E RID: 4622 RVA: 0x0008EC5E File Offset: 0x0008CE5E
	public AudioSource RestoreWhoosh()
	{
		this.aud3.enabled = true;
		return this.aud3;
	}

	// Token: 0x0600120F RID: 4623 RVA: 0x0008EC74 File Offset: 0x0008CE74
	private void FrictionlessSlideParticle()
	{
		this.CreateSlideScrape(this.currentFrictionlessSlideParticle == null, true);
		Vector3 vector = new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z);
		float num = Mathf.Min(35f, vector.magnitude) / 35f;
		this.currentFrictionlessSlideParticle.transform.localScale = Vector3.one * 0.25f * num;
		this.currentFrictionlessSlideParticle.transform.position = base.transform.position;
		this.currentFrictionlessSlideParticle.transform.rotation = Quaternion.LookRotation(vector.normalized);
		for (int i = 0; i < this.fricSlideAuds.Length; i++)
		{
			this.fricSlideAuds[i].volume = Mathf.Lerp(0f, this.fricSlideAudVols[i], num);
			this.fricSlideAuds[i].pitch = Mathf.Lerp(this.fricSlideAudPitches[i] / 2f, this.fricSlideAudPitches[i], num);
		}
	}

	// Token: 0x06001210 RID: 4624 RVA: 0x0008ED90 File Offset: 0x0008CF90
	private void Update()
	{
		if (this.gc.onGround)
		{
			this.CheckForGasoline();
			if (!this.onGasoline && this.groundProperties != null && this.groundProperties.friction == 0f)
			{
				this.FrictionlessSlideParticle();
			}
			else if (this.currentFrictionlessSlideParticle != null)
			{
				Object.Destroy(this.currentFrictionlessSlideParticle);
			}
		}
		else
		{
			if (this.oilSlideEffect.gameObject.activeSelf)
			{
				this.oilSlideEffect.gameObject.SetActive(false);
			}
			RaycastHit raycastHit;
			if (!this.slopeCheck.onGround && Physics.Raycast(this.gc.transform.position + Vector3.up * 0.1f, Vector3.down, out raycastHit, 0.5f, this.frictionlessSurfaceMask, QueryTriggerInteraction.Ignore))
			{
				if (raycastHit.transform.gameObject.layer == 0 || raycastHit.transform.gameObject.tag == "Slippery")
				{
					this.FrictionlessSlideParticle();
				}
			}
			else if (this.currentFrictionlessSlideParticle != null)
			{
				Object.Destroy(this.currentFrictionlessSlideParticle);
			}
		}
		Vector2 vector = Vector2.zero;
		if (this.activated)
		{
			vector = MonoSingleton<InputManager>.Instance.InputSource.Move.ReadValue<Vector2>();
			this.cc.movementHor = vector.x;
			this.cc.movementVer = vector.y;
			this.movementDirection = Vector3.ClampMagnitude(vector.x * base.transform.right + vector.y * base.transform.forward, 1f);
			if (this.punch == null)
			{
				this.punch = base.GetComponentInChildren<FistControl>();
			}
			else if (!this.punch.enabled)
			{
				this.punch.YesFist();
			}
		}
		else
		{
			if (this.currentFallParticle != null)
			{
				Object.Destroy(this.currentFallParticle);
			}
			if (this.currentSlideParticle != null)
			{
				Object.Destroy(this.currentSlideParticle);
			}
			else if (this.slideScrape != null)
			{
				this.DetachSlideScrape();
			}
			if (this.punch == null)
			{
				this.punch = base.GetComponentInChildren<FistControl>();
			}
			else
			{
				this.punch.NoFist();
			}
		}
		if (MonoSingleton<InputManager>.Instance.LastButtonDevice is Gamepad && this.gamepadFreezeCount > 0)
		{
			vector = Vector2.zero;
			this.rb.velocity = new Vector3(0f, this.rb.velocity.y, 0f);
			this.cc.movementHor = 0f;
			this.cc.movementVer = 0f;
			this.movementDirection = Vector3.zero;
			return;
		}
		if (!this.gc.onGround)
		{
			if (this.fallTime < 1f)
			{
				this.fallTime += Time.deltaTime * 5f;
				if (this.fallTime > 1f)
				{
					this.falling = true;
				}
			}
			else if (this.rb.velocity.y < -2f)
			{
				this.fallSpeed = this.rb.velocity.y;
			}
		}
		else if (this.gc.onGround)
		{
			this.fallTime = 0f;
			this.clingFade = 0f;
		}
		if (!this.gc.onGround && this.rb.velocity.y < -20f)
		{
			this.aud3.pitch = this.rb.velocity.y * -1f / 120f;
			if (this.activated)
			{
				this.aud3.volume = this.rb.velocity.y * -1f / 80f;
			}
			else
			{
				this.aud3.volume = this.rb.velocity.y * -1f / 240f;
			}
		}
		else if (this.rb.velocity.y > -20f)
		{
			this.aud3.pitch = 0f;
			this.aud3.volume = 0f;
		}
		if (this.rb.velocity.y < -100f)
		{
			this.rb.velocity = new Vector3(this.rb.velocity.x, -100f, this.rb.velocity.z);
		}
		if (this.gc.onGround && this.falling && !this.jumpCooldown)
		{
			this.falling = false;
			this.slamStorage = false;
			if (this.fallSpeed > -50f)
			{
				this.aud2.clip = this.landingSound;
				this.aud2.volume = 0.5f + this.fallSpeed * -0.01f;
				this.aud2.pitch = Random.Range(0.9f, 1.1f);
				this.aud2.Play();
				MonoSingleton<PlayerAnimations>.Instance.Footstep(0.5f, true, 0f);
				MonoSingleton<PlayerAnimations>.Instance.Footstep(0.5f, true, 0.05f);
			}
			else
			{
				this.gc.hasImpacted = true;
				this.LandingImpact();
			}
			this.fallSpeed = 0f;
			this.gc.heavyFall = false;
			if (this.currentFallParticle != null)
			{
				Object.Destroy(this.currentFallParticle);
			}
		}
		if (!this.gc.onGround && this.activated && MonoSingleton<InputManager>.Instance.InputSource.Slide.WasPerformedThisFrame && !GameStateManager.Instance.PlayerInputLocked)
		{
			if (this.sliding)
			{
				this.StopSlide();
			}
			if (this.boost)
			{
				this.boostLeft = 0f;
				this.boost = false;
			}
			RaycastHit raycastHit2;
			if (this.fallTime > 0.5f && this.slamCooldown == 0f && !Physics.Raycast(this.gc.transform.position + base.transform.up, base.transform.up * -1f, out raycastHit2, 3f, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore) && !this.gc.heavyFall)
			{
				this.stillHolding = true;
				this.rb.velocity = new Vector3(0f, -100f, 0f);
				this.falling = true;
				this.fallSpeed = -100f;
				this.gc.heavyFall = true;
				this.slamForce = 1f;
				if (this.currentFallParticle != null)
				{
					Object.Destroy(this.currentFallParticle);
				}
				this.currentFallParticle = Object.Instantiate<GameObject>(this.fallParticle, base.transform);
			}
		}
		if (this.gc.heavyFall && !this.slamStorage)
		{
			this.rb.velocity = new Vector3(0f, -100f, 0f);
		}
		if (this.gc.heavyFall || this.sliding)
		{
			Physics.IgnoreLayerCollision(2, 12, true);
		}
		else
		{
			Physics.IgnoreLayerCollision(2, 12, false);
		}
		if (!this.slopeCheck.onGround && this.slopeCheck.forcedOff <= 0 && this.modForcedFrictionMultip != 0f && !this.jumping && !this.boost)
		{
			float num = this.playerCollider.height / 2f - this.playerCollider.center.y;
			RaycastHit raycastHit3;
			if (this.rb.velocity != Vector3.zero && Physics.Raycast(base.transform.position, base.transform.up * -1f, out raycastHit3, num + 1f, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
			{
				Vector3 vector2 = new Vector3(base.transform.position.x, base.transform.position.y - raycastHit3.distance + num, base.transform.position.z);
				base.transform.position = Vector3.MoveTowards(base.transform.position, vector2, raycastHit3.distance * Time.deltaTime * 10f);
				if (this.rb.velocity.y > 0f)
				{
					this.rb.velocity = new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z);
				}
			}
		}
		if (this.gc.heavyFall)
		{
			this.slamForce += Time.deltaTime * 5f;
			RaycastHit raycastHit4;
			if (Physics.Raycast(this.gc.transform.position + base.transform.up, base.transform.up * -1f, out raycastHit4, 5f, LayerMaskDefaults.Get(LMD.Environment)) || Physics.SphereCast(this.gc.transform.position + base.transform.up, 1f, base.transform.up * -1f, out raycastHit4, 5f, LayerMaskDefaults.Get(LMD.Environment)))
			{
				Breakable component = raycastHit4.collider.GetComponent<Breakable>();
				if (component != null && ((component.weak && !component.precisionOnly && !component.specialCaseOnly) || component.forceGroundSlammable) && !component.unbreakable)
				{
					Object.Instantiate<GameObject>(this.impactDust, raycastHit4.point, Quaternion.identity);
					component.Break();
				}
				Bleeder bleeder;
				if (raycastHit4.collider.gameObject.TryGetComponent<Bleeder>(out bleeder))
				{
					bleeder.GetHit(raycastHit4.point, GoreType.Head, false);
				}
				Idol idol;
				if (raycastHit4.transform.TryGetComponent<Idol>(out idol))
				{
					idol.Death();
				}
			}
		}
		if (this.stillHolding && MonoSingleton<InputManager>.Instance.InputSource.Slide.WasCanceledThisFrame)
		{
			this.stillHolding = false;
		}
		if (this.activated)
		{
			if (!GameStateManager.Instance.PlayerInputLocked && MonoSingleton<InputManager>.Instance.InputSource.Jump.WasPerformedThisFrame && (!this.falling || this.gc.canJump || this.wc.CheckForEnemyCols()) && !this.jumpCooldown)
			{
				if (this.gc.canJump || this.wc.CheckForEnemyCols())
				{
					this.enemyStepping = true;
					this.EnemyStepResets();
				}
				this.Jump();
			}
			if (!this.gc.onGround && this.wc.onWall)
			{
				RaycastHit raycastHit5;
				if (!this.sliding && Physics.Raycast(base.transform.position, this.movementDirection, out raycastHit5, 1f, LayerMaskDefaults.Get(LMD.Environment)))
				{
					if (this.rb.velocity.y < -1f && !this.gc.heavyFall)
					{
						this.rb.velocity = new Vector3(Mathf.Clamp(this.rb.velocity.x, -1f, 1f), -2f * this.clingFade, Mathf.Clamp(this.rb.velocity.z, -1f, 1f));
						this.CreateWallScrape(raycastHit5.point + Vector3.up, this.wallScrape == null);
						this.wallScrape.transform.forward = raycastHit5.normal;
						this.clingFade = Mathf.MoveTowards(this.clingFade, 50f, Time.deltaTime * 4f);
					}
				}
				else if (this.wallScrape != null)
				{
					this.DetachWallScrape();
				}
				if (!GameStateManager.Instance.PlayerInputLocked && MonoSingleton<InputManager>.Instance.InputSource.Jump.WasPerformedThisFrame && !this.jumpCooldown && this.currentWallJumps < 3 && this.wc && this.wc.CheckForCols())
				{
					this.WallJump();
				}
			}
			else if (this.wallScrape != null)
			{
				this.DetachWallScrape();
			}
		}
		if (!GameStateManager.Instance.PlayerInputLocked && !GameStateManager.Instance.IsStateActive("alter-menu"))
		{
			if (MonoSingleton<InputManager>.Instance.InputSource.Slide.WasPerformedThisFrame && (this.gc.onGround || this.gc.sinceLastGrounded < 0.03f) && this.activated && (!this.slowMode || this.crouching) && !GameStateManager.Instance.PlayerInputLocked && !this.sliding)
			{
				this.StartSlide();
			}
			RaycastHit raycastHit6;
			if (MonoSingleton<InputManager>.Instance.InputSource.Slide.WasPerformedThisFrame && !this.gc.onGround && !this.sliding && !this.jumping && this.activated && !this.slowMode && !GameStateManager.Instance.PlayerInputLocked && Physics.Raycast(this.gc.transform.position + base.transform.up, base.transform.up * -1f, out raycastHit6, 2f, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
			{
				this.StartSlide();
			}
		}
		if ((MonoSingleton<InputManager>.Instance.InputSource.Slide.WasCanceledThisFrame || (this.slowMode && !this.crouching)) && this.sliding)
		{
			this.StopSlide();
		}
		if (this.sliding && this.activated)
		{
			this.standing = false;
			this.slideLength += Time.deltaTime;
			if (this.cc.defaultTarget.y != this.cc.originalPos.y - 0.625f)
			{
				this.cc.defaultTarget = this.cc.originalPos - Vector3.up * 0.625f;
			}
			Vector3 normalized = Vector3.ProjectOnPlane(this.rb.velocity.normalized, Vector3.up).normalized;
			if (this.currentSlideParticle != null)
			{
				this.currentSlideParticle.transform.position = base.transform.position + normalized * 10f;
				this.currentSlideParticle.transform.forward = -this.dodgeDirection;
				this.slideTrail.colorOverLifetime = ((this.boostLeft > 0f && base.gameObject.layer == 15) ? this.invincibleSlideGradient : this.normalSlideGradient);
			}
			if (this.slideSafety > 0f)
			{
				this.slideSafety -= Time.deltaTime * 5f;
			}
			if (this.slideScrape)
			{
				if (this.gc.onGround || this.wc.onWall)
				{
					this.slideScrape.transform.position = base.transform.position + normalized;
					this.slideScrape.transform.forward = -normalized;
					this.cc.CameraShake(0.1f);
				}
				else
				{
					this.slideScrape.transform.position = Vector3.one * 5000f;
				}
			}
		}
		else if (this.groundProperties && this.groundProperties.forceCrouch)
		{
			this.playerCollider.height = 1.25f;
			this.crouching = true;
			if (this.standing)
			{
				this.standing = false;
				base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - 1.125f, base.transform.position.z);
				this.gc.transform.localPosition = this.groundCheckPos + Vector3.up * 1.125f;
			}
			this.cc.defaultTarget = this.cc.originalPos - Vector3.up * 0.625f;
		}
		else
		{
			if (this.activated)
			{
				if (this.playerCollider && this.playerCollider.height != 3.5f)
				{
					Vector3 vector3 = new Vector3(this.playerCollider.bounds.center.x, this.playerCollider.bounds.min.y, this.playerCollider.bounds.center.z);
					if (!Physics.Raycast(vector3, Vector3.up, 3.5f, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore) && !Physics.SphereCast(new Ray(vector3 + Vector3.up * 0.25f, Vector3.up), 0.5f, 2f, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
					{
						this.playerCollider.height = 3.5f;
						this.gc.transform.localPosition = this.groundCheckPos;
						if (Physics.Raycast(base.transform.position, Vector3.up * -1f, 2.25f, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
						{
							base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 1.125f, base.transform.position.z);
						}
						else
						{
							base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - 0.625f, base.transform.position.z);
							this.cc.defaultTarget = this.cc.originalPos;
							this.standing = true;
						}
						if (this.crouching)
						{
							this.crouching = false;
							this.slowMode = false;
						}
					}
					else
					{
						this.crouching = true;
						this.slowMode = true;
					}
				}
				else if (this.cc.defaultTarget != this.cc.originalPos)
				{
					this.cc.defaultTarget = this.cc.originalPos;
				}
				else
				{
					this.standing = true;
				}
			}
			if (this.currentSlideParticle != null)
			{
				Object.Destroy(this.currentSlideParticle);
			}
			if (this.slideScrape != null)
			{
				this.DetachSlideScrape();
			}
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Dodge.WasPerformedThisFrame && this.activated && !this.slowMode && !GameStateManager.Instance.PlayerInputLocked)
		{
			if ((this.groundProperties && !this.groundProperties.canDash) || this.modNoDashSlide)
			{
				if (this.modNoDashSlide || !this.groundProperties.silentDashFail)
				{
					Object.Instantiate<GameObject>(this.staminaFailSound);
				}
			}
			else if (this.boostCharge >= 100f)
			{
				if (this.sliding)
				{
					this.StopSlide();
				}
				this.boostLeft = 100f;
				this.dashStorage = 1f;
				this.boost = true;
				this.dodgeDirection = this.movementDirection;
				if (this.dodgeDirection == Vector3.zero)
				{
					this.dodgeDirection = base.transform.forward;
				}
				Quaternion identity = Quaternion.identity;
				identity.SetLookRotation(this.dodgeDirection * -1f);
				Object.Instantiate<GameObject>(this.dodgeParticle, base.transform.position + this.dodgeDirection * 10f, identity);
				if (!this.asscon.majorEnabled || !this.asscon.infiniteStamina)
				{
					this.boostCharge -= 100f;
				}
				if (this.dodgeDirection == base.transform.forward)
				{
					this.cc.dodgeDirection = 0;
				}
				else if (this.dodgeDirection == base.transform.forward * -1f)
				{
					this.cc.dodgeDirection = 1;
				}
				else
				{
					this.cc.dodgeDirection = 2;
				}
				this.aud.clip = this.dodgeSound;
				this.aud.volume = 1f;
				this.aud.pitch = 1f;
				this.aud.Play();
				MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.Dash);
				if (this.gc.heavyFall)
				{
					this.fallSpeed = 0f;
					this.gc.heavyFall = false;
					if (this.currentFallParticle != null)
					{
						Object.Destroy(this.currentFallParticle);
					}
				}
			}
			else
			{
				Object.Instantiate<GameObject>(this.staminaFailSound);
			}
		}
		if (!this.walking && vector.sqrMagnitude > 0f && !this.sliding && this.gc.onGround)
		{
			this.walking = true;
		}
		else if ((this.walking && Mathf.Approximately(vector.sqrMagnitude, 0f)) || !this.gc.onGround || this.sliding)
		{
			this.walking = false;
		}
		if (this.hurtInvincibility >= 0f && this.hp > 0)
		{
			this.hurtInvincibility = Mathf.MoveTowards(this.hurtInvincibility, 0f, Time.deltaTime);
		}
		if (this.currentColor.a > 0f)
		{
			this.currentColor.a = this.currentColor.a - Time.deltaTime;
			Shader.SetGlobalColor("_HurtScreenColor", this.currentColor);
		}
		if (this.safeExplosionLaunchCooldown > 0f)
		{
			this.safeExplosionLaunchCooldown = Mathf.MoveTowards(this.safeExplosionLaunchCooldown, 0f, Time.deltaTime);
		}
		if (this.boostCharge != 300f && !this.sliding && !this.slowMode)
		{
			float num2 = 1f;
			if (this.difficulty == 1)
			{
				num2 = 1.5f;
			}
			else if (this.difficulty == 0)
			{
				num2 = 2f;
			}
			this.boostCharge = Mathf.MoveTowards(this.boostCharge, 300f, 70f * Time.deltaTime * num2);
		}
		if (this.slamCooldown > 0f)
		{
			this.slamCooldown = Mathf.MoveTowards(this.slamCooldown, 0f, Time.deltaTime);
		}
		Vector3 vector4 = this.hudOriginalPos - this.cc.transform.InverseTransformDirection(this.rb.velocity) / 1000f;
		float num3 = Vector3.Distance(vector4, this.screenHud.transform.localPosition);
		this.screenHud.transform.localPosition = Vector3.MoveTowards(this.screenHud.transform.localPosition, vector4, Time.deltaTime * 15f * num3);
		Vector3 vector5 = Vector3.ClampMagnitude(this.camOriginalPos - this.cc.transform.InverseTransformDirection(this.rb.velocity) / 350f * -1f, 0.2f);
		float num4 = Vector3.Distance(vector5, this.hudCam.transform.localPosition);
		this.hudCam.transform.localPosition = Vector3.MoveTowards(this.hudCam.transform.localPosition, vector5, Time.deltaTime * 25f * num4);
		int rankIndex = MonoSingleton<StyleHUD>.Instance.rankIndex;
		if ((rankIndex == 7 || this.difficulty <= 1) && !this.cantInstaHeal)
		{
			this.antiHp = 0f;
			this.antiHpCooldown = 0f;
		}
		else if (this.antiHpCooldown > 0f)
		{
			if (rankIndex >= 4)
			{
				this.antiHpCooldown = Mathf.MoveTowards(this.antiHpCooldown, 0f, Time.deltaTime * (float)(rankIndex / 2));
			}
			else
			{
				this.antiHpCooldown = Mathf.MoveTowards(this.antiHpCooldown, 0f, Time.deltaTime);
			}
		}
		else if (this.antiHp > 0f)
		{
			this.cantInstaHeal = false;
			if (rankIndex >= 4)
			{
				this.antiHp = Mathf.MoveTowards(this.antiHp, 0f, Time.deltaTime * (float)rankIndex * 10f);
			}
			else
			{
				this.antiHp = Mathf.MoveTowards(this.antiHp, 0f, Time.deltaTime * 15f);
			}
		}
		if (!this.gc.heavyFall && this.currentFallParticle != null)
		{
			Object.Destroy(this.currentFallParticle);
		}
	}

	// Token: 0x06001211 RID: 4625 RVA: 0x00090720 File Offset: 0x0008E920
	private void FixedUpdate()
	{
		this.friction = this.modForcedFrictionMultip * (this.groundProperties ? this.groundProperties.friction : 1f);
		if (this.sliding)
		{
			if (this.slideSafety <= 0f)
			{
				Vector3 vector = new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z);
				float num = 10f;
				if (this.groundProperties && this.groundProperties.speedMultiplier < 1f)
				{
					num *= this.groundProperties.speedMultiplier;
				}
				if (vector.magnitude < num)
				{
					this.slideSafety = Mathf.MoveTowards(this.slideSafety, -0.1f, Time.deltaTime);
					if (this.slideSafety <= -0.1f)
					{
						this.StopSlide();
					}
				}
				else
				{
					this.slideSafety = 0f;
				}
			}
			if (this.wc.onWall && this.rb.velocity.y < 0f)
			{
				this.rb.AddForce(-Physics.gravity * 0.4f, ForceMode.Acceleration);
			}
		}
		if (!this.sliding && this.activated)
		{
			this.framesSinceSlide++;
			if (this.gc.heavyFall)
			{
				this.preSlideDelay = 0.2f;
				this.preSlideSpeed = this.slamForce;
				RaycastHit raycastHit;
				if (Physics.SphereCast(base.transform.position - Vector3.up * 1.5f, 0.35f, Vector3.down, out raycastHit, Time.fixedDeltaTime * Mathf.Abs(this.rb.velocity.y), LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
				{
					base.transform.position = raycastHit.point + Vector3.up * 1.5f;
					this.rb.velocity = Vector3.zero;
				}
			}
			else if (!this.boost && this.falling && this.rb.velocity.magnitude / 24f > this.preSlideSpeed)
			{
				this.preSlideSpeed = this.rb.velocity.magnitude / 24f;
				this.preSlideDelay = 0.2f;
			}
			else
			{
				this.preSlideDelay = Mathf.MoveTowards(this.preSlideDelay, 0f, Time.fixedDeltaTime);
				if (this.preSlideDelay <= 0f)
				{
					this.preSlideDelay = 0.2f;
					this.preSlideSpeed = this.rb.velocity.magnitude / 24f;
				}
			}
		}
		if (!this.boost)
		{
			this.Move();
			return;
		}
		this.rb.useGravity = true;
		this.Dodge();
	}

	// Token: 0x06001212 RID: 4626 RVA: 0x00090A10 File Offset: 0x0008EC10
	private void Move()
	{
		this.slideEnding = false;
		if (this.hurtInvincibility <= 0f && !this.levelOver)
		{
			base.gameObject.layer = 2;
			this.exploded = false;
		}
		if (this.gc.onGround && !this.jumping)
		{
			this.currentWallJumps = 0;
			this.rocketJumps = 0;
			this.hammerJumps = 0;
			this.rocketRides = 0;
		}
		if (this.gc.onGround && this.friction > 0f && !this.jumping)
		{
			float num = this.rb.velocity.y;
			if (this.slopeCheck.onGround && this.movementDirection.x == 0f && this.movementDirection.z == 0f)
			{
				num = 0f;
				this.rb.useGravity = false;
			}
			else
			{
				this.rb.useGravity = true;
			}
			float num2 = 2.75f;
			if (this.slowMode)
			{
				num2 = 1.25f;
			}
			if (this.groundProperties)
			{
				num2 *= this.groundProperties.speedMultiplier;
			}
			this.movementDirection2 = new Vector3(this.movementDirection.x * this.walkSpeed * Time.deltaTime * num2, num, this.movementDirection.z * this.walkSpeed * Time.deltaTime * num2);
			Vector3 vector = this.pushForce;
			if (this.groundProperties && this.groundProperties.push)
			{
				Vector3 vector2 = this.groundProperties.pushForce;
				if (this.groundProperties.pushDirectionRelative)
				{
					vector2 = this.groundProperties.transform.rotation * vector2;
				}
				vector += vector2;
			}
			this.rb.velocity = Vector3.Lerp(this.rb.velocity, this.movementDirection2 + vector, 0.25f * this.friction);
			return;
		}
		this.rb.useGravity = true;
		if (this.slowMode)
		{
			this.movementDirection2 = new Vector3(this.movementDirection.x * this.walkSpeed * Time.deltaTime * 1.25f, this.rb.velocity.y, this.movementDirection.z * this.walkSpeed * Time.deltaTime * 1.25f);
		}
		else
		{
			this.movementDirection2 = new Vector3(this.movementDirection.x * this.walkSpeed * Time.deltaTime * 2.75f, this.rb.velocity.y, this.movementDirection.z * this.walkSpeed * Time.deltaTime * 2.75f);
		}
		this.airDirection.y = 0f;
		if ((this.movementDirection2.x > 0f && this.rb.velocity.x < this.movementDirection2.x) || (this.movementDirection2.x < 0f && this.rb.velocity.x > this.movementDirection2.x))
		{
			this.airDirection.x = this.movementDirection2.x;
		}
		else
		{
			this.airDirection.x = 0f;
		}
		if ((this.movementDirection2.z > 0f && this.rb.velocity.z < this.movementDirection2.z) || (this.movementDirection2.z < 0f && this.rb.velocity.z > this.movementDirection2.z))
		{
			this.airDirection.z = this.movementDirection2.z;
		}
		else
		{
			this.airDirection.z = 0f;
		}
		this.rb.AddForce(this.airDirection.normalized * this.airAcceleration);
	}

	// Token: 0x06001213 RID: 4627 RVA: 0x00090E08 File Offset: 0x0008F008
	private void Dodge()
	{
		if (this.sliding)
		{
			if (this.hurtInvincibility <= 0f && !this.levelOver && this.boostLeft <= 0f)
			{
				base.gameObject.layer = 2;
				this.exploded = false;
			}
			float num = 1f;
			if (this.preSlideSpeed > 1f)
			{
				if (this.preSlideSpeed > 3f)
				{
					this.preSlideSpeed = 3f;
				}
				num = this.preSlideSpeed;
				if (this.gc.onGround && this.friction != 0f)
				{
					this.preSlideSpeed -= Time.fixedDeltaTime * this.preSlideSpeed * this.friction;
				}
				this.preSlideDelay = 0f;
			}
			if (this.modNoDashSlide)
			{
				this.StopSlide();
				return;
			}
			if (this.groundProperties)
			{
				if (!this.groundProperties.canSlide)
				{
					this.StopSlide();
					return;
				}
				num *= this.groundProperties.speedMultiplier;
			}
			Vector3 vector = new Vector3(this.dodgeDirection.x * this.walkSpeed * Time.deltaTime * 4f * num, this.rb.velocity.y, this.dodgeDirection.z * this.walkSpeed * Time.deltaTime * 4f * num);
			if (this.groundProperties && this.groundProperties.push)
			{
				Vector3 vector2 = this.groundProperties.pushForce;
				if (this.groundProperties.pushDirectionRelative)
				{
					vector2 = this.groundProperties.transform.rotation * vector2;
				}
				vector += vector2;
			}
			if (this.gc.onGround)
			{
				this.CreateSlideScrape(false, false);
			}
			if (this.boostLeft > 0f)
			{
				this.dashStorage = Mathf.MoveTowards(this.dashStorage, 0f, Time.fixedDeltaTime);
				if (this.dashStorage <= 0f)
				{
					this.boostLeft = 0f;
				}
			}
			Vector2 vector3 = MonoSingleton<InputManager>.Instance.InputSource.Move.ReadValue<Vector2>();
			this.movementDirection = Vector3.ClampMagnitude(vector3.x * base.transform.right, 1f) * 5f;
			if (!MonoSingleton<HookArm>.Instance || !MonoSingleton<HookArm>.Instance.beingPulled)
			{
				this.rb.velocity = vector + this.pushForce + this.movementDirection;
				return;
			}
			this.StopSlide();
			return;
		}
		else
		{
			float num2 = 0f;
			if (this.slideEnding)
			{
				num2 = this.rb.velocity.y;
			}
			float num3 = 2.75f;
			this.movementDirection2 = new Vector3(this.dodgeDirection.x * this.walkSpeed * Time.deltaTime * num3, num2, this.dodgeDirection.z * this.walkSpeed * Time.deltaTime * num3);
			base.gameObject.layer = 15;
			if (this.slideEnding)
			{
				this.slideEnding = false;
				if (!this.gc.onGround || this.friction == 0f)
				{
					this.boost = false;
					return;
				}
			}
			if (this.boostLeft > 0f)
			{
				this.rb.velocity = this.movementDirection2 * 3f;
				this.boostLeft -= 4f;
				return;
			}
			if (!this.gc.onGround || this.friction != 0f)
			{
				this.rb.velocity = this.movementDirection2;
			}
			this.boost = false;
			return;
		}
	}

	// Token: 0x06001214 RID: 4628 RVA: 0x000911A8 File Offset: 0x0008F3A8
	public void Jump()
	{
		float num = 1500f;
		if (this.modNoJump || this.groundProperties)
		{
			if (this.modNoJump || !this.groundProperties.canJump)
			{
				if (this.modNoJump || !this.groundProperties.silentJumpFail)
				{
					this.aud.clip = this.jumpSound;
					this.aud.volume = 0.75f;
					this.aud.pitch = 0.25f;
					this.aud.Play();
				}
				return;
			}
			num *= this.groundProperties.jumpForceMultiplier;
		}
		this.jumping = true;
		base.CancelInvoke("NotJumping");
		base.Invoke("NotJumping", 0.25f);
		this.falling = true;
		if (this.quakeJump)
		{
			Object.Instantiate<GameObject>(this.quakeJumpSound).GetComponent<AudioSource>().pitch = 1f + Random.Range(0f, 0.1f);
		}
		this.aud.clip = this.jumpSound;
		if (this.gc.superJumpChance > 0f || this.gc.bounceChance > 0f)
		{
			this.aud.volume = 0.85f;
			this.aud.pitch = 2f;
		}
		else
		{
			this.aud.volume = 0.75f;
			this.aud.pitch = 1f;
		}
		this.aud.Play();
		this.rb.velocity = new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z);
		if (this.sliding)
		{
			if (this.slowMode)
			{
				this.rb.AddForce(Vector3.up * this.jumpPower * num);
			}
			else
			{
				this.rb.AddForce(Vector3.up * this.jumpPower * num * 2f);
			}
			this.StopSlide();
		}
		else if (this.boost)
		{
			if (this.enemyStepping)
			{
				Object.Instantiate<GameObject>(this.dashJumpSound);
			}
			else if (this.boostCharge >= 100f)
			{
				if (!this.asscon.majorEnabled || !this.asscon.infiniteStamina)
				{
					this.boostCharge -= 100f;
				}
				Object.Instantiate<GameObject>(this.dashJumpSound);
			}
			else
			{
				this.rb.velocity = new Vector3(this.movementDirection.x * this.walkSpeed * Time.deltaTime * 2.75f, 0f, this.movementDirection.z * this.walkSpeed * Time.deltaTime * 2.75f);
				Object.Instantiate<GameObject>(this.staminaFailSound);
			}
			if (this.slowMode)
			{
				this.rb.AddForce(Vector3.up * this.jumpPower * num * 0.75f);
			}
			else
			{
				this.rb.AddForce(Vector3.up * this.jumpPower * num * 1.5f);
			}
		}
		else if (this.slowMode)
		{
			this.rb.AddForce(Vector3.up * this.jumpPower * num * 1.25f);
		}
		else if (this.gc.superJumpChance > 0f || this.gc.bounceChance > 0f || this.gc.extraJumpChance > 0f)
		{
			if (this.slamForce < 5.5f)
			{
				this.rb.AddForce(Vector3.up * this.jumpPower * num * (3f + (this.slamForce - 1f)));
			}
			else
			{
				this.rb.AddForce(Vector3.up * this.jumpPower * num * 12.5f);
			}
		}
		else
		{
			this.rb.AddForce(Vector3.up * this.jumpPower * num * 2.6f);
		}
		this.TrySSJ(this.dodgeDirection.normalized, 0.5f, (int frame) => 1f / Mathf.Pow(2f, (float)(frame - 1)));
		this.jumpCooldown = true;
		base.CancelInvoke("JumpReady");
		base.Invoke("JumpReady", 0.2f);
		this.boost = false;
		this.gc.bounceChance = 0f;
		this.gc.heavyFall = false;
		this.enemyStepping = false;
	}

	// Token: 0x06001215 RID: 4629 RVA: 0x00091680 File Offset: 0x0008F880
	private void TrySSJ(Vector3 direction, float speedMultiplier, Func<int, float> speedLossFormula)
	{
		if (this.framesSinceSlide > 0 && (float)this.framesSinceSlide < this.ssjMaxFrames && !this.boost)
		{
			float num = speedLossFormula(this.framesSinceSlide);
			float num2 = speedMultiplier * this.walkSpeed * 2.75f * 3f * Time.fixedDeltaTime;
			float y = this.rb.velocity.y;
			float num3 = num * num2;
			this.rb.velocity = this.velocityAfterSlide + direction * num3;
			this.rb.velocity = new Vector3(this.rb.velocity.x, y, this.rb.velocity.z);
			this.rb.velocity = Mathf.Min(this.rb.velocity.magnitude, 100f) * this.rb.velocity.normalized;
			if (MonoSingleton<PrefsManager>.Instance.GetBool("ssjIndicator", false))
			{
				MonoSingleton<SubtitleController>.Instance.DisplaySubtitle(string.Format("SSJ: {0} (+{1})u/s, {2}% speed (Frame {3}/{4})", new object[]
				{
					this.rb.velocity.magnitude,
					num3,
					num * 100f,
					this.framesSinceSlide,
					this.ssjMaxFrames - 1f
				}), null, true);
			}
		}
	}

	// Token: 0x06001216 RID: 4630 RVA: 0x00091808 File Offset: 0x0008FA08
	private void WallJump()
	{
		this.jumping = true;
		base.Invoke("NotJumping", 0.25f);
		this.currentWallJumps++;
		if (this.gc.heavyFall)
		{
			this.slamStorage = true;
		}
		if (this.quakeJump)
		{
			Object.Instantiate<GameObject>(this.quakeJumpSound).GetComponent<AudioSource>().pitch = 1.1f + (float)this.currentWallJumps * 0.05f;
		}
		this.aud.clip = this.jumpSound;
		this.aud.pitch += 0.25f;
		this.aud.volume = 0.75f;
		this.aud.Play();
		if (this.currentWallJumps == 3)
		{
			this.aud2.clip = this.finalWallJump;
			this.aud2.volume = 0.75f;
			this.aud2.Play();
		}
		this.wallJumpPos = base.transform.position - this.wc.poc;
		CustomGroundProperties customGroundProperties;
		if (this.wc.currentCollider && (this.wc.currentCollider.TryGetComponent<CustomGroundProperties>(out customGroundProperties) || (this.wc.currentCollider.attachedRigidbody && this.wc.currentCollider.attachedRigidbody.TryGetComponent<CustomGroundProperties>(out customGroundProperties))) && (customGroundProperties.overrideFootsteps || customGroundProperties.overrideSurfaceType))
		{
			MonoSingleton<PlayerAnimations>.Instance.WallJump(customGroundProperties);
		}
		else
		{
			MonoSingleton<PlayerAnimations>.Instance.WallJump(this.wc.poc);
		}
		if (NonConvexJumpDebug.Active)
		{
			for (int i = 0; i < 4; i++)
			{
				NonConvexJumpDebug.CreateBall(Color.white, Vector3.Lerp(base.transform.position, this.wc.poc, (float)i / 4f), 0.4f);
			}
		}
		if (this.sliding || this.framesSinceSlide < MonoSingleton<PrefsManager>.Instance.GetIntLocal("ssjMaxFrames", 4))
		{
			Vector3.ProjectOnPlane(this.rb.velocity, Vector3.up);
			Vector3 vector = Vector3.Reflect(this.dodgeDirection.normalized, this.wallJumpPos.normalized);
			vector = Vector3.ProjectOnPlane(vector, Vector3.up).normalized;
			vector = (vector + this.wallJumpPos.normalized * 0.35f).normalized;
			this.dodgeDirection = vector;
			this.rb.velocity = vector.normalized * this.rb.velocity.magnitude;
			this.TrySSJ(vector, 0.75f, (int frame) => (this.ssjMaxFrames - (float)frame + 1f) / this.ssjMaxFrames);
			this.rb.velocity = new Vector3(this.rb.velocity.x, Mathf.Max(this.rb.velocity.y, 15f), this.rb.velocity.z);
		}
		else
		{
			this.boost = false;
			this.rb.velocity = Vector3.zero;
			Vector3 vector2 = new Vector3(this.wallJumpPos.normalized.x, 1f, this.wallJumpPos.normalized.z);
			this.rb.AddForce(vector2 * 2000f * this.wallJumpPower);
		}
		this.jumpCooldown = true;
		base.Invoke("JumpReady", 0.1f);
	}

	// Token: 0x06001217 RID: 4631 RVA: 0x00091B84 File Offset: 0x0008FD84
	private void OnCollisionEnter(Collision other)
	{
		if (this.sliding)
		{
			foreach (ContactPoint contactPoint in other.contacts)
			{
				Vector3.Angle(Vector3.ProjectOnPlane(this.dodgeDirection, contactPoint.normal).normalized, this.dodgeDirection);
			}
		}
	}

	// Token: 0x06001218 RID: 4632 RVA: 0x00091BE2 File Offset: 0x0008FDE2
	public void LaunchUp(float multiplier)
	{
		this.Launch(Vector3.up, multiplier, true);
	}

	// Token: 0x06001219 RID: 4633 RVA: 0x00091BF4 File Offset: 0x0008FDF4
	public void Launch(Vector3 direction, float multiplier = 8f, bool ignoreMass = false)
	{
		if (this.groundProperties && !this.groundProperties.launchable)
		{
			return;
		}
		if (direction == Vector3.down && this.gc.onGround)
		{
			return;
		}
		this.jumping = true;
		base.Invoke("NotJumping", 0.5f);
		this.jumpCooldown = true;
		base.Invoke("JumpReady", 0.2f);
		this.boost = false;
		if (this.gc.heavyFall)
		{
			this.fallSpeed = 0f;
			this.gc.heavyFall = false;
			if (this.currentFallParticle != null)
			{
				Object.Destroy(this.currentFallParticle);
			}
		}
		if (direction.magnitude > 0f)
		{
			this.rb.velocity = Vector3.zero;
		}
		this.rb.AddForce(Vector3.ClampMagnitude(direction, 1000f) * multiplier, ignoreMass ? ForceMode.VelocityChange : ForceMode.Impulse);
	}

	// Token: 0x0600121A RID: 4634 RVA: 0x00091CEC File Offset: 0x0008FEEC
	public void LaunchFromPoint(Vector3 position, float strength, float maxDistance = 1f)
	{
		if (this.groundProperties && !this.groundProperties.launchable)
		{
			return;
		}
		Vector3 vector = (base.transform.position - position).normalized;
		if (position == base.transform.position)
		{
			vector = Vector3.up;
		}
		Vector3 vector2;
		if (this.jumping)
		{
			vector2 = vector * maxDistance * strength;
			vector2.y = 0.5f * maxDistance * strength;
		}
		else
		{
			float num = maxDistance - Vector3.Distance(base.transform.position, position);
			vector2 = vector * num * strength;
			vector2.y = 0.5f * num * strength;
		}
		this.Launch(vector2, 8f, false);
	}

	// Token: 0x0600121B RID: 4635 RVA: 0x00091DB0 File Offset: 0x0008FFB0
	public void LaunchFromPointAtSpeed(Vector3 position, float speed)
	{
		if (this.groundProperties && !this.groundProperties.launchable)
		{
			return;
		}
		Vector3 vector = (base.transform.position - position).normalized;
		if (position == base.transform.position)
		{
			vector = Vector3.up;
		}
		Vector3 vector2 = vector * speed;
		vector2.y = Mathf.Max(0.5f * speed, vector2.y);
		this.Launch(vector2, 1f, true);
	}

	// Token: 0x0600121C RID: 4636 RVA: 0x00091E3C File Offset: 0x0009003C
	public void Slamdown(float strength)
	{
		this.boost = false;
		if (this.gc.heavyFall)
		{
			this.fallSpeed = 0f;
			this.gc.heavyFall = false;
			if (this.currentFallParticle != null)
			{
				Object.Destroy(this.currentFallParticle);
			}
		}
		this.rb.velocity = Vector3.zero;
		this.rb.velocity = new Vector3(0f, -strength, 0f);
	}

	// Token: 0x0600121D RID: 4637 RVA: 0x00091EB9 File Offset: 0x000900B9
	private void JumpReady()
	{
		this.jumpCooldown = false;
	}

	// Token: 0x0600121E RID: 4638 RVA: 0x00091EC4 File Offset: 0x000900C4
	public void FakeHurt(bool silent = false)
	{
		this.currentColor.a = 0.25f;
		this.cc.CameraShake(0.1f);
		if (!silent)
		{
			this.hurtAud.pitch = Random.Range(0.8f, 1f);
			this.hurtAud.PlayOneShot(this.hurtAud.clip);
		}
	}

	// Token: 0x0600121F RID: 4639 RVA: 0x00091F24 File Offset: 0x00090124
	public void GetHurt(int damage, bool invincible, float scoreLossMultiplier = 1f, bool explosion = false, bool instablack = false, float hardDamageMultiplier = 0.35f, bool ignoreInvincibility = false)
	{
		if (!this.dead && !this.levelOver && (!invincible || base.gameObject.layer != 15 || ignoreInvincibility) && damage > 0)
		{
			if (explosion)
			{
				this.exploded = true;
			}
			if (this.asscon.majorEnabled)
			{
				damage = Mathf.RoundToInt((float)damage * this.asscon.damageTaken);
			}
			if (Invincibility.Enabled)
			{
				damage = 0;
			}
			if (damage >= 50)
			{
				this.currentColor.a = 0.8f;
			}
			else
			{
				this.currentColor.a = 0.5f;
			}
			if (invincible)
			{
				this.hurtInvincibility = this.currentColor.a;
				base.gameObject.layer = 15;
			}
			this.cc.CameraShake((float)(damage / 20));
			this.hurtAud.pitch = Random.Range(0.8f, 1f);
			this.hurtAud.PlayOneShot(this.hurtAud.clip);
			if (this.hp - damage > 0)
			{
				this.hp -= damage;
			}
			else
			{
				this.hp = 0;
			}
			if (invincible && scoreLossMultiplier != 0f && this.difficulty >= 2 && (!this.asscon.majorEnabled || !this.asscon.disableHardDamage) && this.hp <= 100)
			{
				if (this.antiHp + (float)damage * hardDamageMultiplier < 99f)
				{
					this.antiHp += (float)damage * hardDamageMultiplier;
				}
				else
				{
					this.antiHp = 99f;
				}
				if (this.antiHpCooldown == 0f)
				{
					this.antiHpCooldown += 1f;
				}
				if (this.difficulty >= 3)
				{
					this.antiHpCooldown += 1f;
				}
				this.antiHpFlash.Flash(1f);
				this.antiHpCooldown += (float)(damage / 20);
			}
			if (this.shud == null)
			{
				this.shud = MonoSingleton<StyleHUD>.Instance;
			}
			if (scoreLossMultiplier > 0.5f)
			{
				this.shud.RemovePoints(0);
				this.shud.DescendRank();
			}
			else if (scoreLossMultiplier > 0f)
			{
				this.shud.RemovePoints(Mathf.RoundToInt((float)damage));
			}
			StatsManager instance = MonoSingleton<StatsManager>.Instance;
			if (damage <= 200)
			{
				instance.stylePoints -= Mathf.RoundToInt((float)(damage * 5) * scoreLossMultiplier);
			}
			else
			{
				instance.stylePoints -= Mathf.RoundToInt(1000f * scoreLossMultiplier);
			}
			instance.tookDamage = true;
			if (this.hp == 0)
			{
				if (!this.endlessMode)
				{
					this.deathSequence.gameObject.SetActive(true);
					if (instablack)
					{
						this.deathSequence.EndSequence();
					}
					MonoSingleton<TimeController>.Instance.controlPitch = false;
					this.screenHud.SetActive(false);
				}
				else
				{
					base.GetComponentInChildren<FinalCyberRank>().GameOver();
					CrowdReactions instance2 = MonoSingleton<CrowdReactions>.Instance;
					if (instance2 != null)
					{
						instance2.React(instance2.aww);
					}
				}
				this.rb.constraints = RigidbodyConstraints.None;
				this.rb.AddTorque(Vector3.right * -1f, ForceMode.VelocityChange);
				if (MonoSingleton<PowerUpMeter>.Instance)
				{
					MonoSingleton<PowerUpMeter>.Instance.juice = 0f;
				}
				this.cc.enabled = false;
				if (this.gunc == null)
				{
					this.gunc = base.GetComponentInChildren<GunControl>();
				}
				this.gunc.NoWeapon();
				this.rb.constraints = RigidbodyConstraints.None;
				this.dead = true;
				this.activated = false;
				if (this.punch == null)
				{
					this.punch = base.GetComponentInChildren<FistControl>();
				}
				this.punch.NoFist();
			}
		}
	}

	// Token: 0x06001220 RID: 4640 RVA: 0x000922E8 File Offset: 0x000904E8
	public void ForceAntiHP(float amount, bool silent = false, bool dontOverwriteHp = false, bool addToCooldown = true, bool stopInstaHeal = false)
	{
		if ((!this.asscon.majorEnabled || !this.asscon.disableHardDamage) && this.hp <= 100)
		{
			amount = Mathf.Clamp(amount, 0f, 99f);
			float num = this.antiHp;
			if ((float)this.hp > 100f - amount)
			{
				if (dontOverwriteHp)
				{
					amount = (float)(100 - this.hp);
				}
				else
				{
					this.hp = Mathf.RoundToInt(100f - amount);
				}
			}
			if (MonoSingleton<StyleHUD>.Instance.rankIndex < 7)
			{
				this.antiHpFlash.Flash(1f);
				if (amount > this.antiHp)
				{
					this.FakeHurt(silent);
				}
			}
			this.antiHp = amount;
			this.cantInstaHeal = stopInstaHeal;
			if (addToCooldown)
			{
				if (this.antiHpCooldown < 1f || (this.difficulty >= 3 && this.antiHpCooldown < 2f))
				{
					this.antiHpCooldown = (float)((this.difficulty >= 3) ? 2 : 1);
				}
				if (amount - num < 50f)
				{
					this.antiHpCooldown += (amount - num) / 20f;
					return;
				}
				this.antiHpCooldown += 2.5f;
				return;
			}
			else if (this.antiHpCooldown <= 1f)
			{
				this.antiHpCooldown = 1f;
			}
		}
	}

	// Token: 0x06001221 RID: 4641 RVA: 0x0009242E File Offset: 0x0009062E
	public void ForceAddAntiHP(float amount, bool silent = false, bool dontOverwriteHp = false, bool addToCooldown = true, bool stopInstaHeal = false)
	{
		this.ForceAntiHP(this.antiHp + amount, silent, dontOverwriteHp, addToCooldown, stopInstaHeal);
	}

	// Token: 0x06001222 RID: 4642 RVA: 0x00092444 File Offset: 0x00090644
	public void GetHealth(int health, bool silent, bool fromExplosion = false, bool bloodsplatter = true)
	{
		if (!this.dead && (!this.exploded || !fromExplosion))
		{
			float num = (float)health;
			float num2 = 100f;
			if (this.difficulty == 0 || (this.difficulty == 1 && this.sameCheckpointRestarts > 2))
			{
				num2 = 200f;
			}
			if (num < 1f)
			{
				num = 1f;
			}
			if ((float)this.hp <= num2)
			{
				if ((float)this.hp + num < num2 - (float)Mathf.RoundToInt(this.antiHp))
				{
					this.hp += Mathf.RoundToInt(num);
				}
				else if ((float)this.hp != num2 - (float)Mathf.RoundToInt(this.antiHp))
				{
					this.hp = Mathf.RoundToInt(num2) - Mathf.RoundToInt(this.antiHp);
				}
				this.hpFlash.Flash(1f);
				if (!silent && health > 5)
				{
					if (this.greenHpAud == null)
					{
						this.greenHpAud = this.hpFlash.GetComponent<AudioSource>();
					}
					this.greenHpAud.Play();
				}
			}
			if (!silent && health > 5 && MonoSingleton<PrefsManager>.Instance.GetBoolLocal("bloodEnabled", false))
			{
				Object.Instantiate<GameObject>(this.scrnBlood, this.fullHud.transform);
			}
		}
	}

	// Token: 0x06001223 RID: 4643 RVA: 0x0009257C File Offset: 0x0009077C
	public void FullHeal(bool silent)
	{
		this.GetHealth(200, silent, false, false);
	}

	// Token: 0x06001224 RID: 4644 RVA: 0x0009258C File Offset: 0x0009078C
	public void Parry(EnemyIdentifier eid = null, string customParryText = "")
	{
		MonoSingleton<TimeController>.Instance.ParryFlash();
		this.exploded = false;
		this.GetHealth(999, false, false, true);
		this.FullStamina();
		if (this.shud == null)
		{
			this.shud = MonoSingleton<StyleHUD>.Instance;
		}
		if (!eid || !eid.blessed)
		{
			this.shud.AddPoints(100, (customParryText != "") ? ("<color=green>" + customParryText + "</color>") : "ultrakill.parry", null, null, -1, "", "");
		}
	}

	// Token: 0x06001225 RID: 4645 RVA: 0x00092625 File Offset: 0x00090825
	public void SuperCharge()
	{
		this.GetHealth(100, true, false, true);
		this.hp = 200;
	}

	// Token: 0x06001226 RID: 4646 RVA: 0x00092640 File Offset: 0x00090840
	public void Respawn()
	{
		MonoSingleton<CameraController>.Instance.cam.useOcclusionCulling = true;
		if (this.sliding)
		{
			this.StopSlide();
		}
		this.sameCheckpointRestarts++;
		if (this.difficulty == 0)
		{
			this.hp = 200;
		}
		else
		{
			this.hp = 100;
		}
		this.boostCharge = 299f;
		this.antiHp = 0f;
		this.antiHpCooldown = 0f;
		this.rb.constraints = this.defaultRBConstraints;
		this.activated = true;
		this.deathSequence.gameObject.SetActive(false);
		this.cc.enabled = true;
		if (MonoSingleton<PowerUpMeter>.Instance)
		{
			MonoSingleton<PowerUpMeter>.Instance.juice = 0f;
		}
		StatsManager instance = MonoSingleton<StatsManager>.Instance;
		instance.stylePoints = instance.stylePoints / 3 * 2;
		if (this.gunc == null)
		{
			this.gunc = base.GetComponentInChildren<GunControl>();
		}
		this.gunc.YesWeapon();
		this.screenHud.SetActive(true);
		this.dead = false;
		MonoSingleton<TimeController>.Instance.controlPitch = true;
		HookArm instance2 = MonoSingleton<HookArm>.Instance;
		if (instance2 != null)
		{
			instance2.Cancel();
		}
		if (this.punch == null)
		{
			this.punch = base.GetComponentInChildren<FistControl>();
		}
		this.punch.activated = true;
		this.punch.YesFist();
		this.slowMode = false;
		MonoSingleton<WeaponCharges>.Instance.MaxCharges();
		if (MonoSingleton<WeaponCharges>.Instance.rocketFrozen)
		{
			MonoSingleton<WeaponCharges>.Instance.rocketLauncher.UnfreezeRockets();
		}
	}

	// Token: 0x06001227 RID: 4647 RVA: 0x000927CB File Offset: 0x000909CB
	public void ResetHardDamage()
	{
		this.antiHp = 0f;
		this.antiHpCooldown = 0f;
	}

	// Token: 0x06001228 RID: 4648 RVA: 0x000927E3 File Offset: 0x000909E3
	private void NotJumping()
	{
		this.jumping = false;
	}

	// Token: 0x06001229 RID: 4649 RVA: 0x000927EC File Offset: 0x000909EC
	public void EnemyStepResets()
	{
		this.currentWallJumps = 0;
		this.rocketJumps = 0;
		this.hammerJumps = 0;
		this.clingFade = 0f;
		this.rocketRides = 0;
	}

	// Token: 0x0600122A RID: 4650 RVA: 0x00092818 File Offset: 0x00090A18
	public void LandingImpact()
	{
		Object.Instantiate<GameObject>(this.impactDust, this.gc.transform.position, Quaternion.identity).transform.forward = Vector3.up;
		MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.FallImpact);
		MonoSingleton<PlayerAnimations>.Instance.Footstep(1f, true, 0f);
		MonoSingleton<PlayerAnimations>.Instance.Footstep(1f, true, 0.05f);
		MonoSingleton<SceneHelper>.Instance.CreateEnviroGibs(base.transform.position, Vector3.down, 5f, Mathf.RoundToInt(Mathf.Lerp(3f, 5f, (Mathf.Abs(this.fallSpeed) - 50f) / 50f)), 1f);
	}

	// Token: 0x0600122B RID: 4651 RVA: 0x000928E0 File Offset: 0x00090AE0
	private void StartSlide()
	{
		if (this.currentSlideParticle != null)
		{
			Object.Destroy(this.currentSlideParticle);
		}
		if (this.slideScrape != null)
		{
			this.DetachSlideScrape();
		}
		if (this.modNoDashSlide)
		{
			this.StopSlide();
			return;
		}
		if (MonoSingleton<HookArm>.Instance && MonoSingleton<HookArm>.Instance.beingPulled)
		{
			return;
		}
		if (this.groundProperties && !this.groundProperties.canSlide)
		{
			if (!this.groundProperties.silentSlideFail)
			{
				this.StopSlide();
			}
			return;
		}
		if (!this.crouching)
		{
			this.playerCollider.height = 1.25f;
			base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - 1.125f, base.transform.position.z);
			this.gc.transform.localPosition = this.groundCheckPos + Vector3.up * 1.125f;
		}
		this.slideSafety = 1f;
		this.sliding = true;
		this.boost = true;
		this.dodgeDirection = this.movementDirection;
		if (this.dodgeDirection == Vector3.zero)
		{
			this.dodgeDirection = base.transform.forward;
		}
		this.currentSlideParticle = Object.Instantiate<GameObject>(this.slideParticle, base.transform.position + this.dodgeDirection * 10f, Quaternion.LookRotation(-this.dodgeDirection));
		this.slideTrail = this.currentSlideParticle.GetComponent<ParticleSystem>().trails;
		this.slideTrail.colorOverLifetime = ((this.boostLeft > 0f) ? this.invincibleSlideGradient : this.normalSlideGradient);
		this.CreateSlideScrape(true, false);
		if (this.dodgeDirection == base.transform.forward)
		{
			this.cc.dodgeDirection = 0;
		}
		else if (this.dodgeDirection == base.transform.forward * -1f)
		{
			this.cc.dodgeDirection = 1;
		}
		else
		{
			this.cc.dodgeDirection = 2;
		}
		MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.Slide);
	}

	// Token: 0x0600122C RID: 4652 RVA: 0x00092B3C File Offset: 0x00090D3C
	private void CreateSlideScrape(bool ignorePrevious = false, bool frictionlessVersion = false)
	{
		bool flag = this.groundProperties && this.groundProperties.overrideSurfaceType;
		SceneHelper.HitSurfaceData hitSurfaceData = default(SceneHelper.HitSurfaceData);
		if (flag)
		{
			hitSurfaceData.surfaceType = this.groundProperties.surfaceType;
			hitSurfaceData.particleColor = this.groundProperties.particleColor;
		}
		else if (MonoSingleton<SceneHelper>.Instance.TryGetSurfaceData(base.transform.position, Vector3.down, 3f, out hitSurfaceData))
		{
			flag = true;
		}
		if (flag)
		{
			GameObject gameObject;
			if (!MonoSingleton<DefaultReferenceManager>.Instance.footstepSet.TryGetSlideParticle(hitSurfaceData.surfaceType, out gameObject))
			{
				hitSurfaceData.surfaceType = SurfaceType.Generic;
			}
			if (ignorePrevious || hitSurfaceData.surfaceType != (frictionlessVersion ? this.currentFricSlideSurfaceType : this.currentSlideSurfaceType))
			{
				if (frictionlessVersion)
				{
					if (this.currentFrictionlessSlideParticle != null)
					{
						Object.Destroy(this.currentFrictionlessSlideParticle);
					}
					this.currentFrictionlessSlideParticle = Object.Instantiate<GameObject>(gameObject, base.transform.position, Quaternion.identity);
					this.SetFrictionlessSlideValues();
					this.currentFricSlideSurfaceType = hitSurfaceData.surfaceType;
					return;
				}
				this.DetachScrape(this.slideScrape);
				this.slideScrape = Object.Instantiate<GameObject>(gameObject, base.transform.position + this.dodgeDirection * 2f, Quaternion.LookRotation(-this.dodgeDirection));
				MonoSingleton<SceneHelper>.Instance.SetParticlesColors(this.slideScrape, ref hitSurfaceData);
				this.currentSlideSurfaceType = hitSurfaceData.surfaceType;
				return;
			}
		}
		else if ((frictionlessVersion ? this.currentFricSlideSurfaceType : this.currentSlideSurfaceType) > SurfaceType.Generic || ignorePrevious)
		{
			GameObject gameObject2;
			MonoSingleton<DefaultReferenceManager>.Instance.footstepSet.TryGetSlideParticle(SurfaceType.Generic, out gameObject2);
			if (frictionlessVersion)
			{
				if (this.currentFrictionlessSlideParticle != null)
				{
					Object.Destroy(this.currentFrictionlessSlideParticle);
				}
				this.currentFrictionlessSlideParticle = Object.Instantiate<GameObject>(gameObject2, base.transform.position, Quaternion.identity);
				this.SetFrictionlessSlideValues();
				this.currentFricSlideSurfaceType = SurfaceType.Generic;
				return;
			}
			this.DetachScrape(this.slideScrape);
			this.slideScrape = Object.Instantiate<GameObject>(gameObject2, base.transform.position + this.dodgeDirection * 2f, Quaternion.LookRotation(-this.dodgeDirection));
			this.currentSlideSurfaceType = SurfaceType.Generic;
		}
	}

	// Token: 0x0600122D RID: 4653 RVA: 0x00092D78 File Offset: 0x00090F78
	private void SetFrictionlessSlideValues()
	{
		this.fricSlideAuds = this.currentFrictionlessSlideParticle.GetComponentsInChildren<AudioSource>(true);
		this.fricSlideAudVols = new float[this.fricSlideAuds.Length];
		this.fricSlideAudPitches = new float[this.fricSlideAuds.Length];
		for (int i = 0; i < this.fricSlideAuds.Length; i++)
		{
			this.fricSlideAudVols[i] = this.fricSlideAuds[i].volume;
			this.fricSlideAudPitches[i] = this.fricSlideAuds[i].pitch;
		}
	}

	// Token: 0x0600122E RID: 4654 RVA: 0x00092DFC File Offset: 0x00090FFC
	private void CreateWallScrape(Vector3 position, bool ignorePrevious = false)
	{
		SceneHelper.HitSurfaceData hitSurfaceData;
		if (MonoSingleton<SceneHelper>.Instance.TryGetSurfaceData(base.transform.position, position - base.transform.position, 5f, out hitSurfaceData))
		{
			SurfaceType surfaceType = hitSurfaceData.surfaceType;
			GameObject gameObject;
			if (!MonoSingleton<DefaultReferenceManager>.Instance.footstepSet.TryGetWallScrapeParticle(surfaceType, out gameObject))
			{
				surfaceType = SurfaceType.Generic;
			}
			if (ignorePrevious || this.currentScrapeSurfaceType != surfaceType)
			{
				this.DetachScrape(this.wallScrape);
				this.wallScrape = Object.Instantiate<GameObject>(gameObject, position, Quaternion.identity);
				MonoSingleton<SceneHelper>.Instance.SetParticlesColors(this.wallScrape, ref hitSurfaceData);
				this.currentScrapeSurfaceType = surfaceType;
				return;
			}
			this.wallScrape.transform.position = position;
			return;
		}
		else
		{
			if (this.currentScrapeSurfaceType > SurfaceType.Generic || ignorePrevious)
			{
				this.DetachScrape(this.wallScrape);
				GameObject gameObject2;
				MonoSingleton<DefaultReferenceManager>.Instance.footstepSet.TryGetWallScrapeParticle(SurfaceType.Generic, out gameObject2);
				this.wallScrape = Object.Instantiate<GameObject>(gameObject2, position, Quaternion.identity);
				this.currentScrapeSurfaceType = SurfaceType.Generic;
				return;
			}
			this.wallScrape.transform.position = position;
			return;
		}
	}

	// Token: 0x0600122F RID: 4655 RVA: 0x00092F04 File Offset: 0x00091104
	private void CheckForGasoline()
	{
		Vector3Int vector3Int = StainVoxelManager.WorldToVoxelPosition(base.transform.position + Vector3.down * 1.8333334f);
		if (this.lastCheckedGasolineVoxel == null || this.lastCheckedGasolineVoxel.Value != vector3Int)
		{
			this.lastCheckedGasolineVoxel = new Vector3Int?(vector3Int);
			this.modForcedFrictionMultip = (float)(MonoSingleton<StainVoxelManager>.Instance.HasProxiesAt(vector3Int, 3, VoxelCheckingShape.VerticalBox, ProxySearchMode.AnyFloor, true) ? 0 : 1);
			this.onGasoline = this.modForcedFrictionMultip == 0f;
		}
		if (this.oilSlideEffect.gameObject.activeSelf != (this.modForcedFrictionMultip == 0f))
		{
			this.oilSlideEffect.gameObject.SetActive(this.modForcedFrictionMultip == 0f);
		}
		if (this.modForcedFrictionMultip == 0f)
		{
			float num = Mathf.Min(35f, this.rb.velocity.magnitude) / 35f;
			this.oilSlideEffect.volume = Mathf.Lerp(0f, 0.85f, num);
			this.oilSlideEffect.transform.localScale = Vector3.one * num;
			this.oilSlideEffect.pitch = Mathf.Lerp(1.75f, 2.75f, num);
		}
	}

	// Token: 0x06001230 RID: 4656 RVA: 0x00093050 File Offset: 0x00091250
	public void StopSlide()
	{
		if (this.currentSlideParticle != null)
		{
			Object.Destroy(this.currentSlideParticle);
		}
		if (this.slideScrape != null)
		{
			this.DetachSlideScrape();
		}
		Object.Instantiate<GameObject>(this.slideStopSound);
		this.cc.ResetToDefaultPos();
		this.sliding = false;
		this.slideEnding = true;
		if (this.slideLength > this.longestSlide)
		{
			this.longestSlide = this.slideLength;
		}
		this.slideLength = 0f;
		if (!this.gc.heavyFall)
		{
			Physics.IgnoreLayerCollision(2, 12, false);
		}
		this.framesSinceSlide = 0;
		this.velocityAfterSlide = this.rb.velocity;
		this.sinceSlideEnd = 0f;
		MonoSingleton<RumbleManager>.Instance.StopVibration(RumbleProperties.Slide);
	}

	// Token: 0x06001231 RID: 4657 RVA: 0x00093121 File Offset: 0x00091321
	private void DetachSlideScrape()
	{
		this.DetachScrape(this.slideScrape);
		this.slideScrape = null;
	}

	// Token: 0x06001232 RID: 4658 RVA: 0x00093136 File Offset: 0x00091336
	private void DetachWallScrape()
	{
		this.DetachScrape(this.wallScrape);
		this.wallScrape = null;
	}

	// Token: 0x06001233 RID: 4659 RVA: 0x0009314C File Offset: 0x0009134C
	private void DetachScrape(GameObject scrape)
	{
		if (scrape == null)
		{
			return;
		}
		ParticleSystem[] componentsInChildren = scrape.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Stop();
		}
		AudioSource[] componentsInChildren2 = scrape.GetComponentsInChildren<AudioSource>();
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].Stop();
		}
		scrape.AddComponent<RemoveOnTime>().time = 10f;
	}

	// Token: 0x06001234 RID: 4660 RVA: 0x000931AD File Offset: 0x000913AD
	public void EmptyStamina()
	{
		this.boostCharge = 0f;
	}

	// Token: 0x06001235 RID: 4661 RVA: 0x000931BA File Offset: 0x000913BA
	public void FullStamina()
	{
		this.boostCharge = 300f;
	}

	// Token: 0x06001236 RID: 4662 RVA: 0x000931C7 File Offset: 0x000913C7
	public void DeactivatePlayer()
	{
		this.activated = false;
		MonoSingleton<CameraController>.Instance.activated = false;
		MonoSingleton<GunControl>.Instance.NoWeapon();
		MonoSingleton<FistControl>.Instance.NoFist();
		if (this.sliding)
		{
			this.StopSlide();
		}
	}

	// Token: 0x06001237 RID: 4663 RVA: 0x000931FD File Offset: 0x000913FD
	public void ActivatePlayer()
	{
		this.activated = true;
		MonoSingleton<CameraController>.Instance.activated = true;
		MonoSingleton<GunControl>.Instance.YesWeapon();
		MonoSingleton<FistControl>.Instance.YesFist();
	}

	// Token: 0x06001238 RID: 4664 RVA: 0x00093228 File Offset: 0x00091428
	public void StopMovement()
	{
		if (this.sliding)
		{
			this.StopSlide();
		}
		if (this.boost)
		{
			this.boostLeft = 0f;
			this.boost = false;
		}
		this.movementDirection = Vector3.zero;
		this.rb.velocity = Vector3.zero;
	}

	// Token: 0x06001239 RID: 4665 RVA: 0x00093278 File Offset: 0x00091478
	public void DeactivateMovement()
	{
		this.activated = false;
		this.movementDirection = Vector3.zero;
	}

	// Token: 0x0600123A RID: 4666 RVA: 0x0009328C File Offset: 0x0009148C
	public void ReactivateMovement()
	{
		this.activated = true;
		this.punch.YesFist();
	}

	// Token: 0x0600123B RID: 4667 RVA: 0x000932A0 File Offset: 0x000914A0
	public void LockMovementAxes()
	{
		this.rb.constraints = (RigidbodyConstraints)122;
	}

	// Token: 0x0600123C RID: 4668 RVA: 0x000932AF File Offset: 0x000914AF
	public void UnlockMovementAxes()
	{
		this.rb.constraints = RigidbodyConstraints.FreezeRotation;
	}

	// Token: 0x040018B0 RID: 6320
	[HideInInspector]
	public bool modNoDashSlide;

	// Token: 0x040018B1 RID: 6321
	[HideInInspector]
	public bool modNoJump;

	// Token: 0x040018B2 RID: 6322
	[HideInInspector]
	public float modForcedFrictionMultip = 1f;

	// Token: 0x040018B3 RID: 6323
	private float friction;

	// Token: 0x040018B4 RID: 6324
	private InputManager inman;

	// Token: 0x040018B5 RID: 6325
	[HideInInspector]
	public AssistController asscon;

	// Token: 0x040018B6 RID: 6326
	public float walkSpeed;

	// Token: 0x040018B7 RID: 6327
	public float jumpPower;

	// Token: 0x040018B8 RID: 6328
	public float airAcceleration;

	// Token: 0x040018B9 RID: 6329
	public float wallJumpPower;

	// Token: 0x040018BA RID: 6330
	private bool jumpCooldown;

	// Token: 0x040018BB RID: 6331
	private bool enemyStepping;

	// Token: 0x040018BC RID: 6332
	[HideInInspector]
	public bool falling;

	// Token: 0x040018BD RID: 6333
	[HideInInspector]
	public Rigidbody rb;

	// Token: 0x040018BE RID: 6334
	private Vector3 movementDirection;

	// Token: 0x040018BF RID: 6335
	private Vector3 movementDirection2;

	// Token: 0x040018C0 RID: 6336
	private Vector3 airDirection;

	// Token: 0x040018C1 RID: 6337
	public float timeBetweenSteps;

	// Token: 0x040018C2 RID: 6338
	private float stepTime;

	// Token: 0x040018C3 RID: 6339
	private int currentStep;

	// Token: 0x040018C4 RID: 6340
	private Quaternion tempRotation;

	// Token: 0x040018C5 RID: 6341
	private GameObject forwardPoint;

	// Token: 0x040018C6 RID: 6342
	public GroundCheck gc;

	// Token: 0x040018C7 RID: 6343
	public GroundCheck slopeCheck;

	// Token: 0x040018C8 RID: 6344
	private WallCheck wc;

	// Token: 0x040018C9 RID: 6345
	private Vector3 wallJumpPos;

	// Token: 0x040018CA RID: 6346
	public int currentWallJumps;

	// Token: 0x040018CB RID: 6347
	private AudioSource aud;

	// Token: 0x040018CC RID: 6348
	private AudioSource aud2;

	// Token: 0x040018CD RID: 6349
	private AudioSource aud3;

	// Token: 0x040018CE RID: 6350
	private int currentSound;

	// Token: 0x040018CF RID: 6351
	public AudioClip jumpSound;

	// Token: 0x040018D0 RID: 6352
	public AudioClip landingSound;

	// Token: 0x040018D1 RID: 6353
	public AudioClip finalWallJump;

	// Token: 0x040018D2 RID: 6354
	public bool walking;

	// Token: 0x040018D3 RID: 6355
	public int hp = 100;

	// Token: 0x040018D4 RID: 6356
	public float antiHp;

	// Token: 0x040018D5 RID: 6357
	private float antiHpCooldown;

	// Token: 0x040018D6 RID: 6358
	private bool cantInstaHeal;

	// Token: 0x040018D7 RID: 6359
	public Image hurtScreen;

	// Token: 0x040018D8 RID: 6360
	private AudioSource hurtAud;

	// Token: 0x040018D9 RID: 6361
	private Color hurtColor;

	// Token: 0x040018DA RID: 6362
	private Color currentColor;

	// Token: 0x040018DB RID: 6363
	private float hurtInvincibility;

	// Token: 0x040018DC RID: 6364
	public bool dead;

	// Token: 0x040018DD RID: 6365
	public bool endlessMode;

	// Token: 0x040018DE RID: 6366
	public DeathSequence deathSequence;

	// Token: 0x040018DF RID: 6367
	public FlashImage hpFlash;

	// Token: 0x040018E0 RID: 6368
	public FlashImage antiHpFlash;

	// Token: 0x040018E1 RID: 6369
	private AudioSource greenHpAud;

	// Token: 0x040018E2 RID: 6370
	private float currentAllVolume;

	// Token: 0x040018E3 RID: 6371
	public bool boost;

	// Token: 0x040018E4 RID: 6372
	public Vector3 dodgeDirection;

	// Token: 0x040018E5 RID: 6373
	private float boostLeft;

	// Token: 0x040018E6 RID: 6374
	private float dashStorage;

	// Token: 0x040018E7 RID: 6375
	public float boostCharge = 300f;

	// Token: 0x040018E8 RID: 6376
	public AudioClip dodgeSound;

	// Token: 0x040018E9 RID: 6377
	public CameraController cc;

	// Token: 0x040018EA RID: 6378
	public GameObject staminaFailSound;

	// Token: 0x040018EB RID: 6379
	public GameObject screenHud;

	// Token: 0x040018EC RID: 6380
	private Vector3 hudOriginalPos;

	// Token: 0x040018ED RID: 6381
	public GameObject dodgeParticle;

	// Token: 0x040018EE RID: 6382
	public GameObject scrnBlood;

	// Token: 0x040018EF RID: 6383
	private Canvas fullHud;

	// Token: 0x040018F0 RID: 6384
	public GameObject hudCam;

	// Token: 0x040018F1 RID: 6385
	private Vector3 camOriginalPos;

	// Token: 0x040018F2 RID: 6386
	private RigidbodyConstraints defaultRBConstraints;

	// Token: 0x040018F3 RID: 6387
	private GameObject revolver;

	// Token: 0x040018F4 RID: 6388
	private StyleHUD shud;

	// Token: 0x040018F5 RID: 6389
	private GameObject wallScrape;

	// Token: 0x040018F6 RID: 6390
	private SurfaceType currentScrapeSurfaceType;

	// Token: 0x040018F7 RID: 6391
	public StyleCalculator scalc;

	// Token: 0x040018F8 RID: 6392
	public bool activated;

	// Token: 0x040018F9 RID: 6393
	public int gamepadFreezeCount;

	// Token: 0x040018FA RID: 6394
	private float fallSpeed;

	// Token: 0x040018FB RID: 6395
	public bool jumping;

	// Token: 0x040018FC RID: 6396
	private float fallTime;

	// Token: 0x040018FD RID: 6397
	public GameObject impactDust;

	// Token: 0x040018FE RID: 6398
	public GameObject fallParticle;

	// Token: 0x040018FF RID: 6399
	private GameObject currentFallParticle;

	// Token: 0x04001900 RID: 6400
	[HideInInspector]
	public CapsuleCollider playerCollider;

	// Token: 0x04001901 RID: 6401
	public bool sliding;

	// Token: 0x04001902 RID: 6402
	private float slideSafety;

	// Token: 0x04001903 RID: 6403
	public GameObject slideParticle;

	// Token: 0x04001904 RID: 6404
	private GameObject currentSlideParticle;

	// Token: 0x04001905 RID: 6405
	private ParticleSystem.TrailModule slideTrail;

	// Token: 0x04001906 RID: 6406
	private ParticleSystem.MinMaxGradient normalSlideGradient;

	// Token: 0x04001907 RID: 6407
	public ParticleSystem.MinMaxGradient invincibleSlideGradient;

	// Token: 0x04001908 RID: 6408
	private GameObject slideScrape;

	// Token: 0x04001909 RID: 6409
	private SurfaceType currentSlideSurfaceType;

	// Token: 0x0400190A RID: 6410
	private Vector3 slideMovDirection;

	// Token: 0x0400190B RID: 6411
	public GameObject slideStopSound;

	// Token: 0x0400190C RID: 6412
	private bool crouching;

	// Token: 0x0400190D RID: 6413
	public bool standing;

	// Token: 0x0400190E RID: 6414
	private bool slideEnding;

	// Token: 0x0400190F RID: 6415
	private Vector3 groundCheckPos;

	// Token: 0x04001910 RID: 6416
	public AudioSource oilSlideEffect;

	// Token: 0x04001911 RID: 6417
	private bool onGasoline;

	// Token: 0x04001912 RID: 6418
	private GameObject currentFrictionlessSlideParticle;

	// Token: 0x04001913 RID: 6419
	private SurfaceType currentFricSlideSurfaceType;

	// Token: 0x04001914 RID: 6420
	private AudioSource[] fricSlideAuds;

	// Token: 0x04001915 RID: 6421
	private float[] fricSlideAudVols;

	// Token: 0x04001916 RID: 6422
	private float[] fricSlideAudPitches;

	// Token: 0x04001917 RID: 6423
	private LayerMask frictionlessSurfaceMask;

	// Token: 0x04001918 RID: 6424
	private GunControl gunc;

	// Token: 0x04001919 RID: 6425
	public float currentSpeed;

	// Token: 0x0400191A RID: 6426
	private FistControl punch;

	// Token: 0x0400191B RID: 6427
	public GameObject dashJumpSound;

	// Token: 0x0400191C RID: 6428
	public bool slowMode;

	// Token: 0x0400191D RID: 6429
	public Vector3 pushForce;

	// Token: 0x0400191E RID: 6430
	private float slideLength;

	// Token: 0x0400191F RID: 6431
	[HideInInspector]
	public float longestSlide;

	// Token: 0x04001920 RID: 6432
	private float preSlideSpeed;

	// Token: 0x04001921 RID: 6433
	private float preSlideDelay;

	// Token: 0x04001922 RID: 6434
	public bool quakeJump;

	// Token: 0x04001923 RID: 6435
	public GameObject quakeJumpSound;

	// Token: 0x04001924 RID: 6436
	[HideInInspector]
	public bool exploded;

	// Token: 0x04001925 RID: 6437
	[HideInInspector]
	public float safeExplosionLaunchCooldown;

	// Token: 0x04001926 RID: 6438
	private float clingFade;

	// Token: 0x04001927 RID: 6439
	public bool stillHolding;

	// Token: 0x04001928 RID: 6440
	public float slamForce;

	// Token: 0x04001929 RID: 6441
	private bool slamStorage;

	// Token: 0x0400192A RID: 6442
	[HideInInspector]
	public float slamCooldown;

	// Token: 0x0400192B RID: 6443
	private bool launched;

	// Token: 0x0400192C RID: 6444
	private int difficulty;

	// Token: 0x0400192D RID: 6445
	[HideInInspector]
	public int sameCheckpointRestarts;

	// Token: 0x0400192E RID: 6446
	public CustomGroundProperties groundProperties;

	// Token: 0x0400192F RID: 6447
	[HideInInspector]
	public int rocketJumps;

	// Token: 0x04001930 RID: 6448
	[HideInInspector]
	public int hammerJumps;

	// Token: 0x04001931 RID: 6449
	[HideInInspector]
	public Grenade ridingRocket;

	// Token: 0x04001932 RID: 6450
	[HideInInspector]
	public int rocketRides;

	// Token: 0x04001933 RID: 6451
	private float ssjMaxFrames = 4f;

	// Token: 0x04001934 RID: 6452
	public Light pointLight;

	// Token: 0x04001935 RID: 6453
	public TimeSince sinceSlideEnd;

	// Token: 0x04001936 RID: 6454
	[HideInInspector]
	public bool levelOver;

	// Token: 0x04001937 RID: 6455
	[HideInInspector]
	public HashSet<Water> touchingWaters = new HashSet<Water>();

	// Token: 0x04001938 RID: 6456
	private Vector3Int? lastCheckedGasolineVoxel;

	// Token: 0x04001939 RID: 6457
	private int framesSinceSlide;

	// Token: 0x0400193A RID: 6458
	private Vector3 velocityAfterSlide;
}
