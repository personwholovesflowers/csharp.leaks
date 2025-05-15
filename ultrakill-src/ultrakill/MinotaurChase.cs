using System;
using UnityEngine;

// Token: 0x020002F9 RID: 761
public class MinotaurChase : MonoBehaviour
{
	// Token: 0x06001144 RID: 4420 RVA: 0x000870A5 File Offset: 0x000852A5
	private void Start()
	{
		this.GetValues();
		this.trackTarget = !this.intro;
		if (this.intro)
		{
			this.eid.totalDamageTakenMultiplier = 0f;
			this.QuickHammer();
			return;
		}
		this.IntroEnd();
	}

	// Token: 0x06001145 RID: 4421 RVA: 0x000870E4 File Offset: 0x000852E4
	private void GetValues()
	{
		if (this.gotValues)
		{
			return;
		}
		this.gotValues = true;
		this.rb = base.GetComponent<Rigidbody>();
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.anim = base.GetComponent<Animator>();
		if (this.eid.difficultyOverride >= 0)
		{
			this.difficulty = this.eid.difficultyOverride;
		}
		else
		{
			this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		}
		this.SetSpeed();
	}

	// Token: 0x06001146 RID: 4422 RVA: 0x00087162 File Offset: 0x00085362
	private void UpdateBuff()
	{
		this.SetSpeed();
	}

	// Token: 0x06001147 RID: 4423 RVA: 0x0008716C File Offset: 0x0008536C
	private void SetSpeed()
	{
		this.GetValues();
		if (this.difficulty >= 4)
		{
			this.movementSpeed = 35f;
			this.anim.speed = 1.2f;
		}
		else if (this.difficulty == 3)
		{
			this.movementSpeed = 30f;
			this.anim.speed = 1f;
		}
		else if (this.difficulty == 2)
		{
			this.movementSpeed = 25f;
			this.anim.speed = 1f;
		}
		else if (this.difficulty == 1)
		{
			this.movementSpeed = 20f;
			this.anim.speed = 0.9f;
		}
		else
		{
			this.movementSpeed = 10f;
			this.anim.speed = 0.8f;
		}
		this.movementSpeed *= this.eid.totalSpeedModifier;
		this.anim.speed *= this.eid.totalSpeedModifier;
	}

	// Token: 0x06001148 RID: 4424 RVA: 0x0008726C File Offset: 0x0008546C
	private void Update()
	{
		if (this.trackTarget && (this.eid.target != null || this.tempTarget != null))
		{
			Transform transform = (this.tempTarget ? this.tempTarget : this.eid.target.targetTransform);
			Quaternion quaternion = Quaternion.LookRotation(new Vector3(transform.position.x, base.transform.position.y, transform.position.z) - base.transform.position);
			this.rb.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, (20f + Quaternion.Angle(base.transform.rotation, quaternion)) * ((this.attacking && this.difficulty <= 2) ? 0.5f : 1f) * Time.deltaTime);
			float num = ((base.transform.localRotation.eulerAngles.y > 180f) ? (base.transform.localRotation.eulerAngles.y - 360f) : base.transform.localRotation.eulerAngles.y);
			if (Mathf.Abs(num) > 66f)
			{
				base.transform.localRotation = Quaternion.Euler(0f, Mathf.Clamp(num, -66f, 66f), 0f);
			}
		}
		if (!this.attacking && Mathf.Abs(base.transform.position.z - this.leashPosition.z) < 10f)
		{
			if (this.eid.target != null && ((this.eid.target.position.y > base.transform.position.y + 3f && Vector3.Distance(base.transform.position, new Vector3(this.eid.target.position.x, base.transform.position.y, this.eid.target.position.z)) < 8f) || this.eid.target.position.z > base.transform.position.z))
			{
				if (!this.backAttacking)
				{
					this.backAttacking = true;
					this.HandSwingStart();
					base.Invoke("HandSwinging", 0.5f);
					this.handSwingCheck.CanHitPlayerMultipleTimes(true);
					this.anim.SetBool("BackAttack", true);
				}
			}
			else if (this.backAttacking)
			{
				this.backAttacking = false;
				this.HandSwingStop();
				this.handSwingCheck.CanHitPlayerMultipleTimes(false);
				this.anim.SetBool("BackAttack", false);
			}
			if (this.cooldown <= 0f && !this.backAttacking)
			{
				if (this.currentAttacks >= 2)
				{
					this.HammerSwing();
					this.currentAttacks = 0;
					this.previousAttack = -1;
					this.cooldown = (float)((this.difficulty <= 2) ? 2 : 1);
				}
				else
				{
					int num2 = Random.Range(0, 2);
					if (num2 == this.previousAttack)
					{
						num2++;
					}
					if (num2 >= 2)
					{
						num2 = 0;
					}
					if (num2 != 0)
					{
						if (num2 == 1)
						{
							this.HandSwing();
						}
					}
					else
					{
						this.MeatThrow();
					}
					this.cooldown = (float)((this.difficulty <= 2) ? 2 : 1);
					this.previousAttack = num2;
					this.currentAttacks++;
				}
			}
		}
		if (!this.attacking && this.cooldown > 0f)
		{
			this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.deltaTime);
		}
		if (!this.dead)
		{
			this.currentAnimatorWeight = Mathf.MoveTowards(this.currentAnimatorWeight, (float)((this.attacking || this.backAttacking) ? 1 : 0), Time.deltaTime * 3f);
			this.anim.SetLayerWeight(1, this.currentAnimatorWeight);
		}
	}

	// Token: 0x06001149 RID: 4425 RVA: 0x00087684 File Offset: 0x00085884
	private void FixedUpdate()
	{
		if (this.trackTarget && this.eid.target != null)
		{
			float num = Mathf.MoveTowards(base.transform.position.z, this.leashPosition.z + this.leashRandomizer, Mathf.Min(Mathf.Abs(base.transform.position.z - (this.leashPosition.z + this.leashRandomizer)) * 2f + 0.5f, this.movementSpeed) * 1.5f * Time.fixedDeltaTime);
			Vector3 vector = new Vector3(Mathf.Clamp((base.transform.position + base.transform.forward * this.movementSpeed * Time.fixedDeltaTime).x, this.leashPosition.x - this.movementRange, this.leashPosition.x + this.movementRange), base.transform.position.y, num);
			this.rb.MovePosition(Vector3.MoveTowards(base.transform.position, vector, this.movementSpeed * Time.fixedDeltaTime));
			this.anim.SetFloat("RunSpeed", 1f + Mathf.Min(Mathf.Abs(num - this.leashPosition.z) * 2f, this.movementSpeed) / this.movementSpeed / 3f);
			if (Mathf.Abs(base.transform.position.z - (this.leashPosition.z + this.leashRandomizer)) < 0.01f)
			{
				this.leashRandomizer *= -1f;
			}
		}
		if (this.dragging)
		{
			this.rb.MovePosition(Vector3.MoveTowards(base.transform.position, base.transform.position + Vector3.forward * 100f, this.movementSpeed * 1.5f * Time.fixedDeltaTime));
		}
	}

	// Token: 0x0600114A RID: 4426 RVA: 0x00087898 File Offset: 0x00085A98
	private void MeatThrow()
	{
		this.anim.Play("MeatThrow", 1, 0f);
		this.Roar(this.longGruntClip, 1f);
		this.attacking = true;
	}

	// Token: 0x0600114B RID: 4427 RVA: 0x000878C8 File Offset: 0x00085AC8
	private void HandBlood()
	{
		Object.Instantiate<GameObject>(this.handBlood, this.meatInHand.transform.position, Quaternion.identity);
	}

	// Token: 0x0600114C RID: 4428 RVA: 0x000878EB File Offset: 0x00085AEB
	private void MeatSpawn()
	{
		this.meatInHand.SetActive(true);
		this.HandBlood();
	}

	// Token: 0x0600114D RID: 4429 RVA: 0x000878FF File Offset: 0x00085AFF
	private void MeatThrowPickTarget()
	{
		this.tempTarget = this.GetClosestTram(this.eid.target.position, float.PositiveInfinity).transform;
	}

	// Token: 0x0600114E RID: 4430 RVA: 0x00087928 File Offset: 0x00085B28
	private void MeatThrowThrow()
	{
		this.meatInHand.SetActive(false);
		GameObject closestTram = this.GetClosestTram(base.transform.position, float.PositiveInfinity);
		if (closestTram != null)
		{
			ObjectSpawner componentInChildren = closestTram.GetComponentInChildren<ObjectSpawner>(true);
			if (componentInChildren)
			{
				componentInChildren.SpawnObject(1);
			}
		}
	}

	// Token: 0x0600114F RID: 4431 RVA: 0x00087978 File Offset: 0x00085B78
	private void HandSwing()
	{
		this.anim.Play("HandSwing", 1, 0f);
		this.attacking = true;
		this.Roar(this.shortRoarClip, 0.75f);
		Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.unparryableFlash, this.meatInHand.transform.position, Quaternion.identity).transform.localScale *= 25f;
	}

	// Token: 0x06001150 RID: 4432 RVA: 0x000879F1 File Offset: 0x00085BF1
	private void HandSwingStart()
	{
		this.handSwingStuff.SetActive(true);
		if (this.difficulty == 0)
		{
			this.trackTarget = false;
		}
		this.HandBlood();
	}

	// Token: 0x06001151 RID: 4433 RVA: 0x00087A14 File Offset: 0x00085C14
	private void HandSwinging()
	{
		this.handSwingCheck.DamageStart();
	}

	// Token: 0x06001152 RID: 4434 RVA: 0x00087A21 File Offset: 0x00085C21
	private void HandSwingStop()
	{
		this.handSwingStuff.SetActive(false);
		this.handSwingCheck.DamageStop();
		this.trackTarget = true;
		this.HandBlood();
	}

	// Token: 0x06001153 RID: 4435 RVA: 0x00087A48 File Offset: 0x00085C48
	private void HammerSwing()
	{
		this.anim.Play("HammerSwing", 1, 0f);
		this.attacking = true;
		Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.unparryableFlash, this.hammerTrail.transform.position, Quaternion.identity).transform.localScale *= 25f;
		this.Roar();
	}

	// Token: 0x06001154 RID: 4436 RVA: 0x00087AB6 File Offset: 0x00085CB6
	private void QuickHammer()
	{
		this.anim.Play("HammerSwing", 1, 0.4f);
		this.attacking = true;
	}

	// Token: 0x06001155 RID: 4437 RVA: 0x00087AD5 File Offset: 0x00085CD5
	private void HammerSwingStart()
	{
		this.hammerSwingCheck.DamageStart();
		this.hammerTrail.emitting = true;
	}

	// Token: 0x06001156 RID: 4438 RVA: 0x00087AF0 File Offset: 0x00085CF0
	private void HammerImpact()
	{
		this.trackTarget = false;
		Explosion[] componentsInChildren = Object.Instantiate<GameObject>(this.hammerExplosion, new Vector3(this.hammerPoint.position.x, base.transform.position.y, this.hammerPoint.position.z), Quaternion.identity).GetComponentsInChildren<Explosion>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].toIgnore.Add(EnemyType.Minotaur);
		}
		MonoSingleton<CameraController>.Instance.CameraShake(1.5f);
		if (this.intro)
		{
			this.IntroEnd();
			return;
		}
		GameObject closestTram = this.GetClosestTram(this.hammerPoint.position, 10f);
		if (closestTram != null)
		{
			Harpoon[] componentsInChildren2 = closestTram.GetComponentsInChildren<Harpoon>();
			if (componentsInChildren2 != null && componentsInChildren2.Length != 0)
			{
				foreach (Harpoon harpoon in componentsInChildren2)
				{
					if (harpoon.gameObject.activeInHierarchy)
					{
						TimeBomb componentInChildren = harpoon.GetComponentInChildren<TimeBomb>();
						if (componentInChildren)
						{
							componentInChildren.dontExplode = true;
						}
						Object.Destroy(harpoon.gameObject);
					}
				}
			}
			closestTram.SetActive(false);
			closestTram.transform.position += closestTram.transform.forward * 200f;
			if (this.difficulty >= 4)
			{
				MonoSingleton<DelayedActivationManager>.Instance.Add(closestTram, 7f);
			}
			else if (this.difficulty >= 2)
			{
				MonoSingleton<DelayedActivationManager>.Instance.Add(closestTram, 5f);
			}
			else if (this.difficulty == 1)
			{
				MonoSingleton<DelayedActivationManager>.Instance.Add(closestTram, 3f);
			}
			else
			{
				MonoSingleton<DelayedActivationManager>.Instance.Add(closestTram, 1f);
			}
			ObjectSpawner componentInChildren2 = closestTram.GetComponentInChildren<ObjectSpawner>(true);
			if (componentInChildren2)
			{
				componentInChildren2.SpawnObject(0);
			}
		}
	}

	// Token: 0x06001157 RID: 4439 RVA: 0x00087CB4 File Offset: 0x00085EB4
	private void HammerSwingStop()
	{
		this.hammerSwingCheck.DamageStop();
		this.hammerTrail.emitting = false;
		if (this.difficulty <= 1)
		{
			this.trackTarget = false;
		}
	}

	// Token: 0x06001158 RID: 4440 RVA: 0x00087CE0 File Offset: 0x00085EE0
	public void Death()
	{
		this.dead = true;
		this.HammerSwingStop();
		this.handSwingStuff.SetActive(false);
		this.handSwingCheck.DamageStop();
		this.meatInHand.SetActive(false);
		this.anim.SetBool("BackAttack", false);
		this.anim.SetFloat("RunSpeed", 2.5f);
		this.anim.SetLayerWeight(1, 1f);
		this.anim.Play("Death", 1, 0f);
		this.attacking = true;
		this.trackTarget = false;
		this.Roar(this.squealClip, 1f);
		MonoSingleton<TimeController>.Instance.SlowDown(0.001f);
	}

	// Token: 0x06001159 RID: 4441 RVA: 0x00087D98 File Offset: 0x00085F98
	private void GetDragged()
	{
		if (this.dragging)
		{
			return;
		}
		this.dragging = true;
		this.anim.Play("Death", 0, this.anim.GetCurrentAnimatorStateInfo(1).normalizedTime);
		this.anim.SetLayerWeight(1, 0f);
		Object.Instantiate<GameObject>(this.fallEffect, base.transform.position, Quaternion.identity);
		MonoSingleton<CameraController>.Instance.CameraShake(3f);
		EnemySimplifier componentInChildren = base.GetComponentInChildren<EnemySimplifier>();
		if (componentInChildren)
		{
			componentInChildren.ChangeMaterialNew(EnemySimplifier.MaterialState.normal, this.hurtMaterial);
		}
		else
		{
			ChangeMaterials componentInChildren2 = base.GetComponentInChildren<ChangeMaterials>();
			if (componentInChildren2)
			{
				componentInChildren2.Activate();
			}
		}
		base.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = this.hurtMesh;
	}

	// Token: 0x0600115A RID: 4442 RVA: 0x00087E5A File Offset: 0x0008605A
	public void StopDragging()
	{
		this.dragging = false;
	}

	// Token: 0x0600115B RID: 4443 RVA: 0x00087E63 File Offset: 0x00086063
	private void StartTracking()
	{
		this.trackTarget = true;
	}

	// Token: 0x0600115C RID: 4444 RVA: 0x00087E6C File Offset: 0x0008606C
	private void ResetTarget()
	{
		this.tempTarget = null;
	}

	// Token: 0x0600115D RID: 4445 RVA: 0x00087E75 File Offset: 0x00086075
	private void StopAction()
	{
		this.attacking = false;
		this.trackTarget = true;
	}

	// Token: 0x0600115E RID: 4446 RVA: 0x00087E85 File Offset: 0x00086085
	private void IntroEnd()
	{
		UltrakillEvent ultrakillEvent = this.onIntroEnd;
		if (ultrakillEvent != null)
		{
			ultrakillEvent.Invoke("");
		}
		this.eid.totalDamageTakenMultiplier = 1f;
		this.intro = false;
	}

	// Token: 0x0600115F RID: 4447 RVA: 0x00087EB4 File Offset: 0x000860B4
	public void DisableIntro()
	{
		this.intro = false;
	}

	// Token: 0x06001160 RID: 4448 RVA: 0x00087EC0 File Offset: 0x000860C0
	private GameObject GetClosestTram(Vector3 position, float shortestDistance = float.PositiveInfinity)
	{
		GameObject gameObject = null;
		for (int i = 0; i < this.trams.Length; i++)
		{
			float num = Vector3.Distance(this.trams[i].transform.position, position);
			if (num < shortestDistance)
			{
				shortestDistance = num;
				gameObject = this.trams[i];
			}
		}
		return gameObject;
	}

	// Token: 0x06001161 RID: 4449 RVA: 0x00087F0C File Offset: 0x0008610C
	private void Roar()
	{
		this.Roar(this.roarClip, 1f);
	}

	// Token: 0x06001162 RID: 4450 RVA: 0x00087F1F File Offset: 0x0008611F
	private void Roar(float pitch = 1f)
	{
		this.Roar(this.roarClip, pitch);
	}

	// Token: 0x06001163 RID: 4451 RVA: 0x00087F2E File Offset: 0x0008612E
	private void Roar(AudioClip clip, float pitch = 1f)
	{
		this.roar.clip = clip;
		this.roar.pitch = Random.Range(pitch - 0.1f, pitch + 0.1f);
		this.roar.Play();
	}

	// Token: 0x0400178F RID: 6031
	private Rigidbody rb;

	// Token: 0x04001790 RID: 6032
	private EnemyIdentifier eid;

	// Token: 0x04001791 RID: 6033
	private Animator anim;

	// Token: 0x04001792 RID: 6034
	private bool gotValues;

	// Token: 0x04001793 RID: 6035
	private bool trackTarget;

	// Token: 0x04001794 RID: 6036
	public float movementRange;

	// Token: 0x04001795 RID: 6037
	public Vector3 leashPosition;

	// Token: 0x04001796 RID: 6038
	private float leashRandomizer = 0.1f;

	// Token: 0x04001797 RID: 6039
	private float movementSpeed;

	// Token: 0x04001798 RID: 6040
	private float currentAnimatorWeight;

	// Token: 0x04001799 RID: 6041
	private float cooldown = 1f;

	// Token: 0x0400179A RID: 6042
	private int previousAttack = -1;

	// Token: 0x0400179B RID: 6043
	private int currentAttacks;

	// Token: 0x0400179C RID: 6044
	[SerializeField]
	private GameObject[] trams;

	// Token: 0x0400179D RID: 6045
	private bool attacking;

	// Token: 0x0400179E RID: 6046
	private bool backAttacking;

	// Token: 0x0400179F RID: 6047
	[SerializeField]
	private SwingCheck2 hammerSwingCheck;

	// Token: 0x040017A0 RID: 6048
	[SerializeField]
	private TrailRenderer hammerTrail;

	// Token: 0x040017A1 RID: 6049
	[SerializeField]
	private Transform hammerPoint;

	// Token: 0x040017A2 RID: 6050
	[SerializeField]
	private GameObject hammerExplosion;

	// Token: 0x040017A3 RID: 6051
	[SerializeField]
	private GameObject meatInHand;

	// Token: 0x040017A4 RID: 6052
	[SerializeField]
	private GameObject handBlood;

	// Token: 0x040017A5 RID: 6053
	[SerializeField]
	private GameObject handSwingStuff;

	// Token: 0x040017A6 RID: 6054
	[SerializeField]
	private SwingCheck2 handSwingCheck;

	// Token: 0x040017A7 RID: 6055
	[SerializeField]
	private GameObject fallEffect;

	// Token: 0x040017A8 RID: 6056
	[SerializeField]
	private AudioSource roar;

	// Token: 0x040017A9 RID: 6057
	[SerializeField]
	private AudioClip roarClip;

	// Token: 0x040017AA RID: 6058
	[SerializeField]
	private AudioClip longGruntClip;

	// Token: 0x040017AB RID: 6059
	[SerializeField]
	private AudioClip shortRoarClip;

	// Token: 0x040017AC RID: 6060
	[SerializeField]
	private AudioClip squealClip;

	// Token: 0x040017AD RID: 6061
	[Header("Intro")]
	public bool intro;

	// Token: 0x040017AE RID: 6062
	public UltrakillEvent onIntroEnd;

	// Token: 0x040017AF RID: 6063
	private Transform tempTarget;

	// Token: 0x040017B0 RID: 6064
	private int difficulty;

	// Token: 0x040017B1 RID: 6065
	private bool dead;

	// Token: 0x040017B2 RID: 6066
	private bool dragging;

	// Token: 0x040017B3 RID: 6067
	public Material hurtMaterial;

	// Token: 0x040017B4 RID: 6068
	public Mesh hurtMesh;
}
