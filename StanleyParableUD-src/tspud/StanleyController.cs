using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001A2 RID: 418
public class StanleyController : PortalTraveller
{
	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x06000990 RID: 2448 RVA: 0x0002D446 File Offset: 0x0002B646
	public static StanleyController Instance
	{
		get
		{
			if (StanleyController._instance == null)
			{
				GameMaster instance = Singleton<GameMaster>.Instance;
				StanleyController._instance = ((instance != null) ? instance.RespawnStanley() : null);
			}
			return StanleyController._instance;
		}
	}

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x06000991 RID: 2449 RVA: 0x0002D470 File Offset: 0x0002B670
	// (set) Token: 0x06000992 RID: 2450 RVA: 0x0002D478 File Offset: 0x0002B678
	public Transform stanleyTransform { get; private set; }

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x06000993 RID: 2451 RVA: 0x0002D481 File Offset: 0x0002B681
	// (set) Token: 0x06000994 RID: 2452 RVA: 0x0002D489 File Offset: 0x0002B689
	public Camera cam { get; private set; }

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x06000995 RID: 2453 RVA: 0x0002D492 File Offset: 0x0002B692
	// (set) Token: 0x06000996 RID: 2454 RVA: 0x0002D49A File Offset: 0x0002B69A
	public Camera currentCam { get; private set; }

	// Token: 0x170000CC RID: 204
	// (get) Token: 0x06000997 RID: 2455 RVA: 0x0002D4A3 File Offset: 0x0002B6A3
	// (set) Token: 0x06000998 RID: 2456 RVA: 0x0002D4AB File Offset: 0x0002B6AB
	public Transform camParent { get; private set; }

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x06000999 RID: 2457 RVA: 0x0002D4B4 File Offset: 0x0002B6B4
	// (set) Token: 0x0600099A RID: 2458 RVA: 0x0002D4BC File Offset: 0x0002B6BC
	public Transform camTransform { get; private set; }

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x0600099B RID: 2459 RVA: 0x0002D4C5 File Offset: 0x0002B6C5
	// (set) Token: 0x0600099C RID: 2460 RVA: 0x0002D4CD File Offset: 0x0002B6CD
	public float WalkingSpeedMultiplier { get; private set; } = 1f;

	// Token: 0x170000CF RID: 207
	// (set) Token: 0x0600099D RID: 2461 RVA: 0x0002D4D6 File Offset: 0x0002B6D6
	public bool WalkingSpeedAffectsFootstepSoundSpeed
	{
		set
		{
			this.WalkingSpeedAffectsFootstepSoundSpeedScale = (float)(value ? 1 : 0);
		}
	}

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x0600099E RID: 2462 RVA: 0x0002D4E6 File Offset: 0x0002B6E6
	// (set) Token: 0x0600099F RID: 2463 RVA: 0x0002D4EE File Offset: 0x0002B6EE
	public float WalkingSpeedAffectsFootstepSoundSpeedScale { get; set; }

	// Token: 0x060009A0 RID: 2464 RVA: 0x0002D4F7 File Offset: 0x0002B6F7
	public void SetMovementSpeedMultiplier(float newMultiplier)
	{
		this.WalkingSpeedMultiplier = newMultiplier;
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x0002D500 File Offset: 0x0002B700
	public void SetCharacterHeightMultiplier(float newMultiplier)
	{
		this.characterHeightMultipler = newMultiplier;
	}

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x060009A2 RID: 2466 RVA: 0x0002D509 File Offset: 0x0002B709
	// (set) Token: 0x060009A3 RID: 2467 RVA: 0x0002D511 File Offset: 0x0002B711
	public bool ForceStayCrouched { get; set; }

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x060009A4 RID: 2468 RVA: 0x0002D51A File Offset: 0x0002B71A
	// (set) Token: 0x060009A5 RID: 2469 RVA: 0x0002D522 File Offset: 0x0002B722
	private bool SnapToNewHeightNextFrame { get; set; }

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x060009A6 RID: 2470 RVA: 0x0002D52B File Offset: 0x0002B72B
	// (set) Token: 0x060009A7 RID: 2471 RVA: 0x0002D533 File Offset: 0x0002B733
	public bool ForceCrouched { get; set; }

	// Token: 0x060009A8 RID: 2472 RVA: 0x0002D53C File Offset: 0x0002B73C
	public void ResetVelocity()
	{
		this.movement = (this.movementGoal = Vector3.zero);
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x060009A9 RID: 2473 RVA: 0x0002D55D File Offset: 0x0002B75D
	// (set) Token: 0x060009AA RID: 2474 RVA: 0x0002D56C File Offset: 0x0002B76C
	public float FieldOfView
	{
		get
		{
			return this.FieldOfViewBase + this.FieldOfViewAdditiveModifier;
		}
		set
		{
			this.FieldOfViewAdditiveModifier = value - this.FieldOfViewBase;
		}
	}

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x060009AB RID: 2475 RVA: 0x0002D57C File Offset: 0x0002B77C
	// (set) Token: 0x060009AC RID: 2476 RVA: 0x0002D584 File Offset: 0x0002B784
	public float FieldOfViewBase { get; private set; }

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x060009AD RID: 2477 RVA: 0x0002D58D File Offset: 0x0002B78D
	// (set) Token: 0x060009AE RID: 2478 RVA: 0x0002D595 File Offset: 0x0002B795
	public float FieldOfViewAdditiveModifier { get; set; }

	// Token: 0x060009AF RID: 2479 RVA: 0x0002D5A0 File Offset: 0x0002B7A0
	private void OnFOVChange(LiveData liveData)
	{
		float fieldOfViewBase = this.FieldOfViewBase;
		float fieldOfViewAdditiveModifier = this.FieldOfViewAdditiveModifier;
		float num = fieldOfViewBase + fieldOfViewAdditiveModifier;
		this.FieldOfViewBase = liveData.FloatValue;
		if (this.FieldOfViewAdditiveModifier != 0f)
		{
			this.FieldOfViewAdditiveModifier = num - this.FieldOfViewBase;
		}
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x0002D5E4 File Offset: 0x0002B7E4
	private void Awake()
	{
		if (StanleyController._instance == null)
		{
			StanleyController._instance = this;
		}
		if (StanleyController._instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		Object.DontDestroyOnLoad(base.gameObject);
		if (this != StanleyController.Instance)
		{
			Object.DestroyImmediate(base.gameObject);
			return;
		}
		this.stanleyTransform = base.transform;
		this.character = base.GetComponent<CharacterController>();
		this.cam = base.GetComponentInChildren<Camera>();
		this.camTransform = this.cam.transform;
		this.camParent = this.camTransform.parent;
		AssetBundleControl.OnScenePreLoad = (Action)Delegate.Combine(AssetBundleControl.OnScenePreLoad, new Action(this.OnScenePreLoad));
		AssetBundleControl.OnSceneReady = (Action)Delegate.Combine(AssetBundleControl.OnSceneReady, new Action(this.OnSceneReady));
		this.AColor = this.cam.gameObject.GetComponent<AmplifyColorBase>();
		this.camParentOrigLocalPos = this.camParent.localPosition;
		this.CreateFootstepSources();
		this.CreateFootstepDictionary();
		this.mainCamera = base.GetComponentInChildren<MainCamera>();
		FloatConfigurable floatConfigurable = this.fovSettingConfigurable;
		floatConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(floatConfigurable.OnValueChanged, new Action<LiveData>(this.OnFOVChange));
		this.FieldOfViewBase = this.fovSettingConfigurable.GetFloatValue();
		this.gravityMultiplier = this.groundedGravityMultiplier;
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x0002D74A File Offset: 0x0002B94A
	private void Start()
	{
		this.OnSceneReady();
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x0002D754 File Offset: 0x0002B954
	private void OnDestroy()
	{
		FloatConfigurable floatConfigurable = this.fovSettingConfigurable;
		floatConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(floatConfigurable.OnValueChanged, new Action<LiveData>(this.OnFOVChange));
		AssetBundleControl.OnSceneReady = (Action)Delegate.Remove(AssetBundleControl.OnSceneReady, new Action(this.OnSceneReady));
	}

	// Token: 0x060009B3 RID: 2483 RVA: 0x0002D7A8 File Offset: 0x0002B9A8
	private void CreateFootstepDictionary()
	{
		for (int i = 0; i < this.footstepCollections.Length; i++)
		{
			AudioCollection audioCollection = this.footstepCollections[i];
			int index = audioCollection.GetIndex();
			if (audioCollection != null && !this.footstepDictionary.ContainsKey(index))
			{
				this.footstepDictionary.Add(index, audioCollection);
			}
		}
	}

	// Token: 0x060009B4 RID: 2484 RVA: 0x0002D7FC File Offset: 0x0002B9FC
	private void CreateFootstepSources()
	{
		int num = 8;
		this.footstepSources = new AudioSource[num];
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.footstepSource.gameObject);
			this.footstepSources[i] = gameObject.GetComponent<AudioSource>();
		}
		for (int j = 0; j < this.footstepSources.Length; j++)
		{
			this.footstepSources[j].transform.parent = this.footstepSource.transform;
		}
	}

	// Token: 0x060009B5 RID: 2485 RVA: 0x0002D874 File Offset: 0x0002BA74
	private void OnScenePreLoad()
	{
		Singleton<GameMaster>.Instance;
		this.WalkingSpeedMultiplier = 1f;
		this.WalkingSpeedAffectsFootstepSoundSpeed = false;
		this.characterHeightMultipler = 1f;
		this.ResetVelocity();
		this.FieldOfViewAdditiveModifier = 0f;
		this.wasCrouching = false;
		this.ForceCrouched = false;
		this.ForceStayCrouched = false;
		this.SnapToNewHeightNextFrame = true;
		if (this.cam != null)
		{
			this.cam.farClipPlane = 50f;
		}
		this.cam.gameObject.transform.localPosition = Vector3.zero;
		this.cam.gameObject.transform.localRotation = Quaternion.identity;
		this.Bucket.SetWalkingSpeed(0f);
		this.Bucket.SetAnimationSpeed(1f);
		this.FreezeMotionAndView();
	}

	// Token: 0x060009B6 RID: 2486 RVA: 0x0002D950 File Offset: 0x0002BB50
	private void OnSceneReady()
	{
		Singleton<GameMaster>.Instance;
		this.masterStartFound = false;
		this.inAirTimer = 0f;
		this.outOfBoundsReported = false;
		this.executeJump = false;
		this.jumpValue = 0f;
		this.jumpTime = 0f;
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x0002D9A0 File Offset: 0x0002BBA0
	private void FixedUpdate()
	{
		this.velocityAccumulation += this.character.velocity.magnitude * Time.fixedDeltaTime;
		this.velocityAccumulation += Mathf.Abs(this.character.velocity.y * Time.fixedDeltaTime) * this.footstepsYMultiplier;
	}

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x060009B8 RID: 2488 RVA: 0x0002DA02 File Offset: 0x0002BC02
	public static bool AltKeyPressed
	{
		get
		{
			return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
		}
	}

	// Token: 0x060009B9 RID: 2489 RVA: 0x0002DA1C File Offset: 0x0002BC1C
	private void Update()
	{
		if (!Singleton<GameMaster>.Instance.IsLoading && GameMaster.ONMAINMENUORSETTINGS)
		{
			AudioListener.volume = Singleton<GameMaster>.Instance.masterVolume;
		}
		StanleyController.StanleyPosition = base.transform.position;
		this.cam.fieldOfView = this.FieldOfViewBase + this.FieldOfViewAdditiveModifier;
		if (!this.viewFrozen)
		{
			this.View();
		}
		if (!this.motionFrozen)
		{
			this.Movement();
			this.UpdateCurrentlyStandingOn();
			this.Footsteps();
			this.ClickingOnThings();
		}
		else if (this.character.enabled)
		{
			this.character.Move(Vector2.zero);
		}
		if (!this.viewFrozen)
		{
			this.FloatCamera();
		}
		if (BucketController.HASBUCKET)
		{
			if (this.character.enabled && this.grounded)
			{
				this.Bucket.SetWalkingSpeed(this.character.velocity.magnitude / (this.walkingSpeed * this.WalkingSpeedMultiplier));
				return;
			}
			this.Bucket.SetWalkingSpeed(0f);
		}
	}

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x060009BA RID: 2490 RVA: 0x0002DB2D File Offset: 0x0002BD2D
	private float DeltaTime
	{
		get
		{
			return Time.deltaTime;
		}
	}

	// Token: 0x060009BB RID: 2491 RVA: 0x0002DB34 File Offset: 0x0002BD34
	private float SensitivityRemap(float n)
	{
		return n * n + 0.5f * n + 0.5f;
	}

	// Token: 0x060009BC RID: 2492 RVA: 0x0002DB34 File Offset: 0x0002BD34
	private float HighSensitivityRemap(float n, float factor)
	{
		return n * n + 0.5f * n + 0.5f;
	}

	// Token: 0x060009BD RID: 2493 RVA: 0x0002DB48 File Offset: 0x0002BD48
	private float HighSensitivityRemapDoubling(float n, float doubles = 4f)
	{
		float num = 0.5f;
		return Mathf.Pow(2f, n * doubles) * num;
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x0002DB6C File Offset: 0x0002BD6C
	private void View()
	{
		Vector2 vector = Singleton<GameMaster>.Instance.stanleyActions.View.Vector;
		Vector2 vector2 = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
		float num = Mathf.InverseLerp(this.controllerSensitivityConfigurable.MinValue, this.controllerSensitivityConfigurable.MaxValue, this.controllerSensitivityConfigurable.GetFloatValue());
		float num2 = this.HighSensitivityRemapDoubling(num, 4f);
		vector2.x *= num2;
		vector2.y *= num2;
		if (this.invertYConfigurable.GetBooleanValue())
		{
			vector2.y *= -1f;
		}
		float num3 = Mathf.InverseLerp(this.controllerSensitivityConfigurable.MinValue, this.controllerSensitivityConfigurable.MaxValue, this.controllerSensitivityConfigurable.GetFloatValue());
		float num4 = this.HighSensitivityRemapDoubling(num3, 4f);
		vector.x *= num4;
		vector.y *= num4;
		if (this.invertYConfigurable.GetBooleanValue())
		{
			vector.y *= -1f;
		}
		if (Time.deltaTime > 0f)
		{
			vector2 /= Time.deltaTime * 60f;
		}
		if (Input.touchCount > 0)
		{
			vector2 = Vector2.zero;
		}
		float num5 = -1f;
		if (this.yInvert)
		{
			num5 = 1f;
		}
		vector2 = new Vector2(vector2.x * this.mouseSensitivityX, vector2.y * num5 * this.mouseSensitivityY);
		vector = new Vector2(vector.x * this.controllerSensitivityX, vector.y * num5 * this.controllerSensitivityY);
		Vector2 vector3 = Vector2.zero;
		vector3 = vector2 + vector * 0.25f;
		vector3 *= this.DeltaTime * 70f;
		Quaternion quaternion = Quaternion.AngleAxis(vector3.x, Vector3.up);
		this.viewPitch += vector3.y;
		this.viewPitch = Mathf.Clamp(this.viewPitch, -90f, 90f);
		if (BackgroundCamera.OnRotationUpdate != null)
		{
			BackgroundCamera.OnRotationUpdate(new Vector3(vector3.y, vector3.x));
		}
		this.camParent.localRotation = Quaternion.AngleAxis(this.viewPitch, Vector3.right);
		base.transform.rotation = quaternion * base.transform.rotation;
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x0002DDD0 File Offset: 0x0002BFD0
	private bool RayHitGround(Vector3 offset, out GameObject hitGameObject, out int hitTriangleIndex)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position + offset, Vector3.down, out raycastHit, 3f, this.groundedLayers))
		{
			this.hitNormal = raycastHit.normal;
			hitGameObject = raycastHit.collider.gameObject;
			hitTriangleIndex = raycastHit.triangleIndex;
			return true;
		}
		hitGameObject = null;
		hitTriangleIndex = -1;
		return false;
	}

	// Token: 0x060009C0 RID: 2496 RVA: 0x0002DE3C File Offset: 0x0002C03C
	private void Movement()
	{
		this.grounded = this.character.isGrounded;
		float y = Singleton<GameMaster>.Instance.stanleyActions.Movement.Y;
		float x = Singleton<GameMaster>.Instance.stanleyActions.Movement.X;
		this.movementInput.x = x;
		this.movementInput.z = y;
		if (PlatformSettings.Instance.isStandalone.GetBooleanValue() && this.mouseWalkConfigurable.GetBooleanValue())
		{
			bool flag = false;
			if (this.mouseWalkToggleConfigurable.GetBooleanValue())
			{
				if (Singleton<GameMaster>.Instance.stanleyActions.Autowalk.WasPressed)
				{
					this.autoWalking = !this.autoWalking;
				}
				flag = this.autoWalking;
			}
			else
			{
				this.autoWalking = false;
				if (Singleton<GameMaster>.Instance.stanleyActions.Autowalk.IsPressed)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.movementInput.z = 1f;
			}
		}
		this.movementInput = Vector3.ClampMagnitude(this.movementInput, 1f) * (this.executeJump ? 0.5f : 1f);
		if (this.movementInput.magnitude > 0f)
		{
			this.movementGoal = Vector3.Lerp(this.movementGoal, this.movementInput, this.DeltaTime * this.runAcceleration);
		}
		else
		{
			this.movementGoal = Vector3.Lerp(this.movementGoal, this.movementInput, this.DeltaTime * this.runDeacceleration);
		}
		if (!this.executeJump && this.jumpConfigurable.GetBooleanValue() && Singleton<GameMaster>.Instance.stanleyActions.JumpAction.WasPressed)
		{
			if (!this.executeJump && StanleyController.OnActuallyJumping != null)
			{
				StanleyController.OnActuallyJumping();
			}
			this.executeJump = true;
		}
		if (this.executeJump)
		{
			this.jumpTime += this.DeltaTime * this.jumpAccerlation;
			this.jumpValue = this.jumpCurve.Evaluate(Mathf.Clamp01(this.jumpTime)) * this.jumpPower;
		}
		if (this.jumpTime >= 1f / this.jumpAccerlation * this.jumpAccerlation)
		{
			this.executeJump = false;
			this.jumpValue = 0f;
			this.jumpTime = 0f;
		}
		bool flag2 = Singleton<GameMaster>.Instance.stanleyActions.Crouch.IsPressed;
		if (this.wasCrouching && this.ForceStayCrouched)
		{
			flag2 = true;
		}
		if (this.ForceCrouched)
		{
			flag2 = true;
		}
		float num;
		if (flag2)
		{
			num = this.crouchedColliderHeight;
		}
		else
		{
			num = this.uncrouchedColliderHeight;
		}
		this.character.height = Mathf.SmoothStep(this.character.height, num, this.crouchSmoothing);
		if (this.SnapToNewHeightNextFrame)
		{
			this.character.height = num;
			this.SnapToNewHeightNextFrame = false;
		}
		this.cameraParent.localPosition = Vector3.up * this.character.height / 2f * this.characterHeightMultipler;
		this.camParentOrigLocalPos = this.camParent.localPosition;
		this.wasCrouching = flag2;
		this.movement = this.movementGoal * this.walkingSpeed * this.WalkingSpeedMultiplier;
		this.movement = base.transform.TransformDirection(this.movement);
		this.movement += Vector3.up * this.jumpValue;
		Action<Vector3> onPositionUpdate = BackgroundCamera.OnPositionUpdate;
		if (onPositionUpdate != null)
		{
			onPositionUpdate(new Vector3(0f, this.character.velocity.y, 0f));
		}
		RotatingDoor rotatingDoor = this.WillHitDoor(this.movement * Singleton<GameMaster>.Instance.GameDeltaTime);
		if (rotatingDoor == null)
		{
			if (this.lastHitRotatingDoor != null)
			{
				this.lastHitRotatingDoor.PlayerTouchingDoor = false;
				this.lastHitRotatingDoor = null;
			}
		}
		else
		{
			if (this.lastHitRotatingDoor != null && this.lastHitRotatingDoor != rotatingDoor)
			{
				Debug.LogWarning("Player is hitting multiple doors this should not happen!\n" + this.lastHitRotatingDoor.name + "\n" + rotatingDoor.name);
			}
			this.lastHitRotatingDoor = rotatingDoor;
			this.lastHitRotatingDoor.PlayerTouchingDoor = true;
		}
		this.UpdateInAir(!this.grounded);
		if (!this.grounded)
		{
			this.gravityMultiplier = Mathf.Lerp(this.gravityMultiplier, 1f, Singleton<GameMaster>.Instance.GameDeltaTime * this.gravityFallAcceleration);
			this.movement *= this.inAirMovementMultiplier;
		}
		else
		{
			this.gravityMultiplier = this.groundedGravityMultiplier;
		}
		if (flag2)
		{
			this.movement *= this.crouchMovementMultiplier;
		}
		if (this.character.enabled)
		{
			this.character.Move((this.movement + Vector3.up * this.maxGravity * this.gravityMultiplier) * Singleton<GameMaster>.Instance.GameDeltaTime);
		}
	}

	// Token: 0x060009C1 RID: 2497 RVA: 0x0002E348 File Offset: 0x0002C548
	private void UpdateInAir(bool inAir)
	{
		if (!this.outOfBoundsReported && inAir && this.character.velocity.y <= -1f && !GameMaster.PAUSEMENUACTIVE && !Singleton<GameMaster>.Instance.IsLoading)
		{
			this.inAirTimer += Time.deltaTime;
			if (this.inAirTimer >= this.inAirLimit)
			{
				if (StanleyController.OnOutOfBounds != null)
				{
					StanleyController.OnOutOfBounds();
				}
				this.outOfBoundsReported = true;
				return;
			}
		}
		else
		{
			this.inAirTimer = 0f;
		}
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x0002E3D0 File Offset: 0x0002C5D0
	private RotatingDoor WillHitDoor(Vector3 motion)
	{
		Vector3 vector = this.character.transform.position + motion + this.character.center;
		foreach (Collider collider in Physics.OverlapCapsule(vector - Vector3.up * this.character.height / 2f, vector + Vector3.up * this.character.height / 2f, this.character.radius))
		{
			RotatingDoor component = collider.gameObject.GetComponent<RotatingDoor>();
			if (component != null)
			{
				Vector3 vector2 = collider.ClosestPoint(this.character.transform.position);
				Vector3 vector3 = this.character.transform.position - vector2;
				float magnitude = vector3.magnitude;
				if (magnitude < this.character.radius)
				{
					if (magnitude == 0f)
					{
						Debug.LogError("[StanleyController] Distance to door should NEVER be zero, check if door collider is MeshCollider, replace it with a Box Collider please");
					}
					else
					{
						Vector3 vector4 = vector3 / magnitude;
						float num = this.character.radius - magnitude;
						this.character.Move(vector4 * num);
					}
				}
				return component;
			}
		}
		return null;
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x0002E523 File Offset: 0x0002C723
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		this.hitNormal = hit.normal;
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x0002E534 File Offset: 0x0002C734
	private void ClickingOnThings()
	{
		if (!Singleton<GameMaster>.Instance.FullScreenMoviePlaying && Singleton<GameMaster>.Instance.stanleyActions.UseAction.WasPressed)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(this.camParent.position, this.camParent.forward, out raycastHit, this.armReach, this.clickLayers, QueryTriggerInteraction.Ignore))
			{
				GameObject gameObject = raycastHit.collider.gameObject;
				HammerEntity component = gameObject.GetComponent<HammerEntity>();
				if (component != null)
				{
					component.Use();
				}
				else
				{
					this.PlayKeyboardSound();
				}
				if (StanleyController.OnInteract != null)
				{
					StanleyController.OnInteract(gameObject);
					return;
				}
			}
			else
			{
				this.PlayKeyboardSound();
				if (StanleyController.OnInteract != null)
				{
					StanleyController.OnInteract(null);
				}
			}
		}
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x0002E5F0 File Offset: 0x0002C7F0
	private void OnApplicationPause(bool pause)
	{
		if (pause && this.character.enabled)
		{
			this.character.Move(Vector2.zero);
		}
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x0002E618 File Offset: 0x0002C818
	private void PlayFootstepSound(AudioEntry entry)
	{
		AudioClip audioClip;
		bool clip = entry.GetClip(out audioClip);
		AudioSource availableFootstepSource = this.GetAvailableFootstepSource();
		if (clip && availableFootstepSource != null)
		{
			availableFootstepSource.clip = audioClip;
			availableFootstepSource.pitch = entry.GetPitch();
			availableFootstepSource.volume = entry.GetVolume();
			availableFootstepSource.Play();
		}
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x0002E664 File Offset: 0x0002C864
	private AudioSource GetAvailableFootstepSource()
	{
		for (int i = 0; i < this.footstepSources.Length; i++)
		{
			AudioSource audioSource = this.footstepSources[i];
			if (!audioSource.isPlaying)
			{
				return audioSource;
			}
		}
		return null;
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x0002E698 File Offset: 0x0002C898
	private void PlayKeyboardSound()
	{
		this.useSource.Stop();
		(Singleton<GameMaster>.Instance.barking ? this.barkingCollection : this.keyboardCollection).SetVolumeAndPitchAndPlayClip(this.useSource);
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x0002E6CC File Offset: 0x0002C8CC
	public void Teleport(StanleyController.TeleportType style, Vector3 destination, Vector3 up, bool useAngle, bool freezeAtStartOfTeleport = true, bool unfreezeAtEndOfTeleport = true, Transform orientationTransform = null)
	{
		this.Teleport(style, base.transform.position, destination, up, useAngle, freezeAtStartOfTeleport, unfreezeAtEndOfTeleport, orientationTransform);
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x0002E6F8 File Offset: 0x0002C8F8
	public void Teleport(StanleyController.TeleportType style, Vector3 landmark, Vector3 destination, Vector3 up, bool useAngle, bool freezeAtStartOfTeleport = true, bool unfreezeAtEndOfTeleport = true, Transform orientationTransform = null)
	{
		bool flag = true;
		if (freezeAtStartOfTeleport)
		{
			this.FreezeMotionAndView();
		}
		switch (style)
		{
		case StanleyController.TeleportType.PlayerStart:
			flag = !this.masterStartFound;
			break;
		case StanleyController.TeleportType.PlayerStartMaster:
			this.masterStartFound = true;
			break;
		case StanleyController.TeleportType.TriggerTeleport:
			flag = true;
			break;
		}
		if (flag)
		{
			Vector3 vector = base.transform.position - landmark;
			RaycastHit raycastHit;
			if (Physics.Raycast(new Ray(destination, Vector3.down), out raycastHit, 2f, this.groundedLayers, QueryTriggerInteraction.Ignore))
			{
				Vector3 vector2 = destination + vector;
				float num = StanleyController.Instance.character.height / 2f + StanleyController.Instance.character.skinWidth;
				base.transform.position = new Vector3(vector2.x, raycastHit.point.y + num, vector2.z);
			}
			else
			{
				base.transform.position = destination + vector + Vector3.up * 0.05f;
			}
			this.velocityAccumulation = 0f;
			if (useAngle && orientationTransform != null)
			{
				base.transform.rotation = orientationTransform.rotation;
				base.transform.Rotate(90f, 0f, 0f, Space.Self);
				float num2 = Vector3.Angle(base.transform.up, Vector3.up);
				base.transform.Rotate(-num2, 0f, 0f, Space.Self);
				float num3 = Vector3.Angle(base.transform.forward, -orientationTransform.up);
				this.viewPitch = Mathf.Clamp(num3, -90f, 90f);
				this.camParent.localRotation = Quaternion.AngleAxis(this.viewPitch, Vector3.right);
			}
			else if (useAngle)
			{
				Vector3 vector3 = Vector3.Cross(Vector3.up, up);
				Vector3 vector4 = Vector3.Cross(up, vector3);
				Vector3 vector5 = new Vector3(up.x, 0f, up.z);
				if (vector5 != Vector3.zero)
				{
					base.transform.rotation = Quaternion.LookRotation(vector5, Vector3.up);
				}
				Vector3 vector6 = new Vector3(0f, vector4.y, 0f);
				if (vector6 != Vector3.zero)
				{
					this.viewPitch = Vector3.Angle(Vector3.up, vector6);
					this.camParent.localRotation = Quaternion.AngleAxis(this.viewPitch, Vector3.right);
					if (BackgroundCamera.OnAlignToTransform != null)
					{
						BackgroundCamera.OnAlignToTransform(this.camParent);
					}
				}
			}
			if ((style == StanleyController.TeleportType.PlayerStart || style == StanleyController.TeleportType.PlayerStartMaster) && StanleyController.OnTeleportToPlayerStart != null)
			{
				StanleyController.OnTeleportToPlayerStart();
			}
		}
		if (unfreezeAtEndOfTeleport)
		{
			this.UnfreezeMotionAndView();
		}
	}

	// Token: 0x060009CB RID: 2507 RVA: 0x0002E9B8 File Offset: 0x0002CBB8
	public void Deparent(bool kill = false)
	{
		GameObject gameObject = null;
		if (base.transform.parent != null)
		{
			gameObject = base.transform.parent.gameObject;
		}
		base.transform.parent = null;
		if (kill && gameObject != null)
		{
			Object.Destroy(gameObject);
		}
		Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x0002EA14 File Offset: 0x0002CC14
	public void ParentTo(Transform adopter)
	{
		base.transform.parent = adopter;
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x0002EA22 File Offset: 0x0002CC22
	public void EnableCamera()
	{
		this.cam.enabled = true;
		this.currentCam = this.cam;
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x0002EA3C File Offset: 0x0002CC3C
	public void DisableCamera(Camera replacement)
	{
		this.cam.enabled = false;
		this.currentCam = replacement;
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x0002EA51 File Offset: 0x0002CC51
	public void FreezeMotion(bool commandFreezeMotion = false)
	{
		this.motionFrozen = true;
		this.character.enabled = false;
		if (commandFreezeMotion)
		{
			this.frozenFromCommandMotion = true;
		}
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x0002EA70 File Offset: 0x0002CC70
	public void UnfreezeMotion(bool commandUnfreezeMotion = false)
	{
		if (!commandUnfreezeMotion && this.frozenFromCommandMotion)
		{
			return;
		}
		this.motionFrozen = false;
		this.character.enabled = true;
		if (commandUnfreezeMotion && this.frozenFromCommandMotion)
		{
			this.frozenFromCommandMotion = false;
		}
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x0002EAA3 File Offset: 0x0002CCA3
	public void FreezeView(bool commandFreezeView = false)
	{
		this.viewFrozen = true;
		if (commandFreezeView)
		{
			this.frozenFromCommandView = true;
		}
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x0002EAB6 File Offset: 0x0002CCB6
	public void UnfreezeView(bool commandUnfreezeView = false)
	{
		if (!commandUnfreezeView && this.frozenFromCommandView)
		{
			return;
		}
		this.viewFrozen = false;
		if (commandUnfreezeView)
		{
			this.frozenFromCommandView = false;
		}
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x0002EAD5 File Offset: 0x0002CCD5
	public void ResetClientCommandFreezes()
	{
		this.frozenFromCommandView = false;
		this.frozenFromCommandMotion = false;
	}

	// Token: 0x060009D4 RID: 2516 RVA: 0x0002EAE5 File Offset: 0x0002CCE5
	public void FreezeMotionAndView()
	{
		this.FreezeMotion(false);
		this.FreezeView(false);
	}

	// Token: 0x060009D5 RID: 2517 RVA: 0x0002EAF5 File Offset: 0x0002CCF5
	public void UnfreezeMotionAndView()
	{
		this.UnfreezeMotion(false);
		this.UnfreezeView(false);
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x0002EB05 File Offset: 0x0002CD05
	public void StartFloating()
	{
		this.floatingPos = 0f;
		base.StartCoroutine(this.FloatFadeInOut(1f));
	}

	// Token: 0x060009D7 RID: 2519 RVA: 0x0002EB24 File Offset: 0x0002CD24
	public void EndFloating()
	{
		base.StartCoroutine(this.FloatFadeInOut(0f));
	}

	// Token: 0x060009D8 RID: 2520 RVA: 0x0002EB38 File Offset: 0x0002CD38
	private IEnumerator FloatFadeInOut(float targetStrength = 0f)
	{
		while (this.floatingStrength != targetStrength)
		{
			this.floatingStrength = Mathf.MoveTowards(this.floatingStrength, targetStrength, Singleton<GameMaster>.Instance.GameDeltaTime / 2f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060009D9 RID: 2521 RVA: 0x0002EB50 File Offset: 0x0002CD50
	private void FloatCamera()
	{
		float num = Mathf.Pow(Mathf.Sin(this.floatingPos), 2f) * 0.33f;
		this.camParent.localPosition = this.camParentOrigLocalPos + Vector3.up * num * this.floatingStrength;
		this.floatingPos += Singleton<GameMaster>.Instance.GameDeltaTime / 3f;
	}

	// Token: 0x060009DA RID: 2522 RVA: 0x0002EBC2 File Offset: 0x0002CDC2
	private void ResetFloat()
	{
		this.floatingStrength = 0f;
		base.StopCoroutine("FloatFadeInOut");
		this.camParent.localPosition = this.camParentOrigLocalPos;
	}

	// Token: 0x060009DB RID: 2523 RVA: 0x0002EBEB File Offset: 0x0002CDEB
	public void NewMapReset()
	{
		this.ResetFloat();
		this.EnableCamera();
		this.camParent.localRotation = Quaternion.identity;
		this.masterStartFound = false;
		if (this.mainCamera != null)
		{
			this.mainCamera.UpdatePortals();
		}
	}

	// Token: 0x060009DC RID: 2524 RVA: 0x0002EC29 File Offset: 0x0002CE29
	private void UpdateCurrentlyStandingOn()
	{
		this.standingOnUpdateTimer += Singleton<GameMaster>.Instance.GameDeltaTime;
		if (this.standingOnUpdateTimer >= this.standingOnUpdateLimit)
		{
			this.FindFootstepMaterial();
			this.standingOnUpdateTimer = 0f;
		}
	}

	// Token: 0x060009DD RID: 2525 RVA: 0x0002EC64 File Offset: 0x0002CE64
	private void Footsteps()
	{
		float num = ((this.currentFootstepAudioCollection != null) ? this.currentFootstepAudioCollection.AverageDuration : 0.69f);
		float walkingSpeedMultiplier = this.WalkingSpeedMultiplier;
		float num2 = Mathf.Lerp(num * this.WalkingSpeedMultiplier, num, this.WalkingSpeedAffectsFootstepSoundSpeedScale);
		if (this.velocityAccumulation >= num2)
		{
			this.velocityAccumulation = 0f;
			int num3 = (int)this.currentlyStandingOn;
			if (this.footstepDictionary.TryGetValue(num3, out this.currentFootstepAudioCollection))
			{
				AudioSource availableFootstepSource = this.GetAvailableFootstepSource();
				if (availableFootstepSource != null)
				{
					this.currentFootstepAudioCollection.SetVolumeAndPitchAndPlayClip(availableFootstepSource);
				}
			}
		}
	}

	// Token: 0x060009DE RID: 2526 RVA: 0x0002ED00 File Offset: 0x0002CF00
	private void FindFootstepMaterial()
	{
		StanleyData.FootstepSounds footstepSounds = this.currentlyStandingOn;
		GameObject gameObject;
		int num;
		if (this.RayHitGround(Vector3.up * 0.1f, out gameObject, out num))
		{
			MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
			NonRendererFootstepType component2 = gameObject.GetComponent<NonRendererFootstepType>();
			if (component2 != null && (!(component != null) || !component2.ForceOverrideMaterial))
			{
				this.currentlyStandingOn = component2.FootstepType;
				if (this.playFootstepOnNewMaterial && footstepSounds != this.currentlyStandingOn)
				{
					this.velocityAccumulation = 99f;
				}
				return;
			}
			if (component != null)
			{
				MeshCollider component3 = gameObject.GetComponent<MeshCollider>();
				Material material;
				if (component3 != null && component.sharedMaterials.Length >= 2 && this.GetMaterialFromTriangleIndex(component, component3, num, out material))
				{
					if (material.HasProperty("_FootstepType"))
					{
						this.currentlyStandingOn = (StanleyData.FootstepSounds)material.GetFloat("_FootstepType");
						if (this.playFootstepOnNewMaterial && footstepSounds != this.currentlyStandingOn)
						{
							this.velocityAccumulation = 99f;
						}
						return;
					}
				}
				else
				{
					for (int i = 0; i < component.sharedMaterials.Length; i++)
					{
						Material material2 = component.sharedMaterials[i];
						if (material2 != null && material2.HasProperty("_FootstepType"))
						{
							this.currentlyStandingOn = (StanleyData.FootstepSounds)material2.GetFloat("_FootstepType");
							if (this.playFootstepOnNewMaterial && footstepSounds != this.currentlyStandingOn)
							{
								this.velocityAccumulation = 99f;
							}
							return;
						}
					}
				}
			}
		}
		this.currentlyStandingOn = StanleyData.FootstepSounds.Silence;
	}

	// Token: 0x060009DF RID: 2527 RVA: 0x0002EE70 File Offset: 0x0002D070
	private bool GetMaterialFromTriangleIndex(MeshRenderer mRenderer, MeshCollider mCollider, int triIndex, out Material foundMaterial)
	{
		if (!(mCollider.sharedMesh == null))
		{
			Mesh sharedMesh = mCollider.sharedMesh;
			if (sharedMesh.isReadable && sharedMesh.triangles.Length >= triIndex * 3 + 3)
			{
				if (triIndex < 0)
				{
					string.Concat(new object[] { "triangle index is less than zero ", sharedMesh.name, " of ", mRenderer.name, " index = ", triIndex });
				}
				else
				{
					int num = sharedMesh.triangles[triIndex * 3];
					int num2 = sharedMesh.triangles[triIndex * 3 + 1];
					int num3 = sharedMesh.triangles[triIndex * 3 + 2];
					int num4 = -1;
					for (int i = 0; i < sharedMesh.subMeshCount; i++)
					{
						int[] triangles = sharedMesh.GetTriangles(i);
						for (int j = 0; j < triangles.Length; j += 3)
						{
							if (triangles[j] == num && triangles[j + 1] == num2 && triangles[j + 2] == num3)
							{
								num4 = i;
								break;
							}
						}
						if (num4 != -1)
						{
							break;
						}
					}
					if (num4 != -1)
					{
						foundMaterial = mRenderer.sharedMaterials[num4];
						return foundMaterial != null;
					}
				}
			}
		}
		foundMaterial = null;
		return false;
	}

	// Token: 0x060009E0 RID: 2528 RVA: 0x0002EF9B File Offset: 0x0002D19B
	public void StartPostProcessFade(Texture2D lut, float startVal, float endVal, float duration)
	{
		this.AColor.LutBlendTexture = lut;
		base.StartCoroutine(this.PostProcessFade(startVal, endVal, duration));
	}

	// Token: 0x060009E1 RID: 2529 RVA: 0x0002EFBA File Offset: 0x0002D1BA
	private IEnumerator PostProcessFade(float start, float end, float duration)
	{
		float startTime = Singleton<GameMaster>.Instance.GameTime;
		float endTime = startTime + duration;
		while (Singleton<GameMaster>.Instance.GameTime < endTime)
		{
			float num = Mathf.InverseLerp(startTime, endTime, Singleton<GameMaster>.Instance.GameTime);
			this.AColor.BlendAmount = Mathf.Lerp(start, end, num);
			yield return null;
		}
		this.AColor.BlendAmount = end;
		if (end == 0f)
		{
			this.AColor.LutBlendTexture = null;
		}
		yield break;
	}

	// Token: 0x060009E2 RID: 2530 RVA: 0x0002EFDE File Offset: 0x0002D1DE
	public void SetFarZ(float value, FarZVolume.CameraModes cameraMode = FarZVolume.CameraModes.RenderSkybox)
	{
		if (this.FarZCoroutine != null)
		{
			base.StopCoroutine(this.FarZCoroutine);
		}
		this.FarZCoroutine = this.FarZ(value, cameraMode);
		base.StartCoroutine(this.FarZCoroutine);
	}

	// Token: 0x060009E3 RID: 2531 RVA: 0x0002F00F File Offset: 0x0002D20F
	private IEnumerator FarZ(float value, FarZVolume.CameraModes cameraMode = FarZVolume.CameraModes.RenderSkybox)
	{
		float startTime = Singleton<GameMaster>.Instance.GameTime;
		float endTime = startTime + 1f;
		float startZ = this.cam.farClipPlane;
		if (cameraMode != FarZVolume.CameraModes.RenderSkybox)
		{
			if (cameraMode == FarZVolume.CameraModes.DepthOnly)
			{
				this.cam.clearFlags = CameraClearFlags.Depth;
			}
		}
		else
		{
			this.cam.clearFlags = CameraClearFlags.Skybox;
		}
		while (Singleton<GameMaster>.Instance.GameTime < endTime)
		{
			float num = Mathf.InverseLerp(startTime, endTime, Singleton<GameMaster>.Instance.GameTime);
			this.cam.farClipPlane = Mathf.Lerp(startZ, value, num);
			yield return null;
		}
		this.cam.farClipPlane = value;
		yield break;
	}

	// Token: 0x060009E4 RID: 2532 RVA: 0x0002F02C File Offset: 0x0002D22C
	private void LogController()
	{
		if (!(Singleton<GameMaster>.Instance.stanleyActions.Movement.Vector != Vector2.zero))
		{
			Singleton<GameMaster>.Instance.stanleyActions.View.Vector != Vector2.zero;
		}
		bool wasPressed = Singleton<GameMaster>.Instance.stanleyActions.UseAction.WasPressed;
		bool wasPressed2 = Singleton<GameMaster>.Instance.stanleyActions.JumpAction.WasPressed;
	}

	// Token: 0x04000972 RID: 2418
	private static StanleyController _instance;

	// Token: 0x04000973 RID: 2419
	public static Action<GameObject> OnInteract;

	// Token: 0x04000974 RID: 2420
	public static Vector3 StanleyPosition;

	// Token: 0x04000975 RID: 2421
	public static Action OnActuallyJumping;

	// Token: 0x04000976 RID: 2422
	public static Action OnTeleportToPlayerStart;

	// Token: 0x0400097C RID: 2428
	private CharacterController character;

	// Token: 0x0400097D RID: 2429
	private Vector3 camParentOrigLocalPos;

	// Token: 0x0400097E RID: 2430
	private float floatingStrength;

	// Token: 0x0400097F RID: 2431
	private float floatingPos;

	// Token: 0x04000980 RID: 2432
	private float viewPitch;

	// Token: 0x04000981 RID: 2433
	private RotatingDoor lastHitRotatingDoor;

	// Token: 0x04000982 RID: 2434
	public LayerMask clickLayers;

	// Token: 0x04000983 RID: 2435
	public LayerMask groundedLayers;

	// Token: 0x04000984 RID: 2436
	public Texture fadeTexture;

	// Token: 0x04000985 RID: 2437
	private bool fading;

	// Token: 0x04000986 RID: 2438
	private Color fadeColor = Color.black;

	// Token: 0x04000987 RID: 2439
	private bool fadeHold;

	// Token: 0x04000988 RID: 2440
	private IEnumerator fadeCoroutine;

	// Token: 0x04000989 RID: 2441
	private IEnumerator FarZCoroutine;

	// Token: 0x0400098A RID: 2442
	private AmplifyColorBase AColor;

	// Token: 0x0400098B RID: 2443
	private Vector3 movementInput;

	// Token: 0x0400098C RID: 2444
	private bool masterStartFound;

	// Token: 0x0400098D RID: 2445
	public Transform spinner;

	// Token: 0x0400098E RID: 2446
	public bool motionFrozen;

	// Token: 0x0400098F RID: 2447
	public bool viewFrozen;

	// Token: 0x04000990 RID: 2448
	private StanleyData.FootstepSounds currentlyStandingOn = StanleyData.FootstepSounds.Silence;

	// Token: 0x04000991 RID: 2449
	private GameObject currentlyStandingOnGameObject;

	// Token: 0x04000992 RID: 2450
	private Material currentlyStandingOnMaterial;

	// Token: 0x04000993 RID: 2451
	[Header("Audio")]
	public float stepDistance = 0.3f;

	// Token: 0x04000994 RID: 2452
	public float footstepVolume = 0.4f;

	// Token: 0x04000995 RID: 2453
	[SerializeField]
	private AudioCollection barkingCollection;

	// Token: 0x04000996 RID: 2454
	[SerializeField]
	private AudioCollection keyboardCollection;

	// Token: 0x04000997 RID: 2455
	[SerializeField]
	private AudioCollection[] footstepCollections;

	// Token: 0x04000998 RID: 2456
	[SerializeField]
	private AudioSource footstepSource;

	// Token: 0x04000999 RID: 2457
	[NonSerialized]
	private AudioSource[] footstepSources;

	// Token: 0x0400099A RID: 2458
	[SerializeField]
	private AudioSource useSource;

	// Token: 0x0400099B RID: 2459
	[Header("Movement")]
	[SerializeField]
	private float walkingSpeed = 3f;

	// Token: 0x0400099C RID: 2460
	[SerializeField]
	private float runAcceleration = 14f;

	// Token: 0x0400099D RID: 2461
	[SerializeField]
	private float runDeacceleration = 10f;

	// Token: 0x0400099E RID: 2462
	[SerializeField]
	private float inAirMovementMultiplier = 0.7f;

	// Token: 0x0400099F RID: 2463
	[SerializeField]
	private float crouchMovementMultiplier = 0.65f;

	// Token: 0x040009A0 RID: 2464
	[Header("Audio Movement")]
	[SerializeField]
	[Range(0f, 1f)]
	private float footstepsYMultiplier = 0.01f;

	// Token: 0x040009A1 RID: 2465
	[SerializeField]
	[Range(0f, 1f)]
	private float standingOnUpdateLimit = 0.5f;

	// Token: 0x040009A2 RID: 2466
	[SerializeField]
	private bool playFootstepOnNewMaterial = true;

	// Token: 0x040009A3 RID: 2467
	private AudioCollection currentFootstepAudioCollection;

	// Token: 0x040009A4 RID: 2468
	[Header("Gravity")]
	[SerializeField]
	private float maxGravity = -19f;

	// Token: 0x040009A5 RID: 2469
	[SerializeField]
	[Range(0f, 1f)]
	private float groundedGravityMultiplier = 0.1f;

	// Token: 0x040009A6 RID: 2470
	[SerializeField]
	private float gravityFallAcceleration = 1f;

	// Token: 0x040009A7 RID: 2471
	[NonSerialized]
	private float gravityMultiplier;

	// Token: 0x040009A8 RID: 2472
	[SerializeField]
	private BooleanConfigurable jumpConfigurable;

	// Token: 0x040009AB RID: 2475
	private float velocityAccumulation;

	// Token: 0x040009AC RID: 2476
	[Header("Controls")]
	public float mouseSensitivityX;

	// Token: 0x040009AD RID: 2477
	public float mouseSensitivityY;

	// Token: 0x040009AE RID: 2478
	public float controllerSensitivityX;

	// Token: 0x040009AF RID: 2479
	public float controllerSensitivityY;

	// Token: 0x040009B0 RID: 2480
	public bool yInvert;

	// Token: 0x040009B1 RID: 2481
	public float armReach = 1f;

	// Token: 0x040009B2 RID: 2482
	private Vector3 slidingDirection;

	// Token: 0x040009B3 RID: 2483
	private Vector3 hitNormal;

	// Token: 0x040009B4 RID: 2484
	private bool isSliding;

	// Token: 0x040009B5 RID: 2485
	[Header("Jump")]
	[SerializeField]
	private AnimationCurve jumpCurve;

	// Token: 0x040009B6 RID: 2486
	[SerializeField]
	private float jumpAccerlation = 50f;

	// Token: 0x040009B7 RID: 2487
	[SerializeField]
	private float jumpPower = 50f;

	// Token: 0x040009B8 RID: 2488
	private float jumpValue;

	// Token: 0x040009B9 RID: 2489
	private float jumpTime;

	// Token: 0x040009BA RID: 2490
	private bool executeJump;

	// Token: 0x040009BB RID: 2491
	[Header("Crouch")]
	[SerializeField]
	private Transform cameraParent;

	// Token: 0x040009BC RID: 2492
	[SerializeField]
	private float uncrouchedColliderHeight = 0.632f;

	// Token: 0x040009BD RID: 2493
	[SerializeField]
	private float crouchedColliderHeight = 0.316f;

	// Token: 0x040009BE RID: 2494
	[SerializeField]
	private float crouchSmoothing = 0.3f;

	// Token: 0x040009BF RID: 2495
	[SerializeField]
	private float characterHeightMultipler = 1f;

	// Token: 0x040009C3 RID: 2499
	private bool wasCrouching;

	// Token: 0x040009C4 RID: 2500
	public BucketController Bucket;

	// Token: 0x040009C5 RID: 2501
	[SerializeField]
	private StanleyData stanleyData;

	// Token: 0x040009C6 RID: 2502
	private Dictionary<int, AudioCollection> footstepDictionary = new Dictionary<int, AudioCollection>();

	// Token: 0x040009C7 RID: 2503
	private Vector3 movementGoal = Vector3.zero;

	// Token: 0x040009C8 RID: 2504
	private Vector3 movement = Vector3.zero;

	// Token: 0x040009C9 RID: 2505
	private MainCamera mainCamera;

	// Token: 0x040009CA RID: 2506
	[SerializeField]
	private FloatConfigurable fovSettingConfigurable;

	// Token: 0x040009CB RID: 2507
	[SerializeField]
	[HideInInspector]
	private FloatConfigurable mouseSensitivityConfigurable;

	// Token: 0x040009CC RID: 2508
	[SerializeField]
	private FloatConfigurable controllerSensitivityConfigurable;

	// Token: 0x040009CD RID: 2509
	[SerializeField]
	private BooleanConfigurable invertYConfigurable;

	// Token: 0x040009CE RID: 2510
	[SerializeField]
	private BooleanConfigurable mouseWalkConfigurable;

	// Token: 0x040009CF RID: 2511
	[SerializeField]
	private BooleanConfigurable mouseWalkToggleConfigurable;

	// Token: 0x040009D0 RID: 2512
	private float inAirLimit = 12f;

	// Token: 0x040009D1 RID: 2513
	private bool grounded;

	// Token: 0x040009D2 RID: 2514
	private float inAirTimer;

	// Token: 0x040009D3 RID: 2515
	private bool outOfBoundsReported;

	// Token: 0x040009D4 RID: 2516
	public static Action OnOutOfBounds;

	// Token: 0x040009D7 RID: 2519
	[Header("Debug")]
	[SerializeField]
	private GameObject lightmapDebugUIPrefab;

	// Token: 0x040009D8 RID: 2520
	private GameObject lightmapDebugUIInstance;

	// Token: 0x040009D9 RID: 2521
	private bool autoWalking;

	// Token: 0x040009DA RID: 2522
	private bool frozenFromCommandMotion;

	// Token: 0x040009DB RID: 2523
	private bool frozenFromCommandView;

	// Token: 0x040009DC RID: 2524
	private float standingOnUpdateTimer;

	// Token: 0x040009DD RID: 2525
	private const bool FOOTSTEP_TYPE_DEBUG = false;

	// Token: 0x040009DE RID: 2526
	private const bool FOOTSTEP_TIMING_DEBUG = false;

	// Token: 0x020003F4 RID: 1012
	public enum TeleportType
	{
		// Token: 0x0400148A RID: 5258
		PlayerStart,
		// Token: 0x0400148B RID: 5259
		PlayerStartMaster,
		// Token: 0x0400148C RID: 5260
		TriggerTeleport
	}
}
