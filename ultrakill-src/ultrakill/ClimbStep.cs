using System;
using UnityEngine;

// Token: 0x020000C3 RID: 195
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class ClimbStep : MonoSingleton<ClimbStep>
{
	// Token: 0x060003E0 RID: 992 RVA: 0x00018BAD File Offset: 0x00016DAD
	private new void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.layerMask = LayerMaskDefaults.Get(LMD.Environment);
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x00018BCC File Offset: 0x00016DCC
	private void Start()
	{
		this.newMovement = MonoSingleton<NewMovement>.Instance;
		this.inman = MonoSingleton<InputManager>.Instance;
	}

	// Token: 0x060003E2 RID: 994 RVA: 0x00018BE4 File Offset: 0x00016DE4
	private void FixedUpdate()
	{
		if (this.cooldown <= 0f)
		{
			this.cooldown = 0f;
		}
		else
		{
			this.cooldown -= Time.deltaTime;
		}
		Vector2 vector = MonoSingleton<InputManager>.Instance.InputSource.Move.ReadValue<Vector2>();
		this.movementDirection = Vector3.ClampMagnitude(vector.x * base.transform.right + vector.y * base.transform.forward, 1f);
	}

	// Token: 0x060003E3 RID: 995 RVA: 0x00018C74 File Offset: 0x00016E74
	private void OnCollisionStay(Collision collisionInfo)
	{
		if (MonoSingleton<NewMovement>.Instance.gc.forcedOff > 0)
		{
			return;
		}
		if (this.layerMask == (this.layerMask | (1 << collisionInfo.collider.gameObject.layer)) && this.cooldown == 0f)
		{
			foreach (ContactPoint contactPoint in collisionInfo.contacts)
			{
				if ((this.rb.velocity.y < this.allowedSpeed || this.allowedSpeed == 0f) && this.cooldown == 0f && ((Vector3.Dot(this.movementDirection, -Vector3.ProjectOnPlane(contactPoint.normal, Vector3.up).normalized) > this.allowedInput && !this.newMovement.boost) || (Vector3.Dot(this.newMovement.dodgeDirection, -Vector3.ProjectOnPlane(contactPoint.normal, Vector3.up).normalized) > this.allowedInput && this.newMovement.boost)) && Mathf.Abs(Vector3.Dot(Vector3.up, contactPoint.normal)) < this.allowedAngle)
				{
					this.position = base.transform.position + Vector3.up * this.step + Vector3.up * 0.25f;
					if (this.newMovement.sliding)
					{
						this.position += Vector3.up * 1.125f;
					}
					Collider[] array = Physics.OverlapCapsule(this.position - Vector3.up * this.step, this.position + Vector3.up * 1.25f, 0.499999f, this.layerMask, QueryTriggerInteraction.Ignore);
					Collider[] array2 = Physics.OverlapCapsule(this.position - Vector3.up * 1.25f - Vector3.ProjectOnPlane(contactPoint.normal, Vector3.up) * 0.5f, this.position + Vector3.up * 1.25f - Vector3.ProjectOnPlane(contactPoint.normal, Vector3.up) * 0.5f, 0.5f, this.layerMask, QueryTriggerInteraction.Ignore);
					if (array.Length == 0 && array2.Length == 0)
					{
						this.cooldown = this.cooldownMax;
						Vector3 vector = MonoSingleton<CameraController>.Instance.transform.position;
						float num = 1.75f;
						RaycastHit raycastHit;
						if (Physics.Raycast(this.position - Vector3.up * num - Vector3.ProjectOnPlane(contactPoint.normal, Vector3.up).normalized * this.deltaHorizontal, -Vector3.up, out raycastHit, this.step, this.layerMask, QueryTriggerInteraction.Ignore))
						{
							CustomGroundProperties customGroundProperties;
							if (raycastHit.collider.TryGetComponent<CustomGroundProperties>(out customGroundProperties) && !customGroundProperties.canClimbStep)
							{
								return;
							}
							this.rb.velocity -= new Vector3(0f, this.rb.velocity.y, 0f);
							base.transform.position += Vector3.up * (this.step + this.deltaVertical - raycastHit.distance) - Vector3.ProjectOnPlane(contactPoint.normal, Vector3.up).normalized * this.deltaHorizontal;
							this.rb.velocity = -collisionInfo.relativeVelocity;
						}
						else
						{
							base.transform.position += Vector3.up * (this.step + this.deltaVertical) - Vector3.ProjectOnPlane(contactPoint.normal, Vector3.up).normalized * this.deltaHorizontal;
							this.rb.velocity = -collisionInfo.relativeVelocity;
						}
						MonoSingleton<CameraController>.Instance.transform.position = vector;
						MonoSingleton<CameraController>.Instance.defaultPos = MonoSingleton<CameraController>.Instance.transform.localPosition;
					}
				}
			}
		}
	}

	// Token: 0x040004BD RID: 1213
	private InputManager inman;

	// Token: 0x040004BE RID: 1214
	private Rigidbody rb;

	// Token: 0x040004BF RID: 1215
	private int layerMask;

	// Token: 0x040004C0 RID: 1216
	private NewMovement newMovement;

	// Token: 0x040004C1 RID: 1217
	private float step = 2.1f;

	// Token: 0x040004C2 RID: 1218
	private float allowedAngle = 0.1f;

	// Token: 0x040004C3 RID: 1219
	private float allowedSpeed = 0.1f;

	// Token: 0x040004C4 RID: 1220
	private float allowedInput = 0.5f;

	// Token: 0x040004C5 RID: 1221
	private float cooldown;

	// Token: 0x040004C6 RID: 1222
	private float cooldownMax = 0.1f;

	// Token: 0x040004C7 RID: 1223
	private float deltaVertical;

	// Token: 0x040004C8 RID: 1224
	private float deltaHorizontal = 0.6f;

	// Token: 0x040004C9 RID: 1225
	private Vector3 position;

	// Token: 0x040004CA RID: 1226
	private Vector3 gizmoPosition1;

	// Token: 0x040004CB RID: 1227
	private Vector3 gizmoPosition2;

	// Token: 0x040004CC RID: 1228
	private Vector3 movementDirection;
}
