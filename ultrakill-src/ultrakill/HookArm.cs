using System;
using System.Collections.Generic;
using Sandbox;
using UnityEngine;

// Token: 0x0200024C RID: 588
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class HookArm : MonoSingleton<HookArm>
{
	// Token: 0x06000CDE RID: 3294 RVA: 0x0005FBC4 File Offset: 0x0005DDC4
	private void Start()
	{
		this.targeter = MonoSingleton<CameraFrustumTargeter>.Instance;
		this.lr = base.GetComponent<LineRenderer>();
		this.lr.enabled = false;
		this.anim = base.GetComponent<Animator>();
		this.playerCollider = MonoSingleton<NewMovement>.Instance.GetComponent<CapsuleCollider>();
		this.aud = base.GetComponent<AudioSource>();
		this.throwMask |= 1024;
		this.throwMask |= 2048;
		this.throwMask |= 4096;
		this.throwMask |= 16384;
		this.throwMask |= 65536;
		this.throwMask |= 4194304;
		this.throwMask |= 67108864;
		this.enviroMask |= 64;
		this.enviroMask |= 128;
		this.enviroMask |= 256;
		this.enviroMask |= 65536;
		this.enviroMask |= 262144;
		this.enviroMask |= 16777216;
		this.enemyMask |= 2048;
		this.enemyMask |= 67108864;
		this.enemyMask |= 4096;
		this.deadIgnoreTypes.Add(EnemyType.Drone);
		this.deadIgnoreTypes.Add(EnemyType.MaliciousFace);
		this.deadIgnoreTypes.Add(EnemyType.Mindflayer);
		this.deadIgnoreTypes.Add(EnemyType.Gutterman);
		this.deadIgnoreTypes.Add(EnemyType.Virtue);
		this.deadIgnoreTypes.Add(EnemyType.HideousMass);
		this.lightEnemies.Add(EnemyType.Drone);
		this.lightEnemies.Add(EnemyType.Filth);
		this.lightEnemies.Add(EnemyType.Schism);
		this.lightEnemies.Add(EnemyType.Soldier);
		this.lightEnemies.Add(EnemyType.Stray);
		this.lightEnemies.Add(EnemyType.Streetcleaner);
		this.model.SetActive(false);
	}

	// Token: 0x06000CDF RID: 3295 RVA: 0x0005FE7A File Offset: 0x0005E07A
	public void Inspect()
	{
		this.model.SetActive(true);
		this.inspectLr.enabled = true;
		this.anim.Play("Inspect", -1, 0f);
	}

	// Token: 0x06000CE0 RID: 3296 RVA: 0x0005FEAC File Offset: 0x0005E0AC
	private void Update()
	{
		if (!MonoSingleton<OptionsManager>.Instance || MonoSingleton<OptionsManager>.Instance.paused)
		{
			return;
		}
		if (!this.equipped || MonoSingleton<FistControl>.Instance.shopping || !MonoSingleton<FistControl>.Instance.activated)
		{
			if (this.state != HookState.Ready || this.returning)
			{
				this.Cancel();
			}
			this.model.SetActive(false);
			return;
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Hook.WasPerformedThisFrame)
		{
			if (this.state == HookState.Pulling)
			{
				this.StopThrow(0f, false);
			}
			else if (this.cooldown <= 0f)
			{
				this.cooldown = 0.5f;
				this.model.SetActive(true);
				if (!this.forcingFistControl)
				{
					if (MonoSingleton<FistControl>.Instance.currentPunch)
					{
						MonoSingleton<FistControl>.Instance.currentPunch.CancelAttack();
					}
					MonoSingleton<FistControl>.Instance.forceNoHold++;
					this.forcingFistControl = true;
					MonoSingleton<FistControl>.Instance.transform.localRotation = Quaternion.identity;
					if (MonoSingleton<FistControl>.Instance.fistCooldown > 0.1f)
					{
						MonoSingleton<FistControl>.Instance.fistCooldown = 0.1f;
					}
				}
				this.lr.enabled = true;
				this.hookPoint = base.transform.position;
				this.previousHookPoint = this.hookPoint;
				if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
				{
					this.throwDirection = (this.targeter.CurrentTarget.bounds.center - base.transform.position).normalized;
				}
				else
				{
					this.throwDirection = base.transform.forward;
				}
				this.returning = false;
				if (this.caughtObjects.Count > 0)
				{
					foreach (Rigidbody rigidbody in this.caughtObjects)
					{
						if (rigidbody)
						{
							rigidbody.velocity = (MonoSingleton<NewMovement>.Instance.transform.position - rigidbody.transform.position).normalized * (100f + this.returnDistance / 2f);
						}
					}
					this.caughtObjects.Clear();
				}
				this.state = HookState.Throwing;
				this.lightTarget = false;
				this.throwWarp = 1f;
				this.anim.Play("Throw", -1, 0f);
				this.inspectLr.enabled = false;
				this.hand.transform.localPosition = new Vector3(0.09f, -0.051f, 0.045f);
				if (MonoSingleton<CameraController>.Instance.defaultFov > 105f)
				{
					this.hand.transform.localPosition += new Vector3(0.225f * ((MonoSingleton<CameraController>.Instance.defaultFov - 105f) / 55f), -0.25f * ((MonoSingleton<CameraController>.Instance.defaultFov - 105f) / 55f), 0.05f * ((MonoSingleton<CameraController>.Instance.defaultFov - 105f) / 55f));
				}
				this.caughtPoint = Vector3.zero;
				this.caughtTransform = null;
				this.caughtCollider = null;
				this.caughtEid = null;
				Object.Instantiate<GameObject>(this.throwSound);
				this.aud.clip = this.throwLoop;
				this.aud.panStereo = 0f;
				this.aud.Play();
				this.aud.pitch = Random.Range(0.9f, 1.1f);
				this.semiBlocked = 0f;
				MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.WhiplashThrow, this.model);
			}
		}
		if (this.cooldown != 0f)
		{
			this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.deltaTime);
		}
		if (this.lr.enabled)
		{
			this.throwWarp = Mathf.MoveTowards(this.throwWarp, 0f, Time.deltaTime * 6.5f);
			this.lr.SetPosition(0, this.hand.position);
			for (int i = 1; i < this.lr.positionCount - 1; i++)
			{
				float num = 3f;
				if (i % 2 == 0)
				{
					num = -3f;
				}
				this.lr.SetPosition(i, Vector3.Lerp(this.hand.position, this.hookPoint, (float)i / (float)this.lr.positionCount) + base.transform.up * num * this.throwWarp * (1f / (float)i));
			}
			this.lr.SetPosition(this.lr.positionCount - 1, this.hookPoint);
		}
		if (this.state == HookState.Pulling && !this.lightTarget && MonoSingleton<InputManager>.Instance.InputSource.Jump.WasPerformedThisFrame)
		{
			if (MonoSingleton<NewMovement>.Instance.rb.velocity.y < 1f)
			{
				MonoSingleton<NewMovement>.Instance.rb.velocity = new Vector3(MonoSingleton<NewMovement>.Instance.rb.velocity.x, 1f, MonoSingleton<NewMovement>.Instance.rb.velocity.z);
			}
			MonoSingleton<NewMovement>.Instance.rb.velocity = Vector3.ClampMagnitude(MonoSingleton<NewMovement>.Instance.rb.velocity, 30f);
			if (!MonoSingleton<NewMovement>.Instance.gc.touchingGround && !Physics.Raycast(MonoSingleton<NewMovement>.Instance.gc.transform.position, Vector3.down, 1.5f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies)))
			{
				MonoSingleton<NewMovement>.Instance.rb.AddForce(Vector3.up * 15f, ForceMode.VelocityChange);
			}
			else if (!MonoSingleton<NewMovement>.Instance.jumping)
			{
				MonoSingleton<NewMovement>.Instance.Jump();
			}
			this.StopThrow(1f, false);
		}
		if (MonoSingleton<FistControl>.Instance.currentPunch && MonoSingleton<FistControl>.Instance.currentPunch.holding && this.forcingFistControl)
		{
			MonoSingleton<FistControl>.Instance.currentPunch.heldItem.transform.position = this.hook.position + this.hook.up * 0.2f;
			if (this.state != HookState.Ready || this.returning)
			{
				MonoSingleton<FistControl>.Instance.heldObject.hooked = true;
				if (MonoSingleton<FistControl>.Instance.heldObject.gameObject.layer != 22)
				{
					Transform[] array = MonoSingleton<FistControl>.Instance.heldObject.GetComponentsInChildren<Transform>();
					for (int j = 0; j < array.Length; j++)
					{
						array[j].gameObject.layer = 22;
					}
					return;
				}
			}
			else
			{
				MonoSingleton<FistControl>.Instance.heldObject.hooked = false;
				if (MonoSingleton<FistControl>.Instance.heldObject.gameObject.layer != 13)
				{
					Transform[] array = MonoSingleton<FistControl>.Instance.heldObject.GetComponentsInChildren<Transform>();
					for (int j = 0; j < array.Length; j++)
					{
						array[j].gameObject.layer = 13;
					}
				}
			}
		}
	}

	// Token: 0x06000CE1 RID: 3297 RVA: 0x00060638 File Offset: 0x0005E838
	private void LateUpdate()
	{
		if (this.state != HookState.Ready || this.returning)
		{
			this.hook.position = this.hookPoint;
			this.hook.up = this.throwDirection;
			this.hookModel.layer = 2;
			return;
		}
		this.hookModel.layer = 13;
	}

	// Token: 0x06000CE2 RID: 3298 RVA: 0x00060694 File Offset: 0x0005E894
	private void FixedUpdate()
	{
		if (this.caughtGrenade && this.caughtGrenade.playerRiding)
		{
			if (this.caughtObjects.Contains(this.caughtGrenade.rb))
			{
				this.caughtGrenade.hooked = false;
				this.caughtObjects.Remove(this.caughtGrenade.rb);
			}
			else
			{
				this.caughtObjects.Clear();
			}
			this.caughtGrenade = null;
		}
		if (this.state == HookState.Ready && this.returning)
		{
			Vector3 vector = this.hookPoint;
			this.hookPoint = Vector3.MoveTowards(this.hookPoint, this.hand.position, Time.fixedDeltaTime * (100f + this.returnDistance / 2f));
			for (int i = this.caughtObjects.Count - 1; i >= 0; i--)
			{
				if (this.caughtObjects[i] != null)
				{
					this.caughtObjects[i].position = this.hookPoint;
				}
				else
				{
					this.caughtObjects.RemoveAt(i);
				}
			}
			if (this.hookPoint == this.hand.position)
			{
				this.lr.enabled = false;
				this.returning = false;
				this.anim.Play("Catch", -1, 0f);
				Object.Instantiate<GameObject>(this.catchSound);
				this.aud.Stop();
				if (this.caughtObjects.Count > 0)
				{
					for (int j = this.caughtObjects.Count - 1; j >= 0; j--)
					{
						if (this.caughtObjects[j] != null)
						{
							Grenade grenade;
							if (this.caughtObjects[j].TryGetComponent<Grenade>(out grenade))
							{
								grenade.transform.position = MonoSingleton<NewMovement>.Instance.transform.position;
								grenade.hooked = false;
								grenade.ignoreEnemyType.Clear();
								if (grenade.rocket && !MonoSingleton<NewMovement>.Instance.ridingRocket && Vector3.Angle(Vector3.down, vector - MonoSingleton<NewMovement>.Instance.transform.position) < 45f)
								{
									grenade.PlayerRideStart();
								}
								else
								{
									grenade.Explode(false, false, grenade.rocket && !MonoSingleton<NewMovement>.Instance.gc.onGround, 1f, false, null, true);
								}
							}
							else
							{
								this.caughtObjects[j].velocity = Vector3.zero;
							}
						}
					}
					this.caughtObjects.Clear();
				}
				this.caughtGrenade = null;
				this.caughtCannonball = null;
			}
		}
		if (this.state == HookState.Throwing)
		{
			if (!MonoSingleton<InputManager>.Instance.InputSource.Hook.IsPressed && (this.cooldown <= 0.1f || this.caughtObjects.Count > 0))
			{
				this.StopThrow(0f, false);
			}
			else
			{
				float num = 250f * Time.fixedDeltaTime;
				bool flag = false;
				RaycastHit raycastHit;
				if (Physics.Raycast(this.hookPoint, this.throwDirection, out raycastHit, num, this.enviroMask, QueryTriggerInteraction.Ignore))
				{
					flag = true;
					num = raycastHit.distance;
				}
				RaycastHit[] array = Physics.SphereCastAll(this.hookPoint, Mathf.Min(Vector3.Distance(base.transform.position, this.hookPoint) / 15f, 5f), this.throwDirection, num, this.throwMask, QueryTriggerInteraction.Collide);
				Array.Sort<RaycastHit>(array, (RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance));
				bool flag2 = false;
				int k = 0;
				while (k < array.Length)
				{
					RaycastHit raycastHit2 = array[k];
					bool flag3 = false;
					int layer = raycastHit2.transform.gameObject.layer;
					switch (layer)
					{
					case 10:
					{
						Coin coin;
						if (raycastHit2.transform.gameObject.CompareTag("Coin") && raycastHit2.transform.TryGetComponent<Coin>(out coin))
						{
							raycastHit2.transform.position = this.hookPoint + this.throwDirection.normalized * raycastHit2.distance;
							coin.Bounce();
							goto IL_0E59;
						}
						goto IL_0E59;
					}
					case 11:
					case 12:
						if (!Physics.Raycast(this.hookPoint, raycastHit2.collider.bounds.center - this.hookPoint, Vector3.Distance(this.hookPoint, raycastHit2.collider.bounds.center), this.enviroMask, QueryTriggerInteraction.Ignore))
						{
							this.caughtEid = raycastHit2.transform.GetComponentInParent<EnemyIdentifier>();
							RaycastHit raycastHit3;
							if (!this.caughtEid || (this.caughtEid.enemyType != EnemyType.MaliciousFace && this.caughtEid.enemyType != EnemyType.Gutterman && this.caughtEid.enemyType != EnemyType.HideousMass) || !this.caughtEid.dead || raycastHit2.collider.Raycast(new Ray(this.hookPoint, this.throwDirection), out raycastHit3, num))
							{
								EnemyIdentifierIdentifier enemyIdentifierIdentifier;
								if (this.caughtEid == null && raycastHit2.transform.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
								{
									this.caughtEid = enemyIdentifierIdentifier.eid;
								}
								if (this.caughtEid)
								{
									if (this.caughtEid.hookIgnore)
									{
										this.caughtEid = null;
										goto IL_0E59;
									}
									if (this.caughtCannonball && this.caughtCannonball.hitEnemies.Contains(this.caughtEid))
									{
										this.caughtEid = null;
										this.StopThrow(0f, false);
										return;
									}
									if (this.caughtEid.blessed)
									{
										this.caughtEid.hitter = "hook";
										this.caughtEid.DeliverDamage(this.caughtEid.gameObject, Vector3.zero, raycastHit2.point, 1f, false, 0f, null, false, false);
										this.caughtEid = null;
										break;
									}
									if (this.caughtEid.enemyType == EnemyType.Idol)
									{
										this.caughtEid.hitter = "hook";
										this.caughtEid.DeliverDamage(this.caughtEid.gameObject, Vector3.zero, raycastHit2.point, 1f, false, 0f, null, false, false);
										Object.Instantiate<GameObject>(this.clinkObjectSparks, raycastHit2.point, Quaternion.LookRotation(raycastHit2.normal));
										break;
									}
									this.caughtEid.hitter = "hook";
									this.caughtEid.hooked = true;
									if (((this.caughtEid.enemyType != EnemyType.Drone && this.caughtEid.enemyType != EnemyType.Virtue) || !this.caughtEid.dead) && this.caughtEid.enemyType != EnemyType.Stalker)
									{
										this.caughtEid.DeliverDamage(this.caughtEid.gameObject, Vector3.zero, raycastHit2.point, 0.2f, false, 0f, null, false, false);
									}
									if (this.caughtEid == null)
									{
										return;
									}
									if (MonoSingleton<FistControl>.Instance.heldObject)
									{
										GameObject gameObject = raycastHit2.transform.gameObject;
										if (raycastHit2.transform.gameObject.layer == 12)
										{
											EnemyIdentifierIdentifier componentInChildren = gameObject.GetComponentInChildren<EnemyIdentifierIdentifier>();
											if (componentInChildren)
											{
												gameObject = componentInChildren.gameObject;
											}
										}
										MonoSingleton<FistControl>.Instance.heldObject.SendMessage("HitWith", gameObject, SendMessageOptions.DontRequireReceiver);
										if (MonoSingleton<FistControl>.Instance.heldObject.dropOnHit)
										{
											MonoSingleton<FistControl>.Instance.currentPunch.ForceDrop();
										}
									}
									if (this.caughtEid.dead)
									{
										if (!this.deadIgnoreTypes.Contains(this.caughtEid.enemyType))
										{
											goto IL_0E59;
										}
										if (this.caughtEid.enemyType == EnemyType.Virtue || this.lightEnemies.Contains(this.caughtEid.enemyType))
										{
											this.lightTarget = true;
										}
										this.caughtEid = null;
									}
									else if (this.lightEnemies.Contains(this.caughtEid.enemyType))
									{
										this.lightTarget = true;
									}
								}
								flag2 = true;
								flag3 = true;
								this.caughtTransform = raycastHit2.transform;
								this.hookPoint = raycastHit2.collider.bounds.center;
								this.caughtPoint = this.hookPoint - this.caughtTransform.position;
								this.state = HookState.Caught;
								this.caughtCollider = raycastHit2.collider;
								this.aud.Stop();
								Object.Instantiate<GameObject>(this.hitSound, raycastHit2.point, Quaternion.identity);
								goto IL_0E59;
							}
							this.caughtEid = null;
						}
						break;
					case 13:
					case 15:
						goto IL_0E59;
					case 14:
						if (this.caughtObjects.Count >= 5 || !MonoSingleton<ObjectTracker>.Instance.HasTransform(raycastHit2.transform) || !(raycastHit2.collider.attachedRigidbody != null) || this.caughtObjects.Contains(raycastHit2.collider.attachedRigidbody))
						{
							goto IL_0E59;
						}
						if (this.caughtGrenade == null && MonoSingleton<ObjectTracker>.Instance.grenadeList.Count > 0 && MonoSingleton<ObjectTracker>.Instance.GetGrenade(raycastHit2.transform) && MonoSingleton<ObjectTracker>.Instance.GetGrenade(raycastHit2.transform).rocket && !MonoSingleton<ObjectTracker>.Instance.GetGrenade(raycastHit2.transform).playerRiding)
						{
							this.caughtObjects.Add(raycastHit2.collider.attachedRigidbody);
							Object.Instantiate<GameObject>(this.clinkObjectSparks, raycastHit2.point, Quaternion.LookRotation(raycastHit2.normal));
							this.caughtGrenade = MonoSingleton<ObjectTracker>.Instance.GetGrenade(raycastHit2.transform);
							this.caughtGrenade.rideable = true;
							this.caughtGrenade.hooked = true;
							this.caughtGrenade.ignoreEnemyType.Clear();
							goto IL_0E59;
						}
						if (MonoSingleton<ObjectTracker>.Instance.cannonballList.Count > 0 && MonoSingleton<ObjectTracker>.Instance.GetCannonball(raycastHit2.transform) && MonoSingleton<ObjectTracker>.Instance.GetCannonball(raycastHit2.transform).physicsCannonball)
						{
							Cannonball cannonball = MonoSingleton<ObjectTracker>.Instance.GetCannonball(raycastHit2.transform);
							this.caughtObjects.Add(raycastHit2.collider.attachedRigidbody);
							Object.Instantiate<GameObject>(this.clinkObjectSparks, raycastHit2.point, Quaternion.LookRotation(raycastHit2.normal));
							this.caughtCannonball = cannonball;
							cannonball.Unlaunch(true);
							cannonball.forceMaxSpeed = true;
							cannonball.InstaBreakDefenceCancel();
							goto IL_0E59;
						}
						goto IL_0E59;
					case 16:
					{
						BulletCheck bulletCheck;
						if (raycastHit2.collider.isTrigger && raycastHit2.transform.TryGetComponent<BulletCheck>(out bulletCheck))
						{
							bulletCheck.ForceDodge();
						}
						flag3 = true;
						goto IL_0E59;
					}
					default:
						if (layer != 22)
						{
							if (layer == 26 && !raycastHit2.collider.isTrigger)
							{
								this.StopThrow(0f, false);
								Object.Instantiate<GameObject>(this.clinkSparks, raycastHit2.point, Quaternion.LookRotation(raycastHit2.normal));
								flag2 = true;
								flag3 = true;
								goto IL_0E59;
							}
							goto IL_0E59;
						}
						else
						{
							HookPoint hookPoint;
							if (raycastHit2.transform.TryGetComponent<HookPoint>(out hookPoint))
							{
								if (hookPoint.active && Vector3.Distance(base.transform.position, raycastHit2.transform.position) > 5f)
								{
									flag2 = true;
									flag3 = true;
									this.caughtTransform = raycastHit2.transform;
									this.hookPoint = raycastHit2.transform.position;
									this.caughtPoint = Vector3.zero;
									this.state = HookState.Caught;
									this.caughtCollider = raycastHit2.collider;
									this.aud.Stop();
									this.caughtHook = hookPoint;
									hookPoint.Hooked();
									goto IL_0E59;
								}
							}
							else if (MonoSingleton<FistControl>.Instance.currentPunch && !MonoSingleton<FistControl>.Instance.currentPunch.holding)
							{
								ItemIdentifier itemIdentifier;
								if (raycastHit2.transform.TryGetComponent<ItemIdentifier>(out itemIdentifier))
								{
									if (Physics.Raycast(this.hookPoint, raycastHit2.transform.position - this.hookPoint, Vector3.Distance(this.hookPoint, raycastHit2.transform.position), this.enviroMask, QueryTriggerInteraction.Ignore))
									{
										break;
									}
									if (itemIdentifier.infiniteSource)
									{
										itemIdentifier = itemIdentifier.CreateCopy();
									}
									flag2 = true;
									if (itemIdentifier.ipz == null || (itemIdentifier.ipz.CheckDoorBounds(itemIdentifier.transform.position, this.previousHookPoint, false) && itemIdentifier.ipz.CheckDoorBounds(itemIdentifier.transform.position, base.transform.position, false)))
									{
										MonoSingleton<FistControl>.Instance.currentPunch.ForceHold(itemIdentifier);
									}
									else
									{
										this.ItemGrabError(raycastHit2);
									}
									this.previousHookPoint = this.hookPoint;
								}
							}
							else
							{
								ItemPlaceZone[] components = raycastHit2.transform.GetComponents<ItemPlaceZone>();
								bool flag4 = false;
								foreach (ItemPlaceZone itemPlaceZone in components)
								{
									if (itemPlaceZone.acceptedItemType == MonoSingleton<FistControl>.Instance.heldObject.itemType && !itemPlaceZone.CheckDoorBounds(itemPlaceZone.transform.position, this.previousHookPoint, true) && !itemPlaceZone.CheckDoorBounds(itemPlaceZone.transform.position, base.transform.position, true))
									{
										flag4 = true;
									}
								}
								if (components.Length != 0)
								{
									if (Physics.Raycast(this.hookPoint, raycastHit2.transform.position - this.hookPoint, Vector3.Distance(this.hookPoint, raycastHit2.transform.position), this.enviroMask, QueryTriggerInteraction.Ignore))
									{
										break;
									}
									flag2 = true;
									if (!flag4)
									{
										MonoSingleton<FistControl>.Instance.currentPunch.PlaceHeldObject(components, raycastHit2.transform);
									}
									else
									{
										this.ItemGrabError(raycastHit2);
									}
									this.previousHookPoint = this.hookPoint;
								}
							}
							if (flag2)
							{
								flag3 = true;
								this.StopThrow(0f, false);
								goto IL_0E59;
							}
							if (!Physics.Raycast(this.hookPoint, raycastHit2.transform.position - this.hookPoint, Vector3.Distance(this.hookPoint, raycastHit2.transform.position), this.enviroMask, QueryTriggerInteraction.Ignore))
							{
								flag3 = true;
								goto IL_0E59;
							}
							goto IL_0E59;
						}
						break;
					}
					IL_0E93:
					k++;
					continue;
					IL_0E59:
					if (flag3 && MonoSingleton<FistControl>.Instance.heldObject)
					{
						MonoSingleton<FistControl>.Instance.heldObject.SendMessage("HitWith", raycastHit2.transform.gameObject, SendMessageOptions.DontRequireReceiver);
					}
					if (!flag2)
					{
						goto IL_0E93;
					}
					break;
				}
				Vector3 point = this.hookPoint;
				if (flag && !flag2)
				{
					Breakable breakable;
					if (raycastHit.transform.TryGetComponent<Breakable>(out breakable) && breakable.weak && !breakable.precisionOnly && !breakable.specialCaseOnly)
					{
						breakable.Break();
					}
					SandboxProp sandboxProp;
					if (raycastHit.transform.gameObject.TryGetComponent<SandboxProp>(out sandboxProp) && sandboxProp.rigidbody)
					{
						sandboxProp.rigidbody.AddForceAtPosition(base.transform.forward * -100f, raycastHit.point, ForceMode.VelocityChange);
					}
					else
					{
						Object.Instantiate<GameObject>(this.clinkSparks, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
					}
					point = raycastHit.point;
					this.StopThrow(0f, false);
					flag2 = true;
				}
				if (!flag2 && Vector3.Distance(base.transform.position, this.hookPoint) > 300f)
				{
					this.StopThrow(0f, false);
				}
				else if (!flag2)
				{
					this.hookPoint += this.throwDirection * num;
					point = this.hookPoint;
				}
				for (int m = this.caughtObjects.Count - 1; m >= 0; m--)
				{
					if (this.caughtObjects[m] != null)
					{
						this.caughtObjects[m].position = point;
						if (flag2)
						{
							Grenade grenade2;
							if (this.caughtObjects[m].TryGetComponent<Grenade>(out grenade2))
							{
								if (this.caughtEid && grenade2.originEnemy && this.caughtEid == grenade2.originEnemy)
								{
									MonoSingleton<StyleHUD>.Instance.AddPoints(100, "ultrakill.rocketreturn", null, this.caughtEid, -1, "", "");
								}
								grenade2.hooked = false;
								grenade2.ignoreEnemyType.Clear();
								grenade2.Explode(false, false, false, 1f, false, null, false);
							}
							else
							{
								this.caughtObjects.RemoveAt(m);
							}
						}
					}
					else
					{
						this.caughtObjects.RemoveAt(m);
					}
				}
			}
		}
		else if (this.state == HookState.Caught)
		{
			if (this.caughtEid != null && (this.caughtEid.dead || this.caughtEid.hookIgnore || this.caughtEid.blessed))
			{
				if (!this.caughtEid.dead || !this.deadIgnoreTypes.Contains(this.caughtEid.enemyType))
				{
					this.StopThrow(0f, false);
					return;
				}
				this.SolveDeadIgnore();
			}
			else if (!this.caughtTransform || Physics.Raycast(this.hand.position, this.caughtTransform.position + this.caughtPoint - this.hand.position, Vector3.Distance(this.hand.position, this.caughtTransform.position + this.caughtPoint), this.enviroMask, QueryTriggerInteraction.Ignore))
			{
				this.StopThrow(0f, false);
				return;
			}
			this.hookPoint = this.caughtTransform.position + this.caughtPoint;
			if (!MonoSingleton<InputManager>.Instance.InputSource.Hook.IsPressed)
			{
				this.anim.Play("Pull", -1, 0f);
				this.hand.transform.localPosition = new Vector3(-0.015f, 0.071f, 0.04f);
				this.state = HookState.Pulling;
				MonoSingleton<RumbleManager>.Instance.SetVibrationTracked(RumbleProperties.WhiplashPull, this.model);
				this.currentWoosh = Object.Instantiate<GameObject>(this.wooshSound);
				Object.Instantiate<GameObject>(this.pullSound);
				this.aud.clip = this.pullLoop;
				this.aud.pitch = Random.Range(0.9f, 1.1f);
				this.aud.panStereo = -0.5f;
				this.aud.Play();
				if (this.caughtHook && this.caughtHook.type == hookPointType.Switch)
				{
					this.caughtHook.SwitchPulled();
					if (!MonoSingleton<NewMovement>.Instance.gc.touchingGround)
					{
						if (MonoSingleton<UnderwaterController>.Instance.inWater)
						{
							MonoSingleton<NewMovement>.Instance.rb.velocity = Vector3.zero;
						}
						else
						{
							MonoSingleton<NewMovement>.Instance.rb.velocity = new Vector3(0f, 15f, 0f);
						}
					}
					this.StopThrow(0f, false);
				}
				else if (!this.forcingGroundCheck && !this.lightTarget)
				{
					this.ForceGroundCheck();
				}
				else if (this.lightTarget)
				{
					Rigidbody rigidbody;
					if (this.caughtEid)
					{
						if (this.enemyGroundCheck != null)
						{
							this.enemyGroundCheck.StopForceOff();
						}
						this.enemyGroundCheck = this.caughtEid.gce;
						if (this.enemyGroundCheck)
						{
							this.enemyGroundCheck.ForceOff();
						}
						this.enemyRigidbody = this.caughtEid.GetComponent<Rigidbody>();
					}
					else if (this.caughtTransform.TryGetComponent<Rigidbody>(out rigidbody))
					{
						this.enemyRigidbody = rigidbody;
					}
					else
					{
						this.StopThrow(0f, false);
					}
					if (!MonoSingleton<NewMovement>.Instance.gc.touchingGround)
					{
						if (!MonoSingleton<UnderwaterController>.Instance.inWater)
						{
							MonoSingleton<NewMovement>.Instance.rb.velocity = new Vector3(0f, 15f, 0f);
						}
						else
						{
							MonoSingleton<NewMovement>.Instance.rb.velocity = new Vector3(0f, 5f, 0f);
						}
					}
				}
			}
		}
		if (this.state == HookState.Pulling)
		{
			if (!this.caughtTransform || !this.caughtCollider)
			{
				this.StopThrow(1f, false);
				return;
			}
			Vector3 vector2 = this.caughtTransform.position + this.caughtPoint - base.transform.position;
			RaycastHit raycastHit4;
			if (Physics.Raycast(base.transform.position, vector2.normalized, out raycastHit4, vector2.magnitude, this.enviroMask, QueryTriggerInteraction.Ignore))
			{
				bool flag5 = true;
				EnemyIdentifier component = raycastHit4.transform.GetComponent<EnemyIdentifier>();
				if (component && component.blessed)
				{
					flag5 = false;
				}
				if (flag5)
				{
					this.StopThrow(1f, false);
					return;
				}
			}
			if (this.caughtEid != null && (this.caughtEid.dead || this.caughtEid.hookIgnore || this.caughtEid.blessed))
			{
				if (!this.caughtEid.dead || !this.deadIgnoreTypes.Contains(this.caughtEid.enemyType))
				{
					this.StopThrow(1f, false);
					return;
				}
				this.SolveDeadIgnore();
			}
			if (this.caughtEid && !MonoSingleton<UnderwaterController>.Instance.inWater && (!MonoSingleton<AssistController>.Instance || !MonoSingleton<AssistController>.Instance.majorEnabled || !MonoSingleton<AssistController>.Instance.disableWhiplashHardDamage))
			{
				if (MonoSingleton<NewMovement>.Instance.antiHp + Time.fixedDeltaTime * 66f <= 50f)
				{
					MonoSingleton<NewMovement>.Instance.ForceAddAntiHP(Time.fixedDeltaTime * 66f, true, true, true, false);
				}
				else if (MonoSingleton<NewMovement>.Instance.antiHp <= 50f)
				{
					MonoSingleton<NewMovement>.Instance.ForceAntiHP(50f, true, true, true, false);
				}
			}
			Vector3 vector3 = this.playerCollider.ClosestPoint(this.hookPoint);
			Collider collider = this.caughtCollider;
			RaycastHit raycastHit5;
			if (Physics.Raycast(base.transform.position, this.caughtCollider.bounds.center - base.transform.position, out raycastHit5, Vector3.Distance(this.caughtCollider.bounds.center, base.transform.position), this.enemyMask))
			{
				collider = raycastHit5.collider;
			}
			if (Vector3.Distance(vector3, collider.ClosestPoint(vector3)) < 0.25f || (!this.lightTarget && Vector3.Distance(vector3 + MonoSingleton<NewMovement>.Instance.rb.velocity * Time.fixedDeltaTime, collider.ClosestPoint(vector3)) < 0.25f))
			{
				if (!this.lightTarget && Vector3.Distance(vector3, collider.ClosestPoint(vector3)) >= 0.25f)
				{
					MonoSingleton<NewMovement>.Instance.rb.MovePosition(collider.ClosestPoint(vector3) - MonoSingleton<NewMovement>.Instance.rb.velocity.normalized * 0.25f);
				}
				if (this.enemyRigidbody)
				{
					if (this.enemyGroundCheck == null || this.enemyGroundCheck.touchingGround || (this.caughtEid && this.caughtEid.underwater) || this.caughtEid.enemyType == EnemyType.Mannequin)
					{
						this.enemyRigidbody.velocity = Vector3.zero;
					}
					else
					{
						this.enemyRigidbody.velocity = new Vector3(0f, 15f, 0f);
					}
				}
				bool flag6 = false;
				if (this.caughtHook)
				{
					if (this.caughtHook.type == hookPointType.Slingshot)
					{
						flag6 = true;
						if (this.caughtHook.slingShotForce != 0f)
						{
							MonoSingleton<NewMovement>.Instance.rb.velocity = this.caughtHook.slingShotForce * MonoSingleton<NewMovement>.Instance.rb.velocity.normalized;
						}
					}
					this.caughtHook.Reached(MonoSingleton<NewMovement>.Instance.rb.velocity.normalized);
				}
				this.StopThrow(1f, false);
				if (!MonoSingleton<NewMovement>.Instance.gc.touchingGround && !flag6)
				{
					if (MonoSingleton<UnderwaterController>.Instance.inWater)
					{
						MonoSingleton<NewMovement>.Instance.rb.velocity = Vector3.zero;
						return;
					}
					if (base.transform.position.y < this.hookPoint.y)
					{
						MonoSingleton<NewMovement>.Instance.rb.velocity = new Vector3(0f, 15f + (this.hookPoint.y - base.transform.position.y) * 3f, 0f);
						return;
					}
					MonoSingleton<NewMovement>.Instance.rb.velocity = new Vector3(0f, 15f, 0f);
				}
				return;
			}
			if (this.caughtEid && this.enemyRigidbody && this.caughtEid.enemyType == EnemyType.Drone)
			{
				if (this.enemyRigidbody.isKinematic)
				{
					this.lightTarget = false;
				}
				else
				{
					this.lightTarget = true;
				}
			}
			if (this.lightTarget && this.forcingGroundCheck)
			{
				this.StopForceGroundCheck();
			}
			else if (!this.lightTarget && !this.forcingGroundCheck)
			{
				this.ForceGroundCheck();
			}
			if (this.lightTarget)
			{
				if (!this.enemyRigidbody)
				{
					this.StopThrow(1f, false);
					return;
				}
				this.hookPoint = this.caughtTransform.position + this.caughtPoint;
				if (this.enemyGroundCheck != null)
				{
					this.enemyRigidbody.velocity = (MonoSingleton<NewMovement>.Instance.transform.position - this.hookPoint).normalized * 60f;
					this.caughtEid.transform.LookAt(new Vector3(MonoSingleton<CameraController>.Instance.transform.position.x, this.caughtEid.transform.position.y, MonoSingleton<CameraController>.Instance.transform.position.z));
					return;
				}
				this.enemyRigidbody.velocity = (MonoSingleton<CameraController>.Instance.transform.position - this.hookPoint).normalized * 60f;
				if (this.caughtEid)
				{
					this.caughtEid.transform.LookAt(MonoSingleton<CameraController>.Instance.transform.position);
					return;
				}
				this.caughtTransform.LookAt(MonoSingleton<CameraController>.Instance.transform.position);
				return;
			}
			else
			{
				this.hookPoint = this.caughtTransform.position + this.caughtPoint;
				this.beingPulled = true;
				if (!MonoSingleton<NewMovement>.Instance.boost || MonoSingleton<NewMovement>.Instance.sliding)
				{
					MonoSingleton<NewMovement>.Instance.rb.velocity = (this.hookPoint - MonoSingleton<NewMovement>.Instance.transform.position).normalized * 60f;
					return;
				}
			}
		}
		else
		{
			this.beingPulled = false;
		}
	}

	// Token: 0x06000CE3 RID: 3299 RVA: 0x00062244 File Offset: 0x00060444
	private void SolveDeadIgnore()
	{
		if (!this.caughtEid)
		{
			return;
		}
		EnemyType enemyType = this.caughtEid.enemyType;
		if (enemyType != EnemyType.MaliciousFace)
		{
			if (enemyType == EnemyType.Virtue)
			{
				this.lightTarget = true;
				this.enemyRigidbody = this.caughtEid.GetComponent<Rigidbody>();
			}
		}
		else
		{
			EnemyIdentifierIdentifier[] componentsInChildren = this.caughtEid.GetComponentsInChildren<EnemyIdentifierIdentifier>();
			if (componentsInChildren.Length != 0)
			{
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					if (componentsInChildren[i].gameObject.layer == 11)
					{
						this.caughtTransform = componentsInChildren[i].transform;
						break;
					}
				}
			}
		}
		this.caughtEid = null;
	}

	// Token: 0x06000CE4 RID: 3300 RVA: 0x000622D4 File Offset: 0x000604D4
	private void ItemGrabError(RaycastHit rhit)
	{
		Object.Instantiate<GameObject>(this.errorSound);
		MonoSingleton<CameraController>.Instance.CameraShake(0.5f);
		MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage("<color=red>ERROR: BLOCKING DOOR WOULD CLOSE</color>", "", "", 0, true, false, true);
	}

	// Token: 0x06000CE5 RID: 3301 RVA: 0x00062310 File Offset: 0x00060510
	public void StopThrow(float animationTime = 0f, bool sparks = false)
	{
		MonoSingleton<RumbleManager>.Instance.StopVibration(RumbleProperties.WhiplashThrow);
		MonoSingleton<RumbleManager>.Instance.StopVibration(RumbleProperties.WhiplashPull);
		if (animationTime == 0f)
		{
			Object.Instantiate<GameObject>(this.pullSound);
			this.aud.clip = this.pullLoop;
			this.aud.pitch = Random.Range(0.9f, 1.1f);
			this.aud.panStereo = -0.5f;
			this.aud.Play();
		}
		else
		{
			Object.Instantiate<GameObject>(this.pullDoneSound);
		}
		if (this.forcingGroundCheck)
		{
			this.StopForceGroundCheck();
		}
		if (this.lightTarget)
		{
			if (this.enemyGroundCheck)
			{
				this.enemyGroundCheck.StopForceOff();
			}
			this.lightTarget = false;
			this.enemyGroundCheck = null;
			this.enemyRigidbody = null;
		}
		if (this.caughtEid)
		{
			this.caughtEid.hooked = false;
			this.caughtEid = null;
		}
		if (this.caughtHook)
		{
			this.caughtHook.Unhooked();
			this.caughtHook = null;
		}
		if (sparks)
		{
			Object.Instantiate<GameObject>(this.clinkSparks, this.hookPoint, Quaternion.LookRotation(base.transform.position - this.hookPoint));
		}
		this.state = HookState.Ready;
		this.anim.Play("Pull", -1, animationTime);
		this.hand.transform.localPosition = new Vector3(-0.015f, 0.071f, 0.04f);
		if (MonoSingleton<CameraController>.Instance.defaultFov > 105f)
		{
			this.hand.transform.localPosition += new Vector3(0.25f * ((MonoSingleton<CameraController>.Instance.defaultFov - 105f) / 55f), 0f, 0.05f * ((MonoSingleton<CameraController>.Instance.defaultFov - 105f) / 60f));
		}
		else if (MonoSingleton<CameraController>.Instance.defaultFov < 105f)
		{
			this.hand.transform.localPosition -= new Vector3(0.05f * ((105f - MonoSingleton<CameraController>.Instance.defaultFov) / 60f), 0.075f * ((105f - MonoSingleton<CameraController>.Instance.defaultFov) / 60f), 0.125f * ((105f - MonoSingleton<CameraController>.Instance.defaultFov) / 60f));
		}
		this.returnDistance = Mathf.Max(Vector3.Distance(base.transform.position, this.hookPoint), 25f);
		this.returning = true;
		this.throwWarp = 0f;
		if (this.currentWoosh)
		{
			Object.Destroy(this.currentWoosh);
		}
	}

	// Token: 0x06000CE6 RID: 3302 RVA: 0x000625DC File Offset: 0x000607DC
	public void Cancel()
	{
		MonoSingleton<RumbleManager>.Instance.StopVibration(RumbleProperties.WhiplashThrow);
		MonoSingleton<RumbleManager>.Instance.StopVibration(RumbleProperties.WhiplashPull);
		if (this.forcingGroundCheck)
		{
			this.StopForceGroundCheck();
		}
		if (this.forcingFistControl)
		{
			MonoSingleton<FistControl>.Instance.forceNoHold--;
			this.forcingFistControl = false;
			if (MonoSingleton<FistControl>.Instance.heldObject)
			{
				MonoSingleton<FistControl>.Instance.heldObject.gameObject.layer = 13;
				MonoSingleton<FistControl>.Instance.heldObject.hooked = false;
			}
		}
		if (this.caughtObjects.Count > 0)
		{
			foreach (Rigidbody rigidbody in this.caughtObjects)
			{
				if (rigidbody)
				{
					rigidbody.velocity = (MonoSingleton<NewMovement>.Instance.transform.position - rigidbody.transform.position).normalized * (100f + this.returnDistance / 2f);
					Cannonball cannonball;
					Grenade grenade;
					if (rigidbody.TryGetComponent<Cannonball>(out cannonball))
					{
						cannonball.hitEnemies.Clear();
						cannonball.forceMaxSpeed = false;
					}
					else if (rigidbody.TryGetComponent<Grenade>(out grenade))
					{
						grenade.hooked = false;
					}
				}
			}
			this.caughtObjects.Clear();
		}
		this.caughtGrenade = null;
		this.caughtCannonball = null;
		if (this.lightTarget)
		{
			if (this.enemyGroundCheck)
			{
				this.enemyGroundCheck.StopForceOff();
			}
			this.lightTarget = false;
			this.enemyGroundCheck = null;
			this.enemyRigidbody = null;
		}
		if (this.caughtEid)
		{
			this.caughtEid.hooked = false;
			this.caughtEid = null;
		}
		if (this.caughtHook)
		{
			this.caughtHook.Unhooked();
			this.caughtHook = null;
		}
		this.state = HookState.Ready;
		this.anim.Play("Idle", -1, 0f);
		this.returning = false;
		this.throwWarp = 0f;
		this.lr.enabled = false;
		this.hookPoint = this.hand.position;
		this.aud.Stop();
		if (MonoSingleton<FistControl>.Instance.currentPunch && MonoSingleton<FistControl>.Instance.currentPunch.holding)
		{
			MonoSingleton<FistControl>.Instance.ResetHeldItemPosition();
		}
		if (this.currentWoosh)
		{
			Object.Destroy(this.currentWoosh);
		}
		this.model.SetActive(false);
	}

	// Token: 0x06000CE7 RID: 3303 RVA: 0x00062874 File Offset: 0x00060A74
	public void CatchOver()
	{
		if (this.state != HookState.Ready || this.returning)
		{
			return;
		}
		if (this.forcingFistControl)
		{
			MonoSingleton<FistControl>.Instance.forceNoHold--;
			this.forcingFistControl = false;
			if (MonoSingleton<FistControl>.Instance.heldObject)
			{
				MonoSingleton<FistControl>.Instance.heldObject.hooked = false;
			}
		}
		if (MonoSingleton<FistControl>.Instance.currentPunch && MonoSingleton<FistControl>.Instance.currentPunch.holding)
		{
			MonoSingleton<FistControl>.Instance.ResetHeldItemPosition();
		}
		this.model.SetActive(false);
	}

	// Token: 0x06000CE8 RID: 3304 RVA: 0x0006290C File Offset: 0x00060B0C
	private void ForceGroundCheck()
	{
		if (MonoSingleton<NewMovement>.Instance.sliding)
		{
			MonoSingleton<NewMovement>.Instance.StopSlide();
		}
		if (MonoSingleton<NewMovement>.Instance.ridingRocket)
		{
			MonoSingleton<NewMovement>.Instance.ridingRocket.PlayerRideEnd();
		}
		this.forcingGroundCheck = true;
		MonoSingleton<NewMovement>.Instance.gc.ForceOff();
		MonoSingleton<NewMovement>.Instance.slopeCheck.ForceOff();
	}

	// Token: 0x06000CE9 RID: 3305 RVA: 0x00062974 File Offset: 0x00060B74
	private void StopForceGroundCheck()
	{
		this.forcingGroundCheck = false;
		MonoSingleton<NewMovement>.Instance.gc.StopForceOff();
		MonoSingleton<NewMovement>.Instance.slopeCheck.StopForceOff();
	}

	// Token: 0x06000CEA RID: 3306 RVA: 0x0006299C File Offset: 0x00060B9C
	private void SemiBlockCheck()
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(this.hand.position, this.caughtTransform.position + this.caughtPoint - this.hand.position, out raycastHit, Vector3.Distance(this.hand.position, this.caughtTransform.position + this.caughtPoint), 2048, QueryTriggerInteraction.Ignore) && raycastHit.collider.transform != this.caughtCollider.transform)
		{
			this.semiBlocked = Mathf.MoveTowards(this.semiBlocked, 1f, Time.fixedDeltaTime);
			if (this.semiBlocked >= 1f)
			{
				this.StopThrow(0f, false);
				return;
			}
		}
		else
		{
			this.semiBlocked = 0f;
		}
	}

	// Token: 0x04001124 RID: 4388
	public bool equipped;

	// Token: 0x04001125 RID: 4389
	private LineRenderer lr;

	// Token: 0x04001126 RID: 4390
	private Animator anim;

	// Token: 0x04001127 RID: 4391
	private Vector3 hookPoint;

	// Token: 0x04001128 RID: 4392
	private Vector3 previousHookPoint;

	// Token: 0x04001129 RID: 4393
	[HideInInspector]
	public HookState state;

	// Token: 0x0400112A RID: 4394
	private bool returning;

	// Token: 0x0400112B RID: 4395
	[SerializeField]
	private GameObject model;

	// Token: 0x0400112C RID: 4396
	private CapsuleCollider playerCollider;

	// Token: 0x0400112D RID: 4397
	public Transform hand;

	// Token: 0x0400112E RID: 4398
	public Transform hook;

	// Token: 0x0400112F RID: 4399
	public GameObject hookModel;

	// Token: 0x04001130 RID: 4400
	private Vector3 throwDirection;

	// Token: 0x04001131 RID: 4401
	private float returnDistance;

	// Token: 0x04001132 RID: 4402
	private LayerMask throwMask;

	// Token: 0x04001133 RID: 4403
	private LayerMask enviroMask;

	// Token: 0x04001134 RID: 4404
	private LayerMask enemyMask;

	// Token: 0x04001135 RID: 4405
	private float throwWarp;

	// Token: 0x04001136 RID: 4406
	private Transform caughtTransform;

	// Token: 0x04001137 RID: 4407
	private Vector3 caughtPoint;

	// Token: 0x04001138 RID: 4408
	private Collider caughtCollider;

	// Token: 0x04001139 RID: 4409
	private EnemyIdentifier caughtEid;

	// Token: 0x0400113A RID: 4410
	private List<EnemyType> deadIgnoreTypes = new List<EnemyType>();

	// Token: 0x0400113B RID: 4411
	private List<EnemyType> lightEnemies = new List<EnemyType>();

	// Token: 0x0400113C RID: 4412
	private GroundCheckEnemy enemyGroundCheck;

	// Token: 0x0400113D RID: 4413
	private Rigidbody enemyRigidbody;

	// Token: 0x0400113E RID: 4414
	private HookPoint caughtHook;

	// Token: 0x0400113F RID: 4415
	private bool lightTarget;

	// Token: 0x04001140 RID: 4416
	[SerializeField]
	private LineRenderer inspectLr;

	// Token: 0x04001141 RID: 4417
	private bool forcingGroundCheck;

	// Token: 0x04001142 RID: 4418
	private bool forcingFistControl;

	// Token: 0x04001143 RID: 4419
	private AudioSource aud;

	// Token: 0x04001144 RID: 4420
	[Header("Sounds")]
	public GameObject throwSound;

	// Token: 0x04001145 RID: 4421
	public GameObject hitSound;

	// Token: 0x04001146 RID: 4422
	public GameObject pullSound;

	// Token: 0x04001147 RID: 4423
	public GameObject pullDoneSound;

	// Token: 0x04001148 RID: 4424
	public GameObject catchSound;

	// Token: 0x04001149 RID: 4425
	public GameObject errorSound;

	// Token: 0x0400114A RID: 4426
	public AudioClip throwLoop;

	// Token: 0x0400114B RID: 4427
	public AudioClip pullLoop;

	// Token: 0x0400114C RID: 4428
	public GameObject wooshSound;

	// Token: 0x0400114D RID: 4429
	private GameObject currentWoosh;

	// Token: 0x0400114E RID: 4430
	public GameObject clinkSparks;

	// Token: 0x0400114F RID: 4431
	public GameObject clinkObjectSparks;

	// Token: 0x04001150 RID: 4432
	private float cooldown;

	// Token: 0x04001151 RID: 4433
	private CameraFrustumTargeter targeter;

	// Token: 0x04001152 RID: 4434
	[HideInInspector]
	public bool beingPulled;

	// Token: 0x04001153 RID: 4435
	private List<Rigidbody> caughtObjects = new List<Rigidbody>();

	// Token: 0x04001154 RID: 4436
	private float semiBlocked;

	// Token: 0x04001155 RID: 4437
	private Grenade caughtGrenade;

	// Token: 0x04001156 RID: 4438
	private Cannonball caughtCannonball;
}
