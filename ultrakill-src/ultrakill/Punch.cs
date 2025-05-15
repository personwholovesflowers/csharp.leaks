using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000365 RID: 869
public class Punch : MonoBehaviour
{
	// Token: 0x0600141F RID: 5151 RVA: 0x000A1BF8 File Offset: 0x0009FDF8
	private void Awake()
	{
		this.inman = MonoSingleton<InputManager>.Instance;
		this.anim = base.GetComponent<Animator>();
		this.smr = base.GetComponentInChildren<SkinnedMeshRenderer>();
		this.rev = base.transform.parent.parent.GetComponentInChildren<Revolver>();
		this.camObj = MonoSingleton<CameraController>.Instance.gameObject;
		this.cc = MonoSingleton<CameraController>.Instance;
		this.aud = base.GetComponent<AudioSource>();
		this.parryLight = base.transform.Find("PunchZone").GetComponent<Light>();
		this.nmov = base.GetComponentInParent<NewMovement>();
		this.tr = base.GetComponentInChildren<TrailRenderer>();
		this.shud = MonoSingleton<StyleHUD>.Instance;
		this.sman = MonoSingleton<StatsManager>.Instance;
		this.environmentMask = LayerMaskDefaults.Get(LMD.Environment);
		this.environmentMask |= 4194304;
	}

	// Token: 0x06001420 RID: 5152 RVA: 0x000A1CDC File Offset: 0x0009FEDC
	private void Start()
	{
		this.holdingInput = false;
		if (this.fc == null)
		{
			this.fc = MonoSingleton<FistControl>.Instance;
		}
		FistType fistType = this.type;
		if (fistType == FistType.Standard)
		{
			this.damage = 1f;
			this.screenShakeMultiplier = 1f;
			this.force = 25f;
			this.tryForExplode = false;
			this.cooldownCost = 2f;
			this.hitter = "punch";
			return;
		}
		if (fistType != FistType.Heavy)
		{
			return;
		}
		this.damage = 2.5f;
		this.screenShakeMultiplier = 2f;
		this.force = 100f;
		this.tryForExplode = true;
		this.cooldownCost = 3f;
		this.hitter = "heavypunch";
	}

	// Token: 0x06001421 RID: 5153 RVA: 0x000A1D98 File Offset: 0x0009FF98
	private void OnEnable()
	{
		this.holdingInput = false;
		this.ReadyToPunch();
		this.ignoreDoublePunch = false;
		if (this.fc == null)
		{
			this.fc = base.GetComponentInParent<FistControl>();
			this.anim = base.GetComponent<Animator>();
		}
		if (this.fc.heldObject != null)
		{
			this.heldItem = this.fc.heldObject;
			this.heldItem.transform.SetParent(this.holder, true);
			this.holding = true;
			if (!this.heldItem.noHoldingAnimation && this.fc.forceNoHold <= 0)
			{
				this.anim.SetBool("SemiHolding", false);
				this.anim.SetBool("Holding", true);
				this.anim.Play("Holding", -1, 0f);
			}
			else
			{
				this.anim.SetBool("SemiHolding", true);
			}
			this.ResetHeldItemPosition();
		}
	}

	// Token: 0x06001422 RID: 5154 RVA: 0x000A1E90 File Offset: 0x000A0090
	public void ResetHeldState()
	{
		this.holding = false;
		this.anim.SetBool("Holding", false);
		this.anim.SetBool("SemiHolding", false);
	}

	// Token: 0x06001423 RID: 5155 RVA: 0x000A1EBC File Offset: 0x000A00BC
	public void ForceThrow()
	{
		if (!this.heldItem)
		{
			this.ResetHeldState();
			return;
		}
		ItemIdentifier itemIdentifier = this.heldItem;
		Rigidbody[] componentsInChildren = itemIdentifier.GetComponentsInChildren<Rigidbody>();
		if (componentsInChildren == null || componentsInChildren.Length == 0)
		{
			return;
		}
		this.ForceDrop();
		Rigidbody[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].AddForce((base.transform.parent.forward + Vector3.up * 0.1f) * 5000f);
		}
		itemIdentifier.transform.position = base.transform.parent.position + base.transform.parent.forward;
	}

	// Token: 0x06001424 RID: 5156 RVA: 0x000A1F70 File Offset: 0x000A0170
	public void ForceDrop()
	{
		if (!this.heldItem)
		{
			this.ResetHeldState();
			return;
		}
		Rigidbody[] componentsInChildren = this.heldItem.GetComponentsInChildren<Rigidbody>();
		if (componentsInChildren != null && componentsInChildren.Length != 0)
		{
			this.heldItem.transform.SetParent(null, true);
			this.heldItem.pickedUp = false;
			if (this.heldItem.reverseTransformSettings)
			{
				this.heldItem.transform.localScale = Vector3.one;
			}
			else
			{
				this.heldItem.transform.localScale = this.heldItem.putDownScale;
			}
			foreach (Transform transform in this.heldItem.GetComponentsInChildren<Transform>())
			{
				transform.gameObject.layer = 22;
				OutdoorsChecker outdoorsChecker;
				if (transform.TryGetComponent<OutdoorsChecker>(out outdoorsChecker) && outdoorsChecker.enabled)
				{
					outdoorsChecker.CancelInvoke("SlowUpdate");
					outdoorsChecker.SlowUpdate();
				}
			}
			Rigidbody[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].isKinematic = false;
			}
			Collider[] componentsInChildren3 = this.heldItem.GetComponentsInChildren<Collider>();
			for (int i = 0; i < componentsInChildren3.Length; i++)
			{
				componentsInChildren3[i].enabled = true;
			}
			if (!this.heldItem.hooked)
			{
				this.heldItem.transform.position = base.transform.parent.position + base.transform.parent.forward;
			}
			this.heldItem.SendMessage("PutDown", SendMessageOptions.DontRequireReceiver);
			this.anim.SetBool("Holding", false);
			this.anim.SetBool("SemiHolding", false);
			this.holding = false;
			this.fc.heldObject = null;
			this.heldItem = null;
		}
	}

	// Token: 0x06001425 RID: 5157 RVA: 0x000A2124 File Offset: 0x000A0324
	public void PlaceHeldObject(ItemPlaceZone[] placeZones, Transform target)
	{
		if (!this.heldItem)
		{
			this.ResetHeldState();
			return;
		}
		AnimatorStateInfo currentAnimatorStateInfo = this.anim.GetCurrentAnimatorStateInfo(0);
		if (currentAnimatorStateInfo.IsName("JabHolding"))
		{
			this.ignoreDoublePunch = true;
			this.anim.Play("Jab", 0, currentAnimatorStateInfo.normalizedTime);
		}
		this.holding = false;
		this.anim.SetBool("Holding", false);
		this.anim.SetBool("SemiHolding", false);
		this.heldItem.transform.SetParent(target);
		this.heldItem.pickedUp = false;
		if (this.heldItem.reverseTransformSettings)
		{
			this.heldItem.transform.localPosition = Vector3.zero;
			this.heldItem.transform.localScale = Vector3.one;
			this.heldItem.transform.localRotation = Quaternion.identity;
		}
		else
		{
			this.heldItem.transform.localPosition = this.heldItem.putDownPosition;
			this.heldItem.transform.localScale = this.heldItem.putDownScale;
			this.heldItem.transform.localRotation = Quaternion.Euler(this.heldItem.putDownRotation);
		}
		foreach (Transform transform in this.heldItem.GetComponentsInChildren<Transform>())
		{
			transform.gameObject.layer = 22;
			OutdoorsChecker outdoorsChecker;
			if (transform.TryGetComponent<OutdoorsChecker>(out outdoorsChecker) && outdoorsChecker.enabled)
			{
				outdoorsChecker.CancelInvoke("SlowUpdate");
				outdoorsChecker.SlowUpdate();
			}
		}
		Collider[] componentsInChildren2 = this.heldItem.GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].enabled = true;
		}
		this.heldItem.SendMessage("PutDown", SendMessageOptions.DontRequireReceiver);
		Object.Instantiate<GameObject>(this.heldItem.pickUpSound);
		this.heldItem = null;
		this.fc.heldObject = null;
		for (int i = 0; i < placeZones.Length; i++)
		{
			placeZones[i].CheckItem(false);
		}
		this.ResetHeldState();
	}

	// Token: 0x06001426 RID: 5158 RVA: 0x000A2334 File Offset: 0x000A0534
	public void ResetHeldItemPosition()
	{
		if (this.heldItem.reverseTransformSettings)
		{
			this.heldItem.transform.localPosition = this.heldItem.putDownPosition;
			this.heldItem.transform.localScale = this.heldItem.putDownScale;
			this.heldItem.transform.localRotation = Quaternion.Euler(this.heldItem.putDownRotation);
		}
		else
		{
			this.heldItem.transform.localPosition = Vector3.zero;
			this.heldItem.transform.localScale = Vector3.one;
			this.heldItem.transform.localRotation = Quaternion.identity;
		}
		Transform[] componentsInChildren = this.heldItem.GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = 13;
		}
	}

	// Token: 0x06001427 RID: 5159 RVA: 0x000A2410 File Offset: 0x000A0610
	public void ForceHold(ItemIdentifier itid)
	{
		this.holding = true;
		FishObjectReference fishObjectReference;
		if (itid.TryGetComponent<FishObjectReference>(out fishObjectReference) && MonoSingleton<FishManager>.Instance && MonoSingleton<FishManager>.Instance.recognizedFishes.ContainsKey(fishObjectReference.fishObject) && !MonoSingleton<FishManager>.Instance.recognizedFishes[fishObjectReference.fishObject])
		{
			MonoSingleton<FishManager>.Instance.UnlockFish(fishObjectReference.fishObject);
			MonoSingleton<FishingHUD>.Instance.ShowFishCaught(true, fishObjectReference.fishObject);
		}
		if (!itid.noHoldingAnimation && this.fc.forceNoHold <= 0)
		{
			this.anim.SetBool("SemiHolding", false);
			this.anim.SetBool("Holding", true);
		}
		else
		{
			this.anim.SetBool("SemiHolding", true);
		}
		AnimatorStateInfo currentAnimatorStateInfo = this.anim.GetCurrentAnimatorStateInfo(0);
		if (currentAnimatorStateInfo.IsName("Jab") || currentAnimatorStateInfo.IsName("Jab2"))
		{
			this.ignoreDoublePunch = true;
			this.anim.Play("JabHolding", 0, currentAnimatorStateInfo.normalizedTime);
		}
		ItemPlaceZone[] componentsInParent = itid.GetComponentsInParent<ItemPlaceZone>();
		itid.ipz = null;
		this.heldItem = itid;
		itid.transform.SetParent(this.holder);
		this.fc.heldObject = itid;
		itid.pickedUp = true;
		itid.beenPickedUp = true;
		itid.SendMessage("OffCorrectUse", SendMessageOptions.DontRequireReceiver);
		this.ResetHeldItemPosition();
		Transform[] componentsInChildren = this.heldItem.GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = 13;
		}
		Rigidbody[] componentsInChildren2 = this.heldItem.GetComponentsInChildren<Rigidbody>();
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].isKinematic = true;
		}
		Collider[] componentsInChildren3 = this.heldItem.GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren3.Length; i++)
		{
			componentsInChildren3[i].enabled = false;
		}
		Object.Instantiate<GameObject>(itid.pickUpSound);
		this.heldItem.SendMessage("PickUp", SendMessageOptions.DontRequireReceiver);
		if (componentsInParent != null && componentsInParent.Length != 0)
		{
			ItemPlaceZone[] array = componentsInParent;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].CheckItem(false);
			}
		}
	}

	// Token: 0x06001428 RID: 5160 RVA: 0x000A2638 File Offset: 0x000A0838
	private void OnDisable()
	{
		this.holding = false;
		this.anim.SetBool("Holding", false);
		this.anim.SetBool("SemiHolding", false);
		this.ignoreDoublePunch = false;
		if (this.punchChainsawsRoutine != null)
		{
			this.punchChainsawsRoutine = null;
			foreach (Chainsaw chainsaw in this.punchedChainsaws)
			{
				if (!(chainsaw == null))
				{
					chainsaw.beingPunched = false;
				}
			}
		}
	}

	// Token: 0x06001429 RID: 5161 RVA: 0x000A26D4 File Offset: 0x000A08D4
	private void Update()
	{
		if (MonoSingleton<OptionsManager>.Instance.paused)
		{
			return;
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Punch.WasPerformedThisFrame && this.ready && !this.shopping && this.fc.fistCooldown <= 0f && MonoSingleton<WeaponCharges>.Instance.punchStamina >= 1f && this.fc.activated && !GameStateManager.Instance.PlayerInputLocked)
		{
			this.heldAction = MonoSingleton<InputManager>.Instance.InputSource.Punch.Action;
			this.PunchStart();
		}
		if (this.holdingInput && this.heldAction.WasReleasedThisFrame())
		{
			this.holdingInput = false;
		}
		float layerWeight = this.anim.GetLayerWeight(1);
		if (this.shopping && layerWeight < 1f)
		{
			this.anim.SetLayerWeight(1, Mathf.MoveTowards(layerWeight, 1f, Time.deltaTime / 10f + 5f * Time.deltaTime * (1f - layerWeight)));
		}
		else if (!this.shopping && layerWeight > 0f)
		{
			this.anim.SetLayerWeight(1, Mathf.MoveTowards(layerWeight, 0f, Time.deltaTime / 10f + 5f * Time.deltaTime * layerWeight));
		}
		if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame && this.shopping)
		{
			this.anim.SetTrigger("ShopTap");
		}
		if (this.returnToOrigRot)
		{
			base.transform.parent.localRotation = Quaternion.RotateTowards(base.transform.parent.localRotation, Quaternion.identity, (Quaternion.Angle(base.transform.parent.localRotation, Quaternion.identity) * 5f + 5f) * Time.deltaTime * 5f);
			if (base.transform.parent.localRotation == Quaternion.identity)
			{
				this.returnToOrigRot = false;
			}
		}
		if (this.fc.shopping && !this.shopping)
		{
			this.ShopMode();
		}
		else if (!this.fc.shopping && this.shopping)
		{
			this.StopShop();
		}
		if (this.holding)
		{
			if (this.heldItem.Equals(null))
			{
				MonoSingleton<FistControl>.Instance.currentPunch.ResetHeldState();
				return;
			}
			if (!this.heldItem.noHoldingAnimation && this.fc.forceNoHold <= 0)
			{
				this.anim.SetBool("SemiHolding", false);
				this.anim.SetBool("Holding", true);
				return;
			}
			this.anim.SetBool("SemiHolding", true);
		}
	}

	// Token: 0x0600142A RID: 5162 RVA: 0x000A2990 File Offset: 0x000A0B90
	public void PunchStart()
	{
		this.holdingInput = true;
		if (this.ready)
		{
			this.ready = false;
			this.anim.SetFloat("PunchRandomizer", Random.Range(0f, 1f));
			this.anim.SetTrigger("Punch");
			this.fc.fistCooldown = this.cooldownCost * 0.25f;
			MonoSingleton<WeaponCharges>.Instance.punchStamina -= this.cooldownCost / 2f;
			this.hitSomething = false;
			this.parriedSomething = false;
			this.alreadyHitCoin = false;
			this.aud.pitch = Random.Range(0.9f, 1.1f);
			this.aud.Play();
			this.tr.widthMultiplier = 0.5f;
			MonoSingleton<HookArm>.Instance.Cancel();
			if (this.holding && this.heldItem)
			{
				this.heldItem.SendMessage("PunchWith", SendMessageOptions.DontRequireReceiver);
			}
			MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.Punch);
		}
	}

	// Token: 0x0600142B RID: 5163 RVA: 0x000A2AA4 File Offset: 0x000A0CA4
	private void ActiveStart()
	{
		if (this.ignoreDoublePunch)
		{
			this.ignoreDoublePunch = false;
			return;
		}
		this.returnToOrigRot = false;
		this.hitSomething = false;
		this.parriedSomething = false;
		this.activeFrames = (MonoSingleton<AssistController>.Instance ? MonoSingleton<AssistController>.Instance.punchAssistFrames : 6);
		bool flag = this.holding;
		this.hasHeldItem = this.holding;
		this.ActiveFrame(true);
		if (flag && this.holding && this.heldItem != null)
		{
			this.ForceThrow();
		}
	}

	// Token: 0x0600142C RID: 5164 RVA: 0x000A2B2C File Offset: 0x000A0D2C
	private void FixedUpdate()
	{
		if (this.activeFrames > 0)
		{
			this.activeFrames--;
			this.ActiveFrame(false);
			if (this.activeFrames == 0)
			{
				this.hasHeldItem = false;
			}
		}
	}

	// Token: 0x0600142D RID: 5165 RVA: 0x000A2B5C File Offset: 0x000A0D5C
	private void ActiveFrame(bool firstFrame = false)
	{
		if (this.type == FistType.Standard && !this.parriedSomething)
		{
			Collider[] array = Physics.OverlapSphere(this.cc.GetDefaultPos(), 0.01f, 16384, QueryTriggerInteraction.Collide);
			List<Transform> list = new List<Transform>();
			foreach (Collider collider in array)
			{
				list.Add(collider.transform);
				if (this.TryParryProjectile((collider.attachedRigidbody != null) ? collider.attachedRigidbody.transform : collider.transform, firstFrame))
				{
					break;
				}
			}
			bool flag = Physics.Raycast(this.cc.GetDefaultPos(), this.camObj.transform.forward, out this.hit, 4f, 16384);
			if (!flag)
			{
				flag = Physics.BoxCast(this.cc.GetDefaultPos(), Vector3.one * 0.3f, this.camObj.transform.forward, out this.hit, this.camObj.transform.rotation, 4f, 16384);
			}
			if (!flag || list.Contains(this.hit.transform) || !this.TryParryProjectile(this.hit.transform, firstFrame))
			{
				if (this.ppz == null)
				{
					this.ppz = base.transform.parent.GetComponentInChildren<ProjectileParryZone>();
				}
				if (this.ppz != null)
				{
					Projectile projectile = this.ppz.CheckParryZone();
					if (projectile != null)
					{
						bool flag2 = !this.alreadyBoostedProjectile && firstFrame;
						if (!list.Contains(projectile.transform) && !projectile.unparryable && !projectile.undeflectable && (flag2 || !projectile.playerBullet))
						{
							this.ParryProjectile(projectile);
							this.parriedSomething = true;
							this.hitSomething = true;
						}
					}
				}
			}
		}
		else if (this.type == FistType.Heavy && !this.hitSomething)
		{
			Transform transform = null;
			Collider[] array3 = Physics.OverlapSphere(this.cc.GetDefaultPos(), 0.1f, 16384);
			if (array3.Length != 0)
			{
				transform = array3[0].transform;
			}
			else if (Physics.Raycast(this.cc.GetDefaultPos(), this.camObj.transform.forward, out this.hit, 4f, 16384) || Physics.BoxCast(this.cc.GetDefaultPos(), Vector3.one * 0.3f, this.camObj.transform.forward, out this.hit, this.camObj.transform.rotation, 4f, 16384))
			{
				transform = this.hit.transform;
			}
			if (transform != null)
			{
				MassSpear massSpear;
				if (transform.TryGetComponent<MassSpear>(out massSpear) && massSpear.hitPlayer)
				{
					Object.Instantiate<AudioSource>(this.specialHit, base.transform.position, Quaternion.identity);
					MonoSingleton<TimeController>.Instance.HitStop(0.1f);
					this.cc.CameraShake(0.5f * this.screenShakeMultiplier);
					massSpear.GetHurt(10f);
					this.hitSomething = true;
				}
				Chainsaw chainsaw;
				if (transform.TryGetComponent<Chainsaw>(out chainsaw))
				{
					MonoSingleton<WeaponCharges>.Instance.punchStamina = 2f;
					chainsaw.transform.position = MonoSingleton<CameraController>.Instance.GetDefaultPos();
					chainsaw.transform.rotation = Quaternion.LookRotation(chainsaw.transform.position - Punch.GetParryLookTarget());
					chainsaw.transform.position -= chainsaw.transform.forward;
					chainsaw.rb.velocity = chainsaw.transform.forward * -105f;
					chainsaw.stopped = false;
					MonoSingleton<TimeController>.Instance.ParryFlash();
					chainsaw.TurnIntoSawblade();
					this.hitSomething = true;
				}
			}
		}
		bool flag3 = Physics.Raycast(this.cc.GetDefaultPos(), this.camObj.transform.forward, out this.hit, 4f, LayerMaskDefaults.Get(LMD.Enemies), QueryTriggerInteraction.Collide);
		if (!flag3)
		{
			flag3 = Physics.SphereCast(this.cc.GetDefaultPos(), 1f, this.camObj.transform.forward, out this.hit, 4f, LayerMaskDefaults.Get(LMD.Enemies), QueryTriggerInteraction.Collide);
		}
		if (flag3)
		{
			if (!this.alreadyHitCoin && this.type == FistType.Standard && this.hit.collider.CompareTag("Coin"))
			{
				Coin component = this.hit.collider.GetComponent<Coin>();
				if (component && component.doubled)
				{
					this.anim.Play("Hook", 0, 0.065f);
					component.DelayedPunchflection();
					this.alreadyHitCoin = true;
				}
			}
			if (this.hitSomething)
			{
				return;
			}
			bool flag4 = false;
			RaycastHit raycastHit;
			if (Physics.Raycast(this.cc.GetDefaultPos(), this.hit.point - this.cc.GetDefaultPos(), out raycastHit, 5f, this.environmentMask) && Vector3.Distance(this.cc.GetDefaultPos(), this.hit.point) > Vector3.Distance(this.cc.GetDefaultPos(), raycastHit.point))
			{
				flag4 = true;
			}
			if (!flag4)
			{
				this.PunchSuccess(this.hit.point, this.hit.transform);
				this.hitSomething = true;
			}
		}
		if (this.hitSomething)
		{
			return;
		}
		Collider[] array4 = Physics.OverlapSphere(this.cc.GetDefaultPos(), 0.1f, LayerMaskDefaults.Get(LMD.Enemies), QueryTriggerInteraction.Collide);
		if (array4 != null && array4.Length != 0)
		{
			foreach (Collider collider2 in array4)
			{
				this.PunchSuccess(this.cc.GetDefaultPos(), collider2.transform);
			}
			this.hitSomething = true;
		}
		if (this.type == FistType.Standard && !this.hitSomething && !this.parriedSomething)
		{
			Collider[] array5 = Physics.OverlapSphere(this.cc.GetDefaultPos() + this.camObj.transform.forward * 3f, 3f, 16384, QueryTriggerInteraction.Collide);
			bool flag5 = false;
			foreach (Collider collider3 in array5)
			{
				Nail nail;
				if (collider3.attachedRigidbody)
				{
					nail = collider3.attachedRigidbody.GetComponent<Nail>();
				}
				else
				{
					nail = collider3.GetComponent<Nail>();
				}
				if (!(nail == null) && nail.sawblade && nail.punchable)
				{
					flag5 = true;
					if (nail.stopped)
					{
						nail.stopped = false;
						nail.rb.velocity = (Punch.GetParryLookTarget() - nail.transform.position).normalized * nail.originalVelocity.magnitude;
					}
					else
					{
						nail.rb.velocity = (Punch.GetParryLookTarget() - nail.transform.position).normalized * nail.rb.velocity.magnitude;
					}
					nail.punched = true;
					if (nail.magnets.Count > 0)
					{
						nail.punchDistance = Vector3.Distance(nail.transform.position, nail.GetTargetMagnet().transform.position);
					}
				}
			}
			if (!flag5)
			{
				foreach (Collider collider4 in Physics.OverlapSphere(this.cc.GetDefaultPos() + this.camObj.transform.forward, 1f, 1, QueryTriggerInteraction.Collide))
				{
					float num = Vector3.Distance(this.cc.GetDefaultPos() + this.camObj.transform.forward, collider4.transform.position);
					Magnet magnet;
					if (num >= 6f && num <= 12f && Mathf.Abs((this.cc.GetDefaultPos() + this.camObj.transform.forward).y - collider4.transform.position.y) <= 3f && collider4.TryGetComponent<Magnet>(out magnet) && magnet.sawblades.Count > 0)
					{
						float num2 = float.PositiveInfinity;
						int num3 = -1;
						for (int j = magnet.sawblades.Count - 1; j >= 0; j--)
						{
							if (magnet.sawblades[j] == null)
							{
								magnet.sawblades.RemoveAt(j);
								if (flag5)
								{
									num3--;
								}
							}
							else
							{
								float num4 = Vector3.Distance(magnet.sawblades[j].transform.position, this.cc.GetDefaultPos());
								if (magnet.sawblades[j] != null && (num3 < 0 || num2 < num4))
								{
									num3 = j;
									num2 = num4;
									flag5 = true;
								}
							}
						}
						Nail nail2;
						if (flag5 && magnet.sawblades[num3].TryGetComponent<Nail>(out nail2))
						{
							nail2.transform.position = this.cc.GetDefaultPos() + this.cc.transform.forward;
							if (nail2.stopped)
							{
								nail2.stopped = false;
								nail2.rb.velocity = (Punch.GetParryLookTarget() - nail2.transform.position).normalized * nail2.originalVelocity.magnitude;
							}
							else
							{
								nail2.rb.velocity = (Punch.GetParryLookTarget() - nail2.transform.position).normalized * nail2.rb.velocity.magnitude;
							}
							nail2.punched = true;
							if (nail2.magnets.Count <= 0)
							{
								break;
							}
							Magnet targetMagnet = nail2.GetTargetMagnet();
							if (Vector3.Distance(nail2.transform.position + nail2.rb.velocity.normalized, targetMagnet.transform.position) > Vector3.Distance(nail2.transform.position, targetMagnet.transform.position))
							{
								nail2.MagnetRelease(targetMagnet);
								break;
							}
							nail2.punchDistance = Vector3.Distance(nail2.transform.position, targetMagnet.transform.position);
							break;
						}
					}
				}
			}
			if (flag5)
			{
				Object.Instantiate<AudioSource>(this.specialHit, base.transform.position, Quaternion.identity);
				MonoSingleton<TimeController>.Instance.HitStop(0.1f);
				this.anim.Play("Hook", -1, 0.065f);
				this.parriedSomething = true;
				this.hitSomething = true;
			}
		}
		if (Physics.CheckSphere(this.cc.GetDefaultPos(), 0.01f, this.environmentMask, QueryTriggerInteraction.Collide))
		{
			foreach (Collider collider5 in Physics.OverlapSphere(this.cc.GetDefaultPos(), 0.01f, this.environmentMask))
			{
				this.AltHit(collider5.transform);
			}
			return;
		}
		if (Physics.Raycast(this.cc.GetDefaultPos(), this.camObj.transform.forward, out this.hit, 4f, this.environmentMask))
		{
			this.AltHit(this.hit.transform);
			if (LayerMaskDefaults.IsMatchingLayer(this.hit.transform.gameObject.layer, LMD.Environment))
			{
				this.hitSomething = true;
				base.transform.parent.localRotation = Quaternion.identity;
				this.cc.CameraShake(0.2f * this.screenShakeMultiplier);
				Object.Instantiate<AudioSource>(this.normalHit, base.transform.position, Quaternion.identity);
				this.currentDustParticle = Object.Instantiate<GameObject>(this.dustParticle, this.hit.point, base.transform.rotation);
				this.currentDustParticle.transform.forward = this.hit.normal;
				Breakable component2 = this.hit.transform.gameObject.GetComponent<Breakable>();
				if (component2 != null && !component2.precisionOnly && !component2.specialCaseOnly && (component2.weak || this.type == FistType.Heavy))
				{
					component2.Break();
				}
				Bleeder bleeder;
				if (this.hit.collider.gameObject.TryGetComponent<Bleeder>(out bleeder))
				{
					if (this.type == FistType.Standard)
					{
						bleeder.GetHit(this.hit.point, GoreType.Body, false);
					}
					else
					{
						bleeder.GetHit(this.hit.point, GoreType.Head, false);
					}
				}
				if (this.type == FistType.Heavy)
				{
					Glass component3 = this.hit.collider.gameObject.GetComponent<Glass>();
					if (component3 != null && !component3.broken)
					{
						component3.Shatter();
					}
				}
				this.HitSurface(this.hit);
			}
		}
	}

	// Token: 0x0600142E RID: 5166 RVA: 0x000A38F7 File Offset: 0x000A1AF7
	private void HitSurface(RaycastHit hit)
	{
		if (this.holding)
		{
			this.heldItem.SendMessage("HitSurface", hit, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x0600142F RID: 5167 RVA: 0x000A3918 File Offset: 0x000A1B18
	private bool TryParryProjectile(Transform target, bool canProjectileBoost = false)
	{
		ParryHelper parryHelper;
		if (target.TryGetComponent<ParryHelper>(out parryHelper))
		{
			target = parryHelper.target;
		}
		Projectile projectile;
		if (target.TryGetComponent<Projectile>(out projectile) && !projectile.unparryable && !projectile.undeflectable && ((!this.alreadyBoostedProjectile && canProjectileBoost) || !projectile.playerBullet))
		{
			this.ParryProjectile(projectile);
			this.hitSomething = true;
			this.parriedSomething = true;
			return true;
		}
		Cannonball cannonball;
		if (target.TryGetComponent<Cannonball>(out cannonball) && cannonball.launchable)
		{
			this.anim.Play("Hook", 0, 0.065f);
			if (!cannonball.parry)
			{
				MonoSingleton<TimeController>.Instance.ParryFlash();
			}
			else
			{
				this.Parry(false, null, "");
			}
			Vector3 parryLookTarget = Punch.GetParryLookTarget();
			if (Vector3.Distance(cannonball.transform.position, parryLookTarget) < 10f)
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(MonoSingleton<CameraController>.Instance.GetDefaultPos(), MonoSingleton<CameraController>.Instance.transform.forward, out raycastHit, 5f, LayerMaskDefaults.Get(LMD.EnemiesAndEnvironment)))
				{
					cannonball.transform.position = raycastHit.point;
				}
				else
				{
					cannonball.transform.position = MonoSingleton<CameraController>.Instance.GetDefaultPos() + MonoSingleton<CameraController>.Instance.transform.forward * 5f;
				}
				cannonball.transform.forward = MonoSingleton<CameraController>.Instance.transform.forward;
			}
			else
			{
				cannonball.transform.LookAt(parryLookTarget);
			}
			cannonball.Launch();
			this.hitSomething = true;
			this.parriedSomething = true;
			return true;
		}
		ParryReceiver parryReceiver;
		if (target.TryGetComponent<ParryReceiver>(out parryReceiver))
		{
			if (!parryReceiver.enabled)
			{
				return false;
			}
			this.anim.Play("Hook", 0, 0.065f);
			if (parryReceiver.parryHeal)
			{
				this.Parry(false, null, "");
			}
			else
			{
				MonoSingleton<TimeController>.Instance.ParryFlash();
			}
			parryReceiver.Parry();
			this.hitSomething = true;
			this.parriedSomething = true;
			return true;
		}
		else
		{
			ThrownSword thrownSword;
			if (target.TryGetComponent<ThrownSword>(out thrownSword) && !thrownSword.friendly && thrownSword.active)
			{
				thrownSword.GetParried();
				this.anim.Play("Hook", -1, 0.065f);
				this.Parry(false, thrownSword.returnTransform.GetComponentInParent<EnemyIdentifier>(), "");
				this.hitSomething = true;
				this.parriedSomething = true;
				return true;
			}
			MassSpear massSpear;
			if (target.TryGetComponent<MassSpear>(out massSpear))
			{
				if (!massSpear.beenStopped || massSpear.hittingPlayer)
				{
					massSpear.Deflected();
					this.anim.Play("Hook", -1, 0.065f);
					this.Parry(false, null, "");
					this.hitSomething = true;
					this.parriedSomething = true;
				}
				else
				{
					if (!massSpear.hitPlayer || this.hitSomething)
					{
						return false;
					}
					Object.Instantiate<AudioSource>(this.specialHit, base.transform.position, Quaternion.identity);
					MonoSingleton<TimeController>.Instance.HitStop(0.1f);
					this.cc.CameraShake(0.5f * this.screenShakeMultiplier);
					massSpear.GetHurt(5f);
					this.hitSomething = true;
				}
				return true;
			}
			Landmine landmine;
			if (target.TryGetComponent<Landmine>(out landmine))
			{
				this.anim.Play("Hook", 0, 0.065f);
				this.Parry(false, null, "");
				landmine.transform.LookAt(Punch.GetParryLookTarget());
				landmine.Parry();
				this.hitSomething = true;
				this.parriedSomething = true;
				return true;
			}
			Chainsaw chainsaw;
			if (target.TryGetComponent<Chainsaw>(out chainsaw))
			{
				this.anim.Play("Hook", 0, 0.065f);
				chainsaw.beingPunched = true;
				MonoSingleton<WeaponCharges>.Instance.punchStamina = 2f;
				if (!this.punchedChainsaws.Contains(chainsaw))
				{
					this.punchedChainsaws.Add(chainsaw);
				}
				if (this.punchChainsawsRoutine == null)
				{
					this.punchChainsawsRoutine = base.StartCoroutine(this.ChainsawPunchRoutine());
				}
			}
			return false;
		}
	}

	// Token: 0x06001430 RID: 5168 RVA: 0x000A3CF1 File Offset: 0x000A1EF1
	private IEnumerator ChainsawPunchRoutine()
	{
		while (this.punchedChainsaws.Count > 0)
		{
			this.punchedChainsaws.RemoveAll((Chainsaw x) => x == null);
			if (this.punchedChainsaws.Count == 0)
			{
				break;
			}
			Chainsaw chainsaw = this.punchedChainsaws[this.punchedChainsaws.Count - 1];
			chainsaw.GetPunched();
			if (chainsaw.stopped)
			{
				chainsaw.stopped = false;
			}
			chainsaw.transform.position = MonoSingleton<CameraController>.Instance.GetDefaultPos() + MonoSingleton<CameraController>.Instance.transform.forward;
			chainsaw.rb.velocity = (Punch.GetParryLookTarget() - chainsaw.transform.position).normalized * 105f;
			Object.Instantiate<AudioSource>(this.specialHit, base.transform.position, Quaternion.identity);
			MonoSingleton<TimeController>.Instance.HitStop(0.1f);
			this.parriedSomething = true;
			this.hitSomething = true;
			this.punchedChainsaws.RemoveAt(this.punchedChainsaws.Count - 1);
			yield return new WaitForSeconds(0.05f);
		}
		this.punchChainsawsRoutine = null;
		yield break;
	}

	// Token: 0x06001431 RID: 5169 RVA: 0x000A3D00 File Offset: 0x000A1F00
	public void CoinFlip()
	{
		if (this.ready && MonoSingleton<FistControl>.Instance.forceNoHold <= 0)
		{
			this.anim.SetTrigger("CoinFlip");
		}
	}

	// Token: 0x06001432 RID: 5170 RVA: 0x000A3D27 File Offset: 0x000A1F27
	private void ActiveEnd()
	{
		this.tr.widthMultiplier = 0f;
		this.ignoreDoublePunch = false;
		if (this.type == FistType.Standard)
		{
			this.ResetFistRotation();
		}
	}

	// Token: 0x06001433 RID: 5171 RVA: 0x000A3D4E File Offset: 0x000A1F4E
	public void ResetFistRotation()
	{
		this.returnToOrigRot = true;
	}

	// Token: 0x06001434 RID: 5172 RVA: 0x00004AE3 File Offset: 0x00002CE3
	private void PunchEnd()
	{
	}

	// Token: 0x06001435 RID: 5173 RVA: 0x000A3D57 File Offset: 0x000A1F57
	private void ReadyToPunch()
	{
		this.returnToOrigRot = true;
		this.holdingInput = false;
		this.ready = true;
		this.alreadyBoostedProjectile = false;
		this.ignoreDoublePunch = false;
	}

	// Token: 0x06001436 RID: 5174 RVA: 0x000A3D7C File Offset: 0x000A1F7C
	private void PunchSuccess(Vector3 point, Transform target)
	{
		base.transform.parent.LookAt(point);
		if (Quaternion.Angle(base.transform.parent.localRotation, Quaternion.identity) > 45f)
		{
			Quaternion localRotation = base.transform.parent.localRotation;
			float num = localRotation.eulerAngles.x;
			float num2 = localRotation.eulerAngles.y;
			float num3 = localRotation.eulerAngles.z;
			if (num > 180f)
			{
				num -= 360f;
			}
			if (num2 > 180f)
			{
				num2 -= 360f;
			}
			if (num3 > 180f)
			{
				num3 -= 360f;
			}
			localRotation.eulerAngles = new Vector3(Mathf.Clamp(num, -45f, 45f), Mathf.Clamp(num2, -45f, 45f), Mathf.Clamp(num3, -45f, 45f));
			base.transform.parent.localRotation = localRotation;
		}
		ParryHelper parryHelper;
		if (target.TryGetComponent<ParryHelper>(out parryHelper))
		{
			target = parryHelper.target;
		}
		EnemyIdentifier enemyIdentifier2;
		if (target.gameObject.CompareTag("Enemy") || target.gameObject.CompareTag("Armor") || target.gameObject.CompareTag("Head") || target.gameObject.CompareTag("Body") || target.gameObject.CompareTag("Limb") || target.gameObject.CompareTag("EndLimb"))
		{
			if (this.anim.GetFloat("PunchRandomizer") < 0.5f)
			{
				this.anim.Play("Jab", 0, 0.075f);
			}
			else
			{
				this.anim.Play("Jab2", 0, 0.075f);
			}
			Object.Instantiate<AudioSource>(this.heavyHit, base.transform.position, Quaternion.identity);
			MonoSingleton<TimeController>.Instance.HitStop(0.1f);
			this.cc.CameraShake(0.5f * this.screenShakeMultiplier);
			EnemyIdentifier enemyIdentifier = null;
			EnemyIdentifierIdentifier enemyIdentifierIdentifier;
			if (target.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
			{
				enemyIdentifier = enemyIdentifierIdentifier.eid;
			}
			if (enemyIdentifier)
			{
				if (enemyIdentifier.drillers.Count > 0 && this.type != FistType.Heavy)
				{
					this.anim.Play("Hook", 0, 0.065f);
					MonoSingleton<TimeController>.Instance.ParryFlash();
					Harpoon harpoon = enemyIdentifier.drillers[enemyIdentifier.drillers.Count - 1];
					harpoon.transform.forward = this.cc.transform.forward;
					harpoon.transform.position = this.cc.GetDefaultPos();
					harpoon.Punched();
				}
				enemyIdentifier.hitter = this.hitter;
				enemyIdentifier.DeliverDamage(target.gameObject, this.camObj.transform.forward * this.force * 1000f, point, this.damage, this.tryForExplode, 0f, null, false, false);
			}
			if (this.holding)
			{
				this.heldItem.SendMessage("HitWith", target.gameObject, SendMessageOptions.DontRequireReceiver);
				return;
			}
		}
		else if (target.TryGetComponent<EnemyIdentifier>(out enemyIdentifier2) && enemyIdentifier2.enemyType == EnemyType.Idol)
		{
			enemyIdentifier2.hitter = this.hitter;
			enemyIdentifier2.DeliverDamage(target.gameObject, this.camObj.transform.forward * this.force * 1000f, point, this.damage, this.tryForExplode, 0f, null, false, false);
		}
	}

	// Token: 0x06001437 RID: 5175 RVA: 0x000A410C File Offset: 0x000A230C
	public void Parry(bool hook = false, EnemyIdentifier eid = null, string customParryText = "")
	{
		this.parriedSomething = true;
		this.hitSomething = true;
		this.activeFrames = 0;
		if (hook)
		{
			this.anim.Play("Hook", 0, 0.065f);
		}
		this.aud.pitch = Random.Range(0.7f, 0.8f);
		MonoSingleton<NewMovement>.Instance.Parry(eid, customParryText);
	}

	// Token: 0x06001438 RID: 5176 RVA: 0x000A4170 File Offset: 0x000A2370
	private void ParryProjectile(Projectile proj)
	{
		proj.hittingPlayer = false;
		proj.friendly = true;
		proj.parried = true;
		proj.speed *= 2f;
		proj.homingType = HomingType.None;
		proj.explosionEffect = this.parriedProjectileHitObject;
		proj.precheckForCollisions = true;
		if (proj.connectedBeams.Count > 0)
		{
			foreach (ContinuousBeam continuousBeam in proj.connectedBeams)
			{
				if (continuousBeam)
				{
					continuousBeam.SetPlayerCooldown(0.3f);
					if (continuousBeam.enemy)
					{
						continuousBeam.parryMultiplier = 2.5f;
					}
					continuousBeam.enemy = false;
				}
			}
		}
		Rigidbody component = proj.GetComponent<Rigidbody>();
		if (proj.playerBullet)
		{
			this.alreadyBoostedProjectile = true;
			proj.boosted = true;
			proj.GetComponent<SphereCollider>().radius *= 4f;
			proj.damage = 0f;
			if (component)
			{
				component.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			}
			Color color = new Color(1f, 0.35f, 0f);
			MeshRenderer meshRenderer;
			if (proj.TryGetComponent<MeshRenderer>(out meshRenderer) && meshRenderer.material && meshRenderer.material.HasProperty("_Color"))
			{
				meshRenderer.material.SetColor("_Color", color);
			}
			TrailRenderer trailRenderer;
			if (proj.TryGetComponent<TrailRenderer>(out trailRenderer))
			{
				Gradient gradient = new Gradient();
				gradient.SetKeys(new GradientColorKey[]
				{
					new GradientColorKey(color, 0f),
					new GradientColorKey(color, 1f)
				}, new GradientAlphaKey[]
				{
					new GradientAlphaKey(1f, 0f),
					new GradientAlphaKey(0f, 1f)
				});
				trailRenderer.colorGradient = gradient;
			}
			Light light;
			if (proj.TryGetComponent<Light>(out light))
			{
				light.color = color;
			}
		}
		if (component)
		{
			component.constraints = RigidbodyConstraints.FreezeRotation;
		}
		this.anim.Play("Hook", 0, 0.065f);
		if (!proj.playerBullet)
		{
			this.Parry(false, null, "");
		}
		else
		{
			MonoSingleton<TimeController>.Instance.ParryFlash();
		}
		if (proj.explosive)
		{
			proj.explosive = false;
		}
		Rigidbody component2 = proj.GetComponent<Rigidbody>();
		if (component2 && component2.useGravity)
		{
			component2.useGravity = false;
		}
		Vector3 parryLookTarget = Punch.GetParryLookTarget();
		proj.transform.LookAt(parryLookTarget);
		if (proj.speed == 0f)
		{
			component2.velocity = (parryLookTarget - base.transform.position).normalized * 250f;
		}
		else if (proj.speed < 100f)
		{
			proj.speed = 100f;
		}
		if (proj.spreaded)
		{
			ProjectileSpread componentInParent = proj.GetComponentInParent<ProjectileSpread>();
			if (componentInParent != null)
			{
				componentInParent.ParriedProjectile();
			}
		}
		proj.transform.SetParent(null, true);
	}

	// Token: 0x06001439 RID: 5177 RVA: 0x000A447C File Offset: 0x000A267C
	public void BlastCheck()
	{
		if (this.heldAction.IsPressed())
		{
			this.holdingInput = false;
			this.anim.SetTrigger("PunchBlast");
			Vector3 vector = MonoSingleton<CameraController>.Instance.GetDefaultPos() + MonoSingleton<CameraController>.Instance.transform.forward * 2f;
			RaycastHit raycastHit;
			if (Physics.Raycast(MonoSingleton<CameraController>.Instance.GetDefaultPos(), MonoSingleton<CameraController>.Instance.transform.forward, out raycastHit, 2f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies)))
			{
				vector = raycastHit.point - this.camObj.transform.forward * 0.1f;
			}
			Object.Instantiate<GameObject>(this.blastWave, vector, MonoSingleton<CameraController>.Instance.transform.rotation);
		}
	}

	// Token: 0x0600143A RID: 5178 RVA: 0x000A4550 File Offset: 0x000A2750
	public void Eject()
	{
		if (this.ejectorAud == null)
		{
			this.ejectorAud = this.shellEjector.GetComponent<AudioSource>();
		}
		this.ejectorAud.Play();
		for (int i = 0; i < 2; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.shell, this.shellEjector.position + this.shellEjector.right * 0.075f, this.shellEjector.rotation);
			if (i == 1)
			{
				gameObject.transform.position = gameObject.transform.position - this.shellEjector.right * 0.15f;
			}
			gameObject.transform.Rotate(Vector3.forward, (float)Random.Range(-45, 45), Space.Self);
			gameObject.GetComponent<Rigidbody>().AddForce((this.shellEjector.forward / 1.75f + this.shellEjector.up / 2f + Vector3.up / 1.75f) * (float)Random.Range(8, 12), ForceMode.VelocityChange);
		}
	}

	// Token: 0x0600143B RID: 5179 RVA: 0x00004AE3 File Offset: 0x00002CE3
	public void Hide()
	{
	}

	// Token: 0x0600143C RID: 5180 RVA: 0x000A4681 File Offset: 0x000A2881
	public void ShopMode()
	{
		this.shopping = true;
		this.holdingInput = false;
		this.shopRequests++;
	}

	// Token: 0x0600143D RID: 5181 RVA: 0x000A469F File Offset: 0x000A289F
	public void StopShop()
	{
		this.shopRequests--;
		if (this.shopRequests <= 0)
		{
			this.shopping = false;
		}
	}

	// Token: 0x0600143E RID: 5182 RVA: 0x000A46BF File Offset: 0x000A28BF
	public void EquipAnimation()
	{
		if (this.anim == null)
		{
			this.anim = base.GetComponent<Animator>();
		}
		this.anim.SetTrigger("Equip");
	}

	// Token: 0x0600143F RID: 5183 RVA: 0x000A46EC File Offset: 0x000A28EC
	private void AltHit(Transform target)
	{
		if (target.gameObject.layer == 22)
		{
			ItemIdentifier itemIdentifier = target.GetComponent<ItemIdentifier>();
			if (itemIdentifier && this.hasHeldItem)
			{
				return;
			}
			ItemPlaceZone[] components = target.GetComponents<ItemPlaceZone>();
			if (itemIdentifier && itemIdentifier.infiniteSource)
			{
				itemIdentifier = itemIdentifier.CreateCopy();
			}
			if (this.holding && components != null && components.Length != 0)
			{
				this.PlaceHeldObject(components, target);
				this.hitSomething = true;
			}
			else if (!this.holding && itemIdentifier != null)
			{
				this.ForceHold(itemIdentifier);
				this.hitSomething = true;
			}
		}
		if (this.holding)
		{
			this.heldItem.SendMessage("HitWith", target.gameObject, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06001440 RID: 5184 RVA: 0x000A479C File Offset: 0x000A299C
	public void CancelAttack()
	{
		this.anim.Rebind();
		this.anim.Update(0f);
		this.ActiveEnd();
		this.ReadyToPunch();
	}

	// Token: 0x06001441 RID: 5185 RVA: 0x000A47C8 File Offset: 0x000A29C8
	public static Vector3 GetParryLookTarget()
	{
		Vector3 vector = MonoSingleton<CameraController>.Instance.transform.forward;
		if (MonoSingleton<CameraFrustumTargeter>.Instance && MonoSingleton<CameraFrustumTargeter>.Instance.CurrentTarget && MonoSingleton<CameraFrustumTargeter>.Instance.IsAutoAimed)
		{
			vector = MonoSingleton<CameraFrustumTargeter>.Instance.CurrentTarget.bounds.center - MonoSingleton<CameraController>.Instance.transform.position;
		}
		RaycastHit raycastHit;
		if (Physics.Raycast(MonoSingleton<CameraController>.Instance.GetDefaultPos(), vector, out raycastHit, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Enemies), QueryTriggerInteraction.Ignore))
		{
			return raycastHit.point;
		}
		return MonoSingleton<CameraController>.Instance.GetDefaultPos() + vector * 1000f;
	}

	// Token: 0x04001BA7 RID: 7079
	private InputManager inman;

	// Token: 0x04001BA8 RID: 7080
	public FistType type;

	// Token: 0x04001BA9 RID: 7081
	private string hitter;

	// Token: 0x04001BAA RID: 7082
	private float damage;

	// Token: 0x04001BAB RID: 7083
	private float screenShakeMultiplier;

	// Token: 0x04001BAC RID: 7084
	private float force;

	// Token: 0x04001BAD RID: 7085
	private bool tryForExplode;

	// Token: 0x04001BAE RID: 7086
	private float cooldownCost;

	// Token: 0x04001BAF RID: 7087
	public bool ready = true;

	// Token: 0x04001BB0 RID: 7088
	[HideInInspector]
	public Animator anim;

	// Token: 0x04001BB1 RID: 7089
	private SkinnedMeshRenderer smr;

	// Token: 0x04001BB2 RID: 7090
	private Revolver rev;

	// Token: 0x04001BB3 RID: 7091
	[SerializeField]
	private AudioSource aud;

	// Token: 0x04001BB4 RID: 7092
	private GameObject camObj;

	// Token: 0x04001BB5 RID: 7093
	private CameraController cc;

	// Token: 0x04001BB6 RID: 7094
	private RaycastHit hit;

	// Token: 0x04001BB7 RID: 7095
	private LayerMask environmentMask;

	// Token: 0x04001BB8 RID: 7096
	private NewMovement nmov;

	// Token: 0x04001BB9 RID: 7097
	private TrailRenderer tr;

	// Token: 0x04001BBA RID: 7098
	private Light parryLight;

	// Token: 0x04001BBB RID: 7099
	private GameObject currentDustParticle;

	// Token: 0x04001BBC RID: 7100
	public GameObject dustParticle;

	// Token: 0x04001BBD RID: 7101
	public AudioSource normalHit;

	// Token: 0x04001BBE RID: 7102
	public AudioSource heavyHit;

	// Token: 0x04001BBF RID: 7103
	public AudioSource specialHit;

	// Token: 0x04001BC0 RID: 7104
	private StyleHUD shud;

	// Token: 0x04001BC1 RID: 7105
	private StatsManager sman;

	// Token: 0x04001BC2 RID: 7106
	public bool holding;

	// Token: 0x04001BC3 RID: 7107
	public Transform holder;

	// Token: 0x04001BC4 RID: 7108
	public ItemIdentifier heldItem;

	// Token: 0x04001BC5 RID: 7109
	private bool hasHeldItem;

	// Token: 0x04001BC6 RID: 7110
	private FistControl fc;

	// Token: 0x04001BC7 RID: 7111
	private bool shopping;

	// Token: 0x04001BC8 RID: 7112
	private int shopRequests;

	// Token: 0x04001BC9 RID: 7113
	public GameObject parriedProjectileHitObject;

	// Token: 0x04001BCA RID: 7114
	private ProjectileParryZone ppz;

	// Token: 0x04001BCB RID: 7115
	private bool returnToOrigRot;

	// Token: 0x04001BCC RID: 7116
	public GameObject blastWave;

	// Token: 0x04001BCD RID: 7117
	private bool holdingInput;

	// Token: 0x04001BCE RID: 7118
	public GameObject shell;

	// Token: 0x04001BCF RID: 7119
	public Transform shellEjector;

	// Token: 0x04001BD0 RID: 7120
	private AudioSource ejectorAud;

	// Token: 0x04001BD1 RID: 7121
	private bool alreadyBoostedProjectile;

	// Token: 0x04001BD2 RID: 7122
	private bool ignoreDoublePunch;

	// Token: 0x04001BD3 RID: 7123
	public bool hitSomething;

	// Token: 0x04001BD4 RID: 7124
	public bool parriedSomething;

	// Token: 0x04001BD5 RID: 7125
	public bool alreadyHitCoin;

	// Token: 0x04001BD6 RID: 7126
	public int activeFrames;

	// Token: 0x04001BD7 RID: 7127
	public InputAction heldAction;

	// Token: 0x04001BD8 RID: 7128
	private List<Chainsaw> punchedChainsaws = new List<Chainsaw>();

	// Token: 0x04001BD9 RID: 7129
	private Coroutine punchChainsawsRoutine;
}
