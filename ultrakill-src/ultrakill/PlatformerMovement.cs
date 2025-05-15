using System;
using System.Collections.Generic;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x0200033E RID: 830
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class PlatformerMovement : MonoSingleton<PlatformerMovement>
{
	// Token: 0x06001307 RID: 4871 RVA: 0x00097434 File Offset: 0x00095634
	protected override void Awake()
	{
		base.Awake();
		this.rbSlide = new InputBinding("<Gamepad>/rightShoulder", null, "Gamepad", null, null, "rbSlide");
		this.dpadMove = new InputBinding("<Gamepad>/dpad", null, "Gamepad", null, null, "dpadMove");
	}

	// Token: 0x06001308 RID: 4872 RVA: 0x00097484 File Offset: 0x00095684
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.aud = base.GetComponent<AudioSource>();
		this.playerCollider = base.GetComponent<CapsuleCollider>();
		this.anim = base.GetComponent<Animator>();
		this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		this.smr = base.GetComponentInChildren<SkinnedMeshRenderer>();
		this.block = new MaterialPropertyBlock();
		this.UpdateWings();
		base.transform.position = MonoSingleton<NewMovement>.Instance.gc.transform.position;
		this.rb.velocity = MonoSingleton<NewMovement>.Instance.rb.velocity;
		this.currentYPos = base.transform.position.y;
		if (MonoSingleton<PlayerTracker>.Instance.pmov == null)
		{
			MonoSingleton<PlayerTracker>.Instance.currentPlatformerPlayerPrefab = base.transform.parent.gameObject;
			MonoSingleton<PlayerTracker>.Instance.pmov = this;
			MonoSingleton<PlayerTracker>.Instance.ChangeToPlatformer();
		}
	}

	// Token: 0x06001309 RID: 4873 RVA: 0x00097584 File Offset: 0x00095784
	public void CheckItem()
	{
		if (MonoSingleton<FistControl>.Instance && MonoSingleton<FistControl>.Instance.heldObject)
		{
			MonoSingleton<FistControl>.Instance.heldObject.transform.SetParent(this.holder, true);
			MonoSingleton<FistControl>.Instance.ResetHeldItemPosition();
			Transform[] componentsInChildren = MonoSingleton<FistControl>.Instance.heldObject.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = 22;
			}
		}
	}

	// Token: 0x0600130A RID: 4874 RVA: 0x00097600 File Offset: 0x00095800
	protected override void OnEnable()
	{
		base.OnEnable();
		global::PlayerInput inputSource = MonoSingleton<InputManager>.Instance.InputSource;
		inputSource.Jump.Action.ApplyBindingOverride("<Gamepad>/buttonSouth", "Gamepad", null);
		inputSource.Fire1.Action.ApplyBindingOverride("<Gamepad>/buttonWest", "Gamepad", null);
		inputSource.Dodge.Action.ApplyBindingOverride("<Gamepad>/buttonNorth", "Gamepad", null);
		inputSource.Slide.Action.ApplyBindingOverride("<Gamepad>/buttonEast", "Gamepad", null);
		inputSource.Slide.Action.AddBinding(this.rbSlide);
		inputSource.Move.Action.AddBinding(this.dpadMove);
		this.slideLength = 0f;
	}

	// Token: 0x0600130B RID: 4875 RVA: 0x000976C4 File Offset: 0x000958C4
	private void OnDisable()
	{
		if (!base.gameObject.scene.isLoaded)
		{
			return;
		}
		global::PlayerInput inputSource = MonoSingleton<InputManager>.Instance.InputSource;
		inputSource.Jump.Action.RemoveAllBindingOverrides();
		inputSource.Fire1.Action.RemoveAllBindingOverrides();
		inputSource.Dodge.Action.RemoveAllBindingOverrides();
		inputSource.Slide.Action.RemoveAllBindingOverrides();
		inputSource.Slide.Action.ChangeBinding(this.rbSlide).Erase();
		inputSource.Move.Action.ChangeBinding(this.dpadMove).Erase();
		this.cameraTargets.Clear();
		if (this.sliding)
		{
			this.StopSlide();
		}
	}

	// Token: 0x0600130C RID: 4876 RVA: 0x00097788 File Offset: 0x00095988
	private void UpdateWings()
	{
		float num = 0f;
		Color red = Color.red;
		if (this.boostCharge >= 300f)
		{
			red = new Color(1f, 0.66f, 0f);
		}
		else if (this.boostCharge >= 200f)
		{
			red = new Color(1f, 0.33f, 0f);
		}
		for (int i = 1; i < this.smr.materials.Length; i++)
		{
			switch (i)
			{
			case 1:
			case 2:
			case 3:
			case 6:
				num = ((this.boostCharge >= 300f) ? 1.2f : 0f);
				break;
			case 4:
			case 7:
				num = ((this.boostCharge >= 200f) ? 1.2f : 0f);
				break;
			case 5:
			case 8:
				num = ((this.boostCharge >= 100f) ? 1.2f : 0f);
				break;
			}
			this.smr.GetPropertyBlock(this.block, i);
			this.block.SetFloat(UKShaderProperties.EmissiveIntensity, num);
			this.block.SetColor(UKShaderProperties.EmissiveColor, red);
			this.smr.SetPropertyBlock(this.block, i);
		}
	}

	// Token: 0x0600130D RID: 4877 RVA: 0x000978CC File Offset: 0x00095ACC
	private void Update()
	{
		if (MonoSingleton<OptionsManager>.Instance.paused)
		{
			return;
		}
		this.UpdateWings();
		if (this.aboutToSlam)
		{
			this.rb.velocity = Vector3.up * 5f / (this.slamWindUp * 10f);
			if (this.slamWindUp >= 0.5f)
			{
				this.Slam();
			}
			return;
		}
		if (this.groundCheck.heavyFall)
		{
			this.rb.velocity = Vector3.down * 100f;
		}
		Vector2 vector = Vector2.zero;
		this.airFrictionless = new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z).magnitude > 25f;
		if (this.activated)
		{
			vector = MonoSingleton<InputManager>.Instance.InputSource.Move.ReadValue<Vector2>();
			this.movementDirection = Vector3.ClampMagnitude(vector.x * Vector3.right + vector.y * Vector3.forward, 1f);
			this.movementDirection = Quaternion.Euler(0f, this.platformerCamera.rotation.eulerAngles.y, 0f) * this.movementDirection;
		}
		else
		{
			this.rb.velocity = new Vector3(0f, this.rb.velocity.y, 0f);
			this.movementDirection = Vector3.zero;
		}
		if (this.movementDirection.magnitude > 0f)
		{
			this.anim.SetBool("Running", true);
		}
		else
		{
			this.anim.SetBool("Running", false);
		}
		if (this.rb.velocity.y < -100f)
		{
			this.rb.velocity = new Vector3(this.rb.velocity.x, -100f, this.rb.velocity.z);
		}
		if (this.activated && MonoSingleton<InputManager>.Instance.InputSource.Jump.WasPerformedThisFrame && !this.falling && !this.jumpCooldown)
		{
			this.Jump(false, 1f);
		}
		if (!this.groundCheck.onGround)
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
		else
		{
			this.fallTime = 0f;
		}
		if (this.groundCheck.onGround && this.falling && !this.jumpCooldown)
		{
			this.falling = false;
			this.SlamEnd(!this.slamming);
			if (this.aboutToSlam)
			{
				this.anim.Play("Landing", -1, 0f);
			}
		}
		if (!this.groundCheck.onGround && !this.aboutToSlam && this.activated && MonoSingleton<InputManager>.Instance.InputSource.Slide.WasPerformedThisFrame && !GameStateManager.Instance.PlayerInputLocked)
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
			RaycastHit raycastHit;
			if (this.fallTime > 0.5f && !Physics.Raycast(this.groundCheck.transform.position + base.transform.up, base.transform.up * -1f, out raycastHit, 3f, LayerMaskDefaults.Get(LMD.Environment)) && !this.groundCheck.heavyFall)
			{
				this.aboutToSlam = true;
				if (this.spinning)
				{
					this.StopSpin();
				}
				Object.Instantiate<AudioSource>(this.slamReadySound, base.transform.position, Quaternion.identity);
				this.anim.Play("SlamStart", -1, 0f);
				this.slamWindUp = 0f;
			}
		}
		if (this.groundCheck.heavyFall)
		{
			this.slamForce += Time.deltaTime * 5f;
			RaycastHit raycastHit2;
			if (Physics.Raycast(this.groundCheck.transform.position + base.transform.up, base.transform.up * -1f, out raycastHit2, 5f, LayerMaskDefaults.Get(LMD.Environment)) || Physics.SphereCast(this.groundCheck.transform.position + base.transform.up, 1f, base.transform.up * -1f, out raycastHit2, 5f, LayerMaskDefaults.Get(LMD.Environment)))
			{
				Breakable component = raycastHit2.collider.GetComponent<Breakable>();
				if (component != null && ((component.weak && !component.precisionOnly && !component.specialCaseOnly) || component.forceGroundSlammable) && !component.unbreakable)
				{
					component.Break();
				}
				Bleeder bleeder;
				if (raycastHit2.collider.gameObject.TryGetComponent<Bleeder>(out bleeder))
				{
					bleeder.GetHit(raycastHit2.point, GoreType.Head, false);
				}
				Idol idol;
				if (raycastHit2.transform.TryGetComponent<Idol>(out idol))
				{
					idol.Death();
				}
			}
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Slide.WasPerformedThisFrame && this.groundCheck.onGround && this.activated && !this.sliding)
		{
			this.StartSlide();
		}
		RaycastHit raycastHit3;
		if (MonoSingleton<InputManager>.Instance.InputSource.Slide.WasPerformedThisFrame && !this.groundCheck.onGround && !this.sliding && !this.jumping && this.activated && Physics.Raycast(this.groundCheck.transform.position + base.transform.up, base.transform.up * -1f, out raycastHit3, 2f, LayerMaskDefaults.Get(LMD.Environment)))
		{
			this.StartSlide();
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Slide.WasCanceledThisFrame && this.sliding)
		{
			this.StopSlide();
		}
		if (this.sliding && this.activated)
		{
			this.slideLength += Time.deltaTime;
			if (this.currentSlideEffect != null)
			{
				this.currentSlideEffect.transform.position = base.transform.position + this.dodgeDirection * 10f;
			}
			if (this.slideSafety > 0f)
			{
				this.slideSafety -= Time.deltaTime * 5f;
			}
			if (this.groundCheck.onGround)
			{
				this.currentSlideScrape.transform.position = base.transform.position + this.dodgeDirection;
			}
			else
			{
				this.currentSlideScrape.transform.position = Vector3.one * 5000f;
			}
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Dodge.WasPerformedThisFrame && this.activated)
		{
			if (this.groundProperties && !this.groundProperties.canDash)
			{
				if (!this.groundProperties.silentDashFail)
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
				this.boost = true;
				this.anim.Play("Dash", -1, 0f);
				this.dodgeDirection = this.movementDirection;
				if (this.dodgeDirection == Vector3.zero)
				{
					this.dodgeDirection = this.playerModel.forward;
				}
				Quaternion identity = Quaternion.identity;
				identity.SetLookRotation(this.dodgeDirection * -1f);
				Object.Instantiate<GameObject>(this.dodgeParticle, base.transform.position + Vector3.up * 2f + this.dodgeDirection * 10f, identity).transform.localScale *= 2f;
				if (!MonoSingleton<AssistController>.Instance.majorEnabled || !MonoSingleton<AssistController>.Instance.infiniteStamina)
				{
					this.boostCharge -= 100f;
				}
				this.aud.clip = this.dodgeSound;
				this.aud.volume = 1f;
				this.aud.pitch = 1f;
				this.aud.Play();
			}
			else
			{
				Object.Instantiate<GameObject>(this.staminaFailSound);
			}
		}
		if (this.boostCharge != 300f && !this.sliding && !this.spinning)
		{
			float num = 1f;
			if (this.difficulty == 1)
			{
				num = 1.5f;
			}
			else if (this.difficulty == 0)
			{
				num = 2f;
			}
			this.boostCharge = Mathf.MoveTowards(this.boostCharge, 300f, 70f * Time.deltaTime * num);
		}
		if (this.spinCooldown > 0f)
		{
			this.spinCooldown = Mathf.MoveTowards(this.spinCooldown, 0f, Time.deltaTime);
		}
		if (this.activated && !this.spinning && this.spinCooldown <= 0f && !MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && (MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame || MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasPerformedThisFrame || MonoSingleton<InputManager>.Instance.InputSource.Punch.WasPerformedThisFrame) && !MonoSingleton<OptionsManager>.Instance.paused)
		{
			this.Spin();
		}
		if (this.spinning)
		{
			this.playerModel.Rotate(Vector3.up, Time.deltaTime * 3600f, Space.Self);
		}
		else if (this.movementDirection.magnitude != 0f || this.boost || this.sliding)
		{
			Quaternion quaternion = Quaternion.LookRotation(this.movementDirection);
			if (this.sliding)
			{
				quaternion = Quaternion.LookRotation(this.rb.velocity);
			}
			else if (this.boost)
			{
				quaternion = Quaternion.LookRotation(this.dodgeDirection);
			}
			this.playerModel.rotation = Quaternion.RotateTowards(this.playerModel.rotation, quaternion, (Quaternion.Angle(this.playerModel.rotation, quaternion) + 20f) * 35f * this.movementDirection.magnitude * Time.deltaTime);
		}
		if ((this.groundCheck.onGround && !this.jumping) || base.transform.position.y < this.lastYPos)
		{
			this.beenOverYPosMax = false;
			this.lastYPos = base.transform.position.y;
		}
		if (this.cameraTrack)
		{
			float num2 = this.lastYPos;
			if (base.transform.position.y > this.lastYPos + 10f || this.beenOverYPosMax)
			{
				if (!this.beenOverYPosMax)
				{
					this.beenOverYPosMax = true;
					this.yPosDifferential = 10f;
				}
				RaycastHit raycastHit4;
				if (this.rb.velocity.y < 0f && Physics.Raycast(base.transform.position, Vector3.down, out raycastHit4, this.yPosDifferential, LayerMaskDefaults.Get(LMD.Environment)) && !raycastHit4.transform.gameObject.CompareTag("Breakable") && raycastHit4.distance <= Mathf.Max(5f, this.yPosDifferential))
				{
					this.yPosDifferential = Mathf.MoveTowards(this.yPosDifferential, raycastHit4.distance, Time.deltaTime * (0.1f + Mathf.Abs(this.rb.velocity.y)));
				}
				else if (base.transform.position.y - this.lastYPos <= Mathf.Max(5f, this.yPosDifferential))
				{
					this.yPosDifferential = base.transform.position.y - this.lastYPos;
				}
				else
				{
					this.yPosDifferential = Mathf.MoveTowards(this.yPosDifferential, Mathf.Min(5f, this.yPosDifferential), Time.deltaTime * (0.1f + Mathf.Abs(this.rb.velocity.y)));
				}
				this.currentYPos = base.transform.position.y - this.yPosDifferential;
			}
			else
			{
				this.currentYPos = Mathf.MoveTowards(this.currentYPos, num2, Time.deltaTime * 15f * (0.1f + Mathf.Abs(Mathf.Abs(num2) - Mathf.Abs(this.currentYPos))));
			}
			if (!this.freeCamera)
			{
				this.CheckCameraTarget(false);
				Vector3 vector2 = new Vector3(base.transform.position.x, this.currentYPos, base.transform.position.z) + this.cameraTarget;
				if ((Physics.CheckSphere(vector2, 0.5f, LayerMaskDefaults.Get(LMD.Environment)) && (!Physics.CheckSphere(vector2 - Vector3.up * 2f, 0.5f, LayerMaskDefaults.Get(LMD.Environment)) || this.cameraLowered)) || (Physics.SphereCast(new Ray(vector2, base.transform.forward), 0.5f, 2f, LayerMaskDefaults.Get(LMD.Environment)) && (!Physics.SphereCast(new Ray(vector2 - Vector3.up * 2f, base.transform.forward), 0.5f, 2f, LayerMaskDefaults.Get(LMD.Environment)) || this.cameraLowered)))
				{
					this.cameraLowered = true;
					vector2 -= Vector3.up * 2f;
				}
				else
				{
					this.cameraLowered = false;
				}
				Vector3 vector3 = Vector3.MoveTowards(this.platformerCamera.position, vector2, Time.deltaTime * 15f * (0.1f + Vector3.Distance(this.platformerCamera.position, vector2)));
				Quaternion quaternion2 = Quaternion.RotateTowards(this.platformerCamera.transform.rotation, Quaternion.Euler(this.cameraRotation), Time.deltaTime * 15f * (0.1f + Vector3.Distance(this.platformerCamera.rotation.eulerAngles, this.cameraRotation)));
				this.platformerCamera.transform.SetPositionAndRotation(vector3, quaternion2);
			}
			else if (!MonoSingleton<OptionsManager>.Instance.paused)
			{
				this.platformerCamera.SetPositionAndRotation(base.transform.position + (this.freeCamera ? this.defaultFreeCameraTarget : this.defaultCameraTarget), Quaternion.Euler(this.defaultCameraRotation));
				Vector2 vector4 = MonoSingleton<InputManager>.Instance.InputSource.Look.ReadValue<Vector2>();
				if (!MonoSingleton<CameraController>.Instance.reverseY)
				{
					this.rotationX += vector4.y * (MonoSingleton<OptionsManager>.Instance.mouseSensitivity / 10f);
				}
				else
				{
					this.rotationX -= vector4.y * (MonoSingleton<OptionsManager>.Instance.mouseSensitivity / 10f);
				}
				if (!MonoSingleton<CameraController>.Instance.reverseX)
				{
					this.rotationY += vector4.x * (MonoSingleton<OptionsManager>.Instance.mouseSensitivity / 10f);
				}
				else
				{
					this.rotationY -= vector4.x * (MonoSingleton<OptionsManager>.Instance.mouseSensitivity / 10f);
				}
				if (this.rotationY > 180f)
				{
					this.rotationY -= 360f;
				}
				else if (this.rotationY < -180f)
				{
					this.rotationY += 360f;
				}
				this.rotationX = Mathf.Clamp(this.rotationX, -69f, 109f);
				float num3 = 2.5f;
				if (this.sliding || Physics.Raycast(base.transform.position + Vector3.up * 0.625f, Vector3.up, 2.5f, LayerMaskDefaults.Get(LMD.Environment)))
				{
					num3 = 0.625f;
				}
				Vector3 vector5 = base.transform.position + Vector3.up * num3;
				this.platformerCamera.RotateAround(vector5, Vector3.left, this.rotationX);
				this.platformerCamera.RotateAround(vector5, Vector3.up, this.rotationY);
				RaycastHit raycastHit5;
				if (Physics.SphereCast(vector5, 0.25f, this.platformerCamera.position - vector5, out raycastHit5, Vector3.Distance(vector5, this.platformerCamera.position), LayerMaskDefaults.Get(LMD.Environment)))
				{
					this.platformerCamera.position = raycastHit5.point + 0.5f * raycastHit5.normal;
				}
			}
		}
		RaycastHit raycastHit6;
		if (Physics.SphereCast(base.transform.position + Vector3.up, 0.5f, Vector3.down, out raycastHit6, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
		{
			this.jumpShadow.position = raycastHit6.point + Vector3.up * 0.05f;
			this.jumpShadow.forward = raycastHit6.normal;
		}
		else
		{
			this.jumpShadow.position = base.transform.position - Vector3.up * 1000f;
			this.jumpShadow.forward = Vector3.up;
		}
		if (this.coinTimer > 0f)
		{
			this.coinTimer = Mathf.MoveTowards(this.coinTimer, 0f, Time.deltaTime);
		}
		if (this.coinEffectTimer > 0f)
		{
			this.coinEffectTimer = Mathf.MoveTowards(this.coinEffectTimer, 0f, Time.deltaTime);
		}
		else if (this.queuedCoins > 0)
		{
			this.CoinGetEffect();
		}
		if (this.invincible && this.extraHits < 3)
		{
			if (this.blinkTimer > 0f)
			{
				this.blinkTimer = Mathf.MoveTowards(this.blinkTimer, 0f, Time.deltaTime);
			}
			else
			{
				this.blinkTimer = 0.05f;
				if (this.playerModel.gameObject.activeSelf)
				{
					this.playerModel.gameObject.SetActive(false);
				}
				else
				{
					this.playerModel.gameObject.SetActive(true);
				}
			}
		}
		if (this.superTimer > 0f)
		{
			if (!NoWeaponCooldown.NoCooldown)
			{
				this.superTimer = Mathf.MoveTowards(this.superTimer, 0f, Time.deltaTime);
			}
			if (this.superTimer == 0f)
			{
				this.GetHit();
			}
		}
	}

	// Token: 0x0600130E RID: 4878 RVA: 0x00098C50 File Offset: 0x00096E50
	private void CheckCameraTarget(bool instant = false)
	{
		Vector3 vector = (this.freeCamera ? this.defaultFreeCameraTarget : this.defaultCameraTarget);
		Vector3 rotation = this.defaultCameraRotation;
		if (this.cameraTargets.Count > 0)
		{
			for (int i = this.cameraTargets.Count - 1; i >= 0; i--)
			{
				if (this.cameraTargets[i].caller && this.cameraTargets[i].caller.activeInHierarchy)
				{
					vector = this.cameraTargets[i].position;
					rotation = this.cameraTargets[i].rotation;
					break;
				}
				this.cameraTargets.RemoveAt(i);
			}
		}
		if (instant)
		{
			this.cameraTarget = vector;
			this.cameraRotation = rotation;
			return;
		}
		this.cameraTarget = Vector3.MoveTowards(this.cameraTarget, vector, Time.deltaTime * 2f * (0.1f + Vector3.Distance(this.cameraTarget, vector)));
		this.cameraRotation = Vector3.MoveTowards(this.cameraRotation, rotation, Time.deltaTime * 2f * (0.1f + Vector3.Distance(this.cameraRotation, rotation)));
	}

	// Token: 0x0600130F RID: 4879 RVA: 0x00098D78 File Offset: 0x00096F78
	private void FixedUpdate()
	{
		this.SlideValues();
		if (this.boost || this.spinning)
		{
			this.rb.useGravity = true;
			this.Dodge();
			return;
		}
		base.gameObject.layer = 2;
		if (this.groundCheck.onGround && !this.jumping)
		{
			this.anim.SetBool("InAir", false);
			this.inSpecialJump = false;
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
			if (this.groundProperties)
			{
				num2 *= this.groundProperties.speedMultiplier;
			}
			this.movementDirection2 = new Vector3(this.movementDirection.x * this.walkSpeed * Time.deltaTime * num2, num, this.movementDirection.z * this.walkSpeed * Time.deltaTime * num2);
			float num3 = 2.5f;
			Vector3 vector = Vector3.zero;
			if (this.groundProperties)
			{
				num3 *= this.groundProperties.friction;
				if (this.groundProperties.push)
				{
					Vector3 vector2 = this.groundProperties.pushForce;
					if (this.groundProperties.pushDirectionRelative)
					{
						vector2 = this.groundProperties.transform.rotation * vector2;
					}
					vector += vector2;
				}
			}
			this.rb.velocity = Vector3.MoveTowards(this.rb.velocity, this.movementDirection2 + vector, num3);
			return;
		}
		this.anim.SetBool("InAir", true);
		this.rb.useGravity = true;
		this.movementDirection2 = new Vector3(this.movementDirection.x * this.walkSpeed * Time.deltaTime * 2.75f, this.rb.velocity.y, this.movementDirection.z * this.walkSpeed * Time.deltaTime * 2.75f);
		this.airDirection.y = 0f;
		if ((this.movementDirection2.x > 0f && this.rb.velocity.x < this.movementDirection2.x) || (this.movementDirection2.x < 0f && this.rb.velocity.x > this.movementDirection2.x))
		{
			this.airDirection.x = this.movementDirection2.x;
		}
		else
		{
			this.airDirection.x = 0f;
			if (!this.inSpecialJump && !this.airFrictionless)
			{
				this.rb.velocity = new Vector3(Mathf.MoveTowards(this.rb.velocity.x, this.movementDirection2.x, Time.fixedDeltaTime * 25f), this.rb.velocity.y, this.rb.velocity.z);
			}
		}
		if ((this.movementDirection2.z > 0f && this.rb.velocity.z < this.movementDirection2.z) || (this.movementDirection2.z < 0f && this.rb.velocity.z > this.movementDirection2.z))
		{
			this.airDirection.z = this.movementDirection2.z;
		}
		else
		{
			this.airDirection.z = 0f;
			if (!this.inSpecialJump && !this.airFrictionless)
			{
				this.rb.velocity = new Vector3(this.rb.velocity.x, this.rb.velocity.y, Mathf.MoveTowards(this.rb.velocity.z, this.movementDirection2.z, Time.fixedDeltaTime * 25f));
			}
		}
		this.rb.AddForce(this.airDirection.normalized * 6000f);
		LayerMask layerMask = LayerMaskDefaults.Get(LMD.Environment);
		if (this.rb.velocity.y < 0f)
		{
			layerMask |= 4096;
		}
		RaycastHit raycastHit;
		if (Physics.SphereCast(base.transform.position + Vector3.up * 2.5f * base.transform.localScale.y, (base.transform.localScale.x + base.transform.localScale.z) / 2f * 0.75f - 0.1f, Vector3.up * this.rb.velocity.y, out raycastHit, 2.51f + this.rb.velocity.y * Time.fixedDeltaTime, layerMask))
		{
			EnemyIdentifier enemyIdentifier2;
			if (LayerMaskDefaults.IsMatchingLayer(raycastHit.transform.gameObject.layer, LMD.Environment))
			{
				Breakable breakable;
				EnemyIdentifier enemyIdentifier;
				if (raycastHit.transform.TryGetComponent<Breakable>(out breakable) && breakable.crate)
				{
					if (this.groundCheck.heavyFall && !breakable.precisionOnly && !breakable.specialCaseOnly)
					{
						breakable.Break();
						return;
					}
					if (this.groundCheck.heavyFall)
					{
						this.SlamEnd(false);
					}
					if (breakable.bounceHealth > 1)
					{
						this.aud.clip = this.bounceSound;
						this.aud.pitch = Mathf.Lerp(1f, 2f, (float)(breakable.originalBounceHealth - breakable.bounceHealth) / (float)breakable.originalBounceHealth);
						this.aud.volume = 0.75f;
						this.aud.Play();
					}
					breakable.Bounce();
					if (base.transform.position.y < raycastHit.transform.position.y)
					{
						this.rb.velocity = new Vector3(MonoSingleton<PlatformerMovement>.Instance.rb.velocity.x, -10f, MonoSingleton<PlatformerMovement>.Instance.rb.velocity.z);
						return;
					}
					if (MonoSingleton<InputManager>.Instance.InputSource.Jump.IsPressed)
					{
						this.Jump(true, 1.35f);
						return;
					}
					this.Jump(true, 0.75f);
					return;
				}
				else if (raycastHit.transform.gameObject.CompareTag("Armor") && raycastHit.transform.TryGetComponent<EnemyIdentifier>(out enemyIdentifier))
				{
					enemyIdentifier.InstaKill();
					return;
				}
			}
			else if (raycastHit.transform.TryGetComponent<EnemyIdentifier>(out enemyIdentifier2) && !enemyIdentifier2.dead)
			{
				if (!enemyIdentifier2.blessed)
				{
					enemyIdentifier2.Splatter(true);
				}
				if (!this.groundCheck.heavyFall)
				{
					if (MonoSingleton<InputManager>.Instance.InputSource.Jump.IsPressed)
					{
						this.Jump(true, 1.25f);
						return;
					}
					this.Jump(true, 0.75f);
				}
			}
		}
	}

	// Token: 0x06001310 RID: 4880 RVA: 0x000994C0 File Offset: 0x000976C0
	public void Jump(bool silent = false, float multiplier = 1f)
	{
		float num = 1500f * multiplier;
		if (this.groundCheck.heavyFall)
		{
			this.SlamEnd(true);
		}
		if (multiplier > 1f || base.transform.position.y > this.lastYPos + 1f)
		{
			if (multiplier <= 1f)
			{
				this.beenOverYPosMax = false;
			}
			else
			{
				this.beenOverYPosMax = true;
				this.yPosDifferential = 5f;
			}
			this.lastYPos = base.transform.position.y;
		}
		if (this.groundProperties)
		{
			if (!this.groundProperties.canJump)
			{
				if (!this.groundProperties.silentJumpFail)
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
		this.anim.SetBool("InAir", true);
		this.anim.Play("Jump");
		this.falling = true;
		this.jumping = true;
		base.Invoke("NotJumping", 0.25f);
		if (!silent)
		{
			this.aud.clip = this.jumpSound;
			if (this.groundCheck.superJumpChance > 0f)
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
		}
		this.rb.velocity = new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z);
		if (this.sliding)
		{
			this.inSpecialJump = true;
			this.rb.AddForce(Vector3.up * this.jumpPower * num * 2f);
			this.StopSlide();
		}
		else if (this.boost)
		{
			if (this.boostCharge >= 100f)
			{
				if (!MonoSingleton<AssistController>.Instance.majorEnabled || !MonoSingleton<AssistController>.Instance.infiniteStamina)
				{
					this.boostCharge -= 100f;
				}
				Object.Instantiate<GameObject>(this.dashJumpSound);
				this.inSpecialJump = true;
			}
			else
			{
				this.rb.velocity = new Vector3(this.movementDirection.x * this.walkSpeed * Time.deltaTime * 2.75f, 0f, this.movementDirection.z * this.walkSpeed * Time.deltaTime * 2.75f);
				Object.Instantiate<GameObject>(this.staminaFailSound);
				this.inSpecialJump = false;
			}
			this.rb.AddForce(Vector3.up * this.jumpPower * num * 1.5f);
		}
		else
		{
			this.inSpecialJump = false;
			this.rb.AddForce(Vector3.up * this.jumpPower * num * 2.6f);
		}
		this.jumpCooldown = true;
		base.Invoke("JumpReady", 0.2f);
		this.boost = false;
	}

	// Token: 0x06001311 RID: 4881 RVA: 0x00099820 File Offset: 0x00097A20
	private void Dodge()
	{
		this.aboutToSlam = false;
		if (this.spinning)
		{
			this.movementDirection2 = new Vector3(this.movementDirection.x * this.spinSpeed, this.rb.velocity.y, this.movementDirection.z * this.spinSpeed);
			if (this.movementDirection.magnitude == 0f && !this.falling)
			{
				this.rb.velocity = new Vector3(Mathf.MoveTowards(this.rb.velocity.x, 0f, Time.fixedDeltaTime * 150f), this.rb.velocity.y, Mathf.MoveTowards(this.rb.velocity.z, 0f, Time.fixedDeltaTime * 150f));
			}
			else
			{
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
				if (this.falling)
				{
					this.rb.AddForce(this.airDirection.normalized * 4000f);
				}
				else
				{
					this.rb.AddForce(this.airDirection.normalized * 24000f);
				}
			}
			this.spinJuice = Mathf.MoveTowards(this.spinJuice, 0f, Time.fixedDeltaTime * 3f);
			if (this.spinJuice <= 0f)
			{
				this.StopSpin();
				return;
			}
		}
		else
		{
			if (this.sliding)
			{
				float num = 1f;
				if (this.preSlideSpeed > 1f)
				{
					if (this.preSlideSpeed > 3f)
					{
						this.preSlideSpeed = 3f;
					}
					num = this.preSlideSpeed;
					this.preSlideSpeed -= Time.fixedDeltaTime * this.preSlideSpeed;
					this.preSlideDelay = 0f;
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
				Vector3 vector = new Vector3(this.dodgeDirection.x * this.walkSpeed * Time.fixedDeltaTime * 5f * num, this.rb.velocity.y, this.dodgeDirection.z * this.walkSpeed * Time.fixedDeltaTime * 5f * num);
				if (this.groundProperties && this.groundProperties.push)
				{
					Vector3 vector2 = this.groundProperties.pushForce;
					if (this.groundProperties.pushDirectionRelative)
					{
						vector2 = this.groundProperties.transform.rotation * vector2;
					}
					vector += vector2;
				}
				this.rb.velocity = vector;
				return;
			}
			float num2 = 0f;
			if (this.slideEnding)
			{
				num2 = this.rb.velocity.y;
			}
			float num3 = 2.25f;
			this.movementDirection2 = new Vector3(this.dodgeDirection.x * this.walkSpeed * Time.fixedDeltaTime * num3, num2, this.dodgeDirection.z * this.walkSpeed * Time.fixedDeltaTime * num3);
			if (!this.slideEnding || this.groundCheck.onGround)
			{
				this.rb.velocity = this.movementDirection2 * 3f;
			}
			base.gameObject.layer = 15;
			this.boostLeft -= 4f;
			if (this.boostLeft <= 0f)
			{
				this.boost = false;
				if (!this.groundCheck.onGround && !this.slideEnding)
				{
					this.rb.velocity = this.movementDirection2;
				}
			}
			this.slideEnding = false;
		}
	}

	// Token: 0x06001312 RID: 4882 RVA: 0x00099CE4 File Offset: 0x00097EE4
	public void Slam()
	{
		this.aboutToSlam = false;
		this.rb.velocity = new Vector3(0f, -100f, 0f);
		this.falling = true;
		this.fallSpeed = -100f;
		this.slamming = true;
		this.groundCheck.heavyFall = true;
		this.slamForce = 1f;
		if (this.currentFallParticle != null)
		{
			Object.Destroy(this.currentFallParticle);
		}
		this.currentFallParticle = Object.Instantiate<GameObject>(this.fallParticle, base.transform);
	}

	// Token: 0x06001313 RID: 4883 RVA: 0x00099D78 File Offset: 0x00097F78
	public void SlamEnd(bool cancel = false)
	{
		this.fallSpeed = 0f;
		this.groundCheck.heavyFall = false;
		this.slamming = false;
		if (this.currentFallParticle != null)
		{
			Object.Destroy(this.currentFallParticle);
		}
		if (!cancel)
		{
			this.anim.Play("SlamEnd", -1, 0f);
			MonoSingleton<CameraController>.Instance.CameraShake(0.5f);
			Object.Instantiate<GameObject>(this.impactDust, base.transform.position, Quaternion.identity);
			MonoSingleton<SceneHelper>.Instance.CreateEnviroGibs(base.transform.position + Vector3.up * 0.1f, Vector3.down, 3f, 5, 1f);
		}
	}

	// Token: 0x06001314 RID: 4884 RVA: 0x00099E3C File Offset: 0x0009803C
	private void Spin()
	{
		this.anim.Play("Spin", -1, 0f);
		this.anim.SetBool("Spinning", true);
		this.spinning = true;
		this.spinJuice = 1f;
		this.spinZone.SetActive(true);
		if (this.sliding)
		{
			float num = 1f;
			if (this.preSlideSpeed > 1f)
			{
				if (this.preSlideSpeed > 3f)
				{
					this.preSlideSpeed = 3f;
				}
				num = this.preSlideSpeed;
			}
			if (this.groundProperties)
			{
				num *= this.groundProperties.speedMultiplier;
			}
			this.spinDirection = this.dodgeDirection;
			this.spinSpeed = this.walkSpeed * 5f * num * Time.fixedDeltaTime;
			this.StopSlide();
			this.boostLeft = 0f;
			this.boost = false;
			this.playerModel.rotation = Quaternion.LookRotation(this.movementDirection);
		}
		else if (this.boost)
		{
			this.spinDirection = this.dodgeDirection;
			this.spinSpeed = this.walkSpeed * 8.25f * Time.fixedDeltaTime;
			this.boostLeft = 0f;
			this.boost = false;
		}
		else
		{
			Vector3 vector = this.rb.velocity;
			vector += this.movementDirection * this.walkSpeed * Time.fixedDeltaTime;
			vector.y = 0f;
			if (vector.magnitude <= 0.25f)
			{
				this.spinDirection = this.playerModel.forward;
			}
			else
			{
				this.spinDirection = vector;
			}
			this.spinSpeed = vector.magnitude;
		}
		this.rb.velocity = new Vector3(this.spinDirection.normalized.x * this.spinSpeed, this.rb.velocity.y, this.spinDirection.normalized.z * this.spinSpeed);
	}

	// Token: 0x06001315 RID: 4885 RVA: 0x0009A03C File Offset: 0x0009823C
	private void StopSpin()
	{
		this.spinning = false;
		this.anim.SetBool("Spinning", false);
		this.spinJuice = 0f;
		this.playerModel.forward = this.spinDirection;
		this.spinCooldown = 0.2f;
		this.spinZone.SetActive(false);
	}

	// Token: 0x06001316 RID: 4886 RVA: 0x0009A094 File Offset: 0x00098294
	private void StartSlide()
	{
		this.slideLength = 0f;
		this.anim.SetBool("Sliding", true);
		if (this.currentSlideEffect != null)
		{
			Object.Destroy(this.currentSlideEffect);
		}
		if (this.currentSlideScrape != null)
		{
			Object.Destroy(this.currentSlideScrape);
		}
		if (this.groundProperties && !this.groundProperties.canSlide)
		{
			if (!this.groundProperties.silentSlideFail)
			{
				this.StopSlide();
			}
			return;
		}
		this.playerCollider.height = 1.25f;
		this.playerCollider.center = Vector3.up * 0.625f;
		this.slideSafety = 1f;
		this.sliding = true;
		this.boost = true;
		this.dodgeDirection = this.movementDirection;
		if (this.dodgeDirection == Vector3.zero)
		{
			this.dodgeDirection = this.playerModel.forward;
		}
		Quaternion identity = Quaternion.identity;
		identity.SetLookRotation(this.dodgeDirection * -1f);
		this.currentSlideEffect = Object.Instantiate<GameObject>(this.slideEffect, base.transform.position + this.dodgeDirection * 10f, identity);
		this.currentSlideScrape = Object.Instantiate<GameObject>(this.slideScrape, base.transform.position + this.dodgeDirection * 2f, identity);
	}

	// Token: 0x06001317 RID: 4887 RVA: 0x0009A210 File Offset: 0x00098410
	public void StopSlide()
	{
		this.anim.SetBool("Sliding", false);
		if (this.currentSlideEffect != null)
		{
			Object.Destroy(this.currentSlideEffect);
		}
		if (this.currentSlideScrape != null)
		{
			Object.Destroy(this.currentSlideScrape);
		}
		if (this.sliding)
		{
			Object.Instantiate<GameObject>(this.slideStopSound);
		}
		this.sliding = false;
		this.slideEnding = true;
		if (this.slideLength > MonoSingleton<NewMovement>.Instance.longestSlide)
		{
			MonoSingleton<NewMovement>.Instance.longestSlide = this.slideLength;
		}
		this.slideLength = 0f;
		if (!this.crouching)
		{
			this.playerCollider.height = 5f;
			this.playerCollider.center = Vector3.up * 2.5f;
		}
	}

	// Token: 0x06001318 RID: 4888 RVA: 0x0009A2E4 File Offset: 0x000984E4
	private void SlideValues()
	{
		if (this.sliding && this.slideSafety <= 0f)
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
		if (!this.sliding && this.activated && !this.groundCheck.heavyFall)
		{
			if (!this.boost && !this.falling && this.rb.velocity.magnitude / 24f > this.preSlideSpeed)
			{
				this.preSlideSpeed = this.rb.velocity.magnitude / 24f;
				this.preSlideDelay = 0.2f;
				return;
			}
			this.preSlideDelay = Mathf.MoveTowards(this.preSlideDelay, 0f, Time.fixedDeltaTime);
			if (this.preSlideDelay <= 0f)
			{
				this.preSlideDelay = 0.2f;
				this.preSlideSpeed = this.rb.velocity.magnitude / 24f;
			}
		}
	}

	// Token: 0x06001319 RID: 4889 RVA: 0x0009A484 File Offset: 0x00098684
	public void EmptyStamina()
	{
		this.boostCharge = 0f;
	}

	// Token: 0x0600131A RID: 4890 RVA: 0x0009A491 File Offset: 0x00098691
	public void FullStamina()
	{
		this.boostCharge = 300f;
	}

	// Token: 0x0600131B RID: 4891 RVA: 0x0009A49E File Offset: 0x0009869E
	private void JumpReady()
	{
		this.jumpCooldown = false;
	}

	// Token: 0x0600131C RID: 4892 RVA: 0x0009A4A7 File Offset: 0x000986A7
	private void NotJumping()
	{
		this.jumping = false;
	}

	// Token: 0x0600131D RID: 4893 RVA: 0x0009A4B0 File Offset: 0x000986B0
	public void AddExtraHit(int amount = 1)
	{
		this.extraHits = Mathf.Clamp(this.extraHits + amount, 0, 3);
		this.CheckProtector();
		Object.Instantiate<GameObject>(this.protectorGet, this.playerCollider.bounds.center, Quaternion.identity, base.transform);
		if (this.extraHits >= 3)
		{
			this.invincible = true;
			this.playerModel.gameObject.SetActive(true);
			this.superTimer = 20f;
		}
	}

	// Token: 0x0600131E RID: 4894 RVA: 0x0009A530 File Offset: 0x00098730
	private void CheckProtector()
	{
		this.extraHits = Mathf.Clamp(this.extraHits, 0, 3);
		for (int i = 0; i <= 2; i++)
		{
			if (i == this.extraHits - 1)
			{
				this.protectors[i].SetActive(true);
			}
			else
			{
				this.protectors[i].SetActive(false);
			}
		}
	}

	// Token: 0x0600131F RID: 4895 RVA: 0x0009A588 File Offset: 0x00098788
	private void GetHit()
	{
		MonoSingleton<StatsManager>.Instance.tookDamage = true;
		this.extraHits--;
		this.CheckProtector();
		Object.Instantiate<GameObject>(this.protectorLose, this.playerCollider.bounds.center, Quaternion.identity, base.transform);
		this.invincible = true;
		base.Invoke("StopInvincibility", 3f);
	}

	// Token: 0x06001320 RID: 4896 RVA: 0x0009A5F5 File Offset: 0x000987F5
	private void StopInvincibility()
	{
		this.playerModel.gameObject.SetActive(true);
		this.invincible = false;
	}

	// Token: 0x06001321 RID: 4897 RVA: 0x0009A610 File Offset: 0x00098810
	private void Death()
	{
		this.cameraTrack = false;
		this.dead = true;
		MonoSingleton<StatsManager>.Instance.tookDamage = true;
		if (this.extraHits > 0)
		{
			this.extraHits = 0;
			this.CheckProtector();
		}
		if (!this.freeCamera)
		{
			this.platformerCamera.transform.position = base.transform.position + this.cameraTarget;
		}
		if (this.boost || this.spinning)
		{
			this.StopSpin();
			this.boost = false;
		}
	}

	// Token: 0x06001322 RID: 4898 RVA: 0x0009A697 File Offset: 0x00098897
	public void Fall()
	{
		if (this.dead)
		{
			return;
		}
		this.Death();
		Object.Instantiate<GameObject>(this.fallSound, base.transform.position, Quaternion.identity);
		base.Invoke("DeathOver", 2f);
	}

	// Token: 0x06001323 RID: 4899 RVA: 0x0009A6D4 File Offset: 0x000988D4
	public void Explode(bool ignoreInvincible = false)
	{
		if (this.dead || (!ignoreInvincible && (this.invincible || Invincibility.Enabled)))
		{
			if (!this.dead && (this.extraHits == 3 || Invincibility.Enabled))
			{
				Object.Instantiate<GameObject>(this.protectorOof, this.playerCollider.bounds.center, Quaternion.identity, base.transform);
				this.Jump(true, 1f);
			}
			return;
		}
		if (!ignoreInvincible && this.extraHits > 0)
		{
			this.GetHit();
			return;
		}
		this.Death();
		GoreZone goreZone = GoreZone.ResolveGoreZone(base.transform);
		GameObject gameObject = Object.Instantiate<GameObject>(MonoSingleton<BloodsplatterManager>.Instance.head, this.playerCollider.bounds.center, Quaternion.identity, goreZone.goreZone);
		foreach (Transform transform in this.playerModel.GetComponentsInChildren<Transform>(true))
		{
			if (transform.childCount <= 0 && Random.Range(0f, 1f) <= 0.5f)
			{
				gameObject = MonoSingleton<BloodsplatterManager>.Instance.GetGore(GoreType.Body, false, false, false, null, false);
				if (!gameObject)
				{
					break;
				}
				gameObject.transform.parent = goreZone.goreZone;
				gameObject.transform.position = transform.position;
				gameObject.SetActive(true);
			}
		}
		base.gameObject.SetActive(false);
		base.Invoke("DeathOver", 2f);
	}

	// Token: 0x06001324 RID: 4900 RVA: 0x0009A844 File Offset: 0x00098A44
	public void Burn(bool ignoreInvincible = false)
	{
		if (this.dead || (!ignoreInvincible && (this.invincible || Invincibility.Enabled)))
		{
			if (!this.dead && (this.extraHits == 3 || Invincibility.Enabled))
			{
				Object.Instantiate<GameObject>(this.protectorOof, this.playerCollider.bounds.center, Quaternion.identity, base.transform);
				this.Jump(true, 1f);
			}
			return;
		}
		if (!ignoreInvincible && this.extraHits > 0)
		{
			this.GetHit();
			return;
		}
		if (this.currentCorpse != null)
		{
			base.CancelInvoke();
			Object.Destroy(this.currentCorpse);
		}
		this.Death();
		if (this.defaultBurnEffect)
		{
			Object.Instantiate<GameObject>(this.defaultBurnEffect, base.transform.position, Quaternion.identity);
		}
		this.currentCorpse = Object.Instantiate<GameObject>(this.playerModel.gameObject, this.playerModel.position, this.playerModel.rotation);
		base.gameObject.SetActive(false);
		SandboxUtils.StripForPreview(this.currentCorpse.transform, this.burnMaterial, true);
		base.Invoke("BecomeAsh", 1f);
	}

	// Token: 0x06001325 RID: 4901 RVA: 0x0009A97C File Offset: 0x00098B7C
	private void BecomeAsh()
	{
		if (!this.currentCorpse)
		{
			return;
		}
		Object.Instantiate<GameObject>(this.ashSound, base.transform.position, Quaternion.identity);
		foreach (Transform transform in this.currentCorpse.transform.GetComponentsInChildren<Transform>())
		{
			Object.Instantiate<GameObject>(this.ashParticle, transform.position, Quaternion.identity);
		}
		Object.Destroy(this.currentCorpse);
		base.Invoke("DeathOver", 1f);
	}

	// Token: 0x06001326 RID: 4902 RVA: 0x0009AA08 File Offset: 0x00098C08
	private void DeathOver()
	{
		this.Respawn();
		MonoSingleton<StatsManager>.Instance.Restart();
	}

	// Token: 0x06001327 RID: 4903 RVA: 0x0009AA1C File Offset: 0x00098C1C
	public void Respawn()
	{
		this.cameraTrack = true;
		this.dead = false;
		this.jumping = false;
		this.jumpCooldown = false;
		this.extraHits = 0;
		this.boostCharge = 300f;
		this.rb.velocity = Vector3.zero;
		base.CancelInvoke();
		if (this.currentCorpse)
		{
			Object.Destroy(this.currentCorpse);
		}
		this.CheckProtector();
		this.StopInvincibility();
	}

	// Token: 0x06001328 RID: 4904 RVA: 0x0009AA91 File Offset: 0x00098C91
	public void CoinGet()
	{
		this.queuedCoins++;
	}

	// Token: 0x06001329 RID: 4905 RVA: 0x0009AAA4 File Offset: 0x00098CA4
	public void CoinGetEffect()
	{
		AudioSource component = Object.Instantiate<GameObject>(this.coinGet, this.playerCollider.bounds.center, Quaternion.identity).GetComponent<AudioSource>();
		if (this.coinTimer > 0f)
		{
			if (this.coinPitch < 1.35f)
			{
				this.coinPitch += 0.025f;
			}
			component.pitch = this.coinPitch;
		}
		else
		{
			this.coinPitch = 1f;
		}
		this.coinTimer = 1.5f;
		this.coinEffectTimer = 0.05f;
		this.queuedCoins--;
	}

	// Token: 0x0600132A RID: 4906 RVA: 0x0009AB43 File Offset: 0x00098D43
	public void SnapCamera()
	{
		this.CheckCameraTarget(true);
		this.platformerCamera.SetPositionAndRotation(base.transform.position + this.cameraTarget, Quaternion.Euler(this.cameraRotation));
	}

	// Token: 0x0600132B RID: 4907 RVA: 0x0009AB78 File Offset: 0x00098D78
	public void SnapCamera(Vector3 targetPos, Vector3 targetRot)
	{
		this.cameraTarget = targetPos;
		this.cameraRotation = targetRot;
		this.platformerCamera.SetPositionAndRotation(targetPos, Quaternion.Euler(targetRot));
	}

	// Token: 0x0600132C RID: 4908 RVA: 0x0009AB9A File Offset: 0x00098D9A
	public void ResetCamera(float degreesY, float degreesX = 0f)
	{
		this.rotationY = degreesY;
		this.rotationX = degreesX;
	}

	// Token: 0x04001A20 RID: 6688
	public Transform platformerCamera;

	// Token: 0x04001A21 RID: 6689
	public Vector3 cameraTarget = new Vector3(0f, 7f, -5.5f);

	// Token: 0x04001A22 RID: 6690
	public Vector3 defaultCameraTarget = new Vector3(0f, 9f, -6.5f);

	// Token: 0x04001A23 RID: 6691
	public Vector3 defaultFreeCameraTarget = new Vector3(0f, 7f, -5.5f);

	// Token: 0x04001A24 RID: 6692
	public Vector3 cameraRotation = new Vector3(20f, 0f, 0f);

	// Token: 0x04001A25 RID: 6693
	private Vector3 defaultCameraRotation = new Vector3(20f, 0f, 0f);

	// Token: 0x04001A26 RID: 6694
	[HideInInspector]
	public List<CameraTargetInfo> cameraTargets = new List<CameraTargetInfo>();

	// Token: 0x04001A27 RID: 6695
	private bool cameraTrack = true;

	// Token: 0x04001A28 RID: 6696
	private bool cameraLowered;

	// Token: 0x04001A29 RID: 6697
	public bool freeCamera;

	// Token: 0x04001A2A RID: 6698
	[HideInInspector]
	public float rotationY;

	// Token: 0x04001A2B RID: 6699
	[HideInInspector]
	public float rotationX;

	// Token: 0x04001A2C RID: 6700
	private float lastYPos;

	// Token: 0x04001A2D RID: 6701
	private float currentYPos;

	// Token: 0x04001A2E RID: 6702
	private bool beenOverYPosMax;

	// Token: 0x04001A2F RID: 6703
	private float yPosDifferential;

	// Token: 0x04001A30 RID: 6704
	public GroundCheck groundCheck;

	// Token: 0x04001A31 RID: 6705
	[SerializeField]
	private GroundCheck slopeCheck;

	// Token: 0x04001A32 RID: 6706
	public Transform playerModel;

	// Token: 0x04001A33 RID: 6707
	[HideInInspector]
	public Rigidbody rb;

	// Token: 0x04001A34 RID: 6708
	private AudioSource aud;

	// Token: 0x04001A35 RID: 6709
	private CapsuleCollider playerCollider;

	// Token: 0x04001A36 RID: 6710
	private Animator anim;

	// Token: 0x04001A37 RID: 6711
	[SerializeField]
	private AudioClip jumpSound;

	// Token: 0x04001A38 RID: 6712
	[SerializeField]
	private AudioClip dodgeSound;

	// Token: 0x04001A39 RID: 6713
	[SerializeField]
	private AudioClip bounceSound;

	// Token: 0x04001A3A RID: 6714
	[HideInInspector]
	public bool activated = true;

	// Token: 0x04001A3B RID: 6715
	private Vector3 movementDirection;

	// Token: 0x04001A3C RID: 6716
	private Vector3 movementDirection2;

	// Token: 0x04001A3D RID: 6717
	private Vector3 airDirection;

	// Token: 0x04001A3E RID: 6718
	private Vector3 dodgeDirection;

	// Token: 0x04001A3F RID: 6719
	private float walkSpeed = 600f;

	// Token: 0x04001A40 RID: 6720
	private float jumpPower = 80f;

	// Token: 0x04001A41 RID: 6721
	private bool airFrictionless;

	// Token: 0x04001A42 RID: 6722
	private bool boost;

	// Token: 0x04001A43 RID: 6723
	private float boostCharge = 300f;

	// Token: 0x04001A44 RID: 6724
	private float boostLeft;

	// Token: 0x04001A45 RID: 6725
	[SerializeField]
	private GameObject staminaFailSound;

	// Token: 0x04001A46 RID: 6726
	[SerializeField]
	private GameObject dodgeParticle;

	// Token: 0x04001A47 RID: 6727
	[SerializeField]
	private GameObject dashJumpSound;

	// Token: 0x04001A48 RID: 6728
	private MaterialPropertyBlock block;

	// Token: 0x04001A49 RID: 6729
	private SkinnedMeshRenderer smr;

	// Token: 0x04001A4A RID: 6730
	[HideInInspector]
	public bool sliding;

	// Token: 0x04001A4B RID: 6731
	private bool crouching;

	// Token: 0x04001A4C RID: 6732
	private bool slideEnding;

	// Token: 0x04001A4D RID: 6733
	private float preSlideSpeed;

	// Token: 0x04001A4E RID: 6734
	private float preSlideDelay;

	// Token: 0x04001A4F RID: 6735
	private float slideSafety;

	// Token: 0x04001A50 RID: 6736
	private float slideLength;

	// Token: 0x04001A51 RID: 6737
	[SerializeField]
	private GameObject slideStopSound;

	// Token: 0x04001A52 RID: 6738
	[SerializeField]
	private GameObject slideEffect;

	// Token: 0x04001A53 RID: 6739
	[SerializeField]
	private GameObject slideScrape;

	// Token: 0x04001A54 RID: 6740
	private GameObject currentSlideEffect;

	// Token: 0x04001A55 RID: 6741
	private GameObject currentSlideScrape;

	// Token: 0x04001A56 RID: 6742
	[SerializeField]
	private GameObject fallParticle;

	// Token: 0x04001A57 RID: 6743
	private GameObject currentFallParticle;

	// Token: 0x04001A58 RID: 6744
	private bool jumping;

	// Token: 0x04001A59 RID: 6745
	private bool inSpecialJump;

	// Token: 0x04001A5A RID: 6746
	private bool jumpCooldown;

	// Token: 0x04001A5B RID: 6747
	[HideInInspector]
	public CustomGroundProperties groundProperties;

	// Token: 0x04001A5C RID: 6748
	public Transform jumpShadow;

	// Token: 0x04001A5D RID: 6749
	private bool falling;

	// Token: 0x04001A5E RID: 6750
	private float fallSpeed;

	// Token: 0x04001A5F RID: 6751
	private float fallTime;

	// Token: 0x04001A60 RID: 6752
	private bool aboutToSlam;

	// Token: 0x04001A61 RID: 6753
	private TimeSince slamWindUp;

	// Token: 0x04001A62 RID: 6754
	[SerializeField]
	private AudioSource slamReadySound;

	// Token: 0x04001A63 RID: 6755
	private bool slamming;

	// Token: 0x04001A64 RID: 6756
	public float slamForce;

	// Token: 0x04001A65 RID: 6757
	[SerializeField]
	private GameObject impactDust;

	// Token: 0x04001A66 RID: 6758
	private bool spinning;

	// Token: 0x04001A67 RID: 6759
	private float spinJuice;

	// Token: 0x04001A68 RID: 6760
	private Vector3 spinDirection;

	// Token: 0x04001A69 RID: 6761
	private float spinSpeed;

	// Token: 0x04001A6A RID: 6762
	private float spinCooldown;

	// Token: 0x04001A6B RID: 6763
	public Transform holder;

	// Token: 0x04001A6C RID: 6764
	private int difficulty;

	// Token: 0x04001A6D RID: 6765
	[SerializeField]
	private GameObject spinZone;

	// Token: 0x04001A6E RID: 6766
	[SerializeField]
	private GameObject coinGet;

	// Token: 0x04001A6F RID: 6767
	private float coinTimer;

	// Token: 0x04001A70 RID: 6768
	private float coinPitch;

	// Token: 0x04001A71 RID: 6769
	private int queuedCoins;

	// Token: 0x04001A72 RID: 6770
	private float coinEffectTimer;

	// Token: 0x04001A73 RID: 6771
	public int extraHits;

	// Token: 0x04001A74 RID: 6772
	private bool invincible;

	// Token: 0x04001A75 RID: 6773
	private float blinkTimer;

	// Token: 0x04001A76 RID: 6774
	public GameObject[] protectors;

	// Token: 0x04001A77 RID: 6775
	private float superTimer;

	// Token: 0x04001A78 RID: 6776
	public GameObject protectorGet;

	// Token: 0x04001A79 RID: 6777
	public GameObject protectorLose;

	// Token: 0x04001A7A RID: 6778
	public GameObject protectorOof;

	// Token: 0x04001A7B RID: 6779
	private InputBinding rbSlide;

	// Token: 0x04001A7C RID: 6780
	private InputBinding dpadMove;

	// Token: 0x04001A7D RID: 6781
	[Header("Death Stuff")]
	[SerializeField]
	private Material burnMaterial;

	// Token: 0x04001A7E RID: 6782
	[SerializeField]
	private GameObject defaultBurnEffect;

	// Token: 0x04001A7F RID: 6783
	[SerializeField]
	private GameObject ashParticle;

	// Token: 0x04001A80 RID: 6784
	[SerializeField]
	private GameObject ashSound;

	// Token: 0x04001A81 RID: 6785
	private GameObject currentCorpse;

	// Token: 0x04001A82 RID: 6786
	[SerializeField]
	private GameObject fallSound;

	// Token: 0x04001A83 RID: 6787
	[HideInInspector]
	public bool dead;
}
