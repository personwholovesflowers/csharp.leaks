using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200017E RID: 382
public class RotatingDoor : HammerEntity
{
	// Token: 0x170000BB RID: 187
	// (get) Token: 0x060008E9 RID: 2281 RVA: 0x0002A644 File Offset: 0x00028844
	// (set) Token: 0x060008EA RID: 2282 RVA: 0x0002A64C File Offset: 0x0002884C
	public bool PlayerTouchingDoor { get; set; }

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x060008EB RID: 2283 RVA: 0x0002A655 File Offset: 0x00028855
	public bool IsMoving
	{
		get
		{
			return this.currentAngle != this.targetAngle;
		}
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x0002A668 File Offset: 0x00028868
	private void Awake()
	{
		this.hasDoorBlocker = this.doorBlocker != null;
		if (this.brushAxisFixup)
		{
			this.dirChange *= -1f;
			if (this.reverseDirection)
			{
				this.dirChange *= -1f;
			}
		}
		this._renderer = base.GetComponent<MeshRenderer>();
		this._collider = base.GetComponent<BoxCollider>();
		if (this.renderColor != Color.white)
		{
			this._renderer.material.SetColor("_Color", this.renderColor);
		}
		if (!this.isDrawing)
		{
			this._renderer.enabled = false;
			this._collider.enabled = false;
		}
		this.closedRotation = base.transform.rotation;
		this.openRotation = Quaternion.AngleAxis(this.maxAngle * this.dirChange, this.axis) * base.transform.rotation;
		this.currentAngle = this.spawnAngle * this.dirChange;
		this.targetAngle = this.currentAngle;
		base.transform.rotation = Quaternion.AngleAxis(this.currentAngle, this.axis) * base.transform.rotation;
		this.source = base.GetComponent<AudioSource>();
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x0002A7B8 File Offset: 0x000289B8
	private void OnValidate()
	{
		Rigidbody component = base.GetComponent<Rigidbody>();
		if (component != null)
		{
			if (component.interpolation != RigidbodyInterpolation.Interpolate)
			{
				Debug.LogWarning("Please set door rigidbody interpolation to Interpolate", this);
			}
			if (component.collisionDetectionMode != CollisionDetectionMode.Continuous && component.collisionDetectionMode != CollisionDetectionMode.ContinuousSpeculative)
			{
				Debug.LogWarning("Please set door rigidbody collision mode to Continuous Speculative", this);
			}
		}
		if (this.doorBlocker != null && this.doorBlocker.transform.parent != base.transform)
		{
			this.doorBlocker = null;
		}
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x0002A838 File Offset: 0x00028A38
	[ContextMenu("SetTargetAngleTo0")]
	public void SetTargetAngleTo0()
	{
		this.targetAngle = 0f;
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x0002A845 File Offset: 0x00028A45
	[ContextMenu("SetTargetAngleTo90")]
	public void SetTargetAngleTo90()
	{
		this.targetAngle = 90f;
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x0002A854 File Offset: 0x00028A54
	private void Update()
	{
		if (!this.isDrawing)
		{
			return;
		}
		if (this.currentAngle == this.targetAngle)
		{
			return;
		}
		if (this.touchingPlayer)
		{
			return;
		}
		if (this.PlayerTouchingDoor)
		{
			return;
		}
		if (this.directions == RotatingDoor.DoorDirections.BackwardOnly && this.targetAngle > 0f)
		{
			this.targetAngle = 0f;
		}
		if (this.directions == RotatingDoor.DoorDirections.ForwardOnly && this.targetAngle < 0f)
		{
			this.targetAngle = 0f;
		}
		this.currentAngle = Mathf.MoveTowards(this.currentAngle, this.targetAngle, this.speed * Singleton<GameMaster>.Instance.GameDeltaTime);
		base.transform.rotation = Quaternion.AngleAxis(this.currentAngle, this.axis) * this.closedRotation;
		if (this.currentAngle == this.targetAngle)
		{
			if (Mathf.Abs(this.currentAngle) == this.maxAngle)
			{
				base.FireOutput(Outputs.OnFullyOpen);
				this.PlaySound(RotatingDoor.Sounds.Open);
				return;
			}
			if (this.currentAngle == 0f)
			{
				base.FireOutput(Outputs.OnFullyClosed);
				this.PlaySound(RotatingDoor.Sounds.Close);
			}
		}
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x0002A96C File Offset: 0x00028B6C
	[ContextMenu("Update Door Blocker Collider")]
	private void UpdateDoorBlockerColliderDymanicPosition()
	{
		float x = base.GetComponent<BoxCollider>().size.x;
		float num = Quaternion.Angle(this.closedRotation, base.transform.rotation);
		if (this.reverseDoorBlockerDirection)
		{
			num *= -1f;
		}
		this.doorBlocker.transform.localPosition = new Vector3(-x, 0f, 0f);
		this.doorBlocker.transform.localEulerAngles = new Vector3(0f, 0f, (180f - num) / 2f);
		float num2 = 2f * x * Mathf.Sin(0.017453292f * num / 2f);
		Vector3 size = this.doorBlocker.size;
		Vector3 center = this.doorBlocker.center;
		size.x = num2;
		center.x = num2 / 2f;
		this.doorBlocker.size = size;
		this.doorBlocker.center = center;
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x0002AA64 File Offset: 0x00028C64
	private void DoorBlockerUpdate()
	{
		if (!this.IsMoving)
		{
			if (this.doorBlocker.enabled)
			{
				this.doorBlocker.enabled = false;
			}
			return;
		}
		if (!this.doorBlocker.enabled)
		{
			this.doorBlocker.enabled = true;
		}
		if (this.doorBlockerMode == RotatingDoor.DoorBlockerMode.DynamicAndEnabledOnMoving)
		{
			this.UpdateDoorBlockerColliderDymanicPosition();
		}
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x0002AABC File Offset: 0x00028CBC
	private void PlaySound(RotatingDoor.Sounds sound)
	{
		if (!this.source)
		{
			return;
		}
		switch (sound)
		{
		case RotatingDoor.Sounds.Open:
			if (this.openClips.Count > 0)
			{
				this.source.pitch = this.openPitchRange.Random();
				this.source.volume = this.openVol;
				this.source.PlayOneShot(this.openClips[Random.Range(0, this.openClips.Count)]);
				return;
			}
			break;
		case RotatingDoor.Sounds.Close:
			if (this.closeClips.Count > 0)
			{
				this.source.pitch = this.closePitchRange.Random();
				this.source.volume = this.closeVol;
				this.source.PlayOneShot(this.closeClips[Random.Range(0, this.closeClips.Count)]);
				return;
			}
			break;
		case RotatingDoor.Sounds.Move:
			if (this.moveClips.Count > 0)
			{
				this.source.pitch = this.movePitchRange.Random();
				this.source.volume = this.moveVol;
				this.source.PlayOneShot(this.moveClips[Random.Range(0, this.moveClips.Count)]);
				return;
			}
			break;
		case RotatingDoor.Sounds.Locked:
			if (this.lockedClips.Count > 0)
			{
				this.source.pitch = this.lockedPitchRange.Random();
				this.source.volume = this.lockedVol;
				this.source.PlayOneShot(this.lockedClips[Random.Range(0, this.lockedClips.Count)]);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x0002AC6A File Offset: 0x00028E6A
	private void FixedUpdate()
	{
		if (this.hasDoorBlocker)
		{
			this.DoorBlockerUpdate();
		}
		this.touchingPlayer = false;
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x0002AC81 File Offset: 0x00028E81
	private void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Player")
		{
			this.touchingPlayer = true;
		}
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x0002ACA1 File Offset: 0x00028EA1
	private void OnCollisionStay(Collision col)
	{
		if (col.gameObject.CompareTag("Player"))
		{
			this.touchingPlayer = true;
		}
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x0002ACBC File Offset: 0x00028EBC
	public void Input_EnableDraw()
	{
		this._renderer.enabled = true;
		this._collider.enabled = true;
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x0002ACD6 File Offset: 0x00028ED6
	public void Input_DisableDraw()
	{
		this._renderer.enabled = false;
		this._collider.enabled = false;
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x0002ACF0 File Offset: 0x00028EF0
	public override void Use()
	{
		if (this.usable)
		{
			if (this.isLocked)
			{
				base.FireOutput(Outputs.OnLockedUse);
				this.PlaySound(RotatingDoor.Sounds.Locked);
			}
			this.Input_Toggle();
		}
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x0002AD18 File Offset: 0x00028F18
	public void Input_Toggle()
	{
		if (this.isLocked)
		{
			return;
		}
		if (this.targetAngle != this.currentAngle)
		{
			return;
		}
		if (Mathf.Abs(this.targetAngle) == this.maxAngle)
		{
			this.Input_Close();
			return;
		}
		if (this.targetAngle == 0f)
		{
			this.Input_Open();
		}
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x0002AD6C File Offset: 0x00028F6C
	public void Input_Open()
	{
		if (this.isLocked)
		{
			return;
		}
		base.FireOutput(Outputs.OnOpen);
		if (this.directions == RotatingDoor.DoorDirections.BackwardOnly)
		{
			this.targetAngle = -this.maxAngle;
		}
		else
		{
			this.targetAngle = this.maxAngle;
		}
		this.PlaySound(RotatingDoor.Sounds.Move);
		this.targetAngle *= this.dirChange;
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x0002ADC8 File Offset: 0x00028FC8
	public void Input_Close()
	{
		this.targetAngle = 0f;
		base.FireOutput(Outputs.OnClose);
		if (Mathf.Abs(this.currentAngle) == 90f)
		{
			this.PlaySound(RotatingDoor.Sounds.Move);
		}
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x0002ADF6 File Offset: 0x00028FF6
	public void Input_Lock()
	{
		this.isLocked = true;
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x0002ADFF File Offset: 0x00028FFF
	public void Input_Unlock()
	{
		this.isLocked = false;
	}

	// Token: 0x040008B7 RID: 2231
	private MeshRenderer _renderer;

	// Token: 0x040008B8 RID: 2232
	private BoxCollider _collider;

	// Token: 0x040008B9 RID: 2233
	public bool isDrawing = true;

	// Token: 0x040008BA RID: 2234
	public float spawnAngle;

	// Token: 0x040008BB RID: 2235
	private float currentAngle;

	// Token: 0x040008BC RID: 2236
	private float targetAngle;

	// Token: 0x040008BD RID: 2237
	public float maxAngle = 90f;

	// Token: 0x040008BE RID: 2238
	public bool reverseDirection;

	// Token: 0x040008BF RID: 2239
	public bool brushAxisFixup;

	// Token: 0x040008C0 RID: 2240
	private float dirChange = 1f;

	// Token: 0x040008C1 RID: 2241
	public Vector3 axis = Vector3.up;

	// Token: 0x040008C2 RID: 2242
	public float speed = 120f;

	// Token: 0x040008C3 RID: 2243
	public bool isLocked;

	// Token: 0x040008C4 RID: 2244
	public RotatingDoor.DoorDirections directions;

	// Token: 0x040008C5 RID: 2245
	public Color renderColor = Color.white;

	// Token: 0x040008C6 RID: 2246
	public bool usable;

	// Token: 0x040008C7 RID: 2247
	private bool touchingPlayer;

	// Token: 0x040008C8 RID: 2248
	private Quaternion closedRotation;

	// Token: 0x040008C9 RID: 2249
	private Quaternion openRotation;

	// Token: 0x040008CA RID: 2250
	public List<AudioClip> openClips;

	// Token: 0x040008CB RID: 2251
	public float openVol = 1f;

	// Token: 0x040008CC RID: 2252
	public MinMax openPitchRange;

	// Token: 0x040008CD RID: 2253
	public List<AudioClip> closeClips;

	// Token: 0x040008CE RID: 2254
	public float closeVol = 1f;

	// Token: 0x040008CF RID: 2255
	public MinMax closePitchRange;

	// Token: 0x040008D0 RID: 2256
	public List<AudioClip> moveClips;

	// Token: 0x040008D1 RID: 2257
	public float moveVol = 1f;

	// Token: 0x040008D2 RID: 2258
	public MinMax movePitchRange;

	// Token: 0x040008D3 RID: 2259
	public List<AudioClip> lockedClips;

	// Token: 0x040008D4 RID: 2260
	public float lockedVol = 1f;

	// Token: 0x040008D5 RID: 2261
	public MinMax lockedPitchRange;

	// Token: 0x040008D7 RID: 2263
	private AudioSource source;

	// Token: 0x040008D8 RID: 2264
	[Header("Null for no blocker, must be a direct child of this class")]
	public BoxCollider doorBlocker;

	// Token: 0x040008D9 RID: 2265
	private bool hasDoorBlocker;

	// Token: 0x040008DA RID: 2266
	public RotatingDoor.DoorBlockerMode doorBlockerMode;

	// Token: 0x040008DB RID: 2267
	[InspectorButton("MIGRATION_PerformMeshColliderSwapWithBoxCollider", null)]
	public bool reverseDoorBlockerDirection;

	// Token: 0x020003E6 RID: 998
	public enum DoorDirections
	{
		// Token: 0x04001460 RID: 5216
		Both,
		// Token: 0x04001461 RID: 5217
		ForwardOnly,
		// Token: 0x04001462 RID: 5218
		BackwardOnly
	}

	// Token: 0x020003E7 RID: 999
	public enum DoorBlockerMode
	{
		// Token: 0x04001464 RID: 5220
		StaticAndEnabledOnMoving,
		// Token: 0x04001465 RID: 5221
		DynamicAndEnabledOnMoving
	}

	// Token: 0x020003E8 RID: 1000
	private enum Sounds
	{
		// Token: 0x04001467 RID: 5223
		Open,
		// Token: 0x04001468 RID: 5224
		Close,
		// Token: 0x04001469 RID: 5225
		Move,
		// Token: 0x0400146A RID: 5226
		Locked
	}
}
