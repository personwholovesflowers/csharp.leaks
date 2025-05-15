using System;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020002F4 RID: 756
public class MinosArm : MonoBehaviour
{
	// Token: 0x060010BD RID: 4285 RVA: 0x00080EF2 File Offset: 0x0007F0F2
	private void Start()
	{
		this.stat = base.GetComponent<Statue>();
		this.originalHealth = this.stat.health;
		this.SetSpeed();
	}

	// Token: 0x060010BE RID: 4286 RVA: 0x00080F17 File Offset: 0x0007F117
	private void UpdateBuff()
	{
		this.SetSpeed();
	}

	// Token: 0x060010BF RID: 4287 RVA: 0x00080F20 File Offset: 0x0007F120
	private void SetSpeed()
	{
		if (!this.anim)
		{
			this.anim = base.GetComponent<Animator>();
		}
		if (!this.eid)
		{
			this.eid = base.GetComponent<EnemyIdentifier>();
		}
		if (this.difficulty < 0)
		{
			if (this.eid.difficultyOverride >= 0)
			{
				this.difficulty = this.eid.difficultyOverride;
			}
			else
			{
				this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
			}
		}
		if (this.difficulty == 4)
		{
			this.maxSlams = 99;
		}
		else if (this.difficulty == 3)
		{
			this.maxSlams = 3;
		}
		if (this.difficulty == 1)
		{
			this.originalAnimSpeed = 0.85f;
		}
		else if (this.difficulty == 0)
		{
			this.originalAnimSpeed = 0.65f;
		}
		else
		{
			this.originalAnimSpeed = 1f;
		}
		this.originalAnimSpeed *= this.eid.totalSpeedModifier;
		this.anim.speed = this.originalAnimSpeed * (1f + this.speedState / 4f);
	}

	// Token: 0x060010C0 RID: 4288 RVA: 0x00081034 File Offset: 0x0007F234
	private void Update()
	{
		if (this.introOver)
		{
			if (BlindEnemies.Blind || (EnemyIgnorePlayer.Active && this.eid.target == null))
			{
				return;
			}
			if (this.attackCooldown > 0f)
			{
				this.attackCooldown = Mathf.MoveTowards(this.attackCooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
			}
			else if (!this.inAction)
			{
				bool flag = false;
				while (!flag)
				{
					int num = Random.Range(0, 3);
					if (num != this.previousSlam)
					{
						this.previousSlam = num;
						flag = true;
						switch (num)
						{
						case 0:
							this.SlamDown();
							break;
						case 1:
							this.SlamLeft();
							break;
						case 2:
							this.SlamRight();
							break;
						}
						this.currentSlams++;
						if (this.currentSlams >= this.maxSlams)
						{
							this.currentSlams = 0;
							this.attackCooldown = 5f - this.speedState;
						}
					}
				}
			}
		}
		if (this.shaking)
		{
			MonoSingleton<CameraController>.Instance.CameraShake(0.25f);
		}
		if (this.speedState == 1f && this.stat.health < this.originalHealth * 0.4f)
		{
			this.speedState = 2f;
			this.anim.speed = this.originalAnimSpeed * 1.5f;
			this.Flinch();
			return;
		}
		if (this.speedState == 0f && this.stat.health < this.originalHealth * 0.75f)
		{
			this.speedState = 1f;
			this.anim.speed = this.originalAnimSpeed * 1.25f;
			this.Flinch();
		}
	}

	// Token: 0x060010C1 RID: 4289 RVA: 0x000811E0 File Offset: 0x0007F3E0
	private void SlamLeft()
	{
		this.anim.SetTrigger("SlamLeft");
		this.inAction = true;
	}

	// Token: 0x060010C2 RID: 4290 RVA: 0x000811F9 File Offset: 0x0007F3F9
	private void SlamRight()
	{
		this.anim.SetTrigger("SlamRight");
		this.inAction = true;
	}

	// Token: 0x060010C3 RID: 4291 RVA: 0x00081212 File Offset: 0x0007F412
	private void SlamDown()
	{
		this.anim.SetTrigger("SlamDown");
		this.inAction = true;
	}

	// Token: 0x060010C4 RID: 4292 RVA: 0x0008122C File Offset: 0x0007F42C
	public void Slam(int type)
	{
		Vector3 vector = Vector3.down;
		if (type == 1)
		{
			vector = base.transform.right;
		}
		else if (type == 2)
		{
			vector = base.transform.right * -1f;
		}
		Vector3 vector2 = this.hand.position;
		RaycastHit raycastHit;
		if (Physics.Raycast(this.hand.position, vector, out raycastHit, 100f, LayerMaskDefaults.Get(LMD.Environment)))
		{
			vector2 = raycastHit.point - vector * 0.1f;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.slamWave, vector2, Quaternion.identity);
		GoreZone componentInParent = base.GetComponentInParent<GoreZone>();
		if (componentInParent)
		{
			gameObject.transform.SetParent(componentInParent.transform, true);
		}
		else
		{
			gameObject.transform.SetParent(base.transform, true);
		}
		float num = 3f;
		if (type > 0)
		{
			num = 7f;
		}
		gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y * num, gameObject.transform.localScale.z);
		PhysicalShockwave component = gameObject.GetComponent<PhysicalShockwave>();
		if (component)
		{
			if (this.difficulty == 1)
			{
				component.speed = 60f;
			}
			else if (this.difficulty == 0)
			{
				component.speed = 30f;
			}
			else
			{
				component.speed = 90f;
			}
			component.damage = Mathf.RoundToInt(35f * this.eid.totalDamageModifier);
			component.maxSize = 350f;
		}
		if (type > 0)
		{
			gameObject.transform.Rotate(base.transform.forward * 90f);
		}
		this.BigImpact(2f);
	}

	// Token: 0x060010C5 RID: 4293 RVA: 0x000813F8 File Offset: 0x0007F5F8
	public void BigImpact(float shakeAmount = 2f)
	{
		MonoSingleton<CameraController>.Instance.CameraShake(2f);
		this.rubbleSpawner.SpawnObject(0);
		if (shakeAmount != 2f)
		{
			Object.Instantiate<GameObject>(this.impactSound, base.transform.position, base.transform.rotation);
		}
	}

	// Token: 0x060010C6 RID: 4294 RVA: 0x0008144C File Offset: 0x0007F64C
	private void Flinch()
	{
		this.anim.SetTrigger("Flinch");
		this.inAction = true;
		this.currentSlams = 0;
		this.maxSlams++;
		this.attackCooldown = 0f;
		if (!this.introOver)
		{
			if (this.shaking)
			{
				this.StopShaking();
			}
			this.StartEncounter();
			this.IntroEnd();
		}
		Object.Instantiate<GameObject>(this.hurtSound, base.transform.position, Quaternion.identity);
	}

	// Token: 0x060010C7 RID: 4295 RVA: 0x000814CE File Offset: 0x0007F6CE
	public void Retreat()
	{
		this.anim.SetBool("Retreat", true);
		this.inAction = true;
		this.StartShaking();
		Object.Instantiate<GameObject>(this.bigHurtSound, base.transform.position, Quaternion.identity);
	}

	// Token: 0x060010C8 RID: 4296 RVA: 0x0008150A File Offset: 0x0007F70A
	public void EndEncounter()
	{
		this.StopShaking();
		this.encounterEnd.Invoke();
		DoubleRender componentInChildren = base.GetComponentInChildren<DoubleRender>();
		if (componentInChildren == null)
		{
			return;
		}
		componentInChildren.RemoveEffect();
	}

	// Token: 0x060010C9 RID: 4297 RVA: 0x0008152D File Offset: 0x0007F72D
	public void IntroEnd()
	{
		this.introOver = true;
	}

	// Token: 0x060010CA RID: 4298 RVA: 0x00081536 File Offset: 0x0007F736
	public void StopAction()
	{
		this.inAction = false;
	}

	// Token: 0x060010CB RID: 4299 RVA: 0x0008153F File Offset: 0x0007F73F
	public void StartShaking()
	{
		this.shaking = true;
		this.rubbleSpawner.SpawnObject(0);
		this.shakeEffect.SetActive(true);
	}

	// Token: 0x060010CC RID: 4300 RVA: 0x00081560 File Offset: 0x0007F760
	public void StopShaking()
	{
		this.shaking = false;
		this.shakeEffect.SetActive(false);
		this.BigImpact(1f);
	}

	// Token: 0x060010CD RID: 4301 RVA: 0x00081580 File Offset: 0x0007F780
	public void StartEncounter()
	{
		this.encounterStart.Invoke();
	}

	// Token: 0x040016D1 RID: 5841
	private bool introOver;

	// Token: 0x040016D2 RID: 5842
	private float attackCooldown = 1.5f;

	// Token: 0x040016D3 RID: 5843
	private bool inAction;

	// Token: 0x040016D4 RID: 5844
	private Animator anim;

	// Token: 0x040016D5 RID: 5845
	private int previousSlam;

	// Token: 0x040016D6 RID: 5846
	private int maxSlams = 2;

	// Token: 0x040016D7 RID: 5847
	private int currentSlams;

	// Token: 0x040016D8 RID: 5848
	public Transform hand;

	// Token: 0x040016D9 RID: 5849
	public GameObject slamWave;

	// Token: 0x040016DA RID: 5850
	public ObjectSpawner rubbleSpawner;

	// Token: 0x040016DB RID: 5851
	private bool shaking;

	// Token: 0x040016DC RID: 5852
	public GameObject shakeEffect;

	// Token: 0x040016DD RID: 5853
	public GameObject impactSound;

	// Token: 0x040016DE RID: 5854
	public GameObject hurtSound;

	// Token: 0x040016DF RID: 5855
	public GameObject bigHurtSound;

	// Token: 0x040016E0 RID: 5856
	private Statue stat;

	// Token: 0x040016E1 RID: 5857
	private float originalHealth;

	// Token: 0x040016E2 RID: 5858
	private float speedState;

	// Token: 0x040016E3 RID: 5859
	public UnityEvent encounterStart;

	// Token: 0x040016E4 RID: 5860
	public UnityEvent encounterEnd;

	// Token: 0x040016E5 RID: 5861
	private int difficulty = -1;

	// Token: 0x040016E6 RID: 5862
	private float originalAnimSpeed = 1f;

	// Token: 0x040016E7 RID: 5863
	private EnemyIdentifier eid;
}
