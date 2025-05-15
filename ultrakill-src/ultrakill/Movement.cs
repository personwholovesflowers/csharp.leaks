using System;
using UnityEngine;

// Token: 0x02000305 RID: 773
public class Movement : MonoBehaviour
{
	// Token: 0x06001190 RID: 4496 RVA: 0x000889D4 File Offset: 0x00086BD4
	private void Awake()
	{
		QualitySettings.vSyncCount = 0;
		this.rb = base.GetComponent<Rigidbody>();
		this.aud = base.GetComponent<AudioSource>();
		this.anim = base.GetComponentInChildren<Animator>();
		this.body = GameObject.FindWithTag("Body");
		this.gc = base.GetComponentInChildren<GroundCheck>();
		this.wc = base.GetComponentInChildren<WallCheck>();
		this.aud2 = this.gc.GetComponent<AudioSource>();
		this.pa = base.GetComponentInChildren<PlayerAnimations>();
		this.aud3 = this.wc.GetComponent<AudioSource>();
		this.forwardPoint = GameObject.FindWithTag("Forward");
	}

	// Token: 0x06001191 RID: 4497 RVA: 0x00088A74 File Offset: 0x00086C74
	private void Update()
	{
		float axisRaw = Input.GetAxisRaw("Horizontal");
		float axisRaw2 = Input.GetAxisRaw("Vertical");
		if (this.gc.onGround != this.pa.onGround)
		{
			this.pa.onGround = this.gc.onGround;
		}
		if (!this.gc.onGround && this.rb.velocity.y < -10f)
		{
			this.falling = true;
		}
		if (!this.gc.onGround && this.rb.velocity.y < -20f)
		{
			if (this.rb.velocity.y > -120f)
			{
				this.aud3.pitch = this.rb.velocity.y * -1f / 80f;
			}
			else
			{
				this.aud3.pitch = 1.5f;
			}
			this.aud3.volume = this.rb.velocity.y * -1f / 80f;
		}
		else if (this.rb.velocity.y > -20f)
		{
			this.aud3.pitch = 0f;
			this.aud3.volume = 0f;
		}
		if (this.gc.onGround && this.falling)
		{
			this.falling = false;
			this.aud2.clip = this.landingSound;
			this.aud2.volume = 0.5f;
			this.aud2.Play();
		}
		this.movementDirection = (axisRaw * base.transform.right + axisRaw2 * base.transform.forward).normalized;
		if (this.gc.onGround)
		{
			this.aud.pitch = 1f;
			this.currentWallJumps = 0;
			this.movementDirection = new Vector3(this.movementDirection.x * this.walkSpeed * Time.deltaTime, 0f, this.movementDirection.z * this.walkSpeed * Time.deltaTime);
			this.rb.velocity = this.movementDirection;
			this.anim.SetBool("Run", false);
		}
		else
		{
			this.movementDirection = new Vector3(this.movementDirection.x * this.walkSpeed * Time.deltaTime, this.rb.velocity.y, this.movementDirection.z * this.walkSpeed * Time.deltaTime);
			this.airDirection.y = 0f;
			if ((this.movementDirection.x > 0f && this.rb.velocity.x < this.movementDirection.x) || (this.movementDirection.x < 0f && this.rb.velocity.x > this.movementDirection.x))
			{
				this.airDirection.x = this.movementDirection.x;
			}
			else
			{
				this.airDirection.x = 0f;
			}
			if ((this.movementDirection.z > 0f && this.rb.velocity.z < this.movementDirection.z) || (this.movementDirection.z < 0f && this.rb.velocity.z > this.movementDirection.z))
			{
				this.airDirection.z = this.movementDirection.z;
			}
			else
			{
				this.airDirection.z = 0f;
			}
			this.rb.AddForce(this.airDirection.normalized * 15000f * Time.deltaTime);
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Jump.WasPerformedThisFrame && this.gc.onGround && !this.jumpCooldown)
		{
			this.currentSound = Random.Range(0, this.jumpSounds.Length);
			this.aud.clip = this.jumpSounds[this.currentSound];
			this.aud.volume = 0.75f;
			this.aud.pitch = 1f;
			this.aud.Play();
			this.rb.velocity = new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z);
			this.rb.AddForce(Vector3.up * this.jumpPower * 1000f);
			this.jumpCooldown = true;
			base.Invoke("JumpReady", 0.1f);
		}
		else if (MonoSingleton<InputManager>.Instance.InputSource.Jump.WasPerformedThisFrame && !this.gc.onGround && this.wc.onWall && !this.jumpCooldown && this.currentWallJumps < 3)
		{
			this.currentWallJumps++;
			this.currentSound = Random.Range(0, this.jumpSounds.Length);
			this.aud.clip = this.jumpSounds[this.currentSound];
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
			this.rb.velocity = new Vector3(0f, 0f, 0f);
			this.rb.AddForceAtPosition(this.wallJumpPos.normalized * 10000f, base.transform.position);
			this.rb.velocity = new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z);
			this.jumpCooldown = true;
			base.Invoke("JumpReady", 0.1f);
		}
		if (axisRaw2 < 0f)
		{
			this.forwardPoint.transform.localPosition = new Vector3(axisRaw * -1f, this.body.transform.localPosition.y, axisRaw2 * -1f);
		}
		else if (axisRaw2 == 0f && axisRaw == 0f)
		{
			this.forwardPoint.transform.localPosition = new Vector3(0f, this.body.transform.localPosition.y, 1f);
		}
		else
		{
			this.forwardPoint.transform.localPosition = new Vector3(axisRaw, this.body.transform.localPosition.y, axisRaw2);
		}
		if (axisRaw2 > 0f)
		{
			this.anim.SetBool("WalkF", true);
			this.anim.SetBool("WalkB", false);
		}
		else if (axisRaw2 < 0f)
		{
			this.anim.SetBool("WalkB", true);
			this.anim.SetBool("WalkF", false);
		}
		else if (axisRaw != 0f)
		{
			this.anim.SetBool("WalkF", true);
			this.anim.SetBool("WalkB", false);
		}
		else if (axisRaw == 0f && axisRaw2 == 0f)
		{
			this.anim.SetBool("WalkF", false);
			this.anim.SetBool("WalkB", false);
		}
		this.body.transform.LookAt(this.forwardPoint.transform);
	}

	// Token: 0x06001192 RID: 4498 RVA: 0x00089291 File Offset: 0x00087491
	private void JumpReady()
	{
		this.jumpCooldown = false;
	}

	// Token: 0x040017DD RID: 6109
	public float walkSpeed;

	// Token: 0x040017DE RID: 6110
	public float jumpPower;

	// Token: 0x040017DF RID: 6111
	private bool jumpCooldown;

	// Token: 0x040017E0 RID: 6112
	private bool falling;

	// Token: 0x040017E1 RID: 6113
	private Rigidbody rb;

	// Token: 0x040017E2 RID: 6114
	private Vector3 movementDirection;

	// Token: 0x040017E3 RID: 6115
	private Vector3 airDirection;

	// Token: 0x040017E4 RID: 6116
	public float timeBetweenSteps;

	// Token: 0x040017E5 RID: 6117
	private float stepTime;

	// Token: 0x040017E6 RID: 6118
	private int currentStep;

	// Token: 0x040017E7 RID: 6119
	public Animator anim;

	// Token: 0x040017E8 RID: 6120
	private GameObject body;

	// Token: 0x040017E9 RID: 6121
	private Quaternion tempRotation;

	// Token: 0x040017EA RID: 6122
	private GameObject forwardPoint;

	// Token: 0x040017EB RID: 6123
	private GroundCheck gc;

	// Token: 0x040017EC RID: 6124
	private WallCheck wc;

	// Token: 0x040017ED RID: 6125
	private PlayerAnimations pa;

	// Token: 0x040017EE RID: 6126
	private Vector3 wallJumpPos;

	// Token: 0x040017EF RID: 6127
	private int currentWallJumps;

	// Token: 0x040017F0 RID: 6128
	private AudioSource aud;

	// Token: 0x040017F1 RID: 6129
	private AudioSource aud2;

	// Token: 0x040017F2 RID: 6130
	private AudioSource aud3;

	// Token: 0x040017F3 RID: 6131
	private int currentSound;

	// Token: 0x040017F4 RID: 6132
	public AudioClip[] jumpSounds;

	// Token: 0x040017F5 RID: 6133
	public AudioClip landingSound;

	// Token: 0x040017F6 RID: 6134
	public AudioClip finalWallJump;
}
