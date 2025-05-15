using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020002EB RID: 747
public class MassAnimationReceiver : MonoBehaviour
{
	// Token: 0x06001064 RID: 4196 RVA: 0x0007DF4C File Offset: 0x0007C14C
	public void Start()
	{
		this.anim = base.GetComponent<Animator>();
		if (this.fakeMass)
		{
			this.sic = MonoSingleton<StatueIntroChecker>.Instance;
			if (!this.otherBossIntro)
			{
				this.anim.speed = 0f;
			}
		}
		else
		{
			this.mass = base.GetComponentInParent<Mass>();
		}
		if (this.sic && this.sic.beenSeen && this.skipEntirelyOnReplay)
		{
			UnityEvent unityEvent = this.onSkip;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			this.SpawnMass();
		}
	}

	// Token: 0x06001065 RID: 4197 RVA: 0x0007DFD8 File Offset: 0x0007C1D8
	private void Update()
	{
		if (this.turnTowards)
		{
			Quaternion quaternion = Quaternion.LookRotation(new Vector3(this.player.position.x, base.transform.position.y, this.player.position.z) - base.transform.position, Vector3.up);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, quaternion, Time.deltaTime * (Quaternion.Angle(base.transform.rotation, quaternion) / 2f + 1f));
		}
	}

	// Token: 0x06001066 RID: 4198 RVA: 0x0007E080 File Offset: 0x0007C280
	public void GroundBreak()
	{
		Object.Instantiate<GameObject>(this.groundBreakEffect, base.transform.position, Quaternion.identity);
		this.breaks++;
		if (this.breaks == 3)
		{
			this.player = MonoSingleton<CameraController>.Instance.transform;
			this.turnTowards = true;
		}
	}

	// Token: 0x06001067 RID: 4199 RVA: 0x0007E0D7 File Offset: 0x0007C2D7
	public void SmallGroundBreak()
	{
		Object.Instantiate<GameObject>(this.smallGroundBreakEffect, base.transform.position, Quaternion.identity);
	}

	// Token: 0x06001068 RID: 4200 RVA: 0x0007E0F5 File Offset: 0x0007C2F5
	public void SpawnMass()
	{
		if (this.sic && !this.sic.beenSeen)
		{
			this.sic.beenSeen = true;
		}
		this.realMass.SetActive(true);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06001069 RID: 4201 RVA: 0x0007E135 File Offset: 0x0007C335
	public void Footstep()
	{
		if (this.anim.GetLayerWeight(1) > 0.5f)
		{
			Object.Instantiate<GameObject>(this.footstep, base.transform.position, Quaternion.identity);
		}
	}

	// Token: 0x0600106A RID: 4202 RVA: 0x0007E166 File Offset: 0x0007C366
	public void SkipOnReplay()
	{
		if (this.sic && this.sic.beenSeen)
		{
			this.SpawnMass();
		}
	}

	// Token: 0x0600106B RID: 4203 RVA: 0x0007E188 File Offset: 0x0007C388
	public void AnimationEvent(int i)
	{
		if (i == 1)
		{
			UnityEvent unityEvent = this.animationEvent1;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}
	}

	// Token: 0x0600106C RID: 4204 RVA: 0x0007E19E File Offset: 0x0007C39E
	public void ShootSpear()
	{
		this.mass.ShootSpear();
	}

	// Token: 0x0600106D RID: 4205 RVA: 0x0007E1AB File Offset: 0x0007C3AB
	public void StopAction()
	{
		this.mass.StopAction();
	}

	// Token: 0x0600106E RID: 4206 RVA: 0x0007E1B8 File Offset: 0x0007C3B8
	public void ShootHomingR()
	{
		this.mass.ShootHoming(0);
	}

	// Token: 0x0600106F RID: 4207 RVA: 0x0007E1C6 File Offset: 0x0007C3C6
	public void ShootHomingL()
	{
		this.mass.ShootHoming(1);
	}

	// Token: 0x06001070 RID: 4208 RVA: 0x0007E1D4 File Offset: 0x0007C3D4
	public void ShootExplosiveR()
	{
		this.mass.ShootExplosive(0);
	}

	// Token: 0x06001071 RID: 4209 RVA: 0x0007E1E2 File Offset: 0x0007C3E2
	public void ShootExplosiveL()
	{
		this.mass.ShootExplosive(1);
	}

	// Token: 0x06001072 RID: 4210 RVA: 0x0007E1F0 File Offset: 0x0007C3F0
	public void Slam()
	{
		this.mass.SlamImpact();
	}

	// Token: 0x06001073 RID: 4211 RVA: 0x0007E1FD File Offset: 0x0007C3FD
	public void SwingStart()
	{
		this.mass.SwingStart();
	}

	// Token: 0x06001074 RID: 4212 RVA: 0x0007E20A File Offset: 0x0007C40A
	public void SwingEnd()
	{
		this.mass.SwingEnd();
	}

	// Token: 0x06001075 RID: 4213 RVA: 0x0007E217 File Offset: 0x0007C417
	public void CrazyReady()
	{
		this.mass.CrazyReady();
	}

	// Token: 0x06001076 RID: 4214 RVA: 0x0007E224 File Offset: 0x0007C424
	public void Enrage()
	{
		this.mass.Enrage();
	}

	// Token: 0x04001650 RID: 5712
	public GameObject groundBreakEffect;

	// Token: 0x04001651 RID: 5713
	public GameObject smallGroundBreakEffect;

	// Token: 0x04001652 RID: 5714
	public bool fakeMass;

	// Token: 0x04001653 RID: 5715
	public bool otherBossIntro;

	// Token: 0x04001654 RID: 5716
	public GameObject realMass;

	// Token: 0x04001655 RID: 5717
	private Mass mass;

	// Token: 0x04001656 RID: 5718
	public GameObject footstep;

	// Token: 0x04001657 RID: 5719
	private Animator anim;

	// Token: 0x04001658 RID: 5720
	private StatueIntroChecker sic;

	// Token: 0x04001659 RID: 5721
	private bool turnTowards;

	// Token: 0x0400165A RID: 5722
	private Transform player;

	// Token: 0x0400165B RID: 5723
	private int breaks;

	// Token: 0x0400165C RID: 5724
	public bool skipEntirelyOnReplay;

	// Token: 0x0400165D RID: 5725
	public UnityEvent animationEvent1;

	// Token: 0x0400165E RID: 5726
	public UnityEvent onSkip;
}
