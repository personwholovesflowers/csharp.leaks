using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020002F5 RID: 757
public class MinosBoss : MonoBehaviour
{
	// Token: 0x060010CF RID: 4303 RVA: 0x000815BC File Offset: 0x0007F7BC
	private void Start()
	{
		this.stat = base.GetComponent<Statue>();
		this.originalHealth = this.stat.health;
		this.scRight = this.rightArm.GetComponentsInChildren<SwingCheck2>();
		this.rightHandBones = this.rightArm.GetComponentsInChildren<Transform>();
		this.scLeft = this.leftArm.GetComponentsInChildren<SwingCheck2>();
		this.leftHandBones = this.leftArm.GetComponentsInChildren<Transform>();
		this.SetSpeed();
	}

	// Token: 0x060010D0 RID: 4304 RVA: 0x00081630 File Offset: 0x0007F830
	private void UpdateBuff()
	{
		this.SetSpeed();
	}

	// Token: 0x060010D1 RID: 4305 RVA: 0x00081638 File Offset: 0x0007F838
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
		if (this.difficulty == 1)
		{
			this.anim.speed = 0.85f;
		}
		else if (this.difficulty == 0)
		{
			this.anim.speed = 0.65f;
		}
		else if (this.difficulty == 4)
		{
			this.anim.speed = 1.25f;
		}
		else if (this.difficulty == 5)
		{
			this.anim.speed = 1.5f;
		}
		else
		{
			this.anim.speed = 1f;
		}
		this.anim.speed *= this.eid.totalSpeedModifier;
		foreach (Parasite parasite in this.parasites)
		{
			parasite.speedMultiplier = this.eid.totalSpeedModifier;
			parasite.damageMultiplier = this.eid.totalDamageModifier;
		}
	}

	// Token: 0x060010D2 RID: 4306 RVA: 0x00081788 File Offset: 0x0007F988
	private void Update()
	{
		if (this.dead && !this.anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
		{
			this.anim.Play("Death");
		}
		if (this.currentBlackHole == null && this.blackHoleCooldown > 0f && (this.phase < 2 || this.difficulty > 2))
		{
			this.blackHoleCooldown = Mathf.MoveTowards(this.blackHoleCooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
		}
		if (this.stat && this.stat.health < this.originalHealth / 2f && this.phase < 2 && !this.anim.IsInTransition(0))
		{
			this.inPhaseChange = true;
			this.PhaseChange(2);
		}
		if (this.eid.target == null)
		{
			return;
		}
		if (!this.inAction && !this.inPhaseChange)
		{
			if (this.currentBlackHole == null && this.blackHoleCooldown == 0f && this.difficulty >= 2 && (this.phase < 2 || this.difficulty > 2))
			{
				this.BlackHole();
				return;
			}
			if (this.cooldown > 0f)
			{
				this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.deltaTime * this.anim.speed);
				return;
			}
			if (!this.anim.IsInTransition(0))
			{
				if (this.phase == 1 && this.difficulty < 4)
				{
					this.cooldown = (float)((this.difficulty >= 4) ? 1 : 2);
				}
				else if (this.phase == 2 || this.difficulty >= 4)
				{
					if ((this.difficulty == 4 && this.punchesSinceBreak < 2) || this.difficulty == 5)
					{
						this.punchesSinceBreak++;
						this.cooldown = 0f;
					}
					else
					{
						this.punchesSinceBreak = 0;
						this.cooldown = 3f;
					}
				}
				else
				{
					this.cooldown = 0f;
				}
				if (this.onRight)
				{
					if (this.onMiddle && Random.Range(0f, 1f) > 0.5f)
					{
						this.SlamMiddle();
						return;
					}
					this.SlamRight();
					return;
				}
				else if (this.onLeft)
				{
					if (this.onMiddle && Random.Range(0f, 1f) > 0.5f)
					{
						this.SlamMiddle();
						return;
					}
					this.SlamLeft();
					return;
				}
				else
				{
					this.SlamMiddle();
				}
			}
		}
	}

	// Token: 0x060010D3 RID: 4307 RVA: 0x00081A0B File Offset: 0x0007FC0B
	private void SlamRight()
	{
		this.inAction = true;
		this.anim.SetTrigger("SlamRight");
		Object.Instantiate<GameObject>(this.windupSound, this.head);
		this.attackingRight = true;
	}

	// Token: 0x060010D4 RID: 4308 RVA: 0x00081A3D File Offset: 0x0007FC3D
	private void SlamLeft()
	{
		this.inAction = true;
		this.anim.SetTrigger("SlamLeft");
		Object.Instantiate<GameObject>(this.windupSound, this.head);
		this.attackingLeft = true;
	}

	// Token: 0x060010D5 RID: 4309 RVA: 0x00081A70 File Offset: 0x0007FC70
	private void SlamMiddle()
	{
		this.inAction = true;
		Object.Instantiate<GameObject>(this.windupSound, this.head);
		if (Random.Range(0f, 1f) > this.lowMiddleChance)
		{
			if (this.lowMiddleChance < 0.5f)
			{
				this.lowMiddleChance = 0.5f;
			}
			this.lowMiddleChance += 0.25f;
			this.anim.SetTrigger("SlamMiddle");
			this.attackingLeft = true;
			return;
		}
		if (this.lowMiddleChance > 0.5f)
		{
			this.lowMiddleChance = 0.5f;
		}
		this.lowMiddleChance -= 0.25f;
		this.anim.SetTrigger("SlamMiddleLow");
		this.attackingRight = true;
	}

	// Token: 0x060010D6 RID: 4310 RVA: 0x00081B30 File Offset: 0x0007FD30
	public void SwingStart()
	{
		if (this.attackingRight)
		{
			foreach (SwingCheck2 swingCheck in this.scRight)
			{
				swingCheck.damage = 45;
				swingCheck.DamageStart();
			}
			this.stat.ParryableCheck(true);
			foreach (Transform transform in this.rightHandBones)
			{
				this.stat.parryables.Add(transform);
			}
		}
		if (this.attackingLeft)
		{
			foreach (SwingCheck2 swingCheck2 in this.scLeft)
			{
				swingCheck2.damage = 45;
				swingCheck2.DamageStart();
			}
			this.stat.ParryableCheck(true);
			foreach (Transform transform2 in this.leftHandBones)
			{
				this.stat.parryables.Add(transform2);
			}
		}
	}

	// Token: 0x060010D7 RID: 4311 RVA: 0x00081C08 File Offset: 0x0007FE08
	public void SpecialDeath()
	{
		this.inAction = true;
		this.dead = true;
		this.anim.Play("Death");
		if (this.currentBlackHole != null)
		{
			this.currentBlackHole.Explode();
		}
		Parasite[] array = this.parasites;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(false);
		}
		Object.Instantiate<GameObject>(this.bigHurtSound, this.head).GetComponent<AudioSource>().pitch = 0.75f;
		MonoSingleton<CameraController>.Instance.CameraShake(0.5f);
	}

	// Token: 0x060010D8 RID: 4312 RVA: 0x00081C9E File Offset: 0x0007FE9E
	public void Impact()
	{
		Object.Instantiate<GameObject>(this.punchExplosion, this.head);
		MonoSingleton<CameraController>.Instance.CameraShake(3f);
		UnityEvent unityEvent = this.onDeathImpact;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x060010D9 RID: 4313 RVA: 0x00081CD1 File Offset: 0x0007FED1
	public void DeathOver()
	{
		UnityEvent unityEvent = this.onDeathOver;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x060010DA RID: 4314 RVA: 0x00081CE4 File Offset: 0x0007FEE4
	public void SwingEnd()
	{
		MonoSingleton<CameraController>.Instance.CameraShake(2f);
		List<Transform> list = new List<Transform>();
		if (this.attackingRight)
		{
			SwingCheck2[] array = this.scRight;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].DamageStop();
			}
			list.Add(this.rightHand.transform);
		}
		if (this.attackingLeft)
		{
			SwingCheck2[] array = this.scLeft;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].DamageStop();
			}
			list.Add(this.leftHand.transform);
		}
		this.stat.partiallyParryable = false;
		this.stat.parryables.Clear();
		foreach (Transform transform in list)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(transform.position, transform.up, out raycastHit, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Environment)))
			{
				Object.Instantiate<GameObject>(this.punchExplosion, raycastHit.point, Quaternion.identity);
			}
			else
			{
				Object.Instantiate<GameObject>(this.punchExplosion, transform.position, Quaternion.identity);
			}
		}
		list.Clear();
	}

	// Token: 0x060010DB RID: 4315 RVA: 0x00081E2C File Offset: 0x0008002C
	private void BlackHole()
	{
		this.blackHoleCooldown = 5f;
		this.inAction = true;
		this.anim.SetTrigger("SpawnBlackHole");
	}

	// Token: 0x060010DC RID: 4316 RVA: 0x00081E50 File Offset: 0x00080050
	public void SpawnBlackHole()
	{
		if (this.inPhaseChange)
		{
			return;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.blackHole, this.blackHoleSpawnPos);
		this.currentBlackHole = gameObject.GetComponent<BlackHoleProjectile>();
		this.currentBlackHole.target = this.eid.target;
		this.currentBlackHole.FadeIn();
		if (this.difficulty == 4)
		{
			this.currentBlackHole.speed *= 1.5f;
			return;
		}
		if (this.difficulty == 5)
		{
			this.currentBlackHole.speed *= 2f;
		}
	}

	// Token: 0x060010DD RID: 4317 RVA: 0x00081EE6 File Offset: 0x000800E6
	public void LaunchBlackHole()
	{
		this.currentBlackHole.Activate();
	}

	// Token: 0x060010DE RID: 4318 RVA: 0x00081EF4 File Offset: 0x000800F4
	public void GotParried()
	{
		if (!this.dead)
		{
			Object.Instantiate<GameObject>(this.bigHurtSound, this.head);
			this.punchesSinceBreak = 0;
			if (!this.beenParried)
			{
				this.beenParried = true;
				if (this.parryChallenge)
				{
					MonoSingleton<ChallengeManager>.Instance.ChallengeDone();
				}
			}
			MonoSingleton<StyleHUD>.Instance.AddPoints(500, "ultrakill.downtosize", null, this.eid, -1, "", "");
			if (this.attackingRight)
			{
				SwingCheck2[] array = this.scRight;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].DamageStop();
				}
				this.anim.SetTrigger("ParryRight");
				this.eid.hitter = "";
				foreach (Transform transform in this.rightHandBones)
				{
					this.stat.GetHurt(transform.gameObject, Vector3.zero, (float)(35 / this.rightHandBones.Length), 0f, transform.position, null, false);
					transform.gameObject.layer = 10;
				}
			}
			if (this.attackingLeft)
			{
				SwingCheck2[] array = this.scLeft;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].DamageStop();
				}
				this.anim.SetTrigger("ParryLeft");
				this.eid.hitter = "";
				foreach (Transform transform2 in this.leftHandBones)
				{
					this.stat.GetHurt(transform2.gameObject, Vector3.zero, (float)(35 / this.leftHandBones.Length), 0f, transform2.position, null, false);
					transform2.gameObject.layer = 10;
				}
			}
			this.stat.partiallyParryable = false;
			this.stat.parryables.Clear();
			this.eid.hitter = "";
		}
	}

	// Token: 0x060010DF RID: 4319 RVA: 0x000820D8 File Offset: 0x000802D8
	public void ResetColliders()
	{
		if (this.attackingRight)
		{
			Transform[] array = this.rightHandBones;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.layer = 11;
			}
		}
		if (this.attackingLeft)
		{
			Transform[] array = this.leftHandBones;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.layer = 11;
			}
		}
	}

	// Token: 0x060010E0 RID: 4320 RVA: 0x0008213D File Offset: 0x0008033D
	public void StopAction()
	{
		this.attackingLeft = false;
		this.attackingRight = false;
		this.inAction = false;
	}

	// Token: 0x060010E1 RID: 4321 RVA: 0x00082154 File Offset: 0x00080354
	public void PlayerInZone(int zone)
	{
		switch (zone)
		{
		case 0:
			this.onRight = true;
			return;
		case 1:
			this.onMiddle = true;
			return;
		case 2:
			this.onLeft = true;
			return;
		default:
			return;
		}
	}

	// Token: 0x060010E2 RID: 4322 RVA: 0x00082180 File Offset: 0x00080380
	public void PlayerExitZone(int zone)
	{
		switch (zone)
		{
		case 0:
			this.onRight = false;
			return;
		case 1:
			this.onMiddle = false;
			return;
		case 2:
			this.onLeft = false;
			return;
		default:
			return;
		}
	}

	// Token: 0x060010E3 RID: 4323 RVA: 0x000821AC File Offset: 0x000803AC
	private void PhaseChange(int targetPhase)
	{
		this.phase = targetPhase;
		this.inAction = true;
		this.cooldown = (float)((this.difficulty >= 4) ? 1 : 4);
		Object.Instantiate<GameObject>(this.bigHurtSound, this.head);
		if (this.phase == 2)
		{
			this.anim.SetTrigger("PhaseParasite");
			if (this.attackingRight)
			{
				SwingCheck2[] array = this.scRight;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].DamageStop();
				}
			}
			if (this.attackingLeft)
			{
				SwingCheck2[] array = this.scLeft;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].DamageStop();
				}
			}
			this.stat.partiallyParryable = false;
			this.stat.parryables.Clear();
			if (this.currentBlackHole && (this.difficulty <= 2 || this.currentBlackHole.fadingIn))
			{
				this.currentBlackHole.Explode();
				return;
			}
		}
		else
		{
			this.inPhaseChange = false;
		}
	}

	// Token: 0x060010E4 RID: 4324 RVA: 0x000822A8 File Offset: 0x000804A8
	public void ShutEye(int eye)
	{
		this.eyes[eye].SetActive(false);
		if (eye == 1)
		{
			SkinnedMeshRenderer componentInChildren = base.GetComponentInChildren<SkinnedMeshRenderer>();
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			componentInChildren.GetPropertyBlock(materialPropertyBlock);
			materialPropertyBlock.SetTexture("_MainTex", this.eyeless.GetTexture("_MainTex"));
			componentInChildren.SetPropertyBlock(materialPropertyBlock, 0);
			this.eid.UpdateBuffs(true, true);
		}
	}

	// Token: 0x060010E5 RID: 4325 RVA: 0x0008230C File Offset: 0x0008050C
	public void SpawnParasites()
	{
		MonoSingleton<CameraController>.Instance.CameraShake(2f);
		GoreZone componentInParent = base.GetComponentInParent<GoreZone>();
		foreach (GameObject gameObject in this.eyes)
		{
			for (int j = 0; j < 3; j++)
			{
				GameObject gore = MonoSingleton<BloodsplatterManager>.Instance.GetGore(GoreType.Head, this.eid, false);
				if (gore)
				{
					gore.transform.position = gameObject.transform.position;
					gore.transform.localScale = gore.transform.localScale * 3f;
					if (componentInParent)
					{
						gore.transform.SetParent(componentInParent.goreZone, true);
					}
					else
					{
						gore.transform.parent = null;
					}
				}
			}
		}
		Parasite[] array2 = this.parasites;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].gameObject.SetActive(true);
		}
		this.inPhaseChange = false;
		this.eid.weakPoint = this.parasites[0].GetComponentInChildren<EnemyIdentifierIdentifier>().gameObject;
	}

	// Token: 0x060010E6 RID: 4326 RVA: 0x00082430 File Offset: 0x00080630
	public void TargetBeenHit(SwingCheck2 swing)
	{
		if (this.attackingRight)
		{
			SwingCheck2[] array = this.scRight;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].DamageStop();
			}
			Transform[] array2 = this.rightHandBones;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].gameObject.layer = 10;
			}
		}
		if (this.attackingLeft)
		{
			SwingCheck2[] array = this.scLeft;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].DamageStop();
			}
			Transform[] array2 = this.leftHandBones;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].gameObject.layer = 10;
			}
		}
	}

	// Token: 0x040016E8 RID: 5864
	private Animator anim;

	// Token: 0x040016E9 RID: 5865
	private EnemyIdentifier eid;

	// Token: 0x040016EA RID: 5866
	private Statue stat;

	// Token: 0x040016EB RID: 5867
	public Transform head;

	// Token: 0x040016EC RID: 5868
	private bool inAction;

	// Token: 0x040016ED RID: 5869
	private bool inPhaseChange;

	// Token: 0x040016EE RID: 5870
	private float cooldown = 3f;

	// Token: 0x040016EF RID: 5871
	public int phase = 1;

	// Token: 0x040016F0 RID: 5872
	public Transform rightArm;

	// Token: 0x040016F1 RID: 5873
	public Transform rightHand;

	// Token: 0x040016F2 RID: 5874
	private Transform[] rightHandBones;

	// Token: 0x040016F3 RID: 5875
	private SwingCheck2[] scRight;

	// Token: 0x040016F4 RID: 5876
	private bool attackingRight;

	// Token: 0x040016F5 RID: 5877
	public Transform leftArm;

	// Token: 0x040016F6 RID: 5878
	public Transform leftHand;

	// Token: 0x040016F7 RID: 5879
	private Transform[] leftHandBones;

	// Token: 0x040016F8 RID: 5880
	private SwingCheck2[] scLeft;

	// Token: 0x040016F9 RID: 5881
	private bool attackingLeft;

	// Token: 0x040016FA RID: 5882
	public GameObject windupSound;

	// Token: 0x040016FB RID: 5883
	public GameObject bigHurtSound;

	// Token: 0x040016FC RID: 5884
	public GameObject punchExplosion;

	// Token: 0x040016FD RID: 5885
	public bool onRight;

	// Token: 0x040016FE RID: 5886
	public bool onMiddle;

	// Token: 0x040016FF RID: 5887
	public bool onLeft;

	// Token: 0x04001700 RID: 5888
	private float blackHoleCooldown = 10f;

	// Token: 0x04001701 RID: 5889
	public GameObject blackHole;

	// Token: 0x04001702 RID: 5890
	private BlackHoleProjectile currentBlackHole;

	// Token: 0x04001703 RID: 5891
	public Transform blackHoleSpawnPos;

	// Token: 0x04001704 RID: 5892
	private float lowMiddleChance = 0.5f;

	// Token: 0x04001705 RID: 5893
	public GameObject[] eyes;

	// Token: 0x04001706 RID: 5894
	public Material eyeless;

	// Token: 0x04001707 RID: 5895
	public Parasite[] parasites;

	// Token: 0x04001708 RID: 5896
	private float originalHealth;

	// Token: 0x04001709 RID: 5897
	public UnityEvent onDeathImpact;

	// Token: 0x0400170A RID: 5898
	public UnityEvent onDeathOver;

	// Token: 0x0400170B RID: 5899
	private bool dead;

	// Token: 0x0400170C RID: 5900
	private int difficulty = -1;

	// Token: 0x0400170D RID: 5901
	private bool beenParried;

	// Token: 0x0400170E RID: 5902
	public bool parryChallenge;

	// Token: 0x0400170F RID: 5903
	private int punchesSinceBreak;
}
